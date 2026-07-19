<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="Location.aspx.cs" Inherits="Location" Title="Location"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%--<%@ OutputCache Duration="60" VaryByParam="*" %>--%>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_Location" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
    <script type="text/javascript">
        //< ! --
        function checkDescLength(sender, args) {
            var re = /^[\w0-9\-\.\:\s]+$/;

            var tDesc = document.getElementById('<%=txtDesc.ClientID%>').value;
            if (tDesc == '') {
                // do nothing
            }
            else if (tDesc.length > parseInt('<%=txtDesc.MaxLength.ToString() %>')) {
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

        function checkLocType() {
            var ddlocType = document.getElementById('<%=ddlLocationTypeList.ClientID%>');
            if (ddlocType.selectedIndex != -1) {

                if (ddlocType.options[ddlocType.selectedIndex].text === "Room") {

                    document.getElementById('<%=txtFloor.ClientID%>').disabled = false;
                }
                else {
                    document.getElementById('<%=txtFloor.ClientID%>').disabled = true;
                }
            }
            return true;
        }
        // -->
    </script>
    <script id="Infragistics" type="text/javascript">


        //< ! --
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }
       

        function ibCreate_JS_Click(oButton, oEvent) {
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
        }
        function openLocationList(url) {
            var ctrlLocType = document.getElementById('<%=ddlLocationTypeList.ClientID%>');
            if (ctrlLocType.selectedIndex != -1) {
                if (ctrlLocType.options[ctrlLocType.selectedIndex].value == "0") {
                    alert('Select Location Type');
                }
                else {
                    winSettings = "scroll:auto; width=400; height=500;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    var hWnd = open_window(url, "LocationList", winSettings)

                    if ((document.window != null) && (hWnd.opener))
                        hWnd.opener = document.window;
                }
            }

        }

        function openModelList(url) {
            var ctrlMfg = document.getElementById('<%=ddlMfg.ClientID%>');
            var ctrlRHA = document.getElementById('<%=hdnRackHasAssets.ClientID%>');
            if (ctrlRHA.value == "1") {
                alert('Rack has assets. Asset Model can\'t be changed');
            }
            else {
                if (ctrlMfg.options[ctrlMfg.selectedIndex].value == "0") {
                    alert('Select Manufacturer');
                }
                else {
                    var winSettings = "scroll:auto; width=400; height=500;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    var hWnd = open_window(url, "ModelList", winSettings)

                    if ((document.window != null) && (hWnd.opener))
                        hWnd.opener = document.window;
                }
            }

        }

        function getValuesFromChild(txt, val, flag, header) {
            var hdnParentLocID = document.getElementById('<%=hdnParentLocationID.ClientID%>');
            var txtParentLoc = document.getElementById('<%=txtParentLocation.ClientID%>');
            var txtHdnLoc = document.getElementById('<%=hdnLocation.ClientID%>');
            var hdnMName = document.getElementById('<%=hdnModelName.ClientID%>');
            var txtmodel = document.getElementById('<%=txtModel.ClientID%>');
            var hdnmid = document.getElementById('<%=hdnModelID.ClientID%>');

            if (header == "Loc") {
                if (txtParentLoc.value == "Already Assigned to Site") {
                }
                else {
                    txtParentLoc.value = txt;
                    txtHdnLoc.value = txt;
                    hdnParentLocID.value = val;
                }
            }
            else {
                txtmodel.value = txt;
                hdnmid.value = val;
                hdnMName.value = txt;
            }

        }

        // -->

    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
        <tr>
            <td valign="top" style="width: 100%">
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif); height: 199px;">
                        </td>
                        <td valign="top" style="height: 199px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 28px;">
                                        <asp:Label ID="lblLocation" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblLocationResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 28px; width: 635px;" valign="middle" colspan="5">
                                        <asp:TextBox ID="txtLocation" runat="server" Width="227px" CssClass="FieldValue"
                                            MaxLength="50" Height="16px" TabIndex="1" meta:resourcekey="txtLocationResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqLocationVal" runat="server" CssClass="ErrValStyle"
                                            Display="Dynamic" ControlToValidate="txtLocation" meta:resourcekey="reqLocationValResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revLocation" runat="server" ControlToValidate="txtLocation"
                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                            Height="15px" Width="166px" meta:resourcekey="revLocationResource1"></asp:RegularExpressionValidator>
                                    </td>
                                    <td class="ControlTD" style="height: 28px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 82px;" align="right">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblDescResource1"> </asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 82px; width: 635px;" valign="top" colspan="5">
                                        <asp:TextBox ID="txtDesc" runat="server" Width="226px" CssClass="FieldValue" MaxLength="150"
                                            TabIndex="2" TextMode="MultiLine" Height="98px" meta:resourcekey="txtDescResource1"></asp:TextBox>
                                        <br />
                                        <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                            Display="Dynamic" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                                    </td>
                                    <td class="ControlTD" style="height: 82px" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 6px" valign="top">
                                    </td>
                                    <td class="ControlTD" style="height: 6px; width: 635px;" valign="top" colspan="5">
                                    </td>
                                    <td class="ControlTD" style="height: 6px" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 27px" valign="top">
                                        <asp:Label ID="lblloctype" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblloctypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 27px; width: 635px;" valign="top" colspan="5">
                                        <%--<ig:WebDropDown ID="ddlLocationTypeList" runat="server" Width="200px" 
                                    EnableCustomValues="False" TabIndex="5" >

                                </ig:WebDropDown>--%>
                                        <asp:DropDownList ID="ddlLocationTypeList" runat="server" Width="200px" TabIndex="3"
                                            meta:resourcekey="ddlLocationTypeListResource1" OnSelectedIndexChanged="ddlLocationTypeList_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="ErrValStyle"
                                            ControlToValidate="ddlLocationTypeList" TabIndex="6" InitialValue="0" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="ControlTD" style="height: 27px" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 29px" valign="top">
                                        <asp:Label ID="lblloc" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lbllocResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 29px; width: 530px;" valign="top">
                                        <asp:TextBox ID="txtParentLocation" runat="server" Width="523px" CssClass="FieldValue"
                                            Enabled="false" MaxLength="150" TabIndex="4" Height="16px" meta:resourcekey="txtParentLocationResource1"></asp:TextBox>
                                        &nbsp;
                                    </td>
                                    <td class="ControlTD" style="height: 29px; width: 635px;" valign="top">
                                        <a href="javascript:openLocationList('TreeList.aspx?Type=Locations&Header=Location&LocType=<%=ddlLocTypeVal%>');">
                                            <img id="imgLocButton" alt="Load List" src="images/search.gif" style="border: 0;" />
                                        </a>
                                    </td>
                                    <td class="ControlTD" style="height: 29px; width: 635px;" valign="top">
                                    </td>
                                    <td class="ControlTD" style="height: 29px; width: 635px;" valign="top">
                                    </td>
                                    <td class="ControlTD" style="height: 29px; width: 635px;" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 6px; height: 30px">
                                        <asp:Label ID="lblFloor" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblFloorResource1"></asp:Label>
                                    </td>
                                    <td align="left" style="height: 6px; width: 635px;" valign="top" colspan="5">
                                        <asp:TextBox ID="txtFloor" runat="server" Width="80px" CssClass="FieldValue" MaxLength="10"
                                            Height="16px" TabIndex="5" meta:resourcekey="txtFloorResource1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revFloor" runat="server" ControlToValidate="txtFloor"
                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[\w0-9\-\.\:\s]+$"
                                            meta:resourcekey="revFloorResource1"></asp:RegularExpressionValidator>
                                    </td>
                                    <td align="left" style="height: 6px" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 30px">
                                        <asp:Label ID="lblSerialNumber" runat="server" CssClass="FieldName" Width="109px"
                                            meta:resourcekey="lblSerialNumberResource1"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 635px;" valign="top" colspan="5">
                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="FieldValue" Width="160px"
                                            TabIndex="6" meta:resourcekey="txtSerialNoResource1" Enabled="False" MaxLength="50"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[\w0-9\-\.]+([\w0-9\-\.]+)*$"
                                            meta:resourcekey="revSerialResource1"></asp:RegularExpressionValidator>
                                    </td>
                                    <td align="left" style="height: 30px" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 30px">
                                        <asp:Label ID="lblManufacturer" runat="server" CssClass="FieldName" Width="109px"
                                            meta:resourcekey="lblManufacturerResource1"></asp:Label>
                                    </td>
                                    <td align="left" style="height: 30px; width: 635px;" valign="top" colspan="5">
                                        <asp:DropDownList ID="ddlMfg" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                            OnSelectedIndexChanged="ddlMfg_SelectedIndexChanged" TabIndex="7" Width="156px"
                                            meta:resourcekey="ddlMfgResource1">
                                            <asp:ListItem Value=" " meta:resourcekey="ListItemResource10"></asp:ListItem>
                                            <asp:ListItem Value="A" meta:resourcekey="ListItemResource11"></asp:ListItem>
                                            <asp:ListItem Value="N" meta:resourcekey="ListItemResource12"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;
                                    </td>
                                    <td align="left" style="height: 30px" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 30px">
                                        <asp:Label ID="lblModel" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblModelResource1"></asp:Label>
                                    </td>
                                    <td align="left" style="height: 30px; width: 635px;" valign="top" colspan="5">
                                        <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="ControlTD" align="left" valign="top">
                                                    <asp:TextBox ID="txtModel" runat="server" CssClass="FieldValue" Enabled="false" MaxLength="150"
                                                        meta:resourcekey="txtModelResource1" TabIndex="9" Text="No Model" Width="250px"></asp:TextBox>
                                                </td>
                                                <td class="ControlTD" align="left" valign="top">
                                                    <a href="javascript:openModelList('TreeList.aspx?Type=RackModels&MfgName=<%=ddlMfgName %>&MfgID=<%=ddlMfgID %>&BU=<%=hdnBusinessUnit.Value %>&Header=RackModels');">
                                                        <img id="img1" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="left" style="height: 30px" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 30px">
                                        <asp:Label ID="lblTag" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblTagResource1"></asp:Label>
                                    </td>
                                    <td align="left" style="height: 30px; width: 635px;" valign="top" colspan="5">
                                        <asp:TextBox ID="txtTag" runat="server" Width="120px" CssClass="FieldValue" MaxLength="24"
                                            Height="16px" TabIndex="12" meta:resourcekey="txtHeightResource1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="revTag" runat="server" ControlToValidate="txtTag"
                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[A-Za-z0-9]+$"
                                            meta:resourcekey="revTagResource1"></asp:RegularExpressionValidator>
                                    </td>
                                    <td align="left" style="height: 30px" valign="top">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 635px;" align="left" valign="top" colspan="5">
                                        <table cellpadding="0" cellspacing="0" style="width: 205px">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="height: 30px; width: 398px;">
                                                    <%--<td>--%>
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="5" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    <%-- </td>
                                                <td>--%><%--&nbsp;--%>
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" Text="Reset" UseBrowserDefaults="False"
                                                        CausesValidation="False" OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory=""
                                                        TabIndex="6" meta:resourcekey="ibResetResource1">
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--  <td colspan="2">--%>
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="357px" meta:resourcekey="lblMessageResource1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 27px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 27px; width: 800px;" align="right" colspan="6">
                                        <table>
                                            <tr>
                                                <td>
                                                    <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                                        TabIndex="7" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                                        meta:resourcekey="ibExportToExcelResource1">
                                                    </igtxt:WebImageButton>
                                                </td>
                                                <td>
                                                    <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                                        TabIndex="8" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                                        meta:resourcekey="ibDeleteResource1">
                                                        <ClientSideEvents Click="ibDelete_Click" />
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="height: 27px" align="right">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 199px; width: 6px;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="width: 100%;">
                            <table style="width: 100%; height: 315px;" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 19px; width: 135%;" valign="top">
                                        <table width="100%" style="vertical-align: top;">
                                            <tr>
                                                <td style="width: 100%" align="left" valign="top">
                                                    <ig:WebDataGrid ID="grdLocation" runat="server" AutoGenerateColumns="False" DataKeyFields="LocationID"
                                                        OnDataFiltered="grdLocation_DataFiltered" Width="98%" OnItemCommand="grdLocation_ItemCommand"
                                                        Height="300px" OnDataBound="grdLocation_DataBound" HeaderCaptionCssClass="GridHeader"
                                                        TabIndex="9" OnInitializeRow="grdLocation_InitializeRow">
                                                        <Columns>
                                                            <ig:TemplateDataField Key="Location">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="Location" NavigateUrl='<%# "~/LocationDetails.aspx?LocationID=" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "LocationID") %>'
                                                                        runat="server" Text='<%# Bind("Location") %>' meta:resourcekey="LocationResource2"></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <Header Text="Location" />
                                                            </ig:TemplateDataField>
                                                            <ig:BoundDataField DataFieldName="Description" Key="Description">
                                                                <Header Text="Description" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="ParentLocation" Key="ParentLocation">
                                                                <Header Text="Parent Location" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="LocationType" Key="LocationType">
                                                                <Header Text="Location Type" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="TagID" Key="TagID">
                                                                <Header Text="Location Tag" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <Header Text="Edit" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                        CommandArgument='<%# Eval("LocationId") %>' ImageUrl="images/edit_line.gif" ToolTip='<%# Bind("Location") %>'
                                                                        meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <Header Text="Delete" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# Bind("LocationId") %>' Visible="False"
                                                                        meta:resourcekey="lblDeleteIDResource1"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <input id="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this,'chkDelete');"
                                                                        type="checkbox" />
                                                                    Select All
                                                                </HeaderTemplate>
                                                            </ig:TemplateDataField>
                                                        </Columns>
                                                        <Behaviors>
                                                            <ig:EditingCore>
                                                            </ig:EditingCore>
                                                            <ig:Filtering>
                                                            </ig:Filtering>
                                                            <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
                                                                <PagerTemplate>
                                                                    <asp:Label ID="Label5" runat="server" Style="text-align: right;" meta:resourcekey="Label5Resource1"></asp:Label>
                                                                    <asp:Label ID="lblTotRecordCount" runat="server" Style="text-align: right;"><%=totalRecordCount %></asp:Label>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="Label6" runat="server" Style="text-align: right;" meta:resourcekey="Label6Resource1"></asp:Label>
                                                                    <asp:Label ID="lblFilRecordCount" runat="server" Style="text-align: right;"><%=this.FilterCount.ToString()%></asp:Label>
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
                            <asp:ValidationSummary ID="valLocationSummary" runat="server" CssClass="ErrValSummary"
                                HeaderText="The following items(s) cannot be blank" ShowSummary="False" meta:resourcekey="valLocationSummaryResource1" />
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 100%; width: 6px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
    <input type="hidden" id="hdnParentLocationID" runat="server" />
    <input type="hidden" id="hdnLocation" runat="server" />
    <input type="hidden" id="hdnFilterCount" runat="server" />
    <input type="hidden" id="hdnModelID" runat="server" />
    <input type="hidden" id="hdnModelName" runat="server" />
    <input type="hidden" id="hdnBusinessUnit" runat="server" />
    <input type="hidden" id="hdnRackHasAssets" runat="server" />
</asp:Content>
