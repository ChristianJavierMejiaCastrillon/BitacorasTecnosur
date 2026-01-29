using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class MaquinaModuloDAL
    {
        public List<MaquinaModuloItem> ListarPorMaquina(int idMaquina, bool incluirInactivos = false)
        {
            var lista = new List<MaquinaModuloItem>();

            string sql = @"
                SELECT IdMaquinaModulo, IdMaquina, Nombre, Descripcion, Activo
                FROM dbo.MaquinaModulo
                WHERE IdMaquina = @IdMaquina " + (incluirInactivos ? "" : " AND Activo = 1") + @"
                ORDER BY Nombre;";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdMaquina", idMaquina);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new MaquinaModuloItem
                        {
                            IdMaquinaModulo = (int)dr["IdMaquinaModulo"],
                            IdMaquina = (int)dr["IdMaquina"],
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"] == DBNull.Value ? "" : dr["Descripcion"].ToString(),
                            Activo = (bool)dr["Activo"]
                        });
                    }
                }
            }

            return lista;
        }

        public int Agregar(int idMaquina, string nombre, string descripcion, int? idUsuarioCrea = null)
        {
            const string sql = @"
                INSERT INTO dbo.MaquinaModulo (IdMaquina, Nombre, Descripcion, IdUsuarioCrea)
                OUTPUT INSERTED.IdMaquinaModulo
                VALUES (@IdMaquina, @Nombre, @Descripcion, @IdUsuarioCrea);";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdMaquina", idMaquina);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion",
                    string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion);
                cmd.Parameters.AddWithValue("@IdUsuarioCrea",
                    idUsuarioCrea.HasValue ? (object)idUsuarioCrea.Value : DBNull.Value);

                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void Editar(int idMaquinaModulo, string nombre, string descripcion)
        {
            const string sql = @"
                UPDATE dbo.MaquinaModulo
                SET Nombre = @Nombre,
                    Descripcion = @Descripcion
                WHERE IdMaquinaModulo = @IdMaquinaModulo;";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdMaquinaModulo", idMaquinaModulo);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion",
                    string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Desactivar(int idMaquinaModulo)
        {
            const string sql = @"UPDATE dbo.MaquinaModulo SET Activo = 0 WHERE IdMaquinaModulo = @Id;";
            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@Id", idMaquinaModulo);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Reactivar(int idMaquinaModulo)
        {
            const string sql = @"UPDATE dbo.MaquinaModulo SET Activo = 1 WHERE IdMaquinaModulo = @Id;";
            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@Id", idMaquinaModulo);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class MaquinaModuloItem
    {
        public int IdMaquinaModulo { get; set; }
        public int IdMaquina { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
