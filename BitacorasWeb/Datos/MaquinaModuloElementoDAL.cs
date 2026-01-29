using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class MaquinaModuloElementoDAL
    {
        public List<MaquinaModuloElementoItem> ListarPorModulo(int idMaquinaModulo, bool incluirInactivos = false)
        {
            var lista = new List<MaquinaModuloElementoItem>();

            string sql = @"
                SELECT IdMaquinaModuloElemento, IdMaquinaModulo, Nombre, Descripcion, Activo
                FROM dbo.MaquinaModuloElemento
                WHERE IdMaquinaModulo = @IdMaquinaModulo " + (incluirInactivos ? "" : " AND Activo = 1") + @"
                ORDER BY Nombre;";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdMaquinaModulo", idMaquinaModulo);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new MaquinaModuloElementoItem
                        {
                            IdMaquinaModuloElemento = (int)dr["IdMaquinaModuloElemento"],
                            IdMaquinaModulo = (int)dr["IdMaquinaModulo"],
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"] == DBNull.Value ? "" : dr["Descripcion"].ToString(),
                            Activo = (bool)dr["Activo"]
                        });
                    }
                }
            }

            return lista;
        }

        public int Agregar(int idMaquinaModulo, string nombre, string descripcion, int? idUsuarioCrea = null)
        {
            const string sql = @"
                INSERT INTO dbo.MaquinaModuloElemento (IdMaquinaModulo, Nombre, Descripcion, IdUsuarioCrea)
                OUTPUT INSERTED.IdMaquinaModuloElemento
                VALUES (@IdMaquinaModulo, @Nombre, @Descripcion, @IdUsuarioCrea);";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdMaquinaModulo", idMaquinaModulo);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion",
                    string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion);
                cmd.Parameters.AddWithValue("@IdUsuarioCrea",
                    idUsuarioCrea.HasValue ? (object)idUsuarioCrea.Value : DBNull.Value);

                cn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void Editar(int idMaquinaModuloElemento, string nombre, string descripcion)
        {
            const string sql = @"
                UPDATE dbo.MaquinaModuloElemento
                SET Nombre = @Nombre,
                    Descripcion = @Descripcion
                WHERE IdMaquinaModuloElemento = @Id;";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@Id", idMaquinaModuloElemento);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion",
                    string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Desactivar(int id)
        {
            const string sql = @"UPDATE dbo.MaquinaModuloElemento SET Activo = 0 WHERE IdMaquinaModuloElemento = @Id;";
            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Reactivar(int id)
        {
            const string sql = @"UPDATE dbo.MaquinaModuloElemento SET Activo = 1 WHERE IdMaquinaModuloElemento = @Id;";
            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class MaquinaModuloElementoItem
    {
        public int IdMaquinaModuloElemento { get; set; }
        public int IdMaquinaModulo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
