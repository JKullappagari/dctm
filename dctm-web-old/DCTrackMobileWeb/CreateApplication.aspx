<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="CreateApplication.aspx.cs" Inherits="CreateApplication"
    Title="Create Application" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
    <!--
        function checkDescLength(sender, args) {
            var re = /^[\w0-9\-\.\:\s\&\,]+$/;

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
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            //            return ValidateDeletionNew(oButton, oEvent, 'Do you wish to Delete the selected items ?');
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }

       
        function ibCreate_JS_Click(oButton, oEvent) {
            //Add code to handle your event here.

            //	    alert(objMaxOtHr.value);
            //	    alert(objMaxWHr.value);
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
            //document.getElementById("ctl00_Master_ContentPlaceHolder_lblMessage").innerHTML = "";
        }

        function searchOriginatorPopup() {
            var url = "UserSearchPopup.aspx?BusinessUnit=" + document.forms[0].elements['<%=ddlBU.ClientID%>'].value;
            url = url + "&DisplayNameField=" + document.getElementById("<%=txtOwner.ClientID %>").id;
            url = url + "&IDField=" + document.getElementById("<%=hdnOwnerID.ClientID %>").id;
            url = url + "&NameField=" + document.getElementById("<%=hdnOwnerName.ClientID %>").id;

            //alert(url);

            winSettings = "scroll:auto; width=880; height=470;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
            var hWnd = open_window(url, "searchsubcon", winSettings)


            //var hWnd=window.open(url,"","toolbar,status,scrollbars,resizable,height=420,width=700,screenx=0")
            if ((document.window != null) && (hWnd.opener))
                hWnd.opener = document.window;


        }
        
        //-->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
        <tr>
            <td valign="top" style="width: 100%">
                &nbsp;
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif); height: 199px;">
                        </td>
                        <td valign="top" style="height: 199px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="labelTD" style="width: 21px; text-align: left; height: 30px;">
                                        <asp:Label ID="lblApplCriticality0" runat="server" CssClass="FieldName" Width="145px"
                                            Height="23px" meta:resourcekey="lblApplCriticality0Resource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 30px; width: 635px; margin-left: 40px;" valign="middle">
                                        <asp:DropDownList ID="ddlBU" runat="server" CssClass="dropdownText" Width="196px"
                                            TabIndex="6" AutoPostBack="True" OnSelectedIndexChanged="ddlBU_SelectedIndexChanged"
                                            meta:resourcekey="ddlBUResource1">
                                            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqApplCriticality0" runat="server" ControlToValidate="ddlBU"
                                            CssClass="ErrValStyle" InitialValue="0" meta:resourcekey="reqApplCriticality0Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 21px; text-align: left; height: 30px;">
                                        <asp:Label ID="lblApplicationName" runat="server" CssClass="FieldName" Width="135px"
                                            Height="24px" meta:resourcekey="lblApplicationNameResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 30px; width: 635px; margin-left: 40px;" valign="middle">
                                        <asp:TextBox ID="txtApplicationName" runat="server" CssClass="FieldValue" MaxLength="250"
                                            Style="vertical-align: top" TabIndex="1" Width="350px" meta:resourcekey="txtApplicationNameResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvApplName" runat="server" ControlToValidate="txtApplicationName"
                                            CssClass="ErrValStyle" Width="200px" meta:resourcekey="rfvApplNameResource1"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revApplication" runat="server" ControlToValidate="txtApplicationName"
                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[\w\-\.&,:]+(\s{1}[\w\-\.&,\:]+)*\s{0,1}$"
                                            Width="166px" meta:resourcekey="revApplicationResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 21px; text-align: left; height: 58px;">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="145px" Height="22px"
                                            meta:resourcekey="lblDescResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 58px; width: 635px;" valign="top">
                                        <asp:TextBox ID="txtDesc" runat="server" Width="226px" CssClass="FieldValue" MaxLength="150"
                                            TabIndex="2" TextMode="MultiLine" Height="44px" meta:resourcekey="txtDescResource1"></asp:TextBox>
                                        <br />
                                        <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                            Display="Dynamic" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 21px; text-align: left; height: 28px;">
                                        <asp:Label ID="lblApplType" runat="server" CssClass="FieldName" Width="145px" Height="23px"
                                            meta:resourcekey="lblApplTypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 28px; width: 635px;" valign="middle">
                                        <asp:DropDownList ID="ddlApplType" runat="server" CssClass="dropdownText" Width="196px"
                                            TabIndex="3" meta:resourcekey="ddlApplTypeResource1">
                                            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:RegularExpressionValidator ID="regLocationVal" runat="server" ControlToValidate="txtLocation"
                                    CssClass="ErrValStyle" Display="Dynamic" ErrorMessage="Invalid Location Name - use alphabets/digits and no spaces allowed"
                                    ValidationExpression="^[A-Za-z0-9]+" Height="15px" Width="166px"></asp:RegularExpressionValidator>  --%>
                                        <%--<asp:RequiredFieldValidator ID="reqApplType" runat="server" 
                                    ControlToValidate="ddlApplType" CssClass="ErrValStyle" 
                                    EnableClientScript="true" ErrorMessage="Select Application Type" 
                                    InitialValue="0" ValidationGroup="grpAppl"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 21px; text-align: left; height: 27px;">
                                        <asp:Label ID="lblApplStatus" runat="server" CssClass="FieldName" Width="145px" Height="16px"
                                            meta:resourcekey="lblApplStatusResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 27px; width: 635px;" valign="top">
                                        <asp:DropDownList ID="ddlAppStatus" runat="server" CssClass="dropdownText" TabIndex="5"
                                            Width="196px" meta:resourcekey="ddlAppStatusResource1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 21px; text-align: left; height: 29px" 
                                        valign="top">
                                        <asp:Label ID="lblApplCriticality" runat="server" CssClass="FieldName" Width="145px"
                                            Height="23px" meta:resourcekey="lblApplCriticalityResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 29px; width: 635px;" valign="top">
                                        <asp:DropDownList ID="ddlApplCriticality" runat="server" CssClass="dropdownText"
                                            TabIndex="5" Width="196px" meta:resourcekey="ddlApplCriticalityResource1">
                                            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource8"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="reqApplCriticality" 
                                    runat="server" ControlToValidate="ddlApplCriticality"
                                    CssClass="ErrValStyle" EnableClientScript="true" ErrorMessage="Select Application Criticality"
                                    InitialValue="0" ValidationGroup="grpAppl"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 21px; text-align: left; height: 29px" 
                                        valign="top">
                                        <asp:Label ID="lblApplDivision" runat="server" CssClass="FieldName" Width="145px"
                                            Height="16px" meta:resourcekey="lblApplDivisionResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 29px; width: 635px;" valign="top">
                                        <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                            <tr align="left">
                                                <td class="ControlTD" align="left" valign="top">
                                                    <asp:TextBox ID="txtOwner" runat="server" CssClass="FieldValue" MaxLength="255" Enabled="false"
                                                        Width="260px" meta:resourcekey="txtOwnerResource1" TabIndex="16" AutoPostBack="false"></asp:TextBox>
                                                </td>
                                                <td class="ControlTD" align="left" valign="top">
                                                    <asp:HyperLink ID="hlSearch" runat="server" ImageUrl="images/search.gif" meta:resourcekey="hlSearchResource1"
                                                        NavigateUrl="javascript:searchOriginatorPopup();" TabIndex="15" Text="Search"
                                                        Style="border: 0;"></asp:HyperLink>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 21px; text-align: left; height: 32px">
                                        <asp:Label ID="lblApplManage" runat="server" CssClass="FieldName" Width="145px" Height="23px"
                                            meta:resourcekey="lblApplManageResource1"></asp:Label>
                                    </td>
                                    <td align="left" style="height: 32px; width: 635px;" valign="top">
                                        <asp:DropDownList ID="ddlApplManage" runat="server" Width="196px" TabIndex="8" CssClass="dropdownText"
                                            meta:resourcekey="ddlApplManageResource1">
                                            <asp:ListItem Value="-1" meta:resourcekey="ListItemResource10"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="reqApplManageID" runat="server" 
                                    ControlToValidate="ddlApplManage" CssClass="ErrValStyle" 
                                    EnableClientScript="true" ErrorMessage="Select Application ManageID" 
                                    InitialValue="0" ValidationGroup="grpAppl"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 21px; height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 635px;" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 205px">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="height: 30px; width: 398px;">
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="9" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    &nbsp;
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                                        OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" TabIndex="10" meta:resourcekey="ibResetResource1">
                                                    </igtxt:WebImageButton>
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
                                    <td style="width: 21px; height: 27px;">
                                        &nbsp;
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
                        <td style="background-image: url(images/table_right_middle.gif); height: 199px; width: 6px;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="width: 100%;" valign="top">
                            <table style="width: 100%; height: 315px;" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 19px; width: 135%;" valign="top">
                                        <table width="100%" style="vertical-align: top">
                                            <tr>
                                                <td style="width: 100%" align="left" valign="top">
                                                    <ig:WebDataGrid ID="grdAppl" runat="server" AutoGenerateColumns="False" DataKeyFields="ApplID"
                                                        Width="98%" OnItemCommand="grdAppl_ItemCommand" OnDataFiltered="grdAppl_DataFiltered"
                                                        OnDataBound="grdAppl_DataBound" HeaderCaptionCssClass="GridHeader" TabIndex="13"
                                                        Height="300px">
                                                        <Columns>
                                                            <ig:BoundDataField DataFieldName="ApplName" Key="ApplicationName">
                                                                <Header Text="Application Name" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="ApplType" Key="ApplType">
                                                                <Header Text="Application Type" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="ApplStatus" Key="ApplStatus">
                                                                <Header Text="Application Status" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="Owner" Key="Owner">
                                                                <Header Text="Owner" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                        CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ApplID") %>'
                                                                        CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                                <Header Text="Edit" />
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ApplID") %>'
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
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="valBUSummary" runat="server" CssClass="ErrValSummary"
                                ShowSummary="False" meta:resourcekey="valBUSummaryResource1" />
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
    <input id="hdnOwnerID" runat="server" style="width: 18px" type="hidden" />
    <input id="hdnOwnerName" runat="server" style="width: 25px" type="hidden" />
</asp:Content>
