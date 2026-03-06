CREATE OR ALTER PROCEDURE dbo.sp_TipoNovedad_ObtenerPorId
    @IdTipoNovedad INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoNovedad,
        Nombre,
        Activo,
        FechaCreacion
    FROM dbo.TipoNovedad
    WHERE IdTipoNovedad = @IdTipoNovedad;
END