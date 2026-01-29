using System;
using System.Linq;
using BitacorasWeb.Datos;

namespace BitacorasWeb.Admin
{
    public partial class EstructuraMaquina : System.Web.UI.Page
    {
        private readonly MaquinaDAL _maquinaDAL = new MaquinaDAL();
        private readonly UsuarioMaquinaDAL _usuarioMaquinaDAL = new UsuarioMaquinaDAL();
        private readonly MaquinaModuloDAL _moduloDAL = new MaquinaModuloDAL();
        private readonly MaquinaModuloElementoDAL _elementoDAL = new MaquinaModuloElementoDAL();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["IdUsuario"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CargarMaquinasPermitidas();
                LimpiarMensaje();
                pnlModulo.Visible = false;
                pnlElementos.Visible = false;
            }
        }

        private void CargarMaquinasPermitidas()
        {
            string rol = Session["Rol"]?.ToString();
            int idUsuario = (int)Session["IdUsuario"];

            ddlMaquina.Items.Clear();
            ddlMaquina.Items.Add(new System.Web.UI.WebControls.ListItem("-- Seleccione --", ""));

            if (rol == "Administrador")
            {
                var lista = _maquinaDAL.ListarMaquinasParaDropdown();
                foreach (var m in lista)
                    ddlMaquina.Items.Add(new System.Web.UI.WebControls.ListItem(m.Nombre, m.IdMaquina.ToString()));
            }
            else
            {
                var lista = _usuarioMaquinaDAL.ListarMaquinasAsignadas(idUsuario);
                foreach (var m in lista)
                    ddlMaquina.Items.Add(new System.Web.UI.WebControls.ListItem(m.Nombre, m.IdMaquina.ToString()));
            }
        }

        protected void ddlMaquina_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpiarMensaje();
            pnlModulo.Visible = false;
            pnlElementos.Visible = false;

            if (string.IsNullOrWhiteSpace(ddlMaquina.SelectedValue))
            {
                gvModulos.DataSource = null;
                gvModulos.DataBind();
                return;
            }

            int idMaquina = int.Parse(ddlMaquina.SelectedValue);
            ValidarPermisoEstructura(idMaquina);

