using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public static class ConexionBD
    {
        public static SqlConnection CrearConexion()
        {
            string cadenaConexion =
                ConfigurationManager.ConnectionStrings["TQ_Bitacoras"].ConnectionString;

            return new SqlConnection(cadenaConexion);
        }
    }
}