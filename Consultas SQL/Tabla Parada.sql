-- TABLA: Parada
CREATE TABLE Parada (
    IdParada INT IDENTITY(1,1) PRIMARY KEY,
    IdBitacora INT NOT NULL,
    Inicio DATETIME2 NOT NULL,
    Fin DATETIME2 NOT NULL,
    Minutos INT NOT NULL,
    Detalle NVARCHAR(500) NULL,
    CONSTRAINT FK_Parada_Bitacora FOREIGN KEY (IdBitacora)
        REFERENCES Bitacora (IdBitacora)
);
GO