ALTER TABLE dbo.NovedadTecnica
ADD CONSTRAINT CK_NovedadTecnica_TipoMantenimiento
CHECK (
    TipoMantenimiento IS NULL
    OR TipoMantenimiento IN ('Correctivo', 'Preventivo', 'Predictivo')
);