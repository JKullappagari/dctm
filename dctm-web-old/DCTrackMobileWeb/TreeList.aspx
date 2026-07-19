<%@ Page Language="C#" AutoEventWireup="true" Theme="SkinFile" MasterPageFile="~/SmallPopupMaster.master"
    CodeFile="TreeList.aspx.cs" Inherits="TreeList" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Assembly="Infragistics45.Web.v16.1, Version=16.1.20161.2236, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
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
        function nodeClicked(sender, args) {
            var ctrlHeader = '<%=header %>';
            var val = args.getNode().get_valueString();
            var txt = args.getNode().get_text();
            var flagIsBlade = args.getNode().get_key();
            var noFlag = "";
            if (ctrlHeader == "Location") {
                window.opener.getValuesFromChild(txt, val,noFlag, 'Loc');
            }
            else if (ctrlHeader == "Source Location") {
                window.opener.getValuesFromChild(txt, val, noFlag, 'Source Location');
            }
            else if (ctrlHeader == "Destination Location") {
                window.opener.getValuesFromChild(txt, val, noFlag, 'Destination Location');
            }
            else {
                window.opener.getValuesFromChild(txt, val, flagIsBlade, 'Model');
            }

            self.close();
        }
        //-->
    </script>

    <div>
        <%--<asp:TreeView ID="tvList" runat="server" Width="350px" >
       </asp:TreeView>--%>
        <ig:WebDataTree ID="TreeLocation" runat="server" Height="350px" Width="350px" meta:resourcekey="TreeLocationResource1" >
            <ClientEvents NodeClick="nodeClicked" />
        </ig:WebDataTree>
    </div>
</asp:Content>
<%--   </form>
</body>
</html>--%>