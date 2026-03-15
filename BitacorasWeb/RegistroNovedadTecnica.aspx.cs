using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitacorasWeb.Datos;

namespace BitacorasWeb
{
    public partial class RegistroNovedadTecnica : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string rol = Session["Rol"]?.ToString();

            bool permitido =
                rol == "Administrador" ||
                rol == "TecnicoElectronico" ||
                rol == "TecnicoMecanico";

            if (!permitido)
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarMaquinas();
                CargarTurnos();
                CargarTiposNovedadTecnica();
                CargarResponsablesTecnicos();

                ddlModulo.Items.Clear();
                ddlModulo.Items.Insert(0, new ListItem("Seleccione...", "0"));

                ddlElemento.Items.Clear();
                ddlElemento.Items.Insert(0, new ListItem("Seleccione...", "0"));

                if (Request.QueryString["edit"] == "1" &&
                    int.TryParse(Request.QueryString["idNovedadTecnica"], out int idNovedadTecnica))
                {
                    CargarNovedadTecnicaParaEdicion(idNovedadTecnica);
                }
            }
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

        private void CargarTurnos()
        {
            var dal = new TurnoDAL();
            ddlTurno.DataSource = dal.ListarTurnosParaDropDown();
            ddlTurno.DataTextField = "Texto";
            ddlTurno.DataValueField = "Valor";
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarTiposNovedadTecnica()
        {
            var dal = new TipoNovedadDAL();
            ddlTipoNovedadTecnica.DataSource = dal.ListarTiposParaDropdown();
            ddlTipoNovedadTecnica.DataTextField = "Texto";
            ddlTipoNovedadTecnica.DataValueField = "Valor";
            ddlTipoNovedadTecnica.DataBind();
            ddlTipoNovedadTecnica.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarResponsablesTecnicos()
        {
            ddlUsuarioResponsable.Items.Clear();

            string nombreUsuario = Session["NombreCompleto"]?.ToString();
            string idUsuario = Session["IdUsuario"]?.ToString();

            ddlUsuarioResponsable.Items.Add(new ListItem(nombreUsuario, idUsuario));
        }

        private void CargarModulosPorMaquina(int idMaquina)
        {
            ddlModulo.Items.Clear();
            ddlElemento.Items.Clear();

            ddlElemento.Items.Insert(0, new ListItem("Seleccione...", "0"));

            if (idMaquina <= 0)
            {
                ddlModulo.Items.Insert(0, new ListItem("Seleccione...", "0"));
                return;
            }

            var dal = new MaquinaModuloDAL();
            ddlModulo.DataSource = dal.ListarPorMaquina(idMaquina);
            ddlModulo.DataTextField = "Nombre";
            ddlModulo.DataValueField = "IdMaquinaModulo";
            ddlModulo.DataBind();
            ddlModulo.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        private void CargarElementosPorModulo(int idMaquinaModulo)
        {
            ddlElemento.Items.Clear();

            if (idMaquinaModulo <= 0)
            {
                ddlElemento.Items.Insert(0, new ListItem("Seleccione...", "0"));
                return;
            }

            var dal = new MaquinaModuloElementoDAL();
            ddlElemento.DataSource = dal.ListarPorModulo(idMaquinaModulo);
            ddlElemento.DataTextField = "Nombre";
            ddlElemento.DataValueField = "IdMaquinaModuloElemento";
            ddlElemento.DataBind();
            ddlElemento.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }

        protected void ddlMaquina_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idMaquina = int.Parse(ddlMaquina.SelectedValue);
            CargarModulosPorMaquina(idMaquina);
        }

        protected void ddlModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idModulo = int.Parse(ddlModulo.SelectedValue);
            CargarElementosPorModulo(idModulo);
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
                int idNovedadTecnicaEdit = 0;
                int.TryParse(hfIdNovedadTecnica.Value, out idNovedadTecnicaEdit);

                DateTime fecha = DateTime.Parse(txtFecha.Text);
                string turno = ddlTurno.SelectedValue;
                int idMaquina = int.Parse(ddlMaquina.SelectedValue);

                int idTipoNovedadTecnica = int.Parse(ddlTipoNovedadTecnica.SelectedValue);
                string tipoMantenimiento = ddlTipoMantenimiento.SelectedValue;

                int? idMaquinaModulo = null;
                if (int.TryParse(ddlModulo.SelectedValue, out int idModulo) && idModulo > 0)
                    idMaquinaModulo = idModulo;

                int? idMaquinaModuloElemento = null;
                if (int.TryParse(ddlElemento.SelectedValue, out int idElemento) && idElemento > 0)
                    idMaquinaModuloElemento = idElemento;

                int? tiempoPerdidoMinutos = null;
                if (!string.IsNullOrWhiteSpace(txtTiempoPerdido.Text))
                    tiempoPerdidoMinutos = int.Parse(txtTiempoPerdido.Text);

                int idUsuarioActual = (int)Session["IdUsuario"];

                int? idUsuarioResponsable = null;
                if (int.TryParse(ddlUsuarioResponsable.SelectedValue, out int idResp) && idResp > 0)
                    idUsuarioResponsable = idResp;

                string descripcion = txtDescripcion.Text.Trim();
                string diagnostico = txtDiagnostico.Text.Trim();
                string solucionAplicada = txtSolucionAplicada.Text.Trim();
                string nombreUsuarioSesion = Session["NombreCompleto"]?.ToString();

                var novedadTecnicaDal = new NovedadTecnicaDAL();

                if (idNovedadTecnicaEdit > 0)
                {
                    novedadTecnicaDal.ActualizarNovedadTecnica(
                        idNovedadTecnicaEdit,
                        idUsuarioActual,
                        idTipoNovedadTecnica,
                        tipoMantenimiento,
                        descripcion,
                        diagnostico,
                        solucionAplicada,
                        tiempoPerdidoMinutos,
                        idMaquinaModulo,
                        idMaquinaModuloElemento,
                        idUsuarioResponsable
                    );

                    Response.Redirect("~/ReportesNovedadTecnica.aspx");
                    return;
                }

                var bitacoraDal = new BitacoraDAL();
                int idBitacora = bitacoraDal.CrearBitacora(fecha, turno, idMaquina, idUsuarioActual);

                novedadTecnicaDal.InsertarNovedadTecnica(
                    idBitacora,
                    idTipoNovedadTecnica,
                    tipoMantenimiento,
                    descripcion,
                    diagnostico,
                    solucionAplicada,
                    tiempoPerdidoMinutos,
                    idMaquinaModulo,
                    idMaquinaModuloElemento,
                    idUsuarioActual,
                    idUsuarioResponsable,
                    nombreUsuarioSesion
                );

                lblMensaje.Text = "<span class='text-success'>✅ Novedad técnica guardada correctamente.</span>";
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
            txtTiempoPerdido.Text = "";
            txtDescripcion.Text = "";
            txtDiagnostico.Text = "";
            txtSolucionAplicada.Text = "";

            ddlTurno.SelectedValue = "0";
            ddlMaquina.SelectedValue = "0";
            ddlTipoNovedadTecnica.SelectedValue = "0";
            ddlTipoMantenimiento.SelectedValue = "0";
            if (ddlUsuarioResponsable.Items.Count > 0)
            {
                ddlUsuarioResponsable.SelectedIndex = 0;
            }

            ddlModulo.Items.Clear();
            ddlModulo.Items.Insert(0, new ListItem("Seleccione...", "0"));

            ddlElemento.Items.Clear();
            ddlElemento.Items.Insert(0, new ListItem("Seleccione...", "0"));

            hfIdNovedadTecnica.Value = "";
            btnGuardar.Text = "Guardar";

            txtFecha.Enabled = true;
            ddlTurno.Enabled = true;
            ddlMaquina.Enabled = true;
        }

        private void CargarNovedadTecnicaParaEdicion(int idNovedadTecnica)
        {
            lblMensaje.Text = "";

            var dal = new NovedadTecnicaDAL();
            var nov = dal.ObtenerNovedadTecnicaParaEdicion(idNovedadTecnica);

            if (nov == null)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>❌ No se encontró la novedad técnica.</div>";
                return;
            }

            int idUsuarioSesion = (int)Session["IdUsuario"];
            string rol = Session["Rol"]?.ToString();

            if (rol != "Administrador" && nov.IdUsuarioReporta != idUsuarioSesion)
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            hfIdNovedadTecnica.Value = nov.IdNovedadTecnica.ToString();

            txtFecha.Text = nov.Fecha.ToString("yyyy-MM-dd");
            ddlTurno.SelectedValue = nov.Turno;
            ddlMaquina.SelectedValue = nov.IdMaquina.ToString();

            CargarModulosPorMaquina(nov.IdMaquina);

            if (nov.IdMaquinaModulo.HasValue)
            {
                ddlModulo.SelectedValue = nov.IdMaquinaModulo.Value.ToString();
                CargarElementosPorModulo(nov.IdMaquinaModulo.Value);
            }

            if (nov.IdMaquinaModuloElemento.HasValue)
                ddlElemento.SelectedValue = nov.IdMaquinaModuloElemento.Value.ToString();

            ddlTipoNovedadTecnica.SelectedValue = nov.IdTipoNovedadTecnica.ToString();
            ddlTipoMantenimiento.SelectedValue = string.IsNullOrWhiteSpace(nov.TipoMantenimiento) ? "0" : nov.TipoMantenimiento;
            ddlUsuarioResponsable.SelectedValue = nov.IdUsuarioResponsable.HasValue ? nov.IdUsuarioResponsable.Value.ToString() : "0";

            txtTiempoPerdido.Text = nov.TiempoPerdidoMinutos.HasValue ? nov.TiempoPerdidoMinutos.Value.ToString() : "";
            txtDescripcion.Text = nov.Descripcion;
            txtDiagnostico.Text = nov.Diagnostico ?? "";
            txtSolucionAplicada.Text = nov.SolucionAplicada ?? "";

            btnGuardar.Text = "Actualizar";

            txtFecha.Enabled = false;
            ddlTurno.Enabled = false;
            ddlMaquina.Enabled = false;
        }
    }
}