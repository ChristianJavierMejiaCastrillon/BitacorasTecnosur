INSERT INTO Rol (Nombre, Descripcion)
SELECT 'Coordinador', 'Visualiza reportes de sus maquinas asignadas'
WHERE NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre='Coordinador');