<%@ Page Language="C#" StylesheetTheme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="Sites.aspx.cs" Inherits="Sites" Title="Site"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_Site" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script type="text/javascript">
    //<!--
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);

        }
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
    // -->
    </script>
    <table id="Table1" border="0" cellpadding="1" cellspacing="1" width="100% ">
        <tr>
            <td style="background-image: url(images/table_left_middle.gif); height: 223px;">
            </td>
            <td valign="top" style="height: 223px">
                <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                    border="0">
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 30px;">
                            <asp:Label ID="lblSite" runat="server" CssClass="FieldName" Width="93px" meta:resourcekey="lblSiteResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="height: 30px" valign="top">
                            <asp:TextBox ID="txtSite" runat="server" Width="227px" CssClass="FieldValue" MaxLength="25"
                                meta:resourcekey="txtSiteResource1" TabIndex="1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqSiteVal" runat="server" CssClass="ErrValStyle"
                                ControlToValidate="txtSite" meta:resourcekey="reqSiteValResource1"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revSite" runat="server" ControlToValidate="txtSite"
                                CssClass="ErrValStyle" ValidationExpression="^[\w0-9\-\.]+([\w0-9\-\.]+)*$" Display="dynamic"
                                Height="15px" Width="166px" meta:resourcekey="revSiteResource1"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 56px;">
                            <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="93px" meta:resourcekey="lblDescResource1"> </asp:Label>
                        </td>
                        <td class="ControlTD" style="height: 56px" valign="top">
                            <asp:TextBox ID="txtDesc" runat="server" Width="230px" CssClass="FieldValue" MaxLength="150"
                                TabIndex="2" TextMode="MultiLine" Height="71px" meta:resourcekey="txtDescResource1"></asp:TextBox><br />
                            <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                Display="Dynamic" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 11px;">
                        </td>
                        <td align="left" class="ControlTD" style="height: 11px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 30px;">
                            <asp:Label ID="lblRegion" runat="server" CssClass="FieldName" Width="93px" meta:resourcekey="lblRegionResource1"> </asp:Label>
                        </td>
                        <td align="left" class="ControlTD" style="height: 30px" valign="top">
                            <asp:DropDownList ID="ddlRegion" runat="server" Width="193px" CssClass="dropdownText"
                                OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" TabIndex="3" AutoPostBack="true">
                                <asp:ListItem Selected="True" Value="0">-Select-</asp:ListItem>
                                <asp:ListItem>APJ</asp:ListItem>
                                <asp:ListItem>EMEA</asp:ListItem>
                                <asp:ListItem>AMERICAS</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 30px;">
                            <asp:Label ID="lblCountry" runat="server" CssClass="FieldName" Width="93px" meta:resourcekey="lblCountryResource1"> </asp:Label>
                        </td>
                        <td align="left" class="ControlTD" style="height: 30px" valign="top">
                            <asp:DropDownList ID="ddlCountry" runat="server" Width="192px" CssClass="dropdownText"
                                OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" TabIndex="4" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 30px;">
                            <asp:Label ID="lblCity" runat="server" CssClass="FieldName" Width="93px" meta:resourcekey="lblCityResource1"> </asp:Label>
                        </td>
                        <td align="left" class="ControlTD" style="height: 30px" valign="top">
                            <asp:DropDownList ID="ddlCity" runat="server" Width="193px" CssClass="dropdownText"
                                TabIndex="5">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 58px;">
                            &nbsp;
                        </td>
                        <td align="left" valign="top">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 58px;">
                        </td>
                        <td align="left" valign="top">
                            <table cellpadding="0" cellspacing="0" style="width: 205px">
                                <tr>
                                    &nbsp;
                                    <td align="left" colspan="2" valign="top" style="height: 30px">
                                        <%-- <td>--%>
                                        <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                            TabIndex="3" SkinID="uwButton" ImageDirectory="" Height="39px" meta:resourcekey="ibCreateResource1">
                                        </igtxt:WebImageButton>
                                        <%-- </td>
                                        <td--%>
                                        <%-- &nbsp;--%>
                                        <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                            OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" TabIndex="4" Height="40px"
                                            meta:resourcekey="ibResetResource1">
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%-- <td colspan="3">--%>
                                        <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="492px" meta:resourcekey="lblMessageResource1"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 58px;">
                        </td>
                        <td align="right" valign="bottom">
                            <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
                            <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                TabIndex="5" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                meta:resourcekey="ibExportToExcelResource1">
                                <Appearance>
                                    <Image Url="./icons/excelsmall.gif" />
                                </Appearance>
                            </igtxt:WebImageButton>
                            <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                TabIndex="6" UseBrowserDefaults="False" CausesValidation="False" Focusable="False"
                                ImageDirectory="" Height="28px" meta:resourcekey="ibDeleteResource1">
                                <ClientSideEvents Click="ibDelete_Click" />
                            </igtxt:WebImageButton>
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td style="background-image: url(images/table_right_middle.gif); height: 223px; width: 6px;">
            </td>
        </tr>
        <tr>
            <td style="background-image: url(images/table_left_middle.gif);">
            </td>
            <td style="width: 100%">
                <table style="width: 100%" cellspacing="0" cellpadding="0">
                    <tr style="width: 100%">
                        <td style="height: 19px">
                            <table style="width: 100%">
                                <tr style="width: 100%">
                                    <td style="width: 100%;" valign="top">
                                        <ig:WebDataGrid ID="grdSite" runat="server" AutoGenerateColumns="False" DataKeyFields="SiteID"
                                            Width="98%" EnableRelativeLayout="True" Height="300px" OnItemCommand="grdSite_ItemCommand"
                                            OnDataBound="grdSite_DataBound" OnDataFiltered="grdSite_DataFiltered" HeaderCaptionCssClass="GridHeader"
                                            TabIndex="7" OnInitializeRow="grdSite_InitializeRow">
                                            <Columns>
                                                <ig:BoundDataField DataFieldName="Site" Key="Site">
                                                    <Header Text="Site" />
                                                </ig:BoundDataField>
                                                <ig:BoundDataField DataFieldName="Country" Key="Country">
                                                    <Header Text="Country" />
                                                </ig:BoundDataField>
                                                <ig:BoundDataField DataFieldName="City" Key="City">
                                                    <Header Text="City" />
                                                </ig:BoundDataField>
                                                <ig:TemplateDataField Key="Edit">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                            CommandArgument='<%# Eval("SiteID") %>' CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                    </ItemTemplate>
                                                    <Header Text="Edit" />
                                                </ig:TemplateDataField>
                                                <ig:TemplateDataField Key="Delete">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                            meta:resourcekey="chkDeleteResource1" />
                                                        <asp:Label ID="lblDeleteID" runat="server" Text='<%# Bind("SiteID") %>' Visible="False"
                                                            meta:resourcekey="lblDeleteIDResource1"></asp:Label>
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
                                                <ig:Paging PagerAppearance="Top" PageSize="10" PagerCssClass="igg_CustomPager" Enabled="true">
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
                                                <ig:Filtering>
                                                </ig:Filtering>
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
                <asp:ValidationSummary ID="valSiteSummary" runat="server" CssClass="ErrValSummary"
                    ShowSummary="False" ValidationGroup="valSiteGroup" meta:resourcekey="valSiteSummaryResource1" />
            </td>
            <td style="background-image: url(images/table_right_middle.gif); height: 100%; width: 6px;">
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
    <input type="hidden" id="hdnFilterCount" runat="server" />
</asp:Content>
