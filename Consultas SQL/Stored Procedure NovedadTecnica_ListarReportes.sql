CREATE PROCEDURE dbo.sp_NovedadTecnica_ListarReportes
    @Fecha DATE = NULL,
    @Turno NVARCHAR(50) = NULL,
    @IdMaquina INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        nt.IdNovedadTecnica,
        nt.IdBitacora,
        b.Fecha,
        b.Turno,
        b.IdMaquina,
        m.Nombre AS NombreMaquina,

        nt.IdTipoNovedadTecnica,
        tnt.Nombre AS TipoNovedadTecnica,

        nt.Descripcion,
        nt.Diagnostico,
        nt.SolucionAplicada,
        nt.ReportadoPor,
        nt.Validado,
        nt.TiempoPerdidoMinutos,

        nt.IdMaquinaModulo,
        mm.Nombre AS NombreModulo,

        nt.IdMaquinaModuloElemento,
        mme.Nombre AS NombreElemento,

        nt.IdUsuarioReporta,
        (ur.Nombres + ' ' + ur.Apellidos) AS NombreUsuarioReporta,

        nt.IdUsuarioResponsable,
        CASE
            WHEN ures.IdUsuario IS NOT NULL THEN (ures.Nombres + ' ' + ures.Apellidos)
            ELSE NULL
        END AS NombreUsuarioResponsable,

        nt.FechaCreacion
    FROM dbo.NovedadTecnica nt
    INNER JOIN dbo.Bitacora b
        ON b.IdBitacora = nt.IdBitacora
    INNER JOIN dbo.Maquina m
        ON m.IdMaquina = b.IdMaquina
    INNER JOIN dbo.TipoNovedadTecnica tnt
        ON tnt.IdTipoNovedadTecnica = nt.IdTipoNovedadTecnica
    INNER JOIN dbo.Usuario ur
        ON ur.IdUsuario = nt.IdUsuarioReporta
    LEFT JOIN dbo.Usuario ures
        ON ures.IdUsuario = nt.IdUsuarioResponsable
    LEFT JOIN dbo.MaquinaModulo mm
        ON mm.IdMaquinaModulo = nt.IdMaquinaModulo
    LEFT JOIN dbo.MaquinaModuloElemento mme
        ON mme.IdMaquinaModuloElemento = nt.IdMaquinaModuloElemento
    WHERE
        (@Fecha IS NULL OR b.Fecha = @Fecha)
        AND (@Turno IS NULL OR @Turno = '' OR @Turno = '0' OR b.Turno = @Turno)
        AND (@IdMaquina IS NULL OR @IdMaquina = 0 OR b.IdMaquina = @IdMaquina)
    ORDER BY
        b.Fecha DESC,
        b.Turno ASC,
        nt.IdNovedadTecnica DESC;
END