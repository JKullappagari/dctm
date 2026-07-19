<%@ Page Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master" AutoEventWireup="true"
    CodeFile="GeneralError.aspx.cs" Inherits="GeneralErrorPage" Title="General Error Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <div align="left">
        <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" height="350">
            <tbody>
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0" width="75%" align="center">
                            <tr>
                                <td colspan="2" height="10">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <img src="./images/error.gif" border="0">
                                </td>
                                <td class="displayText" valign="top">
                                    <asp:Label ID="lblMsg" runat="server" Text="Error occured while handling your request."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                </td>
                                <td class="displayText">
                                    <asp:LinkButton ID="lnkSendErrReport" runat="server" CssClass="label" Visible="False"
                                        OnClick="lnkSendErrReport_Click">Send Error Report</asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkViewErrReport" runat="server" CssClass="label" OnClick="lnkViewErrReport_Click">View Error Report</asp:LinkButton>&nbsp;
                                    <asp:LinkButton ID="lnkHome" runat="server" CssClass="label" OnClick="lnkHome_Click">Go to Home Page</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 38px">
                                </td>
                                <td valign="top" class="displayText" style="height: 38px">
                                    <div style="width: 600px; word-wrap: break-word; overflow: auto">
                                        <asp:Literal ID="litErrorText" runat="server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 10px">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
