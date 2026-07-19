<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" AutoEventWireup="true"
    Inherits="DeWriteoffAsset" CodeFile="DeWriteoffAsset.aspx.cs" Title="Reinstate Written Off Asset"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<asp:Content ID="ContentMainPage" ContentPlaceHolderID="GeneralPopupContentPlaceHolder"
    runat="Server">
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
        //-->
    </script>
    <div align="center">
        <igmisc:WebGroupBox ID="wgbBarredDetails" runat="server" CssClass="panel" Text="Writeoff Details"
            Height="100%" Width="98%" meta:resourcekey="wgbBarredDetailsResource1" StyleSetName=""
            TitleAlignment="Left">
            <Template>
                <table id="Table5" align="center" cellpadding="4" width="600">
                    <tr>
                        <td class="labelTD" style="width: 68px">
                            <asp:Label ID="lblBlacklistedDate" runat="server" CssClass="FieldName" Width="112px"
                                meta:resourcekey="lblBlacklistedDateResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="width: 335px;">
                            <asp:TextBox ID="txtRestrictDate" runat="server" CssClass="viewfieldvalue" ReadOnly="True"
                                Width="102px" meta:resourcekey="txtRestrictDateResource1"></asp:TextBox>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 68px">
                            <asp:Label ID="lblBlacklistedBy" runat="server" CssClass="FieldName" Width="109px"
                                meta:resourcekey="lblBlacklistedByResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="width: 335px;">
                            <asp:TextBox ID="txtRestrictedBy" runat="server" CssClass="viewfieldvalue" ReadOnly="True"
                                Width="121px" meta:resourcekey="txtRestrictedByResource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="height: 84px; width: 68px;">
                            <asp:Label ID="lblBlacklistedReason" runat="server" CssClass="FieldName" meta:resourcekey="lblBlacklistedReasonResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="width: 335px; height: 84px;">
                            <asp:TextBox ID="txtRestrictReason" runat="server" CssClass="FieldValue" Height="80px"
                                ReadOnly="True" TextMode="MultiLine" Width="412px" MaxLength="255" meta:resourcekey="txtRestrictReasonResource1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </Template>
        </igmisc:WebGroupBox>
        <igmisc:WebGroupBox ID="wgbUnBarringDetails" runat="server" CssClass="panel" Text="Enter Reinstate Details"
            TitleAlignment="Left" Width="98%" Height="100%" meta:resourcekey="wgbUnBarringDetailsResource1"
            StyleSetName="">
            <Template>
                <table id="Table2" cellpadding="4" align="center" width="600">
                    <tbody>
                        <tr style="height: 15px">
                            <td class="labelTD" valign="top" style="width: 129px; height: 15px">
                                <asp:Label ID="lblReason" runat="server" CssClass="FieldName" Width="118px" meta:resourcekey="lblReasonResource1"></asp:Label>
                            </td>
                            <td class="ControlTD" valign="top">
                                <asp:TextBox ID="txtReason" runat="server" Width="412px" CssClass="FieldValue" Height="80px"
                                    TextMode="MultiLine" MaxLength="255" meta:resourcekey="txtReasonResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason"
                                    CssClass="ErrValStyle" EnableViewState="False" meta:resourcekey="rfvReasonResource1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                            <td class="labelTD" style="width: 129px">
                                <asp:Label ID="lblAssetID" runat="server" CssClass="FieldName" Visible="False" meta:resourcekey="lblAssetIDResource1"></asp:Label>
                            </td>
                            <td class="ControlTD" align="right">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 75px; height: 18px">
                                            <igtxt:WebImageButton ID="ibCreate" runat="server" OnClick="ibCreate_Click" TabIndex="8"
                                                UseBrowserDefaults="False" SkinID="uwButton" Width="70px" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                    WidthOfRightEdge="13" />
                                                <Appearance>
                                                    <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                    </ButtonStyle>
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td style="width: 69px; height: 18px">
                                            <igtxt:WebImageButton ID="ibReset" runat="server" CausesValidation="False" OnClick="ibReset_Click"
                                                UseBrowserDefaults="False" SkinID="uwButton" Width="70px" ImageDirectory="" meta:resourcekey="ibResetResource1">
                                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                    WidthOfRightEdge="13" />
                                                <Appearance>
                                                    <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                    </ButtonStyle>
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblMessage" runat="server" CssClass="MessageStyle" ForeColor="Red"
                                    Visible="False" meta:resourcekey="lblMessageResource1"></asp:Label>
                        </tr>
                    </tbody>
                </table>
            </Template>
        </igmisc:WebGroupBox>
</asp:Content>
