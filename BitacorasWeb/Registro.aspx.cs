using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitacorasWeb
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarOperarios();
            }
        }

        private void CargarOperarios()
        {
            var dal = new BitacorasWeb.Datos.UsuarioDAL();

            ddlOperario.DataSource = dal.ListarUsuariosParaDropdown();
            ddlOperario.DataTextField = "NombreCompleto";
            ddlOperario.DataValueField = "IdUsuario";
            ddlOperario.DataBind();

            ddlOperario.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione...", "0"));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // TODO: Guardar en BD (pronto)
                // Por ahora, mostramos un aviso simple:
                ClientScript.RegisterStartupScript(GetType(), "ok",
                    "alert('Novedad registrada correctamente (demo).');", true);

                LimpiarFormulario();
            }
        }


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();

        }

        private void LimpiarFormulario()
        {
            txtFecha.Text = "";
            ddlTurno.SelectedIndex = 0;
            ddlMaquina.SelectedIndex = 0;
            ddlProducto.SelectedIndex = 0;
            ddlTipo.SelectedIndex = 0;
            txtDescripcion.Text = "";
        }
    }
}