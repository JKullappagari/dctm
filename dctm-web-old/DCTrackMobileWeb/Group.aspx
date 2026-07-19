<%@ Page Language="C#" Theme="SkinFile" MaintainScrollPositionOnPostback="true" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="Group.aspx.cs" Inherits="ASPX_Group" Title="Group"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_Group" ContentPlaceHolderID="Master_ContentPlaceHolder"
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
        function wibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            //	return ValidateDeletion(oButton,oEvent);
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }
       

        function ibCreate_Click(oButton, oEvent) {
            //Add code to handle your event here.
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
        }
        // -->
    </script>
    <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td style="height: 19px;">
                <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr style="height: 15px;">
                        <td class="labelTD" style="width: 58px; height: 38px;">
                            <asp:Label ID="lblGroup" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblGroupResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="height: 34px; width: 817px;">
                            <asp:TextBox ID="txtGroup" runat="server" Width="227px" CssClass="FieldValue" MaxLength="50"
                                Height="17px" TabIndex="2" meta:resourcekey="txtGroupResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqGroupVal" runat="server" CssClass="ErrValStyle"
                                ControlToValidate="txtGroup" meta:resourcekey="reqGroupValResource1"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revGroup" runat="server" ControlToValidate="txtGroup"
                                CssClass="ErrValStyle" ValidationExpression="^[\w\-\.\:]+$"
                                Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revGroupResource1"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 36px;">
                            <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="74px" meta:resourcekey="lblDescResource1"> </asp:Label>
                        </td>
                        <td class="ControlTD" style="height: 36px; width: 817px;" valign="top">
                            <asp:TextBox ID="txtDesc" runat="server" Width="227px" CssClass="FieldValue" MaxLength="150"
                                TabIndex="3" TextMode="MultiLine" Height="67px" Rows="10" meta:resourcekey="txtDescResource1"></asp:TextBox><br />
                            <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                Display="Dynamic" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD" style="width: 58px; height: 20px">
                        </td>
                        <td align="left" style="height: 20px; width: 817px;" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 58px; height: 7px;">
                            &nbsp;
                        </td>
                        <td style="height: 7px; width: 817px;" align="left" valign="top">
                            <table cellpadding="0" cellspacing="0" style="width: 634px">
                                <tr>
                                    <td align="left" colspan="2" valign="top" style="height: 30px">
                                        <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                            TabIndex="4" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                            <RoundedCorners WidthOfRightEdge="13" RenderingType="FileImages" MaxWidth="63" MaxHeight="20"
                                                HeightOfBottomEdge="0"></RoundedCorners>
                                            <Appearance>
                                                <style cursor="Hand" font-size="8pt" font-names="Arial">
                                                    
                                                </style>
                                                <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                </ButtonStyle>
                                            </Appearance>
                                            <ClientSideEvents Click="ibCreate_Click" />
                                        </igtxt:WebImageButton>
                                        &nbsp;
                                        <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                            OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibResetResource1">
                                            <RoundedCorners WidthOfRightEdge="13" RenderingType="FileImages" MaxWidth="63" MaxHeight="20"
                                                HeightOfBottomEdge="0"></RoundedCorners>
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
                                        <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="450px" meta:resourcekey="lblMessageResource1"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 58px; height: 27px">
                            <input id="hdnGrpID" runat="server" style="width: 83px" type="hidden" />
                        </td>
                        <td align="right" style="height: 27px; width: 817px;">
                            <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
                            <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                TabIndex="5" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                meta:resourcekey="ibExportToExcelResource1">
                                <Appearance>
                                    <Image Url="./icons/excelsmall.gif" />
                                </Appearance>
                            </igtxt:WebImageButton>
                            <igtxt:WebImageButton ID="wibDelete" runat="server" UseBrowserDefaults="False" TabIndex="3"
                                SkinID="uwButton" CausesValidation="False" OnClick="wibDelete_Click" ImageDirectory=""
                                meta:resourcekey="wibDeleteResource1">
                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="20" MaxWidth="63" RenderingType="FileImages"
                                    WidthOfRightEdge="13" />
                                <Appearance>
                                    <style cursor="Hand" font-names="Arial" font-size="8pt">
                                        
                                    </style>
                                    <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                    </ButtonStyle>
                                </Appearance>
                                <ClientSideEvents Click="wibDelete_Click" />
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tr>
                        <td width="100%">
                            <ig:WebDataGrid ID="grdGroup" runat="server" AutoGenerateColumns="False" DataKeyFields="GroupID"
                                Width="98%" EnableRelativeLayout="True" OnItemCommand="grdGroup_ItemCommand"
                                Height="300px" OnDataBound="grdGroup_DataBound" HeaderCaptionCssClass="GridHeader"
                                OnInitializeRow="grdGroup_InitializeRow">
                                <Columns>
                                    <ig:BoundDataField DataFieldName="Group" Key="Group">
                                        <Header Text="User Group" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Description" Key="Description">
                                        <Header Text="Description" />
                                    </ig:BoundDataField>
                                    <ig:TemplateDataField Key="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                CommandArgument='<%# Eval("GroupId") %>' CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                        </ItemTemplate>
                                        <Header Text="Edit" />
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Key="Delete">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                meta:resourcekey="chkDeleteResource1" />
                                            <asp:Label ID="lblDeleteID" runat="server" Text='<%# Bind("GroupId") %>' Visible="False"
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
                                    <ig:Filtering>
                                    </ig:Filtering>
                                    <ig:Paging PagerAppearance="Top" PageSize="10" PagerCssClass="igg_CustomPager" Enabled="true">
                                        <PagerTemplate>
                                            <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
                                        </PagerTemplate>
                                    </ig:Paging>
                                </Behaviors>
                                <EmptyRowsTemplate>
                                    <div style="text-align: center;">
                                        <br />
                                        <br />
                                        <img src="images/WebDataGrid/attention.png" align="middle" alt="" />
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
    <asp:ValidationSummary ID="valGroupSummary" runat="server" CssClass="ErrValSummary"
        ShowSummary="False" meta:resourcekey="valGroupSummaryResource1" />
    <input type="hidden" id="hdnMessage" runat="server" />
</asp:Content>
