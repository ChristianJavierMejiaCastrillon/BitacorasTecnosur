CREATE PROCEDURE dbo.sp_TipoNovedadTecnica_Guardar
    @IdTipoNovedadTecnica INT = NULL,
    @Nombre NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SET @Nombre = LTRIM(RTRIM(@Nombre));

    IF (@Nombre = '')
    BEGIN
        RAISERROR('El nombre es obligatorio.', 16, 1);
        RETURN;
    END

    IF EXISTS
    (
        SELECT 1
        FROM dbo.TipoNovedadTecnica
        WHERE Nombre = @Nombre
          AND (@IdTipoNovedadTecnica IS NULL OR IdTipoNovedadTecnica <> @IdTipoNovedadTecnica)
    )
    BEGIN
        RAISERROR('Ya existe un tipo de novedad técnica con ese nombre.', 16, 1);
        RETURN;
    END

    IF (@IdTipoNovedadTecnica IS NULL OR @IdTipoNovedadTecnica = 0)
    BEGIN
        INSERT INTO dbo.TipoNovedadTecnica (Nombre, Activo)
        VALUES (@Nombre, 1);
    END
    ELSE
    BEGIN
        UPDATE dbo.TipoNovedadTecnica
        SET Nombre = @Nombre
        WHERE IdTipoNovedadTecnica = @IdTipoNovedadTecnica;
    END
END