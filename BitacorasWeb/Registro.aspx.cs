using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitacorasWeb.Datos;

namespace BitacorasWeb
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarOperarios();
                CargarMaquinas();
                CargarProductos();
                CargarTurnos();
                CargarTiposNovedad();
            }
        }

        private void CargarOperarios()
        {
            var dal = new UsuarioDAL();

            ddlOperario.DataSource = dal.ListarUsuariosParaDropdown();
            ddlOperario.DataTextField = "NombreCompleto";
            ddlOperario.DataValueField = "IdUsuario";
            ddlOperario.DataBind();
            ddlOperario.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarMaquinas()
        {
            var dal = new MaquinaDAL();
            ddlMaquina.DataSource = dal.ListarMaquinasParaDropdown();
            ddlMaquina.DataTextField = "Nombre";
            ddlMaquina.DataValueField = "IdMaquina";
            ddlMaquina.DataBind();
            ddlMaquina.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarProductos()
        {
            var dal = new ProductoDAL();
            ddlProducto.DataSource = dal.ListarProductosParaDropdown();
            ddlProducto.DataTextField = "Nombre";
            ddlProducto.DataValueField = "IdProducto";
            ddlProducto.DataBind();

            ddlProducto.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarTurnos()
        {
            var dal = new TurnoDAL();
            ddlTurno.DataSource = dal.ListarTurnosParaDropDown();
            ddlTurno.DataTextField = "Texto";
            ddlTurno.DataValueField = "valor";
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarTiposNovedad()
        {
            var dal = new TipoNovedadDAL();
            ddlTipo.DataSource = dal.ListarTiposParaDropdown();
            ddlTipo.DataTextField = "Texto";
            ddlTipo.DataValueField = "valor";
            ddlTipo.DataBind();
            ddlTipo.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            if (!Page.IsValid)
            {
                lblMensaje.Text = "<span class='text-danger'>❌ Revisa los campos obligatorios.</span>";
                return;
            }

            try
            {
                // 1) Leer valores del formulario
                DateTime fecha = DateTime.Parse(txtFecha.Text);
                string turno = ddlTurno.SelectedValue;

                int idUsuario = int.Parse(ddlOperario.SelectedValue);
                int idMaquina = int.Parse(ddlMaquina.SelectedValue);
                int idProducto = int.Parse(ddlProducto.SelectedValue);

                string tipoNovedad = ddlTipo.SelectedValue;
                string descripcion = txtDescripcion.Text.Trim();

                int tiempoPerdidoMin = int.Parse(txtTiempoPerdido.Text);

                // 2) Crear Bitácora
                var bitacoraDal = new BitacoraDAL();
                int idBitacora = bitacoraDal.CrearBitacora(fecha, turno, idMaquina, idUsuario);

                // 3) Insertar Novedad
                var novedadDal = new NovedadDAL();
                novedadDal.InsertarNovedad(idBitacora, idProducto, tipoNovedad, descripcion, tiempoPerdidoMin, null);

                // 4) Mensaje OK + limpiar
                lblMensaje.Text = "<span class='text-success'>✅ Novedad guardada correctamente.</span>";
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<span class='text-danger'>❌ Error al guardar: " + ex.Message + "</span>";
            }
        }


        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            LimpiarFormulario();

        }

        private void LimpiarFormulario()
        {
            txtFecha.Text = "";
            txtDescripcion.Text = "";
            txtTiempoPerdido.Text = "";

            ddlTurno.SelectedValue = "0";
            ddlOperario.SelectedValue = "0";
            ddlMaquina.SelectedValue = "0";
            ddlProducto.SelectedValue = "0";
            ddlTipo.SelectedValue = "0";
        }
    }
}