using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class MaquinaDAL
    {
        public List<MaquinaItem> ListarMaquinasParaDropdown()
        {
            var lista = new List<MaquinaItem>();

            const string sql = @"
                SELECT IdMaquina, Nombre
                FROM Maquina
                ORDER BY Nombre;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new MaquinaItem
                        {
                            IdMaquina = (int)lector["IdMaquina"],
                            Nombre = lector["Nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public class MaquinaItem
        {
            public int IdMaquina { get; set; }
            public string Nombre { get; set; }
        }
    }
}