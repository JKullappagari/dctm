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

public partial class UserSearchPopup : System.Web.UI.Page
{

    string m_DisplayNameField = "";
    string m_IDField = "ctl00$Master_ContentPlaceHolder$hdnUserID";

    public string IDField
    {
        get { return m_IDField; }
        set { m_IDField = value; }
    }
    string m_NameField = "ctl00$Master_ContentPlaceHolder$hdnUserName";

    public string NameField
    {
        get { return m_NameField; }
        set { m_NameField = value; }
    }

    public string DisplayNameField
    {
        get { return m_DisplayNameField; }
        set { m_DisplayNameField = value; }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Device/ Service Owner Lookup";

        if (Request.QueryString["BusinessUnit"] != "")
        {
            SearchUser.BusinessUnit = Convert.ToInt32(Request.QueryString["BusinessUnit"].ToString());
        }

        if (Request.QueryString["DisplayNameField"] != "")
        {
            this.DisplayNameField = Request.QueryString["DisplayNameField"].ToString();
        }

        if (Request.QueryString["NameField"] != "")
        {
            this.NameField = Request.QueryString["NameField"].ToString();
        }


        if (Request.QueryString["IDField"] != "")
        {
            this.IDField = Request.QueryString["IDField"].ToString();
        }
    }
}
