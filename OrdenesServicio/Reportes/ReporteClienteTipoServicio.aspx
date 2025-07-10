<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteClienteTipoServicio.aspx.cs" Inherits="ZOE.OrdenesServicio.Reportes.ReporteClienteTipoServicio" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
</head>
<body style="padding: 30px 30px 15px 30px;">
    <form id="form1" runat="server">
    <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>

    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="562px" 
        Width="788px">
    </rsweb:ReportViewer>
    <div>
    
    </div>
    </form>
</body>
</html>
