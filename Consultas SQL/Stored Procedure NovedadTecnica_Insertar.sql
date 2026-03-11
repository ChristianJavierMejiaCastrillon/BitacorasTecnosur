CREATE PROCEDURE dbo.sp_NovedadTecnica_Insertar
    @IdBitacora INT,
    @IdTipoNovedadTecnica INT,
    @Descripcion NVARCHAR(1000),
    @Diagnostico NVARCHAR(1000) = NULL,
    @SolucionAplicada NVARCHAR(1000) = NULL,
    @TiempoPerdidoMinutos INT = NULL,
    @IdMaquinaModulo INT = NULL,
    @IdMaquinaModuloElemento INT = NULL,
    @IdUsuarioReporta INT,
    @IdUsuarioResponsable INT = NULL,
    @ReportadoPor NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar tipo técnico activo
    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.TipoNovedadTecnica
        WHERE IdTipoNovedadTecnica = @IdTipoNovedadTecnica
          AND Activo = 1
    )
    BEGIN
        RAISERROR('El tipo de novedad técnica seleccionado está inactivo o no existe.', 16, 1);
        RETURN;
    END

    -- Validar bitácora
    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Bitacora
        WHERE IdBitacora = @IdBitacora
    )
    BEGIN
        RAISERROR('La bitácora indicada no existe.', 16, 1);
        RETURN;
    END

    -- Validar usuario reporta
    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Usuario
        WHERE IdUsuario = @IdUsuarioReporta
          AND Activo = 1
    )
    BEGIN
        RAISERROR('El usuario que reporta no existe o está inactivo.', 16, 1);
        RETURN;
    END

    -- Validar usuario responsable si viene informado
    IF @IdUsuarioResponsable IS NOT NULL
       AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.Usuario
        WHERE IdUsuario = @IdUsuarioResponsable
          AND Activo = 1
    )
    BEGIN
        RAISERROR('El usuario responsable no existe o está inactivo.', 16, 1);
        RETURN;
    END

    -- Validar módulo si viene informado
    IF @IdMaquinaModulo IS NOT NULL
       AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.MaquinaModulo
        WHERE IdMaquinaModulo = @IdMaquinaModulo
          AND Activo = 1
    )
    BEGIN
        RAISERROR('El módulo de máquina seleccionado no existe o está inactivo.', 16, 1);
        RETURN;
    END

    -- Validar elemento si viene informado
    IF @IdMaquinaModuloElemento IS NOT NULL
       AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.MaquinaModuloElemento
        WHERE IdMaquinaModuloElemento = @IdMaquinaModuloElemento
          AND Activo = 1
    )
    BEGIN
        RAISERROR('El elemento del módulo seleccionado no existe o está inactivo.', 16, 1);
        RETURN;
    END

    -- Validar coherencia módulo-elemento
    IF @IdMaquinaModulo IS NOT NULL
       AND @IdMaquinaModuloElemento IS NOT NULL
       AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.MaquinaModuloElemento mme
        WHERE mme.IdMaquinaModuloElemento = @IdMaquinaModuloElemento
          AND mme.IdMaquinaModulo = @IdMaquinaModulo
    )
    BEGIN
        RAISERROR('El elemento seleccionado no pertenece al módulo indicado.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.NovedadTecnica
    (
        IdBitacora,
        IdTipoNovedadTecnica,
        Descripcion,
        Diagnostico,
        SolucionAplicada,
        ReportadoPor,
        Validado,
        TiempoPerdidoMinutos,
        IdMaquinaModulo,
        IdMaquinaModuloElemento,
        IdUsuarioReporta,
        IdUsuarioResponsable
    )
    VALUES
    (
        @IdBitacora,
        @IdTipoNovedadTecnica,
        @Descripcion,
        @Diagnostico,
        @SolucionAplicada,
        @ReportadoPor,
        0,
        @TiempoPerdidoMinutos,
        @IdMaquinaModulo,
        @IdMaquinaModuloElemento,
        @IdUsuarioReporta,
        @IdUsuarioResponsable
    );
END