using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class MaquinaProductoDAL
    {
        public List<ProductoItem> ListarProductosPorMaquina(int idMaquina)
        {
            var lista = new List<ProductoItem>();

            const string sql = @"
                SELECT p.IdProducto, p.Nombre
                FROM dbo.MaquinaProducto mp
                INNER JOIN dbo.Producto p ON p.IdProducto = mp.IdProducto
                INNER JOIN dbo.Maquina m ON m.IdMaquina = mp.IdMaquina
                WHERE mp.Activo = 1
                  AND m.Activo = 1
                  AND mp.IdMaquina = @IdMaquina
                ORDER BY p.Nombre;";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@IdMaquina", SqlDbType.Int).Value = idMaquina;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ProductoItem
                        {
                            IdProducto = (int)dr["IdProducto"],
                            Nombre = dr["Nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void ReemplazarAsignaciones(int idProducto, List<int> idsMaquina)
        {
            // 1) Convertimos la lista a un DataTable (para el TVP dbo.IntList)
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));

            foreach (int id in idsMaquina)
                dt.Rows.Add(id);

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_MaquinaProducto_ReemplazarAsignaciones", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@IdProducto", idProducto);

                // 2) Parámetro Structured (TVP)
                SqlParameter p = comando.Parameters.AddWithValue("@Maquinas", dt);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "dbo.IntList"; // <-- EXACTO como lo creaste en SQL

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public List<int> ListarIdsMaquinasPorProducto(int idProducto)
        {
            var lista = new List<int>();

            const string sql = @"
        SELECT IdMaquina
        FROM MaquinaProducto
        WHERE IdProducto = @IdProducto
          AND Activo = 1;";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add((int)dr["IdMaquina"]);
                }
            }

            return lista;
        }



        public class ProductoItem
        {
            public int IdProducto { get; set; }
            public string Nombre { get; set; }
        }
    }
}
