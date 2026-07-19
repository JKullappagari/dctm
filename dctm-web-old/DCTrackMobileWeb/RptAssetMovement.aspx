<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptAssetMovement.aspx.cs"
    Inherits="RptAssetMovement" MasterPageFile="~/iAssetTrackMasterPage.master" Theme="SkinFile"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.jQuery.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<asp:Content ID="RptAssetmvmt" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.6.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="ig_ui/js/infragistics.js"></script>
    <script id="Script1" type="text/javascript">
        //<!--
        function openLocationListSRC(url) {

            var ctrlSite = document.getElementById('<%=ddlSrcSite.ClientID%>');
            if (ctrlSite.selectedIndex != -1) {
                if (ctrlSite.options[ctrlSite.selectedIndex].value == "0") {
                    alert('Select Site');
                }
                else {
                    winSettings = "scroll:auto; width=400; height=500;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    var hWnd = open_window(url, "LocationList", winSettings)

                    if ((document.window != null) && (hWnd.opener))
                        hWnd.opener = document.window;
                }
            }

        }

        function openLocationListDST(url) {
            var ctrlSite = document.getElementById('<%=ddlDstSite.ClientID%>');
            if (ctrlSite.selectedIndex != -1) {
                if (ctrlSite.options[ctrlSite.selectedIndex].value == "0") {
                    alert('Select Site');
                }
                else {
                    winSettings = "scroll:auto; width=400; height=500;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    var hWnd = open_window(url, "LocationList", winSettings)

                    if ((document.window != null) && (hWnd.opener))
                        hWnd.opener = document.window;
                }
            }

        }

        function getValuesFromChild(txt, val, header) {
            var hdnParentLocID1 = document.getElementById('<%=hdnLocationID1.ClientID%>');
            var txtParentLoc1 = document.getElementById('<%=txtSrcParentLocation.ClientID%>');
            var hdnLName1 = document.getElementById('<%=hdnLocName1.ClientID%>');

            var hdnParentLocID2 = document.getElementById('<%=hdnLocationID2.ClientID%>');
            var txtParentLoc2 = document.getElementById('<%=txtDstParentLocation.ClientID%>');
            var hdnLName2 = document.getElementById('<%=hdnLocName2.ClientID%>');


            //            if (header == "Loc") {
            //                txtParentLoc.value = txt;
            //                hdnParentLocID.value = val;
            //                hdnLName.value = txt;
            //            }
            //            else
            if (header == "Source Location") {
                txtParentLoc1.value = txt;
                hdnParentLocID1.value = val;
                hdnLName1.value = txt;
            }
            else if (header == "Destination Location") {
                txtParentLoc2.value = txt;
                hdnParentLocID2.value = val;
                hdnLName2.value = txt;
            }

        }

        //        function openLocationList2(url) {
        //            var ctrlSite = document.getElementById('<%=ddlDstSite.ClientID%>');
        //            if (ctrlSite.selectedIndex != -1) {
        //                if (ctrlSite.options[ctrlSite.selectedIndex].value == "0") {
        //                    alert('Select Site');
        //                }
        //                else {
        //                    winSettings = "scroll:auto; width=300; height=500;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
        //                    var hWnd = window.open(url, "LocationList", winSettings)

        //                    if ((document.window != null) && (hWnd.opener))
        //                        hWnd.opener = document.window;
        //                }
        //            }

        //        }

        //        function getValuesFromChild2(txt, val, header) {
        //            var hdnParentLocID = document.getElementById('<%=hdnLocationID2.ClientID%>');
        //            var txtParentLoc = document.getElementById('<%=txtDstParentLocation.ClientID%>');
        //            var hdnLName = document.getElementById('<%=hdnLocName2.ClientID%>');


        //            if (header == "Loc") {
        //                txtParentLoc.value = txt;
        //                hdnParentLocID.value = val;
        //                hdnLName.value = txt;
        //            }

        //        }
        function imgLocButton_onclick() {

        }

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
   
    <table style="height: 100%; width:98%" cellspacing="0" cellpadding="0" border="0">
        <tr style="width: auto;">
            <td class="labelTD" align="left" width="100%" colspan="4">
                <div style="width: 100%;">
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
                                    <table id="Table1" align="center" border="0" cellpadding="0" cellspacing="2" style="border-right: gray 1px solid;
                                        border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;
                                        height: 46px;" width="100%">
                                        <tr>
                                            <td style="width: 138px; height: 27px; text-align: justify;" valign="top" align="left">
                                                <asp:Label ID="lblStartDate" runat="server" CssClass="FieldName" Height="17px" meta:resourcekey="lblStartDateResource1"
                                                    Width="94px"> </asp:Label>
                                            </td>
                                            <td style="width: 320px; height: 27px; text-align: justify;" valign="top" align="left">
                                                <ig:WebDatePicker ID="WebDatePickerStartDate" runat="server" meta:resourcekey="WebDatePickerStartDateResource1"
                                                    Style="margin-left: 0px" TabIndex="1">
                                                </ig:WebDatePicker>
                                                <asp:RequiredFieldValidator ID="reqStartDateVal" runat="server" ControlToValidate="WebDatePickerStartDate"
                                                    CssClass="ErrValStyle" Height="16px" meta:resourcekey="reqStartDateValResource1"
                                                    ValidationGroup="PageValidationGroup" Width="120px"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="ControlTD" style="height: 17px; width: 17px;" valign="bottom">
                                            </td>
                                            <td class="ControlTD" style="height: 17px; width: 15px;" valign="bottom">
                                            </td>
                                            <td style="width: 144px; height: 27px; text-align: justify;" valign="top" align="left">
                                                <asp:Label ID="Label2" runat="server" CssClass="FieldName" Height="16px" meta:resourcekey="Label2Resource1"
                                                    Width="87px"></asp:Label>
                                            </td>
                                            <td style="width: 304px; height: 27px; text-align: justify;" valign="top" align="left">
                                                <ig:WebDatePicker ID="WebDatePickerEndDate" runat="server" meta:resourcekey="WebDatePickerEndDateResource1"
                                                    Style="margin-left: 0px; text-align: left;" TabIndex="2">
                                                </ig:WebDatePicker>
                                                <asp:RequiredFieldValidator ID="reqEndDateVal" runat="server" ControlToValidate="WebDatePickerEndDate"
                                                    Display="Dynamic" CssClass="ErrValStyle" meta:resourcekey="reqEndDateValResource1"
                                                    Width="120px" ValidationGroup="PageValidationGroup"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="WebDatePickerStartDate"
                                                    ControlToValidate="WebDatePickerEndDate" Operator="GreaterThanEqual" Type="Date"
                                                    Display="Dynamic" CssClass="ErrValStyle" meta:resourcekey="CompareValidator1Resource1"
                                                    Width="200px" ValidationGroup="PageValidationGroup"></asp:CompareValidator>
                                                <asp:CompareValidator ID="cvEndDate2" runat="server" ControlToValidate="WebDatePickerEndDate"
                                                    Display="Dynamic" Operator="LessThanEqual" Type="Date" CssClass="ErrValStyle"
                                                    meta:resourcekey="cvEndDate2Resource1" ValidationGroup="PageValidationGroup"
                                                    Width="200px"></asp:CompareValidator>
                                            </td>
                                            <td class="ControlTD" style="height: 17px;" valign="bottom">
                                            </td>
                                        </tr>
                                        <tr style="height: 19px">
                                            <td style="width: 138px; text-align: justify; height: 23px;" align="right">
                                            </td>
                                            <td class="labelTD" style="height: 19px; width: 320px;" valign="top">
                                                <asp:Label ID="lblSrc" runat="server" CssClass="FieldName" Height="16px" meta:resourcekey="lblSrcResource1"
                                                    Width="71px"></asp:Label>
                                            </td>
                                            <td class="ControlTD" style="height: 19px; width: 17px;">
                                            </td>
                                            <td class="ControlTD" style="height: 19px; width: 15px;">
                                            </td>
                                            <td style="width: 144px; text-align: justify; height: 23px;" align="right">
                                            </td>
                                            <td style="width: 304px">
                                                <asp:Label ID="lblDest" runat="server" CssClass="FieldName" Height="16px" meta:resourcekey="lblDestResource1"
                                                    Width="107px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6" style="width: 138px">
                                            </td>
                                            <td style="width: 320px">
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td style="width: 15px">
                                            </td>
                                            <td style="width: 144px">
                                            </td>
                                            <td class="ControlTD" style="height: 19px; width: 304px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 138px; text-align: justify; height: 25px;" align="Right" valign="top">
                                                <asp:Label ID="Label3" runat="server" CssClass="FieldName" meta:resourcekey="Label3Resource1"
                                                    Width="150px" Text=""></asp:Label>
                                            </td>
                                            <td style="width: 320px; text-align: justify; height: 25px;" align="Right" valign="top">
                                                <asp:DropDownList ID="ddlSrcRootEntity" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    meta:resourcekey="ddlSrcRootEntityResource1" OnSelectedIndexChanged="ddlSrcRootEntity_SelectedIndexChanged"
                                                    TabIndex="3" Width="140px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlSrcRootEntity"
                                                    CssClass="ErrValStyle" Display="Dynamic" Height="16px" InitialValue="0" meta:resourcekey="RequiredFieldValidator1Resource1"
                                                    ValidationGroup="PageValidationGroup" Width="88px"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 17px; height: 25px;">
                                            </td>
                                            <td style="width: 15px; height: 25px;">
                                            </td>
                                            <td style="width: 144px; text-align: justify; height: 25px;" align="Right" valign="top">
                                                <asp:Label ID="Label6" runat="server" CssClass="FieldName" Height="16px" meta:resourcekey="Label6Resource1"
                                                    Width="93px"></asp:Label>
                                            </td>
                                            <td style="width: 304px; text-align: left; height: 25px;" valign="top">
                                                <asp:DropDownList ID="ddlDstRootEntity" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    meta:resourcekey="ddlDstRootEntityResource1" OnSelectedIndexChanged="ddlDstRootEntity_SelectedIndexChanged"
                                                    TabIndex="6" Width="140px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Height="16px"
                                                    ControlToValidate="ddlDstRootEntity" CssClass="ErrValStyle" Display="Dynamic"
                                                    Width="150px" InitialValue="0" meta:resourcekey="RequiredFieldValidator2Resource1"
                                                    ValidationGroup="PageValidationGroup"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 138px; text-align: left; height: 26px;" valign="top">
                                                <asp:Label ID="Label4" Width="150px" runat="server" CssClass="FieldName" meta:resourcekey="Label4Resource1"
                                                    Text=""></asp:Label>
                                            </td>
                                            <td style="width: 320px; text-align: left; height: 26px;">
                                                <asp:DropDownList ID="ddlSrcSite" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    meta:resourcekey="ddlSrcSiteResource1" OnSelectedIndexChanged="ddlSrcSite_SelectedIndexChanged"
                                                    TabIndex="4" Width="140px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 17px; height: 26px;">
                                            </td>
                                            <td style="width: 15px; height: 26px;">
                                            </td>
                                            <td style="width: 144px; text-align: left; height: 26px;">
                                                <asp:Label ID="Label8" Width="150px" runat="server" CssClass="FieldName" meta:resourcekey="Label8Resource1"
                                                    Text=""></asp:Label>
                                            </td>
                                            <td style="width: 304px; text-align: left; height: 26px;">
                                                <asp:DropDownList ID="ddlDstSite" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    meta:resourcekey="ddlDstSiteResource1" OnSelectedIndexChanged="ddlDstSite_SelectedIndexChanged"
                                                    TabIndex="7" Width="140px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ControlTD" style="height: 19px; width: 138px;" valign="top">
                                                <asp:Label ID="Label5" Width="150px" runat="server" CssClass="FieldName" meta:resourcekey="Label5Resource1"
                                                    Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSrcParentLocation" runat="server" CssClass="FieldValue" Enabled="False"
                                                    MaxLength="150" meta:resourcekey="txtSrcParentLocationResource1" Width="191px"></asp:TextBox>
                                                <a href="javascript:openLocationListSRC('TreeList.aspx?Type=RptLocations&Site=<%=hdnSrcSite.Value %>&Header=Source Location');">
                                                    <img id="imgLocButton" alt="Load List" onclick="return imgLocButton_onclick()" src="images/search.gif"
                                                        style="border: 0;" /></a>
                                            </td>
                                            <td class="ControlTD" style="height: 19px; width: 15px;" valign="top">
                                            </td>
                                            <td class="ControlTD" style="height: 19px; width: 15px;" valign="top">
                                            </td>
                                            <td class="ControlTD" style="height: 19px; width: 144px;" valign="top">
                                                <asp:Label ID="Label9" Width="150px" runat="server" CssClass="FieldName" meta:resourcekey="Label9Resource1"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDstParentLocation" runat="server" CssClass="FieldValue" Enabled="False"
                                                    MaxLength="150" meta:resourcekey="txtDstParentLocationResource1" Width="200px"></asp:TextBox>
                                                <a href="javascript:openLocationListDST('TreeList.aspx?Type=RptLocations&Site=<%=hdnDstSite.Value %>&Header=Destination Location');">
                                                    <img id="img1" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6" style="width: 138px">
                                            </td>
                                            <td style="width: 320px">
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td style="width: 15px">
                                            </td>
                                            <td style="width: 144px">
                                            </td>
                                            <td align="left" style="text-align: left; width: 304px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6" style="width: 138px">
                                            </td>
                                            <td style="width: 320px; text-align: left;" valign="top">
                                                <%--<asp:Button ID="btnShowReport" runat="server" OnClick="btnShowReport_Click" Text="Show Report" />--%>
                                                <asp:CheckBox ID="chkShowInTransit" runat="server" CssClass="checkBoxText" meta:resourcekey="chkShowInTransitResource1"
                                                    Style="margin-left: 0px" TabIndex="10" TextAlign="Left" />
                                            </td>
                                            <td style="width: 17px">
                                            </td>
                                            <td style="width: 15px">
                                            </td>
                                            <td style="width: 144px">
                                                <asp:Label ID="Label7" runat="server" CssClass="FieldName" Height="16px" meta:resourcekey="Label7Resource1"
                                                    Width="106px"></asp:Label>
                                            </td>
                                            <td style="width: 304px; text-align: left;">
                                                <asp:DropDownList ID="ddlAssetGroup" runat="server" CssClass="dropdownText" meta:resourcekey="ddlAssetGroupResource1"
                                                    OnSelectedIndexChanged="ddlAssetGroup_SelectedIndexChanged" TabIndex="9" Width="126px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 138px; text-align: justify;">
                                                <%--<asp:Button ID="btnShowReport" runat="server" OnClick="btnShowReport_Click" Text="Show Report" />--%>
                                                <igtxt:WebImageButton ID="btnShowReport" runat="server" ImageDirectory="" meta:resourcekey="btnShowReportResource1"
                                                    OnClick="btnShowReport_Click" SkinID="uwButton" TabIndex="11" UseBrowserDefaults="False"
                                                    Width="118px">
                                                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                        WidthOfRightEdge="13" />
                                                    <ClientSideEvents Click="btnShowReport_JS_Click" />
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td colspan="5" style="text-align: justify;">
                                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ErrValStyle" Height="16px"
                                                    meta:resourcekey="lblErrorMessageResource1" Visible="False" Width="682px"></asp:Label>
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
            <td style="width: 58px;">
            </td>
        </tr>
        <tr>
            <td style="width: 176px; text-align: left;">
            </td>
            <td style="width: 54px">
            </td>
            <td style="width: 239px">
            </td>
            <td>
            </td>
        </tr>
        <%-- </table>--%>
        <tr>
            <td align="left" width="100%" colspan="4">
                <asp:Panel ID="Panel1" runat="server" Width="100%" meta:resourcekey="Panel1Resource1">
                    <%-- <asp:Panel ID="Panel1" runat="server" Width="100%">--%>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Font-Names="Verdana" ShowFindControls="false" ShowBackButton="false"
                        Height="380px" Font-Size="8pt" ShowParameterPrompts="False" KeepSessionAlive="true"
                        Style="text-align: center;overflow:auto;" meta:resourcekey="ReportViewer1Resource1">
                    </rsweb:ReportViewer>
                </asp:Panel>
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
    <%--    </asp:Panel>--%>
    <input type="hidden" id="hdnMessage" runat="server" />
    <input type="hidden" id="hdnLocationID1" runat="server" />
    <input type="hidden" id="hdnLocName1" runat="server" />
    <input type="hidden" id="hdnLocationID2" runat="server" />
    <input type="hidden" id="hdnLocName2" runat="server" />
    <input type="hidden" id="hdnSrcSite" runat="server" />
    <input type="hidden" id="hdnDstSite" runat="server" />
</asp:Content>
