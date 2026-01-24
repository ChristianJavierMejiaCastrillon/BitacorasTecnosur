CREATE TABLE dbo.MaquinaModulo
(
    IdMaquinaModulo INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdMaquina INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_MaquinaModulo_Activo DEFAULT(1),
    FechaCreacion DATETIME NOT NULL CONSTRAINT DF_MaquinaModulo_FechaCreacion DEFAULT(GETDATE()),
    IdUsuarioCrea INT NULL
);