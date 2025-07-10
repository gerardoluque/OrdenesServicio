using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepAsesorHorasPorDia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.dtpInicial.Value = System.DateTime.Today.AddDays(-7).ToString();
                this.dtpFinal.Value = System.DateTime.Today.AddDays(-1).ToString();
            }
        }


        public void Mostrar()
        {
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = @"Reportes\RepAsesorHorasPorDia.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerTotalHorasPorDiaAsesoresPorFecha");
            string fechaIni = "";
            string fechaFin = "";

            fechaIni = dtpInicial.Value.Substring(0, 10);
            fechaFin = dtpFinal.Value.Substring(0, 10);

            objDS.SelectParameters.Add("fechaInicial", System.Data.DbType.DateTime, fechaIni);
            objDS.SelectParameters.Add("fechaFinal", System.Data.DbType.DateTime, fechaFin);
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", objDS);

            Microsoft.Reporting.WebForms.ReportParameter param1 = new Microsoft.Reporting.WebForms.ReportParameter("pFechaInicial", fechaIni);
            Microsoft.Reporting.WebForms.ReportParameter param2 = new Microsoft.Reporting.WebForms.ReportParameter("pFechaFinal", fechaFin);

            ReportViewer1.LocalReport.SetParameters(param1);
            ReportViewer1.LocalReport.SetParameters(param2);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportViewer1.LocalReport.Refresh();
        }

        protected void btnVerReporte_Click(object sender, EventArgs e)
        {
            Mostrar();
        }

    }
}