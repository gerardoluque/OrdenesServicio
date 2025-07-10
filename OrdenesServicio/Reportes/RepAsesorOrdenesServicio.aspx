<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepAsesorOrdenesServicio.aspx.cs" Inherits="ZOE.OrdenesServicio.Reportes.RepAsesorOrdenesServicio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <link href="../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" /> 
 <link href="../Content/themes/base/jquery.ui.base.css" rel="stylesheet" type="text/css" />
 <link href="../Content/themes/base/jquery.ui.datepicker.css" rel="stylesheet" type="text/css" />

 <script src="../Scripts/jquery-1.5.1.min.js" type="text/javascript"></script>
 <script src="../Scripts/jquery-ui-1.8.11.min.js" type="text/javascript"></script>
  <title></title>
  <script type="text/javascript">
      $(function () {
          $("#dtpInicial").datepicker();
      });
      $(function () {
          $("#dtpFinal").datepicker();
      });
  </script>

</head>

<body>
    <form id="form1" runat="server">
        <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>
        <div style="width:977px; border-style:solid; border-width:thin; background-color:#FFFFF0; font-size: 1.00em; font-family:"
            Trebuchet MS" , Verdana, Helvetica, Sans-Serif;">
            <div style="padding-bottom:10px; padding-top:5px; padding-left:5px;">
                <div style="display:inline-block">
                  Asesor: 
                </div>
                <div style="display:inline-block; padding-left:45px;">
                    <asp:DropDownList ID="ddlAsesor" runat="server" Height="22px" Width="249px">
                    </asp:DropDownList>
                </div>
                <div style="display:inline-block">
                    <asp:CheckBox ID="chkTodos" runat="server" Text="Todos" />
                </div>
            </div>
            <div style="padding-left:5px;">
                <div style="display:inline-block">
                    Fecha Inicial:
                </div>
                <div style="display:inline-block;  padding-left:5px;">
                    <input runat="server" type="text" id="dtpInicial" />
                </div>
            </div>
            <div style="padding-left:5px; padding-top:5px;">
                <div  style="display:inline-block">
                    Fecha Final:
                </div>
                <div style="display:inline-block; padding-left:13px;">
                    <input runat="server"  type="text" id="dtpFinal" />
                </div>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="dtpInicial" ControlToValidate="dtpFinal" 
                    ErrorMessage="La fecha inicial debe ser menor o igual a la fecha final" 
                    ForeColor="Red" Operator="GreaterThanEqual"></asp:CompareValidator>
                <div style="padding-bottom:10px;">
                    <asp:CheckBox ID="chkSinRangoFechas" runat="server" Text="Sin Rango de Fechas" />
                </div>
            </div>

            <div style="padding-left:5px; padding-bottom:5px;">
                <div  style="display:inline-block">
                    Status:
                </div>
                <div style="display:inline-block; padding-left:45px;">
                    <asp:DropDownList ID="ddlStatus" runat="server" Height="22px" Width="150px">
                    </asp:DropDownList>
                </div>
                <div style="display:inline-block">
                    <asp:CheckBox ID="chkTodosStatus" runat="server" Text="Todos" />
                </div>
            </div>

            <div style="padding-bottom:15px; padding-left:5px;">
                <asp:Button ID="btnVerReporte" runat="server" Text="Ver Reporte" 
                    onclick="btnVerReporte_Click" />
            </div>
        </div>
        <rsweb:ReportViewer ID="RepAsesorOrdenes" runat="server"
           Height="600px"  Width="981px" Font-Names="Verdana" Font-Size="8pt" 
            InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
            WaitMessageFont-Size="14pt" BorderStyle="Solid" BorderWidth="1px">
        </rsweb:ReportViewer>
    </form>
</body>
</html>
