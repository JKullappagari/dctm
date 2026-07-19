<%@ Page AutoEventWireup="true" CodeFile="ManageUsers.aspx.cs" EnableEventValidation="false"
    Inherits="ASPX_ManageUsers" Language="C#" MasterPageFile="~/iAssetTrackMasterPage.master"
    Theme="SkinFile" Title="Manage Users Page" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.Misc.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.Misc" TagPrefix="igmisc" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="iggrid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
    //<!--
        function ibCreate_JS_Click(oButton, oEvent) {

            document.getElementById('<%=lblMessage.ClientID%>').innerHTML = "";
            //document.getElementById("ctl00_Master_ContentPlaceHolder_lblMessage").innerHTML = "";
        }
        function searchSubcontractor() {
            var url = "SubContractorLkUpManageUsers.aspx";
            //winSettings = "width=675; height=450; center:yes; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no";   
            var winSettings = "left=150px,top=150px,width=680px,height=450px,toolbar=0,resizable=0,menubar=0,status=0,scrollbars=1";
            var hWnd = open_window(url, "ManageUsers", winSettings)

            //var hWnd=window.open(url,"","scroll:auto;left=200;top=250;; unadorned: yes toolbar,status,scrollbars,resizable,height=420,width=700,screenx=0")
            if ((document.window != null) && (hWnd.opener))
                hWnd.opener = document.window;
            hWnd.focus();
        }

        function ibAction_Click(oButton, oEvent) {


            //Add code to handle your event here.
            switch (oButton.getText()) {

                case "Close":
                    //	            window.open(window_opener);
                    break;

                //            case "Issue Badge":                           
                //                winSettings = "scroll:auto; dialogWidth:480px; dialogHeight:340px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";                              
                //                popup = window.showModalDialog("ReadRFIDCardUser.aspx", 'User', winSettings);                           
                //                //window.location=window.location;                           
                //                break;                             
                //                                            
                //            case "Badge Return":                           
                //                winSettings = "scroll:auto; status= true; dialogWidth:580px; dialogHeight:300px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";                              
                //                popup = window.showModalDialog("DeAssignRFIDCardUser.aspx", 'De Assign RFID Card', winSettings);                           
                //                //window.location=window.location;                           
                //                break;                           
                //                                           
                //            case "Lost Badge":                           
                //                winSettings = "scroll:auto; status= true; dialogWidth:580px; dialogHeight:300px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";                              
                //                popup = window.showModalDialog("LostRFIDCard.aspx", 'Lost RFID Card', winSettings);                           
                //                //window.location=window.location;                           
                //                break;                              
            }
        }

        function enable(pwdid, retypepwdid) {
            var pwd = document.getElementById(pwdid);
            if (pwd != null) {
                alert(pwd.innerText);
                pwd.disabled = false;
                pwd.value = '';
            }
            var retypepwdid = document.getElementById(retypepwdid);
            if (retypepwdid != null) {
                retypepwdid.disabled = true;
                retypepwdid.value = '';
            }
        }
        function changeStatus(id) {
            //window.alert('id = ' + id);
            var sid = document.getElementById(id);
            //window.alert('sid = ' + sid);
            if (sid != null) {
                if (sid.checked == true) {
                    alert('sid.Text = ' + sid.Text);
                    sid.Text = 'Disabled';
                    alert('sid.Text = ' + sid.Text);
                }
                else {
                    sid.Text = "Active";
                }
            }
        }

        function checkLogin() {
            var loginID = document.getElementById('<%=txtLogin.ClientID%>');
            //alert(loginID);
            if (loginID != null) {
                //alert(loginID.value);
                if (loginID.value != '') {
                    //alert('value != null'); 
                    return true;

                }
                else {
                    alert('Login name is required');
                    return false;
                }


            }
        }
        //V3.8-Added on 17Oct2013-By Amar Vidya
        function PanelVisibiity() {
            var checkbox = $find("<%=chkEnforceSiteRestriction.ClientID %>");
            var panel = $find("<%=pnlEnforceSiteRestriction.ClientID %>");
            if (checkbox.selected == true) {
                panel.visible = true;
            }
            else {
                panel.visible = false;
            }
        }
        function enablePanel() {

            var checkbox = document.getElementById('<%=chkEnforceSiteRestriction.ClientID %>')

            var panel = document.getElementById('<%=pnlEnforceSiteRestriction.ClientID %>');
            checkbox.checked ? panel.style.display = 'inline' : panel.style.display = 'none';
            if (checkbox.checked) {
                panel.style.width = "300px";
            }



        }; //*

        // -->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
        <tr>
            <td valign="top" style="width: 100%;">
                <ig:WebTab ID="uwtManageUsers" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="0px" ThreeDEffect="False" Width="100%" DynamicTabs="False" Font-Italic="False"
                    OnTabClick="uwtCreateAsset_TabClick" AutoPostBack="True" TabIndex="29" meta:resourcekey="uwtManageUsersResource1">
                    <Tabs>
                        <ig:ContentTabItem Key="Summary" SelectedImage="./images/report_icon.gif" Text="Summary"
                            meta:resourcekey="TabResource1">
                            <Template>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td style="width: 100px" height="20">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <table id="Table1" cellspacing="2" cellpadding="2" width="100%" border="0">
                                                <tr style="height: 15px">
                                                    <td class="labelTD" style="width: 143px; height: 20px">
                                                        <asp:Label ID="lblLogin" runat="server" CssClass="FieldName" Width="89px" meta:resourcekey="lblLoginResource1"> 
                            Name*</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 403px">
                                                        <asp:TextBox ID="txtLogin" runat="server" Width="100px" CssClass="FieldValue" MaxLength="50"
                                                            TabIndex="1" meta:resourcekey="txtLoginResource1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvLoginIntSearch" runat="server" CssClass="ErrValStyle"
                                                            ControlToValidate="txtLogin" Width="300px" Display="Dynamic" meta:resourcekey="rfvLoginIntSearchResource1"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="regLoginVal" runat="server" ControlToValidate="txtLogin"
                                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[A-Za-z0-9.]+"
                                                            Height="15px" Width="336px" meta:resourcekey="regLoginValResource1"></asp:RegularExpressionValidator>
                                                        <asp:ImageButton ID="ibInternalSearch" runat="server" ImageUrl="~/infragistics/Images/MSNExplorer/PeopleSearch2.gif"
                                                            OnClick="ibInternalSearch_Click" OnClientClick="return checkLogin();" Visible="False"
                                                            meta:resourcekey="ibInternalSearchResource1" />
                                                    </td>
                                                    <td class="labelTD">
                                                        &nbsp;
                                                    </td>
                                                    <td class="ControlTD">
                                                    </td>
                                                </tr>
                                                <tr style="height: 15px;">
                                                    <td style="height: 20px; width: 143px;" class="labelTD">
                                                        <asp:Label ID="lblFirstName" CssClass="FieldName" runat="server" Width="83px" meta:resourcekey="lblFirstNameResource1">
                            Name*</asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="width: 403px">
                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="FieldValue" Width="200px"
                                                            MaxLength="50" TabIndex="3" meta:resourcekey="txtFirstNameResource1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqFirstNameVal" runat="server" CssClass="ErrValStyle"
                                                            ControlToValidate="txtFirstName" Width="127px" Display="Dynamic" meta:resourcekey="reqFirstNameValResource1"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="regFirstNameVal" runat="server" ControlToValidate="txtFirstName"
                                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[A-Za-z0-9 .]+"
                                                            Height="15px" Width="166px" meta:resourcekey="regFirstNameValResource1"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td class="labelTD">
                                                        <asp:Label ID="lblLastName" runat="server" CssClass="FieldName" Width="80px" meta:resourcekey="lblLastNameResource1">
                                                        </asp:Label>
                                                    </td>
                                                    <td class="ControlTD">
                                                        <asp:TextBox ID="txtLastName" CssClass="FieldValue" runat="server" MaxLength="50"
                                                            TabIndex="4" Width="200px" meta:resourcekey="txtLastNameResource1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqLastNameVal" runat="server" CssClass="ErrValStyle"
                                                            ControlToValidate="txtLastName" Width="136px" Display="Dynamic" meta:resourcekey="reqLastNameValResource1"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="regLastNameVal" runat="server" ControlToValidate="txtLastName"
                                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[A-Za-z0-9 .]+"
                                                            Height="15px" Width="166px" meta:resourcekey="regLastNameValResource1"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 22px; width: 143px;" class="labelTD">
                                                        <asp:Label ID="lblEmail" runat="server" CssClass="FieldName" Width="51px" meta:resourcekey="lblEmailResource1"></asp:Label>
                                                    </td>
                                                    <td style="height: 22px; width: 403px;" class="ControlTD">
                                                        <asp:TextBox ID="txtEmail" runat="server" Width="200px" CssClass="FieldValue" MaxLength="80"
                                                            TabIndex="5" meta:resourcekey="txtEmailResource1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqEmailVal" runat="server" ControlToValidate="txtEmail"
                                                            CssClass="ErrValStyle" Display="Dynamic" meta:resourcekey="reqEmailValResource1"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="regEmailVal" runat="server" ControlToValidate="txtEmail"
                                                            CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            Height="15px" Width="166px" meta:resourcekey="regEmailValResource1"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td style="height: 22px;" class="labelTD">
                                                        &nbsp;<asp:Label ID="Label1" runat="server" CssClass="FieldName" Width="80px" meta:resourcekey="Label1Resource1"></asp:Label>
                                                    </td>
                                                    <td valign="top" style="height: 22px" align="left">
                                                        <asp:RadioButton ID="rdoActiveInt" runat="server" CssClass="displayText" GroupName="Internal"
                                                            Style="cursor: hand" Checked="True" TabIndex="6" meta:resourcekey="rdoActiveIntResource1" />&nbsp;
                                                        <asp:RadioButton ID="rdoDisabledInt" runat="server" CssClass="displayText" GroupName="Internal"
                                                            Style="cursor: hand" TabIndex="7" meta:resourcekey="rdoDisabledIntResource1" />&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelTD" style="height: 22px; width: 143px;">
                                                        <asp:Label ID="lblSpecial" runat="server" CssClass="FieldName" Width="51px" Visible="False"
                                                            meta:resourcekey="lblSpecialResource1"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" style="height: 22px; width: 403px;">
                                                        <asp:CheckBox ID="cbIsAllowUserSelection" runat="server" CssClass="label" TabIndex="8"
                                                            Visible="False" meta:resourcekey="cbIsAllowUserSelectionResource1" />
                                                    </td>
                                                    <td class="labelTD" style="height: 22px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 143px; height: 20px" valign="top">
                                                        <asp:Label ID="lblDepartment" runat="server" CssClass="FieldName" Width="103px" meta:resourcekey="lblDepartmentResource1"></asp:Label>
                                                    </td>
                                                    <td style="height: 20px" colspan="3" valign="top" align="left">
                                                        <asp:CheckBoxList ID="cblBusinessUnit" runat="server" CssClass="label" RepeatDirection="Horizontal"
                                                            Style="cursor: hand" TabIndex="10" RepeatColumns="5" Width="100%" Enabled="false"
                                                            meta:resourcekey="cblBusinessUnitResource1">
                                                        </asp:CheckBoxList>
                                                        <asp:Label ID="lblDefaultBUInvalid" runat="server" CssClass="ErrValStyle" Visible="False"
                                                            Width="305px" meta:resourcekey="lblDefaultBUInvalidResource1"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 143px; height: 49px" valign="top" rowspan="1">
                                                    </td>
                                                    <td colspan="3" style="height: 49px" valign="top" align="left">
                                                        &nbsp;<igmisc:WebGroupBox ID="WebGroupBox1" runat="server" CssClass="FieldName" meta:resourcekey="WebGroupBox1Resource1"
                                                            StyleSetName="" TitleAlignment="Left" Width="650px">
                                                            <Template>
                                                                <table>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td style="width: 100px">
                                                                                <asp:Label ID="Label5" runat="server" CssClass="FieldName" Width="103px" meta:resourcekey="Label5Resource1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 100px">
                                                                                <asp:Label ID="lblSiteList" runat="server" CssClass="FieldName" Width="92px" meta:resourcekey="lblSiteListResource1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 100px">
                                                                                <asp:Label ID="lblLocation" runat="server" CssClass="FieldName" Width="69px" meta:resourcekey="lblLocationResource1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100px; height: 21px">
                                                                                <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                                                    OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged" TabIndex="11" Width="229px"
                                                                                    meta:resourcekey="ddlBusinessUnitResource1">
                                                                                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td style="width: 100px; height: 21px">
                                                                                <asp:DropDownList ID="ddlPrimarySite" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                                                                    OnSelectedIndexChanged="ddlPrimarySite_SelectedIndexChanged" TabIndex="12" Width="196px"
                                                                                    meta:resourcekey="ddlPrimarySiteResource1">
                                                                                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td style="width: 100px; height: 21px">
                                                                                <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="True" CssClass="dropdownText"
                                                                                    TabIndex="13" Width="194px" meta:resourcekey="ddlLocationResource1">
                                                                                    <asp:ListItem Value="0" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100px; height: 14px">
                                                                                <asp:CompareValidator ID="cvBusinessUnitID" runat="server" ControlToValidate="ddlBusinessUnit"
                                                                                    CssClass="ErrValStyle" Operator="NotEqual" ValueToCompare="0" Width="192px" meta:resourcekey="cvBusinessUnitIDResource1"></asp:CompareValidator>
                                                                            </td>
                                                                            <td style="width: 100px; height: 14px">
                                                                                <%--<asp:CompareValidator ID="cvSiteID" runat="server" ControlToValidate="ddlPrimarySite"
                                                                                    CssClass="ErrValStyle" Operator="NotEqual" ValueToCompare="0" Width="171px" meta:resourcekey="cvSiteIDResource1"></asp:CompareValidator>--%>
                                                                            </td>
                                                                            <td style="width: 100px; height: 14px">
                                                                               <%-- <asp:CompareValidator ID="cvLocationID" runat="server" ControlToValidate="ddlLocation"
                                                                                    CssClass="ErrValStyle" Width="250px" Operator="NotEqual" ValueToCompare="0" meta:resourcekey="cvLocationIDResource1"></asp:CompareValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100px; height: 14px">
                                                                                <%-- &nbsp;--%>
                                                                                <asp:CheckBox ID="chkEnforceSiteRestriction" runat="server" CssClass="label" OnCheckedChanged="chkEnforceSiteRestriction_OnCheckedChanged"
                                                                                    Style="cursor: hand" Width="100%" Enabled="true" meta:resourcekey="chkEnforceSiteRestrictionResource1"
                                                                                    AutoPostBack="true" onclick="enablePanel()" Visible="False" />
                                                                            </td>
                                                                            <td style="width: 100px; height: 14px">
                                                                            </td>
                                                                            <td style="width: 100px; height: 14px">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="width: 300px; height: 14px">
                                                                                <asp:Panel ID="pnlEnforceSiteRestriction" runat="server" GroupingText="Enforce Site Restriction"
                                                                                    Style="display: none;">
                                                                                    <div>
                                                                                        <iggrid:WebDataGrid ID="grdSiteRestriction" runat="server" AutoGenerateColumns="False"
                                                                                            DataKeyFields="SiteID" EnableRelativeLayout="false" HeaderCaptionCssClass="GridHeader"
                                                                                            Width="300px" Height="250px" OnInitializeRow="grdSiteRestriction_InitializeRow" EnableDataViewState="true">
                                                                                            <Columns>
                                                                                                <iggrid:BoundDataField DataFieldName="SiteID" Key="SiteID" Hidden="true">
                                                                                                    <Header Text="SiteID" />
                                                                                                </iggrid:BoundDataField>
                                                                                                <iggrid:BoundDataField DataFieldName="Site" Key="Site" Width="100px">
                                                                                                    <Header Text="Site" />
                                                                                                </iggrid:BoundDataField>
                                                                                                <iggrid:TemplateDataField Key="Operation">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="ddlSiteOperation" runat="server">
                                                                                                            <asp:ListItem Text="No Access" Value="0"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Read" Value="1"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Read/Write" Value="2"></asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <Header Text="Operation" />
                                                                                                </iggrid:TemplateDataField>
                                                                                                <iggrid:BoundDataField DataFieldName="ResTypeID" Key="ResTypeID" Hidden="true">
                                                                                                    <Header Text="ResTypeID" />
                                                                                                </iggrid:BoundDataField>
                                                                                            </Columns>
                                                                                            <%--<EditorProviders>
                                                                                                <iggrid:DropDownProvider ID="ddpAccess">
                                                                                                    <EditorControl DropDownContainerMaxHeight="200px" EnableAnimations="False" EnableDropDownAsChild="False"
                                                                                                        DisplayMode="DropDown" DropDownOrientation="BottomRight" DropDownContainerWidth="100px"
                                                                                                        runat="server" ID="ddlAccess" EnableCustomValues="false">
                                                                                                    </EditorControl>
                                                                                                </iggrid:DropDownProvider>
                                                                                            </EditorProviders>--%>
                                                                                            <Behaviors>
                                                                                                <iggrid:EditingCore>
                                                                                                   <%-- <Behaviors>
                                                                                                        <iggrid:CellEditing Enabled="true">
                                                                                                            <EditModeActions EnableF2="true" EnableOnActive="true" MouseClick="Single" />
                                                                                                            <ColumnSettings>
                                                                                                                <iggrid:EditingColumnSetting ColumnKey="AccessOp" EditorID="ddpAccess" ReadOnly="false" />
                                                                                                            </ColumnSettings>
                                                                                                        </iggrid:CellEditing>
                                                                                                    </Behaviors>--%>
                                                                                                </iggrid:EditingCore>
                                                                                            </Behaviors>
                                                                                            <EmptyRowsTemplate>
                                                                                                <div style="text-align: center;">
                                                                                                    <br />
                                                                                                    <br />
                                                                                                    <img src="images/WebDataGrid/attention.png" alt="" align="middle" />
                                                                                                    No records returned.
                                                                                                </div>
                                                                                            </EmptyRowsTemplate>
                                                                                        </iggrid:WebDataGrid>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100px; height: 14px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td style="width: 100px; height: 14px">
                                                                            </td>
                                                                            <td style="width: 100px; height: 14px">
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </Template>
                                                        </igmisc:WebGroupBox>
                                                    </td>
                                                </tr>
                                                <tr style="height: 15px">
                                                    <td class="labelTD" height="20" style="width: 143px">
                                                        <asp:Label ID="lblPasswordRule" runat="server" CssClass="FieldName" 
                                                            meta:resourcekey="lblPasswordRuleResource1" Width="88px"></asp:Label></td>
                                                    <td class="ControlTD" colspan="3" height="20" width="90%">
                                                         <asp:Label ID="lblRule" runat="server" CssClass="info" 
                                                            meta:resourcekey="lblRuleResource1"  Width="600px"></asp:Label></td>
                                                </tr>
                                                <tr style="height: 15px">
                                                    <td class="labelTD" height="20" style="width: 143px">
                                                        <asp:Label ID="lblPassword" runat="server" CssClass="FieldName" 
                                                            meta:resourcekey="lblPasswordResource1" Text="Password*" Width="88px"></asp:Label>
                                                    </td>
                                                    <td class="ControlTD" colspan="3" height="20" width="90%">
                                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="FieldValue" 
                                                            MaxLength="20" meta:resourcekey="txtPasswordResource1" TabIndex="16" 
                                                            TextMode="Password" Width="167px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                                            ControlToValidate="txtPassword" CssClass="ErrValStyle" Display="Dynamic" 
                                                            meta:resourcekey="rfvPasswordResource1"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="rfvLoginPassword" runat="server" 
                                                            ControlToValidate="txtPassword" CssClass="ErrValStyle" Display="Dynamic" 
                                                            Height="15px" meta:resourcekey="rfvLoginPasswordResource1" 
                                                            ValidationExpression="(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,10})$" 
                                                            Width="166px"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="labelTD" height="20" style="width: 143px">
                                                    </td>
                                                    <td style="width: 86px" align="left">
                                                        <igtxt:WebImageButton ID="ibGeneratePass" runat="server" ImageDirectory="" SkinID="uwButton"
                                                            UseBrowserDefaults="False" Width="150px" OnClick="ibGeneratePass_Click" CausesValidation="False"
                                                            meta:resourcekey="ibGeneratePassResource1">
                                                            <Appearance>
                                                                <ButtonStyle ForeColor="Black">
                                                                </ButtonStyle>
                                                            </Appearance>
                                                        </igtxt:WebImageButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 143px; height: 20px;">
                                                    </td>
                                                    <td style="height: 20px; width: 403px;" align="left" valign="top">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 69px; height: 10px">
                                                                </td>
                                                                <td style="width: 86px">
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td style="width: 1px">
                                                                </td>
                                                                <td style="width: 1px">
                                                                </td>
                                                                <td style="width: 1px">
                                                                </td>
                                                                <td style="width: 1px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 69px; height: 18px;" valign="top">
                                                                    <igtxt:WebImageButton ID="ibCreateInternal" runat="server" ImageDirectory="" SkinID="uwButton"
                                                                        UseBrowserDefaults="False" Width="118px" OnClick="ibCreateInternal_Click" meta:resourcekey="ibCreateInternalResource1">
                                                                        <Appearance>
                                                                            <ButtonStyle ForeColor="Black">
                                                                            </ButtonStyle>
                                                                        </Appearance>
                                                                        <ClientSideEvents Click="ibCreate_JS_Click" />
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                                <td style="width: 86px">
                                                                    <igtxt:WebImageButton ID="ibCancelInternal" runat="server" ImageDirectory="" SkinID="uwButton"
                                                                        UseBrowserDefaults="False" Width="118px" OnClick="ibCancelInternal_Click" CausesValidation="False"
                                                                        meta:resourcekey="ibCancelInternalResource1">
                                                                        <Appearance>
                                                                            <ButtonStyle ForeColor="Black">
                                                                            </ButtonStyle>
                                                                        </Appearance>
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                                <td style="width: 86px">
                                                                    <igtxt:WebImageButton ID="ibResetPass" runat="server" ImageDirectory="" SkinID="uwButton"
                                                                        UseBrowserDefaults="False" Width="118px" OnClick="ibResetPass_Click" CausesValidation="False"
                                                                        meta:resourcekey="ibResetPassResource1">
                                                                        <Appearance>
                                                                            <ButtonStyle ForeColor="Black">
                                                                            </ButtonStyle>
                                                                        </Appearance>
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                                <td style="width: 1px">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 143px">
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:Button ID="btnAssignRightInt" runat="server" CausesValidation="False" CssClass="displayText"
                                                            Style="cursor: hand" PostBackUrl="~/ASPX/AssignRights.aspx" Visible="False" meta:resourcekey="btnAssignRightIntResource1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 143px;">
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="3" align="left">
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="displayText" ForeColor="Red"
                                                            meta:resourcekey="lblMessageResource1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                        </td>
                                    </tr>
                                </table>
                            </Template>
                        </ig:ContentTabItem>
                        <%--<igtab:Tab Text="History" Key="History">
                            <ContentTemplate>
                                &nbsp;<table width="100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <ig:WebDataGrid ID="uwgUserHistory" runat="server" Height="300px" Width="100%" DataKeyFields="StatusHistoryID"
                                                AutoGenerateColumns="false" HeaderCaptionCssClass="GridHeader">
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
                                            </ig:WebDataGrid>
                                        </td>
                                    </tr>
                                </table>
                                &nbsp;
                            </ContentTemplate>
                        </igtab:Tab>--%>
                    </Tabs>
                </ig:WebTab>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="height: 20px; width: 885px;">
                            <table>
                                <tr>
                                    <td style="width: 863px; height: 21px;">
                                        <asp:Label ID="lblUserID" runat="server" Visible="False" CssClass="displayText" meta:resourcekey="lblUserIDResource1"></asp:Label>
                                        <input id="hdnSubcontractorID" runat="server" style="width: 44px" type="hidden" />
                                        <asp:Label ID="lblUserType" runat="server" CssClass="displayText" Visible="False"
                                            meta:resourcekey="lblUserTypeResource1"></asp:Label>
                                        <input id="hdnSubcontractor" runat="server" style="width: 44px" type="hidden" />
                                        <input id="hdnUserGuid" runat="server" style="width: 44px" type="hidden" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="valMUSummary" runat="server" CssClass="ErrValSummary"
                    ShowSummary="False" ValidationGroup="Internal" meta:resourcekey="valMUSummaryResource1" />
            </td>
        </tr>
    </table>
</asp:Content>
