CREATE TABLE UsuarioMaquina (
    IdUsuarioMaquina INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdMaquina INT NOT NULL,
    TipoAsignacion NVARCHAR(30) NOT NULL, -- 'Coordinador','TecnicoElectrico','TecnicoMecanico'
    Activo BIT NOT NULL DEFAULT 1,
    FechaInicio DATETIME NOT NULL DEFAULT GETDATE(),
    FechaFin DATETIME NULL,

    CONSTRAINT FK_UsuarioMaquina_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_UsuarioMaquina_Maquina FOREIGN KEY (IdMaquina) REFERENCES Maquina(IdMaquina)
);
GO