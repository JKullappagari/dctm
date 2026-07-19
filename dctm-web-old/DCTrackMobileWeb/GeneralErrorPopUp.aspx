<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" AutoEventWireup="true" CodeFile="GeneralErrorPopUp.aspx.cs" Inherits="GeneralErrorPopup" Title="General Error Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="GeneralPopupContentPlaceHolder" Runat="Server">
<div align="left">
<table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" style="height:350px;">
	<tbody>
		<tr>
			<td valign="top" >
				<table cellpadding="0" cellspacing="0" border="0" width="75%" align="center">
					<tr>
						<td colspan="2" height="10"></td>
					</tr>
					<tr>
						<td align="center"><img  alt="" src="./images/error.gif" border="0" /></td>
						<td class="displayText" valign="top"><asp:Label ID="lblMsg" runat="server" Text="Error occured while handling your request."></asp:Label></td>
					</tr>
					<tr>
						<td align="center"></td>
						<td class="displayText">
							<asp:LinkButton id="lnkSendErrReport" runat="server" CssClass="label" Visible="False" OnClick="lnkSendErrReport_Click">Send Error Report</asp:LinkButton>&nbsp;
							<asp:LinkButton id="lnkViewErrReport" runat="server" CssClass="label" OnClick="lnkViewErrReport_Click">View Error Report</asp:LinkButton>&nbsp;
							</td>
					</tr>
					<tr>
						<td align="center" style="height: 38px"></td>
						<td valign="top" class="displayText" style="height: 38px">&nbsp;<br/>
							<asp:Literal id="litErrorText" runat="server"></asp:Literal></td>
					</tr>
					<tr>
						<td colspan="2" style="height: 10px"></td>
					</tr>
				</table>
			</td>
		</tr>
	</tbody>
</table>
</div>
</asp:Content>

