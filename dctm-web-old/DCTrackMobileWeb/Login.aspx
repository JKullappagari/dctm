<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="ASPX_Login" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
<head>
    <title>DCTrackMobileWeb Login</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/text.css" rel="stylesheet" type="text/css" />
     <link rel="shortcut icon" type="image/x-icon" href="images/favicon.ico" />
    <script language="javascript" type="text/javascript">
    //<!--
        function window_onload() {
            if (window.opener != null || window.opener != 'undefined') {
                window.opener.location.reload();
                window.close();
            }
            //onload="return window_onload()"
        }
        //-->
    </script>

    <style type="text/css">
        .style2
        {
            width: 279px;
        }
    </style>
</head>
<body bgcolor="#1e1e1e" >
    <form id="frmLogin" runat="server">
    <div align="center">
        <table width="900" border="0" cellspacing="1" cellpadding="0">
            <tr>
                <td colspan="2" valign="top" height="70px">
                    <table width="900">
                        <tr>
                            <td width="10%" align="right">
                            <img src="images/Infrastructure_white.png" alt=""  width="50px" height="50px" />
                            </td>
                            <td width="70%" align="left" valign="middle" height="70px"  style="font-family: Arial, Helvetica, sans-serif;
                                font-size: x-large; font-weight: lighter; color: #FFFFFF;">
                                DCTrackMobileWeb
                            </td>
                            <td width="20%">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
            <td  style="height:5px;">
            </td>
            </tr>
            <tr>
                <td valign="top" 
                    style="border-width: 1px; background-color: #181818; border-top-style: solid; border-top-color: #FFFFFF; border-right-style: solid; border-right-color: #FFFFFF; border-bottom-style: solid; border-bottom-color: #FFFFFF; border-left-style: solid; border-left-color: #FFFFFF; position: inherit;" 
                    class="style2">
                    <img style="background-position: center; width: 250px; height: 250px; margin-top: 1px;" alt="Login"
                        src="images/login.png" />
                </td>
                <td width="620" valign="middle" style="background-color: #333333; border: 1px solid #dfdfdf">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top: 0px">
                        <tr>
                            <td colspan="3" valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="19%" valign="top">
                                &nbsp;
                            </td>
                            <td width="17%" valign="top">
                                <asp:Label ForeColor="White" CssClass="displayText" runat="server" ID="lblUser" 
                                    Width="98px" meta:resourcekey="lblUserResource1"></asp:Label>
                            </td>
                            <td width="64%" valign="top">
                                <asp:TextBox ID="txtUser" runat="server" TabIndex="1" 
                                    meta:resourcekey="txtUserResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqtxtUserVal" runat="server" 
                                    ControlToValidate="txtUser" CssClass="displayText" Display="Dynamic" 
                                    Font-Bold="True" meta:resourcekey="reqtxtUserValResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="10" colspan="3" valign="top">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="height: 24px">
                                &nbsp;
                            </td>
                            <td valign="top" style="height: 24px">
                                <asp:Label ForeColor="White" runat="server" CssClass="displayText" ID="lblPassword"
                                    Width="98px" meta:resourcekey="lblPasswordResource1"></asp:Label>
                            </td>
                            <td valign="top" style="height: 24px">
                                <asp:TextBox ID="txtPassword" runat="server" TabIndex="2" TextMode="Password" 
                                    meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqtxtPasswrodVal" runat="server" 
                                    ControlToValidate="txtPassword" CssClass="displayText" Display="Dynamic" 
                                    Font-Bold="True" meta:resourcekey="reqtxtPasswrodValResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                &nbsp;
                            </td>
                            <td valign="top" class="text">
                                &nbsp;
                            </td>
                            <td valign="top">
                                <asp:Button ID="btnLogin" runat="server" Style="cursor: hand;" OnClick="btnLogin_Click"
                                     TabIndex="3" CssClass="displayText" ToolTip="Login" BackColor="Black"
                                    BorderColor="White" BorderStyle="Solid" BorderWidth="1px" 
                                    ForeColor="White" meta:resourcekey="btnLoginResource1" />
                                &nbsp;
                                <asp:Button ID="btnReset" runat="server" Style="cursor: hand;" CausesValidation="False"
                                    OnClick="btnReset_Click" CssClass="displayText" ToolTip="Reset" BackColor="Black"
                                    BorderColor="White" BorderStyle="Solid" BorderWidth="1px" 
                                    ForeColor="White" meta:resourcekey="btnResetResource1" />
                            </td>
                            <td valign="top">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 95px">
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblMsg" runat="server" CssClass="displayText" Width="499px" Font-Bold="True" 
                                    ForeColor="Red" meta:resourcekey="lblMsgResource1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr >
                <td height="33" colspan="2" valign="top" align="center">
                    &nbsp;
                </td>
            </tr>
            <tr >
                <td height="33" colspan="2" valign="top" align="center">
                    <div style="color: Gray">
                        &nbsp;   DCTrackMobile,v5.4.                   </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
