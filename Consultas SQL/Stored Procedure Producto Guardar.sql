CREATE OR ALTER PROCEDURE dbo.sp_Producto_Guardar
(
    @IdProducto INT = NULL,  -- NULL o 0 = insertar
    @Codigo NVARCHAR(50),
    @Nombre NVARCHAR(150),
    @Descripcion NVARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT
    IF (@IdProducto IS NULL OR @IdProducto = 0)
    BEGIN
        INSERT INTO dbo.Producto (Codigo, Nombre, Descripcion)
        VALUES (@Codigo, @Nombre, @Descripcion);

        SELECT SCOPE_IDENTITY() AS IdProducto;
        RETURN;
    END

    -- UPDATE
    UPDATE dbo.Producto
       SET Codigo = @Codigo,
           Nombre = @Nombre,
           Descripcion = @Descripcion
     WHERE IdProducto = @IdProducto;

    SELECT @IdProducto AS IdProducto;
END
GO
