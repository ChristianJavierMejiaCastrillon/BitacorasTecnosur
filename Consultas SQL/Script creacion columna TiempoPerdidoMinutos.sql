USE TQ_Bitacoras;
GO

IF COL_LENGTH('dbo.Novedad', 'TiempoPerdidoMinutos') IS NULL
BEGIN
    ALTER TABLE dbo.Novedad
    ADD TiempoPerdidoMinutos INT NULL;
END
GO