<%@ Page Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" AutoEventWireup="true"
    Inherits="BarringAsset" CodeFile="BarringAsset.aspx.cs" Title="Restrict Asset For a Period" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="BarringAssetID" ContentPlaceHolderID="GeneralPopupContentPlaceHolder"
    runat="Server">
    <script type="text/javascript">
    //<!--
        function checkDescLength(sender, args) {
            var re = /^[\w0-9\-\.\:\s]+$/;

            var tDesc = document.getElementById('<%=txtBarredReason.ClientID%>').value;
            if (tDesc == '') {
                // do nothing
            }
            else if (tDesc.length > parseInt('<%=txtBarredReason.MaxLength.ToString() %>')) {
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

            //-->
        }
    </script>
    <igmisc:WebGroupBox ID="wgbBarredDetails" CssClass="panel" runat="server" Text="Enter Asset Restriction Details"
        Width="98%">
        <Template>
            <table id="Table2" cellpadding="4" align="left" width="600" 
                style="width: 664px">
                <tbody>
                    <tr>
                        <td class="labelTD" style="width: 117px">
                            <asp:Label ID="lblPeriod" runat="server" Width="150px" CssClass="FieldName">Restriction Period*</asp:Label>
                        </td>
                        <td class="ControlTD">
                            <table cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <ig:WebDatePicker ID="wdcBarredFromDate" runat="server" Width="114px" NullDateLabel="-Select-"
                                                NullValueRepresentation="Null" CssClass="dropdownText">
                                            </ig:WebDatePicker>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="wdcBarredFromDate"
                                                Display="Dynamic" CssClass="ErrValStyle" ErrorMessage="<br>Select From Date"></asp:RequiredFieldValidator>
                                        </td>
                                        <td valign="top">
                                            -
                                        </td>
                                        <td valign="top">
                                            <ig:WebDatePicker ID="wdcBarredToDate" runat="server" Width="104px" NullDateLabel="-Select-"
                                                NullValueRepresentation="Null" CssClass="dropdownText">
                                            </ig:WebDatePicker>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="wdcBarredToDate"
                                                Display="Dynamic" CssClass="ErrValStyle" ErrorMessage="<br>Select to Date"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:CompareValidator ID="cvBarredPeriod" runat="server" ControlToCompare="wdcBarredFromDate"
                                                ControlToValidate="wdcBarredToDate" CssClass="ErrValStyle" ErrorMessage="<br>Invalid Restriction Period"
                                                Display="Dynamic" Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:RangeValidator ID="rvBarredFromDate" runat="server" ControlToValidate="wdcBarredFromDate"
                                CssClass="ErrValStyle" Display="Dynamic" ErrorMessage="Invalid restriction from date"
                                Type="Date" Width="200px"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" valign="top" style="width: 117px">
                            <asp:Label ID="lblReason" runat="server" Width="150px" CssClass="FieldName">Reason*</asp:Label>
                        </td>
                        <td class="ControlTD" valign="top" style="width:450px;">
                            <asp:TextBox ID="txtBarredReason" runat="server" Width="257px" CssClass="FieldValue"
                                Height="95px" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                <br />
                            <asp:CustomValidator ID="cvReason" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength" Width="250px"
                                Display="Dynamic" ControlToValidate="txtBarredReason" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr style="height: 15px">
                        <td class="labelTD" style="width: 117px">
                            <asp:Label ID="lblAssetID" runat="server" CssClass="FieldName" Visible="False"></asp:Label>
                        </td>
                        <td class="ControlTD">
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
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblMessage" runat="server" CssClass="MessageStyle" ForeColor="Red"
                                Text="Message" Visible="False"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </Template>
    </igmisc:WebGroupBox>
</asp:Content>
