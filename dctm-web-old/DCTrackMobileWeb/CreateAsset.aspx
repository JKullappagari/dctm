<%@ Page Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master" Theme="SkinFile"
    AutoEventWireup="true" CodeFile="CreateAsset.aspx.cs" Inherits="CreateAsset"
    Title="Create New Asset" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script id="Infragistics" type="text/javascript">

</script>
    <script id="popup" type="text/javascript">
        //<!--
        //Gets a lookup window for user search
        function searchOriginatorPopup() {
            var url = "UserSearchPopup.aspx?BusinessUnit=" + document.forms[0].elements['<%=ddlBusinessUnit.ClientID%>'].value;
            url = url + "&DisplayNameField=" + document.getElementById("<%=txtOwner.ClientID %>").id;
            url = url + "&IDField=" + document.getElementById("<%=hdnOwnerID.ClientID %>").id;
            url = url + "&NameField=" + document.getElementById("<%=hdnOwnerName.ClientID %>").id;

            //alert(url);

            winSettings = "scroll:auto; width=880; height=470;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
            var hWnd = open_window(url, "searchsubcon", winSettings)


            //var hWnd=window.open(url,"","toolbar,status,scrollbars,resizable,height=420,width=700,screenx=0")
            if ((document.window != null) && (hWnd.opener))
                hWnd.opener = document.window;


        }
        //*Added on 17Apr2013
        function searchParentAssetPopup() {
            var ctrlMfg = document.getElementById('<%=ddlMfg.ClientID%>');
            var ctrlBU = document.getElementById('<%=ddlBusinessUnit.ClientID%>');
            var ctrlModelName = document.getElementById('<%=hdnModelName.ClientID%>');
            var flagIsBlade = document.getElementById('<%=hdnIsBlade.ClientID%>');
            var locID = document.getElementById('<%=hdnLocID.ClientID%>').value;
            var siteID = document.getElementById('<%=hdnSiteID.ClientID%>').value;

            if (ctrlBU.options[ctrlBU.selectedIndex].value == "0") {
                alert('Select BusineesUnit');
            }
            else {
                if (ctrlMfg.options[ctrlMfg.selectedIndex].value == "0") {
                    alert('Select Manufacturer');
                }
                else {
                    if (ctrlModelName.value == "" || ctrlModelName.value == "No Model") {
                        alert('Select Asset Model');
                    }
                    else {
                        if (flagIsBlade.value == "True") {
                            var url = "ParentAssetSearchPage.aspx?BusinessUnit=" + document.forms[0].elements['<%=ddlBusinessUnit.ClientID%>'].value + "&MFG=" + document.forms[0].elements['<%=ddlMfg.ClientID%>'].value
                                + "&LID=" + locID + "&SID=" + siteID;
                            winSettings = "scroll:auto; width=990; height=550;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                            var hWnd = open_window(url, "searchsubcon", winSettings)
                            if ((document.window != null) && (hWnd.opener))
                                hWnd.opener = document.window;

                        }
                        else {
                            alert('Asset is not a Blade/Module');
                        }

                    }
                }
            }

        }
        function PopUpWindowCallBack(id, name, Site, locId, locname, ParentAssetId) {
            //*V3.8-Added on 23-Oct-2013- By Amar Vidya 
            if (ParentAssetId > 0) {
                alert("Cannot be assigned!.Selected asset has a Parent")
                //                document.getElementById('<%=hdnLocID.ClientID%>').value = "";
                //                document.getElementById('<%=txtLocation.ClientID%>').value = "";
                document.getElementById('<%=txtParentAsset.ClientID%>').value = "";
                document.getElementById('<%=hdnParentAssetID.ClientID%>').value = "";
                document.getElementById('<%=ddlSite.ClientID%>').disabled = false;
                document.getElementById('<%=ddlSite.ClientID%>').selectedIndex = 0;


            }
            else {//*

                document.getElementById('<%=hdnParentAssetName.ClientID%>').value = name;
                document.getElementById('<%=txtParentAsset.ClientID%>').value = name;
                document.getElementById('<%=hdnParentAssetID.ClientID%>').value = id;

                document.getElementById('<%=hdnSiteID.ClientID%>').value = Site;
                document.getElementById('<%=ddlSite.ClientID%>').value = Site;

                document.getElementById('<%=ddlSite.ClientID%>').disabled = true;
                document.getElementById('<%=imgLocButton.ClientID%>').className = "disabled";
                document.getElementById('<%=imgRemoveParent.ClientID%>').disabled = false;

                document.getElementById('<%=hdnLocID.ClientID%>').value = locId;
                document.getElementById('<%=txtLocation.ClientID%>').value = locname;
                document.getElementById('<%=hdnLocName.ClientID%>').value = locname;
                document.getElementById("<%=ddlOrientation.ClientID%>").selectedIndex = 0;
                document.getElementById("<%=ddlPosition.ClientID%>").selectedIndex = 0;
                document.getElementById('<%=btnParentLoad.ClientID%>').click();

            }
        }

        function imgRemoveParent_Click() {
            document.getElementById('<%=txtParentAsset.ClientID%>').value = "";
            document.getElementById('<%=hdnParentAssetName.ClientID%>').value = "";
            document.getElementById('<%=hdnParentAssetID.ClientID%>').value = "";
            document.getElementById('<%=ddlSite.ClientID%>').disabled = false;

            //            Location selection image will be disable in Edit Asset Page
            document.getElementById('<%=imgLocButton.ClientID%>').className = "enabled";

            document.getElementById('<%=imgRemoveParent.ClientID%>').disabled = true;
        } //*

        //-->

    </script>
    <script type="text/javascript">
        //<!--
        function ibCreate_JS_Click(oButton, oEvent) {
            if (Page_ClientValidate()) {
                //Add code to handle your event here.
                var throwError = false;
                var throwError1 = false;
                var pos = [];
                var locationtype = '<%=this.LocationType.ToString()%>';
                var mounttype = '<%=this.MountType.ToString()%>';
                var orn = $('#<%=ddlOrientation.ClientID %> option:selected').text();

                if (locationtype == "Rack") {

                    var modeltype = '<%=this.ModelType.ToString() %>';


                    if (locationtype != modeltype) {

                        var startpos = $('#<%=ddlPosition.ClientID %> option:selected').text();
                        var noofrus = '<%=this.NoOfRUs.ToString()%>';
                        var isblade = '<%=this.IsBlade.ToString()%>';

                        if (isblade == 'True') {
                            var parentid = $('#<%=hdnParentAssetID.ClientID%>').val();
                            if (parentid == null || parentid == '')
                                parentid = 0;
                            if (orn == "-Select-") {
                                alert('Select Orientation');
                                oEvent.cancel = true;
                            }
                            else {

                                if (orn == "Front" || orn == "Rear") {
                                    if (parentid < 1) {
                                        alert('Select Parent Asset');
                                        oEvent.cancel = true;
                                    }
                                    else {
                                        if (startpos == "-Select-") {
                                            alert('Select Position');
                                            oEvent.cancel = true;
                                        }
                                        else {
                                            //check whether positions are available in the position list
                                            if (orn == "Front") {
                                                if (parseInt('<%=this.BladeRowCount.ToString()%>') > parseInt('<%=this.EnclFrontRowCount.ToString()%>') ||
                                                    parseInt('<%=this.BladeColCount.ToString()%>') > parseInt('<%=this.EnclFrontColCount.ToString()%>')) {
                                                    throwError1 = true;
                                                }
                                                else {
                                                    for (var enclr = 1; enclr <= parseInt('<%=this.EnclFrontRowCount.ToString()%>'); enclr++) {
                                                        if (parseInt(startpos) <= (parseInt('<%=this.EnclFrontColCount.ToString()%>') * enclr)) {
                                                            for (var bc = 1; bc <= parseInt('<%=this.BladeColCount.ToString()%>'); bc++) {
                                                                if ((parseInt(startpos) + bc - 1) > (parseInt('<%=this.EnclFrontColCount.ToString()%>') * enclr)) {
                                                                    throwError1 = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (orn == "Rear") {
                                                if (parseInt('<%=this.BladeRowCount.ToString()%>') > parseInt('<%=this.EnclRearRowCount.ToString()%>') ||
                                                    parseInt('<%=this.BladeColCount.ToString()%>') > parseInt('<%=this.EnclRearColCount.ToString()%>')) {
                                                    throwError1 = true;
                                                }
                                                else {
                                                    for (var enclr = 1; enclr <= parseInt('<%=this.EnclRearRowCount.ToString()%>'); enclr++) {
                                                        if (parseInt(startpos) <= (parseInt('<%=this.EnclRearColCount.ToString()%>') * enclr)) {
                                                            for (var bc = 1; bc <= parseInt('<%=this.BladeColCount.ToString()%>'); bc++) {
                                                                if ((parseInt(startpos) + bc - 1) > (parseInt('<%=this.EnclRearColCount.ToString()%>') * enclr)) {
                                                                    throwError1 = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (!throwError1) {
                                                $('#<%=ddlPosition.ClientID %> option').each(function () {
                                                    pos.push($(this).attr('value'))
                                                });
                                                if (orn == "Front") {

                                                    for (var r = 0; r < parseInt('<%=this.BladeRowCount.ToString()%>'); r++) {
                                                        for (var c = 0; c < parseInt('<%=this.BladeColCount.ToString()%>'); c++) {

                                                            if ($.inArray((parseInt(startpos) + (parseInt('<%=this.EnclFrontColCount.ToString()%>') * r) + c).toString(), pos) == -1) {
                                                                //value not in the list
                                                                throwError = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (orn == "Rear") {
                                                    for (var r = 0; r < parseInt('<%=this.BladeRowCount.ToString()%>'); r++) {
                                                        for (var c = 0; c < parseInt('<%=this.BladeColCount.ToString()%>'); c++) {

                                                            if ($.inArray((parseInt(startpos) + (parseInt('<%=this.EnclRearColCount.ToString()%>') * r) + c).toString(), pos) == -1) {
                                                                //value not in the list
                                                                throwError = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (throwError) {
                                                    alert('Selected contiguous bay positions not available in this enclosure');
                                                    oEvent.cancel = true;
                                                }
                                            }
                                            else {
                                                alert('Blade is not compatible with selected enclosure');
                                                oEvent.cancel = true;
                                            }
                                        }
                                    }
                                }
                                else {
                                    alert('Orientation must be Front or Rear for Blade/Module');
                                    oEvent.cancel = true;
                                }

                            }
                        }
                        else if (mounttype.indexOf('Standalone') != -1) {
                            //standalone - no position
                            if (orn == "-Select-") {
                                alert('Select Orientation');
                                oEvent.cancel = true;
                            }
                            else if (orn != "Front" && orn != "Rear") {
                                alert('For Standalone, orientation must be either Front or Rear');
                                oEvent.cancel = true;
                            }
                            else {
                                if (locationtype == 'Rack') {
                                    alert('Standalone can\'t be placed inside a rack');
                                    oEvent.cancel = true;
                                }

                            }
                        }
                        else if (mounttype.indexOf('Vertical') != -1) {
                            if (orn == "-Select-") {
                                alert('Select Orientation');
                                oEvent.cancel = true;
                            }
                            else {
                                if (orn == "Front" || orn == "Rear") {
                                    alert('For Vertical Mount, orientation can\'t be Front or Rear');
                                    oEvent.cancel = true;
                                }
                            }
                        }
                        else {

                            if (orn == "-Select-") {
                                alert('Select Orientation');
                                oEvent.cancel = true;
                            }
                            else {

                                if (orn != "Front" && orn != "Rear") {
                                    alert('For RackMount or Stack In Rack, Orientation must be Front or Rear');
                                    oEvent.cancel = true;
                                }
                                else if (startpos == "-Select-") {
                                    alert('Select Start Position');
                                    oEvent.cancel = true;
                                }
                                else {
                                    $('#<%=ddlPosition.ClientID %> option').each(function () {
                                        pos.push($(this).attr('value'))
                                    });
                                    var uh = parseInt(startpos) + parseInt(noofrus);
                                    for (var i = startpos; i < uh; i++) {
                                        if ($.inArray(i.toString(), pos) == -1) {
                                            //value not in the list
                                            throwError = true;
                                            break;
                                        }
                                    }
                                    if (throwError) {
                                        alert('Selected contiguous U-space is not available in this rack');
                                        oEvent.cancel = true;
                                    }

                                }

                            }
                        }



                    }
                    else {
                        alert('Rack asset can\'t be placed inside a Rack');
                        oEvent.cancel = true;
                    }
                }
                else {
                    if (mounttype.indexOf('Standalone') != -1) {
                        //standalone - no position
                        if (orn == "-Select-") {
                            alert('Select Orientation');
                            oEvent.cancel = true;
                        }
                        else if (orn != "Front" && orn != "Rear") {
                            alert('For Standalone, orientation must be either Front or Rear');
                            oEvent.cancel = true;
                        }
                    }
                    else if (mounttype.indexOf('Vertical') != -1) {
                        if (orn == "-Select-") {
                            alert('Select Orientation');
                            oEvent.cancel = true;
                        }
                        else {
                            if (orn == "Front" || orn == "Rear") {
                                alert('For Vertical Mount, orientation can\'t be Front or Rear');
                                oEvent.cancel = true;
                            }
                        }
                    }
                    else {
                        if (orn == "-Select-") {
                            alert('Select Orientation');
                            oEvent.cancel = true;
                        }
                        else {

                            var startpos = $('#<%=ddlPosition.ClientID %> option:selected').text();
                            var isblade = '<%=this.IsBlade.ToString()%>';
                            var parentid = $('#<%=hdnParentAssetID.ClientID%>').val();
                            if (parentid == null || parentid == '')
                                parentid = 0;
                            if (isblade == 'True' && parentid > 0 && startpos == "-Select-") {
                                alert('Select Position');
                                oEvent.cancel = true;
                            }

                            if (orn != "Front" && orn != "Rear") {
                                alert('Orientation must be Front or Rear');
                                oEvent.cancel = true;
                            }
                        }
                    }

                }

                $('<%=lblErrorMsg.ClientID%>').text("");
            }
        }
        //-->
    </script>
    <script language="javascript" type="text/javascript">
        //<!--
        var hostArray = [];

        function openCreateHost(url) {
            winSettings = "scroll:auto; width=1024; height=768;top=50;left=50;status=1; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
            var hWnd = open_window(url, "HostSub", winSettings)

            if ((document.window != null) && (hWnd.opener))
                hWnd.opener = document.window;

        }



        function AddHosts_Click() {

            var lHost = document.getElementById('<%=lstHosts.ClientID%>');
            var txtHost = document.getElementById('<%=txtHostName.ClientID%>');
            var hList = document.getElementById('<%=hdnHostNames.ClientID%>');


            if (txtHost.value != '') {
                PageMethods.getHostNames(txtHost.value, 1, onSuccess_getHosts, onError_getHosts);
            }
            else {
                alert('Enter Host Name');
            }
        }

        function onSuccess_getHosts(arrHost) {
            var txtHost = document.getElementById('<%=txtHostName.ClientID%>');
            var hList = document.getElementById('<%=hdnHostNames.ClientID%>');
            var lHost = document.getElementById('<%=lstHosts.ClientID%>');
            hostArray = arrHost;
            var listItems = [];
            var upperCaseNames = hostArray.map(function (value) {
                return value.toUpperCase();
            });

            //            for (var i = 0; i < lHost.options.length; i++) {
            //                listItems.push(lHost.options[i].text);
            //            }

            if (upperCaseNames.indexOf(txtHost.value.toUpperCase()) > -1) {
                //In the array!
                //if text box contains text
                if (hList.value.indexOf(txtHost.value) == -1) {
                    var newOption = new Option(); // Create a new instance of ListItem
                    newOption.text = txtHost.value;
                    newOption.value = txtHost.value;
                    lHost.options[lHost.length] = newOption;
                    hList.value += txtHost.value + ',';
                    txtHost.value = '';
                }
            }
            else {
                alert('Host does not exists, use Create Host link to create host');
            }
        }

        function onError_getHosts() {
        }


        function DeleteHosts_Click() {

            var lHost = document.getElementById('<%=lstHosts.ClientID%>');
            var hList = document.getElementById('<%=hdnHostNames.ClientID%>');
            //alert(lHost.options[lHost.options.selectedIndex].text);
            if (lHost.options.selectedIndex > -1) {
                var selectedValue = lHost.options[lHost.options.selectedIndex].text + ',';
                //            alert(selectedValue);
                if (hList.value.indexOf(selectedValue) == -1) {

                }
                else {
                    hList.value = hList.value.replace(selectedValue, "");
                }

                lHost.remove(lHost.options.selectedIndex);  //Remove the item
            }
        }

        function uwtDistributionList_ItemChecked(treeId, nodeId, bCheck) {
            //var treeDistList = igtree_getTreeById(treeId);
            var curNode = igtree_getNodeById(nodeId);
            var childNodes = curNode.getChildNodes();

            for (var i = 0; i < childNodes.length; i++) {
                childNodes[i].setChecked(bCheck);
            }

        }


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
                    var winSettings = "scroll:auto; width=400; height=500;top=50;left=50;status=1; resizable:yes; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    var hWnd = open_window(url, "ModelList", winSettings)

                    if ((document.window != null) && (hWnd.opener))
                        hWnd.opener = document.window;
                }
            }
        }

        function openLocationList(url) {

            var ctrlSite = document.getElementById('<%=ddlSite.ClientID%>');
            var ctrlBU = document.getElementById('<%=ddlBusinessUnit.ClientID%>');
            if (ctrlBU.options[ctrlBU.selectedIndex].value == "0") {
                alert('Select BusineesUnit');
            }
            else {
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
            var hdnParentLocID = document.getElementById('<%=hdnLocID.ClientID%>');
            var txtParentLoc = document.getElementById('<%=txtLocation.ClientID%>');
            var hdnLName = document.getElementById('<%=hdnLocName.ClientID%>');
            var hdnMID = document.getElementById('<%=hdnModelID.ClientID%>');
            var txtAssetModel = document.getElementById('<%=txtModel.ClientID%>');
            var hdnMName = document.getElementById('<%=hdnModelName.ClientID%>');
            var hdnIsBlade = document.getElementById('<%=hdnIsBlade.ClientID%>');
            var parentSelect = document.getElementById('<%=HyperLink1.ClientID%>');


            if (header == "Loc") {
                txtParentLoc.value = txt;
                hdnParentLocID.value = val;
                hdnLName.value = txt;
                document.getElementById('<%=btnReload.ClientID%>').click();
            }
            else {
                txtAssetModel.value = txt;
                hdnMID.value = val;
                hdnMName.value = txt;
                hdnIsBlade.value = flag;
                if (flag == "True") {
                    document.getElementById('<%=HyperLink1.ClientID%>').disabled = false;
                }
                else {
                    document.getElementById('<%=HyperLink1.ClientID%>').disabled = true;
                }

                document.getElementById('<%=txtParentAsset.ClientID%>').value = "";
                document.getElementById('<%=hdnParentAssetName.ClientID%>').value = "";
                document.getElementById('<%=hdnParentAssetID.ClientID%>').value = "";
                document.getElementById('<%=ddlSite.ClientID%>').disabled = false;
                document.getElementById('<%=imgLocButton.ClientID%>').className = "enabled";
                document.getElementById('<%=imgRemoveParent.ClientID%>').disabled = true;

                document.getElementById('<%=btnReload.ClientID%>').click();


            }
        }

        //-->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
        <tr align="center">
            <td>
                <igtxt:WebImageButton ID="ibCreateAsset2" runat="server" ImageDirectory="" OnClick="ibCreateAsset_Click"
                    SkinID="uwButton" TabIndex="26" UseBrowserDefaults="False" Width="118px" meta:resourcekey="ibCreateAsset2Resource1">
                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                        WidthOfRightEdge="13" />
                    <ClientSideEvents Click="ibCreate_JS_Click" />
                </igtxt:WebImageButton>
            </td>
            <td>
                <igtxt:WebImageButton ID="ibReset2" runat="server" ImageDirectory="" OnClick="ibResetDocument_Click"
                    SkinID="uwButton" TabIndex="27" UseBrowserDefaults="False" Width="118px" CausesValidation="False"
                    meta:resourcekey="ibReset2Resource1">
                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                        WidthOfRightEdge="13" />
                </igtxt:WebImageButton>
            </td>
            <td>
                <igtxt:WebImageButton ID="ibClose2" runat="server" CausesValidation="False" ImageDirectory=""
                    OnClick="ibClose_Click" SkinID="uwButton" TabIndex="28" UseBrowserDefaults="False"
                    Width="118px" meta:resourcekey="ibClose2Resource1">
                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                        WidthOfRightEdge="13" />
                </igtxt:WebImageButton>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 5px"></td>
        </tr>
        <tr>
            <td colspan="3">
                <igmisc:WebGroupBox ID="WebGroupBox1" runat="server" Width="98%" meta:resourcekey="uwtCreateAssetResource1">
                    <Template>
                        <table>
                            <tr>
                                <td style="height: 20px;" colspan="6" align="left">
                                    <asp:Label ID="lblErrorMsg" runat="server" class="ErrValStyleLarge" Visible="False"
                                        Width="884px" Height="16px" meta:resourcekey="lblErrorMsgResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="left">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true"
                                        CssClass="ErrSummaryNew" DisplayMode="BulletList" ShowMessageBox="false" meta:resourcekey="ValidationSummary1Resource1" />
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" align="left" style="width: 10%;">
                                    <asp:Label ID="lblBusinessUnit" runat="server" CssClass="FieldName" Width="99px"
                                        meta:resourcekey="lblBusinessUnitResource1"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;">
                                    <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="dropdownText" Width="160px"
                                        OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged" AutoPostBack="True"
                                        TabIndex="1" meta:resourcekey="ddlBusinessUnitResource1">
                                        <asp:ListItem Selected="True" Value="-1" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="labelTD" align="left" style="width: 15%;">
                                    <asp:CompareValidator ID="cvBusinessUnit" runat="server" ControlToValidate="ddlBusinessUnit"
                                        CssClass="ErrValStyle" meta:resourcekey="cvBusinessUnitResource1" Operator="NotEqual"
                                        Type="Integer" ValueToCompare="-1"></asp:CompareValidator>
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblDept" runat="server" CssClass="FieldName" meta:resourcekey="lblDeptResource1"
                                        Width="94px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;">
                                    <asp:DropDownList ID="ddlSite" runat="server" CssClass="dropdownText" Width="160px"
                                        TabIndex="2" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged" AutoPostBack="True"
                                        meta:resourcekey="ddlSiteResource1">
                                        <asp:ListItem Value="-1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="ControlTD" align="left" style="width: 10%;">
                                    <asp:CompareValidator ID="cvSite" runat="server" ControlToValidate="ddlSite" CssClass="ErrValStyle"
                                        Display="None" meta:resourcekey="cvSiteResource1" Operator="NotEqual" Type="Integer"
                                        ValueToCompare="0"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" align="left" style="width: 10%;">
                                    <asp:Label ID="lblLocation" runat="server" CssClass="FieldName" Width="99px" Height="16px"
                                        meta:resourcekey="lblLocationResource1"></asp:Label>
                                </td>
                                <td class="ControlTD" valign="top" style="width: 25%;">
                                    <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                        <tr align="left">
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:TextBox ID="txtLocation" runat="server" Width="250px" CssClass="FieldValue"
                                                    Enabled="false" MaxLength="150" TabIndex="3" meta:resourcekey="txtLocationResource1"
                                                    OnTextChanged="txtLocation_TextChanged"></asp:TextBox>
                                            </td>
                                            <td class="ControlTD" align="left" valign="top">
                                                <a href="javascript:openLocationList('TreeList.aspx?Type=CA&Site=<%=ddlSiteVal %>&Header=Location');">
                                                    <img id="imgLocButton" runat="server" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 15%;" class="ControlTD" align="left">
                                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation"
                                        Display="None" CssClass="ErrValStyle" meta:resourcekey="rfvLocationResource1"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;">&nbsp;
                                </td>
                                <td class="ControlTD" style="width: 25%;">&nbsp;
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblAssetNameActual" runat="server" CssClass="FieldName" meta:resourcekey="lblAssetNameAResource1"
                                        Width="108px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;">
                                    <asp:TextBox ID="txtAssetNameA" runat="server" CssClass="FieldValue" MaxLength="100"
                                        Style="vertical-align: top" TabIndex="4" Width="250px"></asp:TextBox>
                                </td>
                                <td align="left" class="labelTD" style="width: 15%;">
                                    <asp:RegularExpressionValidator ID="revAssetName" runat="server" ControlToValidate="txtAssetNameA"
                                        CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                        Height="15px" Width="166px" meta:resourcekey="revAssetNameResource1"></asp:RegularExpressionValidator>
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblHostName" runat="server" CssClass="FieldName" meta:resourcekey="lblHostNameResource1"
                                        Width="108px"></asp:Label>
                                </td>
                                <td align="left" class="ControlTD" colspan="2" style="width: 35%;" valign="top">
                                    <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                        <tr align="left">
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:TextBox ID="txtHostName" runat="server" CssClass="FieldValue" MaxLength="100"
                                                    meta:resourcekey="txtHostNameResource1" TabIndex="5" Width="250px"></asp:TextBox>
                                            </td>
                                            <td class="ControlTD" align="left" valign="top">
                                                <input type="button" id="addHost" onclick="JAVASCRIPT: AddHosts_Click();" value="Add Host" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div>
                                                    <a class="lnk" href="javascript:openCreateHost('HostPopup.aspx');">Host not found?,Create
                                                        Host</a>
                                                </div>
                                                <ajaxToolkit:AutoCompleteExtender ID="autoComplete1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="getHostNames" ServicePath=""
                                                    TargetControlID="txtHostName">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:RegularExpressionValidator ID="revHostName" runat="server" ControlToValidate="txtHostName"
                                        CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                        Height="15px" Width="166px" meta:resourcekey="revHostNameResource1"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" align="left" style="width: 10%;">
                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="FieldName" Width="99px" Height="16px"
                                        meta:resourcekey="lblSerialNoResource1"></asp:Label>
                                </td>
                                <td class="ControlTD" valign="top" style="width: 25%;">
                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="FieldValue" Width="255px"
                                        MaxLength="100" TextMode="SingleLine" TabIndex="6" OnTextChanged="txtSerialNo_TextChanged"
                                        Style="vertical-align: top" meta:resourcekey="txtSerialNoResource1"></asp:TextBox>
                                </td>
                                <td class="labelTD" align="left" style="width: 15%;" valign="top">
                                    <asp:RegularExpressionValidator ID="revSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                        CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                        Height="15px" Width="166px" meta:resourcekey="revSerialNoResource1"></asp:RegularExpressionValidator>
                                </td>
                                <td align="left" class="ControlTD" style="width: 10%;" valign="top"></td>
                                <td class="ControlTD" style="width: 10%;" valign="top">
                                    <asp:ListBox ID="lstHosts" runat="server" CssClass="dropdownText" Height="60px" meta:resourcekey="lstHostsResource1"
                                        Width="332px" TabIndex="7"></asp:ListBox>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top">
                                    <input id="deleteHost" onclick="JAVASCRIPT: DeleteHosts_Click();" type="button" value="Delete Host" />
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" align="left" style="width: 10%;">
                                    <asp:Label ID="lblMfg" runat="server" CssClass="FieldName" meta:resourcekey="lblMfgResource1"
                                        Width="99px"></asp:Label>
                                </td>
                                <td class="ControlTD" align="left" style="width: 25%;">
                                    <asp:DropDownList ID="ddlMfg" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                        meta:resourcekey="ddlMfgResource1" OnSelectedIndexChanged="ddlMfg_SelectedIndexChanged"
                                        TabIndex="8" Width="240px">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelTD" align="left" style="width: 15%;">
                                    <asp:RequiredFieldValidator ID="rfvMfg" runat="server" ControlToValidate="ddlMfg"
                                        Display="None" CssClass="ErrValStyle" InitialValue="0" meta:resourcekey="rfvMfgResource1"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblModel" runat="server" CssClass="FieldName" meta:resourcekey="lblModelResource1"
                                        Width="99px"></asp:Label>
                                </td>
                                <td class="ControlTD" align="left" valign="top" style="width: 25%;">
                                    <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:TextBox ID="txtModel" runat="server" CssClass="FieldValue" Enabled="false" MaxLength="150"
                                                    meta:resourcekey="txtModelResource1" TabIndex="9" Text="No Model" Width="250px"></asp:TextBox>
                                            </td>
                                            <td class="ControlTD" align="left" valign="top">
                                                <a href="javascript:openModelList('TreeList.aspx?Type=Models&MfgName=<%=ddlMfgName %>&MfgID=<%=ddlMfgID %>&BU=<%=ddlBusinessUnitVal %>&Header=AssetModel');">
                                                    <img id="img1" alt="Load List" src="images/search.gif" style="border: 0;" /></a>
                                                <asp:Button ID="btnReload" runat="server" CausesValidation="false" OnClick="btnReload_Click"
                                                    Style="display: none;" />
                                                <asp:Button ID="btnParentLoad" runat="server" CausesValidation="false" OnClick="btnParentLoad_Click"
                                                    Style="display: none;" />
                                                <asp:Button ID="btnRemoveParentReload" runat="server" CausesValidation="false" OnClick="btnRemoveParentReload_Click"
                                                    Style="display: none;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="ControlTD" align="left" style="width: 10%;">
                                    <asp:RequiredFieldValidator ID="reqBUVal1" runat="server" ControlToValidate="txtModel"
                                        Display="None" CssClass="ErrValStyle" ErrorMessage="Select Model" meta:resourcekey="reqBUVal1Resource1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblOrientation" runat="server" CssClass="FieldName" meta:resourcekey="lblOrientationResource"
                                        Width="108px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top">
                                    <asp:DropDownList ID="ddlOrientation" runat="server" CssClass="FieldValue" meta:resourcekey="ddlOrientationResource"
                                        TabIndex="10" Width="160px" OnSelectedIndexChanged="ddlOrientation_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td align="right" class="labelTD" style="width: 15%;" valign="top">&nbsp;
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;" valign="top">
                                    <asp:Label ID="lblPosition" runat="server" CssClass="FieldName" meta:resourcekey="lblPositionResource"
                                        Width="108px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;">
                                    <asp:DropDownList ID="ddlPosition" runat="server" CssClass="dropdownText" TabIndex="11"
                                        Width="70px">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelTD" style="width: 10%;" valign="top">&nbsp;
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblMaxPower" runat="server" CssClass="FieldName" meta:resourcekey="lblMaxPowerResource1"
                                        Width="150px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top">
                                    <asp:TextBox ID="txtMaxPower" runat="server" Width="150px" CssClass="FieldValue"
                                        Enabled="false" MaxLength="12" meta:resourcekey="txtMaxPowerResource1"></asp:TextBox>
                                </td>
                                <td align="right" class="labelTD" style="width: 15%;" valign="top">&nbsp;
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;" valign="top">
                                    <asp:Label ID="lblDeratedPower" runat="server" CssClass="FieldName" meta:resourcekey="lblDeratedPowerResource1"
                                        Width="150px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top" align="left">
                                    <asp:TextBox ID="txtDerated" runat="server" Width="150px" CssClass="FieldValue" MaxLength="10"
                                        meta:resourcekey="txtDeratedResource1" TabIndex="13"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revDerated" runat="server" ControlToValidate="txtDerated"
                                        Display="None" ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" CssClass="ErrValStyle"
                                        meta:resourcekey="revDeratedrResource1"> </asp:RegularExpressionValidator>
                                    <asp:RangeValidator ID="rvDerated" runat="server" ControlToValidate="txtDerated"
                                        Display="None" Type="Double" MinimumValue="0" MaximumValue="10000" CssClass="ErrValStyle"
                                        meta:resourcekey="rvDeratedResource1"> </asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="rfvDerated" runat="server" CssClass="ErrValStyle"
                                        meta:resourcekey="revDeratedrResource1" Display="None" ControlToValidate="txtDerated"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvSPower" runat="server" ControlToValidate="txtDerated"
                                        ControlToCompare="txtMaxPower" Type="Double" Operator="LessThanEqual" meta:resourcekey="cvSPowerResource1"
                                        CssClass="ErrValStyle" Display="None"></asp:CompareValidator>
                                </td>
                                <td class="labelTD" style="width: 10%;" align="left" valign="top">&nbsp;
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblCustodian" runat="server" CssClass="FieldName" meta:resourcekey="lblCustodianResource1"
                                        Width="90px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top">
                                    <table align="left" cellpadding="0" cellspacing="0" class="leftaligned">
                                        <tr align="left">
                                            <td align="left" class="ControlTD" valign="top">
                                                <asp:TextBox ID="txtOwner" runat="server" AutoPostBack="false" CssClass="FieldValue"
                                                    Enabled="false" MaxLength="255" meta:resourcekey="txtOwnerResource1" TabIndex="14"
                                                    Width="250px"></asp:TextBox>
                                            </td>
                                            <td align="left" class="ControlTD" valign="top">
                                                <asp:HyperLink ID="hlSearch" runat="server" ImageUrl="images/search.gif" meta:resourcekey="hlSearchResource1"
                                                    NavigateUrl="javascript:searchOriginatorPopup();" Style="border: 0;" TabIndex="15"
                                                    Text="Search"></asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right" class="labelTD" style="width: 15%;" valign="top">&nbsp;
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;" valign="top">
                                    <asp:Label ID="lblCreateDate" runat="server" CssClass="FieldName" meta:resourcekey="lblCreateDateResource1"
                                        Width="111px"></asp:Label>
                                </td>
                                <td align="left" class="ControlTD" style="width: 25%;" valign="top">
                                    <ig:WebDatePicker ID="wdcCreatedDate" runat="server" CssClass="dropdownText" MinValue="2000-1-1"
                                        NullDateLabel="-- Select --" NullValueRepresentation="Null" TabIndex="15" Width="140px">
                                    </ig:WebDatePicker>
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;" valign="top">
                                    <asp:RequiredFieldValidator ID="rfvDocumentDate" runat="server" ControlToValidate="wdcCreatedDate"
                                        CssClass="ErrValStyle" Display="None" meta:resourcekey="rfvDocumentDateResource1"
                                        Width="239px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" align="left" valign="top" style="width: 10%;">
                                    <asp:Label ID="lblOS" runat="server" CssClass="FieldName" meta:resourcekey="lblOSResource1"
                                        Width="121px"></asp:Label>
                                </td>
                                <td class="ControlTD" align="left" valign="top" style="width: 25%;">
                                    <asp:TextBox ID="txtOS" runat="server" CssClass="FieldValue" meta:resourcekey="txtOSResource1"
                                        TabIndex="16" Width="160px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td class="labelTD" style="width: 15%;" valign="top" align="left">
                                    <asp:RegularExpressionValidator ID="revOS" runat="server" ControlToValidate="txtOS"
                                        CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                        Height="15px" Width="166px" meta:resourcekey="revOSResource1"></asp:RegularExpressionValidator>
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;" valign="top">
                                    <asp:Label ID="lblCPU" runat="server" CssClass="FieldName" meta:resourcekey="lblAssetGroup1Resource1"
                                        Width="80px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top">
                                    <asp:TextBox ID="txtCPU" runat="server" CssClass="FieldValue" meta:resourcekey="txtCPUResource1"
                                        TabIndex="17" Width="160px" MaxLength="50"></asp:TextBox>
                                </td>
                                <td class="labelTD" align="left" style="width: 10%;" valign="top">
                                    <asp:RegularExpressionValidator ID="revCPU" runat="server" ControlToValidate="txtCPU"
                                        CssClass="ErrValStyle" Display="none" ValidationExpression="^[\w\-\.]+(\s{1}[\w\-\.]+)*\s{0,1}$"
                                        Height="15px" Width="166px" meta:resourcekey="revCPUResource1"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" style="width: 10%;" valign="top" align="left">
                                    <asp:Label ID="lblCpuCount" runat="server" CssClass="FieldName" Height="16px" meta:resourcekey="lblCpuCountResource1"
                                        Width="93px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top">
                                    <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                        <tr align="left">
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:TextBox ID="txtCpuCount" runat="server" CssClass="FieldValue" MaxLength="2"
                                                    meta:resourcekey="txtCpuCountResource1" TabIndex="18" Width="50px"></asp:TextBox>
                                            </td>
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:RegularExpressionValidator ID="regCpuCount" runat="server" ControlToValidate="txtCpuCount"
                                                    CssClass="ErrValStyle" Display="Dynamic" Height="15px" meta:resourcekey="regCpuCountResource1"
                                                    ValidationExpression="^[0-9]{0,2}" Width="166px"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="labelTD" style="width: 15%;" valign="top" align="left">&nbsp;
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;" valign="top">
                                    <asp:Label ID="lblCPUCore" runat="server" CssClass="FieldName" Height="16px" meta:resourcekey="lblCPUCoreResource1"
                                        Width="93px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;" valign="top">
                                    <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                        <tr align="left">
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:TextBox ID="txtCPUCore" runat="server" CssClass="FieldValue" MaxLength="2" meta:resourcekey="txtCPUCoreResource1"
                                                    TabIndex="19" Width="50px"></asp:TextBox>
                                            </td>
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:RegularExpressionValidator ID="regCpuCore" runat="server" ControlToValidate="txtCPUCore"
                                                    CssClass="ErrValStyle" Display="Dynamic" Height="15px" meta:resourcekey="regCpuCoreResource1"
                                                    ValidationExpression="^[0-9]{0,2}" Width="166px"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="labelTD" style="height: 29px; width: 600px;" valign="top" align="left"></td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" style="width: 10%;" valign="top" align="left">
                                    <asp:Label ID="lblParentAsset" runat="server" CssClass="FieldName" meta:resourcekey="lblParentAssetResource1"
                                        Width="90px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;">
                                    <table class="leftaligned" align="left" cellpadding="0" cellspacing="0">
                                        <tr align="left">
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:TextBox ID="txtParentAsset" runat="server" CssClass="FieldValue" Enabled="False"
                                                    MaxLength="150" TabIndex="20" Width="250px"></asp:TextBox>
                                            </td>
                                            <td class="ControlTD" align="left" valign="top">
                                                <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="images/search.gif" NavigateUrl="javascript:searchParentAssetPopup();"
                                                    TabIndex="15" Text="ParentAssetSearch" Style="border: 0;"></asp:HyperLink>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgRemoveParent" ImageUrl="images/ed_delete.gif" runat="server"
                                                    ToolTip="Remove Parent Asset" OnClientClick="imgRemoveParent_Click();return false;"
                                                    CausesValidation="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="labelTD" style="width: 15%;">&nbsp;
                                </td>
                                <td align="left" class="labelTD" style="width: 10%;">
                                    <asp:Label ID="lblBarcode" runat="server" CssClass="FieldName" meta:resourcekey="lblBarcodeResource1"
                                        Width="111px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;">
                                    <asp:TextBox ID="txtBarcode" runat="server" CssClass="FieldValue" MaxLength="24"
                                        Style="vertical-align: top" TabIndex="21" Width="250px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revBarcode" runat="server" ControlToValidate="txtBarcode"
                                        Display="None" ValidationExpression="^[A-Za-z0-9]+$" CssClass="ErrValStyle" meta:resourcekey="revBarcodeResource1"> </asp:RegularExpressionValidator>
                                </td>
                                <td class="labelTD" style="width: 10%;"></td>
                            </tr>
                            <tr style="height: 29px;">
                                <td class="labelTD" style="width: 10%;" align="left">
                                    <asp:Label ID="lblTechCat" runat="server" CssClass="FieldName" meta:resourcekey="lblTechCatResource1"
                                        Width="108px"></asp:Label>
                                </td>
                                <td class="ControlTD" style="width: 25%;">
                                    <asp:DropDownList ID="ddlTechCat" runat="server" CssClass="FieldValue" meta:resourcekey="ddlTechCatResource1"
                                        OnSelectedIndexChanged="ddlTechCat_SelectedIndexChanged" TabIndex="22" Width="160px">
                                    </asp:DropDownList>
                                    <br />
                                </td>
                                <td class="labelTD" style="width: 15%;"></td>
                                <td align="left" class="labelTD" style="height: 33px">&nbsp;
                                </td>
                                <td class="ControlTD" style="width: 10%;">
                                    <br />
                                </td>
                                <td class="labelTD" style="width: 10%;"></td>
                            </tr>
                        </table>
                    </Template>
                </igmisc:WebGroupBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" style="height: 5px"></td>
        </tr>
        <tr align="center">
            <td>
                <igtxt:WebImageButton ID="ibCreateAsset" runat="server" ImageDirectory="" OnClick="ibCreateAsset_Click"
                    SkinID="uwButton" TabIndex="17" UseBrowserDefaults="False" Width="118px" meta:resourcekey="ibCreateAssetResource1">
                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                        WidthOfRightEdge="13" />
                    <ClientSideEvents Click="ibCreate_JS_Click" />
                </igtxt:WebImageButton>
            </td>
            <td>
                <igtxt:WebImageButton ID="ibResetDocument" runat="server" ImageDirectory="" OnClick="ibResetDocument_Click"
                    SkinID="uwButton" TabIndex="18" ToolTip="Cancel Unsaved Changes" UseBrowserDefaults="False"
                    Width="118px" CausesValidation="False" meta:resourcekey="ibResetDocumentResource1">
                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                        WidthOfRightEdge="13" />
                </igtxt:WebImageButton>
            </td>
            <td>
                <igtxt:WebImageButton ID="ibClose" runat="server" CausesValidation="False" ImageDirectory=""
                    OnClick="ibClose_Click" SkinID="uwButton" TabIndex="19" UseBrowserDefaults="False"
                    Width="118px" meta:resourcekey="ibCloseResource1">
                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                        WidthOfRightEdge="13" />
                </igtxt:WebImageButton>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="uwlGroupUsers_hdnSelectedItemKey" runat="server" Value="Test" />
    <asp:HiddenField ID="uwlSelectedUsers_hdnSelectedItemKey" runat="server" Value="Test" />
    <input id="hdnOwnerID" runat="server" style="width: 18px" type="hidden" />
    <input id="hdnOwnerName" runat="server" style="width: 25px" type="hidden" />
    <input id="hdnLocationID" runat="server" style="width: 18px" type="hidden" />
    <input id="hdnLocationName" runat="server" style="width: 25px" type="hidden" />
    <input id="hdnHostNames" runat="server" style="width: 100px" type="hidden" />
    <input type="hidden" id="hdnLocID" runat="server" />
    <input type="hidden" id="hdnModelID" runat="server" />
    <input type="hidden" id="hdnModelName" runat="server" />
    <input type="hidden" id="hdnIsBlade" runat="server" />
    <input type="hidden" id="hdnLocName" runat="server" />
    <input type="hidden" id="hdnTechCat" runat="server" />
    <input type="hidden" id="hdnAssetID" runat="server" />
    <input type="hidden" id="hdnParentAssetID" runat="server" />
    <input type="hidden" id="hdnParentAssetName" runat="server" />
    <input type="hidden" id="hdnSiteID" runat="server" />
    <input type="hidden" id="hdnSiteName" runat="server" />
</asp:Content>
