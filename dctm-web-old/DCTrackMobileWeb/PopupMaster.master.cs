
/*
File Name:	PopupMaster.master

Description:	popup master screen

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Atchuta 		27/03/2006		File has been created.
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class PopupMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["PageHeader"] != null)
        {
            lblPageName.Text = Session["PageHeader"].ToString();
        }

        if (Session["UserID"] == null) 
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();
            script.Append("<script language='javascript'>");            
            script.Append("window.returnValue = null;");            
            script.Append("window.close();");
            script.Append("</script>");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "returnUser", script.ToString());
        }
        this.KeepAlive();
    }

    private void KeepAlive()
    {

        int int_millisecondstimeout = Convert.ToInt32(Session["TimoutInMin"].ToString()) * 60000;

        System.Text.StringBuilder str_script = new StringBuilder();
        str_script.Append("<script type='text/javascript'>");
        str_script.Append("function logout(){");
        str_script.Append("alert('Session has expired');");
        str_script.Append("window.close();");
        str_script.Append(" }");
        str_script.Append("handle=window.setInterval('logout()', " + int_millisecondstimeout.ToString() + " );");
        str_script.Append("</script>");
        Page.ClientScript.RegisterStartupScript(typeof(Page), "logout", str_script.ToString());
    }


}
