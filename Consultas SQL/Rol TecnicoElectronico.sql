INSERT INTO Rol (Nombre, Descripcion)
SELECT 'TecnicoElectronico', 'Complementa reportes (especialidad electrica)'
WHERE NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre='TecnicoElectronico');