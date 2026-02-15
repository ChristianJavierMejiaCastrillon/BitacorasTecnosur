using BitacorasWeb.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitacorasWeb.Admin
{
    public partial class ProductoFormulario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Solo Admin
            if (Session["Rol"] == null || Session["Rol"].ToString() != "Administrador")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarMaquinas();

                if (int.TryParse(Request.QueryString["id"], out int idProducto) && idProducto > 0)
                {
                    CargarProductoParaEdicion(idProducto);
                }
                else
                {
                    titulo.InnerText = "Nuevo Producto";
                }
            }
        }

        private void CargarMaquinas()
        {
            var dal = new MaquinaDAL();
            var lista = dal.ListarMaquinasParaDropdown(); // solo activas

            cblMaquinas.DataSource = lista;
            cblMaquinas.DataTextField = "Nombre";
            cblMaquinas.DataValueField = "IdMaquina";
            cblMaquinas.DataBind();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Productos.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // 1️ Detectar si es edición (si viene id en la URL)
                int? idProducto = null;

                if (int.TryParse(Request.QueryString["id"], out int idQuery) && idQuery > 0)
                {
                    idProducto = idQuery;
                }

                // 2️ Guardar producto (insert o update)
                ProductoDAL productoDal = new ProductoDAL();

                int idGenerado = productoDal.GuardarProducto(
                    idProducto,
                    txtCodigo.Text.Trim(),
                    txtNombre.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtDescripcion.Text)
                        ? null
                        : txtDescripcion.Text.Trim()
                );

                // 3️ Obtener máquinas seleccionadas
                List<int> idsMaquina = new List<int>();

                foreach (ListItem item in cblMaquinas.Items)
                {
                    if (item.Selected)
                        idsMaquina.Add(Convert.ToInt32(item.Value));
                }

                // Validación: mínimo 1 máquina
                if (idsMaquina.Count == 0)
                {
                    lblMensaje.Text = "Debes seleccionar al menos una máquina.";
                    return;
                }



                // 4️ Reemplazar asignaciones
                MaquinaProductoDAL mpDal = new MaquinaProductoDAL();
                mpDal.ReemplazarAsignaciones(idGenerado, idsMaquina);

                // 5️ Redirigir al listado con mensaje
                string mensaje = idProducto.HasValue
                    ? "Producto actualizado correctamente."
                    : "Producto creado correctamente.";

                Response.Redirect("~/Admin/Productos.aspx?msg=" + Server.UrlEncode(mensaje));
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
            }
        }



        private void CargarProductoParaEdicion(int idProducto)
        {
            ProductoDAL productoDal = new ProductoDAL();
            MaquinaProductoDAL mpDal = new MaquinaProductoDAL();

            // 1) Cargar datos del producto
            var producto = productoDal.ObtenerProductoPorId(idProducto);

            if (producto == null)
            {
                lblMensaje.Text = "Producto no encontrado.";
                return;
            }

            hfIdProducto.Value = producto.IdProducto.ToString();
            txtCodigo.Text = producto.Codigo;
            txtNombre.Text = producto.Nombre;
            txtDescripcion.Text = producto.Descripcion;

            titulo.InnerText = "Editar Producto";

            // 2) Cargar máquinas ya asignadas
            var maquinasAsignadas = mpDal.ListarIdsMaquinasPorProducto(idProducto);

            foreach (ListItem item in cblMaquinas.Items)
            {
                if (maquinasAsignadas.Contains(Convert.ToInt32(item.Value)))
                    item.Selected = true;
            }
        }
    }
}