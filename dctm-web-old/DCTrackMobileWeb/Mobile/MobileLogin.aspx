<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/Mobile/MobileLogin.aspx.cs"
    Inherits="ASPX_Mobile_Login" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
<head>
    <meta name="viewport" content="width=pageWidth,initial-scale=1.0" />
    <title>DCTrackMobileWeb Login</title>
    <link href="../css/text.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">

        function window_onload() {
            if (window.opener != null || window.opener != 'undefined') {
                window.opener.location.reload();
                window.close();
            }
            //onload="return window_onload()"
        }
    </script>
    <style type="text/css">
        .style7
        {
            height: 52px;
        }
        .style8
        {
            height: 31px;
        }
        </style>
</head>
<body style="margin-left:0;margin-right:0;margin-top:0;margin-bottom:0">
    <form id="frmLogin" runat="server">
    <div align="left">
        <table border="0" cellspacing="1" cellpadding="0" 
            style="background-color:Black; width:100%;height:20%">
            <tr>
                <td valign="top" class="style7" >
                    <table>
                        <tr bgcolor="#000000" >
                            <td style="font-family: Arial, Helvetica, sans-serif; font-size:x-large; font-weight: lighter;
                                font-style: italic; color: #FFFFFF;" align="left" >
                                &nbsp;<img alt="" 
                                    src="../images/Infrastructure_white.png" style="width: 34px; height: 34px;
                                    vertical-align: middle;" />
                                DCTrackMobileWeb
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" style="background-color: #333333; border: 1px solid #dfdfdf" 
                    class="style8">
                    <table border="0" cellspacing="0" cellpadding="0" style="margin-top: 0px">
                        <tr>
                            <td colspan="2" valign="top" >
                                <asp:Label ForeColor="White" CssClass="displayText" runat="server" ID="lblUser" meta:resourcekey="lblUserResource1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" >
                                <asp:TextBox ID="txtUser" runat="server" TabIndex="1" meta:resourcekey="txtUserResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqtxtUserVal" runat="server" ControlToValidate="txtUser"
                                    CssClass="displayText" Display="Dynamic" Font-Bold="True" meta:resourcekey="reqtxtUserValResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="10" colspan="2" valign="top" >
                                <asp:Label ForeColor="White" runat="server" CssClass="displayText" ID="lblPassword"
                                    meta:resourcekey="lblPasswordResource1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" >
                                <asp:TextBox ID="txtPassword" runat="server" TabIndex="2" TextMode="Password" meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqtxtPasswrodVal" runat="server" ControlToValidate="txtPassword"
                                    CssClass="displayText" Display="Dynamic" Font-Bold="True" meta:resourcekey="reqtxtPasswrodValResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" >
                                <asp:Button ID="btnLogin" runat="server" Style="cursor: hand;" OnClick="btnLogin_Click"
                                    TabIndex="3" CssClass="displayText" ToolTip="Login" BackColor="Black" BorderColor="White"
                                    BorderStyle="Solid" BorderWidth="1px" ForeColor="White" meta:resourcekey="btnLoginResource1" />
                                &nbsp;
                                <asp:Button ID="btnReset" runat="server" Style="cursor: hand;" CausesValidation="False"
                                    OnClick="btnReset_Click" CssClass="displayText" ToolTip="Reset" BackColor="Black"
                                    BorderColor="White" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" meta:resourcekey="btnResetResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right" >
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" >
                                <asp:Label ID="lblMsg" runat="server" CssClass="displayText" Font-Bold="True" ForeColor="Red"
                                    meta:resourcekey="lblMsgResource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="25" valign="top" align="left">
                    <div style="color: Gray">
                        DCTrackMobile,v5.4.
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>