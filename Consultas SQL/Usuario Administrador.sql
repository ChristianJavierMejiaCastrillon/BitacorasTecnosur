INSERT INTO Usuario (Nombres, Apellidos, IdRol, Activo, UsuarioLogin, CodigoTrabajador)
VALUES ('Admin', 'Sistema', 1, 1, 'admin', 'Lobo080913');

SELECT * FROM Usuario WHERE UsuarioLogin = 'admin';
