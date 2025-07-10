using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepActividadesProyectoPorCliente : System.Web.UI.Page
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

        public void Mostrar()
        {
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = @"Reportes\RepProyectoActividades.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerProyectosDetalleServiciosPorCliente");
            string clienteId = "0";
            string fechaIni = "";
            string fechaFin = "";
            string sinFecha = "false";

            if (!chkTodos.Checked)
                clienteId = ddlClientes.SelectedValue;

            if (chkSinRangoFechas.Checked)
                sinFecha = "true";

            fechaIni = dtpInicial.Value;
            fechaFin = dtpFinal.Value;

            objDS.SelectParameters.Add("clienteId", System.Data.DbType.Int16, clienteId);
            objDS.SelectParameters.Add("fechaInicial", System.Data.DbType.DateTime, fechaIni);
            objDS.SelectParameters.Add("fechaFinal", System.Data.DbType.DateTime, fechaFin);
            objDS.SelectParameters.Add("sinRangoFecha", System.Data.DbType.Boolean, sinFecha);
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", objDS);

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