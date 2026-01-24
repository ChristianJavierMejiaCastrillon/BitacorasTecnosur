using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class UsuarioMaquinaDAL
    {
        // ===============================
        // LISTAR ASIGNACIONES POR USUARIO
        // ===============================
        public List<UsuarioMaquinaItem> ListarAsignacionesPorUsuario(int idUsuario, bool soloActivas = true)
        {
            var lista = new List<UsuarioMaquinaItem>();

            string sql = @"
                SELECT
                    um.IdUsuarioMaquina,
                    um.IdUsuario,
                    um.IdMaquina,
                    m.Nombre AS Maquina,
                    um.IdTipoAsignacion,
                    ta.Codigo AS TipoCodigo,
                    ta.Nombre AS TipoNombre,
                    um.Activo,
                    um.FechaInicio,
                    um.FechaFin
                FROM dbo.UsuarioMaquina um
                INNER JOIN dbo.Maquina m ON m.IdMaquina = um.IdMaquina
                INNER JOIN dbo.TipoAsignacion ta ON ta.IdTipoAsignacion = um.IdTipoAsignacion
                WHERE um.IdUsuario = @IdUsuario
            ";

            if (soloActivas)
                sql += " AND um.Activo = 1 AND (um.FechaFin IS NULL OR um.FechaFin >= CAST(GETDATE() AS date))";

            sql += " ORDER BY um.Activo DESC, um.FechaInicio DESC, um.IdUsuarioMaquina DESC;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(MapUsuarioMaquinaItem(dr));
                    }
                }
            }

            return lista;
        }

        // ===============================
        // LISTAR ASIGNACIONES POR MAQUINA
        // ===============================
        public List<UsuarioMaquinaItem> ListarAsignacionesPorMaquina(int idMaquina, bool soloActivas = true)
        {
            var lista = new List<UsuarioMaquinaItem>();

            string sql = @"
                SELECT
                    um.IdUsuarioMaquina,
                    um.IdUsuario,
                    um.IdMaquina,
                    m.Nombre AS Maquina,
                    um.IdTipoAsignacion,
                    ta.Codigo AS TipoCodigo,
                    ta.Nombre AS TipoNombre,
                    um.Activo,
                    um.FechaInicio,
                    um.FechaFin
                FROM dbo.UsuarioMaquina um
                INNER JOIN dbo.Maquina m ON m.IdMaquina = um.IdMaquina
                INNER JOIN dbo.TipoAsignacion ta ON ta.IdTipoAsignacion = um.IdTipoAsignacion
                WHERE um.IdMaquina = @IdMaquina
            ";

            if (soloActivas)
                sql += " AND um.Activo = 1 AND (um.FechaFin IS NULL OR um.FechaFin >= CAST(GETDATE() AS date))";

            sql += " ORDER BY um.Activo DESC, um.FechaInicio DESC, um.IdUsuarioMaquina DESC;";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdMaquina", SqlDbType.Int).Value = idMaquina;

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(MapUsuarioMaquinaItem(dr));
                    }
                }
            }

            return lista;
        }

        // =======================================================
        // VALIDAR SI EXISTE ASIGNACION ACTIVA (EVITAR DUPLICADOS)
        // =======================================================
        public bool ExisteAsignacionActiva(int idUsuario, int idMaquina, int idTipoAsignacion)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM dbo.UsuarioMaquina
                WHERE IdUsuario = @IdUsuario
                  AND IdMaquina = @IdMaquina
                  AND IdTipoAsignacion = @IdTipoAsignacion
                  AND Activo = 1
                  AND (FechaFin IS NULL OR FechaFin >= CAST(GETDATE() AS date));
            ";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                cmd.Parameters.Add("@IdMaquina", SqlDbType.Int).Value = idMaquina;
                cmd.Parameters.Add("@IdTipoAsignacion", SqlDbType.Int).Value = idTipoAsignacion;

                con.Open();
                int total = Convert.ToInt32(cmd.ExecuteScalar());
                return total > 0;
            }
        }

        // ============================
        // INSERTAR ASIGNACION (NUEVA)
        // ============================
        public int InsertarAsignacion(int idUsuario, int idMaquina, int idTipoAsignacion, DateTime fechaInicio)
        {
            const string sql = @"
                INSERT INTO dbo.UsuarioMaquina (IdUsuario, IdMaquina, IdTipoAsignacion, Activo, FechaInicio, FechaFin)
                VALUES (@IdUsuario, @IdMaquina, @IdTipoAsignacion, 1, @FechaInicio, NULL);

                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                cmd.Parameters.Add("@IdMaquina", SqlDbType.Int).Value = idMaquina;
                cmd.Parameters.Add("@IdTipoAsignacion", SqlDbType.Int).Value = idTipoAsignacion;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = fechaInicio;

                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ============================
        // FINALIZAR ASIGNACION (HISTORICO)
        // - Deja Activo = 0
        // - Pone FechaFin
        // ============================
        public void FinalizarAsignacion(int idUsuarioMaquina, DateTime fechaFin)
        {
            const string sql = @"
                UPDATE dbo.UsuarioMaquina
                SET Activo = 0,
                    FechaFin = @FechaFin
                WHERE IdUsuarioMaquina = @IdUsuarioMaquina;
            ";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdUsuarioMaquina", SqlDbType.Int).Value = idUsuarioMaquina;
                cmd.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = fechaFin;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ============================
        // REACTIVAR ASIGNACION (OPCIONAL)
        // - Activo = 1
        // - FechaFin = NULL
        // ============================
        public void ReactivarAsignacion(int idUsuarioMaquina, DateTime fechaInicio)
        {
            const string sql = @"
                UPDATE dbo.UsuarioMaquina
                SET Activo = 1,
                    FechaInicio = @FechaInicio,
                    FechaFin = NULL
                WHERE IdUsuarioMaquina = @IdUsuarioMaquina;
            ";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@IdUsuarioMaquina", SqlDbType.Int).Value = idUsuarioMaquina;
                cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = fechaInicio;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ============================
        // MAP (lector -> objeto)
        // ============================
        private UsuarioMaquinaItem MapUsuarioMaquinaItem(SqlDataReader dr)
        {
            return new UsuarioMaquinaItem
            {
                IdUsuarioMaquina = Convert.ToInt32(dr["IdUsuarioMaquina"]),
                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                IdMaquina = Convert.ToInt32(dr["IdMaquina"]),
                Maquina = dr["Maquina"].ToString(),
                IdTipoAsignacion = Convert.ToInt32(dr["IdTipoAsignacion"]),
                TipoCodigo = dr["TipoCodigo"].ToString(),
                TipoNombre = dr["TipoNombre"].ToString(),
                Activo = Convert.ToBoolean(dr["Activo"]),
                FechaInicio = dr["FechaInicio"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaInicio"]),
                FechaFin = dr["FechaFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaFin"])
            };
        }

        public void FinalizarAsignacionesActivasPorUsuario(int idUsuario, DateTime fechaFin)
        {
            const string sql = @"
        UPDATE dbo.UsuarioMaquina
        SET Activo = 0,
            FechaFin = @FechaFin
        WHERE IdUsuario = @IdUsuario
          AND Activo = 1
          AND (FechaFin IS NULL OR FechaFin >= CAST(GETDATE() AS date));";

            using (SqlConnection con = ConexionBD.CrearConexion())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ============================
        // Agregar o editar modulos y elementos
        // - Permite gestionar la estructura de una máquina
        // ============================
        public bool PuedeGestionarEstructura(int idUsuario, int idMaquina)
        {
            const string sql = @"
        SELECT COUNT(1)
        FROM dbo.UsuarioMaquina um
        INNER JOIN dbo.TipoAsignacion ta ON ta.IdTipoAsignacion = um.IdTipoAsignacion
        WHERE um.IdUsuario = @IdUsuario
          AND um.IdMaquina = @IdMaquina
          AND um.Activo = 1
          AND ta.Codigo IN (
              'COORDINADOR_MAQUINA',
              'TEC_MECANICO_PADRINO',
              'TEC_ELECTRICO_PADRINO'
          );";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                comando.Parameters.AddWithValue("@IdMaquina", idMaquina);

                conexion.Open();
                int conteo = (int)comando.ExecuteScalar();
                return conteo > 0;
            }
        }


    }
    // ================================
    // DTO: datos para grillas / listas
    // ================================
    public class UsuarioMaquinaItem
    {
        public int IdUsuarioMaquina { get; set; }
        public int IdUsuario { get; set; }
        public int IdMaquina { get; set; }
        public string Maquina { get; set; }

        public int IdTipoAsignacion { get; set; }
        public string TipoCodigo { get; set; }
        public string TipoNombre { get; set; }

        public bool Activo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
