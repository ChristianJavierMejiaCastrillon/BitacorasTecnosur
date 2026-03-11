CREATE TABLE dbo.NovedadTecnica
(
    IdNovedadTecnica INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdBitacora INT NOT NULL,
    IdTipoNovedadTecnica INT NOT NULL,

    Descripcion NVARCHAR(1000) NOT NULL,
    Diagnostico NVARCHAR(1000) NULL,
    SolucionAplicada NVARCHAR(1000) NULL,

    ReportadoPor NVARCHAR(150) NULL,
    Validado BIT NOT NULL CONSTRAINT DF_NovedadTecnica_Validado DEFAULT (0),
    TiempoPerdidoMinutos INT NULL,

    IdMaquinaModulo INT NULL,
    IdMaquinaModuloElemento INT NULL,

    IdUsuarioReporta INT NOT NULL,
    IdUsuarioResponsable INT NULL,

    FechaCreacion DATETIME NOT NULL CONSTRAINT DF_NovedadTecnica_FechaCreacion DEFAULT (GETDATE())
);
GO