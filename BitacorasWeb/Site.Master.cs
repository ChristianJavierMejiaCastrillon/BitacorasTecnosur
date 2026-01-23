using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitacorasWeb
{
    public partial class SiteMaster : MasterPage
    {
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
                return;
            }

            string rol = Session["Rol"]?.ToString();

            lnkLogin.Visible = false;
            lnkLogout.Visible = true;

            // Registro: solo Operario y Admin
            lnkRegistro.Visible = (rol == "Operario" || rol == "Administrador");

            // Reportes: todos los roles logueados
            lnkReportes.Visible = true;

            // Usuarios: SOLO Admin
            lnkUsuarios.Visible = (rol == "Administrador");
            lnkAsignaciones.Visible = (rol == "Administrador");
            lnkMaquina.Visible = (rol == "Administrador");

        }
    }
}