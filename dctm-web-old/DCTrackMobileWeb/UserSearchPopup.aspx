<%@ Page Language="C#" Theme="SkinFile" MasterPageFile="~/PopupMaster.master" AutoEventWireup="true"
    CodeFile="UserSearchPopup.aspx.cs" Inherits="UserSearchPopup" Title="User Lookup" %>

<%@ Register Src="UserSearchControl.ascx" TagName="UserSearch" TagPrefix="ucxusersearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="GeneralPopupContentPlaceHolder"
    runat="Server">
    <script language="javascript" type="text/javascript">
    
    //<!--

        //Places the selected one from the lookup page in the textbox
        function userSelected(tradecode, tradedescription) {
            var code = tradecode;
            var desc = tradedescription.replace(/~~~/g, "'");
            var ret = new Array(code, desc);
            if (window.opener.closed) {
                //self.close();
            }
            else {
                window.opener.document.getElementById("<%=this.IDField %>").value = code;
                window.opener.document.getElementById("<%=this.NameField %>").value = desc;
                window.opener.document.getElementById("<%=this.DisplayNameField %>").value = desc;

                //alert(window.opener.document.getElementById("ctl00_Master_ContentPlaceHolder_txtSubcontractor").innerText); 
                window.returnValue = tradedescription; //+ "," + tradecode;

                self.close();
            }
        }

    //-->
    </script>
    <ucxusersearch:UserSearch ID="SearchUser" runat="server" BusinessUnit="0" department="0"
        EnableTheming="true" EnableViewState="true" ReturnTextColumn="LastNameCommaFirstName"
        ReturnValueColumn="UserID" usergroup="0" Visible="true"></ucxusersearch:UserSearch>
</asp:Content>
