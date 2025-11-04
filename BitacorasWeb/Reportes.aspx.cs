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
            if (!IsPostBack)
            {
                // Datos demo iniciales
                BindGrid(DatosDemo());
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            var dt = DatosDemo();

            // filtros simples en memoria (luego serán SQL WHERE)
            if (!string.IsNullOrWhiteSpace(txtFiltroFecha.Text))
            {
                DateTime f;
                if (DateTime.TryParse(txtFiltroFecha.Text, out f))
                {
                    dt = dt.AsEnumerable()
                           .Where(r => Convert.ToDateTime(r["Fecha"]).Date == f.Date)
                           .CopyToDataTableOrEmpty(dt);
                }
            }

            if (!string.IsNullOrWhiteSpace(ddlFiltroTurno.SelectedValue))
            {
                dt = dt.AsEnumerable()
                       .Where(r => r["Turno"].ToString() == ddlFiltroTurno.SelectedValue)
                       .CopyToDataTableOrEmpty(dt);
            }

            if (!string.IsNullOrWhiteSpace(ddlFiltroMaquina.SelectedValue))
            {
                dt = dt.AsEnumerable()
                       .Where(r => r["Maquina"].ToString() == ddlFiltroMaquina.SelectedValue)
                       .CopyToDataTableOrEmpty(dt);
            }

            BindGrid(dt);
        }

        protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroFecha.Text = "";
            ddlFiltroTurno.SelectedIndex = 0;
            ddlFiltroMaquina.SelectedIndex = 0;
            BindGrid(DatosDemo());
        }

        protected void gvNovedades_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvNovedades.PageIndex = e.NewPageIndex;
            // Rebind (en real se guardan filtros/resultado en sesión o se consulta otra vez)
            BindGrid(DatosDemo());
        }

        private void BindGrid(DataTable dt)
        {
            gvNovedades.DataSource = dt;
            gvNovedades.DataBind();
        }

        // ====== Datos de ejemplo (mientras conectamos la BD) ======
        private DataTable DatosDemo()
        {
            var dt = new DataTable();
            dt.Columns.Add("Fecha", typeof(DateTime));
            dt.Columns.Add("Turno", typeof(string));
            dt.Columns.Add("Maquina", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Tipo", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));

            dt.Rows.Add(DateTime.Today, "Turno1", "Laminadora 1", "Producto A", "Calidad", "Desalineación leve");
            dt.Rows.Add(DateTime.Today, "Turno 2", "Selladora ZX-5", "Producto B", "Producción", "Parada por ajuste");
            dt.Rows.Add(DateTime.Today.AddDays(-1), "Turno3", "Empacadora MK-200", "Producto C", "Mantenimiento", "Cambio de banda");
            dt.Rows.Add(DateTime.Today.AddDays(-2), "Turno1", "Laminadora 1", "Producto A", "Seguridad", "Derrame controlado");

            return dt;
        }
    }

        // Helper para LINQ -> DataTable vacío si no hay filas
    public static class DataTableExtensions
    {
        public static DataTable CopyToDataTableOrEmpty(this EnumerableRowCollection<DataRow> rows, DataTable schemaSource)
        {
            var result = schemaSource.Clone();
            foreach (var r in rows) result.ImportRow(r);
            return result;
        }
    }
}