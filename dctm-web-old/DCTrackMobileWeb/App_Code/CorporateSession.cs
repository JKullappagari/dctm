using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for CorporateSession
/// </summary>
public class CorporateSession
{
    private System.Collections.Specialized.NameValueCollection colPostFields = new System.Collections.Specialized.NameValueCollection();
    public string Url = "";
    public string Method = "post";
    public string FormName = "iSRPCorporate";

    public void Add(string name, string value)
    {
        colPostFields.Add(name, value);
    }

    public void Post()
    {
        System.Web.HttpContext.Current.Response.Clear();
        System.Web.HttpContext.Current.Response.Write("<html><head>");
        System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload='document.{0}.submit()'>", FormName));
        System.Web.HttpContext.Current.Response.Write(string.Format("<form name='{0}' method='{1}' action='{2}' >", FormName, Method, Url));
        for (int i = 0; i <= colPostFields.Keys.Count - 1; i++)
        {
            System.Web.HttpContext.Current.Response.Write(string.Format("<input name=" + colPostFields.Keys[i] + " type='hidden' value=" + colPostFields[colPostFields.Keys[i]]) + ">");
        }
        System.Web.HttpContext.Current.Response.Write("</form>");
        System.Web.HttpContext.Current.Response.Write("</body></html>");
    }

	public CorporateSession()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}
