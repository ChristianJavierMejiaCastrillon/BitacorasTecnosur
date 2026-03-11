ALTER TABLE dbo.NovedadTecnica
ADD CONSTRAINT FK_NovedadTecnica_Bitacora
FOREIGN KEY (IdBitacora) REFERENCES dbo.Bitacora(IdBitacora);