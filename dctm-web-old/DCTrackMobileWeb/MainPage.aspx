<%@ Page Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="MainPage" Title="Welcome to DCTrackMobileWeb" %>
<asp:Content id="DefaultMainPage" contentplaceholderid="Master_ContentPlaceHolder" runat="Server">
<!-- //HP: Added script to handle display of password expiry status and change password option. -->
<script language="javascript"  type="text/javascript">
//<!--
function openWin(id, action)
{
var winSettings;
winSettings = "scroll:auto; dialogWidth:570px; dialogHeight:296px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";   
switch (action)
    {
        case "PasswordChange":
            window.showModalDialog("ChangePassword.aspx?ID=" + id, 'Popupwindow', winSettings);
            break;
        case "PasswordHelp":
            window.showModalDialog("PasswordHint.aspx", 'Popupwindow', winSettings);
            break;
    }
}

// -->
</script>

<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" >
  <tr>
    <td height="350" colspan="3" valign="top" align="right"><table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" style="width: 109px" class="displayText">
        <nobr>
            <!-- //HP: Added controls to handle display of password expiry status and change password option. -->
            &nbsp;<asp:Label ID="lblPasswordExpiry" runat="server"></asp:Label>
            <asp:HyperLink ID="hypChangePassword" runat="server" OnInit="hypChangePassword_Init" style="cursor:hand;text-decoration:underline;color:Blue;"></asp:HyperLink>
            <asp:HyperLink ID="hypPasswordHelp" runat="server" style="cursor:hand;text-decoration:underline;color:Blue;" OnInit="hypPasswordHelp_Init" Visible="false"></asp:HyperLink></nobr>
        
        </td>
        <td width="48%" height="30" valign="top">&nbsp;</td>
        <td width="3%" valign="top">&nbsp;</td>
      </tr>
      <tr>
        <td valign="top" align="center" class="Heading" colspan="2"></td>
        <td valign="top">&nbsp;</td>
      </tr>
    <tr>
        <td style="width: 109px; height: 20px;" valign="top">
        </td>
        <td class="Heading" valign="top" 
            style="height: 20px; font-weight: bold; font-size: x-large;">
            &nbsp;Welcome to DCTrackMobileWeb</td>
        <td valign="top" style="height: 20px">
        </td>
    </tr>
      <tr>
        <td valign="top" align="left" class="displayText" colspan="2"> 
        <table width="50%">
            <tr>
                <td style="text-align: justify;" >
                    DCTrackMobileWeb is a RFID/Barcode based&nbsp; Asset Tracking application that enables you to 
                    control and monitor Asset Movement with the help of DCTrackMobile, a Windows Mobile based application. </td>
            </tr>
        </table>
    </td>
        <td valign="top">&nbsp;</td>
      </tr>      
    <tr>
        <td align="left" class="displayText" colspan="2" valign="top">
        </td>
        <td valign="top">
        </td>
    </tr>
    </table></td>
  </tr>
    <tr>
        <td colspan="3" height="20">&nbsp;
        </td>
    </tr>
</table>
</asp:Content>
