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
            // 1) Bloqueo si no hay sesión
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // 2) Bloqueo por rol (solo Operario y Admin)
            string rol = Session["Rol"]?.ToString();

            bool permitido = (rol == "Operario" || rol == "Administrador");

            if (!permitido)
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["edit"] == "1" && int.TryParse(Request.QueryString["idNovedad"], out int idNov))
                {
                    CargarNovedadParaEdicion(idNov);
                }

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

            string rol = Session["Rol"]?.ToString();

            if (rol == "Operario")
            {
                int idUsuarioSesion = (int)Session["IdUsuario"];

                ddlOperario.SelectedValue = idUsuarioSesion.ToString();
                ddlOperario.Enabled = false;
            }
            else
            {
                ddlOperario.Enabled = true;
            }
        }

        private void CargarMaquinas()
        {
            string rol = Session["Rol"]?.ToString();
            int idUsuarioSesion = (int)Session["IdUsuario"];

            if (rol == "Operario")
            {
                var umDal = new UsuarioMaquinaDAL();
                ddlMaquina.DataSource = umDal.ListarMaquinasAsignadasOperario(idUsuarioSesion);
                ddlMaquina.DataTextField = "Nombre";
                ddlMaquina.DataValueField = "IdMaquina";
                ddlMaquina.DataBind();
                ddlMaquina.Items.Insert(0, new ListItem("Seleccione...", "0"));
                return;
            }

            // Admin (y otros permitidos) ven todas
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
            int idNovedadEdit = 0;
            int.TryParse(hfIdNovedad.Value, out idNovedadEdit);

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

                string rol = Session["Rol"]?.ToString();

                int idUsuario;
                if (rol == "Operario")
                    idUsuario = (int)Session["IdUsuario"];
                else
                    idUsuario = int.Parse(ddlOperario.SelectedValue);

                int idMaquina = int.Parse(ddlMaquina.SelectedValue);
                int idProducto = int.Parse(ddlProducto.SelectedValue);

                string tipoNovedad = ddlTipo.SelectedValue;
                string descripcion = txtDescripcion.Text.Trim();
                int tiempoPerdidoMin = int.Parse(txtTiempoPerdido.Text);

                // ✅ 2) SI ES EDICIÓN: actualizar y salir (NO crear bitácora)
                if (idNovedadEdit > 0)
                {
                    int idUsuarioActual = (int)Session["IdUsuario"];

                    int? idProductoNullable = (idProducto > 0) ? (int?)idProducto : null;
                    int? tiempoNullable = (int?)tiempoPerdidoMin; // siempre viene

                    var novedadDal = new NovedadDAL();
                    novedadDal.ActualizarNovedad(
                        idNovedadEdit,
                        idUsuarioActual,
                        tipoNovedad,
                        descripcion,
                        idProductoNullable,
                        tiempoNullable
                    );

                    lblMensaje.Text = "<span class='text-success'>✅ Novedad actualizada correctamente.</span>";
                    Response.Redirect("~/Reportes.aspx");
                    return;
                }

                // 3) SI ES NUEVO: crear Bitácora + insertar Novedad
                var bitacoraDal = new BitacoraDAL();
                int idBitacora = bitacoraDal.CrearBitacora(fecha, turno, idMaquina, idUsuario);

                var novedadDalInsert = new NovedadDAL();
                novedadDalInsert.InsertarNovedad(idBitacora, idProducto, tipoNovedad, descripcion, tiempoPerdidoMin, null);

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

        private void CargarNovedadParaEdicion(int idNovedad)
        {
            lblMensaje.Text = "";

            var dal = new NovedadDAL();
            var nov = dal.ObtenerNovedadParaEdicion(idNovedad);

            if (nov == null)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>❌ No se encontró la novedad.</div>";
                return;
            }

            // Seguridad: solo Operario dueño (la regla del turno ya la controlas en Reportes y el SP)
            int idUsuarioSesion = (int)Session["IdUsuario"];
            string rol = Session["Rol"]?.ToString();

            if (rol != "Operario" || nov.IdUsuario != idUsuarioSesion)
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            // Guardar id para el postback
            hfIdNovedad.Value = nov.IdNovedad.ToString();

            // Precargar campos
            txtFecha.Text = nov.Fecha.ToString("yyyy-MM-dd");
            ddlTurno.SelectedValue = nov.Turno;
            ddlMaquina.SelectedValue = nov.IdMaquina.ToString();

            // Estos pueden venir null
            ddlProducto.SelectedValue = (nov.IdProducto.HasValue ? nov.IdProducto.Value.ToString() : "0");

            ddlTipo.SelectedValue = nov.Tipo;

            txtTiempoPerdido.Text = (nov.TiempoPerdidoMinutos.HasValue ? nov.TiempoPerdidoMinutos.Value.ToString() : "");
            txtDescripcion.Text = nov.Descripcion;

            // Ajustes de UI para edición
            btnGuardar.Text = "Actualizar";

            // Bloquear campos que NO se deben cambiar (porque pertenecen a Bitácora)
            txtFecha.Enabled = false;
            ddlTurno.Enabled = false;
            ddlMaquina.Enabled = false;
            ddlOperario.Enabled = false; // ya lo bloqueas para operario, pero por si acaso
        }

    }
}