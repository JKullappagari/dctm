<%@ Page Language="C#" AutoEventWireup="true" Theme="SkinFile" MasterPageFile="~/SmallPopupMaster.master"
    CodeFile="ModelList.aspx.cs" Inherits="ModelList" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%--<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.GridControls" TagPrefix="ig" %>
--%>
<%--<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.ListControls" TagPrefix="ig" %>

--%>
<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

   

</head>
<body>
    <form id="form1" runat="server">--%>
<asp:Content ID="Content_TreeList" ContentPlaceHolderID="GeneralSmallPopupContentPlaceHolder"
    runat="Server">

    <script type="text/javascript" language="javascript">
   // <!--
        function nodeClicked() {
            var ctrlHeader = '<%=header %>';
            var val = "0";
            var txt = '<%=list%>';
            var noFlag = "";
            //alert(txt);
            if (ctrlHeader == "Location") {
                window.opener.getValuesFromChild(txt, val,noFlag, 'Loc');
            }
            else {
                window.opener.getValuesFromChild(txt, val,noFlag, 'Model');
            }

            self.close();
        }
       // -->
    </script>

    <div>
        <%--<asp:TreeView ID="tvList" runat="server" Width="350px" >
       </asp:TreeView>--%>
        <ig:WebDataTree ID="TreeLocation" runat="server" Height="300px" Width="200px" CheckBoxMode="BiState">
        </ig:WebDataTree>
        <igtxt:WebImageButton ID="btnOK" runat="server" ImageDirectory="" OnClick="btnOK_Click" 
            SkinID="uwButton" TabIndex="13" UseBrowserDefaults="False" Width="118px" Text="OK" ToolTip="OK">
            <RoundedCorners HeightOfBottomEdge="0" MaxHeight="23" MaxWidth="500" RenderingType="FileImages"
                WidthOfRightEdge="13" />
            <Appearance>
                <ButtonStyle Cursor="Hand" Font-Names="Arial" Font-Size="8pt">
                </ButtonStyle>
            </Appearance>
        </igtxt:WebImageButton>
    </div>
</asp:Content>
<%--   </form>
</body>
</html>--%>