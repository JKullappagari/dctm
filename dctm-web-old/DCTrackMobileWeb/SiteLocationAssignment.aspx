<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/iAssetTrackMasterPage.master"
    AutoEventWireup="true" CodeFile="SiteLocationAssignment.aspx.cs" Inherits="ASPX_SiteLocationAssignment"
    Title="Site and Location Assignment" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="ContentMainPage" ContentPlaceHolderID="Master_ContentPlaceHolder"
    runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;
        text-align: center;">
        <tr>
            <td valign="top" style="width: 100%">
                <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif); height: 35px;">
                        </td>
                        <td valign="top" style="width: 100%; height: 35px;">
                            <table id="Table3" cellspacing="2" cellpadding="2" width="100%" border="0">
                                <tr>
                                    <td class="labelTD" style="width: 67px; height: 24px;">
                                        <asp:Label ID="lblBU" runat="server" CssClass="FieldName" Width="121px" meta:resourcekey="lblBUResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" valign="top">
                                        <asp:DropDownList ID="ddlBU" CssClass="dropdownText" AutoPostBack="True" runat="server"
                                            TabIndex="1" OnSelectedIndexChanged="ddlBU_SelectedIndexChanged" meta:resourcekey="ddlBUResource1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBU"
                                            CssClass="ErrValStyle" Display="Dynamic" InitialValue="0" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelTD" style="width: 67px; height: 24px">
                                        <asp:Label ID="lblSite" runat="server" CssClass="FieldName" meta:resourcekey="lblSiteResource1"></asp:Label>
                                    </td>
                                    <td class="ControlTD" valign="top">
                                        <asp:DropDownList ID="ddlSite" runat="server" AutoPostBack="True" CssClass="dropdownText"
                                            TabIndex="2" OnSelectedIndexChanged="ddlSite_SelectedIndexChanged" meta:resourcekey="ddlSiteResource1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlSite"
                                            CssClass="ErrValStyle" Display="Dynamic" InitialValue="0" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table2" cellspacing="2" cellpadding="2" border="0" align="left">
                                <tr style="height: 15px;">
                                    <td style="height: 214px; width: 243px;" align="left">
                                        <asp:Label ID="lblSite1" runat="server" CssClass="FieldName" meta:resourcekey="lblSite1Resource1"></asp:Label><br />
                                        <br />
                                        &nbsp;<asp:ListBox ID="lbAvLocation" runat="server" CssClass="listboxText" TabIndex="3"
                                            Width="236px" Height="154px" SelectionMode="Multiple" meta:resourcekey="lbAvLocationResource1">
                                        </asp:ListBox>
                                    </td>
                                    <td style="height: 214px;">
                                        <br />
                                        <br />
                                        <br />
                                        <table style="width: 26px">
                                            <tr>
                                                <td style="width: 25px;" class="ControlTD">
                                                    <igtxt:WebImageButton ID="ibRight" runat="server" Height="27px" UseBrowserDefaults="False"
                                                        Width="19px" TabIndex="4" OnClick="ibRight_Click" ImageDirectory="" meta:resourcekey="ibRightResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="./images/next_hover.gif" PressedImageUrl="./images/next_disabled.gif"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="./images/next_disabled.gif"></RoundedCorners>
                                                        <Appearance>
                                                            <style cursor="Hand" font-size="8pt" font-names="Arial">
                                                                </style>
                                                            <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                            </ButtonStyle>
                                                        </Appearance>
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25px;" class="ControlTD">
                                                    <igtxt:WebImageButton ID="ibLeft" runat="server" Height="27px" UseBrowserDefaults="False"
                                                        CausesValidation="False" Width="19px" TabIndex="5" OnClick="ibLeft_Click" ImageDirectory=""
                                                        meta:resourcekey="ibLeftResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="./images/prev_hover.gif" PressedImageUrl="./images/prev_hover.gif"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="./images/prev_disabled.gif"></RoundedCorners>
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25px;" class="ControlTD">
                                                    <igtxt:WebImageButton ID="ibAllRight" runat="server" Height="27px" UseBrowserDefaults="False"
                                                        CausesValidation="False" Width="19px" TabIndex="6" OnClick="ibAllRight_Click"
                                                        ImageDirectory="" meta:resourcekey="ibAllRightResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="./images/ff_hover.gif" PressedImageUrl="./images/ff_hover.gif"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="./images/ff_disabled.gif"></RoundedCorners>
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25px;" class="ControlTD">
                                                    <igtxt:WebImageButton ID="ibAllLeft" runat="server" Height="27px" UseBrowserDefaults="False"
                                                        CausesValidation="False" Width="19px" TabIndex="7" OnClick="ibAllLeft_Click"
                                                        ImageDirectory="" meta:resourcekey="ibAllLeftResource1">
                                                        <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="./images/rew_hover.gif" PressedImageUrl="./images/rew_hover.gif"
                                                            RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                            ImageUrl="./images/rew_disabled.gif"></RoundedCorners>
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="height: 214px; width: 288px;" align="left">
                                        <asp:Label ID="lblSite2" runat="server" CssClass="FieldName" meta:resourcekey="lblSite2Resource1"></asp:Label><br />
                                        <br />
                                        <asp:ListBox ID="lbAsLocation" runat="server" CssClass="listboxText" TabIndex="8"
                                            Width="236px" Height="154px" SelectionMode="Multiple" meta:resourcekey="lbAsLocationResource1">
                                        </asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        <igtxt:WebImageButton ID="ibCreate" runat="server" UseBrowserDefaults="False" OnClick="ibCreate_Click"
                                            TabIndex="9" SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1">
                                            <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                ImageUrl="silver_button.GIF"></RoundedCorners>
                                            <Appearance>
                                                <style cursor="Hand" font-size="8pt" font-names="Arial">
                                                    </style>
                                                <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                                                </ButtonStyle>
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td align="right" valign="top">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="top">
                                        <igtxt:WebImageButton ID="WebImageButton1" runat="server" UseBrowserDefaults="False"
                                            CausesValidation="False" OnClick="ibReset_Click" SkinID="uwButton" ImageDirectory=""
                                            meta:resourcekey="ibResetResource1">
                                            <RoundedCorners WidthOfRightEdge="13" HoverImageUrl="blue_button.GIF" PressedImageUrl="blue_button.GIF"
                                                RenderingType="FileImages" MaxWidth="63" MaxHeight="18" HeightOfBottomEdge="0"
                                                ImageUrl="silver_button.GIF"></RoundedCorners>
                                            <Appearance>
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td align="center" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 35px; width: 41px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(images/table_left_middle.gif);">
                        </td>
                        <td valign="top" style="width: 912px" align="left">
                            <asp:Label ID="lblMessage" runat="server" CssClass="ErrMsgSmall" Width="492px" meta:resourcekey="lblMessageResource1"></asp:Label>
                        </td>
                        <td style="background-image: url(images/table_right_middle.gif); height: 100%; width: 41px;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
