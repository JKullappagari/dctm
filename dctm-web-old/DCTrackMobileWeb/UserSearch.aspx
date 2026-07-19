<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="UserSearch.aspx.cs" Inherits="ASPX_UserSearch"
    Title="Search Users Page" EnableEventValidation="false" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        //<!--
        function ConfirmOnDelete() {

            if (confirm("Are you sure to delete?") == true)

                return true;

            else

                return false;

        }
        function searchPress() {
            if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {
                var btn = document.getElementById('<%=wibSearch.ClientID %>');
                if (btn != null) {
                    btn.click();
                    return false;
                }
            }
            else
                return true;
        }
        function openWin(page, id, wono) {
            //Add code to handle your event here.
            var winSettings;
            //window.alert(page);
            switch (page) {
                case "ResetPassword":
                    winSettings = "scroll:auto; dialogWidth:600px; dialogHeight:275px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    popup = window.showModalDialog("ResetPassword.aspx?ID=" + id, 'Popupwindow', winSettings);
                    break;
                case "Cancel":
                    winSettings = "scroll:auto; dialogWidth:920px; dialogHeight:350px; resizable:no; scroll:no; help:no; toolbar:no; edge:raised; menubar:no; status:no; center:yes; unadorned: yes";
                    popup = window.showModalDialog("JobCardCancellation.aspx?id=" + id + "&wono=" + wono, 'Popupwindow', winSettings);
                    break;
            }
        }
        // -->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
        <tr>
            <td valign="top" style="width: 100%;">
                &nbsp;
                <table id="Table3" style="height: 100%;" cellspacing="2" cellpadding="2" width="100%"
                    border="0" align="left">
                    <tr>
                        <td class="labelTD" style="width: 52px; height: 20px">
                            <asp:Label ID="lblLogin" runat="server" CssClass="FieldName" Width="141px" meta:resourcekey="lblLoginResource1"></asp:Label>
                        </td>
                        <td class="ControlTD" style="width: 738px; height: 20px">
                            <asp:TextBox ID="txtSearch" runat="server" Width="189px" CssClass="FieldValue" MaxLength="50"
                                TabIndex="1" onkeypress="return searchPress();" meta:resourcekey="txtSearchResource1"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regLoginVal" runat="server" ControlToValidate="txtSearch"
                                CssClass="ErrValStyle" Display="Dynamic" ValidationExpression="^[A-Za-z0-9 .]+"
                                Height="15px" Width="336px" meta:resourcekey="regSearchResource1"></asp:RegularExpressionValidator>
                        </td>
                        <td class="labelTD" style="width: 96px; height: 20px">
                        </td>
                        <td class="ControlTD" colspan="2" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="labelTD">
                            &nbsp;
                        </td>
                        <td colspan="2" align="left">
                            <igtxt:WebImageButton ID="wibSearch" runat="server" UseBrowserDefaults="False" OnClick="wibSearch_Click"
                                TabIndex="3" SkinID="uwButton" ImageDirectory="" meta:resourcekey="wibSearchResource1">
                                <RoundedCorners WidthOfRightEdge="13" RenderingType="FileImages" MaxWidth="63" MaxHeight="20"
                                    HeightOfBottomEdge="0"></RoundedCorners>
                            </igtxt:WebImageButton>
                            &nbsp; &nbsp;&nbsp;<igtxt:WebImageButton ID="wibReset" runat="server" UseBrowserDefaults="False"
                                CausesValidation="False" OnClick="wibReset_Click" TabIndex="4" SkinID="uwButton"
                                ImageDirectory="" meta:resourcekey="wibResetResource1">
                                <RoundedCorners HeightOfBottomEdge="0" MaxHeight="20" MaxWidth="63" RenderingType="FileImages"
                                    WidthOfRightEdge="13" />
                            </igtxt:WebImageButton>
                        </td>
                        <td class="ControlTD" valign="top" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="4" valign="top" align="left">
                            <asp:Label ID="lblMessage" runat="server" CssClass="displayText" Font-Bold="True"
                                ForeColor="Red" meta:resourcekey="lblMessageResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="5" valign="top">
                            <ig:WebDataGrid ID="grdUser" runat="server" AutoGenerateColumns="False" DataKeyFields="UserID"
                                Width="100%" EnableRelativeLayout="True" OnItemCommand="grdUser_ItemCommand"
                                OnDataBound="grdUser_DataBound" HeaderCaptionCssClass="GridHeader" TabIndex="7"
                                OnInitializeRow="grdUser_InitializeRow" Height="350px">
                                <Columns>
                                    <ig:BoundDataField DataFieldName="LoginName" Key="LoginName">
                                        <Header Text="Login Name" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="FirstName" Key="FirstName">
                                        <Header Text="FirstName" />
                                    </ig:BoundDataField>
                                     <ig:BoundDataField DataFieldName="LastName" Key="LastName">
                                        <Header Text="LastName" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="Status" Key="Status">
                                        <Header Text="Status" />
                                    </ig:BoundDataField>
                                    <ig:BoundDataField DataFieldName="AssignGroup" Key="AssignGroup">
                                        <Header Text="Assigned to a Group" />
                                    </ig:BoundDataField>
                                    <ig:TemplateDataField Key="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="~/icons/eduser.gif" runat="server" CausesValidation="False"
                                                CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "UserID") %>'
                                                CommandName="Edit" ID="ibEdit" meta:resourcekey="ibEditResource1" />
                                        </ItemTemplate>
                                        <Header Text="Edit" />
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Key="AssignGroups">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="~/icons/ass_right.gif" runat="server" CausesValidation="False"
                                                CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "UserID") %>'
                                                CommandName="Assign" ID="ImageButton1" meta:resourcekey="ibEditResource1" />
                                        </ItemTemplate>
                                        <Header Text="Assign Groups" />
                                    </ig:TemplateDataField>
                                    <ig:TemplateDataField Key="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ImageUrl="~/images/ed_delete.gif" runat="server" CausesValidation="False"
                                                CommandArgument='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "UserID") %>'
                                                CommandName="Delete" ID="ibDelete" meta:resourcekey="ibEditResource1" OnClientClick="return ConfirmOnDelete();">
                                            </asp:ImageButton>
                                            <asp:Label ID="lblDeleteID1" runat="server" Text='<%# DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "UserID") %>'
                                                Visible="False" meta:resourcekey="lblDeleteIDResource1"></asp:Label>
                                        </ItemTemplate>
                                        <Header Text="Delete" />
                                    </ig:TemplateDataField>
                                </Columns>
                                <Behaviors>
                                    <ig:EditingCore>
                                    </ig:EditingCore>
                                    <ig:Filtering>
                                    </ig:Filtering>
                                    <ig:Paging PageSize="50" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
                                        <PagerTemplate>
                                            <uc1:CustomerPagerControl ID="CustomerPager" runat="server" />
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
                        <td style="width: 52px;">
                            &nbsp;
                        </td>
                        <td style="width: 738px">
                            &nbsp;
                        </td>
                        <td style="width: 96px">
                            &nbsp;
                        </td>
                        <td style="width: 290px">
                            &nbsp;
                        </td>
                        <td align="right">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
