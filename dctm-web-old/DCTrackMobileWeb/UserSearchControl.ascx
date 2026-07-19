<%@ Control AutoEventWireup="true" CodeFile="UserSearchControl.ascx.cs" EnableViewState="true" Inherits="UserSearchControl" Language="C#" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
<%@ Register Src="CustomerPagerControl.ascx" TagName="CustomerPagerControl" TagPrefix="uc1" %>
<link href="css/text.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    
   // <!--

    //Places the selected one from the lookup page in the textbox
    function selectOwner(tradecode, tradedescription) {

        //alert(tradecode);
        //alert(tradedescription);

        var code = tradecode;
        //alert(tradedescription);
        var desc = tradedescription.replace(/~~~/g, "'");
        var ret = new Array(code, desc);
        //alert(window.opener.status);

        if (window.opener != null) {
            if (window.opener.closed) {
                self.close();
            }
            else {
                userSelected(code, desc);
            }
        }
        else {
            userSelected(code, desc);
        }
    }

   // -->
</script>
<table id="Table3" border="0" cellpadding="2">
    <tbody>
        <tr style="height: 400px">
            <td align="left" valign="top">
                <table id="tblSearch" align="center" border="0" cellpadding="4" cellspacing="0" style="border-right: gray 1px solid;
                    border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;
                    height: 46px" width="100%">
                    <tbody>
                        <tr bgcolor="black">
                            <td class="FilterHeading" colspan="2" style="height: 14px">
                                Owner Lookup
                            </td>
                            <td class="FilterHeading" colspan="1" style="width: 18px; height: 14px">
                            </td>
                            <td class="FilterHeading" colspan="1" style="width: 109px; height: 14px">
                            </td>
                            <td class="FilterHeading" colspan="1" style="height: 14px">
                            </td>
                        </tr>
                        <tr style="height: 19px">
                            <td class="labelTD" style="width: 127px; height: 7px" valign="bottom">
                                <asp:Label ID="lblBusinessUnit" runat="server" CssClass="FieldName" Width="114px">Company</asp:Label>&nbsp;
                            </td>
                            <td class="ControlTD" style="width: 54px; height: 7px" valign="bottom">
                                <asp:DropDownList ID="ddlBusinessUnit" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                    OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged" 
                                    Width="276px" AppendDataBoundItems="True"
                                    TabIndex="1" Enabled="False">
                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="ControlTD" style="width: 18px; height: 7px" valign="bottom">
                            </td>
                            <td class="ControlTD" style="width: 109px; height: 7px" valign="bottom">
                                &nbsp;
                            </td>
                            <td class="ControlTD" style="height: 7px" valign="bottom">
                                &nbsp;
                            </td>
                        </tr>
                        <tr style="height: 19px">
                            <td class="labelTD" style="width: 127px; height: 7px" valign="bottom">
                                <asp:Label ID="lblUserGroup" runat="server" CssClass="FieldName" Width="106px">Division</asp:Label>
                            </td>
                            <td class="ControlTD" style="width: 54px; height: 7px" valign="bottom">
                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="dropdownText" Width="276px"
                                    AppendDataBoundItems="True" TabIndex="3">
                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="ControlTD" style="width: 18px; height: 7px" valign="bottom">
                            </td>
                            <td class="ControlTD" style="width: 109px; height: 7px" valign="bottom">
                                <asp:Label ID="lblName" runat="server" CssClass="FieldName" Width="65px">Name</asp:Label>
                            </td>
                            <td class="ControlTD" style="height: 7px" valign="bottom">
                                <asp:TextBox ID="txtName" runat="server" CssClass="FieldValue" MaxLength="150" TabIndex="4"
                                    Width="274px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 19px">
                            <td class="labelTD" style="width: 127px; height: 19px">
                                &nbsp; &nbsp;&nbsp;
                            </td>
                            <td align="right" colspan="4" style="text-align: left">
                                <asp:Label ID="lblMessage" runat="server" Visible="False" CssClass="textlight"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 19px">
                            <td align="right" colspan="5">
                                <igtxt:WebImageButton ID="wibSearch" runat="server" ImageDirectory="" OnClick="wibSearch_Click"
                                    SkinID="uwButton" Text="Search" ToolTip="Search" UseBrowserDefaults="False" Width="75px"
                                    CausesValidation="False">
                                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                        WidthOfRightEdge="13" />
                                    <Appearance>
                                        <style cursor="Hand" font-names="Arial" font-size="8pt"></style>
                                    </Appearance>
                                </igtxt:WebImageButton>
                                &nbsp; &nbsp;
                                <igtxt:WebImageButton ID="wibReset" runat="server" CausesValidation="False" ImageDirectory=""
                                    OnClick="wibReset_Click" SkinID="uwButton" Text="Cancel" ToolTip="Reset" UseBrowserDefaults="False"
                                    Width="75px" ClickOnEnterKey="False">
                                    <RoundedCorners HeightOfBottomEdge="0" MaxHeight="18" MaxWidth="63" RenderingType="FileImages"
                                        WidthOfRightEdge="13" />
                                    <Appearance>
                                        <style cursor="Hand" font-names="Arial" font-size="8pt"></style>
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table id="Table1" align="center" border="0" cellpadding="0" cellspacing="0" width="100%"
                    style="height: 252px">
                    <tbody>
                        <tr>
                            <td align="center">
                                <ig:WebDataGrid ID="grdOwner" runat="server" AutoGenerateColumns="False" DataKeyFields="OwnerID"
                                    Width="98%" EnableRelativeLayout="True" OnItemCommand="grdOwner_ItemCommand" OnDataBound="grdOwner_DataBound"
                                    HeaderCaptionCssClass="GridHeader" TabIndex="7" OnInitializeRow="grdOwner_InitializeRow"
                                    Height="252px">
                                    <Columns>
                                        <ig:TemplateDataField Key="DisplayName">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="DisplayName" runat="server" 
                                                NavigateUrl='<%# "javascript:selectOwner(" + "&#39;" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "OwnerID") + 
                                                 "&#39;,&#39;" + DataBinder.Eval(((Infragistics.Web.UI.TemplateContainer)Container).DataItem, "FullName") + "&#39;);" %>' Text='<%# Bind("FullName") %>'  ></asp:HyperLink>
                                            </ItemTemplate>
                                            <Header Text="Full Name" />
                                        </ig:TemplateDataField>
                                        <ig:BoundDataField DataFieldName="Division" Key="Division">
                                            <Header Text="Division" />
                                        </ig:BoundDataField>
                                        <ig:BoundDataField DataFieldName="OwnerFirstName" Key="OwnerFirstName">
                                            <Header Text="First Name" />
                                        </ig:BoundDataField>
                                        <ig:BoundDataField DataFieldName="OwnerLastName" Key="OwnerLastName">
                                            <Header Text="Last Name" />
                                        </ig:BoundDataField>
                                    </Columns>
                                    <Behaviors>
                                        <ig:EditingCore>
                                        </ig:EditingCore>
                                        <ig:Filtering>
                                        </ig:Filtering>
                                        <ig:Paging PageSize="10" PagerCssClass="igg_CustomPager" PagerAppearance="Top" Enabled="true">
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
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
