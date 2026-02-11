using BitacorasWeb.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static BitacorasWeb.Datos.ReporteNovedadesDAL;

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
        protected void gvNovedades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblMensaje.Text = "";

            // Solo procesar comandos de fila
            if (e.CommandName != "Editar" && e.CommandName != "Eliminar")
                return;

            int idNovedad = Convert.ToInt32(e.CommandArgument);
            int idUsuarioActual = (int)Session["IdUsuario"];
            string rol = Session["Rol"]?.ToString();

            try
            {
                if (e.CommandName == "Eliminar")
                {
                    // Regla: solo el creador y durante el turno (lo valida el SP)
                    var dal = new NovedadDAL();
                    dal.EliminarNovedad(idNovedad, idUsuarioActual);

                    lblMensaje.Text = "<div class='alert alert-success'>✅ Novedad eliminada correctamente.</div>";
                    CargarResultados(); // refresca grilla
                }
                else if (e.CommandName == "Editar")
                {
                    Response.Redirect("~/Registro.aspx?edit=1&idNovedad=" + idNovedad);
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "<div class='alert alert-danger'>❌ " + ex.Message + "</div>";
            }
        }
        protected void gvNovedades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string rol = Session["Rol"]?.ToString();

            var btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
            var btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");

            // Solo operario puede ver acciones
            if (rol != "Operario")
            {
                if (btnEditar != null) btnEditar.Visible = false;
                if (btnEliminar != null) btnEliminar.Visible = false;
                return;
            }

            int idUsuarioSesion = (int)Session["IdUsuario"];
            var item = (BitacorasWeb.Datos.ReporteNovedadesDAL.NovedadReporteItem)e.Row.DataItem;

            bool esCreador = (item.IdUsuario == idUsuarioSesion);
            bool dentroTurno = EstaDentroDelTurno(item.Fecha, item.Turno);

            bool puede = esCreador && dentroTurno;

            if (btnEditar != null) btnEditar.Visible = puede;
            if (btnEliminar != null) btnEliminar.Visible = puede;
        }


        private bool EstaDentroDelTurno(DateTime fecha, string turno)
        {
            DateTime inicio, fin;

            switch (turno)
            {
                case "Turno 1":
                    inicio = fecha.Date.AddHours(6);
                    fin = fecha.Date.AddHours(14);
                    break;

                case "Turno 2":
                    inicio = fecha.Date.AddHours(14);
                    fin = fecha.Date.AddHours(22);
                    break;

                case "Turno 3":
                    inicio = fecha.Date.AddHours(22);
                    fin = fecha.Date.AddDays(1).AddHours(6); // cruza día
                    break;

                case "Turno Administrativo":
                    inicio = fecha.Date.AddHours(7);
                    fin = fecha.Date.AddHours(17);
                    break;

                default:
                    return false;
            }

            DateTime ahora = DateTime.Now;
            return (ahora >= inicio && ahora <= fin);
        }

    }
}