            CargarModulos(idMaquina);
        }

        private void ValidarPermisoEstructura(int idMaquina)
        {
            string rol = Session["Rol"]?.ToString();
            if (rol == "Administrador") return;

            int idUsuario = (int)Session["IdUsuario"];
            bool puede = _usuarioMaquinaDAL.PuedeGestionarEstructura(idUsuario, idMaquina);

            if (!puede)
            {
                Response.Redirect("~/NoAutorizado.aspx");
            }
        }

        private void CargarModulos(int idMaquina)
        {
            bool incluir = chkIncluirInactivosModulos.Checked;
            gvModulos.DataSource = _moduloDAL.ListarPorMaquina(idMaquina, incluir);
            gvModulos.DataBind();
        }

        protected void chkIncluirInactivosModulos_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMaquina.SelectedValue)) return;
            int idMaquina = int.Parse(ddlMaquina.SelectedValue);
            ValidarPermisoEstructura(idMaquina);
            CargarModulos(idMaquina);
        }

        protected void btnNuevoModulo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMaquina.SelectedValue))
            {
                MostrarMensaje("Seleccione una máquina primero.", "warning");
                return;
            }

            int idMaquina = int.Parse(ddlMaquina.SelectedValue);
            ValidarPermisoEstructura(idMaquina);

            pnlModulo.Visible = true;
            lblTituloModulo.Text = "Nuevo módulo";
            hfIdMaquinaModulo.Value = "";
            txtModuloNombre.Text = "";
            txtModuloDesc.Text = "";
        }

        protected void btnCancelarModulo_Click(object sender, EventArgs e)
        {
            pnlModulo.Visible = false;
        }

        protected void btnGuardarModulo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMaquina.SelectedValue))
            {
                MostrarMensaje("Seleccione una máquina primero.", "warning");
                return;
            }

            int idMaquina = int.Parse(ddlMaquina.SelectedValue);
            ValidarPermisoEstructura(idMaquina);

            string nombre = txtModuloNombre.Text.Trim();
            string desc = txtModuloDesc.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarMensaje("El nombre del módulo es obligatorio.", "danger");
                return;
            }

            try
            {
                int idUsuario = (int)Session["IdUsuario"];

                if (string.IsNullOrWhiteSpace(hfIdMaquinaModulo.Value))
                {
                    _moduloDAL.Agregar(idMaquina, nombre, desc, idUsuario);
                    MostrarMensaje("Módulo creado.", "success");
                }
                else
                {
                    int idModulo = int.Parse(hfIdMaquinaModulo.Value);
                    _moduloDAL.Editar(idModulo, nombre, desc);
                    MostrarMensaje("Módulo actualizado.", "success");
                }

                pnlModulo.Visible = false;
                CargarModulos(idMaquina);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error: " + ex.Message, "danger");
            }
        }

        protected void gvModulos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlMaquina.SelectedValue)) return;
            int idMaquina = int.Parse(ddlMaquina.SelectedValue);
            ValidarPermisoEstructura(idMaquina);

            int idModulo = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Editar")
            {
                var lista = _moduloDAL.ListarPorMaquina(idMaquina, true);
                var mod = lista.FirstOrDefault(x => x.IdMaquinaModulo == idModulo);
                if (mod == null) return;

                pnlModulo.Visible = true;
                lblTituloModulo.Text = "Editar módulo";
                hfIdMaquinaModulo.Value = mod.IdMaquinaModulo.ToString();
                txtModuloNombre.Text = mod.Nombre;
                txtModuloDesc.Text = mod.Descripcion;
            }
            else if (e.CommandName == "Desactivar")
            {
                _moduloDAL.Desactivar(idModulo);
                MostrarMensaje("Módulo desactivado.", "success");
                CargarModulos(idMaquina);
            }
            else if (e.CommandName == "Reactivar")
            {
                _moduloDAL.Reactivar(idModulo);
                MostrarMensaje("Módulo reactivado.", "success");
                CargarModulos(idMaquina);
            }
            else if (e.CommandName == "Elementos")
            {
                AbrirElementos(idMaquina, idModulo);
            }
        }

        private void AbrirElementos(int idMaquina, int idModulo)
        {
            var lista = _moduloDAL.ListarPorMaquina(idMaquina, true);
            var mod = lista.FirstOrDefault(x => x.IdMaquinaModulo == idModulo);
            if (mod == null) return;

            pnlElementos.Visible = true;
            pnlElementoForm.Visible = false;

            lblModuloActual.Text = mod.Nombre;
            hfIdModuloElementos.Value = idModulo.ToString();

            CargarElementos(idModulo);
        }

        private void CargarElementos(int idModulo)
        {
            bool incluir = chkIncluirInactivosElementos.Checked;
            gvElementos.DataSource = _elementoDAL.ListarPorModulo(idModulo, incluir);
            gvElementos.DataBind();
        }

        protected void chkIncluirInactivosElementos_CheckedChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfIdModuloElementos.Value)) return;
            int idModulo = int.Parse(hfIdModuloElementos.Value);
            CargarElementos(idModulo);
        }

        protected void btnNuevoElemento_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfIdModuloElementos.Value))
            {
                MostrarMensaje("Seleccione un módulo primero.", "warning");
                return;
            }

            pnlElementoForm.Visible = true;
            lblTituloElemento.Text = "Nuevo elemento";
            hfIdElemento.Value = "";
            txtElementoNombre.Text = "";
            txtElementoDesc.Text = "";
        }

        protected void btnCancelarElemento_Click(object sender, EventArgs e)
        {
            pnlElementoForm.Visible = false;
        }

        protected void btnGuardarElemento_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfIdModuloElementos.Value))
            {
                MostrarMensaje("Seleccione un módulo primero.", "warning");
                return;
            }

            int idModulo = int.Parse(hfIdModuloElementos.Value);

            string nombre = txtElementoNombre.Text.Trim();
            string desc = txtElementoDesc.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MostrarMensaje("El nombre del elemento es obligatorio.", "danger");
                return;
            }

            try
            {
                int idUsuario = (int)Session["IdUsuario"];

                if (string.IsNullOrWhiteSpace(hfIdElemento.Value))
                {
                    _elementoDAL.Agregar(idModulo, nombre, desc, idUsuario);
                    MostrarMensaje("Elemento creado.", "success");
                }
                else
                {
                    int idElemento = int.Parse(hfIdElemento.Value);
                    _elementoDAL.Editar(idElemento, nombre, desc);
                    MostrarMensaje("Elemento actualizado.", "success");
                }

                pnlElementoForm.Visible = false;
                CargarElementos(idModulo);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error: " + ex.Message, "danger");
            }
        }

        protected void gvElementos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfIdModuloElementos.Value)) return;
            int idModulo = int.Parse(hfIdModuloElementos.Value);

            int idElemento = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Editar")
            {
                var lista = _elementoDAL.ListarPorModulo(idModulo, true);
                var el = lista.FirstOrDefault(x => x.IdMaquinaModuloElemento == idElemento);
                if (el == null) return;

                pnlElementoForm.Visible = true;
                lblTituloElemento.Text = "Editar elemento";
                hfIdElemento.Value = el.IdMaquinaModuloElemento.ToString();
                txtElementoNombre.Text = el.Nombre;
                txtElementoDesc.Text = el.Descripcion;
            }
            else if (e.CommandName == "Desactivar")
            {
                _elementoDAL.Desactivar(idElemento);
                MostrarMensaje("Elemento desactivado.", "success");
                CargarElementos(idModulo);
            }
            else if (e.CommandName == "Reactivar")
            {
                _elementoDAL.Reactivar(idElemento);
                MostrarMensaje("Elemento reactivado.", "success");
                CargarElementos(idModulo);
            }
        }

        private void LimpiarMensaje() => lblMensaje.Text = "";

        private void MostrarMensaje(string texto, string tipo)
        {
            lblMensaje.Text = $"<div class='alert alert-{tipo} mt-3' role='alert'>{texto}</div>";
        }
    }
}
