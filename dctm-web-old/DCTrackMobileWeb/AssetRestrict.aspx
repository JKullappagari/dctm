<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" AutoEventWireup="true"
    Inherits="AssetRestrict" CodeFile="AssetRestrict.aspx.cs" Title="Restrict Asset for Indefinte Period"
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
        //-->
    </script>
    <igmisc:WebGroupBox ID="WebGroupBox2" runat="server" CssClass="panel" Text="Enter Permanent Restriction Details"
        Width="97%" Height="110%" meta:resourcekey="WebGroupBox2Resource1" StyleSetName=""
        TitleAlignment="Left">
        <Template>
            <table id="Table2" cellpadding="4" width="500" >
                <tbody>
                    <tr style="height: 15px">
                        <td class="labelTD" style="width: 97px">
                            <asp:Label ID="lblReason" runat="server" CssClass="FieldName" Width="75px" meta:resourcekey="lblReasonResource1"></asp:Label>
                        </td>
                        <td class="ControlTD">
                            <asp:TextBox ID="txtReason" runat="server" Width="235px" CssClass="FieldValue" Height="100px"
                                TextMode="MultiLine" MaxLength="150" meta:resourcekey="txtReasonResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason"
                                CssClass="ErrValStyle" meta:resourcekey="rfvReasonResource1"></asp:RequiredFieldValidator>
                            <br />
                            <asp:CustomValidator ID="cvReason" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                Width="250px" Display="Dynamic" ControlToValidate="txtReason" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr style="height: 15px">
                        <td class="labelTD" style="width: 97px">
                        </td>
                        <td class="ControlTD">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 75px; height: 18px">
                                        <igtxt:WebImageButton ID="ibCreate" runat="server" OnClick="ibCreate_Click" TabIndex="8"
                                            UseBrowserDefaults="False" SkinID="uwButton" Width="70px" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="width: 69px; height: 18px">
                                        <igtxt:WebImageButton ID="ibReset" runat="server" CausesValidation="False" OnClick="ibReset_Click"
                                            UseBrowserDefaults="False" SkinID="uwButton" Width="70px" ImageDirectory="" meta:resourcekey="ibResetResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblMessage" runat="server" CssClass="MessageStyle" ForeColor="Red"
                                Visible="False" meta:resourcekey="lblMessageResource1"></asp:Label>
                            <asp:Label ID="lblAssetID" runat="server" CssClass="FieldName" Visible="False" meta:resourcekey="lblAssetIDResource1"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </Template>
    </igmisc:WebGroupBox>
</asp:Content>
