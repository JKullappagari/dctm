<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptLastTagDetails.aspx.cs" Inherits="RptLastTagDetails"
    MasterPageFile="~/iAssetTrackMasterPage.master" Theme="SkinFile" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<asp:Content ID="RptLastTagDetails" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">


    </script>
    <table cellpadding="0" cellspacing="0" style="vertical-align: top; width: 97%;">
        <tr>
            <td valign="top">
                <div style="width: 100%;">
                  
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" ShowParameterPrompts="true" KeepSessionAlive="true"
                    Font-Names="Verdana" Font-Size="8pt" meta:resourcekey="ReportViewer1Resource1" Visible="true" style="overflow:auto;" ShowFindControls="false" ShowBackButton="false"
                    Height="350px">
                </rsweb:ReportViewer>
            </td>
        </tr>
        <tr>
            <td>
                <input type="hidden" id="hdnLocationID" runat="server" />
                <input type="hidden" id="hdnModelID" runat="server" />
                <input type="hidden" id="hdnModelName" runat="server" />
                <input type="hidden" id="hdnLocName" runat="server" />
                <input type="hidden" id="hdnMessage" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
