using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitacorasWeb.Datos
{
    public class ReporteNovedadesDAL
    {
        public List<NovedadReporteItem> BuscarNovedades(DateTime? fecha, string turno, int? idMaquina)
        {
            var lista = new List<NovedadReporteItem>();

            string sql = @"
                SELECT
                    n.IdNovedad,
                    b.Fecha,
                    b.Turno,
                    m.Nombre AS Maquina,
                    p.Nombre AS Producto,
                    (u.Nombres + ' ' + u.Apellidos) AS Operario,
                    n.Tipo,
                    n.Descripcion,
                    n.TiempoPerdidoMinutos
                FROM Novedad n
                INNER JOIN Bitacora b ON b.IdBitacora = n.IdBitacora
                INNER JOIN Maquina m ON m.IdMaquina = b.IdMaquina AND m.Activo = 1
                LEFT JOIN Producto p ON p.IdProducto = n.IdProducto
                INNER JOIN Usuario u ON u.IdUsuario = b.IdUsuario
                WHERE 1 = 1
            ";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand())
            {
                comando.Connection = conexion;

                // Filtro por fecha
                if (fecha.HasValue)
                {
                    sql += " AND b.Fecha = @Fecha";
                    comando.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Value.Date;
                }

                // Filtro por turno
                if (!string.IsNullOrWhiteSpace(turno) && turno != "0")
                {
                    sql += " AND b.Turno = @Turno";
                    comando.Parameters.Add("@Turno", SqlDbType.NVarChar, 20).Value = turno;
                }

                // Filtro por máquina
                if (idMaquina.HasValue && idMaquina.Value > 0)
                {
                    sql += " AND b.IdMaquina = @IdMaquina";
                    comando.Parameters.Add("@IdMaquina", SqlDbType.Int).Value = idMaquina.Value;
                }

                sql += " ORDER BY n.IdNovedad DESC;";

                comando.CommandText = sql;

                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new NovedadReporteItem
                        {
                            IdNovedad = (int)lector["IdNovedad"],
                            Fecha = (DateTime)lector["Fecha"],
                            Turno = lector["Turno"].ToString(),
                            Maquina = lector["Maquina"].ToString(),
                            Producto = lector["Producto"] == DBNull.Value ? "" : lector["Producto"].ToString(),
                            Operario = lector["Operario"].ToString(),
                            Tipo = lector["Tipo"].ToString(),
                            Descripcion = lector["Descripcion"].ToString(),
                            TiempoPerdidoMinutos = lector["TiempoPerdidoMinutos"] == DBNull.Value ? 0 : (int)lector["TiempoPerdidoMinutos"]
                        });
                    }
                }
            }

            return lista;
        }

        public class NovedadReporteItem
        {
            public int IdNovedad { get; set; }
            public DateTime FechaHoraRegistro { get; set; }
            public DateTime Fecha { get; set; }
            public string Turno { get; set; }
            public string Maquina { get; set; }
            public string Producto { get; set; }
            public string Operario { get; set; }
            public string Tipo { get; set; }
            public string Descripcion { get; set; }
            public int TiempoPerdidoMinutos { get; set; }
        }
    }
}