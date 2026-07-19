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

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpRequest request = Page.Request;
        if (request != null && request.UrlReferrer != null)
        {
            if (request.UrlReferrer.ToString().Contains("MobileLogin.aspx")  || request.Browser.IsMobileDevice)
            {
                Response.Redirect("~/Mobile/Default.aspx");
            }
            else
            {
                Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["iDocTrack_URL"].ToString());
            }
        }
        else
        {
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["iDocTrack_URL"].ToString());
        }
    }
}
