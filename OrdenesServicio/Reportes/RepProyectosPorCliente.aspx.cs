using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZOE.OrdenesServicio.Reportes
{
    public partial class RepProyectosPorCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int proyectoId = 0;

            if (!Page.IsPostBack)
            {
                if (Request["clienteId"] != null)
                    MostrarProyectosPorCliente(Convert.ToInt32(Request["clienteId"]));
                else
                {
                    if (Request["proyectoId"] != null)
                    {
                        proyectoId = Convert.ToInt32(Request["proyectoId"]);
                        MostrarProyectos(proyectoId);
                    }
                }
            }
        }

        public void MostrarProyectosPorCliente(int clienteId)
        {
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = @"Reportes\RepProyectosPorCliente.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerProyectoDetalleServiciosPorCliente");
            objDS.SelectParameters.Add("clienteId", System.Data.DbType.Int32, Convert.ToString(clienteId));
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", objDS);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportViewer1.LocalReport.Refresh();
        }

        public void MostrarProyectos(int proyectoId)
        {
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = @"Reportes\RepProyectoActividades.rdlc";

            ObjectDataSource objDS = new ObjectDataSource("ZOE.OrdenesServicio.Negocio.ReporteBC", "ObtenerProyectoDetalleServicios");
            objDS.SelectParameters.Add("proyectoId", System.Data.DbType.Int32, Convert.ToString(proyectoId));
            Microsoft.Reporting.WebForms.ReportDataSource datasource = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", objDS);

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportViewer1.LocalReport.Refresh();
        }
    }
}