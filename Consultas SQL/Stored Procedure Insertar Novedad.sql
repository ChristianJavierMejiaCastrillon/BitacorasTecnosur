CREATE OR ALTER PROCEDURE dbo.sp_Novedad_Insertar
    @IdBitacora INT,
    @IdProducto INT,
    @IdTipoNovedad INT,
    @Descripcion NVARCHAR(500),
    @TiempoPerdidoMinutos INT,
    @ReportadoPor NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar tipo activo
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TipoNovedad
        WHERE IdTipoNovedad = @IdTipoNovedad
          AND Activo = 1
    )
    BEGIN
        RAISERROR('El tipo de novedad seleccionado está inactivo o no existe.', 16, 1);
        RETURN;
    END

    DECLARE @TipoTexto NVARCHAR(50);

    SELECT @TipoTexto = Nombre
    FROM dbo.TipoNovedad
    WHERE IdTipoNovedad = @IdTipoNovedad;

    INSERT INTO dbo.Novedad
        (IdBitacora, Tipo, Descripcion, ReportadoPor, Validado, IdProducto, TiempoPerdidoMinutos, IdTipoNovedad)
    VALUES
        (@IdBitacora, @TipoTexto, @Descripcion, @ReportadoPor, 0, @IdProducto, @TiempoPerdidoMinutos, @IdTipoNovedad);
END