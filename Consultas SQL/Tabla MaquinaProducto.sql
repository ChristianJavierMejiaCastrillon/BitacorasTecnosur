CREATE TABLE dbo.MaquinaProducto
(
    IdMaquinaProducto INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdMaquina INT NOT NULL,
    IdProducto INT NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_MaquinaProducto_Activo DEFAULT(1),
    FechaAlta DATETIME NOT NULL CONSTRAINT DF_MaquinaProducto_FechaAlta DEFAULT(GETDATE())
);

ALTER TABLE dbo.MaquinaProducto
ADD CONSTRAINT FK_MaquinaProducto_Maquina
FOREIGN KEY (IdMaquina) REFERENCES dbo.Maquina(IdMaquina);

ALTER TABLE dbo.MaquinaProducto
ADD CONSTRAINT FK_MaquinaProducto_Producto
FOREIGN KEY (IdProducto) REFERENCES dbo.Producto(IdProducto);

-- Evitar duplicados (misma referencia asignada dos veces a misma máquina)
ALTER TABLE dbo.MaquinaProducto
ADD CONSTRAINT UQ_MaquinaProducto UNIQUE (IdMaquina, IdProducto);
