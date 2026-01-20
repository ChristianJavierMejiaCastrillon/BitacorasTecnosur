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
}
