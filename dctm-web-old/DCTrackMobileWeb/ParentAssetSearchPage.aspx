<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/PopupMaster.master" AutoEventWireup="true"
    CodeFile="ParentAssetSearchPage.aspx.cs" Inherits="ParentAssetSearchPage" Title="Asset Search"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<asp:Content ID="Content_AssetSearchPage" ContentPlaceHolderID="GeneralPopupContentPlaceHolder"
    runat="Server">
    <style type="text/css">
        .disabled
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
    //<!--

        function openModelList(url) {
            var ctrlMfg = document.getElementById('<%=ddlMfg.ClientID%>');
            var ctrlBU = document.getElementById('<%=ddlBusinessUnit.ClientID%>');
            if (ctrlBU.options[ctrlBU.selectedIndex].value == "0") {
                alert('Select BusineesUnit');
            }
            else {
                if (ctrlMfg.options[ctrlMfg.selectedIndex].value == "0") {
                    alert('Select Manufacturer');
                }
                else {
                    var winSettings = "scroll:auto; width=400; height=500;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    var hWnd = open_window(url, "ModelList", winSettings)

                    if ((document.window != null) && (hWnd.opener))
                        hWnd.opener = document.window;
                }
            }
        }

        function openLocationList(url) {
            var ctrlSite = document.getElementById('<%=ddlPrimarySite.ClientID%>');
            if (ctrlSite.selectedIndex != -1) {
                if (ctrlSite.options[ctrlSite.selectedIndex].value == "0") {
                    alert('Select Site');
                }
                else {
                    winSettings = "scroll:auto; width=400; height=500;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    var hWnd = open_window(url, "LocationList", winSettings)

                    if ((document.window != null) && (hWnd.opener))
                        hWnd.opener = document.window;
                }
            }

        }
        function getValuesFromChild(txt, val, flag, header) {
            var hdnParentLocID = document.getElementById('<%=hdnLocationID.ClientID%>');
            var txtParentLoc = document.getElementById('<%=txtParentLocation.ClientID%>');
            var hdnLName = document.getElementById('<%=hdnLocName.ClientID%>');

            var hdnMID = document.getElementById('<%=hdnModelID.ClientID%>');
            var txtAssetModel = document.getElementById('<%=txtModel.ClientID%>');
            var hdnMName = document.getElementById('<%=hdnModelName.ClientID%>');

            if (header == "Loc") {
                txtParentLoc.value = txt;
                hdnParentLocID.value = val;
                hdnLName.value = txt;
            }
            else {
                txtAssetModel.value = txt;
                hdnMID.value = val;
                hdnMName.value = txt;
            }
        }

        //-->
    </script>
    <table id="Table3" border="0" cellpadding="2" style="width: 100%;">
        <tr>
            <td align="left" colspan="2" valign="top">
                <div style="width: 100%;">
                    <ig:WebExplorerBar ID="webAssetSearch" runat="server" GroupExpandAction="HeaderClick"
                        Width="100%" BorderWidth="1px">
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
                                    <table id="tblSearch" align="center" border="0" cellpadding="4" cellspacing="0" style="height: 46px"
                                        width="80%">
                                        <tr>
                                            <td class="labelTD" style="width: 100px;" valign="bottom">
                                                <asp:Label ID="lblBusinessUnit" runat="server" CssClass="FieldName" Width="100px">Company</asp:Label>
                                            </td>
                                            <td class="ControlTD" style="width: 54px;" valign="bottom">
                                                <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    Width="154px" AppendDataBoundItems="True" TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="ControlTD" style="width: 18px;" valign="bottom">
                                            </td>
                                            <td class="ControlTD" style="width: 100px;" valign="bottom">
                                                <asp:Label ID="lblPrimarySite" runat="server" CssClass="FieldName" Width="100px"
                                                    Text="Site"></asp:Label>
                                            </td>
                                            <td class="ControlTD" valign="bottom">
                                                <asp:DropDownList ID="ddlPrimarySite" runat="server" CssClass="dropdownText" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlPrimarySite_SelectedIndexChanged" TabIndex="2" Width="154px">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="ControlTD" style="width: 18px;" valign="bottom">
                                            </td>
                                            <td class="ControlTD" style="width: 100px;" valign="bottom">
                                                <asp:Label ID="lblLocation" runat="server" CssClass="FieldName" Text="Location" Width="100px"></asp:Label>
                                            </td>
                                            <td class="ControlTD" valign="top">
                                                <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtParentLocation" runat="server" Width="120px" CssClass="FieldValue"
                                                                MaxLength="150" Enabled="False" Text="(All)"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <a href="javascript:openLocationList('TreeList.aspx?Type=RptLocations&Site=<%=ddlPrimarySiteVal %>&Header=Location');">
                                                                <img id="imgLocButton" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td class="ControlTD" align="left" valign="top">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelTD" style="width: 100px;" valign="bottom">
                                                <asp:Label ID="lblManufacturer" runat="server" CssClass="FieldName" Width="106px">Manufacturer</asp:Label>
                                            </td>
                                            <td class="ControlTD" style="width: 54px;" valign="bottom">
                                                <asp:DropDownList ID="ddlMfg" runat="server" CssClass="dropdownText" Width="154px"
                                                    OnSelectedIndexChanged="ddlMfg_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="ControlTD" style="width: 18px;" valign="bottom">
                                            </td>
                                            <td class="ControlTD" style="width: 100px;" valign="bottom">
                                                <asp:Label ID="lblModel" runat="server" CssClass="FieldName" Width="100px">Asset Model</asp:Label>
                                            </td>
                                            <td class="ControlTD" valign="bottom">
                                                <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtModel" runat="server" Width="154px" CssClass="FieldValue" Enabled="False"
                                                                MaxLength="150" TabIndex="6" Text="(All)"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <a href="javascript:openModelList('TreeList.aspx?Type=SearchModels&MfgName=<%=ddlMfgName %>&MfgID=<%=ddlMfgID %>&BU=<%=ddlBusinessUnitVal %>&Header=AssetModel');">
                                                                <img id="img1" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td class="labelTD" style="width: 10px;" valign="top">
                                            </td>
                                            <td align="left" class="labelTD" style="width: 100px;" valign="top">
                                                <asp:Label ID="lblRefNumber" runat="server" CssClass="FieldName" Width="100px">SerialNo#</asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:TextBox ID="txtRefNumber" runat="server" CssClass="FieldValue" MaxLength="50"
                                                    TabIndex="11" Width="154px"></asp:TextBox>
                                            </td>
                                            <td class="ControlTD" style="width: 10px;" valign="top">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="9">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <igtxt:WebImageButton ID="wibSearch" runat="server" ImageDirectory="" OnClick="wibSearch_Click"
                                                                SkinID="uwButton" Text="Search" ToolTip="Search" UseBrowserDefaults="False" Width="75px"
                                                                CausesValidation="False">
                                                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                                    WidthOfRightEdge="13" />
                                                            </igtxt:WebImageButton>
                                                        </td>
                                                        <td>
                                                            <igtxt:WebImageButton ID="wibReset" runat="server" CausesValidation="False" ImageDirectory=""
                                                                OnClick="wibReset_Click" SkinID="uwButton" Text="Reset" ToolTip="Reset" UseBrowserDefaults="False"
                                                                Width="75px" ClickOnEnterKey="False">
                                                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                                                    WidthOfRightEdge="13" />
                                                            </igtxt:WebImageButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </Template>
                            </ig:ItemTemplate>
                        </Templates>
                    </ig:WebExplorerBar>
                </div>
            </td>
        </tr>
    </table>
    <table id="Table1" align="center" border="0" cellpadding="0" cellspacing="0" width="100%"
        style="height: 252px">
        <tr>
            <td align="center">
                <ig:WebDataGrid ID="wdgParentAssetSearch" runat="server" AutoGenerateColumns="False"
                    DataKeyFields="AssetID" HeaderCaptionCssClass="GridHeader" OnInitializeRow="wdgParentAssetSearch_InitializeRow"
                    DefaultColumnWidth="100px" Width="98%" EnableDataViewState="True" Height="300px">
                    <Columns>
                        <ig:BoundDataField DataFieldName="AssetID" Key="AssetID" Width="150px" Hidden="true">
                            <Header Text="AssetID" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="BusinessUnit" Key="BusinessUnit" Hidden="true">
                            <Header Text="Entity" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="AssetName" Key="AssetName" Width="150px" Hidden="true">
                            <Header Text="Host Name" />
                        </ig:BoundDataField>
                        <ig:TemplateDataField Key="RefNumber">
                            <ItemTemplate>
                                <asp:HyperLink ID="RefNumber" NavigateUrl='<%# "javascript:window.opener.PopUpWindowCallBack(" + "&#39;" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "AssetID") + 
                                                 "&#39;,&#39;" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "RefNumber")+ "&#39;,&#39;" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "PrimarySiteID")
                                                  +"&#39;,&#39;" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "LastSeenLocationID")+ "&#39;,&#39;"+ DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "Location")
                                                  + "&#39;,&#39;"+ DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ParentAssetID")+ "&#39;,&#39;" + "&#39;);window.close();" %>'
                                    runat="server" Text='<%# Bind("RefNumber") %>'></asp:HyperLink>
                            </ItemTemplate>
                            <Header Text="Serial No" />
                        </ig:TemplateDataField>
                        <ig:BoundDataField DataFieldName="RFID Card No" Key="RFID Card No" Width="160px">
                            <Header Text="Barcode/EPC" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MfgName" Key="MfgName" Width="100px">
                            <Header Text="Manufacturer" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="ModelName" Key="ModelName" Width="175px">
                            <Header Text="Model" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="AssetGroup" Key="AssetGroup" Width="100px">
                            <Header Text="Asset Type" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Primary Site" Key="Site" Width="80px">
                            <Header Text="Site" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Location" Key="Location" Width="80px">
                            <Header Text="Location" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Owner" Key="Owner" Width="120px" Hidden="true">
                            <Header Text="Custodian" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="IssuedDate" Key="IssuedDate" Width="120px" Hidden="true">
                            <Header Text="Check Out Date" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="IssuedBy" Key="IssuedBy" Width="120px" Hidden="true">
                            <Header Text="Check Out By" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="ReceivedDate" Key="ReceivedDate" Width="120px"
                            Hidden="true">
                            <Header Text="Check In Date" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="IssuedToName" Key="IssuedToName" Width="100px"
                            Hidden="True">
                            <Header Text="Issued To" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Asset Creation Date" Key="CreatedDate" Width="80px"
                            Hidden="true">
                            <Header Text="Created Date" />
                        </ig:BoundDataField>
                        <ig:TemplateDataField Key="RFIDTagIcon" Width="18px" Hidden="true">
                            <Header Text="Tag" />
                        </ig:TemplateDataField>
                        <ig:BoundDataField DataFieldName="RFID Card No" Key="RFIDTagID" Width="18px" Hidden="true">
                            <Header Text="RFID Tag" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Asset Status" Key="Status" Hidden="true">
                            <Header Text="Asset Status" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="ParentAssetID" Key="ParentAssetID" Hidden="true">
                            <Header Text="ParentAssetID" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="PrimarySiteID" Key="PrimarySiteID" Hidden="true">
                            <Header Text="PrimarySiteID" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="LastSeenLocationID" Key="LastSeenLocationID" Hidden="true">
                            <Header Text="LastSeenLocationID" />
                        </ig:BoundDataField>
                    </Columns>
                    <Behaviors>
                        <ig:EditingCore>
                        </ig:EditingCore>
                        <ig:Sorting Enabled="true" SortingMode="Multi" AscendingImageAltText="Up" DescendingImageAltText="Down">
                            <ColumnSettings>
                                <ig:SortingColumnSetting ColumnKey="BusinessUnit" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="AssetGroup" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="RefNumber" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="MfgName" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="ModelName" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="RFID Card No" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="SPCModel" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="Site" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="Location" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="Creator" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="IssuedDate" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="CurrentOwner" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="ReceivedDate" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="IssuedDate" Sortable="true" />
                                <ig:SortingColumnSetting ColumnKey="BusinessUnit"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="AssetGroup"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="RefNumber"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="MfgName"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="ModelName"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="RFID Card No"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="SPCModel"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="Site"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="Location"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="Creator"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="IssuedDate"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="CurrentOwner"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="ReceivedDate"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="IssuedDate"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="BusinessUnit"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="AssetGroup"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="RefNumber"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="MfgName"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="ModelName"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="RFID Card No"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="SPCModel"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="Site"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="Location"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="Creator"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="IssuedDate"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="CurrentOwner"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="ReceivedDate"></ig:SortingColumnSetting>
                                <ig:SortingColumnSetting ColumnKey="IssuedDate"></ig:SortingColumnSetting>
                            </ColumnSettings>
                        </ig:Sorting>
                        <ig:ColumnFixing>
                        </ig:ColumnFixing>
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
    <input id="hdnUserName" runat="server" style="width: 25px" type="hidden" />
    <input id="hdnUserID" runat="server" style="width: 25px" type="hidden" />
    <input id="hdnPageSelections" runat="server" style="width: 25px" type="hidden" />
    <input type="hidden" id="hdnLocationID" runat="server" />
    <input type="hidden" id="hdnModelID" runat="server" />
    <input type="hidden" id="hdnModelName" runat="server" />
    <input type="hidden" id="hdnLocName" runat="server" />
    <input type="hidden" id="hdnAssetId" runat="server" />
    <input type="hidden" id="hdnAssetCount" runat="server" />
</asp:Content>
