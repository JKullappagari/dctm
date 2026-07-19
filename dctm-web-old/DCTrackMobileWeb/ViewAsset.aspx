<%@ Page AutoEventWireup="true" CodeFile="ViewAsset.aspx.cs" Inherits="ViewAsset"
    Language="C#" MasterPageFile="~/PopupMaster.master" Theme="SkinFile" Title="View Asset"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="GeneralPopupContentPlaceHolder">
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <script type="text/javascript">
    //<!--
        function ibAction_Click(oButton, oEvent) {
            //Add code to handle your event here.
            switch (oButton.getText()) {

                case "Close":
                    //	            window.open(window_opener);
                    break;
                //	        case "Edit":            
                //	            var docID = document.getElementById("<%=hdnAssetID.ClientID%>");            
                //	            //alert(docID.value);            
                //	            //window.location = "CreateAsset.aspx?ID" + docID.value;            
                //	            window.location = "www.google.com.sg";            
                //                    break;      
                case "Restrict Asset":
                    //winSettings = "scroll:auto; Width:670px; Height:330px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=670; height=330; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("BarringAsset.aspx", 'Popupwindow', winSettings);
                    break;
                case "Remove Restriction":
                    // winSettings = "scroll:auto; dialogWidth:670px; dialogHeight:470px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=670; height=470; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("UnBarringAsset.aspx", 'Popupwindow', winSettings);
                    break;
                case "Perm Restrict":
                    //winSettings = "scroll:auto; dialogWidth:590px; dialogHeight:290px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=590; height=290; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("AssetRestrict.aspx", 'Popupwindow', winSettings);
                    break;
                case "Remove Perm Restriction":
                    //winSettings = "scroll:auto; dialogWidth:670px; dialogHeight:440px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=670; height=440; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("DeRestrictAsset.aspx", 'Popupwindow', winSettings);
                    break;
                case "Decommission":
                    //winSettings = "scroll:auto; dialogWidth:670px; dialogHeight:340px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=670; height=340; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("MusterAsset.aspx?Action=Decomm", 'Popupwindow', winSettings);
                    break;
                case "Recommission":
                    //winSettings = "scroll:auto; dialogWidth:670px; dialogHeight:340px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=670; height=340; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("MusterAsset.aspx?Action=Recomm", 'Popupwindow', winSettings);
                    break;
                case "Writeoff Asset":
                    //winSettings = "scroll:auto; dialogWidth:670px; dialogHeight:340px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=670; height=340; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("AssetWriteOff.aspx", 'Popupwindow', winSettings);
                    break;
                case "Reinstate Asset":
                    //winSettings = "scroll:auto; dialogWidth:670px; dialogHeight:670px; resizable:yes; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=670; height=670; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("DeWriteOffAsset.aspx", 'Popupwindow', winSettings);
                    break;
                case "Print RFID":
                    //winSettings = "scroll:auto; dialogWidth:480px; dialogHeight:340px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=480; height=340; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("PrintRFID.aspx", 'Popupwindow', winSettings);
                    break;
                case "DeAssign RFID":
                    //winSettings = "scroll:auto; status= true; dialogWidth:580px; dialogHeight:300px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    winSettings = "scroll:auto; width=580; height=300; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; unadorned: yes";
                    popup = open_window("DeAssignRFIDCard.aspx", 'De Assign RFID Card', winSettings);
                    break;


            }
        }
    
    //-->
        
    </script>
    <table id="Table1" border="0" cellpadding="0" style="width: 100%;">
        <tr valign="top">
            <td align="left" style="width: 99%">
                <table>
                    <tr>
                        <td align="left" style="width: 99%" colspan="10">
                            <table id="Table2" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibEdit2" runat="server" ImageDirectory="" OnClick="ibEdit_Click"
                                            SkinID="uwButton" TabIndex="8" Text="Edit" ToolTip="Modify Document" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibEdit2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <%--<ClientSideEvents Click="ibAction_Click" />--%>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibAssignRFID2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Print RFID" ToolTip="Print RFID Tag" UseBrowserDefaults="False"
                                            Width="118px" Visible="False" meta:resourcekey="ibAssignRFID2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibDeassignRFID2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="9" Text="DeAssign RFID" ToolTip="DeAssign RFID Card" UseBrowserDefaults="False"
                                            Width="123px" Visible="False" meta:resourcekey="ibDeassignRFID2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibBarr2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Restrict Asset" ToolTip="Restrict Asset for a period" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibBarr2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibUnBarr2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Remove Restriction" ToolTip="Remove restriction on the asset"
                                            UseBrowserDefaults="False" Width="122px" meta:resourcekey="ibUnBarr2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibPermRestrict2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Perm Restrict" ToolTip="Permanently Restrict Asset" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibPermRestrict2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibDePermRestrict2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Remove Perm Restriction" ToolTip="Remove the Permanent Restriction on the Asset"
                                            UseBrowserDefaults="False" Width="155px" meta:resourcekey="ibDePermRestrict2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibMuster2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Decommission" ToolTip="Decommission Asset" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibMuster2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibWriteOff2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Writeoff Asset" ToolTip="Write-Off Asset" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibWriteOff2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibReinstate2" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Reinstate Asset" ToolTip="Reinstate Written off Asset" UseBrowserDefaults="False"
                                            Width="118px" Visible="False" meta:resourcekey="ibReinstate2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px; width: 96px;">
                                        <igtxt:WebImageButton ID="ibReset2" runat="server" CausesValidation="False" ImageDirectory=""
                                            OnClick="ibReset_Click" SkinID="uwButton" TabIndex="9" Text="Cancel" ToolTip="Cancel"
                                            UseBrowserDefaults="False" Width="118px" meta:resourcekey="ibReset2Resource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td align="center" style="width: 99%;">
                <ig:WebTab ID="uwtCreateAsset" runat="server" AutoPostBack="True" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="0px" DynamicTabs="False" Font-Names="Tahoma"
                    OnTabClick="uwtCreateAsset_TabClick" ThreeDEffect="False" Width="98%" meta:resourcekey="uwtCreateAssetResource1"
                    SelectedTab="0">
                    <Tabs>
                        <ig:ContentTabItem Key="Summary" SelectedImage="./images/report_icon.gif" Text="Summary"
                            meta:resourcekey="TabResource1">
                            <Template>
                                &nbsp;<table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td style="width: 100px; height: 20px;">
                                            <table id="Table3" border="1" cellpadding="2" cellspacing="2" width="100%">
                                                <tr >
                                                    <td class="labelTD" style="width: 20%; height: 20px" align="left">
                                                        <asp:Label ID="lblDocumentId" runat="server" CssClass="FieldName" Width="99px" meta:resourcekey="lblDocumentIdResource1">Serial No #</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%; height: 20px">
                                                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="viewtabfieldvalue" TextMode="MultiLine"
                                                            Height="50px" Width="316px" meta:resourcekey="txtRefNoResource1" ReadOnly="True"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnAssetID" runat="server" />
                                                    </td>
                                                    <td class="ControlTD"  style="width:20%; height: 20px">
                                                        &nbsp;<asp:Label ID="lblRFID" runat="server" CssClass="FieldName" Width="99px" meta:resourcekey="lblRFIDResource1">Tag:</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%; height: 20px" bgcolor="#CCCCCC">
                                                        <asp:TextBox ID="txtRFIDTag" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="154px" meta:resourcekey="txtRFIDTagResource1"></asp:TextBox>
                                                        <asp:TextBox ID="txtRFIDAssigned" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="154px" meta:resourcekey="txtRFIDAssignedResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: Transparent">
                                                    <td class="labelTD" style="width:20%; height: 20px;" align="left">
                                                        <asp:Label ID="lblDocName" runat="server" CssClass="FieldName" Width="103px" meta:resourcekey="lblDocNameResource1">Host Name</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%; height: 20px;">
                                                        <asp:TextBox ID="txtAssetName" runat="server" CssClass="viewtabfieldvalue" TabIndex="1"
                                                            Width="300px" meta:resourcekey="txtAssetNameResource1" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD"  style="width: 20%; height: 20px" align="left">
                                                        <asp:Label ID="lblBusinessUnit0" runat="server" CssClass="FieldName" Width="150px"
                                                            meta:resourcekey="lblBusinessUnit0Resource1">Technology Category</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%; height: 20px">
                                                        <asp:TextBox ID="txtTechCategory" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtTechCategoryResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="labelTD" height="20px" style="width:20%" align="left">
                                                        <asp:Label ID="lblBusinessUnit" runat="server" CssClass="FieldName" Width="99px"
                                                            meta:resourcekey="lblBusinessUnitResource1">Root Entity</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%;" height="20">
                                                        <asp:TextBox ID="txtBusinessUnit" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            meta:resourcekey="txtBusinessUnitResource1"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width: 20%" align="left">
                                                        <asp:Label ID="lblCurrentOwner" runat="server" CssClass="FieldName" Width="108px"
                                                            meta:resourcekey="lblCurrentOwnerResource1">Current Owner</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" colspan="1" height="20" style="width: 30%">
                                                        <asp:TextBox ID="txtCurrentOwner" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtCurrentOwnerResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="labelTD" height="20px" style="width: 20%" align="left">
                                                        <asp:Label ID="lblDept" runat="server" CssClass="FieldName" Width="117px" meta:resourcekey="lblDeptResource1">Site</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%;" height="20">
                                                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            meta:resourcekey="txtDepartmentResource1"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width: 20%;" align="left">
                                                        <asp:Label ID="lblIssuedDate0" runat="server" CssClass="FieldName" Width="113px"
                                                            meta:resourcekey="lblIssuedDate0Resource1">Check Out By</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" colspan="1" height="20" style="width: 30%">
                                                        <asp:TextBox ID="txtCheckOutBy" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtCheckOutByResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="width:20%;height: 15px">
                                                    <td class="labelTD" height="20px" style="width: 224px" align="left">
                                                        <asp:Label ID="Label6" runat="server" CssClass="FieldName" Width="117px" meta:resourcekey="Label6Resource1"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%;" height="20px">
                                                        <asp:TextBox ID="txtLocation" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="300px" meta:resourcekey="txtLocationResource1"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width:20%" align="left">
                                                        <asp:Label ID="lblIssuedDate" runat="server" CssClass="FieldName" Width="113px" meta:resourcekey="lblIssuedDateResource1">Check Out Date</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" colspan="1" height="20" style="width: 30%">
                                                        <asp:TextBox ID="txtIssuedDate" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtIssuedDateResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="width:20%;height: 15px">
                                                    <td class="labelTD" height="20px" style="width: 224px" align="left">
                                                        <asp:Label ID="Label7" runat="server" CssClass="FieldName" Width="117px" meta:resourcekey="Label7Resource1">Manufacturer</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%;" height="20px">
                                                        <asp:TextBox ID="txtMfg" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="300px" meta:resourcekey="txtMfgResource1"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width: 20%" align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="FieldName" Width="99px" meta:resourcekey="Label1Resource1">Received By</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width: 30%">
                                                        <asp:TextBox ID="txtReceivedBy" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtReceivedByResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="labelTD" height="20px" style="width: 20%" align="left">
                                                        <asp:Label ID="Label8" runat="server" CssClass="FieldName" Width="117px" meta:resourcekey="Label8Resource1">Asset Model</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%;" height="20">
                                                        <asp:TextBox ID="txtAssetModel" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="300px" meta:resourcekey="txtAssetModelResource1"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width: 20%" align="left">
                                                        <asp:Label ID="Label2" runat="server" CssClass="FieldName" Width="99px" meta:resourcekey="Label2Resource1">Check In Date</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width: 30%">
                                                        <asp:TextBox ID="txtReceivedDate" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtReceivedDateResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="labelTD" height="20px" style="width: 20%" align="left">
                                                        <asp:Label ID="lblDocGroup" runat="server" CssClass="FieldName" Width="121px" meta:resourcekey="lblDocGroupResource1">Asset Type</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" height="20" style="width:30%;">
                                                        <asp:TextBox ID="txtAssetType" runat="server" CssClass="viewtabfieldvalue" Width="280px"
                                                            meta:resourcekey="txtAssetTypeResource1" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" colspan="1" height="20" style="width: 20%" align="left">
                                                        <asp:Label ID="Label3" runat="server" CssClass="FieldName" Width="115px" meta:resourcekey="Label3Resource1">Restricted From</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" colspan="1" height="20" style="width: 30%">
                                                        <asp:TextBox ID="txtBarredFrom" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtBarredFromResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="labelTD" style="width: 20%" align="left">
                                                        <asp:Label ID="lblDocGroup0" runat="server" CssClass="FieldName" Width="137px" meta:resourcekey="lblDocGroup0Resource1">Operating System</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width:30%;">
                                                        <asp:TextBox ID="txtOS" runat="server" CssClass="viewtabfieldvalue" Width="280px"
                                                            meta:resourcekey="txtOSResource1" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" style="width: 20%" align="left">
                                                        <asp:Label ID="Label4" runat="server" CssClass="FieldName" Width="99px" meta:resourcekey="Label4Resource1">Restricted To</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%">
                                                        <asp:TextBox ID="txtBarredTo" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtBarredToResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="labelTD" style="width: 20%" align="left">
                                                        <asp:Label ID="lblDocGroup2" runat="server" CssClass="FieldName" Height="26px" Width="137px"
                                                            meta:resourcekey="lblDocGroup2Resource1">CPU</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%">
                                                        <asp:TextBox ID="txtCPU" runat="server" CssClass="viewtabfieldvalue" Width="150px"
                                                            meta:resourcekey="txtCPUResource1" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" style="width: 20%" align="left">
                                                        <asp:Label ID="lblDocGroup3" runat="server" CssClass="FieldName" Height="26px" Width="137px"
                                                            meta:resourcekey="lblDocGroup3Resource1">CPU Count</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%">
                                                        <asp:TextBox ID="txtCPUCount" runat="server" CssClass="viewtabfieldvalue" Width="150px"
                                                            meta:resourcekey="txtCPUCountResource1" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="labelTD" style="width: 20%" align="left">
                                                        <asp:Label ID="lblCreatedBy" runat="server" CssClass="FieldName" Text="Custodian"
                                                            Width="90px" meta:resourcekey="lblCreatedByResource1" Visible="true"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%">
                                                        <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="300px" meta:resourcekey="txtCreatedByResource1" Visible="true"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" style="width: 20%" align="left">
                                                        <asp:Label ID="lblDocGroup1" runat="server" CssClass="FieldName" Height="26px" Width="137px"
                                                            meta:resourcekey="lblDocGroup1Resource1">CPU Core</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%">
                                                        <asp:TextBox ID="txtCPUCore" runat="server" CssClass="viewtabfieldvalue" Width="150px"
                                                            meta:resourcekey="txtCPUCoreResource1" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelTD" style="width: 20%" align="left">
                                                        <asp:Label ID="lblCreateDate" runat="server" CssClass="FieldName" Width="145px" meta:resourcekey="lblCreateDateResource1">Created Date</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 30%; height: 22px">
                                                        <asp:TextBox ID="txtCreateDate" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            meta:resourcekey="txtCreateDateResource1"></asp:TextBox>
                                                    </td>
                                                    <td class="ControlTD" style="height: 22px; width: 20%;" align="left">
                                                        <asp:Label ID="Label5" runat="server" CssClass="FieldName" Width="150px" 
                                                            meta:resourcekey="Label5Resource1" Text="Perm Restricted"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 130%; height: 22px;">
                                                        <asp:TextBox ID="txtIsRestricted" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            Width="216px" meta:resourcekey="txtIsRestrictedResource1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;" valign="top" align="left">
                                                        <asp:Label ID="lblExpiryDate" runat="server" CssClass="FieldName" Width="150px" 
                                                            meta:resourcekey="lblExpiryDateResource1" Text="Decommission Date &amp; Reason"></asp:Label>
                                                    </td>
                                                    <td style="height: 20px; width: 30%;" valign="top">
                                                        <asp:TextBox ID="txtMusteringDate" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            meta:resourcekey="txtMusteringDateResource1" Height="16px" Width="125px"></asp:TextBox>
                                                        <asp:TextBox ID="txtMusterReason" runat="server" CssClass="viewtabfieldvalue" ReadOnly="True"
                                                            meta:resourcekey="txtMusterReasonResource1" Width="175px"></asp:TextBox>
                                                    </td>
                                                    <td colspan="1" style="width: 20%; height: 20px" valign="top" align="left">
                                                        <asp:Label ID="lblWriteOff" runat="server" CssClass="FieldName" meta:resourcekey="lblWriteOffResource1"
                                                            Width="99px"></asp:Label>
                                                    </td>
                                                    <td style="width: 30%; height: 20px" valign="top">
                                                        <asp:TextBox ID="txtWriteOff" runat="server" CssClass="viewtabfieldvalue" meta:resourcekey="txtWriteOffResource1"
                                                            ReadOnly="True" Width="216px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width:20%;">
                                                        <asp:Label ID="lblParentAsset" runat="server" CssClass="FieldName" Text="Parent Asset"
                                                            Width="90px" meta:resourcekey="lblParentAssetResource1"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="height: 33px;width:30%">
                                                        <asp:TextBox ID="txtParentAsset" runat="server" Width="160px" CssClass="viewtabfieldvalue"
                                                            ReadOnly="True" MaxLength="150"></asp:TextBox>
                                                    </td>
                                                    <td align="left" style="width:20%;">
                                                        <asp:Label ID="lblAssetName" runat="server" CssClass="FieldName" Width="90px" meta:resourcekey="lblAssetNameResource1"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="height: 33px;width:30%">
                                                        <asp:TextBox ID="txtAssetNames" runat="server" Width="160px" CssClass="viewtabfieldvalue"
                                                            ReadOnly="True" MaxLength="150"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width:20%;">
                                                        <asp:Label ID="lblOrientation" runat="server" CssClass="FieldName" Text="Orientation"
                                                            Width="90px" meta:resourcekey="lblOrientationResource1"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="height: 33px;width:30%">
                                                        <asp:TextBox ID="txtOrientation" runat="server" Width="160px" CssClass="viewtabfieldvalue"
                                                            ReadOnly="True" MaxLength="150"></asp:TextBox>
                                                    </td>
                                                    <td align="left" style="width:20%;">
                                                        <asp:Label ID="lblStartPosition" runat="server" CssClass="FieldName" Width="90px"
                                                            meta:resourcekey="lblStartPositionResource1"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="height: 33px;width:30%">
                                                        <asp:TextBox ID="txtStartPosition" runat="server" Width="160px" CssClass="viewtabfieldvalue"
                                                            ReadOnly="True" MaxLength="150"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="width:20%;">
                                                        <asp:Label ID="lblRU" runat="server" CssClass="FieldName" Text="Orientation" Width="90px"
                                                            meta:resourcekey="lblRUResource1"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="height: 33px;width:30%">
                                                        <asp:TextBox ID="txtRU" runat="server" Width="160px" CssClass="viewtabfieldvalue"
                                                            ReadOnly="True" MaxLength="150"></asp:TextBox>
                                                    </td>
                                                    <td align="left" style="width:20%;">
                                                        <asp:Label ID="lblRackOrStand" runat="server" CssClass="FieldName" meta:resourcekey="lblRackOrStandResource1"
                                                            Width="90px"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="height: 33px;width:30%">
                                                        <asp:TextBox ID="txtRackOrStand" runat="server" CssClass="viewtabfieldvalue" MaxLength="25"
                                                            ReadOnly="True" Width="160px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;<asp:Label ID="lblMessage" runat="server" CssClass="displayText" ForeColor="Red"
                                                meta:resourcekey="lblMessageResource1"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </Template>
                        </ig:ContentTabItem>
                        <ig:ContentTabItem Text="History" Key="History" meta:resourcekey="TabResource2">
                            <Template>
                                &nbsp;<table width="100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <ig:WebDataGrid ID="uwgDocumentHistory" runat="server" AutoGenerateColumns="False"
                                                DataKeyFields="StatusHistoryID" Height="300px" Width="100%" SkinID="uwGridCustom"
                                                HeaderCaptionCssClass="GridHeader">
                                                <Columns>
                                                    <ig:BoundDataField DataFieldName="Date" Key="Date">
                                                        <Header Text="Date" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Status" Key="Status">
                                                        <Header Text="Status" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="By" Key="By">
                                                        <Header Text="By" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Reason" Key="Reason">
                                                        <Header Text="Reason" />
                                                    </ig:BoundDataField>
                                                    <ig:BoundDataField DataFieldName="Period" Key="Period">
                                                        <Header Text="Period" />
                                                    </ig:BoundDataField>
                                                </Columns>
                                                <Behaviors>
                                                    <ig:ColumnResizing>
                                                    </ig:ColumnResizing>
                                                    <ig:Selection CellClickAction="Row">
                                                    </ig:Selection>
                                                    <ig:Sorting>
                                                    </ig:Sorting>
                                                </Behaviors>
                                            </ig:WebDataGrid>
                                        </td>
                                    </tr>
                                </table>
                                &nbsp;
                            </Template>
                        </ig:ContentTabItem>
                    </Tabs>
                </ig:WebTab>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 99%" colspan="10">
                <table>
                    <tr>
                        <td align="center" style="width: 99%">
                            <table id="tblActions" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibEdit" runat="server" ImageDirectory="" OnClick="ibEdit_Click"
                                            SkinID="uwButton" TabIndex="8" Text="Edit" ToolTip="Modify Document" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibEditResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <%--<ClientSideEvents Click="ibAction_Click" />--%>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibAssignRFID" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Print RFID" ToolTip="Print RFID Tag" UseBrowserDefaults="False"
                                            Width="118px" Visible="False" meta:resourcekey="ibAssignRFIDResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibDeassignRFID" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="9" Text="DeAssign RFID" ToolTip="DeAssign RFID Card" UseBrowserDefaults="False"
                                            Width="123px" Visible="False" meta:resourcekey="ibDeassignRFIDResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibBarr" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Restrict Asset" ToolTip="Restrict Asset for a period" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibBarrResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibUnBarr" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Remove Restriction" ToolTip="Remove restriction on the asset"
                                            UseBrowserDefaults="False" Width="122px" meta:resourcekey="ibUnBarrResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibPermRestrict" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Perm Restrict" ToolTip="Permanently Restrict document" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibPermRestrictResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibDePermRestrict" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Remove Perm Restriction" ToolTip="Remove the Permanent Restriction on the Asset"
                                            UseBrowserDefaults="False" Width="155px" meta:resourcekey="ibDePermRestrictResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibMuster" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Decommission" ToolTip="Decommission Asset" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibMusterResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibWriteOff" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Writeoff Asset" ToolTip="Write-Off Asset" UseBrowserDefaults="False"
                                            Width="118px" meta:resourcekey="ibWriteOffResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibReinstate" runat="server" ImageDirectory="" SkinID="uwButton"
                                            TabIndex="8" Text="Reinstate Asset" ToolTip="Reinstate Written off Asset" UseBrowserDefaults="False"
                                            Width="118px" Visible="False" meta:resourcekey="ibReinstateResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                            <ClientSideEvents Click="ibAction_Click" />
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td style="height: 26px">
                                        <igtxt:WebImageButton ID="ibReset" runat="server" CausesValidation="False" ImageDirectory=""
                                            OnClick="ibReset_Click" SkinID="uwButton" TabIndex="9" Text="Cancel" ToolTip="Cancel"
                                            UseBrowserDefaults="False" Width="118px" meta:resourcekey="ibResetResource1">
                                            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                                                WidthOfRightEdge="13" />
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="bottom">
            <td align="center" style="width: 99%">
                <div class="text" style="text-align: left">
                    <asp:TextBox ID="txtLabel" runat="server" CssClass="viewtabfieldvalue" MaxLength="150"
                        ReadOnly="True" TabIndex="1" Width="252px" Wrap="False" meta:resourcekey="txtLabelResource1"></asp:TextBox>&nbsp;</div>
            </td>
        </tr>
    </table>
</asp:Content>
