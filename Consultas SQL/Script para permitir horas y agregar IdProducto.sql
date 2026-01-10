USE TQ_Bitacoras;
GO

-- 1) Bitacora: permitir horas null (para que el formulario actual pueda guardar)
ALTER TABLE Bitacora ALTER COLUMN HoraInicio TIME(0) NULL;
ALTER TABLE Bitacora ALTER COLUMN HoraFin TIME(0) NULL;
GO

-- 2) Novedad: agregar IdProducto para guardar lo seleccionado en el formulario
ALTER TABLE Novedad ADD IdProducto INT NULL;
GO

ALTER TABLE Novedad
ADD CONSTRAINT FK_Novedad_Producto
FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto);
GO
