using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class TipoNovedadTecnicaDAL
    {
        public class TipoNovedadTecnicaDTO
        {
            public int IdTipoNovedadTecnica { get; set; }
            public string Nombre { get; set; }
            public bool Activo { get; set; }
            public DateTime FechaCreacion { get; set; }
        }

        public class TipoNovedadTecnicaItem
        {
            public string Valor { get; set; }
            public string Texto { get; set; }
        }

        public List<TipoNovedadTecnicaItem> ListarTiposParaDropdown()
        {
            var lista = new List<TipoNovedadTecnicaItem>();

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedadTecnica_ListarActivosDropdown", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new TipoNovedadTecnicaItem
                        {
                            Valor = lector["IdTipoNovedadTecnica"].ToString(),
                            Texto = lector["Nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<TipoNovedadTecnicaDTO> Listar()
        {
            var lista = new List<TipoNovedadTecnicaDTO>();

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedadTecnica_Listar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new TipoNovedadTecnicaDTO
                        {
                            IdTipoNovedadTecnica = (int)lector["IdTipoNovedadTecnica"],
                            Nombre = lector["Nombre"].ToString(),
                            Activo = (bool)lector["Activo"],
                            FechaCreacion = (DateTime)lector["FechaCreacion"]
                        });
                    }
                }
            }

            return lista;
        }

        public TipoNovedadTecnicaDTO ObtenerPorId(int idTipoNovedadTecnica)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedadTecnica_ObtenerPorId", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@IdTipoNovedadTecnica", SqlDbType.Int).Value = idTipoNovedadTecnica;

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                        return null;

                    return new TipoNovedadTecnicaDTO
                    {
                        IdTipoNovedadTecnica = (int)lector["IdTipoNovedadTecnica"],
                        Nombre = lector["Nombre"].ToString(),
                        Activo = (bool)lector["Activo"],
                        FechaCreacion = (DateTime)lector["FechaCreacion"]
                    };
                }
            }
        }

        public void Guardar(int? idTipoNovedadTecnica, string nombre)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedadTecnica_Guardar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdTipoNovedadTecnica", SqlDbType.Int).Value =
                    idTipoNovedadTecnica.HasValue ? (object)idTipoNovedadTecnica.Value : DBNull.Value;

                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = nombre;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        public void CambiarEstado(int idTipoNovedadTecnica, bool activo)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedadTecnica_CambiarEstado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdTipoNovedadTecnica", SqlDbType.Int).Value = idTipoNovedadTecnica;
                comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
}