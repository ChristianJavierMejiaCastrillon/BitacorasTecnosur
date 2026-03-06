using System;
using System.Web.UI;
using BitacorasWeb.Datos;

namespace BitacorasWeb
{
    public partial class SiteMaster : MasterPage
    {
        private readonly UsuarioMaquinaDAL _usuarioMaquinaDAL = new UsuarioMaquinaDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // 0) SIN SESIÓN
            if (Session["IdUsuario"] == null)
            {
                // derecha
                lnkLogout.Visible = false;
                lnkLogin.Visible = true;

                // Menús izquierda
                menuAdminModulos.Visible = false;  // ✅ no mostrar dropdowns
                menuSimple.Visible = false;        // ✅ (si quieres mostrar solo Inicio, pon true y ajusta)

                return;
            }

            string rol = Session["Rol"]?.ToString() ?? "";
            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            // derecha
            lnkLogin.Visible = false;
            lnkLogout.Visible = true;

            bool esAdmin = (rol == "Administrador");

            // 1) OPERARIO: menú simple
            if (!esAdmin)
            {
                menuAdminModulos.Visible = false;
                menuSimple.Visible = true;

                // Links del menú simple
                lnkRegistro.Visible = (rol == "Operario"); // si otros roles NO deben ver Registro
                lnkReportes.Visible = true;

                return;
            }

            // 2) ADMIN: menú por módulos
            menuSimple.Visible = false;
            menuAdminModulos.Visible = true;

            // Operación (admin)
            lnkRegistroAdmin.Visible = true;
            lnkReportesAdmin.Visible = true;

            // Administración / Catálogos
            lnkUsuarios.Visible = true;
            lnkAsignaciones.Visible = true;
            lnkMaquina.Visible = true;
            lnkProductos.Visible = true;
            lnkProductoFormulario.Visible = true;
            lnkTipoNovedad.Visible = true;

            // Estructura Máquina (tu regla original)
            bool puedeEstructura = true; // admin siempre

            if (!puedeEstructura)
            {
                var maquinasAsignadas = _usuarioMaquinaDAL.ListarMaquinasAsignadas(idUsuario);
                puedeEstructura = maquinasAsignadas != null && maquinasAsignadas.Count > 0;
            }

            lnkEstructuraMaquina.Visible = puedeEstructura;

            // si algún dropdown quedara vacío, lo ocultamos
            menuOperacion.Visible = lnkRegistroAdmin.Visible || lnkReportesAdmin.Visible;
            menuAdministracion.Visible = lnkUsuarios.Visible || lnkAsignaciones.Visible || lnkMaquina.Visible || lnkEstructuraMaquina.Visible;
            menuCatalogos.Visible = lnkProductos.Visible || lnkProductoFormulario.Visible || lnkTipoNovedad.Visible;
        }
    }
}
