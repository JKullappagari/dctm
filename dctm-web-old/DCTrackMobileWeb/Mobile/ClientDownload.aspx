<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/MMaster.master" AutoEventWireup="true" CodeFile="ClientDownload.aspx.cs" Inherits="Mobile_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
<tr>
<td>
Download this file to install mobile pre-requisites for the first time in a new device.
<h6><asp:LinkButton ID="lnkPreReq" runat="server" onclick="lnkPreReq_Click"> DCTrackMobile Pre-requisites App</asp:LinkButton></h6>
<br />
</td>
</tr>
<tr>
<td>
Download this file to install DCTrackMobile application.
<h6><asp:LinkButton ID="lnkMain" runat="server" onclick="lnkMain_Click"> DCTrackMobile App</asp:LinkButton></h6><br />
</td>
</tr>
</table>
</asp:Content>

