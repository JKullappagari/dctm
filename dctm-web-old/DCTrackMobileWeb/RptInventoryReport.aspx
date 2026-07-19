<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptInventoryReport.aspx.cs"
    Inherits="RptInventoryReport" MasterPageFile="~/iAssetTrackMasterPage.master"
    Theme="SkinFile" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.jQuery.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<asp:Content ID="RptAssetmvmt" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.6.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="ig_ui/js/infragistics.js"></script>
    <script type="text/javascript" language="javascript">
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
                                    <ig:ExplorerBarItem TemplateId="tmpAssetSearch">
                                    </ig:ExplorerBarItem>
                                </Items>
                            </ig:ExplorerBarGroup>
                        </Groups>
                        <Templates>
                            <ig:ItemTemplate TemplateID="tmpAssetSearch">
                                <Template>
                                    <%-- <table>--%>
                                    <table width="100%">
                                        <tr>
                                            <td width="30%" align="left" valign="top">
                                                <asp:Label ID="lblLocation" runat="server" Width="200px" CssClass="FieldName" meta:resourcekey="lblLocationResource1"></asp:Label>
                                                <ig:WebDataTree ID="TreeLocation" runat="server" Height="170px" Font-Bold="True"
                                                    Font-Size="X-Small" CheckBoxMode="BiState" BorderStyle="Solid" BorderColor="#999999"
                                                    meta:resourcekey="TreeLocationResource1">
                                                </ig:WebDataTree>
                                            </td>
                                            <td width="70%" valign="top">
                                                <table>
                                                    <tr>
                                                        <td class="labelTD" style="height: 17px" valign="top">
                                                            <asp:Label ID="lblSDate" runat="server" CssClass="FieldName" meta:resourcekey="lblSDateResource1"
                                                                Width="88px"></asp:Label>
                                                        </td>
                                                        <td class="ControlTD" style="height: 17px;" valign="bottom">
                                                            <ig:WebDatePicker ID="WebDatePickerStartDate" runat="server" meta:resourcekey="WebDatePickerStartDateResource1"
                                                                Style="margin-left: 0px" TabIndex="1">
                                                            </ig:WebDatePicker>
                                                        </td>
                                                        <td class="labelTD" style="height: 17px;" valign="top" colspan="2">
                                                            <asp:Label ID="lblEDate" runat="server" CssClass="FieldName" meta:resourcekey="lblEDateResource1"
                                                                Width="89px"></asp:Label>
                                                        </td>
                                                        <td class="ControlTD" style="height: 17px;" valign="bottom">
                                                            <ig:WebDatePicker ID="WebDatePickerEndDate" runat="server" meta:resourcekey="WebDatePickerEndDateResource1"
                                                                Style="margin-left: 0px; text-align: left;" TabIndex="2">
                                                            </ig:WebDatePicker>
                                                        </td>
                                                        <td class="labelTD" style="height: 17px;" valign="bottom">
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="labelTD" style="height: 19px;">
                                                            <asp:RequiredFieldValidator ID="reqStartDateVal" runat="server" CssClass="ErrValStyle"
                                                                ControlToValidate="WebDatePickerStartDate" Width="120px" meta:resourcekey="reqStartDateValResource1"
                                                                ValidationGroup="PageValidationGroup"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="ControlTD" style="height: 15px;" valign="top">
                                                            <asp:CompareValidator ID="cvEndDate" runat="server" ControlToCompare="WebDatePickerStartDate"
                                                                ControlToValidate="WebDatePickerEndDate" Operator="GreaterThanEqual" Type="Date"
                                                                Width="250px" CssClass="ErrValStyle" meta:resourcekey="CompareValidatorEndDateResource1"
                                                                ValidationGroup="PageValidationGroup"></asp:CompareValidator>
                                                        </td>
                                                        <td class="labelTD" style="height: 19px;" colspan="2">
                                                            <asp:CompareValidator ID="cvEndDate2" runat="server" ControlToValidate="WebDatePickerEndDate"
                                                                Operator="LessThanEqual" Type="Date" Width="250px" CssClass="ErrValStyle" meta:resourcekey="cvEndDate2Resource1"
                                                                ValidationGroup="PageValidationGroup"></asp:CompareValidator>
                                                        </td>
                                                        <td class="ControlTD" style="height: 19px;">
                                                            <asp:RequiredFieldValidator ID="reqEndDateVal" runat="server" CssClass="ErrValStyle"
                                                                ControlToValidate="WebDatePickerEndDate" Width="120px" meta:resourcekey="reqEndDateValResource1"
                                                                ValidationGroup="PageValidationGroup"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="labelTD" style="height: 19px;">
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="labelTD" style="height: 19px;">
                                                        </td>
                                                        <td class="ControlTD" style="height: 19px;">
                                                            <%--<asp:CompareValidator ID="cvStartDate" runat="server"  
                                          
                                            CssClass="ErrValStyle" meta:resourcekey="CompareValidatorStartDateResource1" 
                                                          
                                                            ControlToValidate="WebDatePickerStartDate" Operator="GreaterThanEqual" 
                                                            Type="Date"></asp:CompareValidator>--%>
                                                            <%--<asp:CompareValidator ID="cvStartDate" runat="server" 
                                            ControlToValidate="WebDatePickerStartDate" Operator="GreaterThanEqual" Type="Date" 
                                            CssClass="ErrValStyle" meta:resourcekey="CompareValidatorStartDateResource1" 
                                                             />--%>
                                                        </td>
                                                        <td class="labelTD" style="height: 19px;" valign="top" colspan="2">
                                                        </td>
                                                        <td class="ControlTD" style="height: 19px;">
                                                            <%--<asp:CompareValidator ID="cvEndDate" runat="server" ControlToCompare="WebDatePickerStartDate"
                                                                ControlToValidate="WebDatePickerEndDate" Operator="GreaterThanEqual" Type="Date" Width="200px"
                                                                CssClass="ErrValStyle" meta:resourcekey="CompareValidatorEndDateResource1"></asp:CompareValidator>--%>
                                                        </td>
                                                        <td class="labelTD" style="height: 19px;" align="left">
                                                            <%--  <a href="javascript:openModelList('ModelList.aspx?Type=SearchModels&MfgName=<%=ddlMfg.SelectedItem.Text %>&MfgID=<%=ddlMfg.SelectedItem.Value %>&BU=<%=ddlBusinessUnit.SelectedItem.Value %>&Header=AssetModel');">
                                                        <img id="img1" alt="Load List" src="images/search.gif" style="border: 0;" /></a>--%>
                                                        </td>
                                                        <%--<td class="ControlTD" style="height: 19px;">
                                        
                                    </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td colspan="2" class="ControlTD" style="height: 19px;">
                                                            <%--<asp:RequiredFieldValidator ID="rfvTransType" runat="server" ControlToValidate="ddlTransType"
                                                            CssClass="ErrValStyle" InitialValue="0" meta:resourcekey="rfvTransTypeResource"></asp:RequiredFieldValidator>--%>
                                                            <asp:RadioButton ID="rbHistory" CssClass="displayText" runat="server" meta:resourcekey="rbHistoryResource1"
                                                                GroupName="GetDataType" Width="150px" />
                                                        </td>
                                                        <td class="ControlTD" colspan="2" style="height: 19px;">
                                                            <asp:RadioButton ID="rbLatest" runat="server" meta:resourcekey="rbLatestResource1"
                                                                GroupName="GetDataType" CssClass="displayText" Checked="true" Width="150px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <igtxt:WebImageButton ID="btnShowReport" runat="server" ImageDirectory="" OnClick="btnShowReport_Click"
                                                                SkinID="uwButton" TabIndex="13" UseBrowserDefaults="False" Width="118px" meta:resourcekey="btnShowReportResource1">
                                                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                                    WidthOfRightEdge="13" />
                                                                <ClientSideEvents Click="btnShowReport_JS_Click" />
                                                            </igtxt:WebImageButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="100%"></asp:Label>
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
                </div>
                <br />
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td valign="top" align="left">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" ShowParameterPrompts="False"
                    Style="overflow: auto;" KeepSessionAlive="true" ShowFindControls="false" ShowBackButton="false"
                    Font-Names="Verdana" Font-Size="8pt" Height="350px" meta:resourcekey="ReportViewer1Resource1">
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
