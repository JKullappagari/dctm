<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptTransactionHistory.aspx.cs"
    Inherits="RptTransactionHistory" MasterPageFile="~/iAssetTrackMasterPage.master"
    Theme="SkinFile" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.jQuery.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="RptAssetmvmt" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.6.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="ig_ui/js/infragistics.js"></script>
    <script id="Script1" type="text/javascript">
        //<!--
        function btnShowReport_JS_Click(oButton, oEvent) {
            if (performCheck()) {
                ShowProgress();
            }
            else {
                oEvent.cancel = true;
            }
        }

        function performCheck() {
            if (Page_ClientValidate('PageValidationGroup')) {
                return true;
            }
            else {
                return false;
            }
        }

        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        //-->
    </script>
    <table cellpadding="0" cellspacing="0" style="vertical-align: top; width: 97%;">
        <tr>
            <td valign="top">
                <div style="width: 100%; text-align: left;">
                    <ig:WebExplorerBar ID="wpSearchOptions" runat="server" GroupExpandAction="HeaderClick"
                        meta:resourcekey="wpSearchOptionsResource1" Width="100%" BorderWidth="1px">
                        <Groups>
                            <ig:ExplorerBarGroup Text="Search Options" Expanded="true">
                                <Items>
                                    <ig:ExplorerBarItem TemplateId="tmpTransaction">
                                    </ig:ExplorerBarItem>
                                </Items>
                            </ig:ExplorerBarGroup>
                        </Groups>
                        <Templates>
                            <ig:ItemTemplate TemplateID="tmpTransaction">
                                <Template>
                                    <table width="95%">
                                        <tr>
                                            <td width="20%" align="left" valign="top">
                                                <asp:Label ID="lblLocation" runat="server" Width="150px" CssClass="FieldName" meta:resourcekey="lblLocationResource1"></asp:Label>
                                                <ig:WebDataTree ID="TreeLocation" runat="server" Height="170px" Font-Bold="True"
                                                    Width="250px" Font-Size="X-Small" CheckBoxMode="BiState" BorderStyle="Solid"
                                                    BorderColor="#999999" meta:resourcekey="TreeLocationResource1">
                                                </ig:WebDataTree>
                                            </td>
                                            <td width="80%" valign="top" align="left">
                                                <table>
                                                    <tr>
                                                        <td class="labelTD" valign="top" style="width: 15%" align="right">
                                                            <asp:Label ID="lblSDate" runat="server" CssClass="FieldName" meta:resourcekey="lblSDateResource1"
                                                                Width="88px"></asp:Label>
                                                        </td>
                                                        <td class="ControlTD" align="left" valign="bottom" style="width: 15%">
                                                            <ig:WebDatePicker ID="WebDatePickerStartDate" runat="server" meta:resourcekey="WebDatePickerStartDateResource1"
                                                                Style="margin-left: 0px" TabIndex="1">
                                                            </ig:WebDatePicker>
                                                        </td>
                                                        <td class="labelTD" valign="top" style="width: 20%" align="right">
                                                            <asp:Label ID="lblTransStatus" Width="150px" runat="server" CssClass="FieldName"
                                                                meta:resourcekey="Label2Resource1"></asp:Label>
                                                        </td>
                                                        <td class="ControlTD" align="left" valign="bottom" style="width: 20%">
                                                            <asp:DropDownList ID="ddlTransStatus" runat="server" CssClass="dropdownText" meta:resourcekey="ddlTransTypeResource1"
                                                                TabIndex="7" Width="156px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="labelTD" valign="top" style="width: 15%" align="right">
                                                            <asp:Label ID="lblEDate" runat="server" CssClass="FieldName" meta:resourcekey="lblEDateResource1"
                                                                Width="89px"></asp:Label>
                                                        </td>
                                                        <td class="ControlTD" align="left" valign="bottom" style="width: 15%">
                                                            <ig:WebDatePicker ID="WebDatePickerEndDate" runat="server" meta:resourcekey="WebDatePickerEndDateResource1"
                                                                Style="margin-left: 0px; text-align: left;" TabIndex="2">
                                                            </ig:WebDatePicker>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ControlTD" valign="top" style="width: 15%">
                                                            <asp:RequiredFieldValidator ID="reqStartDateVal" runat="server" CssClass="ErrValStyle" ValidationGroup="PageValidationGroup"
                                                                ControlToValidate="WebDatePickerStartDate" Width="88px" meta:resourcekey="reqStartDateValResource1"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="labelTD" style="width: 15%">
                                                            <asp:CompareValidator ID="cvEndDate" runat="server" ControlToCompare="WebDatePickerStartDate"
                                                                ControlToValidate="WebDatePickerEndDate" Operator="GreaterThanEqual" Type="Date" ValidationGroup="PageValidationGroup"
                                                                Width="88px" CssClass="ErrValStyle" meta:resourcekey="CompareValidatorEndDateResource1"></asp:CompareValidator>
                                                        </td>
                                                        <td class="labelTD" valign="top" style="width: 20%">
                                                        </td>
                                                        <td class="ControlTD" valign="top" style="width: 20%">
                                                        </td>
                                                        <td class="ControlTD" style="width: 15%">
                                                            <asp:RequiredFieldValidator ID="reqEndDateVal" runat="server" CssClass="ErrValStyle" ValidationGroup="PageValidationGroup"
                                                                ControlToValidate="WebDatePickerEndDate" Width="88px" meta:resourcekey="reqEndDateValResource1"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="labelTD" style="width: 15%">
                                                            <asp:CompareValidator ID="cvEndDate2" runat="server" ControlToValidate="WebDatePickerEndDate" ValidationGroup="PageValidationGroup"
                                                                Operator="LessThanEqual" Type="Date" Width="88px" CssClass="ErrValStyle" meta:resourcekey="cvEndDate2Resource1"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="labelTD" style="width: 15%" align="right">
                                                            <asp:Label ID="lblSNo" Width="88px" runat="server" CssClass="FieldName" meta:resourcekey="Label3Resource1"></asp:Label>
                                                        </td>
                                                        <td class="ControlTD" style="width: 15%">
                                                            <asp:TextBox ID="txtSNo" runat="server" CssClass="FieldValue" Height="16px" MaxLength="100"
                                                                meta:resourcekey="txtSNoResource1" TabIndex="6" Width="200px"></asp:TextBox>
                                                        </td>
                                                        <td class="labelTD" valign="top" style="width: 70%" colspan="4">
                                                            <asp:RegularExpressionValidator ID="revSerialNo" runat="server" ControlToValidate="txtSNo" ValidationGroup="PageValidationGroup"
                                                                CssClass="ErrValStyle" Display="dynamic" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                                                Height="15px" Width="166px" meta:resourcekey="revSerialNoResource1"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 5px">
                                                        <td>
                                                        </td>
                                                        <td class="ControlTD">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="ControlTD">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6" align="left">
                                                            <igtxt:WebImageButton ID="btnShowReport" runat="server" ImageDirectory="" OnClick="btnShowReport_Click"
                                                                SkinID="uwButton" TabIndex="13" UseBrowserDefaults="False" Width="118px" meta:resourcekey="btnShowReportResource1">
                                                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                                    WidthOfRightEdge="13" />
                                                                <Appearance>
                                                                    <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                                    </ButtonStyle>
                                                                </Appearance>
                                                                <ClientSideEvents Click="btnShowReport_JS_Click" />
                                                            </igtxt:WebImageButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6" align="left">
                                                            <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="100%" meta:resourcekey="lblMessageResource1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </Template>
                            </ig:ItemTemplate>
                        </Templates>
                    </ig:WebExplorerBar>
                    <br />
                    <br />
                    <br />
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" ShowParameterPrompts="False"
                    KeepSessionAlive="true" ShowFindControls="false" ShowBackButton="false" Font-Names="Verdana"
                    Font-Size="8pt" meta:resourcekey="ReportViewer1Resource1" Style="overflow: auto;"
                    Height="350px">
                </rsweb:ReportViewer>
            </td>
        </tr>
        <tr>
            <td>
                <div class="loading" align="center">
                    Loading Report...<br />
                    <img src=".\images\loader.gif" alt="Loading..." />
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
</asp:Content>
