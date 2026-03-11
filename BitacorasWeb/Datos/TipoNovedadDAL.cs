using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class TipoNovedadDAL
    {
        // ==========================
        // DTO
        // ==========================
        public class TipoNovedadDTO
        {
            public int IdTipoNovedad { get; set; }
            public string Nombre { get; set; }
            public bool Activo { get; set; }
            public DateTime FechaCreacion { get; set; }
        }

        // Item simple para DropDownList
        public class TipoNovedadItem
        {
            public string Valor { get; set; }
            public string Texto { get; set; }
        }

        // ==========================
        // 1) DROPDOWN (solo activos)
        // ==========================
        public List<TipoNovedadItem> ListarTiposParaDropdown()
        {
            var lista = new List<TipoNovedadItem>();

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedad_ListarActivosDropdown", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new TipoNovedadItem
                        {
                            Valor = lector["IdTipoNovedad"].ToString(),
                            Texto = lector["Nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        // ==========================
        // 2) ADMIN - LISTAR TODOS
        // ==========================
        public List<TipoNovedadDTO> ListarTodos()
        {
            var lista = new List<TipoNovedadDTO>();

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedad_Listar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new TipoNovedadDTO
                        {
                            IdTipoNovedad = (int)lector["IdTipoNovedad"],
                            Nombre = lector["Nombre"].ToString(),
                            Activo = (bool)lector["Activo"],
                            FechaCreacion = (DateTime)lector["FechaCreacion"]
                        });
                    }
                }
            }

            return lista;
        }

        // ==========================
        // 3) ADMIN - OBTENER POR ID
        // ==========================
        public TipoNovedadDTO ObtenerPorId(int idTipoNovedad)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedad_ObtenerPorId", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@IdTipoNovedad", SqlDbType.Int).Value = idTipoNovedad;

                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                        return null;

                    return new TipoNovedadDTO
                    {
                        IdTipoNovedad = (int)lector["IdTipoNovedad"],
                        Nombre = lector["Nombre"].ToString(),
                        Activo = (bool)lector["Activo"],
                        FechaCreacion = (DateTime)lector["FechaCreacion"]
                    };
                }
            }
        }

        // ==========================
        // 4) ADMIN - GUARDAR (INSERT/UPDATE)
        // ==========================
        public void Guardar(int? idTipoNovedad, string nombre)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedad_Guardar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdTipoNovedad", SqlDbType.Int).Value =
                    idTipoNovedad.HasValue ? (object)idTipoNovedad.Value : DBNull.Value;

                comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = nombre;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }

        // ==========================
        // 5) ADMIN - CAMBIAR ESTADO
        // ==========================
        public void CambiarEstado(int idTipoNovedad, bool activo)
        {
            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand("dbo.sp_TipoNovedad_CambiarEstado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@IdTipoNovedad", SqlDbType.Int).Value = idTipoNovedad;
                comando.Parameters.Add("@Activo", SqlDbType.Bit).Value = activo;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
}

