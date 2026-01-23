using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BitacorasWeb.Datos
{
    public class MaquinaDAL
    {
        ///////////////////////////////////////////
        //Para dropdowns (Registro, Reportes, etc.)
        ///////////////////////////////////////////
        public List<MaquinaItem> ListarMaquinasParaDropdown()
        {
            var lista = new List<MaquinaItem>();

            const string sql = @"
                SELECT IdMaquina, Nombre
                FROM Maquina
                WHERE Activo = 1
                ORDER BY Nombre;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new MaquinaItem
                        {
                            IdMaquina = (int)lector["IdMaquina"],
                            Nombre = lector["Nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
        ///////////////////////////////////////////
        //Para grilla de administración (Gestión de máquinas)
        ///////////////////////////////////////////
        public List<MaquinaAdminItem> ListarMaquinasAdmin(bool incluirInactivas = false)
        {
            var lista = new List<MaquinaAdminItem>();

            string sql = @"
                SELECT IdMaquina, Codigo, Nombre, Descripcion, Activo
                FROM Maquina
                " + (incluirInactivas ? "" : "WHERE Activo = 1") + @"
                ORDER BY Nombre;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                conexion.Open();
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(new MaquinaAdminItem
                        {
                            IdMaquina = (int)lector["IdMaquina"],
                            Codigo = lector["Codigo"].ToString(),
                            Nombre = lector["Nombre"].ToString(),
                            Descripcion = lector["Descripcion"] == DBNull.Value ? "" : lector["Descripcion"].ToString(),
                            Activo = (bool)lector["Activo"]
                        });
                    }
                }
            }

            return lista;
        }
        ///////////////////////////////////////////
        //Crear máquina
        ///////////////////////////////////////////
        public int AgregarMaquina(string codigo, string nombre, string descripcion)
        {
            const string sql = @"
                INSERT INTO Maquina (Codigo, Nombre, Descripcion)
                OUTPUT INSERTED.IdMaquina
                VALUES (@Codigo, @Nombre, @Descripcion);";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@Codigo", codigo);
                comando.Parameters.AddWithValue("@Nombre", nombre);
                comando.Parameters.AddWithValue("@Descripcion",
                    string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion);

                conexion.Open();
                return (int)comando.ExecuteScalar();
            }
        }
        ///////////////////////////////////////////
        //Editar máquina
        ///////////////////////////////////////////
        public void EditarMaquina(int idMaquina, string codigo, string nombre, string descripcion)
        {
            const string sql = @"
                UPDATE Maquina
                SET Codigo = @Codigo,
                    Nombre = @Nombre,
                    Descripcion = @Descripcion
                WHERE IdMaquina = @IdMaquina;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdMaquina", idMaquina);
                comando.Parameters.AddWithValue("@Codigo", codigo);
                comando.Parameters.AddWithValue("@Nombre", nombre);
                comando.Parameters.AddWithValue("@Descripcion",
                    string.IsNullOrWhiteSpace(descripcion) ? (object)DBNull.Value : descripcion);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
        ///////////////////////////////////////////
        //Desactivar máquina (soft delete)
        ///////////////////////////////////////////
        public void DesactivarMaquina(int idMaquina)
        {
            const string sql = @"
                UPDATE Maquina
                SET Activo = 0
                WHERE IdMaquina = @IdMaquina;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdMaquina", idMaquina);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
        ///////////////////////////////////////////
        //Reactivar máquina
        ///////////////////////////////////////////
        public void ReactivarMaquina(int idMaquina)
        {
            const string sql = @"
                UPDATE Maquina
                SET Activo = 1
                WHERE IdMaquina = @IdMaquina;";

            using (SqlConnection conexion = ConexionBD.CrearConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdMaquina", idMaquina);

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
    }
    ///////////////////////////////////////////
    //DTO para dropdown
    ///////////////////////////////////////////
    public class MaquinaItem
    {
        public int IdMaquina { get; set; }
        public string Nombre { get; set; }
    }
    ///////////////////////////////////////////
    //DTO para administración
    ///////////////////////////////////////////
    public class MaquinaAdminItem
    {
        public int IdMaquina { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
