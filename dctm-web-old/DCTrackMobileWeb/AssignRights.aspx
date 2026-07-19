<%@ Page Language="C#" Theme="SkinFile" MaintainScrollPositionOnPostback="true" MasterPageFile="~/iAssetTrackMasterPage.master" AutoEventWireup="true" CodeFile="AssignRights.aspx.cs" Inherits="ASPX_AssignRights" Title="Assign Rights Page" EnableEventValidation="false" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Master_ContentPlaceHolder" Runat="Server">
<table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%">
    <tr>
        <td valign="top" style="width: 100%;">
     
				    <table id="Table3" style="height: 100%;" cellspacing="2" cellpadding="2" width="100%" border="0">
					<tr style="height: 15px;">
						<td style="width: 51px; height: 20px;"  class="labelTD">
		                    <asp:label id="lblLogin" runat="server" CssClass="FieldName" Width="89px" 
                                meta:resourcekey="lblLoginResource1"></asp:label>
						</td>
                        <td class="ControlTD" colspan="3">
                            <asp:TextBox ID="txtLogin" runat="server" Width="140px" CssClass="FieldValue" 
                                MaxLength="50" TabIndex="1" ReadOnly="True" 
                                meta:resourcekey="txtLoginResource1" ></asp:TextBox></td>
                    </tr>
                    <tr style="height: 15px;">
						<td style="width: 51px; height: 20px;" class="labelTD" >
		                    <asp:label id="lblFirstName" CssClass="FieldName" runat="server" width="83px" 
                                meta:resourcekey="lblFirstNameResource1"></asp:label>
		                    
						</td>
                        <td class="ControlTD" >
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="FieldValue" 
                                Width="264px" MaxLength="50" TabIndex="2" ReadOnly="True" 
                                meta:resourcekey="txtFirstNameResource1" ></asp:TextBox>&nbsp;</td>
                        <td style="width: 96px; height: 20px;" class="labelTD">
                            &nbsp;</td>
                        <td class="ControlTD" colspan="2">
                            &nbsp;

                        </td>
                    </tr>

					<tr>
						<td style="width: 51px; height: 20px;" class="labelTD">
		                    <asp:label id="lblGroup" runat="server" CssClass="FieldName" Width="89px" 
                                meta:resourcekey="lblGroupResource1"></asp:label>
						</td>
                        <td colspan="2" align="left" valign="top">
                            <asp:CheckBoxList ID="cblGroup" BorderStyle="Groove" CssClass="displayText" 
                                runat="server"  Width="407px" Height="1px" TabIndex="6" RepeatColumns="2" 
                                BorderWidth="1px" style="cursor:hand" meta:resourcekey="cblGroupResource1"  />
                            &nbsp;</td>
                        <td class="ControlTD" valign="top" colspan="2"></td>

                    </tr>

					<tr>
					    <td style="width: 51px; height: 20px;"></td>
                        <td style="width: 738px; height: 20px;" align="left" valign="top" colspan="4">
                             <table cellpadding="0" cellspacing="0"  >
                            <tr>
                                <td style="width: 69px; height: 18px;">
                                                            <igtxt:webimagebutton id="ibCreate" runat="server" 
                                                usebrowserdefaults="False" OnClick="ibCreate_Click" TabIndex="8" 
                                                                SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCreateResource1" >
                                                <RoundedCorners WidthOfRightEdge="13" RenderingType="FileImages" MaxWidth="63" MaxHeight="20" HeightOfBottomEdge="0"></RoundedCorners>

                                                <Appearance>
                                                <Style Cursor="Hand" Font-Size="8pt" Font-Names="Arial"></Style>

<ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt"></ButtonStyle>

                                                </Appearance>
                                                </igtxt:webimagebutton>
                                        </td>
                                <td>
                                                                    <igtxt:webimagebutton id="ibCancel" runat="server" 
                                                usebrowserdefaults="False"  CausesValidation="False" OnClick="ibCancel_Click" 
                                                                        SkinID="uwButton" ImageDirectory="" meta:resourcekey="ibCancelResource1" >
                                                <RoundedCorners WidthOfRightEdge="13" RenderingType="FileImages" MaxWidth="63" MaxHeight="20" HeightOfBottomEdge="0"></RoundedCorners>

                                                <Appearance>
                                                <Style Cursor="Hand" Font-Size="8pt" Font-Names="Arial"></Style>
<ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt"></ButtonStyle>
                                                </Appearance>
                                                </igtxt:webimagebutton>
</td>
                            </tr>
                            </table>
                            </td> 
                    </tr>
                    <tr>
                        <td style="width: 51px;">&nbsp;</td>
                        <td style="width: 738px" align="left" height="20">
                            <asp:Label ID="lblMessage" runat="server" CssClass="FieldName" Height="21px" 
                                ForeColor="Red" meta:resourcekey="lblMessageResource1" ></asp:Label>&nbsp;</td>
                        <td style="width: 96px">&nbsp;</td>
                        <td style="width: 290px">&nbsp;</td>
                        <td align="right">
                      &nbsp;
                        </td>
                    </tr>
                    </table>
               
        </td>
    </tr>
    </table>

</asp:Content>

