ALTER TABLE dbo.MaquinaModulo
ADD CONSTRAINT UQ_MaquinaModulo_Maquina_Nombre UNIQUE (IdMaquina, Nombre);