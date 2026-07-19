<%@ Page Title="" Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master" AutoEventWireup="true"
    CodeFile="LocationDetails.aspx.cs" Inherits="LocationDetails" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig1" %>
<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <table id="Table1" border="0" cellpadding="0" style="height: 96%; width: 100%;">
        <tr valign="top">
            <td align="left" style="width: 99%">
                &nbsp;
            </td>
        </tr>
        <tr valign="top">
            <td align="left" style="width: 99%">
                <table style="width: 934px">
                    <tr>
                        <td style="width: 466px">
                            <igmisc:WebGroupBox ID="gpBxlocDet" runat="server" TitleAlignment="Left" meta:resourcekey="gpBxlocDetResource1"
                                StyleSetName="">
                                <Template>
                                    <table style="width: 462px">
                                        <tr>
                                            <td style="width: 62px">
                                                <asp:Label ID="lblLocName" runat="server" CssClass="FieldName" meta:resourcekey="lblLocNameResource1"></asp:Label>
                                            </td>
                                            <td style="width: 230px">
                                                <asp:TextBox ID="lblLocNameVal" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                    Width="216px" meta:resourcekey="lblLocNameValResource1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 62px; height: 17px;">
                                                <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" meta:resourcekey="lblDescResource1"></asp:Label>
                                            </td>
                                            <td style="width: 230px; height: 17px;">
                                                <asp:TextBox ID="lblDescval" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                    Width="216px" meta:resourcekey="lblDescvalResource1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 62px">
                                                <asp:Label ID="lblExtDoor" runat="server" CssClass="FieldName" Visible="False" meta:resourcekey="lblExtDoorResource1"></asp:Label>
                                            </td>
                                            <td style="width: 230px">
                                                <asp:TextBox ID="lblextDrVal" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                    Width="216px" meta:resourcekey="lblextDrValResource1" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px">
                                                <asp:Label ID="lblLocationType" runat="server" CssClass="FieldName" meta:resourcekey="lblLocationTypeResource1"></asp:Label>
                                            </td>
                                            <td style="width: 230px">
                                                <asp:TextBox ID="lblLoctypeval" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                    Width="216px" meta:resourcekey="lblLoctypevalResource1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 62px;">
                                                <asp:Label ID="lblIpAdress" runat="server" CssClass="FieldName" Visible="False" meta:resourcekey="lblIpAdressResource1"></asp:Label>
                                            </td>
                                            <td style="width: 230px;">
                                                <asp:TextBox ID="lblipval" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                    Width="216px" meta:resourcekey="lblipvalResource1" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px">
                                                <asp:Label ID="lblParentLocation" runat="server" CssClass="FieldName" meta:resourcekey="lblParentLocationResource1"></asp:Label>
                                            </td>
                                            <td style="width: 230px">
                                                <asp:TextBox ID="lblPlocval" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                    Width="216px" meta:resourcekey="lblPlocvalResource1"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </Template>
                            </igmisc:WebGroupBox>
                        </td>
                        <td style="width: 466px">
                            <ig1:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported"
                                OnRowExported="eExporter_RowExported" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td align="right">
                <igtxt:WebImageButton ID="ibExportToExcel" runat="server" Visible="False" OnClick="ibExportToExcel_Click"
                    TabIndex="9" ToolTip="Export To Excel" UseBrowserDefaults="False" CausesValidation="False"
                    ImageDirectory="" meta:resourcekey="ibExportToExcelResource1">
                    <Appearance>
                        <Image Url="./icons/excelsmall.gif" />
                    </Appearance>
                </igtxt:WebImageButton>
            </td>
        </tr>
        <tr valign="top">
            <td align="center" style="width: 99%;">
                <table style="width: 100%">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <ig:WebTab ID="uwtLocationTab" runat="server" Width="100%" meta:resourcekey="uwtLocationTabResource1"
                                            SelectedIndex="1">
                                            <Tabs>
                                                <ig:ContentTabItem runat="server" Text="Child Location Details" meta:resourcekey="ContentTabItemResource1">
                                                    <Template>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <ig:WebDataGrid ID="grdChildLocations" runat="server" AutoGenerateColumns="False"
                                                                        Width="100%" HeaderCaptionCssClass="GridHeader">
                                                                        <Columns>
                                                                            <ig:BoundDataField DataFieldName="Location" Key="Location">
                                                                                <Header Text="Location" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="Description" Key="Description">
                                                                                <Header Text="Description" />
                                                                            </ig:BoundDataField>
                                                                            <ig:TemplateDataField Key="IsExitDoor" Hidden="true">
                                                                                <Header Text="IsExitDoor" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="IsExitDoor" runat="server" Text='<%# Bind("IsExitDoor") %>' meta:resourcekey="IsExitDoorResource1"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </ig:TemplateDataField>
                                                                            <ig:BoundDataField DataFieldName="IPAddress" Key="IPAddress" Hidden="true">
                                                                                <Header Text="IPAddress" />
                                                                            </ig:BoundDataField>
                                                                        </Columns>
                                                                        <EmptyRowsTemplate>
                                                                            <div style="text-align: center;">
                                                                                <br />
                                                                                <br />
                                                                                <img src="images/WebDataGrid/attention.png" alt="" align="middle" />
                                                                                No Sub Locations found.
                                                                            </div>
                                                                        </EmptyRowsTemplate>
                                                                    </ig:WebDataGrid>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </Template>
                                                </ig:ContentTabItem>
                                                <ig:ContentTabItem runat="server" Text="Asset Details" meta:resourcekey="ContentTabItemResource2">
                                                    <Template>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <ig:WebDataGrid ID="grdAssetDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                        OnDataBound="grdAssetDetails_DataBound" HeaderCaptionCssClass="GridHeader" Height="300px">
                                                                        <Columns>
                                                                            <ig:BoundDataField DataFieldName="Entity" Key="Entity">
                                                                                <Header Text="Company" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="AssetGroup" Key="AssetType">
                                                                                <Header Text="Asset Type" />
                                                                            </ig:BoundDataField>
                                                                            <ig:TemplateDataField Key="RefNumber">
                                                                                <ItemTemplate>
                                                                                    <asp:HyperLink ID="RefNumber" NavigateUrl='<%# "~/ViewAsset.aspx?AssetID=" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "AssetID") %>'
                                                                                        runat="server" Text='<%# Bind("RefNumber") %>' onclick="open_window (this.href, 'popupwindow',  'top=50,left=50,width=1200,height=750,scrollbars,resizable'); return false;" ></asp:HyperLink>
                                                                                </ItemTemplate>
                                                                                <Header Text="SerialNo" />
                                                                            </ig:TemplateDataField>
                                                                            <ig:BoundDataField DataFieldName="AssetName" Key="AssetName">
                                                                                <Header Text="Asset Name" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="Manufacturer" Key="Manufacturer">
                                                                                <Header Text="Manufacturer" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="Model" Key="Model">
                                                                                <Header Text="Model" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="Asset Status" Key="Asset Status" Hidden="true">
                                                                                <Header Text="Asset Status" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="Creator" Key="Creator" Hidden="true">
                                                                                <Header Text="Custodian" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="ReceivedDate" Key="ReceivedDate">
                                                                                <Header Text="Check In Date" />
                                                                            </ig:BoundDataField>
                                                                            <ig:BoundDataField DataFieldName="Asset Creation Date" Key="Asset Creation Date">
                                                                                <Header Text="Created Date" />
                                                                            </ig:BoundDataField>
                                                                        </Columns>
                                                                        <Behaviors>
                                                                            <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager">
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
                                                                                No Assets found for the location.
                                                                            </div>
                                                                        </EmptyRowsTemplate>
                                                                    </ig:WebDataGrid>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </Template>
                                                </ig:ContentTabItem>
                                            </Tabs>
                                        </ig:WebTab>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="bottom">
            <td align="center" style="width: 99%">
                <table border="0">
                    <tr>
                        <td style="width: 223px">
                            &nbsp;
                        </td>
                        <td align="left">
                            <igtxt:WebImageButton ID="ibCancel" runat="server" ImageDirectory="" OnClick="ibCancel_Click"
                                SkinID="uwButton" TabIndex="7" Text="Cancel" ToolTip="Cancel" UseBrowserDefaults="False"
                                meta:resourcekey="ibCancelResource1">
                                <RoundedCorners HeightOfBottomEdge="0" HoverImageUrl="images/Buttob/ig_butMacb2.gif"
                                    ImageUrl="images/Buttob/ig_butMacb1.gif" MaxHeight="20" MaxWidth="500" PressedImageUrl="images/Buttob/ig_butMacb4.gif"
                                    RenderingType="FileImages" WidthOfRightEdge="13" />
                                <HoverAppearance>
                                    <ButtonStyle Cursor="Hand">
                                    </ButtonStyle>
                                </HoverAppearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td style="width: 224px">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="bottom">
            <td align="center" style="width: 99%">
                <div class="text" style="text-align: left">
                    &nbsp;</div>
            </td>
        </tr>
    </table>
</asp:Content>
