using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class BitacoraDAL
    {
        public int CrearBitacora(DateTime fecha, string turno, int idMaquina, int idUsuario)
        {
            const string sql = @"
                INSERT INTO Bitacora (Fecha, HoraInicio, HoraFin, Turno, IdMaquina, IdUsuario)
                VALUES (@Fecha, NULL, NULL, @Turno, @IdMaquina, @IdUsuario);

                SELECT SCOPE_IDENTITY();";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                comando.Parameters.Add("@Turno", SqlDbType.NVarChar, 20).Value = turno;
                comando.Parameters.Add("@IdMaquina", SqlDbType.Int).Value = idMaquina;
                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                conexion.Open();
                object result = comando.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }
    }
}