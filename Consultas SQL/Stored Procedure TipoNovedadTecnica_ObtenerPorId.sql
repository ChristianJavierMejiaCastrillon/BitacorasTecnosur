CREATE PROCEDURE dbo.sp_TipoNovedadTecnica_ObtenerPorId
    @IdTipoNovedadTecnica INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdTipoNovedadTecnica,
        Nombre,
        Activo,
        FechaCreacion
    FROM dbo.TipoNovedadTecnica
    WHERE IdTipoNovedadTecnica = @IdTipoNovedadTecnica;
END