ALTER TABLE dbo.NovedadTecnica
ADD CONSTRAINT FK_NovedadTecnica_MaquinaModulo
FOREIGN KEY (IdMaquinaModulo)
REFERENCES dbo.MaquinaModulo(IdMaquinaModulo);