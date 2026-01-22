using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class TipoAsignacionDAL
    {
        // ============================
        // LISTAR TODOS (para dropdown)
        // ============================
        public List<TipoAsignacionItem> Listar(bool soloActivos = true)
        {
            var lista = new List<TipoAsignacionItem>();

            string sql = @"
                SELECT 
                    ta.IdTipoAsignacion,
                    ta.Codigo,
                    ta.Nombre,
                    ta.Activo
                FROM dbo.TipoAsignacion ta
                WHERE 1 = 1
            ";

            if (soloActivos)
                sql += " AND ta.Activo = 1 ";

            sql += " ORDER BY ta.Nombre ASC;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new TipoAsignacionItem
                        {
                            IdTipoAsignacion = Convert.ToInt32(dr["IdTipoAsignacion"]),
                            Codigo = dr["Codigo"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        });
                    }
                }
            }

            return lista;
        }

        // ============================
        // OBTENER POR ID (opcional)
        // ============================
        public TipoAsignacionItem ObtenerPorId(int idTipoAsignacion)
        {
            const string sql = @"
                SELECT 
                    ta.IdTipoAsignacion,
                    ta.Codigo,
                    ta.Nombre,
                    ta.Activo
                FROM dbo.TipoAsignacion ta
                WHERE ta.IdTipoAsignacion = @IdTipoAsignacion;
            ";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdTipoAsignacion", SqlDbType.Int).Value = idTipoAsignacion;

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new TipoAsignacionItem
                        {
                            IdTipoAsignacion = Convert.ToInt32(dr["IdTipoAsignacion"]),
                            Codigo = dr["Codigo"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"])
                        };
                    }
                }
            }

            return null;
        }
    }
    // ============================
    // DTO para dropdown / listas
    // ============================
    public class TipoAsignacionItem
    {
        public int IdTipoAsignacion { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        // Útil si quieres mostrar "Código - Nombre"
        public string Texto => $"{Codigo} - {Nombre}";
    }
}
