using System;
using System.Web.UI;
using BitacorasWeb.Datos;

namespace BitacorasWeb.Admin
{
    public partial class Usuarios : Page
    {
        private readonly UsuarioDAL _usuarioDal = new UsuarioDAL();
        private readonly RolDAL _rolDal = new RolDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seguridad anti-URL (Admin solamente)
            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string rolSesion = Session["Rol"]?.ToString();
            if (rolSesion != "Administrador")
            {
                Response.Redirect("~/NoAutorizado.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarRoles();
                ddlActivo.SelectedValue = "1";
                ddlEsTurnero.SelectedValue = "0";
                MostrarCampoTurneroSiAplica();
                CargarUsuarios();
            }
        }

        private void CargarRoles()
        {
            var roles = _rolDal.ListarRoles();
            ddlRol.DataSource = roles;
            ddlRol.DataTextField = "Nombre";
            ddlRol.DataValueField = "IdRol";
            ddlRol.DataBind();
        }

        private void CargarUsuarios()
        {
            gvUsuarios.DataSource = _usuarioDal.ListarUsuariosAdmin();
            gvUsuarios.DataBind();
        }

        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarCampoTurneroSiAplica();
        }

        private void MostrarCampoTurneroSiAplica()
        {
            int idRol = int.Parse(ddlRol.SelectedValue);
            string nombreRol = _rolDal.ObtenerNombreRol(idRol);

            bool esTecnico = (nombreRol == "TecnicoElectronico" || nombreRol == "TecnicoMecanico");
            divTurnero.Visible = esTecnico;

            if (!esTecnico)
                ddlEsTurnero.SelectedValue = "0";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.CssClass = "text-danger";

            string usuarioLogin = txtUsuarioLogin.Text.Trim();
            string codigoTrabajador = txtCodigoTrabajador.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string apellidos = txtApellidos.Text.Trim();
            int idRol = int.Parse(ddlRol.SelectedValue);
            bool activo = ddlActivo.SelectedValue == "1";
            bool esTurnero = ddlEsTurnero.SelectedValue == "1";

            if (string.IsNullOrWhiteSpace(usuarioLogin) ||
                string.IsNullOrWhiteSpace(codigoTrabajador) ||
                string.IsNullOrWhiteSpace(nombres) ||
                string.IsNullOrWhiteSpace(apellidos))
            {
                lblMsg.Text = "Todos los campos son obligatorios (incluye Código trabajador).";
                return;
            }

            int idUsuario;
            bool esEdicion = int.TryParse(hfIdUsuario.Value, out idUsuario) && idUsuario > 0;

            try
            {
                if (!esEdicion)
                {
                    // Crear usuario
                    int nuevoId = _usuarioDal.CrearUsuario(usuarioLogin, codigoTrabajador, nombres, apellidos, idRol, activo, esTurnero);

                    // Password inicial = CodigoTrabajador (pero guardado como hash/salt)
                    _usuarioDal.AsignarPassword(nuevoId, codigoTrabajador);

                    lblMsg.CssClass = "text-success";
                    lblMsg.Text = "Usuario creado y contraseña inicial configurada.";
                }
                else
                {
                    // Editar usuario
                    _usuarioDal.ActualizarUsuario(idUsuario, usuarioLogin, codigoTrabajador, nombres, apellidos, idRol, activo, esTurnero);

                    lblMsg.CssClass = "text-success";
                    lblMsg.Text = "Usuario actualizado.";
                }

                LimpiarFormulario();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                lblMsg.CssClass = "text-danger";
                lblMsg.Text = "Error: " + ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblMsg.Text = "";
        }

        private void LimpiarFormulario()
        {
            hfIdUsuario.Value = "";
            txtUsuarioLogin.Text = "";
            txtCodigoTrabajador.Text = "";
            txtNombres.Text = "";
            txtApellidos.Text = "";
            ddlActivo.SelectedValue = "1";
            ddlEsTurnero.SelectedValue = "0";
            MostrarCampoTurneroSiAplica();
        }

        protected void gvUsuarios_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int idUsuario = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "EDITAR")
            {
                var u = _usuarioDal.ObtenerUsuarioPorId(idUsuario);
                if (u == null) return;

                hfIdUsuario.Value = u.IdUsuario.ToString();
                txtUsuarioLogin.Text = u.UsuarioLogin;
                txtCodigoTrabajador.Text = u.CodigoTrabajador;
                txtNombres.Text = u.Nombres;
                txtApellidos.Text = u.Apellidos;
                ddlActivo.SelectedValue = u.Activo ? "1" : "0";
                ddlRol.SelectedValue = u.IdRol.ToString();
                ddlEsTurnero.SelectedValue = u.EsTurnero ? "1" : "0";
                MostrarCampoTurneroSiAplica();

                lblMsg.CssClass = "text-success";
                lblMsg.Text = "Modo edición activado.";
            }
            else if (e.CommandName == "RESET")
            {
                var u = _usuarioDal.ObtenerUsuarioPorId(idUsuario);
                if (u == null) return;

                _usuarioDal.AsignarPassword(idUsuario, u.CodigoTrabajador);

                lblMsg.CssClass = "text-success";
                lblMsg.Text = "Contraseña reseteada al Código trabajador.";
            }
            else if (e.CommandName == "TOGGLE")
            {
                _usuarioDal.ToggleActivo(idUsuario);

                lblMsg.CssClass = "text-success";
                lblMsg.Text = "Estado Activo actualizado.";

                CargarUsuarios();
            }
        }
    }
}
