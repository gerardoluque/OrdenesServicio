<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepPolizaDetalleServicioPorCliente.aspx.cs" Inherits="ZOE.OrdenesServicio.Reportes.RepPolizaDetalleServicioPorCliente" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
            width: 817px;
        }
    </style>
</head>
<body style="padding: 30px 30px 15px 30px;">
    <form id="form1" runat="server">
    <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>

    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="680px" 
        Width="890px">
    </rsweb:ReportViewer>
    <div>
    
    </div>
    </form>
</body>
</html>