CREATE TABLE dbo.TipoNovedadTecnica
(
    IdTipoNovedadTecnica INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Activo BIT NOT NULL CONSTRAINT DF_TipoNovedadTecnica_Activo DEFAULT (1),
    FechaCreacion DATETIME NOT NULL CONSTRAINT DF_TipoNovedadTecnica_FechaCreacion DEFAULT (GETDATE())
);
GO

ALTER TABLE dbo.TipoNovedadTecnica
ADD CONSTRAINT UQ_TipoNovedadTecnica_Nombre UNIQUE (Nombre);
GO