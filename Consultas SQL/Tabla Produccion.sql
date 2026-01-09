-- TABLA: Produccion
CREATE TABLE Produccion (
    IdProduccion INT IDENTITY(1,1) PRIMARY KEY,
    IdBitacora INT NOT NULL,
    IdProducto INT NOT NULL,
    ContadorInicial INT NOT NULL,
    ContadorFinal INT NOT NULL,
    Productos INT NOT NULL,
    ProductosBuenos INT NOT NULL,
    Descarte INT NOT NULL,
    Desperdicio INT NOT NULL,
    CONSTRAINT FK_Produccion_Bitacora FOREIGN KEY (IdBitacora)
        REFERENCES Bitacora (IdBitacora),
    CONSTRAINT FK_Produccion_Producto FOREIGN KEY (IdProducto)
        REFERENCES Producto (IdProducto)
);
GO