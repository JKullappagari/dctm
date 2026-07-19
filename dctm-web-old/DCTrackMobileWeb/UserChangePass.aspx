<%@ Page Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master" Theme="SkinFile"
    AutoEventWireup="true" CodeFile="UserChangePass.aspx.cs" Inherits="UserChangePass"
    Culture="auto" meta:resourcekey="PageResource2" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
<script type="text/javascript">
    //<!--
    function ibConfirm_JS_Click(oButton, oEvent) {
        document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
    }
    //-->
</script>
    <table width="100%">
        <tr>
            <td align="center" style="width: 50%" valign="middle">
                <table>
                    <tr>
                        <td style="width: 10%; height: 43px;" valign="top">
                            <asp:Label ID="Label2" runat="server" CssClass="FieldName" Width="214px" meta:resourcekey="Label2Resource2"></asp:Label>
                        </td>
                        <td style="width: 15%; height: 43px;" valign="top">
                            <asp:TextBox ID="txtCurPassword" runat="server" CssClass="FieldValue" TextMode="Password"
                                Width="224px" meta:resourcekey="txtCurPasswordResource2"></asp:TextBox>
                        </td>
                        <td style="width: 70%; height: 43px;" align="left" valign="top">
                            <asp:RequiredFieldValidator ID="rfvLoginPassword" runat="server" ControlToValidate="txtCurPassword"
                                CssClass="ErrValStyle" Display="Dynamic" Width="148px" SetFocusOnError="True"
                                meta:resourcekey="rfvLoginPasswordResource2"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; height: 27px;" valign="top">
                            <asp:Label ID="Label1" runat="server" CssClass="FieldName" Width="212px" meta:resourcekey="Label1Resource2"></asp:Label>
                        </td>
                        <td style="width: 15%; height: 27px;" valign="top">
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="FieldValue" TextMode="Password"
                                Width="224px" meta:resourcekey="txtNewPasswordResource2"></asp:TextBox>
                        </td>
                        <td align="left" valign="top" style="width: 70%; height: 27px;">
                            <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                                CssClass="ErrValStyle" Display="Dynamic" Width="149px" SetFocusOnError="True"
                                meta:resourcekey="rfvNewPasswordResource2"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvNewPassword" runat="server" ControlToCompare="txtCurPassword"
                                ControlToValidate="txtNewPassword" CssClass="ErrValStyle" Operator="NotEqual"
                                SetFocusOnError="True" Width="248px" Display="Dynamic" meta:resourcekey="cvNewPasswordResource2"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">
                            &nbsp;
                        </td>
                        <td style="width: 85%;" align="left" valign="top" colspan="2">
                            <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtNewPassword"
                                CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$"
                                Height="16px" Width="783px" meta:resourcekey="revPasswordResource2"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%; height: 29px;" valign="top">
                            <asp:Label ID="Label3" runat="server" CssClass="FieldName" Width="212px" meta:resourcekey="Label3Resource2"></asp:Label>
                        </td>
                        <td style="width: 15%; height: 29px;" valign="top">
                            <asp:TextBox ID="txtReEnterPassword" runat="server" CssClass="FieldValue" TextMode="Password"
                                Width="222px" meta:resourcekey="txtReEnterPasswordResource2"></asp:TextBox>
                        </td>
                        <td style="width: 70%; height: 29px;" align="left" valign="top">
                            <asp:RequiredFieldValidator ID="rfvReEnterPassword" runat="server" ControlToValidate="txtReEnterPassword"
                                CssClass="ErrValStyle" Display="Dynamic" Width="140px" SetFocusOnError="True"
                                meta:resourcekey="rfvReEnterPasswordResource2"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvReEnterPassword" runat="server" ControlToCompare="txtNewPassword"
                                ControlToValidate="txtReEnterPassword" CssClass="ErrValStyle" SetFocusOnError="True"
                                Width="188px" meta:resourcekey="cvReEnterPasswordResource2"></asp:CompareValidator>
                        </td>
                        <td style="width: 40%;" align="left" valign="top">
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 50%" valign="top">
                <table>
                    <tr>
                        <td align="left" style="width:50%;">
                            <asp:Label ID="lblPasswordRule" runat="server" CssClass="text" meta:resourcekey="lblPasswordRuleResource1"
                                Width="150px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width:50%;">
                            <asp:Label ID="lblRule" runat="server" CssClass="info" meta:resourcekey="lblRuleResource1"
                                Width="600px"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 100%" valign="middle">
                <asp:Label ID="lblMessage" runat="server" CssClass="displayText" ForeColor="Black"
                    Width="862px" Visible="False" meta:resourcekey="lblMessageResource2"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 100%" valign="top">
                <table>
                    <tr>
                        <td style="width: 45%; height: 29px;">
                        </td>
                        <td style="width: 124px" class="style6">
                            <igtxt:WebImageButton ID="ibConfirm" runat="server" ImageDirectory="" OnClick="ibConfirm_Click"
                                SkinID="uwButton" TabIndex="8" UseBrowserDefaults="False" Width="118px" meta:resourcekey="ibConfirmResource2">
                                <ClientSideEvents Click="ibConfirm_JS_Click" />
                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                    WidthOfRightEdge="13" />
                            </igtxt:WebImageButton>
                        </td>
                        <td style="width: 153px" class="style6">
                            &nbsp;
                            <igtxt:WebImageButton ID="ibReset" runat="server" CausesValidation="False" ImageDirectory=""
                                OnClick="ibReset_Click" SkinID="uwButton" TabIndex="8" Text="Reset" ToolTip="Reset"
                                UseBrowserDefaults="False" Width="118px" meta:resourcekey="ibResetResource2">
                                <ClientSideEvents Click="ibCancel_Click" />
                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                    WidthOfRightEdge="13" />
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
