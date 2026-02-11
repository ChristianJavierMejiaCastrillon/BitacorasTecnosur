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
        public void InsertarNovedad(int idBitacora, int idProducto, string tipo, string descripcion, int tiempoPerdidoMinutos, string reportadoPor)
        {
            const string sql = @"
                INSERT INTO Novedad (IdBitacora, Tipo, Descripcion, ReportadoPor, Validado, IdProducto, TiempoPerdidoMinutos)
                VALUES (@IdBitacora, @Tipo, @Descripcion, @ReportadoPor, 0, @IdProducto, @TiempoPerdidoMinutos);";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdBitacora", SqlDbType.Int).Value = idBitacora;
                comando.Parameters.Add("@Tipo", SqlDbType.NVarChar, 50).Value = tipo;
                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 500).Value = descripcion;
                comando.Parameters.Add("@ReportadoPor", SqlDbType.NVarChar, 150).Value =
                    (object)reportadoPor ?? DBNull.Value;
                comando.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto;
                comando.Parameters.Add("@TiempoPerdidoMinutos", SqlDbType.Int).Value = tiempoPerdidoMinutos;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
        public void ActualizarNovedad(int idNovedad, int idUsuarioActual, string tipo, string descripcion, int? idProducto, int? tiempoPerdidoMinutos)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_Novedad_Actualizar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdNovedad", SqlDbType.Int).Value = idNovedad;
                comando.Parameters.Add("@IdUsuarioActual", SqlDbType.Int).Value = idUsuarioActual;

                comando.Parameters.Add("@Tipo", SqlDbType.NVarChar, 50).Value = tipo;
                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 1000).Value = descripcion;

                comando.Parameters.Add("@IdProducto", SqlDbType.Int).Value = idProducto.HasValue ? (object)idProducto.Value : DBNull.Value;
                comando.Parameters.Add("@TiempoPerdidoMinutos", SqlDbType.Int).Value = tiempoPerdidoMinutos.HasValue ? (object)tiempoPerdidoMinutos.Value : DBNull.Value;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }


        public void EliminarNovedad(int idNovedad, int idUsuarioActual)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_Novedad_Eliminar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdNovedad", SqlDbType.Int).Value = idNovedad;
                comando.Parameters.Add("@IdUsuarioActual", SqlDbType.Int).Value = idUsuarioActual;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public NovedadEdicionDTO ObtenerNovedadParaEdicion(int idNovedad)
        {
            const string sql = @"
        SELECT
            n.IdNovedad,
            n.IdProducto,
            n.Tipo,
            n.Descripcion,
            n.TiempoPerdidoMinutos,
            b.Fecha,
            b.Turno,
            b.IdMaquina,
            b.IdUsuario
        FROM dbo.Novedad n
        INNER JOIN dbo.Bitacora b ON b.IdBitacora = n.IdBitacora
        WHERE n.IdNovedad = @IdNovedad;
    ";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdNovedad", SqlDbType.Int).Value = idNovedad;

                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                        return null;

                    return new NovedadEdicionDTO
                    {
                        IdNovedad = (int)lector["IdNovedad"],
                        Fecha = (DateTime)lector["Fecha"],
                        Turno = lector["Turno"].ToString(),
                        IdMaquina = (int)lector["IdMaquina"],
                        IdUsuario = (int)lector["IdUsuario"],

                        IdProducto = lector["IdProducto"] == DBNull.Value ? (int?)null : (int)lector["IdProducto"],
                        Tipo = lector["Tipo"].ToString(),
                        Descripcion = lector["Descripcion"].ToString(),
                        TiempoPerdidoMinutos = lector["TiempoPerdidoMinutos"] == DBNull.Value ? (int?)null : (int)lector["TiempoPerdidoMinutos"]
                    };
                }
            }
        }

        public class NovedadEdicionDTO
        {
            public int IdNovedad { get; set; }
            public DateTime Fecha { get; set; }
            public string Turno { get; set; }
            public int IdMaquina { get; set; }
            public int IdUsuario { get; set; }

            public int? IdProducto { get; set; }
            public string Tipo { get; set; }
            public string Descripcion { get; set; }
            public int? TiempoPerdidoMinutos { get; set; }
        }

    }
}