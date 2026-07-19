<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="MusterReason.aspx.cs" Inherits="MusterReason"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_MusterReason" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <script id="Infragistics" type="text/javascript">
        //<!--
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
                        <td style="background-image: url(images/table_left_middle.gif); height: 199px;">
                        </td>
                        <td valign="top" style="height: 199px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr style="height: 15px;">
                                    <td class="labelTD" style="width: 58px; height: 34px;">
                                        <asp:Label ID="lblAssetType" runat="server" CssClass="FieldName" Width="129px" meta:resourcekey="lblAssetTypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 34px">
                                        <asp:TextBox ID="txtMusterReason" runat="server" Width="227px" CssClass="FieldValue"
                                            MaxLength="25" Height="16px" TabIndex="1" meta:resourcekey="txtMusterReasonResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqBUVal" runat="server" CssClass="ErrValStyle" ControlToValidate="txtMusterReason"
                                            TabIndex="7" meta:resourcekey="reqBUValResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revMuster" runat="server" ControlToValidate="txtMusterReason"
                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$"
                                            meta:resourcekey="revReasonResource1" Width="166px"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 36px;">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="129px" meta:resourcekey="lblDescResource1"> </asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 36px" valign="top">
                                        <table cellspacing="0">
                                            <tr>
                                                <td class="ControlTD" style="height: 36px" valign="top">
                                                    <asp:TextBox ID="txtDesc" runat="server" Width="226px" CssClass="FieldValue" MaxLength="150"
                                                        TabIndex="2" TextMode="MultiLine" Height="98px" meta:resourcekey="txtDescResource1"></asp:TextBox><br />
                                                    <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                                        Display="Dynamic" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                                                </td>
                                                <td class="labelTD" style="width: 58px; height: 36px;">
                                                    <table cellspacing="0" border="0">
                                                        <tr>
                                                            <td class="labelTD" style="width: 200px; height: 36px;">
                                                                &nbsp;
                                                            </td>
                                                            <td class="labelTD" style="width: 58px; height: 36px;">
                                                                &nbsp;
                                                            </td>
                                                            <td class="labelTD" style="width: 200px; height: 36px; vertical-align: middle;">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="labelTD" style="width: 200px; height: 36px;">
                                                                &nbsp;
                                                            </td>
                                                            <td class="labelTD" style="width: 58px; height: 36px;">
                                                                &nbsp;
                                                            </td>
                                                            <td class="labelTD" style="width: 200px; height: 36px; vertical-align: middle;">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 205px">
                                            <tr>
                                                <%--<td>--%>
                                                <td align="left" colspan="2" valign="top" style="height: 30px; width: 398px;">
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="3" SkinID="uwButton" ImageDirectory="" Height="49px" meta:resourcekey="ibCreateResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="silver_button.GIF"></RoundedCorners>
                                                        <Appearance>
                                                            <style cursor="Hand" font-size="8pt" font-names="Arial">
                                                                
                                                            </style>
                                                            <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                            </ButtonStyle>
                                                        </Appearance>
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    <%-- </td>
                                                      <%--  &nbsp;--%>
                                                    <%-- <td>--%>
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                                        OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" Height="50px" meta:resourcekey="ibResetResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="silver_button.GIF"></RoundedCorners>
                                                        <Appearance>
                                                            <style cursor="Hand" font-size="8pt" font-names="Arial">
                                                                
                                                            </style>
                                                            <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                            </ButtonStyle>
                                                        </Appearance>
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
                                    <td style="width: 58px; height: 27px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 27px" align="right">
                                        <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                            TabIndex="9" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibExportToExcelResource1">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                            TabIndex="5" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibDeleteResource1">
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
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 199px; width: 6px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif);">
                        </td>
                        <td style="width: 100%;" align="center">
                            <table style="width: 100%;" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 19px; width: 135%;">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%" align="left">
                                                    <ig:WebDataGrid ID="grdMusteringReason" runat="server" AutoGenerateColumns="False"
                                                        OnDataFiltered="grdMusteringReason_DataFiltered" DataKeyFields="MusterReasonID"
                                                        Width="98%" EnableRelativeLayout="True" OnItemCommand="grdMusteringReason_ItemCommand"
                                                        OnDataBound="grdMusteringReason_DataBound" HeaderCaptionCssClass="GridHeader"
                                                        Height="300px" OnInitializeRow="grdMusteringReason_InitializeRow">
                                                        <Columns>
                                                            <ig:BoundDataField DataFieldName="MusterReason" Key="MusterReason">
                                                                <Header Text="Reason" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="Description" Key="Description">
                                                                <Header Text="Description" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                        CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "MusterReasonID") %>'
                                                                        CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                                <Header Text="Edit" />
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "MusterReasonID") %>'
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
                            <asp:ValidationSummary ID="valMusterSummary" runat="server" CssClass="ErrValSummary"
                                ShowSummary="False" meta:resourcekey="valMusterSummaryResource1" />
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
