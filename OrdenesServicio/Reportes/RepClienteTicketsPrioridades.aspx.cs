using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepClienteTicketsPrioridades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Obtener el parámetro de clienteId del query string
                int clienteId = Convert.ToInt32(Request.QueryString["clienteId"]);
                ViewState["clienteId"] = clienteId;

                CargarAsesores();
                CargarProyectos(clienteId);
            }
        }

        private void CargarProyectos(int clienteId)
        {
            ddlProyecto.DataValueField = "ProyectoId";
            ddlProyecto.DataTextField = "Descr";
            ddlProyecto.DataSource = OrdenesServicio.Negocio.OrdenServicioBC.ObtenerProyectosPorCliente(clienteId);
            ddlProyecto.DataBind();
        }

        private void CargarAsesores()
        {
            ddlAsesor.DataValueField = "AsesorId";
            ddlAsesor.DataTextField = "NombreCompleto";
            ddlAsesor.DataSource = OrdenesServicio.Negocio.OrdenServicioBC.ObtenerAsesores();
            ddlAsesor.DataBind();
        }

        public void Mostrar()
        {
            string clienteId = Request.QueryString["clienteId"];

            RepClienteTicketsPrioridad.Visible = true;
            RepClienteTicketsPrioridad.LocalReport.ReportPath = @"Reportes\RepClienteTicketsPrioridades.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerTicketsDetallePrioridad");
            ObjectDataSource ticketsPrioridadDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerTicketPrioridades");
            ObjectDataSource proyAbonosDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ProyectoBC", "ObtenerAbonosPorCliente");

            string proyectoId = "0";
            string asesorId = "0";
            string ticket = "0";
            string fechaIni = "";
            string fechaFin = "";
            string sinFecha = "false";

            if (!chkTodosProyecto.Checked)
                proyectoId = ddlProyecto.SelectedValue; 

            if (!chkTodos.Checked)
                asesorId = ddlAsesor.SelectedValue;

            if (!chkTodosTicket.Checked)
                ticket = txtTicket.Value;

            if (chkSinRangoFechas.Checked)
                sinFecha = "true";

            fechaIni = dtpInicial.Value;
            fechaFin = dtpFinal.Value;

            objDS.SelectParameters.Add("clienteId", System.Data.DbType.Int32, clienteId);
            objDS.SelectParameters.Add("proyectoId", System.Data.DbType.Int32, proyectoId);
            objDS.SelectParameters.Add("asesorId", System.Data.DbType.Int32, asesorId);
            objDS.SelectParameters.Add("fechaInicial", System.Data.DbType.DateTime, fechaIni);
            objDS.SelectParameters.Add("fechaFinal", System.Data.DbType.DateTime, fechaFin);
            objDS.SelectParameters.Add("ticket", System.Data.DbType.Int32, ticket);
            objDS.SelectParameters.Add("sinRangoFecha", System.Data.DbType.Boolean, sinFecha);
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", objDS);

            ticketsPrioridadDS.SelectParameters.Add("clienteId", System.Data.DbType.Int32, clienteId);
            Microsoft.Reporting.WebForms.ReportDataSource ticketPrioridad = new Microsoft.Reporting.WebForms.ReportDataSource("dsTicketPrioridad", ticketsPrioridadDS);

            proyAbonosDS.SelectParameters.Add("clienteId", System.Data.DbType.Int32, clienteId);
            Microsoft.Reporting.WebForms.ReportDataSource proyAbonos = new Microsoft.Reporting.WebForms.ReportDataSource("dsAbono", proyAbonosDS);

            RepClienteTicketsPrioridad.LocalReport.DataSources.Clear();
            RepClienteTicketsPrioridad.LocalReport.DataSources.Add(datasource);
            RepClienteTicketsPrioridad.LocalReport.DataSources.Add(ticketPrioridad);
            RepClienteTicketsPrioridad.LocalReport.DataSources.Add(proyAbonos);

            var cliente = Negocio.OrdenServicioBC.ObtenerClientePorId(Convert.ToInt32(clienteId));
            var sla = cliente?.SLAPorcentaje ?? 0;
            RepClienteTicketsPrioridad.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("rpSLAContratado", Convert.ToString(sla)));

            RepClienteTicketsPrioridad.LocalReport.Refresh();
        }

        protected void btnVerReporte_Click(object sender, EventArgs e)
        {
            Mostrar();
        }
    }
}