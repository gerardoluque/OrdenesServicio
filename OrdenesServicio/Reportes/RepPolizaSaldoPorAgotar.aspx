<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepPolizaSaldoPorAgotar.aspx.cs" Inherits="ZOE.OrdenesServicio.Reportes.RepPolizaSaldoPorAgotar" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
            width: 776px;
        }
    </style>
</head>
<body style="padding: 5px 5px 5px 5px;">
    <form id="form1" runat="server">
    <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>
        <div style="width:769px; border-style:solid; border-width:thin; background-color:#FFFFF0; font-size: 1.00em; font-family:"
             Trebuchet MS" , Verdana, Helvetica, Sans-Serif;">
            <div style="padding-bottom:10px; padding-top:5px; padding-left:5px;">
                Rango de Minutos:
            </div>
            <div style="padding-left:5px;">
                <div style="display:inline-block">
                    Minimo:
                </div>
                <div style="display:inline-block;  padding-left:17px;">
                    <asp:TextBox ID="txtMin" runat="server" style="text-align: right" Width="53px">0</asp:TextBox>
                </div>
            </div>
            <div style="padding-left:5px; padding-top:5px;">
                <div  style="display:inline-block">
                    Maximo:
                </div>
                <div style="display:inline-block; padding-left:13px;">
                    <asp:TextBox ID="txtMax" runat="server" style="text-align: right" Width="52px">60</asp:TextBox>
                </div>
            </div>
            <div style="padding-bottom:5px; padding-top:5px;">
                <asp:CheckBox ID="chkNegativos" runat="server" Text="Incluir Negativos" Checked="True" />
            </div>
            <div style="padding-bottom:10px; padding-left:5px; padding-top:10px;">
                <asp:Button ID="btnVerReporte" runat="server" Text="Ver Reporte" 
                    onclick="btnVerReporte_Click" />
            </div>
        </div>


    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="620px" 
        Width="767px">
    </rsweb:ReportViewer>
    <div>
    
    </div>
    </form>
</body>
</html>