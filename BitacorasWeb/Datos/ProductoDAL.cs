using System;
using System.Collections.Generic;
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
        public class ProductoItem
        {
            public int IdProducto { get; set; }
            public string Nombre { get; set; }
        }
    }
}