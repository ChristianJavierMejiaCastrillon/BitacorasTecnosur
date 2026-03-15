using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class ReporteNovedadesTecnicasDAL
    {
        public List<NovedadTecnicaReporteItem> BuscarNovedades(
    DateTime? fechaDesde,
    DateTime? fechaHasta,
    string turno,
    int? idMaquina,
    int? idModulo,
    int? idElemento,
    int? idTipoNovedad,
    string tipoMantenimiento,
    int? idUsuarioResponsable,
    int? tiempoMin,
    int? tiempoMax)
        {
            var lista = new List<NovedadTecnicaReporteItem>();

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_NovedadTecnica_ListarReportes", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@FechaDesde", SqlDbType.Date).Value =
                    fechaDesde.HasValue ? (object)fechaDesde.Value : DBNull.Value;

                comando.Parameters.Add("@FechaHasta", SqlDbType.Date).Value =
                    fechaHasta.HasValue ? (object)fechaHasta.Value : DBNull.Value;

                comando.Parameters.Add("@Turno", SqlDbType.NVarChar, 50).Value =
                    string.IsNullOrWhiteSpace(turno) || turno == "0" ? (object)DBNull.Value : turno;

                comando.Parameters.Add("@IdMaquina", SqlDbType.Int).Value =
                    idMaquina.HasValue && idMaquina > 0 ? (object)idMaquina.Value : DBNull.Value;

                comando.Parameters.Add("@IdMaquinaModulo", SqlDbType.Int).Value =
                    idModulo.HasValue && idModulo > 0 ? (object)idModulo.Value : DBNull.Value;

                comando.Parameters.Add("@IdMaquinaModuloElemento", SqlDbType.Int).Value =
                    idElemento.HasValue && idElemento > 0 ? (object)idElemento.Value : DBNull.Value;

                comando.Parameters.Add("@IdTipoNovedad", SqlDbType.Int).Value =
                    idTipoNovedad.HasValue && idTipoNovedad > 0 ? (object)idTipoNovedad.Value : DBNull.Value;

                comando.Parameters.Add("@TipoMantenimiento", SqlDbType.NVarChar, 20).Value =
                    string.IsNullOrWhiteSpace(tipoMantenimiento) || tipoMantenimiento == "0"
                    ? (object)DBNull.Value
                    : tipoMantenimiento;

                comando.Parameters.Add("@IdUsuarioResponsable", SqlDbType.Int).Value =
                    idUsuarioResponsable.HasValue && idUsuarioResponsable > 0
                    ? (object)idUsuarioResponsable.Value
                    : DBNull.Value;

                comando.Parameters.Add("@TiempoMinimo", SqlDbType.Int).Value =
                    tiempoMin.HasValue ? (object)tiempoMin.Value : DBNull.Value;

                comando.Parameters.Add("@TiempoMaximo", SqlDbType.Int).Value =
                    tiempoMax.HasValue ? (object)tiempoMax.Value : DBNull.Value;

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new NovedadTecnicaReporteItem
                        {
                            IdNovedadTecnica = (int)lector["IdNovedadTecnica"],
                            IdBitacora = (int)lector["IdBitacora"],
                            Fecha = (DateTime)lector["Fecha"],
                            Turno = lector["Turno"].ToString(),
                            IdMaquina = (int)lector["IdMaquina"],
                            NombreMaquina = lector["NombreMaquina"].ToString(),

                            IdTipoNovedadTecnica = (int)lector["IdTipoNovedadTecnica"],
                            TipoNovedadTecnica = lector["TipoNovedadTecnica"].ToString(),

                            TipoMantenimiento = lector["TipoMantenimiento"] == DBNull.Value ? null : lector["TipoMantenimiento"].ToString(),
                            Descripcion = lector["Descripcion"].ToString(),
                            Diagnostico = lector["Diagnostico"] == DBNull.Value ? null : lector["Diagnostico"].ToString(),
                            SolucionAplicada = lector["SolucionAplicada"] == DBNull.Value ? null : lector["SolucionAplicada"].ToString(),
                            ReportadoPor = lector["ReportadoPor"] == DBNull.Value ? null : lector["ReportadoPor"].ToString(),
                            Validado = (bool)lector["Validado"],
                            TiempoPerdidoMinutos = lector["TiempoPerdidoMinutos"] == DBNull.Value ? (int?)null : (int)lector["TiempoPerdidoMinutos"],

                            IdMaquinaModulo = lector["IdMaquinaModulo"] == DBNull.Value ? (int?)null : (int)lector["IdMaquinaModulo"],
                            NombreModulo = lector["NombreModulo"] == DBNull.Value ? null : lector["NombreModulo"].ToString(),

                            IdMaquinaModuloElemento = lector["IdMaquinaModuloElemento"] == DBNull.Value ? (int?)null : (int)lector["IdMaquinaModuloElemento"],
                            NombreElemento = lector["NombreElemento"] == DBNull.Value ? null : lector["NombreElemento"].ToString(),

                            IdUsuarioReporta = (int)lector["IdUsuarioReporta"],
                            NombreUsuarioReporta = lector["NombreUsuarioReporta"].ToString(),

                            IdUsuarioResponsable = lector["IdUsuarioResponsable"] == DBNull.Value ? (int?)null : (int)lector["IdUsuarioResponsable"],
                            NombreUsuarioResponsable = lector["NombreUsuarioResponsable"] == DBNull.Value ? null : lector["NombreUsuarioResponsable"].ToString(),

                            FechaCreacion = (DateTime)lector["FechaCreacion"]
                        });
                    }
                }
            }

            return lista;
        }

        public NovedadTecnicaReporteItem ObtenerPorId(int idNovedadTecnica)
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

                    return new NovedadTecnicaReporteItem
                    {
                        IdNovedadTecnica = (int)lector["IdNovedadTecnica"],
                        IdBitacora = (int)lector["IdBitacora"],
                        IdTipoNovedadTecnica = (int)lector["IdTipoNovedadTecnica"],
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

                        NombreUsuarioReporta = lector["NombreUsuarioReporta"].ToString(),
                        TipoNovedadTecnica = lector["TipoNovedadTecnicaNombre"].ToString()
                    };
                }
            }
        }

        public class NovedadTecnicaReporteItem
        {
            public int IdNovedadTecnica { get; set; }
            public int IdBitacora { get; set; }
            public DateTime Fecha { get; set; }
            public string Turno { get; set; }
            public int IdMaquina { get; set; }
            public string NombreMaquina { get; set; }

            public int IdTipoNovedadTecnica { get; set; }
            public string TipoNovedadTecnica { get; set; }
            public string TipoMantenimiento { get; set; }

            public string Descripcion { get; set; }
            public string Diagnostico { get; set; }
            public string SolucionAplicada { get; set; }
            public string ReportadoPor { get; set; }
            public bool Validado { get; set; }
            public int? TiempoPerdidoMinutos { get; set; }

            public int? IdMaquinaModulo { get; set; }
            public string NombreModulo { get; set; }

            public int? IdMaquinaModuloElemento { get; set; }
            public string NombreElemento { get; set; }

            public int IdUsuarioReporta { get; set; }
            public string NombreUsuarioReporta { get; set; }

            public int? IdUsuarioResponsable { get; set; }
            public string NombreUsuarioResponsable { get; set; }

            public DateTime FechaCreacion { get; set; }
        }
    }
}