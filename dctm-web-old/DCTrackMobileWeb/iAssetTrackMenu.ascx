<%@ Control Language="C#" AutoEventWireup="true" CodeFile="iAssetTrackMenu.ascx.cs" Inherits="Controls_iAssetTrackMenu" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>


<asp:SiteMapDataSource ID="iDocTrackSiteMapDS" runat="server"
    ShowStartingNode="False" SiteMapProvider="SqlSiteMapProvider" />

<table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <ig:WebDataMenu ID="WebDataMenu1" runat="server" 
                DataSourceID="iDocTrackSiteMapDS" EnableScrolling="false" 
                EnableTheming="true" StyleSetName="RubberBlack"  >
                
                <GroupSettings Orientation="Horizontal" />
                <ClientEvents ItemClick="MenuItem_CLick" />
            </ig:WebDataMenu>
        </td>
    </tr>
    <!--
    <tr>
        <td bgcolor="#8EBBD9">&nbsp;</td>
    </tr>
    -->
</table>
