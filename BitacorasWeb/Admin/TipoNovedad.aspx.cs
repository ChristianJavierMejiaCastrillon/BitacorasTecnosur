using System;
using BitacorasWeb.Datos;
using System.Web.UI.WebControls;

namespace BitacorasWeb.Admin
{
    public partial class TiposNovedad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Solo administrador
            if (Session["Rol"]?.ToString() != "Administrador")
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarGrid();
            }
        }

        private void CargarGrid()
        {
            var dal = new TipoNovedadDAL();
            gvTipos.DataSource = dal.ListarTodos();
            gvTipos.DataBind();
        }

        protected void gvTipos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var dal = new TipoNovedadDAL();

                if (e.CommandName == "Editar")
                {
                    var item = dal.ObtenerPorId(id);

                    if (item == null)
                    {
                        lblMensaje.Text = "<div class='alert alert-danger'>No se encontró el tipo de novedad.</div>";
                        return;
                    }

                    hfIdTipoNovedad.Value = item.IdTipoNovedad.ToString();
                    txtNombre.Text = item.Nombre;
                    btnGuardar.Text = "Actualizar";
                    lblMensaje.Text = "";
                }
                else if (e.CommandName == "Toggle")
                {
                    var item = dal.ObtenerPorId(id);

                    if (item == null)
                    {
                        lblMensaje.Text = "<div class='alert alert-danger'>No se encontró el tipo de novedad.</div>";
                        return;
                    }

                    bool nuevoEstado = !item.Activo;
                    dal.CambiarEstado(id, nuevoEstado);

                    lblMensaje.Text = nuevoEstado
                        ? "<div class='alert alert-success'>Tipo de novedad activado correctamente.</div>"
                        : "<div class='alert alert-success'>Tipo de novedad desactivado correctamente.</div>";

                    LimpiarFormulario();
                    CargarGrid();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Error: " + ex.Message + "</div>";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                int id;
                int.TryParse(hfIdTipoNovedad.Value, out id);

                string nombre = txtNombre.Text.Trim();

                var dal = new TipoNovedadDAL();

                dal.Guardar(id == 0 ? (int?)null : id, nombre);

                lblMensaje.Text = (id == 0)
                    ? "<div class='alert alert-success'>Tipo de novedad creado correctamente.</div>"
                    : "<div class='alert alert-success'>Tipo de novedad actualizado correctamente.</div>";

                LimpiarFormulario();
                CargarGrid();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Ya existe"))
                {
                    lblMensaje.Text = "<div class='alert alert-warning'>⚠️ Ya existe un tipo de novedad con ese nombre.</div>";
                }
                else
                {
                    lblMensaje.Text = "<div class='alert alert-danger'>Error: " + ex.Message + "</div>";
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblMensaje.Text = "";
        }

        private void LimpiarFormulario()
        {
            hfIdTipoNovedad.Value = "0";
            txtNombre.Text = "";
            btnGuardar.Text = "Guardar";
        }
    }
}