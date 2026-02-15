using BitacorasWeb.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitacorasWeb.Admin
{
    public partial class Productos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Protección por rol
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                CargarProductos();

                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                {
                    lblMsg.Text = Server.HtmlEncode(Request.QueryString["msg"]);
                    lblMsg.Visible = true;
                    // Quita el msg de la URL para que no se repita al refrescar
                    Response.Redirect("~/Admin/Productos.aspx");
                }
            }
        }
        private void CargarProductos()
        {
            bool incluirInactivos = chkIncluirInactivos.Checked;

            ProductoDAL dal = new ProductoDAL();
            gvProductos.DataSource = dal.ListarProductos(incluirInactivos);
            gvProductos.DataBind();
        }
        protected void chkIncluirInactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarProductos();
        }
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/ProductoFormulario.aspx");
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Por ahora solo capturamos el IdProducto
            if (e.CommandArgument == null) return;

            int idProducto = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                // Más adelante crearemos el formulario para editar y asignar máquinas
                Response.Redirect("~/Admin/ProductoFormulario.aspx?id=" + idProducto);
            }
            else if (e.CommandName == "Desactivar")
            {
                ProductoDAL dal = new ProductoDAL();
                dal.DesactivarProducto(idProducto);

                // Recargar grid
                CargarProductos();
            }
            else if (e.CommandName == "Reactivar")
            {
                ProductoDAL dal = new ProductoDAL();
                dal.ReactivarProducto(idProducto);

                CargarProductos();
            }
        }

    }
}