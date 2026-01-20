using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace BitacorasWeb.Datos
{
    public class UsuarioDAL
    {
        // ==============================
        // DROPDOWN OPERARIOS 
        // ==============================
        public List<UsuarioItem> ListarUsuariosParaDropdown()
        {
            var lista = new List<UsuarioItem>();

            const string sql = @"
                SELECT 
                    IdUsuario,
                    (Nombres + ' ' + Apellidos) AS NombreCompleto
                FROM Usuario
                WHERE Activo = 1
                ORDER BY Nombres, Apellidos;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new UsuarioItem
                        {
                            IdUsuario = (int)lector["IdUsuario"],
                            NombreCompleto = lector["NombreCompleto"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        // ==============================
        // LOGIN (UsuarioLogin + Password)
        // ==============================
        public UsuarioAuth Login(string usuarioLogin, string password)
        {
            const string sql = @"
                SELECT 
                    u.IdUsuario,
                    (u.Nombres + ' ' + u.Apellidos) AS NombreCompleto,
                    r.Nombre AS Rol,
                    u.PasswordHash,
                    u.PasswordSalt
                FROM Usuario u
                INNER JOIN Rol r ON r.IdRol = u.IdRol
                WHERE u.Activo = 1 AND u.UsuarioLogin = @UsuarioLogin;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@UsuarioLogin", SqlDbType.NVarChar, 50).Value = usuarioLogin;

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return null;

                    // Si aún no se ha configurado password, no permite login
                    if (dr["PasswordHash"] == DBNull.Value || dr["PasswordSalt"] == DBNull.Value)
                        return null;

                    byte[] hashDb = (byte[])dr["PasswordHash"];
                    byte[] saltDb = (byte[])dr["PasswordSalt"];

                    if (!VerifyPassword(password, saltDb, hashDb))
                        return null;

                    return new UsuarioAuth
                    {
                        IdUsuario = (int)dr["IdUsuario"],
                        NombreCompleto = dr["NombreCompleto"].ToString(),
                        Rol = dr["Rol"].ToString()
                    };
                }
            }
        }

        // ==========================================
        // ASIGNAR PASSWORD 
        // ==========================================
        public void AsignarPassword(int idUsuario, string password)
        {
            byte[] salt = CreateSalt();
            byte[] hash = HashPassword(password, salt);

            const string sql = @"
                UPDATE Usuario
                SET PasswordSalt = @Salt,
                    PasswordHash = @Hash
                WHERE IdUsuario = @IdUsuario;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Salt", SqlDbType.VarBinary, 16).Value = salt;
                cmd.Parameters.Add("@Hash", SqlDbType.VarBinary, 32).Value = hash;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // Helpers Hash (PBKDF2)
        // =========================
        private static byte[] CreateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }


        private static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32); // 32 bytes = 256 bits
            }
        }

        private static bool VerifyPassword(string password, byte[] salt, byte[] expectedHash)
        {
            byte[] computed = HashPassword(password, salt);

            // Comparación manual (compatible .NET Framework)
            if (computed.Length != expectedHash.Length)
                return false;

            int diff = 0;
            for (int i = 0; i < computed.Length; i++)
                diff |= computed[i] ^ expectedHash[i];

            return diff == 0;
        }
        // ==============================
        // ADMIN - Gestión de usuarios
        // ==============================
        public List<UsuarioAdminItem> ListarUsuariosAdmin()
        {
            var lista = new List<UsuarioAdminItem>();

            const string sql = @"
        SELECT u.IdUsuario, u.UsuarioLogin, u.CodigoTrabajador, u.Nombres, u.Apellidos,
               u.IdRol, r.Nombre AS Rol, u.Activo,
               ISNULL(u.EsTurnero, 0) AS EsTurnero
        FROM Usuario u
        INNER JOIN Rol r ON r.IdRol = u.IdRol
        ORDER BY u.IdUsuario DESC;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        bool activo = (bool)dr["Activo"];
                        bool esTurnero = (bool)dr["EsTurnero"];

                        lista.Add(new UsuarioAdminItem
                        {
                            IdUsuario = (int)dr["IdUsuario"],
                            UsuarioLogin = dr["UsuarioLogin"].ToString(),
                            CodigoTrabajador = dr["CodigoTrabajador"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            IdRol = (int)dr["IdRol"],
                            Rol = dr["Rol"].ToString(),
                            Activo = activo,
                            NombreCompleto = dr["Nombres"].ToString() + " " + dr["Apellidos"].ToString(),
                            ActivoTexto = activo ? "Sí" : "No",
                            EsTurnero = esTurnero,
                            EsTurneroTexto = esTurnero ? "Sí" : "No"
                        });
                    }
                }
            }

            return lista;
        }

        public int CrearUsuario(string usuarioLogin, string codigoTrabajador, string nombres, string apellidos, int idRol, bool activo, bool esTurnero)
        {
            const string sql = @"
        INSERT INTO Usuario (Nombres, Apellidos, IdRol, Activo, UsuarioLogin, CodigoTrabajador, EsTurnero)
        OUTPUT INSERTED.IdUsuario
        VALUES (@Nombres, @Apellidos, @IdRol, @Activo, @UsuarioLogin, @CodigoTrabajador, @EsTurnero);";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Nombres", SqlDbType.NVarChar, 100).Value = nombres;
                cmd.Parameters.Add("@Apellidos", SqlDbType.NVarChar, 100).Value = apellidos;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
                cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;
                cmd.Parameters.Add("@UsuarioLogin", SqlDbType.NVarChar, 50).Value = usuarioLogin;
                cmd.Parameters.Add("@CodigoTrabajador", SqlDbType.NVarChar, 50).Value = codigoTrabajador;
                cmd.Parameters.Add("@EsTurnero", SqlDbType.Bit).Value = esTurnero;

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void ActualizarUsuario(int idUsuario, string usuarioLogin, string codigoTrabajador, string nombres, string apellidos, int idRol, bool activo, bool esTurnero)
        {
            const string sql = @"
        UPDATE Usuario
        SET Nombres = @Nombres,
            Apellidos = @Apellidos,
            IdRol = @IdRol,
            Activo = @Activo,
            UsuarioLogin = @UsuarioLogin,
            CodigoTrabajador = @CodigoTrabajador,
            EsTurnero = @EsTurnero
        WHERE IdUsuario = @IdUsuario;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Nombres", SqlDbType.NVarChar, 100).Value = nombres;
                cmd.Parameters.Add("@Apellidos", SqlDbType.NVarChar, 100).Value = apellidos;
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;
                cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;
                cmd.Parameters.Add("@UsuarioLogin", SqlDbType.NVarChar, 50).Value = usuarioLogin;
                cmd.Parameters.Add("@CodigoTrabajador", SqlDbType.NVarChar, 50).Value = codigoTrabajador;
                cmd.Parameters.Add("@EsTurnero", SqlDbType.Bit).Value = esTurnero;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public UsuarioEditItem ObtenerUsuarioPorId(int idUsuario)
        {
            const string sql = @"
        SELECT IdUsuario, UsuarioLogin, CodigoTrabajador, Nombres, Apellidos, IdRol, Activo, ISNULL(EsTurnero, 0) AS EsTurnero
        FROM Usuario
        WHERE IdUsuario = @IdUsuario;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read()) return null;

                    return new UsuarioEditItem
                    {
                        IdUsuario = (int)dr["IdUsuario"],
                        UsuarioLogin = dr["UsuarioLogin"].ToString(),
                        CodigoTrabajador = dr["CodigoTrabajador"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        Apellidos = dr["Apellidos"].ToString(),
                        IdRol = (int)dr["IdRol"],
                        Activo = (bool)dr["Activo"],
                        EsTurnero = (bool)dr["EsTurnero"]
                    };
                }
            }
        }

        public void ToggleActivo(int idUsuario)
        {
            const string sql = @"
        UPDATE Usuario
        SET Activo = CASE WHEN Activo = 1 THEN 0 ELSE 1 END
        WHERE IdUsuario = @IdUsuario;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // ==============================
    // DTO para dropdown 
    // ==============================
    public class UsuarioItem
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
    }

    // ==============================
    // DTO para autenticación
    // ==============================
    public class UsuarioAuth
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Rol { get; set; }
    }

    // ==============================
    // DTO para Agregar usarios - Admin
    // ==============================
    public class UsuarioAdminItem
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; }
        public string CodigoTrabajador { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; }
        public bool EsTurnero { get; set; }

        public string NombreCompleto { get; set; }
        public string ActivoTexto { get; set; }
        public string EsTurneroTexto { get; set; }
    }

    // ==============================
    // DTO para Editar usarios - Admin
    // ==============================
    public class UsuarioEditItem
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; }
        public string CodigoTrabajador { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int IdRol { get; set; }
        public bool Activo { get; set; }
        public bool EsTurnero { get; set; }
    }

}
