<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reconnect.aspx.cs" Inherits="Reconnect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" id="reconectpage">
<head runat="server">
    <title>Session timeout warning</title>
    <base target="_self" />
    <script type="text/javascript" src="Scripts/jquery-2.0.0.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <link href="CSS/text.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="Scripts/JScript.js"></script>
</head>
<body id="bReconnect" style="background-color: buttonface; border-right: buttonface 1px outset;
    border-top: buttonface 1px outset; border-left: buttonface 1px outset; border-bottom: buttonface 1px outset;
    border-style: outset;">
    <form id="form1" runat="server">
     <asp:ScriptManager ID="WebScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <script type="text/javascript">
        //<!--
        var nCount = 0;
        var interval = null;

        $(document).ready(function () {
            var interval = window.setInterval("windowRefresh()", 1000);
            nCount = 60;

            $('#btnOk').click(function () {
                PageMethods.SessionTimeout(onSuccess_st, onError_st);
            });

            $('#btnCancel').click(function () {
                var args = null;

                window.returnValue = args;
                window.open('', '_parent', '');
                window.close();
                //                    closedialog(args, this);
            });

        });

        function windowRefresh() {
            self.focus();
            nCount--;
            if (nCount <= 0) {
                window.clearInterval(interval);
                nCount = 0;
                window.returnValue = null;
                window.close();
                //                  $("#hdnArgs", window.opener).val(null);
                //                  $("#mDialog").dialog('close');
            }
            else {
                //                var isIE = /*@cc_on!@*/false;                            // At least IE6
                //                if (isIE) {
                //                    lblSecs.innerText = nCount.toString();
                //                    lblSecs.title = nCount.toString();
                //                }
                //                else {
                $('#lblSecs span').text(nCount.toString());
                //                lblSecs.innerHTML = nCount.toString();
                //                }
            }
        }

        function closedialog(args, caller) {

            $("#hdnArgs", window.opener).val(args);
            $("#mDialog").dialog('close');
            //                        $(".ui.dialog").dialog('close');
            //            window.parent.$('ui-dialog-content:visible').dialog('close');
            //            $(caller).closest('.ui-dialog-content').dialog('close');
        }


        function onSuccess_st(st) {
            var args = new Array(st);
            window.returnValue = args;
            window.open('', '_parent', '');
            window.close();
            //                            closedialog(args, this);
        }

        function onError_st() {
        }
        //-->
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td colspan="2" style="height: 15px">
            </td>
        </tr>
        <tr>
            <td colspan="2" class="displayText" align="center" style="height: 15px">
                Session will expire in
                <div id="lblSecs" style="display: inline; padding-top: 3px; height: 22px; padding-right: 10px"
                    align="center">
                    <span></span>
                </div>
                sec(s) do you want to extend ?
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 15px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <input id="btnOk" type="button" value="Yes" runat="server" style="width: 63px; cursor: hand;"
                    class="displayText" />
                <%--onserverclick="btnOk_ServerClick"--%>
            </td>
            <td>
                &nbsp;
                <input id="btnCancel" type="button" value="No" style="width: 63px; cursor: hand;"
                    runat="server" class="displayText" />
                <%--onserverclick="btnCancel_ServerClick"--%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
