using BitacorasWeb.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitacorasWeb
{
    public partial class Reportes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1) Bloqueo si no hay sesión
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // 2) Bloqueo por rol
            string rol = Session["Rol"]?.ToString();

            bool permitido =
                rol == "Administrador" ||
                rol == "Coordinador" ||
                rol == "Operario" ||
                rol == "TecnicoElectronico" ||
                rol == "TecnicoMecanico";

            if (!permitido)
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarTurnos();
                CargarMaquinas();
                CargarResultados(); 
            }
        }

        private void CargarTurnos()
        {
            var turnoDal = new TurnoDAL();
            ddlTurno.DataSource = turnoDal.ListarTurnosParaDropDown();
            ddlTurno.DataTextField = "Texto";
            ddlTurno.DataValueField = "Valor";
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, new ListItem("Todos", "0"));
        }

        private void CargarMaquinas()
        {
            var maquinaDal = new MaquinaDAL();
            ddlMaquina.DataSource = maquinaDal.ListarMaquinasParaDropdown();
            ddlMaquina.DataTextField = "Nombre";
            ddlMaquina.DataValueField = "IdMaquina";
            ddlMaquina.DataBind();
            ddlMaquina.Items.Insert(0, new ListItem("Todas", "0"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarResultados();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            ddlTurno.SelectedValue = "0";
            ddlMaquina.SelectedValue = "0";
            lblMensaje.Text = "";
            CargarResultados();
        }

        private void CargarResultados()
        {
            try
            {
                DateTime? fecha = null;
                if (!string.IsNullOrWhiteSpace(txtFecha.Text))
                {
                    fecha = DateTime.Parse(txtFecha.Text);
                }

                string turno = ddlTurno.SelectedValue;

                int? idMaquina = null;
                int maquinaSeleccionada = int.Parse(ddlMaquina.SelectedValue);
                if (maquinaSeleccionada > 0)
                {
                    idMaquina = maquinaSeleccionada;
                }

                var dal = new ReporteNovedadesDAL();
                var resultados = dal.BuscarNovedades(fecha, turno, idMaquina);

                gvNovedades.DataSource = resultados;
                gvNovedades.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<span class='text-danger'>❌ Error cargando reportes: " + ex.Message + "</span>";
            }
        }
    }
}