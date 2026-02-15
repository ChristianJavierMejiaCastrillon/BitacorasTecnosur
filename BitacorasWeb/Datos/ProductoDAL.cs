using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class ProductoDAL
    {
        public List<ProductoItem> ListarProductosParaDropdown()
        {
            var lista = new List<ProductoItem>();

            const string sql = @"
                SELECT IdProducto, Nombre
                FROM Producto
                ORDER BY Nombre;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new ProductoItem
                        {
                            IdProducto = (int)lector["IdProducto"],
                            Nombre = lector["Nombre"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        public DataTable ListarProductos(bool incluirInactivos = false)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand())
            {
                comando.Connection = conexion;

                string sql = @"
            SELECT
                p.IdProducto,
                p.Codigo,
                p.Nombre,
                p.Descripcion,
                p.Activo
            FROM Producto p
        ";

                if (!incluirInactivos)
                    sql += " WHERE p.Activo = 1 ";

                sql += " ORDER BY p.Nombre;";

                comando.CommandText = sql;

                conexion.Open();

                using (SqlDataAdapter da = new SqlDataAdapter(comando))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public int GuardarProducto(int? idProducto, string codigo, string nombre, string descripcion)
        {
            int idGenerado = 0;

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_Producto_Guardar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                var pId = comando.Parameters.Add("@IdProducto", SqlDbType.Int);
                pId.Value = idProducto.HasValue ? (object)idProducto.Value : DBNull.Value;
                comando.Parameters.AddWithValue("@Codigo", codigo);
                comando.Parameters.AddWithValue("@Nombre", nombre);
                comando.Parameters.AddWithValue("@Descripcion", (object)descripcion ?? DBNull.Value);

                conexion.Open();

                idGenerado = Convert.ToInt32(comando.ExecuteScalar());
            }

            return idGenerado;
        }

        public void DesactivarProducto(int idProducto)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_Producto_Desactivar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@IdProducto", idProducto);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
        public void ReactivarProducto(int idProducto)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand())
            {
                comando.Connection = conexion;
                comando.CommandText = @"
            UPDATE Producto
            SET Activo = 1
            WHERE IdProducto = @IdProducto;";

                comando.Parameters.AddWithValue("@IdProducto", idProducto);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }


        public ProductoDTO ObtenerProductoPorId(int idProducto)
        {
            const string sql = @"
        SELECT IdProducto, Codigo, Nombre, Descripcion
        FROM Producto
        WHERE IdProducto = @IdProducto;";

            using (SqlConnection cn = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read()) return null;

                    return new ProductoDTO
                    {
                        IdProducto = (int)dr["IdProducto"],
                        Codigo = dr["Codigo"].ToString(),
                        Nombre = dr["Nombre"].ToString(),
                        Descripcion = dr["Descripcion"] == DBNull.Value ? "" : dr["Descripcion"].ToString()
                    };
                }
            }
        }

        public class ProductoDTO
        {
            public int IdProducto { get; set; }
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
        }
        public class ProductoItem
        {
            public int IdProducto { get; set; }
            public string Nombre { get; set; }
        }
    }
}