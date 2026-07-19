<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="ApplicationCriticality.aspx.cs" Inherits="ApplicationCriticality"
    Title="Application Criticality" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_AssetType" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <script type="text/javascript" src="scripts/jscolor/jscolor.js"></script>
    <script id="Infragistics" type="text/javascript">
   // <!--
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
        
        //-->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;
        text-align: center;">
        <tr>
            <td valign="top" style="width: 100%">
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                        </td>
                        <td valign="top" style="height: 199px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr style="height: 15px;">
                                    <td class="labelTD" style="height: 34px;" align="left">
                                        <asp:Label ID="lblApplCriticality" runat="server" CssClass="FieldName" Width="160px"
                                            meta:resourcekey="lblApplCriticalityResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 34px">
                                        <asp:TextBox ID="txtApplCriticality" runat="server" Width="227px" CssClass="FieldValue"
                                            MaxLength="50" Height="16px" TabIndex="1" meta:resourcekey="txtApplCriticalityResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqApplCriticality" runat="server" CssClass="ErrValStyle"
                                            ControlToValidate="txtApplCriticality" meta:resourcekey="reqApplCriticalityResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revAppCri" runat="server" ControlToValidate="txtApplCriticality"
                                            CssClass="ErrValStyle" ValidationExpression="^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$"
                                            Display="Dynamic" Height="15px" Width="166px" meta:resourcekey="revAppCriResource1"></asp:RegularExpressionValidator>
                                    </td>
                                    <td style="width: 100%; height: 5px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="height: 59px;" align="left">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="129px" meta:resourcekey="lblDescResource1"> </asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 59px" valign="top" align="left" colspan="2">
                                        <asp:TextBox ID="txtDesc" runat="server" Width="226px" CssClass="FieldValue" MaxLength="150"
                                            TabIndex="2" TextMode="MultiLine" Height="66px" meta:resourcekey="txtDescResource1"></asp:TextBox>
                                        <br />
                                        <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                            Display="Dynamic" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="height: 36px;" align="left">
                                        <asp:Label ID="lblBackColor" runat="server" CssClass="FieldName" Width="129px" meta:resourcekey="lblBackColorResource1"> </asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 36px" valign="top" align="left">
                                        <input class="color" name="backColorVal" id="backColorVal" value="" maxlength="6"/>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 400px">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="height: 30px; width: 398px;">
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="6" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    &nbsp;
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                                        OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" TabIndex="7" meta:resourcekey="ibResetResource1">
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="357px" meta:resourcekey="lblMessageResource1"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 27px;">
                                    </td>
                                    <td style="height: 27px" align="right" colspan="2">
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                                        TabIndex="8" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                                        meta:resourcekey="ibExportToExcelResource1">
                                                        <Appearance>
                                                            <Image Url="./icons/excelsmall.gif" />
                                                        </Appearance>
                                                    </igtxt:WebImageButton>
                                                </td>
                                                <td>
                                                    <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                                        TabIndex="9" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
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
                        <td style="background-image: url(images/table_left_middle.gif);">
                        </td>
                        <td style="width: 100%;" align="center">
                            <table style="width: 100%;" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 19px; width: 135%;">
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 100%; margin-left: 40px;" align="left">
                                                    <ig:WebDataGrid ID="grdApplCriticality" runat="server" AutoGenerateColumns="False"
                                                        DataKeyFields="ApplCriticalityID" Width="98%" EnableRelativeLayout="True" OnItemCommand="grdApplCriticality_ItemCommand"
                                                        OnDataFiltered="grdApplCriticality_DataFiltered" OnDataBound="grdApplCriticality_DataBound"
                                                        HeaderCaptionCssClass="GridHeader" Height="300px" TabIndex="7">
                                                        <Columns>
                                                            <ig:BoundDataField DataFieldName="ApplCriticality" Key="ApplCriticality">
                                                                <Header Text="Application Criticality" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="ApplCriticalityDesc" Key="ApplCriticalityDesc">
                                                                <Header Text="Description" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                        CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ApplCriticalityID") %>'
                                                                        CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                                <Header Text="Edit" />
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ApplCriticalityID") %>'
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
                            <asp:ValidationSummary ID="valBUSummary" runat="server" CssClass="ErrValSummary"
                                ShowSummary="False" meta:resourcekey="valBUSummaryResource1" />
                            <ig:WebExcelExporter ID="eExporter" runat="server" OnCellExported="eExporter_CellExported" />
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
