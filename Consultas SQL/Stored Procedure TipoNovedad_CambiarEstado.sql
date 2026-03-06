CREATE OR ALTER PROCEDURE dbo.sp_TipoNovedad_CambiarEstado
    @IdTipoNovedad INT,
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.TipoNovedad
    SET Activo = @Activo
    WHERE IdTipoNovedad = @IdTipoNovedad;
END