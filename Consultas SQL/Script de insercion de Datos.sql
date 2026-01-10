USE TQ_Bitacoras;
GO

-- ROLES
IF NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre = 'Administrador')
    INSERT INTO Rol (Nombre, Descripcion) VALUES ('Administrador', 'Administra usuarios y catálogos');

IF NOT EXISTS (SELECT 1 FROM Rol WHERE Nombre = 'Operario')
    INSERT INTO Rol (Nombre, Descripcion) VALUES ('Operario', 'Registra novedades y reporta turno');

GO

-- USUARIOS (operarios de prueba)
DECLARE @IdRolOperario INT = (SELECT TOP 1 IdRol FROM Rol WHERE Nombre = 'Operario');

INSERT INTO Usuario (Nombres, Apellidos, IdRol, Activo)
VALUES
('Juan', 'Pérez', @IdRolOperario, 1),
('María', 'Gómez', @IdRolOperario, 1),
('Carlos', 'Ramírez', @IdRolOperario, 1);
GO

-- MÁQUINAS (para pruebas futuras)
INSERT INTO Maquina (Codigo, Nombre, Descripcion)
VALUES
('LAM-01', 'Laminadora 1', 'Máquina de laminación'),
('SEL-05', 'Selladora ZX-5', 'Máquina de sellado'),
('EMP-200', 'Empacadora MK-200', 'Máquina de empaque');
GO

-- PRODUCTOS (para pruebas futuras)
INSERT INTO Producto (Codigo, Nombre, Descripcion)
VALUES
('GOLD-ET4', 'Gold Etapa 4', 'Referencia Gold etapa 4'),
('GOLD-ET5', 'Gold Etapa 5', 'Referencia Gold etapa 5');
GO
