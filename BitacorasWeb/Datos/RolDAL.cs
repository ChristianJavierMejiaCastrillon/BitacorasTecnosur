using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class RolDAL
    {
        public List<RolItem> ListarRoles()
        {
            var lista = new List<RolItem>();

            const string sql = @"SELECT IdRol, Nombre FROM Rol ORDER BY IdRol;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new RolItem
                        {
                            IdRol = (int)dr["IdRol"],
                            Nombre = dr["Nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public string ObtenerNombreRol(int idRol)
        {
            const string sql = @"SELECT Nombre FROM Rol WHERE IdRol = @IdRol;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdRol", SqlDbType.Int).Value = idRol;

                con.Open();
                object result = cmd.ExecuteScalar();
                return result == null ? "" : result.ToString();
            }
        }
    }

    public class RolItem
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
    }
}
