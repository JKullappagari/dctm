<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" AutoEventWireup="true"
    Inherits="DeRestrictAsset" CodeFile="DeRestrictAsset.aspx.cs" Title="Remove Restriction"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<asp:Content ID="ContentMainPage" ContentPlaceHolderID="GeneralPopupContentPlaceHolder"
    runat="Server">
    <script type="text/javascript">

    //<!--
        function checkDescLength(sender, args) {
            var re = /^[\w0-9\-\.\:\s]+$/;

            var tDesc = document.getElementById('<%=txtReason.ClientID%>').value;
            if (tDesc == '') {
                // do nothing
            }
            else if (tDesc.length > parseInt('<%=txtReason.MaxLength.ToString() %>')) {
                args.IsValid = false;
                return;
            }
            else {
                if (re.test(tDesc)) {
                    args.IsValid = true;
                    return;
                }
                else {
                    args.IsValid = false;
                    return;
                }

            }

        }
        // -->
    </script>
    <div align="left">
        <igmisc:WebGroupBox ID="wgbBarredDetails" runat="server" CssClass="panel" Text="Restriction Details"
            Height="100%" Width="98%" meta:resourcekey="wgbBarredDetailsResource1" StyleSetName=""
            TitleAlignment="Left">
            <Template>
                <table id="Table5" align="left" cellpadding="4">
                    <tr>
                        <td class="labelTD" style="width: 100px">
                            <asp:Label ID="lblBlacklistedDate" runat="server" CssClass="FieldName" Width="115px"
                                meta:resourcekey="lblBlacklistedDateResource1"></asp:Label>
                        </td>
                        <td class="ControlTD">
                            <asp:TextBox ID="txtRestrictDate" runat="server" CssClass="viewfieldvalue" ReadOnly="True"
                                Width="102px" meta:resourcekey="txtRestrictDateResource1"></asp:TextBox>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 100px">
                            <asp:Label ID="lblBlacklistedBy" runat="server" CssClass="FieldName" Width="115px"
                                meta:resourcekey="lblBlacklistedByResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="width: 335px;">
                            <asp:TextBox ID="txtRestrictedBy" runat="server" CssClass="viewfieldvalue" ReadOnly="True"
                                Width="121px" meta:resourcekey="txtRestrictedByResource1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 100px">
                            <asp:Label ID="lblBlacklistedReason" runat="server" Width="115px" CssClass="FieldName"
                                meta:resourcekey="lblBlacklistedReasonResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="width: 300px; height: 84px;">
                            <asp:TextBox ID="txtRestrictReason" runat="server" CssClass="FieldValue" Height="80px"
                                ReadOnly="True" TextMode="MultiLine" Width="300px" MaxLength="255" meta:resourcekey="txtRestrictReasonResource1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </Template>
        </igmisc:WebGroupBox>
        <igmisc:WebGroupBox ID="wgbUnBarringDetails" runat="server" CssClass="panel" Text="Enter De-Restriction Details"
            TitleAlignment="Left" Width="98%" Height="100%" meta:resourcekey="wgbUnBarringDetailsResource1"
            StyleSetName="">
            <Template>
                <table id="Table2" cellpadding="4" align="left">
                    <tbody>
                        <tr>
                            <td class="labelTD" style="width: 100px">
                                <asp:Label ID="lblReason" runat="server" CssClass="FieldName" Width="115px" meta:resourcekey="lblReasonResource1"></asp:Label>
                            </td>
                            <td class="ControlTD" valign="top">
                                <asp:TextBox ID="txtReason" runat="server" Width="300px" CssClass="FieldValue" Height="80px"
                                    TextMode="MultiLine" MaxLength="150" meta:resourcekey="txtReasonResource1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason"
                                    CssClass="ErrValStyle" EnableViewState="False" meta:resourcekey="rfvReasonResource1"></asp:RequiredFieldValidator>
                                <br />
                                <asp:CustomValidator ID="cvReason" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                    Width="250px" Display="Dynamic" ControlToValidate="txtReason" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="labelTD">
                                <asp:Label ID="lblAssetID" runat="server" Width="115px" CssClass="FieldName" Visible="False"
                                    meta:resourcekey="lblAssetIDResource1"></asp:Label>
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
    </div>
</asp:Content>
