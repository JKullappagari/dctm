using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mobile_MMaster : System.Web.UI.MasterPage
{
    int pageWidth = 480;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.Request != null)
        {
            pageWidth = Page.Request.Browser.ScreenPixelsWidth;
        }
       

    }
}
