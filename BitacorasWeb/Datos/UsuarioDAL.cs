using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class UsuarioDAL
    {
        public List<UsuarioItem> ListarUsuariosParaDropdown()
        {
            var lista = new List<UsuarioItem>();

            const string sql = @"
                SELECT IdUsuario,
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
    }

    public class UsuarioItem
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
    }
}