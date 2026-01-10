using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class NovedadDAL
    {
        public void InsertarNovedad(int idBitacora, int idProducto, string tipo, string descripcion, string reportadoPor)
        {
            const string sql = @"
                INSERT INTO Novedad (IdBitacora, Tipo, Descripcion, ReportadoPor, Validado, IdProducto)
                VALUES (@IdBitacora, @Tipo, @Descripcion, @ReportadoPor, 0, @IdProducto);";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdBitacora", SqlDbType.Int).Value = idBitacora;
                comando.Parameters.Add("@Tipo", SqlDbType.NVarChar, 50).Value = tipo;
                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = descripcion;
                comando.Parameters.Add("@ReportadoPor", SqlDbType.NVarChar, 150).Value =
                    (object)reportadoPor ?? DBNull.Value;
                comando.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
}