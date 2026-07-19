<%@ Page Language="C#" AutoEventWireup="true" Theme="SkinFile" CodeFile="ApplicationMap.aspx.cs"
    Inherits="ApplicationMap" MasterPageFile="~/iAssetTrackMasterPage.master" Title="Asset-Application Map"
    Culture="auto" meta:resourcekey="PageResource2" UICulture="auto" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="RptAssetmvmt" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        //<!--
        var isChildRowSelected;
        var isSelectedCellCheckBox

        function uwgAsset_CellSelectionChanged(sender, args) {
            if (args.getSelectedCells().getItem(0).get_row().get_rowIslands()[0]) {
                if (args.getSelectedCells().getItem(0).get_index() == 0) {
                    // checkbox cell
                    isSelectedCellCheckBox = true;
                }
                else {
                    //other cells, select row.
                    isSelectedCellCheckBox = false;
                }
            }
        }

        function uwgAsset_RowSelectionChanged(sender, args) {
            var selectedRowID = document.getElementById('<%=hdnSeletedRowID.ClientID %>');
            selectedRowID.value = '';
            if (args.getSelectedRows().getItem(0) != null && args.getSelectedRows().getItem(0).get_rowIslands()[0]) {
                isChildRowSelected = false;

                selectedRowID.value = args.getSelectedRows().getItem(0).get_index();
                //check all its childs(Applications)
                // Get the Infragistics web grid.
                //                var webGrid = $find("<%=uwgAsset.ClientID%>");
                //                if (webGrid == null)
                //                    window.status = "Web grid is not initialized.";
                //                else {
                //                    var isItemChecked;
                //                    var icheck = 0;
                //                    var idischeck = 0;
                //                    var iuncheck = 0;
                //                    var icount = 0;
                //                    var row, cell, elm, childElm;
                //                    var childRow, childCell, childElm, subChildElm;
                //                    var i, j;
                //                    var item;
                //                    var selected = new Array();

                //                    // Get the row of this row index.
                //                    var currentRowIndex = selectedRowID.value;
                //                    if (currentRowIndex != "") {
                //                        // Iterate each row in the grid 
                //                        // to find the rows that are checked.
                //                        for (i = 0; i < webGrid.get_gridView().get_rows().get_length(); i++) {
                //                            if (i == currentRowIndex) {
                //                                row = webGrid.get_gridView().get_rows().get_row(i);

                //                                var childBand = row.get_rowIslands()[0]; // child band
                //                                if (childBand != null) {
                //                                    ichkcount = 0;
                //                                    for (cRow = 0; cRow < childBand.get_rows().get_length(); cRow++) {
                //                                        // Get the cell containing the check box in this row.
                //                                        childRow = childBand.get_rows().get_row(cRow);
                //                                        childCell = childRow.get_cellByColumnKey("SelectChildCheckBox");
                //                                        if (childCell == null)
                //                                            window.status = "Cell is not initialized.";
                //                                        else {
                //                                            // Get the HTML element of the cell containing the check box.
                //                                            childElm = childCell.get_element();
                //                                            if (childElm == null)
                //                                                window.status = "Cell element is not initialized.";
                //                                            else {
                //                                                // Iterate the child elements under the cell element
                //                                                // to find the check box.
                //                                                for (j = 0; j < childElm.children.length; j++) {
                //                                                    subChildElm = childElm.children[j];
                //                                                    if (subChildElm.type == "checkbox") {
                //                                                        // Found the check box in the cell.
                //                                                        if (!subChildElm.checked) {
                //                                                            subChildElm.checked = true;
                //                                                            ichkcount = ichkcount + 1
                //                                                        }
                //                                                        break;
                //                                                    }
                //                                                }

                //                                            }
                //                                        }
                //                                    }
                //                                    //now check the header row
                //                                    if (childBand.get_rows().get_length() == ichkcount) {
                //                                        childBand.get_columns().get_column(0).get_headerElement().children[0].checked = true; //chkAllChild
                //                                    }
                //                                    else {
                //                                        childBand.get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAllChild
                //                                    }
                //                                }
                //                            }

                //                        }
                //                    }
                //                    // now show message stating all child are checked, uncheck to remove application
                //                    alert('All Application(s) running under this host are checked, uncheck application(s) which needs to be removed!');
                //                }
                //            }
                //            
            }
            else {
                isChildRowSelected = true;
                if (args.getSelectedRows().getItem(0) != null) {
                    args.getSelectedRows().clear();
                }
            }

        }
        function ibDelete_Click(oButton, oEvent) {
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
                var chkdAppCount = 0;
                var unchkdAppCount = 0;

                // Get the row of this row index.
                var selectedRowID = document.getElementById('<%=hdnSeletedRowID.ClientID %>');
                var currentRowIndex = selectedRowID.value;
                if (currentRowIndex != "") {
                    // Iterate each row in the grid 
                    // to find the rows that are checked.
                    for (i = 0; i < webGrid.get_gridView().get_rows().get_length(); i++) {
                        if (i == currentRowIndex) {
                            row = webGrid.get_gridView().get_rows().get_row(i);
                            var assignmentID = row.get_cell(10).get_value(); //AssignmentID
                            var selectedAssetID = document.getElementById('<%=hdnSelectedAssetID.ClientID %>');
                            selectedAssetID.value = assignmentID;
                            var childBand = row.get_rowIslands()[0]; // child band
                            if (childBand != null) {
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
                                                        var item = childRow.get_cell(1).get_value();
                                                        if (selected.length == 0)
                                                            selected[0] = item;
                                                        else
                                                            selected[selected.length] = item;

                                                        chkdAppCount++;
                                                    }
                                                    else {
                                                        unchkdAppCount++;
                                                    }
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                }

                            }
                            var selectedApplIDs = document.getElementById('<%=hdnSelectedChildIDs.ClientID %>');
                            var selectedAssetID = document.getElementById('<%=hdnSelectedAssetID.ClientID %>');
                            if (selected.length > 0) {
                                var selectedItems = selected.join(",").toString();
                                selectedApplIDs.value = selectedItems;
                            }
                            else {
                                selectedApplIDs.value = "";
                            }
                            //Add code to handle your event here.
                            if (selectedAssetID.value != "") {

                                if (selectedApplIDs.value != "") {
                                    if (unchkdAppCount == 0) {
                                        alert('No Application(s) unchecked, un-check application(s) to un-map');
                                        oEvent.cancel = true;
                                    }
                                    else {
                                        return ValidateDeletionApplicationMap(oButton, oEvent, 'All unchecked applications will be removed, want to continue?');
                                    }
                                }
                                else {
                                    return ValidateDeletionApplicationMap(oButton, oEvent, 'No Application is checked, all applications will be removed, want to continue?');
                                }
                            }
                            else {
                                alert('Select a Asset/Host row with un-checked applcation(s) to remove mapping');
                                oEvent.cancel = true;

                            }
                        }

                    }
                }
                else {
                    alert('Select a parent row with childs');
                    oEvent.cancel = true;
                }

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
                var childBand = row.get_rowIslands()[0]; // child band
                if (childBand != null) {
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
                                            ichkcount = ichkcount + 1;
                                        }
                                        else {
                                        }
                                        if (childBand.get_rows().get_length() == ichkcount) {
                                            childBand.get_columns().get_column(0).get_headerElement().children[0].checked = true; //chkAllChild
                                        }
                                        else {
                                            childBand.get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAllChild
                                        }
                                    }
                                }
                            }
                        }

                    }

                    //                    if (childBand.get_rows().get_length() == ichkcount) {

                    //                        cell = row.get_cellByColumnKey("SelectCheckBox");
                    //                        if (cell.get_element().children[0].type == "checkbox")
                    //                            cell.get_element().children[0].checked = true;
                    //                    }
                    //                    else {
                    //                        cell = row.get_cellByColumnKey("SelectCheckBox");
                    //                        if (cell.get_element().children[0].type == "checkbox")
                    //                            cell.get_element().children[0].checked = false;
                    //                    }
                }

            }
            //main header
            //            var checkedCount = 0;
            //            for (i = 0; i < webGrid.get_gridView().get_rows().get_length(); i++) {
            //                var mainRow = webGrid.get_gridView().get_rows().get_row(i);
            //                var chkBox = mainRow.get_cellByColumnKey("SelectCheckBox").get_element().children[0];
            //                if (chkBox.type == "checkbox" && chkBox.checked == true) {
            //                    checkedCount = checkedCount + 1;
            //                }
            //            }
            //            if (webGrid.get_gridView().get_rows().get_length() == checkedCount) {

            //                webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = true; //chkAll
            //            }
            //            else {

            //                webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAll
            //            }

        }

        function SelectAllCheckboxesApp(spanChk, name) {

            // Added as ASPX uses SPAN for
            var row, cell, elmMain, elm, childElm;
            var i, k, j;
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
                        if (elmMain[i].checked != xState) {
                            elmMain[i].checked = xState;
                        }
                        if (elmMain[i].disabled == true) {
                            elmMain[i].checked = false;
                            idischeck += 1;
                        }
                        ichkcount += 1;
                    }
                }
            }
            var webGrid = $find("<%=uwgApps.ClientID%>");
            for (k = 0; k < webGrid.get_rows().get_length(); k++) {
                // Get the row of this row index.
                row = webGrid.get_rows().get_row(k);
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
                                    var item = row.get_cell(1).get_value();
                                    //                                    alert(item);
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
            var pageSelectionField = document.getElementById('<%=hdnPageSelectionsApp.ClientID %>');
            if (selected.length > 0) {
                var selectedItems = selected.join(",").toString();
                pageSelectionField.value = "";
                pageSelectionField.value = selectedItems;
                //             alert(pageSelectionField.value);
            }
            else {
                pageSelectionField.value = "";
            }

            if (ichkcount == idischeck) {
                webGrid.get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAll
            }
        }


        function SelectItemCheckboxApp(spanChk, name) {

            // Get the Infragistics web grid.
            var webGrid = $find("<%=uwgApps.ClientID%>");

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
                for (i = 0; i < webGrid.get_rows().get_length(); i++) {
                    // Get the row of this row index.
                    row = webGrid.get_rows().get_row(i);

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
                                        icheck += 1;
                                        icount += 1;
                                        // Check box is selected.
                                        item = row.get_cell(1).get_value();
                                        //  alert(item);
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

                var pageSelectionField = document.getElementById('<%=hdnPageSelectionsApp.ClientID %>');
                if (selected.length > 0) {
                    var selectedItems = selected.join(",").toString();
                    pageSelectionField.value = "";
                    pageSelectionField.value = selectedItems;
                    //                    alert(pageSelectionField.value);
                }
                else {
                    pageSelectionField.value = "";
                }

                if (icount == icheck + idischeck)
                    webGrid.get_columns().get_column(0).get_headerElement().children[0].checked = true; //chkAll
                else
                    webGrid.get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAll
            }
        }

        function SelectAllChildCheckboxes(spanChk, name) {

            // Added as ASPX uses SPAN for checkbox
            var i, j, k;
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
                        }
                    }
                }
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
                ichkcount = 0;
                cell = row.get_cellByColumnKey("SelectCheckBox");
                if (cell == null)
                    window.status = "Cell is not initialized.";
                else {
                    // Get the HTML element of the cell containing the check box.
                    childElm = cell.get_element();
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
                                    var item = row.get_cell(10).get_value(); // AssignmentID
                                    //                                    alert(item);
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

            var pageSelectionField = document.getElementById('<%=hdnPageSelections.ClientID %>');
            if (selected.length > 0) {
                var selectedItems = selected.join(",").toString();
                pageSelectionField.value = "";
                pageSelectionField.value = selectedItems;
                //                              alert(pageSelectionField.value);
            }
            else {
                pageSelectionField.value = "";
            }

        }


        function SelectItemCheckbox(spanChk, name) {

            // Get the Infragistics web grid.
            var webGrid = $find("<%=uwgAsset.ClientID%>");
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

                for (k = 0; k < webGrid.get_gridView().get_rows().get_length(); k++) {
                    // Get the row of this row index.
                    row = webGrid.get_gridView().get_rows().get_row(k);
                    ichkcount = 0;
                    cell = row.get_cellByColumnKey("SelectCheckBox");
                    if (cell == null)
                        window.status = "Cell is not initialized.";
                    else {
                        // Get the HTML element of the cell containing the check box.
                        childElm = cell.get_element();
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
                                        icheck += 1;
                                        icount += 1;
                                        // Check box is selected.
                                        var item = row.get_cell(10).get_value(); // AssignmentID
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
                                    if (subChildElm.disabled == true) {
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
                    pageSelectionField.value = "";
                    pageSelectionField.value = selectedItems;
                    //                    alert(pageSelectionField.value);
                }
                else {
                    pageSelectionField.value = "";
                }

                if (icount == icheck + idischeck)
                    webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = true; //chkAll
                else
                    webGrid.get_gridView().get_columns().get_column(0).get_headerElement().children[0].checked = false; //chkAll
            }
        }


        function ibCreate_JS_Click(oButton, oEvent) {
            document.getElementById('<%=lblMsg.ClientID%>').innerHTML = "";
        }

        // -->
    </script>
    <table cellpadding="0" cellspacing="0" style="vertical-align: top; width: 97%;">
        <tr>
            <%-- <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ></asp:ScriptManager>--%>
            <td valign="top" style="width: 20%; vertical-align: top; text-align: left;">
                <table width="100%">
                    <tr>
                        <td colspan="2" valign="top" align="left" style="text-align: left;">
                            <asp:Label ID="lblHost" runat="server" CssClass="displayText" meta:resourcekey="lblHostResource1"
                                Width="300px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="top">
                            <asp:TextBox ID="txtHost" runat="server" MaxLength="100" CssClass="FieldValue" Width="175px"
                                meta:resourcekey="txtHostResource1"></asp:TextBox><br />
                            <asp:RegularExpressionValidator ID="revHostName" runat="server" ControlToValidate="txtHost"
                                CssClass="ErrValStyle" Display="dynamic" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                Height="15px" Width="166px" meta:resourcekey="revHostNameResource1"></asp:RegularExpressionValidator>
                            <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoComplete1" TargetControlID="txtHost"
                                ServiceMethod="getHostNames" MinimumPrefixLength="1" DelimiterCharacters="" Enabled="True"
                                ServicePath="">
                            </ajaxToolkit:AutoCompleteExtender>
                            <br />
                            <igtxt:WebImageButton ID="btnGetAssets" runat="server" UseBrowserDefaults="False"
                                OnClick="btnGetAssets_Click" TabIndex="13" SkinID="uwButton" ImageDirectory=""
                                meta:resourcekey="btnGetAssetsResource1">
                            </igtxt:WebImageButton>
                            <%--<asp:Button ID="btnGetAssets" runat="server" Text="Show Assets" OnClick="btnGetAssets_Click" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:Panel ID="Panel1" ScrollBars="Both" runat="server" Height="408px" Style="margin-left: 0px"
                                Width="302px">--%>
                            <ig:WebDataTree ID="TreeLocation" runat="server" Height="736px" Width="285px" Font-Bold="True"
                                Font-Size="X-Small" CheckBoxMode="BiState" BorderStyle="Solid" BorderColor="#999999"
                                meta:resourcekey="TreeLocationResource1">
                            </ig:WebDataTree>
                            <%--</asp:Panel>--%>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" style="width: 80%; vertical-align: top; text-align: left;">
                <table>
                    <tr>
                        <td align="left" colspan="2" valign="top" style="width: 350px;">
                            <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                TabIndex="4" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                <ClientSideEvents Click="ibCreate_JS_Click" />
                            </igtxt:WebImageButton>
                            &nbsp;
                            <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" TabIndex="5" meta:resourcekey="ibResetResource1">
                            </igtxt:WebImageButton>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                TabIndex="9" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                meta:resourcekey="ibDeleteResource1">
                                <ClientSideEvents Click="ibDelete_Click" />
                            </igtxt:WebImageButton>
                        </td>
                        <%--<td valign="top">
                            
                        </td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMsg" runat="server" CssClass="ErrValStyle" meta:resourcekey="lblMsgResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAssetList" runat="server" CssClass="Heading" Visible="False" meta:resourcekey="lblAssetListResource1"></asp:Label>
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
                                    <td align="right" style="width: 50%" valign="middle">
                                        <asp:Label ID="lblLegend" runat="server" CssClass="FieldName" Text="Legends :" Visible="False"
                                            meta:resourcekey="lblLegendResource1"></asp:Label>
                                    </td>
                                    <td style="width: 100%" valign="middle">
                                    </td>
                                    <td style="width: 100%" valign="middle">
                                        &nbsp;
                                    </td>
                                    <td style="width: 100%" valign="middle">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" colspan="1" style="width: 2%; height: 11px" valign="top">
                            &nbsp;
                        </td>
                        <td align="left" colspan="1" style="width: 2%; height: 11px" valign="top">
                            <asp:ImageButton ID="ibExport" runat="server" AlternateText="Export" ImageUrl="~/icons/excelsmall.gif"
                                OnClick="ibExport_Click" Visible="False" meta:resourcekey="ibExportResource1" />
                            <igtxt:WebImageButton ID="ibExportToExcel" runat="server" OnClick="ibExportToExcel_Click"
                                SkinID="uwButton" TabIndex="9" UseBrowserDefaults="False" CausesValidation="False"
                                ImageDirectory="" Visible="False" meta:resourcekey="ibExportToExcelResource1">
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <ig:WebHierarchicalDataGrid ID="uwgAsset" runat="server" Height="320px" TabIndex="15"
                                Width="100%" AutoGenerateColumns="False" DataKeyFields="ID" EnableAjax="false"
                                InitialDataBindDepth="-1" Key="Level 0" OnDataBound="uwgAsset_DataBound" DataMember="ParentTable"
                                EnableRelativeLayout="True" OnInitializeRow="uwgAsset_InitializeRow" OnRowIslandDataBinding="uwgAsset_RowIslandDataBinding">
                                <Columns>
                                    <ig:TemplateDataField Key="SelectCheckBox" Width="50px">
                                        <HeaderTemplate>
                                            <input id="chkAll" runat="server" type="checkbox" onclick="SelectAllCheckboxes(this,'ChkSelect');" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkSelect" runat="server" Enabled="true" onClick="SelectItemCheckbox(this,'chkAll');" />
                                            <%--onClick="SelectItemCheckbox(this,'chkAll');" --%>
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:BoundDataField DataFieldName="AssetID" Hidden="True" Key="AssetID" Width="40px">
                                        <Header Text="AssetID"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="RefNumber" Key="RefNumber" Width="120px">
                                        <Header Text="Serial Number"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="RFID Card No" Hidden="True" Key="RFIDTagID" Width="18px">
                                        <Header Text="Barcode/EPC Tag"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="MfgName" Key="MfgName" Width="100px">
                                        <Header Text="Manufacturer"></Header>
                                        <Footer></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="ModelName" Key="ModelName" Width="120px">
                                        <Header Text="Model"></Header>
                                        <Footer></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="SPCModel" Hidden="True" Key="SPCModel" Width="120px">
                                        <Header Text="SPC Model"></Header>
                                        <Footer></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Primary Site" Key="Site" Width="80px">
                                        <Header Text="Site"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Location" Key="Location" Width="80px">
                                        <Header Text="Location"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="HostID" Hidden="True" Key="HostID" Width="40px">
                                        <Header Text="HostID"></Header>
                                        <Footer></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="ID" Hidden="True" Key="ID">
                                        <Header Text="ID"></Header>
                                        <Footer></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="HostName" Key="HostName">
                                        <Header Text="HostName"></Header>
                                        <Footer></Footer>
                                    </ig:BoundDataField>
                                </Columns>
                                <Behaviors>
                                    <ig:Selection RowSelectType="Single" CellClickAction="Row" CellSelectType="Single">
                                        <SelectionClientEvents RowSelectionChanged="uwgAsset_RowSelectionChanged" CellSelectionChanged="uwgAsset_CellSelectionChanged" />
                                    </ig:Selection>
                                </Behaviors>
                                <Bands>
                                    <ig:Band Key="Level 1" AutoGenerateColumns="false" DataMember="ChildTable" DataKeyFields="ApplID"
                                        ShowHeader="true" Width="100%">
                                        <Columns>
                                            <ig:TemplateDataField Key="SelectChildCheckBox" Width="50px">
                                                <Header Text="Check" />
                                                <HeaderTemplate>
                                                    <input id="chkAllChild" runat="server" onclick="SelectAllChildCheckboxes(this,'ChkChild');"
                                                        type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkChild" runat="server" onclick="checkChild();" />
                                                </ItemTemplate>
                                            </ig:TemplateDataField>
                                            <ig:BoundDataField DataFieldName="ApplID" Key="ApplID" Hidden="True">
                                                <Header></Header>
                                                <Footer></Footer>
                                            </ig:BoundDataField>
                                            <ig:BoundDataField DataFieldName="ApplName" Key="ApplName" Width="150px">
                                                <Header Text="Application"></Header>
                                                <Footer></Footer>
                                            </ig:BoundDataField>
                                        </Columns>
                                    </ig:Band>
                                </Bands>
                            </ig:WebHierarchicalDataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:Label ID="lblApplication" runat="server" CssClass="Heading" meta:resourcekey="lblApplicationResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <ig:WebDataGrid ID="uwgApps" runat="server" Height="300px" TabIndex="15" EnableViewState="False"
                                AutoGenerateColumns="false" Visible="False" Width="100%" DataKeyFields="ApplID"
                                OnInitializeRow="uwgApps_InitializeRow" OnDataBound="uwgApps_DataBound" OnDataFiltered="uwgApps_DataFiltered"
                                OnItemCommand="uwgApps_ItemCommand" HeaderCaptionCssClass="GridHeader">
                                <Columns>
                                    <ig:TemplateDataField Key="SelectCheckBox" Width="50px">
                                        <Header Text="Check" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllApp" runat="server" onclick="SelectAllCheckboxesApp(this,'ChkApp');" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkApp" runat="server" onclick="SelectItemCheckboxApp(this,'chkAllApp');" />
                                        </ItemTemplate>
                                    </ig:TemplateDataField>
                                    <ig:BoundDataField DataFieldName="ApplID" Hidden="True" Key="ApplID" Width="20px">
                                        <Header Text="ApplID"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="ApplName" Key="ApplName" Width="265px">
                                        <Header Text="Application"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="ApplType" Key="ApplType" Width="200px">
                                        <Header Text="Application Type"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="ApplStatus" Key="ApplStatus" Width="100px">
                                        <Header Text="Application Status"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Owner" Key="Owner" Width="200px">
                                        <Header Text="Owner"></Header>
                                        <Footer Text=""></Footer>
                                    </ig:BoundDataField>
                                </Columns>
                                <Behaviors>
                                    <ig:Filtering>
                                    </ig:Filtering>
                                    <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
                                        <PagerTemplate>
                                            <%--<asp:Label ID="lblTotRecordCount" runat="server" Style="text-align: right;">Total record count:<%=totalRecordCount %></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                            <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
                                            <%--  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblFilRecordCount" runat="server" Style="text-align: right;">Total filtered record count:<%=hdnFilterCount.Value%></asp:Label>--%>
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
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input id="hdnUserName" runat="server" style="width: 25px" type="hidden" />
    <input id="hdnUserID" runat="server" style="width: 25px" type="hidden" />
    <input id="hdnPageSelections" runat="server" style="width: 255px" type="hidden" />
    <input id="hdnPageSelectionsApp" runat="server" style="width: 255px" type="hidden" />
    <input id="hdnSelectedAssetID" runat="server" style="width: 255px" type="hidden" />
    <input id="hdnSeletedRowID" runat="server" style="width: 255px" type="hidden" />
    <input id="hdnSelectedChildIDs" runat="server" style="width: 255px" type="hidden" />
    <input id="hdnFilterCount" runat="server" style="width: 255px" type="hidden" />
</asp:Content>
