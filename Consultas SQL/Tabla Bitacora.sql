-- TABLA: Bitacora
CREATE TABLE Bitacora (
    IdBitacora INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    HoraInicio TIME(0) NOT NULL,
    HoraFin TIME(0) NOT NULL,
    Turno NVARCHAR(20) NOT NULL,           
    IdMaquina INT NOT NULL,
    IdUsuario INT NOT NULL,                
    CONSTRAINT FK_Bitacora_Maquina FOREIGN KEY (IdMaquina)
        REFERENCES Maquina (IdMaquina),
    CONSTRAINT FK_Bitacora_Usuario FOREIGN KEY (IdUsuario)
        REFERENCES Usuario (IdUsuario)
);
GO