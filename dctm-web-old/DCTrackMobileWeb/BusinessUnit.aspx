<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="BusinessUnit.aspx.cs" Inherits="BusinessUnit"
    Title="Business Unit" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_BusinessUnit" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">

    <script id="Infragistics" type="text/javascript">
    <!--
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
                        <td style="background-image: url(images/table_left_middle.gif);">
                        </td>
                        <td valign="top">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr style="height: 15px;">
                                    <td class="labelTD" style="width: 74px; height: 34px; text-align: left;">
                                        <asp:Label ID="lblBU" runat="server" CssClass="FieldName" Width="100px" 
                                            meta:resourcekey="lblBUResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 34px" align="left">
                                        <asp:TextBox ID="txtBU" runat="server" Width="227px" CssClass="FieldValue" MaxLength="25"
                                            Height="16px" TabIndex="1" meta:resourcekey="txtBUResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqBUVal" runat="server" CssClass="ErrValStyle" ControlToValidate="txtBU"
                                             TabIndex="7" 
                                            ValidationGroup="valBUSummary" meta:resourcekey="reqBUValResource1"></asp:RequiredFieldValidator>
                                        <%--<asp:RegularExpressionValidator ID="regRootEntityVal" runat="server" ControlToValidate="txtBU"
                                CssClass="ErrValStyle" Display="Dynamic" ErrorMessage="Invalid Root Entity Name - use alphabets/digits and no spaces allowed"
                                ValidationExpression="^[A-Za-z0-9]+" Height="15px" Width="166px" ValidationGroup="valBUSummary"></asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 74px; text-align: left; height: 58px;">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="124px" 
                                            meta:resourcekey="lblDescResource1"> </asp:Label>
                                    </td>
                                    <td class="ControlTD" valign="top" style="height: 58px">
                                        <table cellspacing="0">
                                            <td class="ControlTD" valign="top">
                                                <asp:TextBox ID="txtDesc" runat="server" Width="486px" CssClass="FieldValue" MaxLength="255"
                                                    TabIndex="2" TextMode="MultiLine" Height="43px" 
                                                    meta:resourcekey="txtDescResource1"></asp:TextBox>
                                            </td>
                                            <td class="labelTD" style="width: 58px;">
                                            </td>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 74px; height: 7px; text-align: left;">
                                        <asp:Label ID="lblCoPrefix" runat="server" CssClass="FieldName" Width="128px" 
                                            meta:resourcekey="lblCoPrefixResource1"></asp:Label>
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        <asp:TextBox ID="txtCoPrefix" runat="server" Width="102px" CssClass="FieldValue"
                                            MaxLength="2" Height="16px" TabIndex="3" 
                                            meta:resourcekey="txtCoPrefixResource1"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            CssClass="ErrValStyle" ControlToValidate="txtCoPrefix"
                                           ValidationGroup="valBUSummary" 
                                            meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                        <%--<asp:RegularExpressionValidator ID="NUmericValidotor" runat="server" ControlToValidate="txtCoPrefix"
                                            ErrorMessage="Company Prefix should be +ve Integer" ValidationExpression="^\d+$"
                                            CssClass="ErrValStyle" ValidationGroup="valBUSummary"></asp:RegularExpressionValidator>--%>
                                            <asp:RegularExpressionValidator ID="regRootEntityVal" runat="server" ControlToValidate="txtCoPrefix"
                                CssClass="ErrValStyle" Display="Dynamic" 
                                ValidationExpression="^[0-9]+" Height="15px" Width="166px" 
                                            ValidationGroup="valBUSummary" meta:resourcekey="regRootEntityValResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 74px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px" align="left" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 74px; height: 67px;">
                                    </td>
                                    <td style="height: 67px" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 205px">
                                            <tr>
                                            
                                               <td align="left" colspan="2" valign="top" style="width: 398px;">
                                                    <%--<td>--%>
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False"
                                                         OnClick="ibCreate_Click" TabIndex="4" SkinID="uwButton" 
                                                            ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                   <%-- </td>--%>
                                                    <%--&nbsp;--%>
                                                   <%-- <td>--%>
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False"
                                                        CausesValidation="False" OnClick="ibReset_Click" SkinID="uwButton"
                                                        ImageDirectory="" TabIndex="5" meta:resourcekey="ibResetResource1">
                                                    </igtxt:WebImageButton>
                                               <%-- </td>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td>
                                           
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="357px" 
                                                        meta:resourcekey="lblMessageResource1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 74px;">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <ig:WebExcelExporter runat="server" ID="eExporter" 
                                            OnCellExported="eExporter_CellExported" />
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                            TabIndex="6" UseBrowserDefaults="False" CausesValidation="False"
                                            ImageDirectory="" meta:resourcekey="ibExportToExcelResource1">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                            TabIndex="5" UseBrowserDefaults="False"
                                            CausesValidation="False" Focusable="False" ImageDirectory="" 
                                            meta:resourcekey="ibDeleteResource1">
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
                                                        <ig:WebDataGrid ID="grdBU" runat="server" AutoGenerateColumns="False" DataKeyFields="BusinessUnitID"
                                                            Width="98%" EnableRelativeLayout="True" OnItemCommand="grdBU_ItemCommand" OnDataBound="grdBU_DataBound"
                                                            HeaderCaptionCssClass="GridHeader" TabIndex="7" OnInitializeRow="grdBU_InitializeRow">
                                                            <Columns>
                                                                <ig:BoundDataField DataFieldName="BusinessUnit" Key="BusinessUnit">
                                                                    <Header Text="BusinessUnit" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="Description" Key="Description">
                                                                    <Header Text="Description" />
                                                                </ig:BoundDataField>
                                                                <ig:BoundDataField DataFieldName="CoPrefix" Key="CoPrefix">
                                                                    <Header Text="CoPrefix" />
                                                                </ig:BoundDataField>
                                                                <ig:TemplateDataField Key="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                            CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "BusinessUnitID") %>'
                                                                            CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                    </ItemTemplate>
                                                                    <Header Text="Edit" />
                                                                </ig:TemplateDataField>
                                                                <ig:TemplateDataField Key="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkDelete" runat="server" 
                                                                            onclick="javascript:SelectItemCheckbox(this,'chkAll');" 
                                                                            meta:resourcekey="chkDeleteResource1" />
                                                                        <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "BusinessUnitID") %>'
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
                                                                <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
                                                                    <PagerTemplate>
                                                                        <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
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
                            <asp:ValidationSummary ID="valBUSummary" runat="server" CssClass="ErrValSummary"
                               visible=false
                                meta:resourcekey="valBUSummaryResource1" />
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
