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

public partial class UserNotFound : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblHeader.Text = "User Not Found";
        this.lblMsg.Text = "User not found. Please try another user.";
    }
}
