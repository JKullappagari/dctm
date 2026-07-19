<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="Tenant.aspx.cs" Inherits="Tenant" Title="Tenant"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_AssetModel" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />

    <script id="Infragistics" type="text/javascript">



        //<!--
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            //            return ValidateDeletionNew(oButton, oEvent, 'Do you wish to Delete the selected items ?');
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }

        function addCounts(sender) {
            var hiddenCounts = document.getElementById('<%=hdnLocCount.ClientID%>');
            if (hiddenCounts.value.length > 0) {
                if (sender.value > 0) {
                    hiddenCounts.value = hiddenCounts.value + "," + sender.id.toString().substring(sender.id.toString().indexOf("dtxt")) + "#" + sender.value;
                }
            }
            else {
                if (sender.value > 0) {
                    hiddenCounts.value = sender.id.toString().substring(sender.id.toString().indexOf("dtxt")) + "#" + sender.value;
                }
            }
        }


        function ibCreate_JS_Click(oButton, oEvent) {

             var userCount = document.getElementById('<%=txtTotalUsers.ClientID%>');
            if (userCount.value.length > 0) {
                if (userCount.value  < 1) {
                    alert('Atleat one user is must for a tenant account');
                    oEvent.cancel = true;
                }
            }

            //Add code to handle your event here.

            //	    alert(objMaxOtHr.value);
            //	    alert(objMaxWHr.value);
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
            //document.getElementById("ctl00_Master_ContentPlaceHolder_lblMessage").innerHTML = "";
        }

        var minOccurance = 1
        function validate(sender, args) {
            var chkListBox = document.getElementById("<%=wdtLocSelection.ClientID%>");
            var checkbox = chkListBox.getElementsByTagName("input");
            var counter = 0;
            for (var i = 0; i < checkbox.length; i++) {
                if (checkbox[i].checked)
                    counter++;
            }

            if (minOccurance > counter) {
                args.IsValid = false;
            }
            args.IsValid = true;
        }

        //-->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
        <tr>
            <td valign="top" style="width: 100%">&nbsp;
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif); height: 199px;"></td>
                        <td valign="top" style="height: 199px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="90%"
                                border="0">
                                <tr>
                                    <td class="labelTD" style="width: 58px; text-align: left; height: 30px;">
                                        <asp:Label ID="lblFullName" runat="server" CssClass="FieldName" Width="150px" Height="24px"
                                            meta:resourcekey="lblFullNameResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 30px; width: 635px; margin-left: 40px;" valign="middle">
                                        <asp:TextBox ID="txtFullName" runat="server" CssClass="FieldValue" MaxLength="150"
                                            Style="vertical-align: top" TabIndex="1" Width="245px" meta:resourcekey="txtFullNameResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                            CssClass="ErrValStyle" Width="200px" meta:resourcekey="rfvFullNameResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revFullName" runat="server" ControlToValidate="txtFullName"
                                            CssClass="ErrValStyle" ValidationExpression="^[A-Za-z0-9\.]+(\s{1}[A-Za-z0-9\.]+)*\s{0,1}$"
                                            Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revFullNameResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; text-align: left; height: 30px;">
                                        <asp:Label ID="lblShortName" runat="server" CssClass="FieldName" Width="150px" Height="24px"
                                            meta:resourcekey="lblShortNameResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 30px; width: 635px; margin-left: 40px;" valign="middle">
                                        <asp:TextBox ID="txtShortName" runat="server" CssClass="FieldValue" MaxLength="6"
                                            Style="vertical-align: top" TabIndex="2" Width="70px" meta:resourcekey="txtShortNameResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvShortName" runat="server" ControlToValidate="txtShortName"
                                            CssClass="ErrValStyle" meta:resourcekey="rfvShortNameResource1" Width="200px"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revShortName" runat="server" ControlToValidate="txtShortName"
                                            CssClass="ErrValStyle" ValidationExpression="^[a-zA-Z0-9]+$"
                                            Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revShortNameResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; text-align: left; height: 28px;">
                                        <asp:Label ID="lblType" runat="server" CssClass="FieldName" Width="139px" Height="23px"
                                            meta:resourcekey="lblTypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 28px; width: 635px;" valign="middle">
                                        <asp:DropDownList ID="ddlTenantType" runat="server" CssClass="dropdownText" TabIndex="3"
                                            Width="150px" meta:resourcekey="ddlDivisonResource1" AutoPostBack="True" OnSelectedIndexChanged="ddlTenantType_SelectedIndexChanged">
                                            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource9"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlTenantType"
                                            CssClass="ErrValStyle" meta:resourcekey="rfvTypeResource1" Width="200px"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; text-align: left; height: 27px" valign="top">
                                        <asp:Label ID="lblSize" runat="server" CssClass="FieldName" Width="155px" Height="16px"
                                            meta:resourcekey="lblSizeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 27px; width: 635px;" valign="top">
                                        <asp:TextBox ID="txtSize" runat="server" CssClass="FieldValue" MaxLength="2"
                                            Style="vertical-align: top" TabIndex="4" Width="60px" meta:resourcekey="txtSizeResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSize" runat="server" ControlToValidate="txtSize"
                                            CssClass="ErrValStyle" Width="200px" meta:resourcekey="rfvSizeResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revSize" runat="server" ControlToValidate="txtSize"
                                            CssClass="ErrValStyle" ValidationExpression="^[0-9]*$"
                                            Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revSizeResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; text-align: left; height: 27px" valign="top">
                                        <asp:Label ID="lblLocSelection" runat="server" CssClass="FieldName" Width="155px" Height="16px"
                                            meta:resourcekey="lblLocSelectionResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 27px; width: 250px;" valign="top" align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <ig:WebDataTree ID="wdtLocSelection" runat="server" Height="170px" Font-Bold="True"
                                                        Width="389px" Font-Size="X-Small" CheckBoxMode="BiState" BorderStyle="Solid"
                                                        BorderColor="#999999" meta:resourcekey="TreeLocationResource1" TabIndex="5" AutoPostBackFlags-CheckBoxSelectionChanged="On" OnCheckBoxSelectionChanged="wdtLocSelection_CheckBoxSelectionChanged">
                                                    </ig:WebDataTree>
                                                    <asp:CustomValidator ID="cvLocaSelection" runat="server" CssClass="ErrValStyle" EnableClientScript="true"
                                                        Display="Dynamic" Height="15px" Width="150px" ClientValidationFunction="validate"></asp:CustomValidator>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Panel ID="phTypeSize" runat="server" Height="150px" Width="350px" ScrollBars="Auto"></asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; text-align: left; height: 27px" valign="top">
                                        <asp:Label ID="lblTotalUsers" runat="server" CssClass="FieldName" Width="155px" Height="16px"
                                            meta:resourcekey="lblTotalUsersResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 27px; width: 635px;" valign="top">
                                        <asp:TextBox ID="txtTotalUsers" runat="server" CssClass="FieldValue" MaxLength="1"
                                            Style="vertical-align: top" TabIndex="6" Width="60px" meta:resourcekey="txtTotalUsersResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTotalUsers" runat="server" ControlToValidate="txtTotalUsers"
                                            CssClass="ErrValStyle" Width="200px" meta:resourcekey="rfvTotalUsersResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revTotalUsers" runat="server" ControlToValidate="txtTotalUsers"
                                            CssClass="ErrValStyle" ValidationExpression="^[0-9]*$"
                                            Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revTotalUsersResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; text-align: left; height: 27px" valign="top">
                                        <asp:Label ID="lblContact" runat="server" CssClass="FieldName" Width="155px" Height="16px"
                                            meta:resourcekey="lblContactResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 27px; width: 635px;" valign="top">
                                        <table id="tblContact" style="width: 70%;" border="1">
                                            <tr style="height: 30px">
                                                <td class="labelTD" style="width: 58px; text-align: left; height: 27px" valign="top">
                                                    <asp:Label ID="lblFirstName" runat="server" CssClass="FieldName" Width="155px" Height="16px"
                                                        meta:resourcekey="lblFirstNameResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="height: 27px; width: 635px;" valign="top">
                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="FieldValue" MaxLength="50"
                                                        Style="vertical-align: top" TabIndex="7" Width="200px" meta:resourcekey="txtFirstNameResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtFirstName" runat="server" ControlToValidate="txtFirstName"
                                                        CssClass="ErrValStyle" Width="200px" meta:resourcekey="rfvFirstNameResource1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revtxtFirstName" runat="server" ControlToValidate="txtFirstName"
                                                        CssClass="ErrValStyle" ValidationExpression="^[A-Za-z0-9 .]+"
                                                        Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revFirstNameResource1"></asp:RegularExpressionValidator>
                                                </td>
                                                <td class="labelTD" style="width: 58px; text-align: left; height: 27px" valign="top">
                                                    <asp:Label ID="lblLastName" runat="server" CssClass="FieldName" Width="155px" Height="16px"
                                                        meta:resourcekey="lblLastNameResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="height: 27px; width: 635px;" valign="top">
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="FieldValue" MaxLength="50"
                                                        Style="vertical-align: top" TabIndex="8" Width="200px" meta:resourcekey="txtLastNameResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                                        CssClass="ErrValStyle" Width="200px" meta:resourcekey="rfvLastNameResource1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revLastName" runat="server" ControlToValidate="txtLastName"
                                                        CssClass="ErrValStyle" ValidationExpression="^[A-Za-z0-9 .]+"
                                                        Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revLastNameResource1"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 58px; text-align: left; height: 27px" valign="top">
                                                    <asp:Label ID="lblEmail" runat="server" CssClass="FieldName" Width="155px" Height="16px"
                                                        meta:resourcekey="lblEmailResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="height: 27px; width: 635px;" valign="top">
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="FieldValue" MaxLength="50"
                                                        Style="vertical-align: top" TabIndex="9" Width="200px" meta:resourcekey="txtEmailResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                        CssClass="ErrValStyle" Width="200px" meta:resourcekey="rfvEmailResource1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                                        CssClass="ErrValStyle" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revEmailResource1"></asp:RegularExpressionValidator>
                                                </td>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 7px;">&nbsp;</td>
                                    <td style="height: 7px; width: 635px;" align="left" valign="top">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 7px;">&nbsp;
                                    </td>
                                    <td style="height: 7px; width: 95%;" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="height: 30px; width: 398px;">
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="10" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    &nbsp;
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                                        OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" TabIndex="11" meta:resourcekey="ibResetResource1">
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="80%" meta:resourcekey="lblMessageResource1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 27px;">&nbsp;
                                    </td>
                                    <td style="height: 27px; width: 635px;" align="right">
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                            TabIndex="11" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibExportToExcelResource1">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                            TabIndex="12" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            Style="margin-left: 0px" meta:resourcekey="ibDeleteResource1">
                                            <ClientSideEvents Click="ibDelete_Click" />
                                        </igtxt:WebImageButton>
                                        &nbsp; &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 199px; width: 6px;"></td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td style="width: 100%;" align="top">
                            <table style="width: 100%; height: 315px;" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 19px; width: 135%;" valign="top">
                                        <table width="100%" style="vertical-align: top">
                                            <tr>
                                                <td style="width: 100%" align="left" valign="top">
                                                    <ig:WebDataGrid ID="grdTenant" runat="server" AutoGenerateColumns="False" DataKeyFields="TenantId"
                                                        Width="98%" OnItemCommand="grdTenant_ItemCommand" OnDataFiltered="grdTenant_DataFiltered"
                                                        OnDataBound="grdTenant_DataBound" HeaderCaptionCssClass="GridHeader" TabIndex="13"
                                                        Height="300px">
                                                        <Columns>
                                                            <ig:BoundDataField DataFieldName="TenantFullName" Key="TenantFullName">
                                                                <Header Text="Tenant Full Name" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="TenantShortName" Key="TenantShortName">
                                                                <Header Text="Tenant Short Name" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="TenantType" Key="TenantType">
                                                                <Header Text="Tenant Type" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="TenantTypeSize" Key="TenantTypeSize">
                                                                <Header Text="Tenant Type Size" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                        CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "TenantId") %>'
                                                                        CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                                <Header Text="Edit" />
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "TenantId") %>'
                                                                        Visible="False" meta:resourcekey="lblDeleteIDResource1"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <input id="chkAll" runat="server" onclick="javascript: SelectAllCheckboxes(this, 'chkDelete');"
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
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="valBUSummary" runat="server" CssClass="ErrValSummary"
                                ShowSummary="False" meta:resourcekey="valBUSummaryResource1" />
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 100%; width: 6px;"></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
    <input type="hidden" id="hdnFilterCount" runat="server" />
    <input type="hidden" id="hdnLocCount" runat="server" />
</asp:Content>
