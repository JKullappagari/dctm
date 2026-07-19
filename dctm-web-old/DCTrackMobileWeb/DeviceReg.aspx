<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="DeviceReg.aspx.cs" Inherits="BusinessUnit" Title="Register Device"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_BusinessUnit" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <script id="Infragistics" type="text/javascript">
        //<!--
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            return ValidateDeletionForRegDev(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }


        function ibCreate_JS_Click(oButton, oEvent) {

            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
            //document.getElementById("ctl00_Master_ContentPlaceHolder_lblMessage").innerHTML = "";
        }
        // -->
    </script>
    <script type="text/javascript" language="javascript">
        //<!--
        function showHelp() {
            var url = "UUIDhelp.html";
            winSettings = "scroll:auto; width=450; height=250;top=10;left=10;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
            var hWnd = open_window(url, "searchsubcon", winSettings)
            if ((document.window != null) && (hWnd.opener))
                hWnd.opener = document.window;
        }
        //-->
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
                                <tr style="height: 15px;">
                                    <td class="labelTD" style="width: 130px; height: 34px; text-align: left;">
                                        <asp:Label ID="lblDeviceID" runat="server" CssClass="FieldName" Width="100px" meta:resourcekey="lblDeviceIDResource1"></asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="ControlTD" style="height: 34px" align="left">
                                        <asp:TextBox ID="txtDeviceID" runat="server" Width="227px" CssClass="FieldValue"
                                            MaxLength="32" TabIndex="1" meta:resourcekey="txtDeviceIDResource1"></asp:TextBox>
                                        &nbsp;<a onclick="showHelp();" style="cursor: help;"><img alt="how to get UUID" src="./images/help.ico"
                                            height="20" width="20" /></a><asp:RequiredFieldValidator ID="regDeviceID" runat="server"
                                                CssClass="ErrValStyle" ControlToValidate="txtDeviceID" TabIndex="7" meta:resourcekey="regDeviceIDResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revDeviceID" runat="server" ControlToValidate="txtDeviceID"
                                            ValidationExpression="[0-9a-fA-F]{16,32}" CssClass="ErrValStyle" meta:resourcekey="revDeviceIDResource1">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 130px; text-align: left; height: 9px;">
                                        <asp:Label ID="lblDeviceName" runat="server" CssClass="FieldName" Width="124px" meta:resourcekey="lblDeviceNameResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" valign="top" style="height: 34px">
                                        <asp:TextBox ID="txtDeviceName" runat="server" Width="227px" CssClass="FieldValue"
                                            MaxLength="50" TabIndex="1" meta:resourcekey="txtDeviceNameResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="regDeviceName" runat="server" CssClass="ErrValStyle"
                                            ControlToValidate="txtDeviceName" TabIndex="7" meta:resourcekey="regDeviceNameResource1"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:RegularExpressionValidator ID="revGroup" runat="server" ControlToValidate="txtDeviceName"
                                            CssClass="ErrValStyle" ValidationExpression="^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$"
                                            Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revDevResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px; height: 34px; text-align: left;">
                                        <asp:Label ID="lblSite" runat="server" CssClass="FieldName" Width="128px" meta:resourcekey="lblSiteResource1"></asp:Label>
                                    </td>
                                    <td style="height: 34px" align="left" valign="top">
                                        <asp:DropDownList ID="ddlSite" runat="server" Width="170px" CssClass="dropdownText"
                                            meta:resourcekey="ddlSiteResource1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="regSite" runat="server" CssClass="ErrValStyle" ControlToValidate="ddlSite"
                                            meta:resourcekey="regSiteResource1" InitialValue="0"></asp:RequiredFieldValidator>
                                        <%-- <td>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px; height: 7px; text-align: right;">
                                        <asp:Label ID="lblStatus" runat="server" CssClass="FieldName" Width="128px" meta:resourcekey="lblStatusResource1"></asp:Label>
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        <asp:RadioButton ID="rdoActiveInt" runat="server" CssClass="displayText" GroupName="Internal"
                                            Style="cursor: hand" Checked="True" TabIndex="6" meta:resourcekey="rdoActiveIntResource1" />&nbsp;
                                        <asp:RadioButton ID="rdoDisabledInt" runat="server" CssClass="displayText" GroupName="Internal"
                                            Style="cursor: hand" TabIndex="7" meta:resourcekey="rdoDisabledIntResource1" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px; height: 67px;">
                                    </td>
                                    <td style="height: 67px" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 205px">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="width: 398px;">
                                                    <%-- </td>--%>
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="4" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
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
                                    <td style="width: 130px;">
                                        &nbsp;
                                    </td>
                                    <td align="right">
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
                                                        <ig:WebDataGrid ID="grdDevice" runat="server" AutoGenerateColumns="False" DataKeyFields="ID"
                                                            OnDataFiltered="grdDevice_DataFiltered" Width="98%" Height="300px" EnableRelativeLayout="True"
                                                            OnItemCommand="grdDevice_ItemCommand" OnDataBound="grdDevice_DataBound" HeaderCaptionCssClass="GridHeader"
                                                            TabIndex="7" OnInitializeRow="grdDevice_InitializeRow">
                                                            <Columns>
                                                                <ig:BoundDataField DataFieldName="DeviceID" Key="DeviceID">
                                                                    <Header Text="UUID" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="DeviceName" Key="DeviceName">
                                                                    <Header Text="Device Name" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="Site" Key="Site">
                                                                    <Header Text="Site" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="StatusValue" Key="StatusValue">
                                                                    <Header Text="Status" />
                                                                </ig:BoundDataField>
                                                                <ig:TemplateDataField Key="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                            CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ID") %>'
                                                                            CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
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
                            <asp:ValidationSummary ID="valDevSummary" runat="server" CssClass="ErrValSummary"
                                Visible="false" meta:resourcekey="valDevSummaryResource1" />
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
