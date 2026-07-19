<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserNotFound.aspx.cs" Inherits="UserNotFound" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>User Not Found Page</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="CSS/text.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/JScript.js"></script>     
<script language="javascript" src="Scripts/AJAX.js"></script>
</head>
<body>
    <form id="frmUser" runat="server">
    <div>
     <table style="width : 1000px" border="0" cellspacing="0" cellpadding="0">
            <tr bgcolor="#4577AD">                 
                <td style="height : 59px; background-repeat :no-repeat;background-image: url(images/header.gif);">
                    <table border="0" cellpadding="0" cellspacing="0" style="width : 100%;">
                    
                        <tr>
                       
                            <td style="width : 60%; height: 19px;">                                
                            </td>
                            <td align="center" style="width : 50%; height: 19px;">
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                             </td>     
                                                 
                        </tr>
                     
                        
                        <tr>
                        <td style="height: 80%"></td>
                        <td align="right" style="width : 50%;">
                            &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr bgcolor="#4577AD"> 
                <td align="left" valign="top">                    
                     <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            
                        </td>
                     </tr>
                   </table>          
                </td>
            </tr>            
            <tr> 
                <td align="Left" style="height: 30px;">
                    <table cellpadding=0 cellspacing=0 width="1000" height="100%" background="images/submenubg1.gif">
                        <tr>
                            <td align=left valign=middle style="width: 48%">&nbsp;<asp:Label id="lblHeader" runat="server" CssClass="label" Width="429px"></asp:Label></td>
                            <td align=right valign=top style="width: 20%">
                                <asp:Image ID="imgUser" runat="server" ImageUrl="~/images/usericon.gif" ToolTip="User" />
                            </td>
                            <td Class="label" align="left" valign=middle style="width: 30%"><asp:Label id="lblUser" runat="server"  CssClass="Pagetext2"></asp:Label>&nbsp;<br /><font size="1"></font></td>
                            
                        </tr>
                    </table>                    
				    
                </td>
            </tr>
            <tr height="100%"> 
                <td valign="top" style="background-color:#FFFFFF;">
                    <div id="a" style="height:500px; position:relative; overflow:auto; width:100%;">
                    <table align="center" cellpadding="1" cellspacing="1" border="0" width="100%" >
                        <tr>
                            <td align="center" height="50" valign="middle">
                            </td>
                        </tr>
                    <tr>
                        <td valign="middle" align="center">  
                            <asp:Label ID="lblMsg" runat="server" CssClass="displayText" Font-Bold="True" ForeColor="Red"></asp:Label>&nbsp;</td>
                    </tr>
                        <tr>
                            <td align="center" valign="middle" headers="50">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" height="50" valign="middle">
                            </td>
                        </tr>
                    
                        <tr>
                            <td align="center" height="50" valign="middle">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="height: 41px" valign="middle">
                                <asp:Button ID="btnClose" runat="server" CssClass="displayText" style="cursor:hand;" OnClientClick="javascript:window.close();"
                                    Text="Close" /></td>
                        </tr>
                    
                    </table></div>
                </td>
            </tr>
            <tr>
                <td>
                        <table width="100%" height="100%" cellpadding="0" cellspacing="0" >
                                        <tr><td class="displayText" align="center">Copy right ©2006 - Keppel Offshore & Marine</td></tr>
                                    </table>
                </td>
            </tr>
            </table>
    </div>
    </form>
</body>
</html>
