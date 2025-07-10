<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepListadoPolizaSaldo.aspx.cs" Inherits="ZOE.OrdenesServicio.Reportes.RepListadoPolizaSaldo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
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

    <style type="text/css">
        #form1
        {
            width: 817px;
        }
    </style>
</head>
<body style="padding: 5px 5px 5px 5px;">
    <form id="form1" runat="server">
    <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>

        <div style="width:1104px; border-style:solid; border-width:thin; background-color:#FFFFF0; font-size: 1.00em; font-family:"
             Trebuchet MS" , Verdana, Helvetica, Sans-Serif;">
            <div style="padding-bottom:10px; padding-top:5px; padding-left:5px;">
                <div style="display:inline-block">
                  Cliente: 
                </div>
                <div style="display:inline-block; padding-left:40px;">
                    <asp:DropDownList ID="ddlClientes" runat="server" Height="22px" Width="249px">
                    </asp:DropDownList>
                </div>
                <div style="display:inline-block">
                    <asp:CheckBox ID="chkTodos" runat="server" Text="Todos" />
                </div>
            </div>
            <div style="padding-bottom:15px; padding-left:5px;">
                <asp:Button ID="btnVerReporte" runat="server" Text="Ver Reporte" 
                    onclick="btnVerReporte_Click" />
            </div>
        </div>


    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="680px" 
        Width="1108px">
    </rsweb:ReportViewer>
    <div>
    
    </div>
    </form>
</body>
</html>