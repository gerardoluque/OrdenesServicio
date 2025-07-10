using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepPolizaSaldoPorAgotar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        public void MostrarPolizas()
        {
            string min = "0";
            string max = "60";
            string negativos = "true";

            min = txtMin.Text;
            max = txtMax.Text;

            if (!chkNegativos.Checked)
                negativos = "false";

            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = @"Reportes\RepPolizaSaldoPorAgotar.rdlc";
                        
            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerPolizasSaldoPorAgotar");
            objDS.SelectParameters.Add("saldoMin", System.Data.DbType.Int32, min);
            objDS.SelectParameters.Add("saldoMax", System.Data.DbType.Int32, max);
            objDS.SelectParameters.Add("negativos", System.Data.DbType.Boolean, negativos);
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", objDS);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportViewer1.LocalReport.Refresh();
        }

        protected void btnVerReporte_Click(object sender, EventArgs e)
        {
            MostrarPolizas();
        }
    }
}