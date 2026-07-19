<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" AutoEventWireup="true"
    Inherits="UnBarringAsset" CodeFile="UnBarringAsset.aspx.cs" Title="Remove Asset Restriction" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<asp:Content ID="BarringAssetID" ContentPlaceHolderID="GeneralPopupContentPlaceHolder"
    runat="Server">
    <script type="text/javascript">
        //<!--
        function checkDescLength(sender, args) {
            var re = /^[\w0-9\-\.\:\s]+$/;

            var tDesc = document.getElementById('<%=txtUnBarredReason.ClientID%>').value;
            if (tDesc == '') {
                // do nothing
            }
            else if (tDesc.length > parseInt('<%=txtUnBarredReason.MaxLength.ToString() %>')) {
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
    <igmisc:WebGroupBox ID="wgbBarredDetails" runat="server" CssClass="panel" Text="Restriction Details"
        Height="100%" Width="98%">
        <Template>
            <table id="Table5" align="left" cellpadding="4" width="600">
                <tr>
                    <td class="labelTD" style="width: 115px; height: 3px">
                        <asp:Label ID="lblBarredPeriod" runat="server" Width="117px" CssClass="FieldName">Restriction Period</asp:Label>
                    </td>
                    <td class="ControlTD" style="height: 3px; width: 706px;">
                        <asp:TextBox ID="txtBarredFromDate" runat="server" CssClass="viewfieldvalue" ReadOnly="True"
                            Width="68px"></asp:TextBox>
                        -
                        <asp:TextBox ID="txtBarredToDate" runat="server" CssClass="viewfieldvalue" ReadOnly="True"
                            Width="102px"></asp:TextBox>
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="labelTD" style="width: 115px; height: 3px">
                        <asp:Label ID="lblBarredBy" runat="server" CssClass="FieldName" Width="114px">Restricted By</asp:Label>
                    </td>
                    <td class="ControlTD" style="width: 706px; height: 3px">
                        <asp:TextBox ID="txtBarredBy" runat="server" CssClass="viewfieldvalue" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="labelTD" style="width: 115px; height: 3px">
                        <asp:Label ID="lblBarredDate" runat="server" CssClass="FieldName" Width="116px">Transaction Date</asp:Label>
                    </td>
                    <td class="ControlTD" style="width: 706px; height: 3px">
                        <asp:TextBox ID="txtBarredDate" runat="server" CssClass="viewfieldvalue" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="labelTD" style="width: 115px; height: 3px">
                        <asp:Label ID="lblBarredReason" runat="server" CssClass="FieldName" Width="92px">Reason</asp:Label>
                    </td>
                    <td class="ControlTD" style="height: 3px; width: 706px;">
                        <asp:TextBox ID="txtBarredReason" runat="server" CssClass="FieldValue" Height="68px" ReadOnly="true"
                             TextMode="MultiLine" Width="350px" MaxLength="150"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </Template>
    </igmisc:WebGroupBox>
    <igmisc:WebGroupBox ID="wgbUnBarringDetails" runat="server" CssClass="panel" Text="Enter De-Restriction Details"
        TitleAlignment="Left" Width="98%" Height="100%">
        <Template>
            <table id="Table2" cellpadding="4" align="left" width="600">
                <tbody>
                    <tr style="height: 15px">
                        <td style="width: 30px" class="labelTD" valign="top">
                            <asp:Label ID="lblReason" runat="server" Width="112px" CssClass="FieldName">Reason *</asp:Label>
                        </td>
                        <td class="ControlTD" valign="top">
                            <asp:TextBox ID="txtUnBarredReason" runat="server" Width="350px" CssClass="FieldValue"
                                Height="68px" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtUnBarredReason"
                                CssClass="ErrValStyle" ErrorMessage="<br>Enter Reason" EnableViewState="False"></asp:RequiredFieldValidator>
                            <br />
                            <asp:CustomValidator ID="cvReason" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                Width="250px" Display="Dynamic" ControlToValidate="txtUnBarredReason" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr style="height: 15px">
                        <td style="width: 30px; height: 15px;" class="labelTD">
                            <asp:Label ID="lblAssetID" runat="server" CssClass="FieldName" Visible="False"></asp:Label>
                        </td>
                        <td class="ControlTD" style="height: 15px" align="right">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 75px; height: 18px">
                                        <igtxt:WebImageButton ID="ibCreate" runat="server" OnClick="ibCreate_Click" TabIndex="8"
                                            Text="Save" ToolTip="Save" UseBrowserDefaults="False" SkinID="uwButton" Width="70px"
                                            ImageDirectory="">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="width: 69px; height: 18px">
                                        <igtxt:WebImageButton ID="ibReset" runat="server" CausesValidation="False" OnClick="ibReset_Click"
                                            Text="Cancel" ToolTip="Cancel" UseBrowserDefaults="False" SkinID="uwButton" Width="70px"
                                            ImageDirectory="">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <Appearance>
                                                <style cursor="Hand" font-names="Arial" font-size="8pt"></style>
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                            </table>
                            &nbsp;&nbsp;
                            <asp:Label ID="lblMessage" runat="server" CssClass="MessageStyle" ForeColor="Red"
                                Text="Message" Visible="False"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </Template>
    </igmisc:WebGroupBox>
</asp:Content>
