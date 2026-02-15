using System;
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

                if (Request.QueryString["idNovedad"] != null)
                {
                    int id = int.Parse(Request.QueryString["idNovedad"]);
                    hfIdNovedad.Value = id.ToString();
                    CargarNovedad(id);
                }
            }
        }

        private void CargarCombos()
        {
            var tipoDal = new TipoNovedadDAL();
            ddlTipo.DataSource = tipoDal.ListarTiposParaDropdown();
            ddlTipo.DataTextField = "Texto";
            ddlTipo.DataValueField = "Valor";
            ddlTipo.DataBind();

            var prodDal = new ProductoDAL();
            ddlProducto.DataSource = prodDal.ListarProductosParaDropdown();
            ddlProducto.DataTextField = "Nombre";
            ddlProducto.DataValueField = "IdProducto";
            ddlProducto.DataBind();
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

            ddlTipo.SelectedValue = item.Tipo;
            ddlProducto.SelectedValue = item.IdProducto.ToString();
            txtTiempo.Text = item.TiempoPerdidoMinutos.ToString();
            txtDescripcion.Text = item.Descripcion;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hfIdNovedad.Value);

            var dal = new NovedadDAL();
            dal.ActualizarNovedad(
                id,
                (int)Session["IdUsuario"],
                ddlTipo.SelectedValue,
                txtDescripcion.Text.Trim(),
                int.Parse(ddlProducto.SelectedValue),
                int.Parse(txtTiempo.Text)
            );

            Response.Redirect("~/Reportes.aspx");
        }
    }
}
