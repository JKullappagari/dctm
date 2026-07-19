<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="GroupModuleRightsAssignment.aspx.cs" Inherits="ASPX_GrpModuleRights"
    Title="Group Module Access Rights Assignment" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
    //<!--
        function HideCheckbox(node) {
            var childCount = node.get_childrenCount();
            if (childCount != 0) {
                var checkbox = node._get_checkBox();
                var checkboxEl = node._get_checkBox().get_element();
                if (!checkboxEl && node._get_expandCollapseElement()) {
                    checkboxEl = node._get_expandCollapseElement().nextSibling;
                }
                if (checkboxEl) {
                    checkboxEl.style.display = "none";
                }
            }
            for (var x = 0; x < childCount; ++x) {
                var child = node.get_childNode(x);
                HideCheckbox(child);
            }
        }

        function initialize(sender, args) {
            var root = sender.getNode(0);
            while (root != null) {
                HideCheckbox(root);
                root = root.get_nextNode();
            }
        }

        function ToggleExpansion() {
            var grpList = document.getElementById('<%=ddlGroup.ClientID%>');
            if (grpList.options[grpList.selectedIndex].value == "0") {
                alert('Select User Group');
            }
            else {
                var tree = igtree_getTreeById("ctl00MasterContentPlaceHolderuwtGrpModuleRights");
                //var node = tree.getSelectedNode();
                var nodes = tree.getNodes();
                var node;
                var topCounter = 0;
                while (topCounter < nodes.length) {
                    node = nodes[topCounter]
                    SetSelectedNodes(node);
                    topCounter = topCounter + 1;
                    if (node == null)
                        return;
                    if (node.getExpanded() == true)
                        node.setExpanded(false);
                    else
                        node.setExpanded(true);
                }
            }
        }
        //A helper function so that the tree structure can be
        //navigated to reach all nodes

        function SetSelectedNodes(node) {
            var childNodes = node.getChildNodes();
            var childCounter = 0;
            if (node.getTag() == 'Selected') {
                node.getElement().style.color = "blue";
                multiSelectedNodes.push(node);
            }
            else {
                node.getElement().style.color = "";
            }
            while (childCounter < childNodes.length) {
                var childNode = childNodes[childCounter]
                SetSelectedNodes(childNode);
                childCounter = childCounter + 1;
                if (childNode == null)
                    return;
                if (childNode.getExpanded() == true)
                    childNode.setExpanded(false);
                else
                    childNode.setExpanded(true);

            }
        }

        function ibCreate_JS_Click(oButton, oEvent) {
            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
        }

        //-->
    </script>
    <table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
        <tr>
            <td class="labelTD" style="width: 58px; height: 28px;">
                <asp:Label ID="lblGroup" runat="server" CssClass="FieldName" Width="85px" meta:resourcekey="lblGroupResource1"></asp:Label>
            </td>
            <td class="ControlTD" style="height: 28px; width: 600px;" valign="middle">
                <asp:DropDownList ID="ddlGroup" runat="server" AutoPostBack="True" CssClass="dropdownText"
                    OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" TabIndex="1" Width="269px"
                    meta:resourcekey="ddlGroupResource1">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqGroupVal" runat="server" ControlToValidate="ddlGroup"
                    CssClass="ErrValStyle" Display="Dynamic" InitialValue="0" Width="219px" meta:resourcekey="reqGroupValResource1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <table border="0" cellpadding="2" cellspacing="2" style="width: 100%">
                    <tr>
                        <td align="left">
                            <igtxt:WebImageButton ID="wibExpandClose" runat="server" UseBrowserDefaults="False"
                                CausesValidation="true" TabIndex="3" SkinID="uwButton" ImageDirectory="" meta:resourcekey="wibExpandCloseResource1"
                                OnClick="wibExpandClose_Click">
                                 <ClientSideEvents Click="ibCreate_JS_Click" />
                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="20" MaxWidth="63" RenderingType="FileImages"
                                    WidthOfRightEdge="13" />
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <ig:WebDataTree ID="wdtGrpModuleRights" runat="server" meta:resourcekey="uwtGrpModuleRightsResource1"
                                Height="300px" Width="467px" CheckBoxMode="BiState">
                                <ClientEvents Initialize="initialize" />
                            </ig:WebDataTree>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="width: 600px;" align="left" valign="top">
                <table>
                    <tr>
                        <td width="90px">
                            <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                TabIndex="3" SkinID="uwButton" imageurl="images/ig_butMac1.gif" ImageDirectory=""
                                meta:resourcekey="ibCreateResource1">
                                <RoundedCorners WidthOfRightEdge="13" RenderingType="FileImages" MaxWidth="63" MaxHeight="20"
                                    HeightOfBottomEdge="0"></RoundedCorners>
                                    <ClientSideEvents Click ="ibCreate_JS_Click" />
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="ibReset" runat="server" UseBrowserDefaults="False" CausesValidation="False"
                                SkinID="uwButton" OnClick="ibReset_Click" ImageDirectory="" meta:resourcekey="ibResetResource1">
                                <RoundedCorners WidthOfRightEdge="13" RenderingType="FileImages" MaxWidth="63" MaxHeight="20"
                                    HeightOfBottomEdge="0"></RoundedCorners>
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="357px" Height="24px"
                    meta:resourcekey="lblMessageResource1"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
