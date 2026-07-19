<%@ Page Title="Import Asset Models" Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" Theme="SkinFile" CodeFile="ImportAssetModels.aspx.cs"
    Inherits="ImportAssetModels" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.jQuery.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <!-- for WebUpload control-->
    <link href="Styles/themes/ig/jquery.ui.custom.css" rel="stylesheet" type="text/css" />
    <link href="Styles/themes/base/ig.ui.min.css" rel="stylesheet" type="text/css" />
    <!-- for WebUpload control -- end -->
    <script type="text/javascript" src="Scripts/jquery-1.6.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript" src="ig_ui/js/infragistics.js"></script>
    <script type="text/javascript" language="javascript">
        //<!--
        function onErrorHandler(e, args) {
            $("#error-message").html(args.errorMessage).stop(true, true).fadeIn(500).delay(10000).fadeOut(500);
        }

        function onFileUploadAborted(e, args) {
            //            $('#<%=wuImportAsset.ClientID %>').reload();
        }

        function onFileUploaded(e, args) {
            document.getElementById("<%=FileStats.ClientID %>").className = "hidden";
            document.getElementById("<%=UploadStats.ClientID %>").style.display = "none";
            var fileName = '{"FileName":' + '"' + args.filePath + '"}';
            //            var jasonParams = JSON.stringify(fileName);
            $.ajax({
                type: "POST",
                url: "ImportAssetModels.aspx/ShowFileStats",
                data: fileName,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    var txt = response.responseText.replace("{\"d\":\"", "");
                    txt = txt.replace("\"}", "");
                    args.errorMessage = txt;
                    onErrorHandler(e, args);
                    //                    alert(response.d);
                }
            });

        }

        function OnSuccess(response) {
            if (response.d.indexOf("DataSet") >= 0) {
                $('#FileStats').show();
                var xmlDoc = $.parseXML(response.d);
                var xml = $(xmlDoc);
                var stats = xml.find("Table1");
                var row = $("[id*=grdFileStats] tr:last-child").clone(true);
                $("[id*=grdFileStats] tr").not($("[id*=grdFileStats] tr:first-child")).remove();
                $.each(stats, function () {
                    var stat = $(this);
                    $("td", row).eq(0).html($(this).find("Name").text());
                    $("td", row).eq(1).html($(this).find("Value").text());
                    $("[id*=grdFileStats]").append(row);
                    row = $("[id*=grdFileStats] tr:last-child").clone(true);
                });
                //hide
                $('#<%=wuImportAsset.ClientID %>').hide();
                //enable ibCreate button
                $(".hidden").show();
                var oButton = ig_getWebControlById("<%=ibCreate.ClientID%>");
                oButton.setEnabled(true);

            }
            else {
                document.getElementById('<%=lblMessage.ClientID%>').innerHTML = response.d;
            }
        }

        function ibCreate_JS_Click(oButton, oEvent) {
            ShowProgress();
        }

        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        //        $('form').live("submit", function () {
        //            ShowProgress();
        //        });
        //-->
    </script>
    <table id="Table3" style="height: 100%; width: 100%;" cellspacing="0" cellpadding="0"
        border="0">
        <tr style="height: 15px;">
            <td class="labelTD" style="height: 34px; text-align: center;" colspan="7">
                <%--<asp:Label ID="lblImportAsset" runat="server" CssClass="Heading" Text="Import Asset"></asp:Label>--%>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 100%">
                &nbsp;
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top">
                            <table id="Table2" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="errorText" Width="357px" Visible="False"
                                            ForeColor="Red"></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 72px; height: 27px; text-align: right;">
                                        <asp:Label ID="lblBU" runat="server" CssClass="FieldName" Width="128px" Height="16px"
                                            meta:resourcekey="lblBUResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 25px; width: 498px;" valign="top" colspan="2">
                                        <asp:TextBox ID="txtBU" runat="server" Width="227px" CssClass="FieldValue" MaxLength="25"
                                            Height="16px" TabIndex="1" meta:resourcekey="txtBU1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="regRootEntityVal" runat="server" ControlToValidate="txtBU"
                                            CssClass="ErrValStyle" Display="Dynamic" ErrorMessage="" ValidationExpression="^[A-Za-z0-9]+"
                                            Height="16px" Width="769px" meta:resourcekey="regRootEntityValResource1"></asp:RegularExpressionValidator>
                                    </td>
                                    <td style="width: 30%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                    </td>
                                    <td style="width: 30%">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 72px; text-align: right; height: 44px;">
                                        <asp:Label ID="lblUploadAssetFile" runat="server" CssClass="FieldName" Width="128px"
                                            Height="16px" meta:resourcekey="lblUploadAssetFileResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" valign="top" style="width: 650px; height: 44px">
                                        <div>
                                            <ig:WebUpload ID="wuImportAsset" runat="server" ProgressUrl="IGUploadStatusHandler.ashx"
                                                MultipleFiles="false" ShowFileExtensionIcon="true" Width="623px" AutoStartUpload="false"
                                                OnUploadFinished="wuImportAsset_OnUploadFinished" Mode="Single">
                                                <AllowedExtensions>
                                                    <ig:FileUploadExtension Extension="xls" />
                                                    <ig:FileUploadExtension Extension="xlsx" />
                                                </AllowedExtensions>
                                                <ClientEvents OnError="onErrorHandler" FileUploaded="onFileUploaded" FileUploadAborted="onFileUploadAborted" />
                                            </ig:WebUpload>
                                            <div id="error-message" style="color: #FF0000; font-weight: bold; font-size: medium;">
                                            </div>
                                        </div>
                                    </td>
                                    <td style="width: 30%; height: 44px;" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 27px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 27px; margin-left: 40px;" align="left" valign="top" colspan="2">
                                        <asp:Label ID="lblStatus" runat="server" CssClass="FieldName" Width="600px" Height="16px"></asp:Label>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; margin-left: 40px; width: 498px;" align="left" valign="top">
                                    </td>
                                    <td style="width: 30%">
                                        <asp:Label ID="lblDuplicateFile" runat="server" CssClass="FieldName" Width="128px"
                                            Height="16px" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 18px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 18px; width: 498px;" align="left" valign="top">
                                        <asp:Label ID="lblFileUploadStatus" runat="server" CssClass="FieldName" Width="410px"></asp:Label>
                                    </td>
                                    <td style="width: 30%; height: 18px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        <!-- commented - HTML table control to show data using doPostback -->
                                        <%--<asp:Table class="tableborder" runat="server" ID="summaryTable" BackColor="LightGray"
                                                    Visible="false">
                                                    <asp:TableHeaderRow Style="text-align: left;">
                                                        <asp:TableHeaderCell CssClass="tblHeader">
                                            Excel File Statistics: 
                                                        </asp:TableHeaderCell>
                                                    </asp:TableHeaderRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblSites" runat="server" CssClass="FieldName" Width="128px" Height="16px"
                                                                Visible="False">No of Sites found: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblSitesValue" runat="server" CssClass="FieldName" Width="128px" Height="16px"
                                                                Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblRooms" runat="server" CssClass="FieldName" Width="136px" Height="16px"
                                                                Visible="False">No of Rooms found: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblRoomsValue" runat="server" CssClass="FieldName" Width="128px" Height="16px"
                                                                Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblRows" runat="server" CssClass="FieldName" Width="128px" Height="16px"
                                                                Visible="False">No of Rows found: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblRowsValue" runat="server" CssClass="FieldName" Width="128px" Height="16px"
                                                                Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblRacks" runat="server" CssClass="FieldName" Width="130px" Height="16px"
                                                                Visible="False">No of Racks found: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblRacksValue" runat="server" CssClass="FieldName" Width="128px" Height="16px"
                                                                Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblManufacturers" runat="server" CssClass="FieldName" Width="190px"
                                                                Height="16px" Visible="False">No of Manufacturers found: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblManufacturerValue" runat="server" CssClass="FieldName" Width="128px"
                                                                Height="16px" Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblModels" runat="server" CssClass="FieldName" Width="190px" Height="16px"
                                                                Visible="False">No of Asset Models found: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblModelsValue" runat="server" CssClass="FieldName" Width="128px"
                                                                Height="16px" Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblAssetTypes" runat="server" CssClass="FieldName" Width="182px" Height="16px"
                                                                Visible="False">No of Asset Types found: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblAssteTypesValue" runat="server" CssClass="FieldName" Width="128px"
                                                                Height="16px" Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblAssets" runat="server" CssClass="FieldName" Width="137px" Height="16px"
                                                                Visible="False">Total No of Asstes: </asp:Label>
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:Label ID="lblAsstesValue" runat="server" CssClass="FieldName" Width="128px"
                                                                Height="16px" Visible="False"></asp:Label>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>--%>
                                        <!-- GridView control to show data using ajax postback -->
                                        <div id="FileStats" class="hidden" runat="server">
                                            <asp:GridView ID="grdFileStats" runat="server" AutoGenerateColumns="false" Font-Names="Times New Roman"
                                                Font-Size="12pt" RowStyle-BackColor="#FFFFFF" HeaderStyle-BackColor="#000000"
                                                HeaderStyle-ForeColor="White">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="0px" DataField="ID" HeaderText="" Visible="false" />
                                                    <asp:BoundField ItemStyle-Width="200px" DataField="Name" HeaderText="File Statistics"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField ItemStyle-Width="100px" DataField="Value" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td style="width: 30%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                    </td>
                                    <td style="width: 30%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        &nbsp;&nbsp;&nbsp;
                                        <div class="hidden" id="uploadButton" runat="server">
                                            <igtxt:WebImageButton ID="ibCreate" runat="server" Text="Import to DB" UseBrowserDefaults="False"
                                                ToolTip="This will import the excel records to the DB" OnClick="ibCreate_Click"
                                                TabIndex="4" SkinID="uwButton" ImageDirectory="" Enabled="False">
                                                <ClientSideEvents Click="ibCreate_JS_Click" />
                                            </igtxt:WebImageButton>
                                        </div>
                                    </td>
                                    <td style="width: 30%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td style="width: 30%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        <!-- GridView control to show upload data -->
                                        <div id="UploadStats" class="hidden2" runat="server">
                                            <asp:GridView ID="grdUploadStatus" runat="server" AutoGenerateColumns="false" Font-Names="Times New Roman"
                                                Font-Size="12pt" RowStyle-BackColor="#FFFFFF" HeaderStyle-BackColor="#000000"
                                                HeaderStyle-ForeColor="White">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="0px" DataField="ID" HeaderText="" Visible="false" />
                                                    <asp:BoundField ItemStyle-Width="200px" DataField="Name" HeaderText="Upload Statistics"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField ItemStyle-Width="100px" DataField="Value" HeaderText="" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 72px; height: 7px; text-align: right;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 498px;" align="left" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                    </td>
                                    <td align="right">
                                        <ig:WebExcelExporter runat="server" ID="eExporter" ExportMode="Download" OnCellExported="eExporter_CellExported" />
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                            TabIndex="6" ToolTip="Export To Excel" UseBrowserDefaults="False" CausesValidation="false"
                                            ImageDirectory="">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%" align="left" colspan="3">
                                        <div style="width: 100%">
                                            <ig:WebDataGrid ID="grdAssetModel" runat="server" AutoGenerateColumns="False" DataKeyFields="ID"
                                                Width="100%" EnableRelativeLayout="True" Height="290px" OnItemCommand="grdAssetModel_ItemCommand"
                                                OnDataBound="grdAssetModel_DataBound" HeaderCaptionCssClass="GridHeader" TabIndex="7" DefaultColumnWidth="100px"
                                                OnInitializeRow="grdAssetModel_InitializeRow">
                                                <Columns>
                                                    <ig:BoundDataField DataFieldName="ModelName" Key="ModelName">
                                                        <Header Text="Model" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Manufacturer" Key="Manufacturer">
                                                        <Header Text="Manufacturer" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="ModelType" Key="ModelType">
                                                        <Header Text="ModelType" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Height_mm" Key="Height_mm">
                                                        <Header Text="Height_mm" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Width_mm" Key="Width_mm">
                                                        <Header Text="Width_mm" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Depth_mm" Key="Depth_mm">
                                                        <Header Text="Depth_mm" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="UHeight" Key="UHeight">
                                                        <Header Text="UHeight" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Weight_kg" Key="Weight_kg">
                                                        <Header Text="Weight_kg" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Max_Power_Watts" Key="Max_Power_Watts">
                                                        <Header Text="Max_Power_Watts" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Total_PSUCount" Key="Total_PSUCount">
                                                        <Header Text="Total_PSUCount" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Req_PSUCount" Key="Req_PSUCount">
                                                        <Header Text="Req_PSUCount" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Connector_Type_PDUSide" Key="Connector_Type_PDUSide">
                                                        <Header Text="Connector_Type_PDUSide" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Connector_Type_DeviceSide" Key="Connector_Type_DeviceSide">
                                                        <Header Text="Connector_Type_DeviceSide" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="IsBlade" Key="IsBlade">
                                                        <Header Text="IsBlade" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="IsEnclosure" Key="IsEnclosure">
                                                        <Header Text="IsEnclosure" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Encl_Front_Row_Count" Key="Encl_Front_Row_Count">
                                                        <Header Text="Encl_Front_Row_Count" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Encl_Front_Col_Count" Key="Encl_Front_Col_Count">
                                                        <Header Text="Encl_Front_Col_Count" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Encl_Rear_Row_Count" Key="Encl_Rear_Row_Count">
                                                        <Header Text="Encl_Rear_Row_Count" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Encl_Rear_Col_Count" Key="Encl_Rear_Col_Count">
                                                        <Header Text="Encl_Rear_Col_Count" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Blade_Row_Count" Key="Blade_Row_Count">
                                                        <Header Text="Blade_Row_Count" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Blade_Col_Count" Key="Blade_Col_Count">
                                                        <Header Text="Blade_Col_Count" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="MountType" Key="MountType">
                                                        <Header Text="MountType" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="AirFlowDirection" Key="AirFlowDirection">
                                                        <Header Text="AirFlowDirection" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="InternalWidth_Rack" Key="InternalWidth_Rack">
                                                        <Header Text="InternalWidth_Rack" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Internal_Depth_Rack" Key="Internal_Depth_Rack">
                                                        <Header Text="Internal_Depth_Rack" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Internal_Height_Rack" Key="Internal_Height_Rack">
                                                        <Header Text="Internal_Height_Rack" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Status" Key="Status">
                                                        <Header Text="Status" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Reason" Key="Reason">
                                                        <Header Text="Reason" />
                                                    </ig:BoundDataField>
                                                </Columns>
                                                <Behaviors>
                                                    <ig:EditingCore>
                                                    </ig:EditingCore>
                                                    <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
                                                        <PagerTemplate>
                                                            <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
                                                        </PagerTemplate>
                                                    </ig:Paging>
                                                    <ig:ColumnResizing>
                                                    </ig:ColumnResizing>
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
                        <td style="background-image: url(images/table_right_middle.gif); width: 6px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div class="loading" align="center">
                    Import in progress...<br />
                    <img src=".\images\loader.gif" alt="Loading..." />
                </div>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
</asp:Content>
