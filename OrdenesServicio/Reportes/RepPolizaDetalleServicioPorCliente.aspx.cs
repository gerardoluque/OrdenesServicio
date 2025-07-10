using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepPolizaDetalleServicioPorCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["clienteId"] != null)
                    MostrarPolizas(Convert.ToInt32(Request["clienteId"]));
            }
        }

        private void MostrarPolizas(int clienteId)
        {
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = @"Reportes\RepPolizaDetalleServicio.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerPolizaDetalleServiciosPorCliente");
            objDS.SelectParameters.Add("clienteId", System.Data.DbType.Int32, Convert.ToString(clienteId));
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", objDS);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportViewer1.LocalReport.Refresh();
        }
    }
}