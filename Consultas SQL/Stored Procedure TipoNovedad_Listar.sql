CREATE OR ALTER PROCEDURE dbo.sp_TipoNovedad_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoNovedad,
        Nombre,
        Activo,
        FechaCreacion
    FROM dbo.TipoNovedad
    ORDER BY Nombre;
END