
/*
File Name:	Reconnect.aspx

Description:	user reconnect screen

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
using System.Web.Services;

public partial class Reconnect : System.Web.UI.Page
{


    [WebMethod]
    public static string SessionTimeout()
    {
        System.Web.HttpContext.Current.Session.Timeout = Convert.ToInt32(System.Web.HttpContext.Current.Session["TimoutInMin"].ToString()) + 1;
        int int_millisecondstimeout = Convert.ToInt32(System.Web.HttpContext.Current.Session["TimoutInMin"].ToString()) * 60000;

        return int_millisecondstimeout.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// reconnect page
    /// </summary>
    /// <author>Atchuta</author>

    protected void btnOk_ServerClick(object sender, EventArgs e)
    {
        Session.Timeout = Convert.ToInt32(Session["TimoutInMin"].ToString()) + 1;
        int int_millisecondstimeout = Convert.ToInt32(Session["TimoutInMin"].ToString()) * 60000;
        System.Text.StringBuilder script = new System.Text.StringBuilder();
        //Old Javascript Code
        script.Append("<script language='javascript'>");
        script.Append("var newTime = '" + int_millisecondstimeout.ToString() + "';");
        script.Append("var args = new Array(newTime);");
        script.Append("window.returnValue = args;");
        script.Append("window.open('','_parent','');");
        script.Append("window.close();");
        script.Append("</script>");
        Page.ClientScript.RegisterStartupScript(typeof(Page), "returnUser", script.ToString());

        //New Jquery Code
        //script.Append("<script type='text/javascript'>");
        //script.Append("var newTime = '" + int_millisecondstimeout.ToString() + "';");
        //script.Append("var args = new Array(newTime);");
        //script.Append("closedialog(args,this);");
        //script.Append("</script>");
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "returnUser", script.ToString());


    }

    /// <summary>
    /// close the browser
    /// </summary>
    /// <author>Atchuta</author>
    protected void btnCancel_ServerClick(object sender, EventArgs e)
    {
        System.Text.StringBuilder script = new System.Text.StringBuilder();

        //Old javascript Code
        script.Append("<script language='javascript'>");
        script.Append("window.returnValue = null;");
        script.Append("window.open('','_parent','');");
        script.Append("window.close();");
        script.Append("</script>");
        Page.ClientScript.RegisterStartupScript(typeof(Page), "returnUser", script.ToString());

        //New Jquery Code
        //script.Append("<script type='text/javascript'>");
        //script.Append("var args = null;");
        //script.Append("closedialog(args,this);");
        //script.Append("</script>");
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "returnUser", script.ToString());
    }

}
