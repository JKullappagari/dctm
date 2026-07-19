<%@ Page  Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master" AutoEventWireup="true"
    Theme="SkinFile" CodeFile="Rpt_ExternalSys_Import_Export.aspx.cs" Inherits="Rpt_ExternalSys_Import_Export"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto"  %>

<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
    <%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
   <table style="height: 100%; width: 98%;" cellspacing="0" cellpadding="0" border="0">
        <%--<tr style="height: 15px;">
            <td class="labelTD" style="height: 34px;" colspan="9">
                <center>
                    <asp:Label ID="Label1" runat="server" CssClass="Heading" meta:resourcekey="Label1Resource1"></asp:Label>
                </center>
                <br />
                <br />
            </td>
        </tr>--%>
        <tr style="width: auto;">
            <td class="labelTD" align="left" width="100%" colspan="9">
                <div style="width: 100%;">
                 <ig:WebExplorerBar ID="wpSearchOptions" runat="server" GroupExpandAction="HeaderClick"
                        meta:resourcekey="wpSearchOptionsResource1" Width="100%" BorderWidth="1px">
                        <Groups>
                            <ig:ExplorerBarGroup Text="Search Options" Expanded="true">
                                <Items>
                                    <ig:ExplorerBarItem TemplateId="tmpAssetSearch">
                                    </ig:ExplorerBarItem>
                                </Items>
                            </ig:ExplorerBarGroup>
                        </Groups>
                        <Templates>
                            <ig:ItemTemplate TemplateID="tmpAssetSearch">
                        <Template>
                            <table id="Table1" align="center" border="0" cellpadding="0" cellspacing="2" style="border-right: gray 1px solid;
                                border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;
                                height: 46px;" width="100%">
                                <tr>
                                    <td class="labelTD" style="height: 17px; width: 180px;" valign="top">
                                        <asp:Label ID="lblTransactionType" runat="server" CssClass="FieldName" Width="88px"
                                            meta:resourcekey="lblTransactionTypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 17px; width: 132px;" valign="bottom">
                                        <asp:DropDownList ID="ddlTranType" runat="server" Style="margin-left: 0px" meta:resourcekey="ddlTranTypeResource1">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="labelTD" style="height: 17px;" valign="top">
                                        <asp:Label ID="lblStatus" runat="server" CssClass="FieldName" Width="89px"
                                            meta:resourcekey="lblStatusResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 17px;" valign="bottom">
                                        <asp:DropDownList ID="ddlStatus" runat="server" 
                                            meta:resourcekey="ddlStatusResource1" Width="113px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="labelTD" style="height: 17px; width: 155px;" valign="bottom">
                                        <asp:Label ID="lblDateOfUpdate" runat="server" CssClass="FieldName" Width="113px"
                                            meta:resourcekey="lblDateOfUpdateResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 17px;" valign="bottom">
                                        <ig:WebDatePicker ID="WebDatePickerDate" runat="server" DisplayModeFormat="d" Width="154px"
                                            meta:resourcekey="WebDatePickerDateResource1" Height="16px">
                                        </ig:WebDatePicker>
                                        
                                    </td>
                                    <td class="ControlTD" style="height: 17px;" valign="bottom">
                                        
                                    </td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="labelTD" style="height: 19px; width: 180px;">
                                        
                                    </td>
                                    <td class="ControlTD" style="height: 15px; width: 132px;" valign="top">
                                        
                                    </td>
                                    <td class="labelTD" style="height: 19px;">
                                        
                                    </td>
                                    <td class="ControlTD" style="height: 19px;">
                                        
                                    </td>
                                    <td class="labelTD" style="height: 19px; width: 155px;">
                                        
                                    </td>
                                    <td class="ControlTD" style="height: 19px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="ErrValStyle"
                                            ControlToValidate="WebDatePickerDate" Width="120px" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="labelTD" style="height: 19px; width: 180px;">
                                        <igtxt:WebImageButton ID="btnShow" runat="server" ImageDirectory="" OnClick="btnShow_Click"
                                            SkinID="uwButton" TabIndex="13" ToolTip="Show Report" UseBrowserDefaults="False"
                                            Width="118px" Height="20px" meta:resourcekey="btnShowResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <Appearance>
                                                <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                </ButtonStyle>
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td class="ControlTD" style="height: 19px; width: 132px;">
                                        
                                    </td>
                                    <td class="labelTD" style="height: 19px;" valign="top">
                                        
                                    </td>
                                    <td class="ControlTD" style="height: 19px;">
                                        
                                    </td>
                                    <td class="labelTD" style="height: 19px; width: 155px;" align="left">
                                        
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </Template>
                      </ig:ItemTemplate>
                        </Templates>
                    </ig:WebExplorerBar>
                </div>
                <br />
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td align="left" colspan="9" width="100%">
                <%--<asp:Panel ID="Panel1" runat="server" Width="100%" meta:resourcekey="Panel1Resource1">--%>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Font-Names="Verdana"
                        Font-Size="8pt" ShowParameterPrompts="False" ShowFindControls="false" 
                        HyperlinkTarget="_self" OnDrillthrough="ReportViewer1_Drillthrough" ShowBackButton="True"
                        meta:resourcekey="ReportViewer1Resource1">
                    </rsweb:ReportViewer>
                <%--</asp:Panel>--%>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <input type="hidden" id="hdnMessage" runat="server" />
</asp:Content>
