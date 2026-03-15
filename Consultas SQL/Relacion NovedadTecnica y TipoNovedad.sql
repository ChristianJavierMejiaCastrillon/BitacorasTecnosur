ALTER TABLE dbo.NovedadTecnica
ADD CONSTRAINT FK_NovedadTecnica_TipoNovedad
FOREIGN KEY (IdTipoNovedadTecnica)
REFERENCES dbo.TipoNovedad (IdTipoNovedad);