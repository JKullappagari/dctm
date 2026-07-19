<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="AssetSearchPage.aspx.cs" Inherits="AssetSearchPage"
    Title="Asset Search" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_AssetSearchPage" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <script language="javascript" type="text/javascript">
        //<!--
        //    function WDG1_DataFiltering(sender, e) {

        //        //Gets the column filters array which will be used to filter the data in the grid
        //        var columnFilters = e.get_columnFilters();

        //        //Gets the column key
        //        var key = columnFilters[0].get_columnKey();

        //        if (!confirm("Are you sure you want to continue with filtering the column '" + key + "'?"))

        //        //Cancels the 'DataFiltering' event
        //            e.set_cancel(true);

        //    }
        //    function OnDocumentSelected(docId, docNum, docTitle, parentDocId, level) {
        //        var url = "ViewAsset.aspx?AssetID=" + docId;
        //        //	    window.location = url;
        //        winSettings = "scroll:auto; width=800; height=480;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
        //        var hWnd = window.open(url, "searchsubcon", winSettings)
        //    }

        function OnDocumentSelected() {

            //        var url = "ViewAsset.aspx?AssetID=" + docId;hdnAssetCount
            var hdvalue = document.getElementById('<%=hdnAssetId.ClientID %>')
            var hdCount = document.getElementById('<%=hdnAssetCount.ClientID %>')
            //                var url = "CreateAsset.aspx?AssetID=" + hdvalue.value;
            var url = "ModifyAssetAll.aspx?AssetID=" + hdvalue.value;
            // var url = "CreateAsset.aspx?AssetID=" + hdvalue.value, AssetCount = hdCount.value;
            // winSettings = "scroll:auto; width=1500; height=1000;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
            winSettings = "width=800,height=1000,scrollbars,resizable"
            var hWnd = open_window(url, "Ed", winSettings)

            if ((document.window != null) && (hWnd.opener))
                hWnd.opener = document.window;
            //	    window.location = url;
            //winSettings = "scroll:auto; width=800; height=480;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
            //var hWnd = window.open(url, "searchsubcon", winSettings)

            //        winSettings = "scroll:auto; width=800; height=1000;resizable:yes; scroll:no; help:no; toolbar:no";
            //        var hWnd = window.open(url, "searchsubcon", winSettings)
            //        window.open (this.href, 'popupwindow',  'width=800,height=1000,scrollbars,resizable');
            // window.open(this.href, 'popupwindow');
        }
        //    function OnDocumentSelected() {
        //        var url = "ViewAsset.aspx?AssetID=" + docId;
        //        winSettings = "scroll:auto; width=1500; height=1000;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
        //        var hWnd = window.open(url, "CreateAsset", winSettings)

        //        if ((document.window != null) && (hWnd.opener))
        //            hWnd.opener = document.window;
        //        
        //    }



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

        //    function GetKeyPress()
        // {
        //     alert("you Pressed " & event.KeyCode;
        // }
        function uwgAssetSearch_CellClickHandler(gridName, cellId, button) {
            var rowObj = igtbl_getRowById(cellId);

            if (rowObj != null) {
                if (rowObj.getDataKey() == null) {
                    igtbl_cancelPostBack(rowObj.gridId)
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


        var gridHeaderCheckBoxId;

        //function GetGridHeaderCheckBoxID(pCtrl)
        //{
        //    gridHeaderCheckBoxId = pCtrl.id;
        //}

        function initializaLayout(gridName) {
            var grid = igtbl_getGridById(gridName);
            var div = grid.getDivElement();
            div.style.position = "relative";
        }
        function noenter() {
            return !(window.event && window.event.keyCode == 13);
        }

        function SelectAllCheckboxes(spanChk, name) {

            // Added as ASPX uses SPAN for checkbox
            var idischeck = 0;
            var ichkcount = 0;
            var oItem = spanChk.children;
            var sItemName = new String();
            var theBox = (spanChk.type == "checkbox") ?
        spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
                    sItemName = elm[i].name;
                    if (sItemName.indexOf(name, 0) >= 0) {
                        if (elm[i].checked != xState)
                            elm[i].checked = xState;

                        if (elm[i].disabled == true) {
                            elm[i].checked = false;
                            idischeck += 1;
                        }
                        ichkcount += 1;
                    }
                }

            if (ichkcount == idischeck) {
                //var varchkall = document.getElementById('ctl00$Master_ContentPlaceHolder$uwgAssetSearch$ctl00$chkAll');

                var varchkall = document.getElementById('<%=uwgAssetSearch.ClientID%>_ctl00_chkAll');
                varchkall.checked = false;
            }
        }

        function SelectItemCheckbox(spanChk, name) {

            // Get the Infragistics web grid.
            var webGrid = igtbl_getGridById("<%=uwgAssetSearch.ClientID%>");

            if (webGrid == null)
                window.status = "Web grid is not initialized.";
            else {
                var icheck = 0;
                var idischeck = 0;
                var iuncheck = 0;
                var icount = 0;
                var row, cell, elm, childElm;
                var i, j;
                var item;
                var selected = new Array();

                // Iterate each row in the grid 
                // to find the rows that are checked.
                for (i = 0; i < webGrid.Rows.length; i++) {
                    // Get the row of this row index.
                    row = webGrid.Rows.getRow(i);

                    // Get the cell containing the check box in this row.
                    cell = row.getCellFromKey("SelectCheckBox");

                    if (cell == null)
                        window.status = "Cell is not initialized.";
                    else {
                        // Get the HTML element of the cell containing the check box.
                        elm = cell.getElement();

                        if (elm == null)
                            window.status = "Cell element is not initialized.";
                        else {
                            // Iterate the child elements under the cell element
                            // to find the check box.
                            for (j = 0; j < elm.children.length; j++) {
                                childElm = elm.children[j];
                                if (childElm.type == "checkbox") {
                                    // Found the check box in the cell.
                                    if (childElm.checked) {
                                        icheck += 1;
                                        icount += 1;
                                        // Check box is selected.
                                        item = row.getCell(1).getValue();
                                        if (selected.length == 0)
                                            selected[0] = item;
                                        else
                                            selected[selected.length] = item;
                                    }
                                    else {
                                        iuncheck += 1;
                                        icount += 1;

                                    }
                                    //  alert(childElm.enabled);
                                    if (childElm.disabled == true) {
                                        idischeck += 1;
                                        //alert("disabled");
                                    }
                                    // Exit the loop since the check box has been found.
                                    break;
                                }
                            }
                        }
                    }
                }

                var pageSelectionField = document.getElementById('<%=hdnPageSelections.ClientID %>');
                if (selected.length > 0) {
                    var selectedItems = selected.join(",").toString();

                    pageSelectionField.value = selectedItems;
                }
                else {
                    pageSelectionField.value = "";
                }

                //alert(pageSelectionField.value);

                //var varchkall = document.getElementById('ctl00$Master_ContentPlaceHolder$uwgAssetSearch$ctl00$chkAll');
                var varchkall = document.getElementById('<%=uwgAssetSearch.ClientID%>_ctl00_chkAll');
                if (icount == icheck + idischeck)
                    varchkall.checked = true;
                else
                    varchkall.checked = false;
                //if(idischeck==icount)
                // varchkall.checked=false;

            }
        }

        function assignRFID_Click(oButton, oEvent) {

            // Get the Infragistics web grid.
            var webGrid = igtbl_getGridById("<%=uwgAssetSearch.ClientID%>");

            if (webGrid == null)
                window.status = "Web grid is not initialized.";
            else {
                var row, cell, elm, childElm;
                var i, j;
                var item;
                var selected = new Array();

                // Iterate each row in the grid 
                // to find the rows that are checked.
                for (i = 0; i < webGrid.Rows.length; i++) {
                    // Get the row of this row index.
                    row = webGrid.Rows.getRow(i);

                    // Get the cell containing the check box in this row.
                    cell = row.getCellFromKey("Issue");

                    if (cell == null)
                        window.status = "Cell is not initialized.";
                    else {
                        // Get the HTML element of the cell containing the check box.
                        elm = cell.getElement();

                        if (elm == null)
                            window.status = "Cell element is not initialized.";
                        else {
                            // Iterate the child elements under the cell element
                            // to find the check box.
                            for (j = 0; j < elm.children.length; j++) {
                                childElm = elm.children[j];
                                if (childElm.type == "checkbox") {
                                    // Found the check box in the cell.
                                    if (childElm.checked) {
                                        // Check box is selected.
                                        item = row.getCell(1).getValue();
                                        if (selected.length == 0)
                                            selected[0] = item;
                                        else
                                            selected[selected.length] = item;
                                    }
                                    // Exit the loop since the check box has been found.
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (selected.length > 0) {
                var selectedItems = selected.join(",").toString();
                var url = "RFIDCardAssignment.aspx?AssetID=" + selectedItems;



                var feature = "center:yes;resizable:no;dialogWidth:670px;dialogHeight:455px;scroll:no;status:no;help:no;"
                var returnVal = open_window(url, selected, feature);
                // alert(returnVal);
                // if (returnVal != "1") 
                // {
                //     oEvent.cancel = true;
                // }
            }
            else {
                var selectedItems = selected.join(",").toString();
                var url = "RFIDCardMassAssignment.aspx?AssetID=" + selectedItems;
                var feature = "center:yes;resizable:no;dialogWidth:645px;dialogHeight:490px;scroll:no;status:no;help:no;"
                var returnVal = open_window(url, selected, feature);

                //oEvent.cancel = true;
            }
        }
        function ddlAssetStatus() {

            var varddlAssetStatus = document.forms[0].elements['<%=ddlAssetStatus.ClientID%>'].value;

        }
        function ddlRFIDCardStatus() {
            var varddlRFIDCardStatus = document.forms[0].elements['<%=ddlRFIDCardStatus.ClientID%>'].value;

        }


        function ddlIssue() {
        }

        //Gets a lookup window with all the records in the subcontractor table
        function searchUserPopup() {
            var url = "UserSearchPopup.aspx?BusinessUnit=" + document.forms[0].elements['<%=ddlBusinessUnit.ClientID%>'].value;
            url = url + "&DisplayNameField=" + document.getElementById("<%=txtOwner.ClientID %>").id;
            url = url + "&IDField=" + document.getElementById("<%=hdnOwnerID.ClientID %>").id;
            url = url + "&NameField=" + document.getElementById("<%=hdnOwnerName.ClientID %>").id;

            winSettings = "scroll:auto; width=880; height=470;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
            var hWnd = open_window(url, "searchsubcon", winSettings)
            if ((document.window != null) && (hWnd.opener))
                hWnd.opener = document.window;
        }

        // -->
    </script>
    <table id="Table3" border="0" cellpadding="2" style="width: 100%;">
        <tr>
            <td align="left" colspan="2" valign="top">
                <div style="width: 100%;">
                    <ig:WebExplorerBar ID="webAssetSearch" runat="server" GroupExpandAction="HeaderClick"
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
                                            <td colspan="6" align="left">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true"
                                                    CssClass="ErrSummaryNew" DisplayMode="BulletList" ShowMessageBox="false" meta:resourcekey="ValidationSummary1Resource1" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelTD" valign="top">
                                                <asp:Label ID="lblBusinessUnit" runat="server" CssClass="FieldName" Width="88px"
                                                    meta:resourcekey="lblBusinessUnitResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD" valign="bottom">
                                                <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    Enabled="false" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged"
                                                    TabIndex="1" Width="151px" meta:resourcekey="ddlBusinessUnitResource1">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="labelTD" valign="bottom">
                                                <asp:Label ID="lblPrimarySite" runat="server" CssClass="FieldName" Width="89px" meta:resourcekey="lblPrimarySiteResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD" valign="bottom">
                                                <asp:DropDownList ID="ddlPrimarySite" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    OnSelectedIndexChanged="ddlPrimarySite_SelectedIndexChanged" TabIndex="2" Width="163px"
                                                    meta:resourcekey="ddlPrimarySiteResource1">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" class="labelTD" valign="top">
                                                <asp:Label ID="lblLocation" runat="server" CssClass="FieldName" meta:resourcekey="lblLocationResource1"
                                                    Width="100px"></asp:Label>
                                            </td>
                                            <td class="ControlTD" valign="top">
                                                <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtParentLocation" runat="server" Width="250px" CssClass="FieldValue"
                                                                MaxLength="150" Enabled="False" meta:resourcekey="txtParentLocationResource1"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <a href="javascript:openLocationList('TreeList.aspx?Type=RptLocations&Site=<%=ddlPrimarySiteVal %>&Header=Location');">
                                                                <img id="imgLocButton" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelTD">
                                                <asp:Label ID="lblSubcontractor" runat="server" CssClass="FieldName" Width="51px"
                                                    meta:resourcekey="lblSubcontractorResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD" colspan="5">
                                                <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtOwner" runat="server" CssClass="FieldValue" MaxLength="255" Enabled="False"
                                                                Width="250px" meta:resourcekey="txtOwnerResource1" Visible="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:HyperLink ID="hlSearch" runat="server" ImageUrl="images/search.gif" NavigateUrl="javascript:searchUserPopup();"
                                                                TabIndex="5" Text="Search" meta:resourcekey="hlSearchResource1" Visible="true"></asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 19px">
                                            <td class="labelTD" valign="top">
                                                <asp:Label ID="lblAssetType" runat="server" CssClass="FieldName" Width="90px" meta:resourcekey="lblAssetTypeResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="dropdownText" TabIndex="6"
                                                    Width="191px" meta:resourcekey="ddlAssetTypeResource1">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="labelTD" valign="top">
                                                <asp:Label ID="lblRfidCardStatus" runat="server" CssClass="FieldName" Width="94px"
                                                    meta:resourcekey="lblRfidCardStatusResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:DropDownList ID="ddlRFIDCardStatus" runat="server" CssClass="dropdownText" TabIndex="7"
                                                    Width="156px" meta:resourcekey="ddlRFIDCardStatusResource1">
                                                    <asp:ListItem Value=" " meta:resourcekey="ListItemResource1"></asp:ListItem>
                                                    <asp:ListItem Value="A" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                    <asp:ListItem Value="N" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" class="labelTD" valign="top">
                                                <asp:Label ID="lblAssetStatus" runat="server" CssClass="FieldName" meta:resourcekey="lblAssetStatusResource1"
                                                    Width="100px"></asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:DropDownList ID="ddlAssetStatus" runat="server" CssClass="dropdownText" TabIndex="8"
                                                    Width="100px" meta:resourcekey="ddlAssetStatusResource1">
                                                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                                    <asp:ListItem Enabled="False" Value="1" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                                    <asp:ListItem Value="2" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                                    <asp:ListItem Value="3" meta:resourcekey="ListItemResource7"></asp:ListItem>
                                                    <asp:ListItem Value="4" meta:resourcekey="ListItemResource8"></asp:ListItem>
                                                    <asp:ListItem Value="5" meta:resourcekey="ListItemResource9"></asp:ListItem>
                                                    <asp:ListItem Value="6" meta:resourcekey="ListItemResource10"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelTD">
                                                <asp:Label ID="lblDocumentName" runat="server" CssClass="FieldName" Width="78px"
                                                    meta:resourcekey="lblDocumentNameResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:TextBox ID="txtAssetName" runat="server" CssClass="FieldValue" MaxLength="100"
                                                    TabIndex="9" Width="250px" meta:resourcekey="txtAssetNameResource1"></asp:TextBox>
                                                <br />
                                                <asp:RegularExpressionValidator ID="revAssetName" runat="server" ControlToValidate="txtAssetName"
                                                    CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                                    Height="15px" Width="166px" meta:resourcekey="revAssetNameResource1"></asp:RegularExpressionValidator>
                                            </td>
                                            <td class="labelTD" valign="top">
                                                <asp:Label ID="Label4" runat="server" CssClass="FieldName" Width="94px" meta:resourcekey="Label4Resource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:TextBox ID="txtRFIDTag" runat="server" CssClass="FieldValue" MaxLength="24"
                                                    TabIndex="10" Width="155px" meta:resourcekey="txtRFIDTagResource1"></asp:TextBox>
                                                <br />
                                                <asp:RegularExpressionValidator ID="revBarcode" runat="server" ControlToValidate="txtRFIDTag"
                                                    Display="None" ValidationExpression="^[A-Za-z0-9]+$" CssClass="ErrValStyle" meta:resourcekey="revBarcodeResource1"> </asp:RegularExpressionValidator>
                                            </td>
                                            <td align="left" class="labelTD" valign="top">
                                                <asp:Label ID="lblRefNumber" runat="server" CssClass="FieldName" Width="100px" meta:resourcekey="lblRefNumberResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD" colspan="4">
                                                <asp:TextBox ID="txtRefNumber" runat="server" CssClass="FieldValue" MaxLength="100"
                                                    TabIndex="11" Width="212px" meta:resourcekey="txtRefNumberResource1"></asp:TextBox>
                                                <br />
                                                <asp:RegularExpressionValidator ID="revSerialNo" runat="server" ControlToValidate="txtRefNumber"
                                                    CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                                    Height="15px" Width="166px" meta:resourcekey="revSerialNoResource1"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="labelTD">
                                                <asp:Label ID="lblDocumentName0" runat="server" CssClass="FieldName" Width="78px"
                                                    meta:resourcekey="lblDocumentName0Resource1"> </asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:DropDownList ID="ddlMfg" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                    OnSelectedIndexChanged="ddlMfg_SelectedIndexChanged" TabIndex="7" Width="156px"
                                                    meta:resourcekey="ddlMfgResource1">
                                                    <asp:ListItem Value=" " meta:resourcekey="ListItemResource10"></asp:ListItem>
                                                    <asp:ListItem Value="A" meta:resourcekey="ListItemResource11"></asp:ListItem>
                                                    <asp:ListItem Value="N" meta:resourcekey="ListItemResource12"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="labelTD">
                                                <asp:Label ID="lblDocumentName1" runat="server" CssClass="FieldName" Height="26px"
                                                    Width="87px" meta:resourcekey="lblDocumentName1Resource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD" valign="top">
                                                <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtModel" runat="server" Width="250px" CssClass="FieldValue" Enabled="False"
                                                                MaxLength="150" TabIndex="6" Height="16px" Text="(All)" meta:resourcekey="txtModelResource1"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <a href="javascript:openModelList('TreeList.aspx?Type=SearchModels&MfgName=<%=ddlMfgName %>&MfgID=<%=ddlMfgID %>&BU=<%=ddlBusinessUnitVal %>&Header=AssetModel');">
                                                                <img id="img1" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="left" class="labelTD" valign="top">
                                                <asp:Label ID="lblHostName" runat="server" CssClass="FieldName" Width="100px" meta:resourcekey="lblHostNameResource1"></asp:Label>
                                            </td>
                                            <td class="ControlTD">
                                                <asp:TextBox ID="txtHostName" runat="server" CssClass="FieldValue" MaxLength="100"
                                                    TabIndex="9" Width="212px" meta:resourcekey="txtHostNameResource1"></asp:TextBox>
                                                <br />
                                                <asp:RegularExpressionValidator ID="revHostName" runat="server" ControlToValidate="txtHostName"
                                                    CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                                    Height="15px" Width="166px" meta:resourcekey="revHostNameResource1"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr style="height: 19px">
                                            <td align="right" colspan="6">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table cellspacing="0">
                                                                <tr>
                                                                    <td style="width: 78px">
                                                                        <igtxt:WebImageButton ID="wibSearch" runat="server" Text="Search" UseBrowserDefaults="False"
                                                                            ToolTip="Search" OnClick="wibSearch_Click" TabIndex="13" SkinID="uwButton" ImageDirectory=""
                                                                            meta:resourcekey="wibSearchResource1">
                                                                        </igtxt:WebImageButton>
                                                                    </td>
                                                                    <td style="width: 78px">
                                                                        <igtxt:WebImageButton ID="wibReset" runat="server" CausesValidation="False" ImageDirectory=""
                                                                            OnClick="wibReset_Click" SkinID="uwButton" Text="Reset" ToolTip="Reset" UseBrowserDefaults="False"
                                                                            ClickOnEnterKey="False" ClickOnSpaceKey="False" TabIndex="14" meta:resourcekey="wibResetResource1">
                                                                        </igtxt:WebImageButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 78px">
                                                                    </td>
                                                                    <td style="width: 78px">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 19px">
                                            <td class="labelTD" rowspan="1" style="">
                                            </td>
                                            <td class="ControlTD" colspan="5" style="height: 19px" align="left">
                                                <asp:Label ID="lblMsg" runat="server" CssClass="ErrValStyle" meta:resourcekey="lblMsgResource1"></asp:Label>
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
        <tr>
            <td align="left" valign="middle">
                <table>
                    <tr>
                        <td>
                            <igtxt:WebImageButton ID="btnModifyAll" runat="server" CausesValidation="False" ImageDirectory=""
                                OnClick="btnModifyAll_Click" SkinID="uwButton" Text="ModifyAll" ToolTip="ModifyAll"
                                UseBrowserDefaults="False" ClickOnEnterKey="False" ClickOnSpaceKey="False" TabIndex="14">
                                <%--<ClientSideEvents Click="OnDocumentSelected" />--%>
                            </igtxt:WebImageButton>
                        </td>
                        <td class="style5" style="width: 180px">
                            <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" colspan="1" style="width: 2%; height: 11px" valign="top">
            </td>
        </tr>
        <tr>
            <td align="left" valign="middle">
                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="FirstPage" runat="server" CommandName="First" ImageAlign="AbsBottom"
                                ImageUrl="images/rew_hover.gif" OnCommand="NavigationLink_Click" Height="20px"
                                Width="20px" CausesValidation="False" meta:resourcekey="FirstPageResource1" />
                        </td>
                        <td style="width: 2px">
                        </td>
                        <td>
                            <asp:ImageButton ID="btnPreviousPage" runat="server" CommandName="Prev" ImageAlign="AbsBottom"
                                ImageUrl="images/prev_hover.gif" OnCommand="NavigationLink_Click" Height="20px"
                                Width="20px" CausesValidation="False" meta:resourcekey="PreviousPageResource1" />
                        </td>
                        <td style="width: 5px">
                        </td>
                        <td valign="middle">
                            <asp:TextBox ID="GoToPageTxt" runat="server" Font-Bold="True" onkeypress="return CheckNumber();"
                                Width="20px" Font-Names="Tahoma" Font-Size="9pt" Height="15px" meta:resourcekey="GoToPageTxtResource1"></asp:TextBox>
                        </td>
                        <td style="width: 2px">
                        </td>
                        <td>
                            <asp:ImageButton ID="GoToPageImb" runat="server" ImageAlign="AbsBottom" ImageUrl="images/GoToPage.gif"
                                OnClick="GoToPageImb_Click" Height="20px" Width="20px" CausesValidation="False"
                                meta:resourcekey="GoToPageImbResource1" />
                        </td>
                        <td style="width: 5px">
                        </td>
                        <td>
                            <asp:ImageButton ID="NextPage" runat="server" CommandName="Next" ImageAlign="AbsBottom"
                                ImageUrl="images/next_hover.gif" OnCommand="NavigationLink_Click" Height="20px"
                                Width="20px" CausesValidation="False" meta:resourcekey="NextPageResource1" />
                        </td>
                        <td style="width: 2px">
                        </td>
                        <td>
                            <asp:ImageButton ID="LastPage" runat="server" CommandName="Last" ImageAlign="AbsBottom"
                                ImageUrl="images/ff_hover.gif" OnCommand="NavigationLink_Click" Height="20px"
                                Width="20px" CausesValidation="False" meta:resourcekey="LastPageResource1" />
                        </td>
                        <td style="width: 5px">
                        </td>
                        <td align="center" valign="middle">
                            <asp:Label ID="CurrentPage" runat="server" Font-Bold="True" Text="1" Font-Names="Tahoma"
                                Font-Size="9pt" Height="10px" Width="20px" meta:resourcekey="CurrentPageResource1"></asp:Label>
                        </td>
                        <td valign="middle">
                            <asp:Label ID="SepLbl" runat="server" CssClass="FieldName" Enabled="False" Font-Size="X-Small"
                                Height="10px" Width="20px" meta:resourcekey="SepLblResource1">/</asp:Label>
                        </td>
                        <td valign="middle">
                            <asp:Label ID="TotalPages" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="9pt"
                                Height="10px" Width="20px" meta:resourcekey="TotalPagesResource1">1</asp:Label>
                        </td>
                        <td align="right" style="width: 5%" valign="middle">
                            <asp:ImageButton ID="ibExport" runat="server" AlternateText="Export" ImageUrl="~/icons/excelsmall.gif"
                                OnClick="ibExport_Click" ToolTip="Export to Excel" ImageAlign="right" Width="16px"
                                meta:resourcekey="ibExportResource1" />
                        </td>
                        <td style="width: 100%" valign="middle">
                            <asp:CheckBox ID="chkShowChild" runat="server" CssClass="FieldName" meta:resourcekey="chkShowChildResource1">
                            </asp:CheckBox>
                            <asp:Label ID="lblLegend" runat="server" CssClass="FieldName" Text="Legends :" Visible="False"
                                meta:resourcekey="lblLegendResource1"></asp:Label>
                        </td>
                        <td style="width: 100%" valign="middle">
                        </td>
                        <td style="width: 100%" valign="middle">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2" valign="top">
                <ig:WebHierarchicalDataGrid ID="uwgAssetSearch" runat="server" Height="400px" TabIndex="15"
                    Width="100%" AutoGenerateColumns="False" DataKeyFields="AssetID" EnableAjax="False"
                    InitialDataBindDepth="-1" OnInitializeRow="uwgAssetSearch_InitializeRow" Key="Level 0"
                    OnDataBound="uwgAssetSearch_DataBound" DataMember="ParentTable" EnableRelativeLayout="True">
                    <Columns>
                        <ig:BoundDataField DataFieldName="AssetID" Key="AssetID" Width="150px" Hidden="true">
                            <Header Text="AssetID" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="BusinessUnit" Key="BusinessUnit" Width="150px"
                            Hidden="true">
                            <Header Text="Entity" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="StartPos" Key="StartPos" Width="50px">
                            <Header Text="Start Position" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="NoOfRUs" Key="NoOfRUs" Width="75px">
                            <Header Text="U Height" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Orientation" Key="Orientation" Width="75px">
                            <Header Text="Orientation" />
                        </ig:BoundDataField>
                        <ig:TemplateDataField Key="RefNumber" Width="150px">
                            <HeaderTemplate>
                                <asp:Label ID="Label1" runat="server" Text="Serial No"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="RefNumber" NavigateUrl='<%# "~/ViewAsset.aspx?AssetID=" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "AssetID") %>'
                                    runat="server" Text='<%# Bind("RefNumber") %>' onclick="open_window (this.href, 'popupwindow',  'top=50,left=50,width=1200,height=750,scrollbars,resizable'); return false;"></asp:HyperLink>
                            </ItemTemplate>
                            <Header Text="Serial No" />
                        </ig:TemplateDataField>
                        <ig:BoundDataField DataFieldName="RFID Card No" Key="RFID Card No" Width="160px">
                            <Header Text="Barcode/EPC" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="HostName" Key="HostName" Width="150px">
                            <Header Text="Host Name" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="AssetName" Key="AssetName" Width="150px">
                            <Header Text="Asset Name" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="MfgName" Key="MfgName" Width="100px">
                            <Header Text="Manufacturer" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="ModelName" Key="ModelName" Width="120px">
                            <Header Text="Model" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="AssetGroup" Key="AssetGroup" Width="100px">
                            <Header Text="Asset Type" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Primary Site" Key="Site" Width="80px">
                            <Header Text="Site" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Room" Key="Room" Width="80px">
                            <Header Text="Room" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Location" Key="Location" Width="80px">
                            <Header Text="Location" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Owner" Key="Owner" Width="120px">
                            <Header Text="Custodian" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="IssuedDate" Key="IssuedDate" Width="120px">
                            <Header Text="Check Out Date" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="IssuedBy" Key="IssuedBy" Width="120px">
                            <Header Text="Check Out By" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="ReceivedDate" Key="ReceivedDate" Width="120px">
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
                        <ig:BoundDataField DataFieldName="ParentSerialNo" Key="ParentSerialNo" Width="150px">
                            <Header Text="Parent Serial No" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="ParentAssetName" Key="ParentAssetName" Width="150px"
                            Hidden="true">
                            <Header Text="Parent Asset Name" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="RFID Card No" Key="RFIDTagID" Width="18px" Hidden="true">
                            <Header Text="RFID Tag" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="Asset Status" Key="Status" Width="18px" Hidden="True">
                            <Header Text="Asset Status" />
                        </ig:BoundDataField>
                        <ig:BoundDataField DataFieldName="ParentAssetID" Key="ParentAssetID" Width="18px"
                            Hidden="True">
                            <Header Text="ParentAssetID" />
                        </ig:BoundDataField>
                    </Columns>
                    <Behaviors>
                        <ig:ColumnResizing>
                        </ig:ColumnResizing>
                    </Behaviors>
                    <Bands>
                        <ig:Band Key="Level 1" AutoGenerateColumns="false" DataMember="ChildTable" DataKeyFields="AssetID"
                            ShowHeader="true" Width="100%">
                            <Columns>
                                <ig:BoundDataField DataFieldName="AssetID" Key="AssetID" Width="150px" Hidden="true">
                                    <Header Text="AssetID" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="BusinessUnit" Key="BusinessUnit" Width="150px"
                                    Hidden="true">
                                    <Header Text="Entity" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="StartPos" Key="StartPos" Width="50px">
                                    <Header Text="Bay Position" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="ParentSerialNo" Key="ParentSerialNo" Width="75px">
                                    <Header Text="Parent  SerialNo" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Orientation" Key="Orientation" Width="75px">
                                    <Header Text="Orientation" />
                                </ig:BoundDataField>
                                <ig:TemplateDataField Key="RefNumber" Width="150px">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSerNo" runat="server" Text="Serial No"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="RefNumber" NavigateUrl='<%# "~/ViewAsset.aspx?AssetID=" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "AssetID") %>'
                                            runat="server" Text='<%# Bind("RefNumber") %>' onclick="open_window (this.href, 'popupwindow',  'top=50,left=50,width=1200,height=750,scrollbars,resizable'); return false;"></asp:HyperLink>
                                    </ItemTemplate>
                                    <Header Text="Serial No" />
                                </ig:TemplateDataField>
                                <ig:BoundDataField DataFieldName="RFID Card No" Key="RFID Card No" Width="160px">
                                    <Header Text="Barcode/EPC" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="HostName" Key="HostName" Width="150px">
                                    <Header Text="Host Name" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="AssetName" Key="AssetName" Width="150px">
                                    <Header Text="Asset Name" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="MfgName" Key="MfgName" Width="100px">
                                    <Header Text="Manufacturer" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="ModelName" Key="ModelName" Width="120px">
                                    <Header Text="Model" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="AssetGroup" Key="AssetGroup" Width="100px">
                                    <Header Text="Asset Type" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Primary Site" Key="Site" Width="80px">
                                    <Header Text="Site" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Room" Key="Room" Width="80px">
                                    <Header Text="Room" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Location" Key="Location" Width="80px">
                                    <Header Text="Location" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Owner" Key="Owner" Width="120px">
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
                                <ig:BoundDataField DataFieldName="ParentAssetTag" Key="ParentAssetTag" Width="80px">
                                    <Header Text="Parent Asset Tag" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="ParentHostName" Key="ParentHostName" Width="80px">
                                    <Header Text="Parent Host Name" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="ParentAssetName" Key="ParentAssetName" Width="80px"
                                    Hidden="true">
                                    <Header Text="Parent Asset Name" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="Asset Status" Key="Status" Width="150px" Hidden="True">
                                    <Header Text="Asset Status" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="ParentAssetID" Key="ParentAssetID" Width="150px"
                                    Hidden="True">
                                    <Header Text="ParentAssetID" />
                                </ig:BoundDataField>
                                <ig:BoundDataField DataFieldName="SPCModel" Key="SPCModel" Width="120px" Hidden="true">
                                    <Header Text="SPC Model" />
                                </ig:BoundDataField>
                            </Columns>
                        </ig:Band>
                    </Bands>
                    <EmptyRowsTemplate>
                        <div style="text-align: center;">
                            <br />
                            <br />
                            <img src="images/WebDataGrid/attention.png" alt="" align="middle" />
                            No records returned.
                        </div>
                    </EmptyRowsTemplate>
                </ig:WebHierarchicalDataGrid>
            </td>
        </tr>
        <tr>
            <td>
                <input id="hdnOwnerName" runat="server" style="width: 25px" type="hidden" />
                <input id="hdnOwnerID" runat="server" style="width: 25px" type="hidden" />
                <input id="hdnPageSelections" runat="server" style="width: 25px" type="hidden" />
                <input type="hidden" id="hdnLocationID" runat="server" />
                <input type="hidden" id="hdnModelID" runat="server" />
                <input type="hidden" id="hdnModelName" runat="server" />
                <input type="hidden" id="hdnLocName" runat="server" />
                <input type="hidden" id="hdnAssetId" runat="server" />
                <input type="hidden" id="hdnAssetCount" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
