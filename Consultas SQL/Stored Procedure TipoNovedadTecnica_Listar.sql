CREATE PROCEDURE dbo.sp_TipoNovedadTecnica_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoNovedadTecnica,
        Nombre,
        Activo,
        FechaCreacion
    FROM dbo.TipoNovedadTecnica
    ORDER BY Nombre;
END