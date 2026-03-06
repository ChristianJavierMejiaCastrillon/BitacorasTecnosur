CREATE OR ALTER PROCEDURE dbo.sp_TipoNovedad_ListarActivosDropdown
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoNovedad,
        Nombre
    FROM dbo.TipoNovedad
    WHERE Activo = 1
    ORDER BY Nombre;
END