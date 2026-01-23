using BitacorasWeb.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitacorasWeb.Admin
{
    public partial class Maquina : System.Web.UI.Page
    {
        private readonly MaquinaDAL _maquinaDAL = new MaquinaDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seguridad anti-URL (Administrador solamente)
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string rolSesion = Session["Rol"]?.ToString();
            if (rolSesion != "Administrador")
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblMensaje.Text = "";
                pnlFormulario.Visible = false;
                CargarGrilla();
            }
        }
        private void CargarGrilla()
        {
            bool incluirInactivas = chkIncluirInactivas.Checked;
            gvMaquinas.DataSource = _maquinaDAL.ListarMaquinasAdmin(incluirInactivas);
            gvMaquinas.DataBind();
        }

        protected void chkIncluirInactivas_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            MostrarMensaje("", ""); // limpia
            pnlFormulario.Visible = true;
            lblTituloFormulario.Text = "Nueva máquina";
            hfIdMaquina.Value = "";
            LimpiarFormulario();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlFormulario.Visible = false;
            LimpiarFormulario();
            MostrarMensaje("", "");
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(nombre))
            {
                MostrarMensaje("Código y Nombre son obligatorios.", "danger");
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(hfIdMaquina.Value))
                {
                    _maquinaDAL.AgregarMaquina(codigo, nombre, descripcion);
                    MostrarMensaje("Máquina creada correctamente.", "success");
                }
                else
                {
                    int idMaquina = int.Parse(hfIdMaquina.Value);
                    _maquinaDAL.EditarMaquina(idMaquina, codigo, nombre, descripcion);
                    MostrarMensaje("Máquina actualizada correctamente.", "success");
                }

                pnlFormulario.Visible = false;
                LimpiarFormulario();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar: " + ex.Message, "danger");
            }
        }

        protected void gvMaquinas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idMaquina = int.Parse(e.CommandArgument.ToString());

            try
            {
                if (e.CommandName == "Editar")
                {
                    // Forma rápida: buscar en memoria desde el listado
                    var lista = _maquinaDAL.ListarMaquinasAdmin(true);
                    var maq = lista.Find(x => x.IdMaquina == idMaquina);
                    if (maq == null)
                    {
                        MostrarMensaje("No se encontró la máquina seleccionada.", "warning");
                        return;
                    }

                    pnlFormulario.Visible = true;
                    lblTituloFormulario.Text = "Editar máquina";
                    hfIdMaquina.Value = maq.IdMaquina.ToString();
                    txtCodigo.Text = maq.Codigo;
                    txtNombre.Text = maq.Nombre;
                    txtDescripcion.Text = maq.Descripcion;

                    MostrarMensaje("", "");
                }
                else if (e.CommandName == "Desactivar")
                {
                    _maquinaDAL.DesactivarMaquina(idMaquina);
                    MostrarMensaje("Máquina desactivada.", "success");
                    CargarGrilla();
                }
                else if (e.CommandName == "Reactivar")
                {
                    _maquinaDAL.ReactivarMaquina(idMaquina);
                    MostrarMensaje("Máquina reactivada.", "success");
                    CargarGrilla();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error: " + ex.Message, "danger");
            }
        }

        // Mensajes con Bootstrap alerts
        private void MostrarMensaje(string texto, string tipoBootstrap)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                lblMensaje.Text = "";
                return;
            }

            // tipoBootstrap: success, danger, warning, info
            lblMensaje.Text = $"<div class='alert alert-{tipoBootstrap} mt-3' role='alert'>{texto}</div>";
        }
    }
}
