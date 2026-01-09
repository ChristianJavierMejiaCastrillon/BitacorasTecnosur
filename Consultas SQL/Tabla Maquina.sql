-- TABLA: Maquina
CREATE TABLE Maquina (
    IdMaquina INT IDENTITY(1,1) PRIMARY KEY,
    Codigo NVARCHAR(50) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    CONSTRAINT UQ_Maquina_Codigo UNIQUE (Codigo)
);
GO