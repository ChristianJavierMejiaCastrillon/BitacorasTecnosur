using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitacorasWeb.Datos;

namespace BitacorasWeb
{
    public partial class ReportesNovedadTecnica : System.Web.UI.Page
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
                rol == "TecnicoMecanico" ||
                rol == "CoordinadorMantenimientoElectronico" ||
                rol == "CoordinadorMantenimientoMecanico";

            if (!permitido)
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarTurnos();
                CargarMaquinas();
                CargarTiposNovedad();
                CargarResponsablesTecnicos();

                ddlModulo.Items.Clear();
                ddlModulo.Items.Insert(0, new ListItem("Todos", "0"));

                ddlElemento.Items.Clear();
                ddlElemento.Items.Insert(0, new ListItem("Todos", "0"));
            }
        }

        private void CargarTurnos()
        {
            var dal = new TurnoDAL();
            ddlTurno.DataSource = dal.ListarTurnosParaDropDown();
            ddlTurno.DataTextField = "Texto";
            ddlTurno.DataValueField = "Valor";
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, new ListItem("Todos", "0"));
        }

        private void CargarMaquinas()
        {
            var dal = new MaquinaDAL();
            ddlMaquina.DataSource = dal.ListarMaquinasParaDropdown();
            ddlMaquina.DataTextField = "Nombre";
            ddlMaquina.DataValueField = "IdMaquina";
            ddlMaquina.DataBind();
            ddlMaquina.Items.Insert(0, new ListItem("Todas", "0"));
        }

        private void CargarTiposNovedad()
        {
            var dal = new TipoNovedadDAL();
            ddlTipoNovedadTecnica.DataSource = dal.ListarTiposParaDropdown();
            ddlTipoNovedadTecnica.DataTextField = "Texto";
            ddlTipoNovedadTecnica.DataValueField = "Valor";
            ddlTipoNovedadTecnica.DataBind();
            ddlTipoNovedadTecnica.Items.Insert(0, new ListItem("Todos", "0"));
        }

        private void CargarResponsablesTecnicos()
        {
            var dal = new UsuarioDAL();
            ddlUsuarioResponsable.DataSource = dal.ListarUsuariosParaDropdown();
            ddlUsuarioResponsable.DataTextField = "NombreCompleto";
            ddlUsuarioResponsable.DataValueField = "IdUsuario";
            ddlUsuarioResponsable.DataBind();
            ddlUsuarioResponsable.Items.Insert(0, new ListItem("Todos", "0"));
        }

        private void CargarModulosPorMaquina(int idMaquina)
        {
            ddlModulo.Items.Clear();
            ddlElemento.Items.Clear();

            ddlElemento.Items.Insert(0, new ListItem("Todos", "0"));

            if (idMaquina <= 0)
            {
                ddlModulo.Items.Insert(0, new ListItem("Todos", "0"));
                return;
            }

            var dal = new MaquinaModuloDAL();
            ddlModulo.DataSource = dal.ListarPorMaquina(idMaquina);
            ddlModulo.DataTextField = "Nombre";
            ddlModulo.DataValueField = "IdMaquinaModulo";
            ddlModulo.DataBind();
            ddlModulo.Items.Insert(0, new ListItem("Todos", "0"));
        }

        private void CargarElementosPorModulo(int idMaquinaModulo)
        {
            ddlElemento.Items.Clear();

            if (idMaquinaModulo <= 0)
            {
                ddlElemento.Items.Insert(0, new ListItem("Todos", "0"));
                return;
            }

            var dal = new MaquinaModuloElementoDAL();
            ddlElemento.DataSource = dal.ListarPorModulo(idMaquinaModulo);
            ddlElemento.DataTextField = "Nombre";
            ddlElemento.DataValueField = "IdMaquinaModuloElemento";
            ddlElemento.DataBind();
            ddlElemento.Items.Insert(0, new ListItem("Todos", "0"));
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            try
            {
                DateTime? fechaDesde = null;
                if (!string.IsNullOrWhiteSpace(txtFechaDesde.Text))
                    fechaDesde = DateTime.Parse(txtFechaDesde.Text);

                DateTime? fechaHasta = null;
                if (!string.IsNullOrWhiteSpace(txtFechaHasta.Text))
                    fechaHasta = DateTime.Parse(txtFechaHasta.Text);

                int? idMaquina = null;
                if (int.TryParse(ddlMaquina.SelectedValue, out int maquina) && maquina > 0)
                    idMaquina = maquina;

                int? idModulo = null;
                if (int.TryParse(ddlModulo.SelectedValue, out int modulo) && modulo > 0)
                    idModulo = modulo;

                int? idElemento = null;
                if (int.TryParse(ddlElemento.SelectedValue, out int elemento) && elemento > 0)
                    idElemento = elemento;

                int? idTipoNovedad = null;
                if (int.TryParse(ddlTipoNovedadTecnica.SelectedValue, out int tipo) && tipo > 0)
                    idTipoNovedad = tipo;

                int? idUsuarioResponsable = null;
                if (int.TryParse(ddlUsuarioResponsable.SelectedValue, out int responsable) && responsable > 0)
                    idUsuarioResponsable = responsable;

                int? tiempoMinimo = null;
                if (!string.IsNullOrWhiteSpace(txtTiempoMinimo.Text))
                    tiempoMinimo = int.Parse(txtTiempoMinimo.Text);

                int? tiempoMaximo = null;
                if (!string.IsNullOrWhiteSpace(txtTiempoMaximo.Text))
                    tiempoMaximo = int.Parse(txtTiempoMaximo.Text);

                string turno = ddlTurno.SelectedValue;
                string tipoMantenimiento = ddlTipoMantenimiento.SelectedValue;

                var dal = new ReporteNovedadesTecnicasDAL();
                var lista = dal.BuscarNovedades(
                    fechaDesde,
                    fechaHasta,
                    turno,
                    idMaquina,
                    idModulo,
                    idElemento,
                    idTipoNovedad,
                    tipoMantenimiento,
                    idUsuarioResponsable,
                    tiempoMinimo,
                    tiempoMaximo
                );

                var datos = lista.ConvertAll(x => new
                {
                    x.IdNovedadTecnica,
                    x.Fecha,
                    x.Turno,
                    Maquina = x.NombreMaquina,
                    Modulo = x.NombreModulo,
                    Elemento = x.NombreElemento,
                    TipoNovedad = x.TipoNovedadTecnica,
                    x.TipoMantenimiento,
                    TecnicoResponsable = x.NombreUsuarioResponsable,
                    x.TiempoPerdidoMinutos,
                    x.Descripcion,
                    x.Diagnostico,
                    x.SolucionAplicada
                });

                gvReportesNovedadTecnica.DataSource = datos;
                gvReportesNovedadTecnica.DataBind();

                if (lista.Count == 0)
                {
                    lblMensaje.Text = "<span class='text-warning'>No se encontraron resultados con los filtros seleccionados.</span>";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<span class='text-danger'>❌ Error al buscar: " + ex.Message + "</span>";
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            txtTiempoMinimo.Text = "";
            txtTiempoMaximo.Text = "";

            ddlTurno.SelectedValue = "0";
            ddlMaquina.SelectedValue = "0";
            ddlTipoNovedadTecnica.SelectedValue = "0";
            ddlTipoMantenimiento.SelectedValue = "0";
            ddlUsuarioResponsable.SelectedValue = "0";

            ddlModulo.Items.Clear();
            ddlModulo.Items.Insert(0, new ListItem("Todos", "0"));

            ddlElemento.Items.Clear();
            ddlElemento.Items.Insert(0, new ListItem("Todos", "0"));

            gvReportesNovedadTecnica.DataSource = null;
            gvReportesNovedadTecnica.DataBind();

            lblMensaje.Text = "";
        }

        protected void gvReportesNovedadTecnica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idNovedadTecnica = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect("~/RegistroNovedadTecnica.aspx?edit=1&idNovedadTecnica=" + idNovedadTecnica);
                return;
            }

            if (e.CommandName == "Eliminar")
            {
                try
                {
                    int idUsuarioActual = (int)Session["IdUsuario"];

                    var dal = new NovedadTecnicaDAL();
                    dal.EliminarNovedadTecnica(idNovedadTecnica, idUsuarioActual);

                    lblMensaje.Text = "<div class='alert alert-success'>✅ Novedad técnica eliminada correctamente.</div>";
                    btnBuscar_Click(null, null);
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "<div class='alert alert-danger'>❌ Error al eliminar: " + ex.Message + "</div>";
                }
            }
        }
    }
}