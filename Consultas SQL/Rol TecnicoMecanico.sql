INSERT INTO Rol (Nombre, Descripcion)
SELECT 'TecnicoMecanico', 'Complementa reportes (especialidad mecanica)'
WHERE NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre='TecnicoMecanico');