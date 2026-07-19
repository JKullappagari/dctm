
/*
File Name:	SmallPopupMaster.master

Description:	popup master screen

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Jagadeesh babu K 		12/11/2011		File has been created.
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

public partial class SmallPopupMaster : System.Web.UI.MasterPage
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
    }
}
