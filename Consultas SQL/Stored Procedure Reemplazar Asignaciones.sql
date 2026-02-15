CREATE OR ALTER PROCEDURE dbo.sp_MaquinaProducto_ReemplazarAsignaciones
(
    @IdProducto INT,
    @Maquinas dbo.IntList READONLY
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- 1) Desactivar todas las asignaciones actuales del producto
        UPDATE dbo.MaquinaProducto
           SET Activo = 0
         WHERE IdProducto = @IdProducto;

        -- 2) Activar o insertar las asignaciones seleccionadas
        MERGE dbo.MaquinaProducto AS target
        USING (SELECT Id AS IdMaquina FROM @Maquinas) AS src
          ON target.IdProducto = @IdProducto
         AND target.IdMaquina  = src.IdMaquina
        WHEN MATCHED THEN
            UPDATE SET Activo = 1
        WHEN NOT MATCHED THEN
            INSERT (IdMaquina, IdProducto, Activo)
            VALUES (src.IdMaquina, @IdProducto, 1);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO
