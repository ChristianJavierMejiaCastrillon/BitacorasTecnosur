using System;
using System.Web.UI.WebControls;
using BitacorasWeb.Datos;

namespace BitacorasWeb.Admin
{
    public partial class EditarNovedad : System.Web.UI.Page
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
                CargarCombos();

                if (int.TryParse(Request.QueryString["idNovedad"], out int id))
                {
                    hfIdNovedad.Value = id.ToString();
                    CargarNovedad(id);
                }
                else
                {
                    lblMensaje.Text = "<div class='alert alert-danger'>Id de novedad inválido.</div>";
                }
            }
        }

        private void CargarCombos()
        {
            // Tipos desde BD (activos)
            var tipoDal = new TipoNovedadDAL();
            ddlTipo.DataSource = tipoDal.ListarTiposParaDropdown();
            ddlTipo.DataTextField = "Texto";
            ddlTipo.DataValueField = "Valor";
            ddlTipo.DataBind();
            ddlTipo.Items.Insert(0, new ListItem("Seleccione...", "0"));

            // Productos
            var prodDal = new ProductoDAL();
            ddlProducto.DataSource = prodDal.ListarProductosParaDropdown();
            ddlProducto.DataTextField = "Nombre";
            ddlProducto.DataValueField = "IdProducto";
            ddlProducto.DataBind();
            ddlProducto.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarNovedad(int idNovedad)
        {
            var dal = new ReporteNovedadesDAL();
            var item = dal.ObtenerPorId(idNovedad);

            if (item == null)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Registro no encontrado</div>";
                return;
            }

            // Producto
            ddlProducto.SelectedValue = item.IdProducto.ToString();

            // Tiempo / Descripción
            txtTiempo.Text = item.TiempoPerdidoMinutos.ToString();
            txtDescripcion.Text = item.Descripcion;

            // Tipo: preferimos IdTipoNovedad si el objeto lo trae
            // Si NO lo trae, busca por texto
            bool tipoAsignado = false;

            // 1) Intentar por IdTipoNovedad si existe en el objeto
            try
            {
                var prop = item.GetType().GetProperty("IdTipoNovedad");
                if (prop != null)
                {
                    var val = prop.GetValue(item, null);
                    if (val != null)
                    {
                        ddlTipo.SelectedValue = val.ToString();
                        tipoAsignado = true;
                    }
                }
            }
            catch { /* ignorar */ }

            // 2) Fallback: buscar por texto
            if (!tipoAsignado)
            {
                ddlTipo.ClearSelection();
                var li = ddlTipo.Items.FindByText(item.Tipo);
                if (li != null) li.Selected = true;
                else ddlTipo.SelectedValue = "0";
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            if (!int.TryParse(hfIdNovedad.Value, out int id))
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Id de novedad inválido.</div>";
                return;
            }

            // Validaciones mínimas
            if (ddlTipo.SelectedValue == "0")
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Selecciona el tipo de novedad.</div>";
                return;
            }

            if (!int.TryParse(txtTiempo.Text, out int tiempo))
            {
                lblMensaje.Text = "<div class='alert alert-danger'>Tiempo inválido.</div>";
                return;
            }

            int idUsuarioActual = (int)Session["IdUsuario"];
            int idTipoNovedad = int.Parse(ddlTipo.SelectedValue);

            int? idProducto = null;
            if (int.TryParse(ddlProducto.SelectedValue, out int idProd) && idProd > 0)
                idProducto = idProd;

            var dal = new NovedadDAL();

            dal.ActualizarNovedad(
                id,
                idUsuarioActual,
                idTipoNovedad,
                txtDescripcion.Text.Trim(),
                idProducto,
                tiempo
            );

            Response.Redirect("~/Reportes.aspx");
        }
    }
}