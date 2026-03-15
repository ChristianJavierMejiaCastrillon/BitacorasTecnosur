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
            // =========================
            // 0) SIN SESIÓN
            // =========================
            if (Session["IdUsuario"] == null)
            {
                // derecha
                lnkLogin.Visible = true;
                lnkLogout.Visible = false;

                // Menús izquierda
                menuSimple.Visible = false;// ✅ no mostrar dropdowns
                menuAdminModulos.Visible = false;// ✅ (si se quiere mostrar los menus en la barra poner en true, de todas maneras si se intenta acceder a ellos pedira login)

                return;
            }

            string rol = Session["Rol"]?.ToString() ?? "";
            int idUsuario = Convert.ToInt32(Session["IdUsuario"]);

            // derecha
            lnkLogin.Visible = false;
            lnkLogout.Visible = true;

            // =========================
            // 1) PERMISOS POR ROL
            // =========================
            bool esAdmin = rol == "Administrador";
            bool esOperario = rol == "Operario";
            bool esCoordinador = rol == "Coordinador";
            bool esTecnicoElectronico = rol == "TecnicoElectronico";
            bool esTecnicoMecanico = rol == "TecnicoMecanico";
            bool esCoordMantElectrico = rol == "CoordinadorMantenimientoElectrico";
            bool esCoordMantMecanico = rol == "CoordinadorMantenimientoMecanico";

            bool puedeVerRegistroOperario =
                esAdmin || esOperario;

            bool puedeVerReportesOperario =
                esAdmin || esOperario || esCoordinador || esTecnicoElectronico || esTecnicoMecanico;

            bool puedeVerRegistroTecnico =
                esAdmin || esTecnicoElectronico || esTecnicoMecanico;

            bool puedeVerReportesTecnicos =
                esAdmin || esTecnicoElectronico || esTecnicoMecanico || esCoordMantElectrico || esCoordMantMecanico;

            // =========================
            // 2) ADMINISTRADOR
            // =========================
            if (esAdmin)
            {
                menuSimple.Visible = false;
                menuAdminModulos.Visible = true;

                // Operación
                lnkRegistroAdmin.Visible = puedeVerRegistroOperario;
                lnkReportesAdmin.Visible = puedeVerReportesOperario;

                // Bitácora técnicos
                lnkRegistroNovedadTecnicaAdmin.Visible = puedeVerRegistroTecnico;
                //lnkReporteNovedadTecnicaAdmin.Visible = puedeVerReportesTecnicos;

                // Administración / Catálogos
                lnkUsuarios.Visible = true;
                lnkAsignaciones.Visible = true;
                lnkMaquina.Visible = true;
                lnkProductos.Visible = true;
                lnkProductoFormulario.Visible = true;
                lnkTipoNovedad.Visible = true;

                bool puedeEstructura = true; // admin siempre
                if (!puedeEstructura)
                {
                    var maquinasAsignadas = _usuarioMaquinaDAL.ListarMaquinasAsignadas(idUsuario);
                    puedeEstructura = maquinasAsignadas != null && maquinasAsignadas.Count > 0;
                }

                lnkEstructuraMaquina.Visible = puedeEstructura;

                // Ocultar dropdowns vacíos
                menuOperacion.Visible = lnkRegistroAdmin.Visible || lnkReportesAdmin.Visible;
                menuTecnicos.Visible = lnkRegistroNovedadTecnicaAdmin.Visible;
                menuAdministracion.Visible = lnkUsuarios.Visible || lnkAsignaciones.Visible || lnkMaquina.Visible || lnkEstructuraMaquina.Visible;
                menuCatalogos.Visible = lnkProductos.Visible || lnkProductoFormulario.Visible || lnkTipoNovedad.Visible;

                return;
            }

            // ===========================
            // 3) DEMÁS ROLES: MENÚ SIMPLE
            // ===========================
            menuAdminModulos.Visible = false;
            menuSimple.Visible = true;

            lnkInicio.Visible = true;
            lnkRegistro.Visible = puedeVerRegistroOperario;
            lnkReportes.Visible = puedeVerReportesOperario;
            lnkRegistroNovedadTecnica.Visible = puedeVerRegistroTecnico;
            lnkReportesNovedadTecnica.Visible = puedeVerReportesTecnicos;
        }
    }
}