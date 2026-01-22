using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitacorasWeb.Datos;

namespace BitacorasWeb.Admin
{
    public partial class Asignaciones : Page
    {
        private readonly UsuarioMaquinaDAL _usuarioMaquinaDal = new UsuarioMaquinaDAL();
        private readonly TipoAsignacionDAL _tipoDal = new TipoAsignacionDAL();

        private readonly UsuarioDAL _usuarioDal = new UsuarioDAL();
        private readonly MaquinaDAL _maquinaDal = new MaquinaDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
                CargarMaquinas();
                CargarTiposAsignacion();

                if (ddlUsuarios.Items.Count > 1)
                {
                    ddlUsuarios.SelectedIndex = 1;
                    CargarGrillaAsignacionesPorUsuario();
                }
            }
        }

        private void CargarUsuarios()
        {
            ddlUsuarios.Items.Clear();
            ddlUsuarios.Items.Add(new ListItem("-- Seleccione --", "0"));

            var usuarios = _usuarioDal.ListarUsuariosParaDropdown(); 

            foreach (var u in usuarios)
            {
                ddlUsuarios.Items.Add(new ListItem(u.NombreCompleto, u.IdUsuario.ToString()));
            }
        }

        private void CargarMaquinas()
        {
            ddlMaquinas.Items.Clear();
            ddlMaquinas.Items.Add(new ListItem("-- Seleccione --", "0"));

            var maquinas = _maquinaDal.ListarMaquinasParaDropdown(); 

            foreach (var m in maquinas)
            {
                ddlMaquinas.Items.Add(new ListItem(m.Nombre, m.IdMaquina.ToString()));
            }
        }

        private void CargarTiposAsignacion()
        {
            ddlTipoAsignacion.Items.Clear();
            ddlTipoAsignacion.Items.Add(new ListItem("-- Seleccione --", "0"));

            var tipos = _tipoDal.Listar(true);

            foreach (var t in tipos)
            {
                
                ddlTipoAsignacion.Items.Add(new ListItem(t.Texto, t.IdTipoAsignacion.ToString()));
            }
        }

        protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrillaAsignacionesPorUsuario();
        }

        protected void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarGrillaAsignacionesPorUsuario();
        }

        private void CargarGrillaAsignacionesPorUsuario()
        {
            lblMsg.Text = "";

            int idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            if (idUsuario <= 0)
            {
                gvAsignaciones.DataSource = null;
                gvAsignaciones.DataBind();
                return;
            }

            var lista = _usuarioMaquinaDal.ListarAsignacionesPorUsuario(idUsuario, soloActivas: false);
            gvAsignaciones.DataSource = lista;
            gvAsignaciones.DataBind();
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            int idUsuario = Convert.ToInt32(ddlUsuarios.SelectedValue);
            int idMaquina = Convert.ToInt32(ddlMaquinas.SelectedValue);
            int idTipo = Convert.ToInt32(ddlTipoAsignacion.SelectedValue);

            if (idUsuario <= 0 || idMaquina <= 0 || idTipo <= 0)
            {
                MostrarError("Debe seleccionar Usuario, Máquina y Tipo de asignación.");
                return;
            }

            DateTime fechaInicio = ParseFecha(txtFechaInicio.Text);

            if (_usuarioMaquinaDal.ExisteAsignacionActiva(idUsuario, idMaquina, idTipo))
            {
                MostrarError("Ya existe una asignación ACTIVA con ese Usuario + Máquina + Tipo.");
                return;
            }

            _usuarioMaquinaDal.InsertarAsignacion(idUsuario, idMaquina, idTipo, fechaInicio);

            MostrarOk("Usuario asignado correctamente.");
            CargarGrillaAsignacionesPorUsuario(); // <-- ESTO ES CLAVE
        }

        protected void gvAsignaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblMsg.Text = "";

            if (e.CommandName != "FINALIZAR" && e.CommandName != "REACTIVAR")
                return;

            int idUsuarioMaquina = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "FINALIZAR")
            {
                
                _usuarioMaquinaDal.FinalizarAsignacion(idUsuarioMaquina, DateTime.Now);
                MostrarOk("Asignación finalizada.");
            }
            else if (e.CommandName == "REACTIVAR")
            {
                _usuarioMaquinaDal.ReactivarAsignacion(idUsuarioMaquina, DateTime.Now);
                MostrarOk("Asignación reactivada.");
            }

            CargarGrillaAsignacionesPorUsuario();
        }

        private DateTime ParseFecha(string valor)
        {
            
            if (string.IsNullOrWhiteSpace(valor))
                return DateTime.Today;

            if (DateTime.TryParseExact(valor, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime fecha))
                return fecha;

          
            if (DateTime.TryParse(valor, out fecha))
                return fecha;

            return DateTime.Today;
        }

        private void MostrarOk(string mensaje)
        {
            lblMsg.Text = $"<div class='alert alert-success mt-3'>{mensaje}</div>";
        }

        private void MostrarError(string mensaje)
        {
            lblMsg.Text = $"<div class='alert alert-danger mt-3'>{mensaje}</div>";
        }
    }
}
