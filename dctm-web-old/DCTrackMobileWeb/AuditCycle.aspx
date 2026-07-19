<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="AuditCycle.aspx.cs" Inherits="AuditCycle" Title="AuditCycle"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_AuditCycle" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <script id="Infragistics" type="text/javascript">
    <!--
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }


        function ibCreate_JS_Click(oButton, oEvent) {

            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
            //document.getElementById("ctl00_Master_ContentPlaceHolder_lblMessage").innerHTML = "";
        }

         // -->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;
        text-align: center;">
        <tr>
            <td valign="top" style="width: 100%">
                &nbsp;
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif);">
                        </td>
                        <td valign="top">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="labelTD" style="width: 74px; height: 30px; text-align: left;">
                                        <asp:Label ID="lblBU" runat="server" CssClass="FieldName" Width="100px" meta:resourcekey="lblBUResource1"></asp:Label>
                                    </td>
                                    <td class="labelTD" style="height: 30px; text-align: left;">
                                        <asp:DropDownList ID="ddlBU" runat="server" Height="19px" Width="142px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlBU_SelectedIndexChanged" CssClass="dropdownText" meta:resourcekey="ddlBUResource1">
                                            <asp:ListItem Value="0" meta:resourcekey="ListItemResource1">-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVBU" runat="server" CssClass="ErrValStyle" Width="171px"
                                            ControlToValidate="ddlBU" InitialValue="0" Height="16px" meta:resourcekey="RFVBUResource1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="labelTD" style="width: 74px; height: 30px; text-align: left;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 74px; height: 31px; text-align: left;">
                                        <asp:Label ID="lblSite" runat="server" CssClass="FieldName" Width="124px" meta:resourcekey="lblSiteResource1"></asp:Label>
                                    </td>
                                    <td class="labelTD" style="height: 31px; text-align: left;">
                                        <asp:DropDownList ID="ddlSite" runat="server" Height="19px" Width="141px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlSite_SelectedIndexChanged" CssClass="dropdownText"
                                            meta:resourcekey="ddlSiteResource1">
                                            <asp:ListItem Value="0" meta:resourcekey="ListItemResource2">-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVSite" runat="server" InitialValue="0" CssClass="ErrValStyle"
                                            Width="122px" ControlToValidate="ddlSite" Height="16px" meta:resourcekey="RFVSiteResource1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="labelTD" style="width: 74px; height: 31px; text-align: left;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 74px; height: 30px; text-align: left;">
                                        <asp:Label ID="lblRoom" runat="server" CssClass="FieldName" Width="124px" meta:resourcekey="lblRoomResource1"></asp:Label>
                                    </td>
                                    <td class="labelTD" style="height: 30px; text-align: left;">
                                        <asp:DropDownList ID="ddlRoom" runat="server" Height="19px" Width="140px" CssClass="dropdownText"
                                            meta:resourcekey="ddlRoomResource1">
                                            <asp:ListItem Value="0" meta:resourcekey="ListItemResource3">-Select-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVRoom" runat="server" InitialValue="0" CssClass="ErrValStyle"
                                            Width="129px" ControlToValidate="ddlRoom" Height="18px" meta:resourcekey="RFVRoomResource1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="labelTD" style="width: 74px; height: 30px; text-align: left;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 74px; height: 21px; text-align: left;">
                                        <asp:Label ID="lblStartDate" runat="server" CssClass="FieldName" Width="128px" meta:resourcekey="lblStartDateResource1"></asp:Label>
                                    </td>
                                    <td class="labelTD" style="height: 31px; text-align: left;">
                                        <ig:WebDatePicker ID="wdcStartDate" runat="server" NullDateLabel="-Select-"
                                            NullValueRepresentation="Null" TabIndex="16" Width="140px" meta:resourcekey="wdcStartDateResource1"
                                            DataMode="Date" CssClass="dropdownText">
                                        </ig:WebDatePicker>
                                        <asp:RequiredFieldValidator ID="RFVStartDate" runat="server" CssClass="ErrValStyle"
                                            Width="134px" ControlToValidate="wdcStartDate" Height="16px" meta:resourcekey="RFVStartDateResource1"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="wdcStartDate"
                                            Operator="GreaterThanEqual" Type="Date" CssClass="ErrValStyle" meta:resourcekey="CompareValidator2Resource1" />
                                    </td>
                                    <td class="labelTD" style="width: 74px; height: 21px; text-align: left;">
                                    </td>
                                    <td class="labelTD" style="width: 74px; height: 21px; text-align: left;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 74px; height: 19px; text-align: left;">
                                        <asp:Label ID="lblEndDate" runat="server" CssClass="FieldName" Width="128px" meta:resourcekey="lblEndDateResource1"></asp:Label>
                                    </td>
                                    <td class="labelTD" style="height: 31px; text-align: left;">
                                        <ig:WebDatePicker ID="wdcEndDate" runat="server" NullDateLabel="-Select-"
                                            NullValueRepresentation="Null" TabIndex="16" Width="140px" meta:resourcekey="wdcEndDateResource1"
                                            CssClass="dropdownText">
                                        </ig:WebDatePicker>
                                        <asp:RequiredFieldValidator ID="RFVEndDate" runat="server" CssClass="ErrValStyle"
                                            Width="132px" ControlToValidate="wdcEndDate" Height="16px" meta:resourcekey="RFVEndDateResource1"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="wdcStartDate"
                                            ControlToValidate="wdcEndDate" Operator="GreaterThanEqual" Type="Date" CultureInvariantValues="true"
                                            CssClass="ErrValStyle" meta:resourcekey="CompareValidator1Resource1"></asp:CompareValidator>
                                    </td>
                                    <td class="labelTD" style="width: 74px; height: 19px; text-align: left;">
                                    </td>
                                    <td class="labelTD" style="width: 74px; height: 19px; text-align: left;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 74px; height: 67px;">
                                    </td>
                                    <td style="height: 67px; width: 483px;" align="left" valign="top" colspan="4">
                                        <table cellpadding="0" cellspacing="0" style="width: 826px; height: 30px;">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="width: 398px;">
                                                    <%-- </td>--%>
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="4" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1"
                                                        Height="24px">
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    <%-- <td>--%>
                                                    <%-- </td>--%>
                                                    <%-- <td>--%>
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                                        OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" TabIndex="5" meta:resourcekey="ibResetResource1">
                                                    </igtxt:WebImageButton>
                                                    <%-- </td>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="357px" meta:resourcekey="lblMessageResource1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 74px;">
                                        &nbsp;
                                    </td>
                                    <td align="right" style="text-align: right;" colspan="3">
                                        <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                            TabIndex="6" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibExportToExcelResource1">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                            TabIndex="5" UseBrowserDefaults="False" CausesValidation="False" Focusable="False"
                                            ImageDirectory="" meta:resourcekey="ibDeleteResource1">
                                            <ClientSideEvents Click="ibDelete_Click" />
                                        </igtxt:WebImageButton>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); width: 6px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif);">
                        </td>
                        <td style="width: 100%;" align="center">
                            <table style="width: 100%;" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 19px; width: 100%;">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%" align="left">
                                                    <div style="width: 100%">
                                                        <ig:WebDataGrid ID="grdAuditCycle" runat="server" AutoGenerateColumns="False" DataKeyFields="ID"
                                                            Width="98%" EnableRelativeLayout="True" OnItemCommand="grdAuditCycle_ItemCommand"
                                                            Height="300px" OnDataBound="grdAuditCycle_DataBound" HeaderCaptionCssClass="GridHeader"
                                                            TabIndex="7" OnDataFiltered="grdAuditCycle_DataFiltered" OnInitializeRow="grdAuditCycle_InitializeRow">
                                                            <Columns>
                                                                <ig:BoundDataField DataFieldName="Site" Key="Site">
                                                                    <Header Text="Site" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="Room" Key="Room">
                                                                    <Header Text="Room" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="AuditCount" Key="AuditCount">
                                                                    <Header Text="AuditCount" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="StartDate" Key="StartDate">
                                                                    <Header Text="StartDate" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="EndDate" Key="EndDate">
                                                                    <Header Text="EndDate" />
                                                                </ig:BoundDataField>
                                                                <ig:TemplateDataField Key="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                            CommandArgument='<%# Eval("ID") %>' CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                    </ItemTemplate>
                                                                    <Header Text="Edit" />
                                                                </ig:TemplateDataField>
                                                                <ig:TemplateDataField Key="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                            meta:resourcekey="chkDeleteResource1" />
                                                                        <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ID") %>'
                                                                            Visible="False" meta:resourcekey="lblDeleteIDResource1"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <input id="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this,'chkDelete');"
                                                                            type="checkbox" />
                                                                        Select All
                                                                    </HeaderTemplate>
                                                                    <Header Text="Delete" />
                                                                </ig:TemplateDataField>
                                                            </Columns>
                                                            <Behaviors>
                                                                <ig:EditingCore>
                                                                </ig:EditingCore>
                                                                <ig:Filtering>
                                                                </ig:Filtering>
                                                                <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
                                                                    <PagerTemplate>
                                                                        <asp:Label ID="Label1" runat="server" Style="text-align: right;" meta:resourcekey="Label1Resource1"></asp:Label>
                                                                        <asp:Label ID="Label2" runat="server" Style="text-align: right;"><%=totalRecordCount %></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:Label ID="Label4" runat="server" Style="text-align: right;" meta:resourcekey="Label2Resource1"></asp:Label>
                                                                        <asp:Label ID="Label5" runat="server" Style="text-align: right;"><%=this.FilterCount.ToString()%></asp:Label>
                                                                    </PagerTemplate>
                                                                </ig:Paging>
                                                            </Behaviors>
                                                            <EmptyRowsTemplate>
                                                                <div style="text-align: center;">
                                                                    <br />
                                                                    <br />
                                                                    <img src="images/WebDataGrid/attention.png" alt="" align="middle" />
                                                                    No records returned.
                                                                </div>
                                                            </EmptyRowsTemplate>
                                                        </ig:WebDataGrid>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="valBUSummary" runat="server" CssClass="ErrValSummary"
                                Visible="False" meta:resourcekey="valBUSummaryResource1" />
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 100%; width: 6px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
    <input type="hidden" id="hdnFilterCount" runat="server" />
</asp:Content>
