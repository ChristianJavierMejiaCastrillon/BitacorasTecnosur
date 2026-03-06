CREATE TABLE dbo.TipoNovedad
(
    IdTipoNovedad INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre        VARCHAR(50) NOT NULL,
    Activo        BIT NOT NULL CONSTRAINT DF_TipoNovedad_Activo DEFAULT (1),
    FechaCreacion DATETIME NOT NULL CONSTRAINT DF_TipoNovedad_FechaCreacion DEFAULT (GETDATE())
);

-- Evita duplicados
CREATE UNIQUE INDEX UX_TipoNovedad_Nombre ON dbo.TipoNovedad(Nombre);