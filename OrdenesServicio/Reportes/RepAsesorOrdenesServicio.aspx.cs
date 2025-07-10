using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepAsesorOrdenesServicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarAsesores();
            }
        }

        private void CargarAsesores()
        {
            ddlAsesor.DataValueField = "AsesorId";
            ddlAsesor.DataTextField = "NombreCompleto";
            ddlAsesor.DataSource = OrdenesServicio.Negocio.OrdenServicioBC.ObtenerAsesores();
            ddlAsesor.DataBind();

            ddlStatus.DataValueField = "OSStatusId";
            ddlStatus.DataTextField = "Descr";
            ddlStatus.DataSource = OrdenesServicio.Negocio.OrdenServicioBC.ObtenerStatus();
            ddlStatus.DataBind();
        }

        public void Mostrar()
        {
            RepAsesorOrdenes.Visible = true;
            RepAsesorOrdenes.LocalReport.ReportPath = @"Reportes\RepAsesorOrdenesServicio.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerOrdenesServicioPorAsesor");
            string asesorId = "0";
            string statusId = "0";
            string fechaIni = "";
            string fechaFin = "";
            string sinFecha = "false";

            if (!chkTodos.Checked)
                asesorId = ddlAsesor.SelectedValue;

            if (!chkTodosStatus.Checked)
                statusId = ddlStatus.SelectedValue;

            if (chkSinRangoFechas.Checked)
                sinFecha = "true";

            fechaIni = dtpInicial.Value;
            fechaFin = dtpFinal.Value;

            objDS.SelectParameters.Add("asesorId", System.Data.DbType.Int16, asesorId);
            objDS.SelectParameters.Add("statusId", System.Data.DbType.Int16, statusId);
            objDS.SelectParameters.Add("fechaInicial", System.Data.DbType.DateTime, fechaIni);
            objDS.SelectParameters.Add("fechaFinal", System.Data.DbType.DateTime, fechaFin);
            objDS.SelectParameters.Add("sinRangoFecha", System.Data.DbType.Boolean, sinFecha);
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("dsOrdenesServicio", objDS);

            RepAsesorOrdenes.LocalReport.DataSources.Clear();
            RepAsesorOrdenes.LocalReport.DataSources.Add(datasource);
           
            RepAsesorOrdenes.LocalReport.Refresh();
        }

        protected void btnVerReporte_Click(object sender, EventArgs e)
        {
            Mostrar();
        }
    }
}