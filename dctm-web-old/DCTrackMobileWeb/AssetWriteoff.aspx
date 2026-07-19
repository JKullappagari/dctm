<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" AutoEventWireup="true"
    Inherits="AssetWriteoff" CodeFile="AssetWriteoff.aspx.cs" Title="Write Off Asset"
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
    <igmisc:WebGroupBox ID="WebGroupBox2" runat="server" CssClass="panel" Text="Enter Asset Write Off Details"
        Width="97%" Height="110%" meta:resourcekey="WebGroupBox2Resource1" StyleSetName=""
        TitleAlignment="Left">
        <Template>
            <table id="Table2" cellpadding="4" width="500" height="150">
                <tbody>
                    <tr>
                        <td class="labelTD">
                            <asp:Label ID="Label1" runat="server" CssClass="FieldName" meta:resourcekey="Label1Resource1"
                                Width="150px"></asp:Label>
                        </td>
                        <td class="ControlTD">
                            <asp:DropDownList ID="ddlMusterReason" runat="server" CssClass="dropdownText" meta:resourcekey="ddlMusterReasonResource1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvReason1" runat="server" ControlToValidate="ddlMusterReason"
                                InitialValue="0" Display="Dynamic" CssClass="ErrValStyle" meta:resourcekey="rfvReason1Resource1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD">
                            <asp:Label ID="lblReason" runat="server" CssClass="FieldName" meta:resourcekey="lblReasonResource1"
                                Width="150px"></asp:Label>
                        </td>
                        <td class="ControlTD">
                            <asp:TextBox ID="txtReason" runat="server" CssClass="FieldValue" Height="100px" MaxLength="100"
                                meta:resourcekey="txtReasonResource1" TextMode="MultiLine" Width="412px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason"
                                Display="Dynamic" CssClass="ErrValStyle" meta:resourcekey="rfvReasonResource1"></asp:RequiredFieldValidator>
                            <br />
                            <asp:CustomValidator ID="cvMusterReason" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                Display="Dynamic" ControlToValidate="txtReason" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr style="height: 15px">
                        <td class="labelTD" style="width: 97px">
                        </td>
                        <td class="ControlTD">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 75px; height: 18px">
                                        <igtxt:WebImageButton ID="ibCreate" runat="server" ImageDirectory="" meta:resourcekey="ibCreateResource1"
                                            OnClick="ibCreate_Click" SkinID="uwButton" TabIndex="8" UseBrowserDefaults="False"
                                            Width="70px">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <Appearance>
                                                <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                </ButtonStyle>
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="width: 69px; height: 18px">
                                        <igtxt:WebImageButton ID="ibReset" runat="server" CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibResetResource1" OnClick="ibReset_Click" SkinID="uwButton"
                                            UseBrowserDefaults="False" Width="70px">
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
                                meta:resourcekey="lblMessageResource1" Visible="False"></asp:Label>
                            <asp:Label ID="lblAssetID" runat="server" CssClass="FieldName" meta:resourcekey="lblAssetIDResource1"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </Template>
    </igmisc:WebGroupBox>
</asp:Content>
