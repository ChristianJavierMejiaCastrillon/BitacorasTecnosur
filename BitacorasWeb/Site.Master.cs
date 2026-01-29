using System;
using System.Web.UI;
using BitacorasWeb.Datos;

namespace BitacorasWeb
{
    public partial class SiteMaster : MasterPage
    {
        private readonly UsuarioMaquinaDAL _usuarioMaquinaDAL = new UsuarioMaquinaDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null)
            {
                lnkRegistro.Visible = false;
                lnkReportes.Visible = false;
                lnkLogout.Visible = false;
                lnkLogin.Visible = true;

                lnkUsuarios.Visible = false;
                lnkAsignaciones.Visible = false;
                lnkMaquina.Visible = false;
                lnkEstructuraMaquina.Visible = false;
                return;
            }

            string rol = Session["Rol"]?.ToString() ?? "";
            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            lnkLogin.Visible = false;
            lnkLogout.Visible = true;

            // Registro: solo Operario y Administrador
            lnkRegistro.Visible = (rol == "Operario" || rol == "Administrador");

            // Reportes: todos los roles logueados
            lnkReportes.Visible = true;

            // Usuarios / Asignaciones / Máquinas: SOLO Administrador
            bool esAdmin = (rol == "Administrador");
            lnkUsuarios.Visible = esAdmin;
            lnkAsignaciones.Visible = esAdmin;
            lnkMaquina.Visible = esAdmin;

            // Estructura Máquina: Admin o (Coordinador/Padrinos con máquina asignada)
            bool puedeEstructura = esAdmin;

            if (!puedeEstructura)
            {
                // Si tiene al menos 1 máquina asignada como:
                // COORDINADOR_MAQUINA / TEC_MECANICO_PADRINO / TEC_ELECTRICO_PADRINO
                var maquinasAsignadas = _usuarioMaquinaDAL.ListarMaquinasAsignadas(idUsuario);
                puedeEstructura = maquinasAsignadas != null && maquinasAsignadas.Count > 0;
            }

            lnkEstructuraMaquina.Visible = puedeEstructura;
        }
    }
}
