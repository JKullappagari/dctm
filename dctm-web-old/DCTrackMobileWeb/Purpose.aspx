<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="Purpose.aspx.cs" Inherits="Purpose" Title="Purpose"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_Purpose" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
    <script id="Infragistics" type="text/javascript">
        // <!--
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }
        function ibCreate_JS_Click(oButton, oEvent) {

            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
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
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
        <tr>
            <td valign="top" style="width: 100%">
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif); height: 223px;">
                        </td>
                        <td valign="top" style="height: 223px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr style="height: 15px;">
                                    <td class="labelTD" style="width: 58px; height: 31px;">
                                        <asp:Label ID="lblPurpose" runat="server" CssClass="FieldName" Width="93px" meta:resourcekey="lblPurposeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 34px">
                                        <asp:TextBox ID="txtPurpose" runat="server" Width="227px" CssClass="FieldValue" MaxLength="25"
                                            Height="15px" TabIndex="1" meta:resourcekey="txtPurposeResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqPurposeVal" runat="server" CssClass="ErrValStyle"
                                            ControlToValidate="txtPurpose" meta:resourcekey="reqPurposeValResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revPurpose" runat="server" ControlToValidate="txtPurpose"
                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[\w0-9\-\.]+$"
                                            meta:resourcekey="revPurposeResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 25px;">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="74px" meta:resourcekey="lblDescResource1"> </asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 36px" valign="top">
                                        <asp:TextBox ID="txtDesc" runat="server" Width="230px" CssClass="FieldValue" MaxLength="150"
                                            TabIndex="2" TextMode="MultiLine" Height="98px" meta:resourcekey="txtDescResource1"></asp:TextBox><br />
                                        <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                            Display="Dynamic" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 205px">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="height: 30px">
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="3" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="silver_button.GIF"></RoundedCorners>
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                                        OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibResetResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="silver_button.GIF"></RoundedCorners>
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="492px" meta:resourcekey="lblMessageResource1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 27px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 27px" align="right">
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                            TabIndex="4" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibExportToExcelResource1">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="ibDelete" runat="server" CausesValidation="False" OnClick="ibDelete_Click"
                                            SkinID="uwButton" ImageDirectory="" TabIndex="5" UseBrowserDefaults="False" meta:resourcekey="ibDeleteResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="20" MaxWidth="63" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <Appearance>
                                                <style cursor="Hand" font-names="Arial" font-size="8pt">
                                                    
                                                </style>
                                                <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                </ButtonStyle>
                                            </Appearance>
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
                                                <td style="width: 100%">
                                                    <ig:WebDataGrid ID="grdPurpose" runat="server" AutoGenerateColumns="False" DataKeyFields="PurposeID"
                                                        OnDataFiltered="grdPurpose_DataFiltered" Width="98%" EnableRelativeLayout="True"
                                                        OnItemCommand="grdPurpose_ItemCommand" OnDataBound="grdPurpose_DataBound" Height="300px"
                                                        HeaderCaptionCssClass="GridHeader" TabIndex="6" OnInitializeRow="grdPurpose_InitializeRow">
                                                        <Columns>
                                                            <ig:BoundDataField DataFieldName="Purpose" Key="Purpose">
                                                                <Header Text="Purpose" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="Description" Key="Description">
                                                                <Header Text="Description" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                        CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "PurposeID") %>'
                                                                        CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                                <Header Text="Edit" />
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "PurposeID") %>'
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
                            <asp:ValidationSummary ID="valPurposeSummary" runat="server" CssClass="ErrValSummary"
                                ShowSummary="False" meta:resourcekey="valPurposeSummaryResource1" />
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
