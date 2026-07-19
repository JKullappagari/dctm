<%@ Page Language="C#" Theme="SkinFile" AutoEventWireup="true" CodeFile="DeAssignRFIDCard.aspx.cs"
    Inherits="DeAssignRFIDCard" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>RFID Tag De-Assignment</title>
    <link href="CSS/text.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <script language="javascript" type="text/javascript">
        //<!--
        function GetPassedValue() {

            var PassedString = window.dialogArguments;
            // alert(PassedString);
            document.forms[0]
            //document.getElementById("lblWorkerID").innerText = PassedString;
            document.forms[0].elements['<%=txtLabel.ClientID%>'].value = window.dialogArguments;
            //document.forms[0].elements["Title"].value=PassedString;

        }
        // -->
    </script>
    <script type="text/javascript">
        //<!--
        // Keep user from entering more than maxLength characters
        function doKeypress(control, maxLength) {
            value = control.value;
            if (maxLength && value.length > maxLength - 1) {
                event.returnValue = false;
                maxLength = parseInt(maxLength);
            }
        }
        // Cancel default behavior
        function doBeforePaste(control, maxLength) {
            if (maxLength) {
                event.returnValue = false;
            }
        }
        // Cancel default behavior and create a new paste routine
        function doPaste(control, maxLength) {
            value = control.value;
            if (maxLength) {
                event.returnValue = false;
                maxLength = parseInt(maxLength);
                var oTR = control.document.selection.createRange();
                var iInsertLength = maxLength - value.length + oTR.text.length;
                var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
                oTR.text = sData;
            }
        }

        // -->
    </script>
</head>
<body>
    <form id="frmReadRFIDCard" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="height: 122px">
            <tr>
                <td valign="top">
                    <table id="Table1" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                            </td>
                            <td style="width: 242px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="height: 36px">
                                <img alt="" src="images/table_left_top.gif" />
                            </td>
                            <td style="background-image: url(images/table_top_middle.gif); height: 36px; width: 242px;">
                                <div class="text" style="text-align: left">
                                    <asp:TextBox ID="txtLabel" runat="server" AutoCompleteType="Disabled" CssClass="viewtabfieldvalue"
                                        MaxLength="150" ReadOnly="True" TabIndex="1" Wrap="False" Width="126px">RFID Tag DeAssignment</asp:TextBox>&nbsp;</div>
                            </td>
                            <td style="height: 36px">
                                <img alt="" src="images/table_top_right.gif" />
                            </td>
                        </tr>
                        <tr>
                            <td style="background-image: url(images/table_left_middle.gif); height: 78px; width: 14px;">
                            </td>
                            <td align="left" valign="top">
                                <igmisc:WebGroupBox ID="WebGroupBox2" runat="server" CssClass="panel" Text="Enter De-Assignment Details"
                                    Width="98%" Height="160px">
                                    <Template>
                                        <table id="Table2" cellpadding="4" width="500" height="150">
                                            <tbody>
                                                <tr style="height: 15px">
                                                    <td class="labelTD" style="width: 97px">
                                                        <asp:Label ID="lblReason" runat="server" CssClass="FieldName" Width="74px">Reason *</asp:Label>
                                                    </td>
                                                    <td class="ControlTD">
                                                        <asp:TextBox ID="txtReason" runat="server" CssClass="FieldValue" Height="100px" TextMode="MultiLine"
                                                            Width="412px" MaxLength="1000" TabIndex="1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason"
                                                            CssClass="ErrValStyle" ErrorMessage="<br>Enter De-Assignment Reason"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr style="height: 15px">
                                                    <td class="labelTD" style="width: 97px">
                                                    </td>
                                                    <td class="ControlTD">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 75px; height: 18px">
                                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" OnClick="ibCreate_Click" TabIndex="2"
                                                                        Text="Save" ToolTip="Save" UseBrowserDefaults="False" SkinID="uwButton" Width="70px"
                                                                        ImageDirectory="">
                                                                        <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                                            WidthOfRightEdge="13" HoverImageUrl="./images/Buttob/blue_button.GIF" ImageUrl="./images/Buttob/blue_button.GIF"
                                                                            PressedImageUrl="./images/Buttob/blue_button.GIF" />
                                                                        <Appearance>
                                                                            <style cursor="Hand" font-names="Arial" font-size="8pt"></style>
                                                                        </Appearance>
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                                <td style="width: 69px; height: 18px">
                                                                    <igtxt:WebImageButton ID="ibReset" runat="server" CausesValidation="False" OnClick="ibReset_Click"
                                                                        Text="Cancel" ToolTip="Cancel" UseBrowserDefaults="False" SkinID="uwButton" TabIndex="3"
                                                                        Width="70px" ImageDirectory="">
                                                                        <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                                            WidthOfRightEdge="13" HoverImageUrl="./images/Buttob/blue_button.GIF" ImageUrl="./images/Buttob/blue_button.GIF"
                                                                            PressedImageUrl="./images/Buttob/blue_button.GIF" />
                                                                        <Appearance>
                                                                            <style cursor="Hand" font-names="Arial" font-size="8pt"></style>
                                                                        </Appearance>
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="MessageStyle" ForeColor="Red"
                                                            Text="Message" Visible="False"></asp:Label>
                                                        <asp:Label ID="lblAssetID" runat="server" CssClass="FieldName" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </Template>
                                </igmisc:WebGroupBox>
                            </td>
                            <td style="background-image: url(images/table_right_middle.gif); height: 78px; width: 6px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 17px; width: 14px;">
                                <img alt="" src="images/table_bottom_left.gif" />
                            </td>
                            <td style="background-image: url(images/table_bottom_middle.gif); width: 242px; height: 17px;">
                            </td>
                            <td style="height: 17px;">
                                <img alt="" src="images/table_bottom_right.gif" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
