ALTER TABLE dbo.NovedadTecnica
ADD CONSTRAINT FK_NovedadTecnica_UsuarioReporta
FOREIGN KEY (IdUsuarioReporta)
REFERENCES dbo.Usuario(IdUsuario);