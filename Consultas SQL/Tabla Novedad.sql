-- TABLA: Novedad
CREATE TABLE Novedad (
    IdNovedad INT IDENTITY(1,1) PRIMARY KEY,
    IdBitacora INT NOT NULL,
    Tipo NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(500) NOT NULL,
    ReportadoPor NVARCHAR(150) NULL,
    Validado BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Novedad_Bitacora FOREIGN KEY (IdBitacora)
        REFERENCES Bitacora (IdBitacora)
);
GO