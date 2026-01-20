using BitacorasWeb.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitacorasWeb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuarioLogin = txtUsuarioLogin.Text.Trim();
            string pass = txtPass.Text;

            var dal = new UsuarioDAL();
            var user = dal.Login(usuarioLogin, pass);

            if (user == null)
            {
                return;
            }

            Session["IdUsuario"] = user.IdUsuario;
            Session["Rol"] = user.Rol;
            Session["NombreCompleto"] = user.NombreCompleto;

            if (user.Rol == "Administrador")
                Response.Redirect("~/Reportes.aspx");
            else if (user.Rol == "Operario")
                Response.Redirect("~/Registro.aspx");
            else
                Response.Redirect("~/Reportes.aspx");
        }
    }
}