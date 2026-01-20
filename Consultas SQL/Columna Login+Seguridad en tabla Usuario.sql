ALTER TABLE Usuario
ADD UsuarioLogin NVARCHAR(50) NULL,
    CodigoTrabajador NVARCHAR(20) NULL,
    PasswordHash VARBINARY(32) NULL,
    PasswordSalt VARBINARY(16) NULL;
GO

CREATE UNIQUE INDEX UX_Usuario_UsuarioLogin
ON Usuario(UsuarioLogin)
WHERE UsuarioLogin IS NOT NULL;
GO
