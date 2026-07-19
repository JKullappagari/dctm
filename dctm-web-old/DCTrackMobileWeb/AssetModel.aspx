<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="AssetModel.aspx.cs" Inherits="AssetModel" Title="Asset Model"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content_AssetModel" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <ig:WebExcelExporter runat="server" ID="eExporter" OnCellExported="eExporter_CellExported" />
    <script id="Infragistics" type="text/javascript">
        //<!--
        function checkDescLength(sender, args) {
            var tDesc = document.getElementById('<%=txtDesc.ClientID%>').value;
            if (tDesc == '') {
                // do nothing
            }
            else if (tDesc.length > parseInt('<%=txtDesc.MaxLength.ToString() %>')) {
                args.IsValid = false;
                return;
            }
            else {
                args.IsValid = true;
                return;
            }

        }

        function ibDelete_Click(oButton, oEvent) {
            //Add code to handle your event here.
            return ValidateDeletionNew(oButton, oEvent, document.forms[0].elements['<%=hdnMessage.ClientID%>'].value);
        }
        function ConfirmOnDelete() {

            if (confirm("Are you sure to delete?") == true)

                return true;

            else

                return false;

        }

        function ibMap_Click(oButton, oEvent) {
            //Add code to handle your event here.
            var techlist = document.forms[0].elements['<%=ddlTech.ClientID%>'];
            if (techlist.selectedIndex > 0) {
                return ValidateMapNew(oButton, oEvent, 'Do you want to map selected Model(s) with selected Tech category?');
            }
            else {
                alert(' Select Technology Category')
                oEvent.cancel = true;
            }
        }


        function ibCreate_JS_Click(oButton, oEvent) {
            //Add code to handle your event here.

            //	    alert(objMaxOtHr.value);
            //	    alert(objMaxWHr.value);
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
            //document.getElementById("ctl00_Master_ContentPlaceHolder_lblMessage").innerHTML = "";
        }

        function checkRadio(element, type) {
            var ctrlMountType = document.getElementById('<%=ddlMountType.ClientID%>');


            if (ctrlMountType.selectedIndex == -1 || parseInt(ctrlMountType.options[ctrlMountType.selectedIndex].value) < 1) {
                alert('Select Mount Type');
                element.checked = false;
                return false;
            }
            else {

                var mountType = ctrlMountType.options[ctrlMountType.selectedIndex].text;
                if (mountType === 'RackMount') {
                    if (type === 'Enclosure') {
                        element.checked = true;
                        return true;
                    }
                    else {
                        element.checked = false;
                        return false;
                    }

                }
                else {
                    if (type === 'Blade') {
                        element.checked = true;
                        return true;
                    }
                    else {
                        element.checked = false;
                        return false;
                    }
                }

            }
        }
        //-->
    </script>
    <script type="text/javascript">
        //<!--
        $(document).ready(function () {
            $('#<%=txtMaxPower.ClientID %>').blur(function () {
                $('#<%=txtSSWatts.ClientID %>').val((parseFloat($('#<%=txtMaxPower.ClientID %>').val()) * parseFloat('<%=this.PowerDeratedFactor.ToString() %>')).toFixed(3));
            });

            $('#<%=txtHeight.ClientID %>').blur(function () {

                var modeltype = $('#<%=ddlModelType.ClientID %> option:selected').text();
                if (modeltype.indexOf('Blade') < 0 && modeltype.indexOf('Module') < 0)
                    $('#<%=txtUHeight.ClientID %>').val(Math.ceil($('#<%=txtHeight.ClientID %>').val() / 44.45));



                if (modeltype === 'Rack') {
                    $('#<%=txtRInternalHeight.ClientID %>').val($('#<%=txtHeight.ClientID %>').val());
                }
            });

            $('#<%=txtDepth.ClientID %>').blur(function () {
                var modeltype = $('#<%=ddlModelType.ClientID %> option:selected').text();

                if (modeltype === 'Rack') {
                    $('#<%=txtRInternalDepth.ClientID %>').val($('#<%=txtDepth.ClientID %>').val());
                }

            });

            setRounding();
        });

        function validateMountType() {
            var seltext = $('#<%=ddlMountType.ClientID %> option:selected').text();
            var modeltype = $('#<%=ddlModelType.ClientID %> option:selected').text();

            if (seltext === 'Enclosure Mount') {

                if (modeltype === '-Select-') {
                    alert('Select Model Type');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if (modeltype.indexOf('Blade') < 0 && modeltype.indexOf('Module') < 0) {
                    alert('Mount Type: Enclosure Mount is not allowed when model type is not a blade/module');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (seltext === 'Floor Standalone') {

                if (modeltype === '-Select-') {
                    alert('Select Model Type');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if (modeltype.indexOf('Standalone') < 0 || $modeltype.indexOf('Rack') < 0) {
                    alert('Mount Type: Floor Standalone is not allowed when model type is not a Standalone');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if ($modeltype.indexOf('Rack') > -1) {

                }
                else {
                    return true;
                }
            }
            else if (seltext === 'Vertical Mount') {

                if (modeltype === '-Select-') {
                    alert('Select Model Type');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if (modeltype.indexOf('Zero U') < 0) {
                    alert('Mount Type: Vertical Mount is allowed when model type is Rack PDU-Zero U');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else {
                    return true;
                }
            }
            else if (seltext === 'RackMount' || seltext === 'Stack In Rack') {

                if (modeltype === '-Select-') {
                    alert('Select Model Type');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if (modeltype.indexOf('Zero U') > 0) {
                    alert('Model Type: Rack PDU-Zero U is not allowed.');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if (modeltype.indexOf('Blade') > 0 || modeltype.indexOf('Module') > 0) {
                    alert('Model type: blade/module is not allowed');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if (modeltype.indexOf('Standalone') > 0) {
                    alert('Model type Standalone is not allowed');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else if (modeltype.indexOf('PDU') < 0 && modeltype.indexOf('Rack') >= 0 ) {
                    alert('Model type Standalone is not allowed');
                    $('#<%=ddlMountType.ClientID %>').prop('selectedIndex', 0);
                    return false;
                }
                else {
                    return true;
                }
            }

        }

        function setRounding() {

            if ($('#<%=txtWidth.ClientID %>').val() != '' && $('#<%=txtWidth.ClientID %>').val() > 0)
                $('#<%=txtWidth.ClientID %>').val(parseFloat($('#<%=txtWidth.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtDepth.ClientID %>').val() != '' && $('#<%=txtDepth.ClientID %>').val() > 0)
                $('#<%=txtDepth.ClientID %>').val(parseFloat($('#<%=txtDepth.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtHeight.ClientID %>').val() != '' && $('#<%=txtHeight.ClientID %>').val() > 0)
                $('#<%=txtHeight.ClientID %>').val(parseFloat($('#<%=txtHeight.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtWeight.ClientID %>').val() != '' && $('#<%=txtWeight.ClientID %>').val() > 0)
                $('#<%=txtWeight.ClientID %>').val(parseFloat($('#<%=txtWeight.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtMaxPower.ClientID %>').val() != '' && $('#<%=txtMaxPower.ClientID %>').val() > 0)
                $('#<%=txtMaxPower.ClientID %>').val(parseFloat($('#<%=txtMaxPower.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtSSWatts.ClientID %>').val() != '' && $('#<%=txtSSWatts.ClientID %>').val() > 0)
                $('#<%=txtSSWatts.ClientID %>').val(parseFloat($('#<%=txtSSWatts.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtRInternalDepth.ClientID %>').val() != '' && $('#<%=txtRInternalDepth.ClientID %>').val() > 0)
                $('#<%=txtRInternalDepth.ClientID %>').val(parseFloat($('#<%=txtRInternalDepth.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtRInternalHeight.ClientID %>').val() != '' && $('#<%=txtRInternalHeight.ClientID %>').val() > 0)
                $('#<%=txtRInternalHeight.ClientID %>').val(parseFloat($('#<%=txtRInternalHeight.ClientID %>').val()).toFixed(3));

            if ($('#<%=txtRInternalWidth.ClientID %>').val() != '' && $('#<%=txtRInternalWidth.ClientID %>').val() > 0)
                $('#<%=txtRInternalWidth.ClientID %>').val(parseFloat($('#<%=txtRInternalWidth.ClientID %>').val()).toFixed(3));
        }

        //-->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: center;">
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true"
                    CssClass="ErrSummaryNew" DisplayMode="BulletList" ShowMessageBox="false" meta:resourcekey="ValidationSummary1Resource1" />
            </td>
        </tr>
        <tr>
            <td valign="top" style="width: 100%">
                &nbsp;
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif); height: 199px;">
                        </td>
                        <td valign="top" style="height: 199px">
                            <table id="Table3" style="height: 100%;" cellspacing="0" cellpadding="0" width="100%"
                                border="0">
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 30px;">
                                        <asp:Label ID="lblComment0" runat="server" CssClass="FieldName" Width="106px" meta:resourcekey="lblComment0Resource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 30px; width: 600px;" valign="middle">
                                        <asp:DropDownList ID="ddlBusinessUnit" runat="server" CssClass="dropdownText" 
                                            Width="196px" InitialValue="-Select-" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged"
                                            meta:resourcekey="ddlBusinessUnitResource1">
                                        </asp:DropDownList>
                                        &nbsp;
                                        <asp:RequiredFieldValidator ID="reqBUVal" runat="server" CssClass="ErrValStyle" ControlToValidate="ddlBusinessUnit"
                                            InitialValue="0" meta:resourcekey="reqBUValResource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 28px;">
                                        <asp:Label ID="lblModel" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblModelResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 28px; width: 600px;" valign="middle">
                                        <asp:TextBox ID="txtModel" runat="server" Width="354px" CssClass="FieldValue" MaxLength="150"
                                            TabIndex="1" meta:resourcekey="txtModelResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvModel" runat="server" CssClass="ErrValStyle" ControlToValidate="txtModel"
                                            Display="None" TabIndex="7" meta:resourcekey="rfvModelResource1"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revModel" runat="server" ControlToValidate="txtModel"
                                            CssClass="ErrValStyle" ValidationExpression="^^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$"
                                            Display="None" Height="15px" Width="166px" meta:resourcekey="revModelResource1"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 58px;">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblDescResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="height: 58px; width: 600px;" valign="top">
                                        <asp:TextBox ID="txtDesc" runat="server" Width="226px" CssClass="FieldValue" MaxLength="100"
                                            TabIndex="2" TextMode="MultiLine" Height="44px" meta:resourcekey="txtDescResource1"></asp:TextBox>
                                        <br />
                                        <asp:RegularExpressionValidator ID="revDesc" runat="server" ControlToValidate="txtDesc"
                                            CssClass="ErrValStyle" Display="None" ValidationExpression="^[\w0-9\-\.\:\s]+$"
                                            meta:resourcekey="revDesc1Resource1"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="cvDesc" runat="server" CssClass="ErrValStyle" ClientValidationFunction="checkDescLength"
                                            Display="None" ControlToValidate="txtDesc" meta:resourcekey="revDesc1Resource1"></asp:CustomValidator>
                                    </td>
                                    <td class="labelTD" style="width: 58px; height: 36px;">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 29px" valign="top">
                                        <asp:Label ID="lblloctype" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblloctypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="width: 600px;" valign="top">
                                        <asp:DropDownList ID="ddlMfg" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                            Height="20px" OnSelectedIndexChanged="ddlMfg_SelectedIndexChanged" Width="197px"
                                            TabIndex="3" meta:resourcekey="ddlMfgResource1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqBUVal0" runat="server" CssClass="ErrValStyle"
                                            Display="None" ControlToValidate="ddlMfg" InitialValue="0" meta:resourcekey="reqBUVal0Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 29px" valign="top">
                                        <asp:Label ID="lblModelType" runat="server" CssClass="FieldName" Width="109px" meta:resourcekey="lblModelTypeResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" style="width: 600px;" valign="top">
                                        <table>
                                            <tr>
                                                <td style ="width:50%">
                                                    <asp:DropDownList ID="ddlModelType" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                        OnSelectedIndexChanged="ddlModelType_SelectedIndexChanged" Width="197px" 
                                                        meta:resourcekey="ddlMfgResource1" TabIndex="4">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvModelType" runat="server" CssClass="ErrValStyle"
                                                        Display="none" ControlToValidate="ddlModelType" InitialValue="0" meta:resourcekey="rfvModelTypeResource1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>

                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 20px" valign="middle">
                                    </td>
                                    <td colspan="2" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 38px" valign="middle">
                                        <asp:Label ID="lblMounting" runat="server" CssClass="FieldName" Width="110px" meta:resourcekey="lblMountingResource1"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <table id="tblMounting" border="1" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblMountType" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblMountTypeResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:DropDownList ID="ddlMountType" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                        onchange="if(!validateMountType()) return false;" OnSelectedIndexChanged="ddlMountType_SelectedIndexChanged"
                                                        Width="197px" meta:resourcekey="ddlMountTypeResource1" TabIndex="5">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvMountType" runat="server" ControlToValidate="ddlMountType"
                                                        InitialValue="0" meta:resourcekey="rfvIMountResource1" CssClass="ErrValStyle"
                                                        Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblAFDirection" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblAFDirectionResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:DropDownList ID="ddlAFDirection" runat="server" AutoPostBack="false" CssClass="dropdownText"
                                                        OnSelectedIndexChanged="ddlAFDirection_SelectedIndexChanged" Width="197px" 
                                                        meta:resourcekey="ddlAFDirectionResource1" TabIndex="6">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvAFDirection" runat="server" ControlToValidate="ddlAFDirection"
                                                        InitialValue="0" meta:resourcekey="rfvAFResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 20px" valign="middle">
                                    </td>
                                    <td colspan="2" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 38px" valign="middle">
                                        <asp:Label ID="lblPhysical" runat="server" CssClass="FieldName" Width="110px" meta:resourcekey="lblPhysicalResource1"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <table id="tblPhysical" border="1" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblWidth" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblWidthResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtWidth" runat="server" Width="150px" CssClass="FieldValue" MaxLength="10"
                                                        meta:resourcekey="txtWidthResource1" TabIndex="7"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revWidth" runat="server" ControlToValidate="txtWidth"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revWidthResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvWidth" runat="server" ControlToValidate="txtWidth" MinimumValue="1"
                                                        Type="Double" MaximumValue="1500" meta:resourcekey="rv1500WResource1" CssClass="ErrValStyle"
                                                        Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvWidth" runat="server" ControlToValidate="txtWidth"
                                                        meta:resourcekey="revWidthResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblDepth" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblDepthResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtDepth" runat="server" Width="150px" CssClass="FieldValue" MaxLength="10"
                                                        meta:resourcekey="txtDepthResource1" TabIndex="8"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revDepth" runat="server" ControlToValidate="txtDepth"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revDepthResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvDepth" runat="server" ControlToValidate="txtDepth" MinimumValue="1"
                                                        Type="Double" MaximumValue="1500" meta:resourcekey="rv1500DResource1" CssClass="ErrValStyle"
                                                        Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvDepth" runat="server" ControlToValidate="txtDepth"
                                                        meta:resourcekey="revDepthResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblHeight" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblHeightResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtHeight" runat="server" Width="150px" CssClass="FieldValue" MaxLength="10"
                                                        meta:resourcekey="txtHeightResource1" TabIndex="9"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revHeight" runat="server" ControlToValidate="txtHeight"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revHeightResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvHeight" runat="server" ControlToValidate="txtHeight" MinimumValue="1"
                                                        Type="Double" MaximumValue="3000" meta:resourcekey="rv3000HResource1" CssClass="ErrValStyle"
                                                        Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvHeight" runat="server" ControlToValidate="txtHeight"
                                                        meta:resourcekey="revHeightResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblUHeight" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblUHeightResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtUHeight" runat="server" Width="150px" CssClass="FieldValue" MaxLength="10"
                                                        meta:resourcekey="txtUHeightResource1" TabIndex="10"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revUHeight" runat="server" ControlToValidate="txtUHeight"
                                                        ValidationExpression="([0-9])[0-9]*" meta:resourcekey="revUHeightResource1" CssClass="ErrValStyle"
                                                        Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvUHeight" runat="server" ControlToValidate="txtUHeight"
                                                        Type="Integer" MinimumValue="1" MaximumValue="68" meta:resourcekey="rvUHeightResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvUHeight" runat="server" ControlToValidate="txtUHeight"
                                                        meta:resourcekey="revUHeightResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblWeight" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblWeightResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtWeight" runat="server" Width="150px" CssClass="FieldValue" MaxLength="10"
                                                        meta:resourcekey="txtWeightResource1" TabIndex="11"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revWeight" runat="server" ControlToValidate="txtWeight"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revWeightResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvWeight" runat="server" ControlToValidate="txtWeight" MinimumValue="0"
                                                        Type="Double" MaximumValue="2000" meta:resourcekey="rvWeightResource1" CssClass="ErrValStyle"
                                                        Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvWeight" runat="server" ControlToValidate="txtWeight"
                                                        meta:resourcekey="revWeightResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    &nbsp;
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 20px" valign="middle">
                                    </td>
                                    <td colspan="2" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 38px" valign="middle">
                                        <asp:Label ID="lblPower" runat="server" CssClass="FieldName" Width="110px" meta:resourcekey="lblPowerResource1"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <table id="Table2" border="1" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblMaxPower" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblMaxPowerResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtMaxPower" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtMaxPowerResource1" TabIndex="12"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revMaxPower" runat="server" ControlToValidate="txtMaxPower"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revMaxPowerResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvMaxPower" runat="server" ControlToValidate="txtMaxPower"
                                                        Type="Double" MinimumValue="0" MaximumValue="10000" meta:resourcekey="rvMPowerResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvMaxPower" runat="server" ControlToValidate="txtMaxPower"
                                                        meta:resourcekey="revMaxPowerResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblIdealPower" runat="server" CssClass="FieldName" Width="200px" meta:resourcekey="lblIdealPowerResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtSSWatts" runat="server" Width="150px" CssClass="FieldValue" MaxLength="10"
                                                        meta:resourcekey="txtSSWattsResource1" TabIndex="13"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revSSWatts" runat="server" ControlToValidate="txtSSWatts"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revSPowerResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvSSWatts" runat="server" ControlToValidate="txtSSWatts"
                                                        Type="Double" MinimumValue="0" MaximumValue="10000" meta:resourcekey="rvSPowerResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvSSWatts" runat="server" ControlToValidate="txtSSWatts"
                                                        meta:resourcekey="revSPowerResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvSPower" runat="server" ControlToValidate="txtSSWatts"
                                                        ControlToCompare="txtMaxPower" Type="Double" Operator="LessThanEqual" meta:resourcekey="cvSPowerResource1"
                                                        CssClass="ErrValStyle" Display="None"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblConnPDUSide" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblConnPDUSideResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:DropDownList ID="ddlCPDUSide" runat="server" CssClass="dropdownText" 
                                                        Width="200px" TabIndex="14">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvCPDUSide" runat="server" ControlToValidate="ddlCPDUSide"
                                                        InitialValue="0" meta:resourcekey="revConnPDUResource1" CssClass="ErrValStyle"
                                                        Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblConnDevSide" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblConnDevSideResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:DropDownList ID="ddlCDevSide" runat="server" CssClass="dropdownText" 
                                                        Width="200px" TabIndex="15">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvCDevSide" runat="server" ControlToValidate="ddlCDevSide"
                                                        InitialValue="0" meta:resourcekey="revConnDevResource1" CssClass="ErrValStyle"
                                                        Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblTotalPSUCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblTotalPSUCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtTotalPSUCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtTotalPSUCountResource1" TabIndex="16"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revTotalPSUCount" runat="server" ControlToValidate="txtTotalPSUCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revTPSUResource1" CssClass="ErrValStyle"
                                                        Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvTotalPSUCount" runat="server" ControlToValidate="txtTotalPSUCount"
                                                        Type="Integer" MinimumValue="0" MaximumValue="10" meta:resourcekey="rvTPSUResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvTotalPSUCount" runat="server" ControlToValidate="txtTotalPSUCount"
                                                        meta:resourcekey="revTPSUResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblReqPSUCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblReqPSUCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtReqPSUCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtReqPSUCountResource1" TabIndex="17"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revReqPSUCount" runat="server" ControlToValidate="txtReqPSUCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revRPSUResource1" CssClass="ErrValStyle"
                                                        Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvReqPSUCount" runat="server" ControlToValidate="txtReqPSUCount"
                                                        Type="Integer" MinimumValue="0" MaximumValue="10" meta:resourcekey="rvRPSUResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvReqPSUCount" runat="server" ControlToValidate="txtReqPSUCount"
                                                        meta:resourcekey="revRPSUResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvReqPSUCount" runat="server" ControlToValidate="txtReqPSUCount"
                                                        ControlToCompare="txtTotalPSUCount" Type="Integer" Operator="LessThanEqual" meta:resourcekey="cvRPSUResource1"
                                                        CssClass="ErrValStyle" Display="None"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 20px" valign="middle">
                                    </td>
                                    <td colspan="2" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 38px" valign="middle">
                                        <asp:Label ID="lblRackSpecific" runat="server" CssClass="FieldName" Width="110px"
                                            meta:resourcekey="lblRackSpecificResource1"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <table id="tblRackSpecific" border="1" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblInternalWidth" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblInternalWidthResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtRInternalWidth" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtRInternalWidthResource1" TabIndex="18"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revRInternalWidth" runat="server" ControlToValidate="txtRInternalWidth"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revIWResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvRInternalWidth" runat="server" ControlToValidate="txtRInternalWidth"
                                                        Type="Double" MinimumValue="1" MaximumValue="1500" meta:resourcekey="rv1500IWResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvRInternalWidth" runat="server" ControlToValidate="txtRInternalWidth"
                                                        meta:resourcekey="revIWResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvIWidth" runat="server" ControlToValidate="txtRInternalWidth"
                                                        ControlToCompare="txtWidth" Type="Double" Operator="LessThanEqual" meta:resourcekey="cvIWResource1"
                                                        CssClass="ErrValStyle" Display="None"></asp:CompareValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblInternalDepth" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblInternalDepthResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtRInternalDepth" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtRInternalDepthResource1" TabIndex="19"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revRInternalDepth" runat="server" ControlToValidate="txtRInternalDepth"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revIDResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvRInternalDepth" runat="server" ControlToValidate="txtRInternalDepth"
                                                        Type="Double" MinimumValue="1" MaximumValue="1500" meta:resourcekey="rv1500IDResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvRInternalDepth" runat="server" ControlToValidate="txtRInternalDepth"
                                                        meta:resourcekey="revIDResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvIDepth" runat="server" ControlToValidate="txtRInternalDepth"
                                                        ControlToCompare="txtDepth" Type="Double" Operator="LessThanEqual" meta:resourcekey="cvIDResource1"
                                                        CssClass="ErrValStyle" Display="None"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblInternalHeight" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblInternalHeightResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtRInternalHeight" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtRInternalHeightResource1" 
                                                        TabIndex="20"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revRInternalHeight" runat="server" ControlToValidate="txtRInternalHeight"
                                                        ValidationExpression="([0-9])[0-9]*[.]?[0-9]*" meta:resourcekey="revIHResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvRInternalHeight" runat="server" ControlToValidate="txtRInternalHeight"
                                                        Type="Double" MinimumValue="1" MaximumValue="3000" meta:resourcekey="rv3000IHResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvRInternalHeight" runat="server" ControlToValidate="txtRInternalHeight"
                                                        meta:resourcekey="revIHResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvIHeight" runat="server" ControlToValidate="txtRInternalHeight"
                                                        ControlToCompare="txtHeight" Type="Double" Operator="LessThanEqual" meta:resourcekey="cvIHResource1"
                                                        CssClass="ErrValStyle" Display="None"></asp:CompareValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    &nbsp;
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 20px" valign="middle">
                                    </td>
                                    <td colspan="2" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 38px" valign="middle">
                                        <asp:Label ID="lblEnclosureSpecific" runat="server" CssClass="FieldName" Width="110px"
                                            meta:resourcekey="lblEnclosureSpecificResource1"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <table id="Table4" border="1" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="labelTD" style="width: 100%; height: 29px" valign="top" align="left" colspan="4">
                                                    <asp:RadioButton ID="chkIsBlade" GroupName="EnclosureProp" runat="server" CssClass="checkBoxText"
                                                        meta:resourcekey="chkIsBladeResource1" OnCheckedChanged="chkIsBlade_CheckedChanged"
                                                        AutoPostBack="True" TabIndex="21" />
                                                    &nbsp;&nbsp;
                                                    <asp:RadioButton ID="chkIsEnclosure" GroupName="EnclosureProp" runat="server" CssClass="checkBoxText"
                                                        meta:resourcekey="chkIsEnclosureResource1" OnCheckedChanged="chkIsEnclosure_CheckedChanged"
                                                        AutoPostBack="True" TabIndex="22" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 1000%; height: 29px" valign="top" colspan="4" align="left">
                                                    <asp:Label ID="lblEnclDetails" runat="server" CssClass="FieldName" Width="110px"
                                                        meta:resourcekey="lblEnclDetailsResource1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblEnclFrontRowCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblEnclFrontRowCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtEnclFrontRowCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtEnclFrontRowCountResource1" 
                                                        TabIndex="23"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revEnclFrontRowCount" runat="server" ControlToValidate="txtEnclFrontRowCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revIntEnclFRResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvEnclFrontRowCount" runat="server" ControlToValidate="txtEnclFrontRowCount"
                                                        Type="Integer" MinimumValue="1" MaximumValue="15" meta:resourcekey="rvEnclFRResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEnclFrontRowCount" runat="server" ControlToValidate="txtEnclFrontRowCount"
                                                        meta:resourcekey="revIntEnclFRResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblEnclFrontColCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblEnclFrontColCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtEnclFrontColCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtEnclFrontColCountResource1" 
                                                        TabIndex="24"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revEnclFrontColCount" runat="server" ControlToValidate="txtEnclFrontColCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revIntEnclFCResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvEnclFrontColCount" runat="server" ControlToValidate="txtEnclFrontColCount"
                                                        Type="Integer" MinimumValue="1" MaximumValue="15" meta:resourcekey="rvEnclFCResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEnclFrontColCount" runat="server" ControlToValidate="txtEnclFrontColCount"
                                                        meta:resourcekey="revIntEnclFCResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblEnclRearRowCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblEnclRearRowCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtEnclRearRowCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtEnclRearRowCountResource1" 
                                                        TabIndex="25"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revEnclRearRowCount" runat="server" ControlToValidate="txtEnclRearRowCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revIntEnclRRResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvEnclRearRowCount" runat="server" ControlToValidate="txtEnclRearRowCount"
                                                        Type="Integer" MinimumValue="1" MaximumValue="15" meta:resourcekey="rvEnclRRResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEnclRearRowCount" runat="server" ControlToValidate="txtEnclRearRowCount"
                                                        meta:resourcekey="revIntEnclRRResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblEnclRearColCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblEnclRearColCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtEnclRearColCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtEnclRearColCountResource1" 
                                                        TabIndex="26"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revEnclRearColCount" runat="server" ControlToValidate="txtEnclRearColCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revIntEnclRCResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvEnclRearColCount" runat="server" ControlToValidate="txtEnclRearColCount"
                                                        Type="Integer" MinimumValue="1" MaximumValue="15" meta:resourcekey="rvEnclRCResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEnclRearColCount" runat="server" ControlToValidate="txtEnclRearColCount"
                                                        meta:resourcekey="revIntEnclRCResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 100%; height: 29px" valign="top" colspan="4" align="left">
                                                    <asp:Label ID="lblBladeDetails" runat="server" CssClass="FieldName" Width="110px"
                                                        meta:resourcekey="lblBladeDetailsResource1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblBladeRowCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblBladeRowCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtBladeRowCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtBladeRowCountResource1" TabIndex="27"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revBladeRowCount" runat="server" ControlToValidate="txtBladeRowCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revIntBladeRResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvBladeRowCount" runat="server" ControlToValidate="txtBladeRowCount"
                                                        Type="Integer" MinimumValue="1" MaximumValue="10" meta:resourcekey="rvBladeRResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvBladeRowCount" runat="server" ControlToValidate="txtBladeRowCount"
                                                        meta:resourcekey="revIntBladeRResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="labelTD" style="width: 20%; height: 29px" valign="top">
                                                    <asp:Label ID="lblBladeColCount" runat="server" CssClass="FieldName" Width="200px"
                                                        meta:resourcekey="lblBladeColCountResource1"></asp:Label>
                                                </td>
                                                <td class="ControlTD" style="width: 30%;" valign="top">
                                                    <asp:TextBox ID="txtBladeColCount" runat="server" Width="150px" CssClass="FieldValue"
                                                        MaxLength="10" meta:resourcekey="txtBladeColCountResource1" TabIndex="28"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revBladeColCount" runat="server" ControlToValidate="txtBladeColCount"
                                                        ValidationExpression="^[0-9]{1,2}$" meta:resourcekey="revIntBladeCResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RangeValidator ID="rvBladeColCount" runat="server" ControlToValidate="txtBladeColCount"
                                                        Type="Integer" MinimumValue="1" MaximumValue="10" meta:resourcekey="rvBladeCResource1"
                                                        CssClass="ErrValStyle" Display="None">
                                                    </asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="rfvBladeColCount" runat="server" ControlToValidate="txtBladeColCount"
                                                        meta:resourcekey="revIntBladeCResource1" CssClass="ErrValStyle" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 58px; height: 20px" valign="middle">
                                    </td>
                                    <td colspan="2" style="height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 32px" valign="top">
                                        <asp:Label ID="lblloc0" runat="server" CssClass="FieldName" Width="102px" meta:resourcekey="lblloc0Resource1"></asp:Label>
                                    </td>
                                    <td align="left" style="height: 32px; width: 600px;" valign="top">
                                        <asp:DropDownList ID="ddlTech" runat="server" CssClass="dropdownText" OnSelectedIndexChanged="ddlMfg_SelectedIndexChanged"
                                            Width="197px" TabIndex="29" meta:resourcekey="ddlTechResource1">
                                        </asp:DropDownList>
                                        &nbsp;<%--<asp:RequiredFieldValidator ID="rfvTech" runat="server" CssClass="ErrValStyle"
                                            ControlToValidate="ddlTech" 
                                    ErrorMessage="Select Technology Category" 
                                            InitialValue="0" EnableClientScript="true"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 7px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 7px; width: 600px;" align="left" valign="top">
                                        <table cellpadding="0" cellspacing="0" style="width: 205px">
                                            <tr>
                                                <td align="left" colspan="2" valign="top" style="height: 30px; width: 398px;">
                                                    <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                                        TabIndex="30" SkinID="uwButton" ImageDirectory="" 
                                                        meta:resourcekey="ibCreateResource1">
                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                    </igtxt:WebImageButton>
                                                    <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                                        OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory="" 
                                                        meta:resourcekey="ibResetResource1">
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="lblMessage" runat="server" ReadOnly="true" CssClass="ErrMsgSmallTextBox"
                                                        Width="357px" meta:resourcekey="lblMessageResource1"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px; height: 27px;">
                                        &nbsp;
                                    </td>
                                    <td style="height: 27px; width: 600px;" align="right" colspan="2">
                                        <igtxt:WebImageButton ID="ibExportToExcel" runat="server" 
                                            OnClick="ibExportToExcel_Click" UseBrowserDefaults="False" 
                                            CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibExportToExcelResource1">
                                            <Appearance>
                                                <Image Url="./icons/excelsmall.gif" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        <igtxt:WebImageButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" SkinID="uwButton"
                                            TabIndex="10" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            meta:resourcekey="ibDeleteResource1">
                                            <ClientSideEvents Click="ibDelete_Click" />
                                        </igtxt:WebImageButton>
                                        &nbsp;
                                        <igtxt:WebImageButton ID="ibMap" runat="server" OnClick="ibMap_Click" 
                                            SkinID="uwButton" UseBrowserDefaults="False" CausesValidation="False" ImageDirectory=""
                                            Visible="false" meta:resourcekey="ibMapResource1">
                                            <ClientSideEvents Click="ibMap_Click" />
                                        </igtxt:WebImageButton>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 199px; width: 6px;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="width: 100%;" valign="top">
                            <table style="width: 100%; height: 315px;" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="height: 19px; width: 135%;" valign="top">
                                        <table width="100%" style="vertical-align: top">
                                            <tr>
                                                <td style="width: 100%" align="left" valign="top">
                                                    <ig:WebDataGrid ID="grdModel" runat="server" AutoGenerateColumns="False" DataKeyFields="ModelID"
                                                        OnDataFiltered="grdModel_DataFiltered" Width="98%" OnItemCommand="grdModel_ItemCommand"
                                                        OnDataBound="grdModel_DataBound" HeaderCaptionCssClass="GridHeader"
                                                        Height="300px" OnInitializeRow="grdModel_InitializeRow">
                                                        <Columns>
                                                            <ig:BoundDataField DataFieldName="ModelName" Key="ModelName">
                                                                <Header Text="Model" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="MfgName" Key="MfgName">
                                                                <Header Text="Manufacturer" />
                                                            </ig:BoundDataField>
                                                            <ig:BoundDataField DataFieldName="AssetCount" Key="AssetCount">
                                                                <Header Text="#Assets" />
                                                            </ig:BoundDataField>
                                                             <ig:BoundDataField DataFieldName="ModelType" Key="ModelType">
                                                                <Header Text="Model Type" />
                                                            </ig:BoundDataField>
                                                             <ig:BoundDataField DataFieldName="MountType" Key="MountType" >
                                                                <Header Text="Mount Type" />
                                                            </ig:BoundDataField>
                                                            <ig:TemplateDataField Key="Edit">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ImageUrl="~/images/edit_line.gif" runat="server" CausesValidation="False"
                                                                        CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ModelID") %>'
                                                                        CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                                                </ItemTemplate>
                                                                <Header Text="Edit" />
                                                            </ig:TemplateDataField>
                                                            <ig:TemplateDataField Key="Delete">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDelete" runat="server" onclick="javascript:SelectItemCheckbox(this,'chkAll');"
                                                                        meta:resourcekey="chkDeleteResource1" />
                                                                    <asp:Label ID="lblDeleteID" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "ModelID") %>'
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
                                                            <ig:Filtering>
                                                            </ig:Filtering>
                                                            <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
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
                                                            <ig:Selection CellClickAction="Row" RowSelectType="Single" Enabled="true">
                                                            </ig:Selection>
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
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 100%; width: 6px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnMessage" runat="server" />
    <input type="hidden" id="hdnParentModelID" runat="server" />
    <input type="hidden" id="hdnFilterCount" runat="server" />
    <input type="hidden" id="hdnSPCID" runat="server" />
</asp:Content>
