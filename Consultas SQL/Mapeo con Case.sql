UPDATE n
SET IdTipoNovedad = tn.IdTipoNovedad
FROM dbo.Novedad n
JOIN dbo.TipoNovedad tn
  ON tn.Nombre =
     CASE
        WHEN n.Tipo IN ('Mecanica', 'Mecánica') THEN 'Mecánica'
        WHEN n.Tipo IN ('Electrica', 'Eléctrica') THEN 'Eléctrica'
        WHEN n.Tipo IN ('Calidad') THEN 'Calidad'
        WHEN n.Tipo IN ('Material') THEN 'Material'
        WHEN n.Tipo IN ('Seguridad') THEN 'Seguridad'
        ELSE 'Otro'
     END;