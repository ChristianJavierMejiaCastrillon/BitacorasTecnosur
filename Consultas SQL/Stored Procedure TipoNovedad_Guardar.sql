CREATE OR ALTER PROCEDURE dbo.sp_TipoNovedad_Guardar
    @IdTipoNovedad INT = NULL,
    @Nombre VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Nombre = LTRIM(RTRIM(@Nombre));

    IF (@Nombre = '')
    BEGIN
        RAISERROR('El nombre es obligatorio.', 16, 1);
        RETURN;
    END

    -- Validar duplicado (ignora el mismo registro si es update)
    IF EXISTS (
        SELECT 1
        FROM dbo.TipoNovedad
        WHERE Nombre = @Nombre
          AND (@IdTipoNovedad IS NULL OR IdTipoNovedad <> @IdTipoNovedad)
    )
    BEGIN
        RAISERROR('Ya existe un tipo de novedad con ese nombre.', 16, 1);
        RETURN;
    END

    IF @IdTipoNovedad IS NULL OR @IdTipoNovedad = 0
    BEGIN
        INSERT INTO dbo.TipoNovedad (Nombre, Activo)
        VALUES (@Nombre, 1);
    END
    ELSE
    BEGIN
        UPDATE dbo.TipoNovedad
        SET Nombre = @Nombre
        WHERE IdTipoNovedad = @IdTipoNovedad;
    END
END