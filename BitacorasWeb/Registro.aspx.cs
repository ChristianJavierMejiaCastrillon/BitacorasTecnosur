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
                // Aquí más adelante podrás cargar listas desde BD (Línea, Producto, etc.)
            }
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
            txtOperario.Text = "";
            ddlTipo.SelectedIndex = 0;
            txtDescripcion.Text = "";
        }
    }
}