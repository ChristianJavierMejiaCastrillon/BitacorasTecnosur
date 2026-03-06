ALTER TABLE dbo.Novedad
ADD CONSTRAINT FK_Novedad_TipoNovedad
FOREIGN KEY (IdTipoNovedad) REFERENCES dbo.TipoNovedad(IdTipoNovedad);