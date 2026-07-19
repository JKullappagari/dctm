<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="LocationType.aspx.cs" Inherits="LocationType"
    Title="Location Type" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_LocationType" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">

    <script id="Infragistics" type="text/javascript">
    //<!--
        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }
        function ibCreate_JS_Click(oButton, oEvent) {

            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
        }
    // -->
    </script>

    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
        <tr>
            <td valign="top" style="width: 100%">
                &nbsp;
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif); height: 223px;">
                        </td>
                        <td valign="top" style="height: 223px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr style="height: 15px;">
                                    <td class="labelTD" style="width: 58px; height: 31px;">
                                        <asp:Label ID="lblLocationType" runat="server" CssClass="FieldName" 
                                            Width="120px" meta:resourcekey="lblLocationTypeResource1"> 
                             </asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 34px">
                                        <asp:TextBox ID="txtLocationType" runat="server" Width="227px" CssClass="FieldValue"
                                            MaxLength="25" Height="15px" TabIndex="1" 
                                            meta:resourcekey="txtLocationTypeResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqLocationTypeVal" runat="server" CssClass="ErrValStyle"
                                            ControlToValidate="txtLocationType" 
                                            ValidationGroup="valLocationTypeSummary" 
                                            meta:resourcekey="reqLocationTypeValResource1"></asp:RequiredFieldValidator>
                                        <%--<asp:RegularExpressionValidator ID="regLocationTypeVal" runat="server" ControlToValidate="txtLocationType"
                                CssClass="ErrValStyle" Display="Dynamic" ErrorMessage="Invalid Type - use alphabets/digits and no spaces allowed"
                                ValidationExpression="^[A-Za-z0-9]+" Height="15px" Width="166px" ValidationGroup="valLocationTypeSummary" ></asp:RegularExpressionValidator>        --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 25px;">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="120px" 
                                            meta:resourcekey="lblDescResource1">  </asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 36px" valign="top">
                                        <asp:TextBox ID="txtDesc" runat="server" Width="230px" CssClass="FieldValue" MaxLength="255"
                                            TabIndex="2" TextMode="MultiLine" Height="98px" 
                                            meta:resourcekey="txtDescResource1"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 25px;">
                                        <asp:Label ID="lblStorageType" runat="server" CssClass="FieldName" 
                                             meta:resourcekey="lblStorageTypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 36px" valign="top">
                                        <asp:RadioButtonList ID="rdoStorageType" runat="server" RepeatDirection="Horizontal"
                                            CssClass="FieldName" Height="11px" TabIndex="3" 
                                            meta:resourcekey="rdoStorageTypeResource1">
                                            <asp:ListItem Value="1" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 25px;">
                                        <asp:Label ID="lblRfidLocation" runat="server" CssClass="FieldName" 
                                             meta:resourcekey="lblRfidLocationResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 36px" valign="top">
                                        <asp:RadioButtonList ID="rdoRfidLocation" runat="server" RepeatDirection="Horizontal"
                                            CssClass="FieldName" Height="11px" TabIndex="3" 
                                            meta:resourcekey="rdoRfidLocationResource1">
                                            <asp:ListItem Value="1" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                        </asp:RadioButtonList>
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
                                                <td align="left" colspan="2" valign="top" style="height: 30px">
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server"  UseBrowserDefaults="False"
                                                         OnClick="ibCreate_Click" TabIndex="3" SkinID="uwButton" 
                                                    ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="silver_button.GIF"></RoundedCorners>
                                                        <Appearance>
                                                            <style cursor="Hand" font-size="8pt" font-names="Arial">
                                                                </style>
<ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt"></ButtonStyle>
                                                        </Appearance>
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    <%--</td>
                                                    <%--&nbsp;--%>
                                                    <%--<td>--%>
                                                    <igtxt:WebImageButton ID="ibReset" runat="server"  UseBrowserDefaults="False"
                                                        CausesValidation="False" OnClick="ibReset_Click" SkinID="uwButton"
                                                        ImageDirectory="" meta:resourcekey="ibResetResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="silver_button.GIF"></RoundedCorners>
                                                        <Appearance>
                                                            <style cursor="Hand" font-size="8pt" font-names="Arial">
                                                                </style>
<ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt"></ButtonStyle>
                                                        </Appearance>
                                                    </igtxt:WebImageButton>
                                               </td>
                                            </tr>
                                            <tr>
                                               <td>
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="492px" 
                                                        meta:resourcekey="lblMessageResource1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 27px;">
                                        <ig:WebExcelExporter runat="server" ID="eExporter" 
                                            OnCellExported="eExporter_CellExported" />
                                    </td>
                                    <td style="height: 27px" align="right">
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                            TabIndex="9" UseBrowserDefaults="False" CausesValidation="False"
                                            ImageDirectory="" meta:resourcekey="ibExportToExcelResource1">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="ibDelete" runat="server" CausesValidation="False" OnClick="ibDelete_Click"
                                            SkinID="uwButton" ImageDirectory="" TabIndex="5" 
                                            UseBrowserDefaults="False" meta:resourcekey="ibDeleteResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="20" MaxWidth="63" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <Appearance>
                                                <style cursor="Hand" font-names="Arial" font-size="8pt">
                                                    </style>
<ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt"></ButtonStyle>
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
                                                    <ig:WebDataGrid ID="grdLocationType" runat="server" AutoGenerateColumns="False" DataKeyFields="LocationTypeID"
                                                        Width="100%" EnableRelativeLayout="True" OnItemCommand="grdLocationType_ItemCommand"
                                                        OnDataBound="grdLocationType_DataBound" HeaderCaptionCssClass="GridHeader" 
                                                        OnInitializeRow="grdLocationType_InitializeRow">
                                                        <Columns>
                                                            <ig:BoundDataField DataFieldName="LocationType" Key="LocationType">
                                                                <Header Text="LocationType" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="Description" Key="Description">
                                                                <Header Text="Description" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="IsStorageType">
                                                                <Header Text="IsStorageType" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="IsStorageType" runat="server" 
                                                                        Text='<%# Bind("IsStorageType") %>' meta:resourcekey="IsStorageTypeResource1"></asp:Label>
                                                                </ItemTemplate>
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="IsRfidLocation">
                                                                <Header Text="IsRfidLocation" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="IsRfidLocation" runat="server" 
                                                                        Text='<%# Bind("IsRfidLocation") %>' meta:resourcekey="IsRfidLocationResource1"></asp:Label>
                                                                </ItemTemplate>
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                        CommandArgument='<%# Eval("LocationTypeID") %>' CommandName="Edit" 
                                                                        ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                                <Header Text="Edit" />
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" 
                                                                        onclick="javascript:SelectItemCheckbox(this,'chkAll');" 
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# Bind("LocationTypeID") %>' 
                                                                        Visible="False" meta:resourcekey="lblDeleteIDResource1"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderTemplate>
                                                                    <input id="chkAll" runat="server" onclick="javascript:SelectAllCheckboxes(this,'chkDelete');"
                                                                        type="checkbox" /> Select All
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
                                                                    <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
                                                                </PagerTemplate>
                                                            </ig:Paging>
                                                        </Behaviors>
                                                    </ig:WebDataGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="valLocationTypeSummary" runat="server" CssClass="ErrValSummary"
                                 ShowSummary="False" 
                                meta:resourcekey="valLocationTypeSummaryResource1" />
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 100%; width: 6px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
</asp:Content>
