ALTER TABLE dbo.NovedadTecnica
ADD CONSTRAINT FK_NovedadTecnica_UsuarioResponsable
FOREIGN KEY (IdUsuarioResponsable)
REFERENCES dbo.Usuario(IdUsuario);