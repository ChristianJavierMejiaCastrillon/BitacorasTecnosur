CREATE TABLE dbo.MaquinaModuloElemento
(
    IdMaquinaModuloElemento INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdMaquinaModulo INT NOT NULL,
    Nombre VARCHAR(120) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_MaquinaModuloElemento_Activo DEFAULT(1),
    FechaCreacion DATETIME NOT NULL CONSTRAINT DF_MaquinaModuloElemento_FechaCreacion DEFAULT(GETDATE()),
    IdUsuarioCrea INT NULL
);