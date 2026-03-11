CREATE PROCEDURE dbo.sp_TipoNovedadTecnica_ListarActivosDropdown
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoNovedadTecnica,
        Nombre
    FROM dbo.TipoNovedadTecnica
    WHERE Activo = 1
    ORDER BY Nombre;
END