CREATE PROCEDURE dbo.sp_TipoNovedadTecnica_CambiarEstado
    @IdTipoNovedadTecnica INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.TipoNovedadTecnica
    SET Activo = @Activo
    WHERE IdTipoNovedadTecnica = @IdTipoNovedadTecnica;
END