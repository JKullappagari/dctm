<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" AutoEventWireup="true"
    Inherits="MusterAsset" CodeFile="MusterAsset.aspx.cs" Title="Decommission Asset"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="MusterAssetID" ContentPlaceHolderID="GeneralPopupContentPlaceHolder"
    runat="Server">
    <script type="text/javascript">
        //<!--
        function checkDescLength(sender, args) {
            var re = /^[\w0-9\-\.\:\s]+$/;

            var tDesc = document.getElementById('<%=txtMusterReason.ClientID%>').value;
            if (tDesc == '') {
                // do nothing
            }
            else if (tDesc.length > parseInt('<%=txtMusterReason.MaxLength.ToString() %>')) {
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
        //-->
    </script>
    <igmisc:WebGroupBox ID="wgbBarredDetails" CssClass="panel" runat="server" Text="Enter Decommissioning  Details"
        Width="98%" BorderStyle="None" Font-Names="Tahoma" Font-Size="9pt" meta:resourcekey="wgbBarredDetailsResource1"
        StyleSetName="" TitleAlignment="Left">
        <Template>
            <table id="Table2" cellpadding="4" align="left" width="600">
                <tbody>
                    <tr>
                        <td class="labelTD">
                            <asp:Label ID="lblPeriod" runat="server" Width="150px" CssClass="FieldName" meta:resourcekey="lblPeriodResource1"></asp:Label>
                        </td>
                        <td class="ControlTD">
                            <table cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <ig:WebDatePicker ID="wdcExpiryDate" runat="server" Width="114px" nulldatelabel="-- Select --"
                                                SkinID="wdcDate" nullvaluerepresentation="Null" Font-Names="Tahoma" Font-Size="9pt"
                                                meta:resourcekey="wdcExpiryDateResource1">
                                            </ig:WebDatePicker>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="wdcExpiryDate"
                                                Display="Dynamic" CssClass="ErrValStyle" meta:resourcekey="rfvFromDateResource1"></asp:RequiredFieldValidator>
                                        </td>
                                        <td valign="top">
                                            &nbsp;
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:RangeValidator ID="rvBarredFromDate" runat="server" ControlToValidate="wdcExpiryDate"
                                CssClass="ErrValStyle" Display="Dynamic" Type="Date" Width="304px" meta:resourcekey="rvBarredFromDateResource1"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" valign="top">
                            <asp:Label ID="Label1" runat="server" CssClass="FieldName" Width="103px" meta:resourcekey="Label1Resource1"></asp:Label>
                        </td>
                        <td class="ControlTD" valign="top">
                            <asp:DropDownList ID="ddlMusterReason" runat="server" CssClass="dropdownText" meta:resourcekey="ddlMusterReasonResource1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="ddlMusterReason"
                                CssClass="ErrValStyle" InitialValue="0" ErrorMessage="Enter Reason" meta:resourcekey="rfvReasonResource1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" valign="top">
                            <asp:Label ID="lblReason" runat="server" Width="150px" CssClass="FieldName" meta:resourcekey="lblReasonResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" valign="top">
                            <asp:TextBox ID="txtMusterReason" runat="server" Width="412px" CssClass="FieldValue"
                                Height="95px" TextMode="MultiLine" MaxLength="100" meta:resourcekey="txtMusterReasonResource1"></asp:TextBox>
                            <br />
                            <asp:CustomValidator ID="cvMusterReason" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                Display="Dynamic" ControlToValidate="txtMusterReason" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="height: 20px">
                            <asp:Label ID="lblAssetID" runat="server" CssClass="FieldName" Visible="False" meta:resourcekey="lblAssetIDResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="height: 20px">
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
                                            <DisabledAppearance>
                                                <ButtonStyle Font-Bold="False">
                                                </ButtonStyle>
                                            </DisabledAppearance>
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
                                Text="Message" Visible="False" meta:resourcekey="lblMessageResource1"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </Template>
    </igmisc:WebGroupBox>
</asp:Content>
