ALTER TABLE dbo.MaquinaModuloElemento
ADD CONSTRAINT UQ_MaquinaModuloElemento_Modulo_Nombre UNIQUE (IdMaquinaModulo, Nombre);