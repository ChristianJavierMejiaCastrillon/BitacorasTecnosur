using System;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class NovedadTecnicaDAL
    {
        public void InsertarNovedadTecnica(
    int idBitacora,
    int idTipoNovedadTecnica,
    string tipoMantenimiento,
    string descripcion,
    string diagnostico,
    string solucionAplicada,
    int? tiempoPerdidoMinutos,
    int? idMaquinaModulo,
    int? idMaquinaModuloElemento,
    int idUsuarioReporta,
    int? idUsuarioResponsable,
    string reportadoPor)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_NovedadTecnica_Insertar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdBitacora", SqlDbType.Int).Value = idBitacora;
                comando.Parameters.Add("@IdTipoNovedadTecnica", SqlDbType.Int).Value = idTipoNovedadTecnica;
                comando.Parameters.Add("@TipoMantenimiento", SqlDbType.NVarChar, 20).Value =
                    string.IsNullOrWhiteSpace(tipoMantenimiento) ? (object)DBNull.Value : tipoMantenimiento;

                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 1000).Value = descripcion;
                comando.Parameters.Add("@Diagnostico", SqlDbType.NVarChar, 1000).Value =
                    string.IsNullOrWhiteSpace(diagnostico) ? (object)DBNull.Value : diagnostico;

                comando.Parameters.Add("@SolucionAplicada", SqlDbType.NVarChar, 1000).Value =
                    string.IsNullOrWhiteSpace(solucionAplicada) ? (object)DBNull.Value : solucionAplicada;

                comando.Parameters.Add("@TiempoPerdidoMinutos", SqlDbType.Int).Value =
                    tiempoPerdidoMinutos.HasValue ? (object)tiempoPerdidoMinutos.Value : DBNull.Value;

                comando.Parameters.Add("@IdMaquinaModulo", SqlDbType.Int).Value =
                    idMaquinaModulo.HasValue ? (object)idMaquinaModulo.Value : DBNull.Value;

                comando.Parameters.Add("@IdMaquinaModuloElemento", SqlDbType.Int).Value =
                    idMaquinaModuloElemento.HasValue ? (object)idMaquinaModuloElemento.Value : DBNull.Value;

                comando.Parameters.Add("@IdUsuarioReporta", SqlDbType.Int).Value = idUsuarioReporta;

                comando.Parameters.Add("@IdUsuarioResponsable", SqlDbType.Int).Value =
                    idUsuarioResponsable.HasValue ? (object)idUsuarioResponsable.Value : DBNull.Value;

                comando.Parameters.Add("@ReportadoPor", SqlDbType.NVarChar, 150).Value =
                    string.IsNullOrWhiteSpace(reportadoPor) ? (object)DBNull.Value : reportadoPor;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public void ActualizarNovedadTecnica(
    int idNovedadTecnica,
    int idUsuarioActual,
    int idTipoNovedadTecnica,
    string tipoMantenimiento,
    string descripcion,
    string diagnostico,
    string solucionAplicada,
    int? tiempoPerdidoMinutos,
    int? idMaquinaModulo,
    int? idMaquinaModuloElemento,
    int? idUsuarioResponsable)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_NovedadTecnica_Actualizar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdNovedadTecnica", SqlDbType.Int).Value = idNovedadTecnica;
                comando.Parameters.Add("@IdUsuarioActual", SqlDbType.Int).Value = idUsuarioActual;
                comando.Parameters.Add("@IdTipoNovedadTecnica", SqlDbType.Int).Value = idTipoNovedadTecnica;
                comando.Parameters.Add("@TipoMantenimiento", SqlDbType.NVarChar, 20).Value =
                    string.IsNullOrWhiteSpace(tipoMantenimiento) ? (object)DBNull.Value : tipoMantenimiento;

                comando.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 1000).Value = descripcion;
                comando.Parameters.Add("@Diagnostico", SqlDbType.NVarChar, 1000).Value =
                    string.IsNullOrWhiteSpace(diagnostico) ? (object)DBNull.Value : diagnostico.Trim();
                comando.Parameters.Add("@SolucionAplicada", SqlDbType.NVarChar, 1000).Value =
                    string.IsNullOrWhiteSpace(solucionAplicada) ? (object)DBNull.Value : solucionAplicada.Trim();
                comando.Parameters.Add("@TiempoPerdidoMinutos", SqlDbType.Int).Value =
                    tiempoPerdidoMinutos.HasValue ? (object)tiempoPerdidoMinutos.Value : DBNull.Value;
                comando.Parameters.Add("@IdMaquinaModulo", SqlDbType.Int).Value =
                    idMaquinaModulo.HasValue ? (object)idMaquinaModulo.Value : DBNull.Value;
                comando.Parameters.Add("@IdMaquinaModuloElemento", SqlDbType.Int).Value =
                    idMaquinaModuloElemento.HasValue ? (object)idMaquinaModuloElemento.Value : DBNull.Value;
                comando.Parameters.Add("@IdUsuarioResponsable", SqlDbType.Int).Value =
                    idUsuarioResponsable.HasValue ? (object)idUsuarioResponsable.Value : DBNull.Value;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public void EliminarNovedadTecnica(int idNovedadTecnica, int idUsuarioActual)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_NovedadTecnica_Eliminar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdNovedadTecnica", SqlDbType.Int).Value = idNovedadTecnica;
                comando.Parameters.Add("@IdUsuarioActual", SqlDbType.Int).Value = idUsuarioActual;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public NovedadTecnicaEdicionDTO ObtenerNovedadTecnicaParaEdicion(int idNovedadTecnica)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_NovedadTecnica_ObtenerPorId", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@IdNovedadTecnica", SqlDbType.Int).Value = idNovedadTecnica;

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                        return null;

                    return new NovedadTecnicaEdicionDTO
                    {
                        IdNovedadTecnica = (int)lector["IdNovedadTecnica"],
                        IdBitacora = (int)lector["IdBitacora"],
                        IdTipoNovedadTecnica = (int)lector["IdTipoNovedadTecnica"],
                        TipoMantenimiento = lector["TipoMantenimiento"] == DBNull.Value ? null : lector["TipoMantenimiento"].ToString(),
                        Descripcion = lector["Descripcion"].ToString(),
                        Diagnostico = lector["Diagnostico"] == DBNull.Value ? null : lector["Diagnostico"].ToString(),
                        SolucionAplicada = lector["SolucionAplicada"] == DBNull.Value ? null : lector["SolucionAplicada"].ToString(),
                        ReportadoPor = lector["ReportadoPor"] == DBNull.Value ? null : lector["ReportadoPor"].ToString(),
                        Validado = (bool)lector["Validado"],
                        TiempoPerdidoMinutos = lector["TiempoPerdidoMinutos"] == DBNull.Value ? (int?)null : (int)lector["TiempoPerdidoMinutos"],
                        IdMaquinaModulo = lector["IdMaquinaModulo"] == DBNull.Value ? (int?)null : (int)lector["IdMaquinaModulo"],
                        IdMaquinaModuloElemento = lector["IdMaquinaModuloElemento"] == DBNull.Value ? (int?)null : (int)lector["IdMaquinaModuloElemento"],
                        IdUsuarioReporta = (int)lector["IdUsuarioReporta"],
                        IdUsuarioResponsable = lector["IdUsuarioResponsable"] == DBNull.Value ? (int?)null : (int)lector["IdUsuarioResponsable"],
                        FechaCreacion = (DateTime)lector["FechaCreacion"],

                        Fecha = (DateTime)lector["Fecha"],
                        Turno = lector["Turno"].ToString(),
                        IdMaquina = (int)lector["IdMaquina"],
                        IdUsuarioBitacora = (int)lector["IdUsuarioBitacora"],

                        NombreUsuarioReporta = lector["NombreUsuarioReporta"].ToString(),
                        TipoNovedadTecnicaNombre = lector["TipoNovedadTecnicaNombre"].ToString()
                    };
                }
            }
        }

        public class NovedadTecnicaEdicionDTO
        {
            public int IdNovedadTecnica { get; set; }
            public int IdBitacora { get; set; }
            public int IdTipoNovedadTecnica { get; set; }
            public string TipoMantenimiento { get; set; }
            public string Descripcion { get; set; }
            public string Diagnostico { get; set; }
            public string SolucionAplicada { get; set; }
            public string ReportadoPor { get; set; }
            public bool Validado { get; set; }
            public int? TiempoPerdidoMinutos { get; set; }
            public int? IdMaquinaModulo { get; set; }
            public int? IdMaquinaModuloElemento { get; set; }
            public int IdUsuarioReporta { get; set; }
            public int? IdUsuarioResponsable { get; set; }
            public DateTime FechaCreacion { get; set; }

            public DateTime Fecha { get; set; }
            public string Turno { get; set; }
            public int IdMaquina { get; set; }
            public int IdUsuarioBitacora { get; set; }

            public string NombreUsuarioReporta { get; set; }
            public string TipoNovedadTecnicaNombre { get; set; }
        }
    }
}