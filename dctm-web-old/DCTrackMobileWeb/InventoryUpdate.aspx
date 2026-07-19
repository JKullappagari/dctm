<%@ Page Language="C#" AutoEventWireup="true" Theme="SkinFile" CodeFile="InventoryUpdate.aspx.cs"
    Inherits="InventoryUpdate" MasterPageFile="~/iAssetTrackMasterPage.master" Title="Inventory Update"
    Culture="auto" meta:resourcekey="PageResource2" UICulture="auto" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="InvUpdate" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <style type="text/css">
        .disabled
        {
            visibility: collapse;
        }
        tbody > tr.Hidden > td
        {
            width: 0px;
            display: none;
        }
        tbody > tr.Scanned > td
        {
            background-color: Green;
            color: White;
            background-repeat: repeat-x;
            background-image: none;
            border-width: 0px;
            padding: 0px 8px 0px 8px;
            overflow: hidden;
            text-decoration: none;
            text-align: left;
            font-size: 10pt;
            font-family: tahoma;
            border-color: Transparent;
            border-bottom: solid 0px Black;
            border-left: solid 0px Black;
            border-right: solid 0px Black;
        }
        tbody > tr.Missing > td
        {
            background-color: Orange;
            color: Black;
            background-repeat: repeat-x;
            background-image: none;
            border-width: 0px;
            padding: 0px 8px 0px 8px;
            overflow: hidden;
            text-decoration: none;
            text-align: left;
            font-size: 10pt;
            font-family: tahoma;
            border-color: Transparent;
            border-bottom: solid 0px Black;
            border-left: solid 0px Black;
            border-right: solid 0px Black;
        }
        tbody > tr.Misplaced > td
        {
            background-color: Yellow;
            color: Black;
            background-repeat: repeat-x;
            background-image: none;
            border-width: 0px;
            padding: 0px 8px 0px 8px;
            overflow: hidden;
            text-decoration: none;
            text-align: left;
            font-size: 10pt;
            font-family: tahoma;
            border-color: Transparent;
            border-bottom: solid 0px Black;
            border-left: solid 0px Black;
            border-right: solid 0px Black;
        }
    </style>
    <%--<script type="text/javascript" src="scripts/jquery-1.4.4.js"></script>--%>
    <script language="javascript" type="text/javascript">

        //<!--
        function uwgAsset_RowSelectionChanged(sender, args) {
            var selectedRowID = document.getElementById('<%=hdnSeletedRowID.ClientID %>');
            if (args.getSelectedRows().getItem(0) != null && args.getSelectedRows().getItem(0).get_rowIslands()[0]) {
                selectedRowID.value = args.getSelectedRows().getItem(0).get_index();
            }

        }

        var selectedRow;
        function uwgAsset_CellEditing_EnteringEditMode(sender, e) {
            var selectedRow = e.get_cell().get_row();
            var selectedRowID = document.getElementById('<%=hdnSeletedRowID.ClientID %>');
            selectedRowID.value = false;
            if (row.get_rowIslands()[0]) {
                selectedRowID.value = false;
            }
            else {

                selectedRowID.value = true;
            }
        }

        function cellValueChanged(sender, args) {
            var selectedRow = args.get_cell().get_row();
            var selectedRowID = document.getElementById('<%=hdnSeletedRowID.ClientID %>');
            selectedRowID.value = false;
            if (row.get_rowIslands()[0]) {
                selectedRowID.value = false;
            }
            else {

                selectedRowID.value = true;
            }
        }

        function checkChild() {
            // Added as ASPX uses SPAN for checkbox
            var i, j, k;
            var row, cell, elmMain, elm, childElm;
            var selected = new Array();
            var row;
            var idischeck = 0;
            var ichkcount = 0;

            var webGrid = $find("<%=uwgAsset.ClientID%>");
            for (k = 0; k < webGrid.get_gridView().get_rows().get_length(); k++) {
                // Get the row of this row index.
                row = webGrid.get_gridView().get_rows().get_row(k);
                // Get the cell containing the check box in this row.
                //                var mainIDs = document.getElementById('<%=hdnIDs.ClientID %>');
                //                mainIDs.value = mainIDs.value.replace(',' + row.get_cell(1).get_value(), '');
                var ids = document.getElementById('<%=hdnAssetIDs.ClientID %>');
                var childBand = row.get_rowIslands()[0]; // child band
                ichkcount = 0;
                for (cRow = 0; cRow < childBand.get_rows().get_length(); cRow++) {
                    // Get the cell containing the check box in this row.
                    childRow = childBand.get_rows().get_row(cRow);
                    childCell = childRow.get_cellByColumnKey("SelectChildCheckBox");
                    if (childCell == null)
                        window.status = "Cell is not initialized.";
                    else {
                        // Get the HTML element of the cell containing the check box.
                        childElm = childCell.get_element();
                        if (childElm == null)
                            window.status = "Cell element is not initialized.";
                        else {
                            // Iterate the child elements under the cell element
                            // to find the check box.
                            for (j = 0; j < childElm.children.length; j++) {
                                subChildElm = childElm.children[j];
                                if (subChildElm.type == "checkbox") {
                                    // Found the check box in the cell.
                                    if (subChildElm.checked) {
                                        var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                        ids.value = ids.value.replace(item + ',', '');
                                        ids.value = ids.value + item + ',';
                                        ichkcount = ichkcount + 1;
                                    }
                                    else {
                                        var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                        ids.value = ids.value.replace(item + ',', '');
                                    }
                                }
                            }
                        }
                    }
                }
                if (childBand.get_rows().get_length() == ichkcount) {

                    cell = row.get_cellByColumnKey("SelectCheckBox");
                    if (cell.get_element().children[0].type == "checkbox")
                        cell.get_element().children[0].checked = true;
                    var mainIDs = document.getElementById('<%=hdnIDs.ClientID %>');
                    mainIDs.value = mainIDs.value.replace(',' + row.get_cell(1).get_value(), '');
                    mainIDs.value = mainIDs.value + ',' + row.get_cell(1).get_value();
                }
                else {
                    cell = row.get_cellByColumnKey("SelectCheckBox");
                    if (cell.get_element().children[0].type == "checkbox")
                        cell.get_element().children[0].checked = false;
                    var mainIDs = document.getElementById('<%=hdnIDs.ClientID %>');
                    mainIDs.value = mainIDs.value.replace(',' + row.get_cell(1).get_value(), '');
                }

            }
            //main header
            var checkedCount = 0;
            for (i = 0; i < webGrid.get_gridView().get_rows().get_length(); i++) {
                var mainRow = webGrid.get_gridView().get_rows().get_row(i);
                var chkBox = mainRow.get_cellByColumnKey("SelectCheckBox").get_element().children[0];
                if (chkBox.type == "checkbox" && chkBox.checked == true) {
                    checkedCount = checkedCount + 1;
                }
            }
            if (webGrid.get_gridView().get_rows().get_length() == checkedCount) {

                webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = true; //chkAll
            }
            else {

                webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAll
            }

        }

        function SelectAllCheckboxes(spanChk, name) {

            // Added as ASPX uses SPAN for checkbox
            var i, j, k;
            var row, cell, elmMain, elm, childElm;
            var selected = new Array();
            var row;
            var idischeck = 0;
            var ichkcount = 0;
            var oItem = spanChk.children;
            var sItemName = new String();
            var theBox = (spanChk.type == "checkbox") ?
            spanChk : spanChk.children.item[0];
            var xState = theBox.checked;
            elmMain = theBox.form.elements;

            for (i = 0; i < elmMain.length; i++) {
                if (elmMain[i].type == "checkbox" &&
              elmMain[i].id != theBox.id) {
                    sItemName = elmMain[i].name;
                    if (sItemName.indexOf(name, 0) >= 0) {

                        if (elmMain[i].checked != xState)
                            elmMain[i].checked = xState;

                        if (elmMain[i].disabled == true) {
                            elmMain[i].checked = false;
                            idischeck += 1;
                        }
                        ichkcount += 1;
                    }
                }
            }
            var webGrid = $find("<%=uwgAsset.ClientID%>");
            for (k = 0; k < webGrid.get_gridView().get_rows().get_length(); k++) {
                // Get the row of this row index.
                row = webGrid.get_gridView().get_rows().get_row(k);
                // Get the cell containing the check box in this row.
                cell = row.get_cellByColumnKey("SelectCheckBox");

                if (cell == null)
                    window.status = "Cell is not initialized.";
                else {
                    // Get the HTML element of the cell containing the check box.
                    elm = cell.get_element();

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
                                    var mainIDs = document.getElementById('<%=hdnIDs.ClientID %>');
                                    mainIDs.value = mainIDs.value.replace(',' + row.get_cell(1).get_value(), '');
                                    mainIDs.value = mainIDs.value + ',' + row.get_cell(1).get_value();
                                    var ids = document.getElementById('<%=hdnAssetIDs.ClientID %>');
                                    var childBand = row.get_rowIslands()[0]; // child band
                                    for (cRow = 0; cRow < childBand.get_rows().get_length(); cRow++) {

                                        // Get the cell containing the check box in this row.
                                        childRow = childBand.get_rows().get_row(cRow);
                                        childCell = childRow.get_cellByColumnKey("SelectChildCheckBox");
                                        if (childCell == null)
                                            window.status = "Cell is not initialized.";
                                        else {
                                            // Get the HTML element of the cell containing the check box.
                                            childElm = childCell.get_element();
                                            if (childElm == null)
                                                window.status = "Cell element is not initialized.";
                                            else {
                                                // Iterate the child elements under the cell element
                                                // to find the check box.
                                                for (l = 0; l < childElm.children.length; l++) {
                                                    subChildElm = childElm.children[l];
                                                    if (subChildElm.type == "checkbox") {
                                                        // Found the check box in the cell.
                                                        if (subChildElm.checked) {
                                                            var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                                            ids.value = ids.value.replace(item + ',', '');
                                                            ids.value = ids.value + item + ',';
                                                        }
                                                        else {
                                                            var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                                            ids.value = ids.value.replace(item + ',', '');
                                                        }

                                                    }
                                                }

                                            }
                                        }
                                    }

                                }
                                else {
                                    var mainIDs = document.getElementById('<%=hdnIDs.ClientID %>');
                                    mainIDs.value = mainIDs.value.replace(',' + row.get_cell(1).get_value(), '');
                                    var ids = document.getElementById('<%=hdnAssetIDs.ClientID %>');
                                    var childBand = row.get_rowIslands()[0]; // child band
                                    for (cRow = 0; cRow < childBand.get_rows().get_length(); cRow++) {

                                        // Get the cell containing the check box in this row.
                                        childRow = childBand.get_rows().get_row(cRow);
                                        childCell = childRow.get_cellByColumnKey("SelectChildCheckBox");
                                        if (childCell == null)
                                            window.status = "Cell is not initialized.";
                                        else {
                                            // Get the HTML element of the cell containing the check box.
                                            childElm = childCell.get_element();
                                            if (childElm == null)
                                                window.status = "Cell element is not initialized.";
                                            else {
                                                // Iterate the child elements under the cell element
                                                // to find the check box.

                                                for (l = 0; l < childElm.children.length; l++) {
                                                    subChildElm = childElm.children[l];
                                                    if (subChildElm.type == "checkbox") {
                                                        // Found the check box in the cell.
                                                        if (subChildElm.checked) {
                                                            var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                                            ids.value = ids.value.replace(item + ',', '');
                                                            ids.value = ids.value + item + ',';
                                                        }
                                                        else {
                                                            var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                                            ids.value = ids.value.replace(item + ',', '');
                                                        }

                                                    }
                                                }

                                            }
                                        }
                                    }


                                }
                                // Exit the loop since the check box has been found.
                                break;
                            }
                        }
                    }
                }

            }
        }


        function SelectItemCheckbox(spanChk, name) {
            // Get the Infragistics web grid.
            var webGrid = $find("<%=uwgAsset.ClientID%>");

            if (webGrid == null)
                window.status = "Web grid is not initialized.";
            else {
                var isItemChecked;
                var icheck = 0;
                var idischeck = 0;
                var iuncheck = 0;
                var icount = 0;
                var row, cell, elm, childElm;
                var childRow, childCell, childElm, subChildElm;
                var i, j;
                var item;
                var selected = new Array();

                // Iterate each row in the grid 
                // to find the rows that are checked.
                for (i = 0; i < webGrid.get_gridView().get_rows().get_length(); i++) {
                    // Get the row of this row index.
                    var selectedRowID = document.getElementById('<%=hdnSeletedRowID.ClientID %>');
                    var currentRowIndex = selectedRowID.value;
                    if (i == currentRowIndex) {
                        row = webGrid.get_gridView().get_rows().get_row(i);
                        // Get the cell containing the check box in this row.
                        cell = row.get_cellByColumnKey("SelectCheckBox");
                        isItemChecked = false;
                        if (cell == null)
                            window.status = "Cell is not initialized.";
                        else {

                            // Get the HTML element of the cell containing the check box.
                            elm = cell.get_element();
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
                                            isItemChecked = true;
                                            var mainIDs = document.getElementById('<%=hdnIDs.ClientID %>');
                                            mainIDs.value = mainIDs.value.replace(',' + row.get_cell(1).get_value(), '');
                                            mainIDs.value = mainIDs.value + ',' + row.get_cell(1).get_value();
                                            var ids = document.getElementById('<%=hdnAssetIDs.ClientID %>');
                                            var childBand = row.get_rowIslands()[0];
                                            for (cRow = 0; cRow < childBand.get_rows().get_length(); cRow++) {

                                                // Get the cell containing the check box in this row.
                                                childRow = childBand.get_rows().get_row(cRow);
                                                childCell = childRow.get_cellByColumnKey("SelectChildCheckBox");
                                                if (childCell == null)
                                                    window.status = "Cell is not initialized.";
                                                else {
                                                    // Get the HTML element of the cell containing the check box.
                                                    childElm = childCell.get_element();
                                                    if (childElm == null)
                                                        window.status = "Cell element is not initialized.";
                                                    else {
                                                        // Iterate the child elements under the cell element
                                                        // to find the check box.
                                                        for (j = 0; j < childElm.children.length; j++) {
                                                            subChildElm = childElm.children[j];
                                                            if (subChildElm.type == "checkbox") {
                                                                // Found the check box in the cell.
                                                                if (!subChildElm.checked) {
                                                                    subChildElm.checked = true;
                                                                    var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                                                    ids.value = ids.value.replace(item + ',', '');
                                                                    ids.value = ids.value + item + ',';
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        else {
                                            var mainIDs = document.getElementById('<%=hdnIDs.ClientID %>');
                                            mainIDs.value = mainIDs.value.replace(',' + row.get_cell(1).get_value(), '');
                                            var ids = document.getElementById('<%=hdnAssetIDs.ClientID %>');
                                            var childBand = row.get_rowIslands()[0];
                                            for (cRow = 0; cRow < childBand.get_rows().get_length(); cRow++) {

                                                // Get the cell containing the check box in this row.
                                                childRow = childBand.get_rows().get_row(cRow);
                                                childCell = childRow.get_cellByColumnKey("SelectChildCheckBox");
                                                if (childCell == null)
                                                    window.status = "Cell is not initialized.";
                                                else {
                                                    // Get the HTML element of the cell containing the check box.
                                                    childElm = childCell.get_element();
                                                    if (childElm == null)
                                                        window.status = "Cell element is not initialized.";
                                                    else {
                                                        // Iterate the child elements under the cell element
                                                        // to find the check box.
                                                        for (j = 0; j < childElm.children.length; j++) {
                                                            subChildElm = childElm.children[j];
                                                            if (subChildElm.type == "checkbox") {
                                                                // Found the check box in the cell.
                                                                if (subChildElm.checked) {
                                                                    subChildElm.checked = false;
                                                                    var item = childRow.get_cell(14).get_value() + '#' + childRow.get_cell(15).get_value();
                                                                    ids.value = ids.value.replace(item + ',', '');
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isItemChecked == false) {

                webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAll
            }
            else {
                var checkedCount = 0;
                for (i = 0; i < webGrid.get_gridView().get_rows().get_length(); i++) {
                    row = webGrid.get_gridView().get_rows().get_row(i);
                    var chkBox = row.get_cellByColumnKey("SelectCheckBox").get_element().children[0];
                    if (chkBox.type == "checkbox" && chkBox.checked == true) {
                        checkedCount = checkedCount + 1;
                    }
                }
                if (webGrid.get_gridView().get_rows().get_length() == checkedCount) {
                    webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = true; //chkAll
                }
            }
        }
        function ibUpdate_JS_Click(oButton, oEvent) {
            document.getElementById('<%=lblMsg.ClientID%>').innerHTML = "";
        }

        // -->
    </script>
    <br />
    <br />
    <table cellpadding="0" cellspacing="0" style="vertical-align: top; width: 97%;">
        <tr>
            <td valign="top" style="width: 20%; vertical-align: top; text-align: left;">
                <table width="100%">
                    <tr>
                        <td colspan="2" valign="top">
                            <%--<asp:Button ID="btnGetAssets" runat="server" Text="Show Assets" OnClick="btnGetAssets_Click" />--%>
                            <igtxt:WebImageButton ID="ibShowData" runat="server" UseBrowserDefaults="False" OnClick="ibShowData_Click"
                                TabIndex="4" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibShowDataResource1">
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 25px;">
                            <asp:Label ID="lblLocation" runat="server" CssClass="FieldName" meta:resourcekey="lblLocationResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" colspan="2">
                            <%--<asp:Panel ID="Panel1" ScrollBars="Both" runat="server" Height="408px" Style="margin-left: 0px"
                                Width="302px">--%>
                            <ig:WebDataTree ID="TreeLocation" runat="server" Height="500px" Width="100%" Font-Bold="True"
                                Font-Size="X-Small" CheckBoxMode="BiState" BorderStyle="Solid" BorderColor="#999999"
                                meta:resourcekey="TreeLocationResource1">
                            </ig:WebDataTree>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="width: 80%; vertical-align: top; text-align: left;">
                <table width="100%">
                    <tr>
                        <td align="left" colspan="2" valign="top" style="width: 350px;">
                            <igtxt:WebImageButton ID="ibUpdate" runat="server" UseBrowserDefaults="False" OnClick="ibUpdate_Click"
                                TabIndex="4" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibUpdateResource1">
                                <ClientSideEvents Click="ibUpdate_JS_Click" />
                            </igtxt:WebImageButton>
                            &nbsp; &nbsp; &nbsp;
                            <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" TabIndex="5" meta:resourcekey="ibResetResource1">
                            </igtxt:WebImageButton>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        </td>
                        <%--<td valign="top">
                            
                        </td>--%>
                    </tr>
                    <tr>
                        <td style="height: 21px;">
                            <asp:Label ID="lblMsg" runat="server" CssClass="ErrValStyle" meta:resourcekey="lblMsgResource1"></asp:Label>
                        </td>
                        <td align="left" colspan="1" style="width: 2%; height: 11px" valign="top">
                            <asp:ImageButton ID="ibExport" runat="server" AlternateText="Export" ImageUrl="~/icons/excelsmall.gif"
                                OnClick="ibExport_Click" Visible="False" meta:resourcekey="ibExportResource1" />
                            <igtxt:WebImageButton ID="WebImageButton1" runat="server" OnClick="ibExportToExcel_Click"
                                SkinID="uwButton" TabIndex="9" UseBrowserDefaults="False" CausesValidation="False"
                                ImageDirectory="" Visible="False" meta:resourcekey="ibExportToExcelResource1">
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" align="left" colspan="2">
                            <ig:WebHierarchicalDataGrid ID="uwgAsset" runat="server" Height="500px" TabIndex="15"
                                Width="100%" AutoGenerateColumns="False" DataKeyFields="ID" EnableAjax="false"
                                InitialDataBindDepth="-1" OnInitializeRow="uwgAsset_InitializeRow" Key="Level 0"
                                OnDataBound="uwgAsset_DataBound" DataMember="ParentTable" OnRowIslandDataBinding="uwgAsset_RowIslandDataBinding"
                                EnableRelativeLayout="True" OnDataFiltered="uwgAsset_DataFiltered">
                                <%--   Key="Level 0" 
                                OnRowIslandsPopulating="uwgAsset_RowIslandsPopulating" OnInitializeBand="uwgAsset_InitializeBand"
                                OnPreRender="uwgAsset_PreRender" OnRowIslandDataBound="uwgAsset_RowIslandDataBound"--%>
                                <Columns>
                                    <ig:TemplateDataField Key="SelectCheckBox" Width="50px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" onclick="SelectAllCheckboxes(this,'ChkSelect');" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkSelect" runat="server" Enabled="true" onClick="SelectItemCheckbox(this,'chkAll');" />
                                            <%--onClick="SelectItemCheckbox(this,'chkAll');" --%>
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:BoundDataField DataFieldName="ID" Hidden="True" Key="ID" Width="50px">
                                        <Header Text="ID" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="STDATETIME" Key="STDATETIME" Width="100px">
                                        <Header Text="Inv. Date" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Site" Key="Site" Width="100px">
                                        <Header Text="Site" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Location" Key="Location" Width="150px">
                                        <Header Text="Location" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="AssetCount" Key="AssetCount" Width="100px">
                                        <Header Text="Asset Count" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="UnTaggedCount" Key="UnTaggedCount" Width="100px">
                                        <Header Text="UnTagged Count" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="ScannedCount" Key="ScannedCount" Width="100px">
                                        <Header Text="Scanned Count" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="MissingCount" Key="MissingCount" Width="100px">
                                        <Header Text="Missing Count" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="OverScannedCount" Key="OverScannedCount" Width="100px">
                                        <Header Text="Misplaced Count" />
                                    </ig:BoundDataField>
                                </Columns>
                                <Bands>
                                    <ig:Band Key="Level 1" AutoGenerateColumns="false" DataMember="ChildTable" DataKeyFields="AssetID"
                                        ShowHeader="true" Width="100%">
                                        <Columns>
                                            <ig:TemplateDataField Key="SelectChildCheckBox" Width="50px">
                                                <Header Text="Check" />
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkSelectChild" runat="server" onclick="checkChild();" />
                                                </ItemTemplate>
                                            </ig:TemplateDataField>
                                            <%-- <ig:UnboundCheckBoxField Key="ChkSelectChild" HeaderCheckBoxMode="Off" HeaderChecked="false" >
                                            </ig:UnboundCheckBoxField>--%>
                                            <ig:BoundDataField DataFieldName="STTYPE" Key="STTYPE" Width="100px">
                                                <Header Text="Type" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="Site" Key="Site" Width="100px">
                                                <Header Text="Site" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="Location" Key="Location" Width="150px">
                                                <Header Text="Location" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="RefNumber" Key="RefNumber" Width="120px">
                                                <Header Text="Serial No" />
                                            </ig:BoundDataField>
                                             <ig:BoundDataField DataFieldName="CurrentRFIDCardNumber" Key="CurrentRFIDCardNumber" Width="120px">
                                                <Header Text="Asset Tag" />
                                            </ig:BoundDataField>
                                             <ig:BoundDataField DataFieldName="StartPos" Key="StartPos" Width="70px">
                                                <Header Text="U-Position" />
                                            </ig:BoundDataField>
                                             <ig:BoundDataField DataFieldName="Orientation" Key="Orientation" Width="100px">
                                                <Header Text="Orientation" />
                                            </ig:BoundDataField>
                                             <ig:BoundDataField DataFieldName="ParentAssettag" Key="ParentAssettag" Width="120px">
                                                <Header Text="Parent Asset tag" />
                                            </ig:BoundDataField>
                                             <ig:BoundDataField DataFieldName="ValidationMsg" Key="ValidationMsg" Width="250px">
                                                <Header Text="Validation Message" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="AssetGroup" Key="AssetGroup" Width="120px">
                                                <Header Text="Asset Type" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="MfgName" Key="MfgName" Width="120px">
                                                <Header Text="Manufacture" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="ModelName" Key="ModelName" Width="120px">
                                                <Header Text="Model" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="Custodian" Key="Custodian" Width="100px">
                                                <Header Text="User" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="ID" Key="ID" Hidden="True" Width="100px">
                                                <Header Text="ID" />
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="AssetID" Key="AssetID" Hidden="True" Width="100px">
                                                <Header Text="AssetID" />
                                            </ig:BoundDataField>
                                             <ig:BoundDataField DataFieldName="IsValidated" Key="IsValidated" Hidden="True" Width="50px" DataType="Boolean" >
                                                <Header Text="IsValidated" />
                                            </ig:BoundDataField>
                                        </Columns>
                                        <Behaviors>
                                            <ig:EditingCore Enabled="true" AutoCRUD="false">
                                                <EditingClientEvents CellValueChanged="cellValueChanged" />
                                                <Behaviors>
                                                    <ig:CellEditing>
                                                        <ColumnSettings>
                                                            <ig:EditingColumnSetting ColumnKey="SelectChildCheckBox" ReadOnly="false" />
                                                            <ig:EditingColumnSetting ColumnKey="STTYPE" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="Site" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="Location" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="RefNumber" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="AssetGroup" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="MfgName" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="ModelName" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="Custodian" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="ID" ReadOnly="true" />
                                                            <ig:EditingColumnSetting ColumnKey="AssetID" ReadOnly="true" />
                                                        </ColumnSettings>
                                                        <CellEditingClientEvents EnteringEditMode="uwgAsset_CellEditing_EnteringEditMode" />
                                                    </ig:CellEditing>
                                                    <ig:RowEditingTemplate>
                                                    </ig:RowEditingTemplate>
                                                </Behaviors>
                                            </ig:EditingCore>
                                        </Behaviors>
                                    </ig:Band>
                                </Bands>
                                <Behaviors>
                                    <ig:Activation Enabled="true">
                                    </ig:Activation>
                                    <ig:Selection RowSelectType="Single" CellClickAction="Row" CellSelectType="Single">
                                        <SelectionClientEvents RowSelectionChanged="uwgAsset_RowSelectionChanged" />
                                    </ig:Selection>
                                    <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
                                        <PagerTemplate>
                                            <uc1:CustomerPagerControl ID="CustomerPager" runat="server" Visible="true" />
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
                            </ig:WebHierarchicalDataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="hdnUserName" runat="server" style="width: 25px" type="hidden" />
                            <input id="hdnUserID" runat="server" style="width: 25px" type="hidden" />
                            <input id="hdnIDs" runat="server" style="width: 255px" type="hidden" />
                            <input id="hdnAssetIDs" runat="server" style="width: 255px" type="hidden" />
                            <input id="hdnPageSelectionsApp" runat="server" style="width: 255px" type="hidden" />
                            <input id="hdnSelectedID" runat="server" style="width: 255px" type="hidden" />
                            <input id="hdnSeletedRowID" runat="server" style="width: 255px" type="hidden" />
                            <input id="hdnSelectedChildIDs" runat="server" style="width: 255px" type="hidden" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
</asp:Content>
