CREATE PROCEDURE dbo.sp_NovedadTecnica_Eliminar
    @IdNovedadTecnica INT,
    @IdUsuarioActual INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InicioDT DATETIME, @FinDT DATETIME;
    DECLARE @EsAdmin BIT = 0;
    DECLARE @RolActual NVARCHAR(100);

    -- Detectar rol del usuario actual
    SELECT @RolActual = r.Nombre
    FROM dbo.Usuario u
    INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
    WHERE u.IdUsuario = @IdUsuarioActual
      AND u.Activo = 1;

    IF @RolActual = 'Administrador'
        SET @EsAdmin = 1;

    -- Solo admin o técnicos pueden eliminar
    IF @RolActual NOT IN ('Administrador', 'TecnicoElectronico', 'TecnicoMecanico')
    BEGIN
        RAISERROR('No autorizado para eliminar novedades técnicas.', 16, 1);
        RETURN;
    END

    -- Obtener ventana del turno
    -- Admin: no filtra por dueño
    -- Técnico: solo si él creó la novedad
    SELECT
        @InicioDT =
            CASE
                WHEN b.HoraInicio IS NOT NULL
                    THEN DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', b.HoraInicio), CAST(b.Fecha AS datetime))
                ELSE
                    CASE b.Turno
                        WHEN 'Turno 1' THEN DATEADD(HOUR, 6, CAST(b.Fecha AS datetime))
                        WHEN 'Turno 2' THEN DATEADD(HOUR, 14, CAST(b.Fecha AS datetime))
                        WHEN 'Turno 3' THEN DATEADD(HOUR, 22, CAST(b.Fecha AS datetime))
                        WHEN 'Turno Administrativo' THEN DATEADD(HOUR, 7, CAST(b.Fecha AS datetime))
                        ELSE CAST(b.Fecha AS datetime)
                    END
            END,
        @FinDT =
            CASE
                WHEN b.HoraFin IS NOT NULL AND b.HoraInicio IS NOT NULL
                    THEN
                        CASE
                            WHEN b.HoraFin < b.HoraInicio
                                THEN DATEADD(DAY, 1, DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', b.HoraFin), CAST(b.Fecha AS datetime)))
                            ELSE DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', b.HoraFin), CAST(b.Fecha AS datetime))
                        END
                ELSE
                    CASE b.Turno
                        WHEN 'Turno 1' THEN DATEADD(HOUR, 14, CAST(b.Fecha AS datetime))
                        WHEN 'Turno 2' THEN DATEADD(HOUR, 22, CAST(b.Fecha AS datetime))
                        WHEN 'Turno 3' THEN DATEADD(HOUR, 6, DATEADD(DAY, 1, CAST(b.Fecha AS datetime)))
                        WHEN 'Turno Administrativo' THEN DATEADD(HOUR, 17, CAST(b.Fecha AS datetime))
                        ELSE CAST(b.Fecha AS datetime)
                    END
            END
    FROM dbo.NovedadTecnica nt
    INNER JOIN dbo.Bitacora b ON b.IdBitacora = nt.IdBitacora
    WHERE nt.IdNovedadTecnica = @IdNovedadTecnica
      AND (@EsAdmin = 1 OR nt.IdUsuarioReporta = @IdUsuarioActual);

    IF @InicioDT IS NULL OR @FinDT IS NULL
    BEGIN
        RAISERROR('No autorizado o registro no encontrado.', 16, 1);
        RETURN;
    END

    IF @EsAdmin = 0 AND NOT (GETDATE() BETWEEN @InicioDT AND @FinDT)
    BEGIN
        RAISERROR('No se puede eliminar: solo está permitido durante el turno en que fue reportado.', 16, 1);
        RETURN;
    END

    DELETE FROM dbo.NovedadTecnica
    WHERE IdNovedadTecnica = @IdNovedadTecnica;
END