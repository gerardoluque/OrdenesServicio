using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepListadoPolizaSaldo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarClientes();
            }
        }

        private void CargarClientes()
        {
            ddlClientes.DataValueField = "ClienteId";
            ddlClientes.DataTextField = "Nombre";
            ddlClientes.DataSource = OrdenesServicio.Negocio.OrdenServicioBC.ObtenerClientes();
            ddlClientes.DataBind();
        }

        private void MostrarPolizas()
        {
            string clienteId = "0";

            if (!chkTodos.Checked)
                clienteId = ddlClientes.SelectedValue;

            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = @"Reportes\RepListadoPolizaSaldo.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerPolizasSaldo");
            objDS.SelectParameters.Add("clienteId", System.Data.DbType.Int16, clienteId);
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