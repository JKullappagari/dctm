/*
File Name:	AssetRestrict.aspx.cs

Description:	Used to Restrict a document permanently .	

Date created:	

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

using iAssetTrack.BAL;
using iAssetTrack.DALC;

public partial class AssetRestrict : System.Web.UI.Page
{
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 Dec 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (IsPostBack!= true)
        {
           Response.Cache.SetCacheability(HttpCacheability.NoCache);
           CheckDocumentDetails();
           this.lblAssetID.Text = Session["AssetID"].ToString();
        }

    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvReason.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtReason.MaxLength.ToString());

    }

    /// <summary>
    /// Used to Init the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Restrict Asset for indefinite period";

    }

    /// <summary>
    /// Used to validate the Restriction Status.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckDocumentDetails()
    {
        AssetRestrictionBAL objAssetRestrictonBAL = new AssetRestrictionBAL();
        objAssetRestrictonBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        DataRow dr = objAssetRestrictonBAL.FillAssetDetails();
        bool IsRestricted = false;
        if (dr != null)
        {
            IsRestricted = Convert.ToBoolean(dr["IsPermRestrict"]);
        }
        if (IsRestricted == true)
        {
            Response.Write("<script language='javascript'>validNavigation = true;alert('Document already Restricted by another user');</script>");
            Response.Write("<script language='javascript'>window.close();</script>");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Used to save Restriction details to the database.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (CheckDocumentDetails() == false)
            return;
        AssetRestrictionBAL objAssetRestrictonBAL = new AssetRestrictionBAL();
        objAssetRestrictonBAL.AssetID = Convert.ToInt32(this.lblAssetID.Text);
        Session["AssetID"] = this.lblAssetID.Text;
        if (Session["UserID"] != null)
            objAssetRestrictonBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objAssetRestrictonBAL.UpdatedBy = 0;
        objAssetRestrictonBAL.RestrictionReason = txtReason.Text.Trim();
        objAssetRestrictonBAL.Persist(DALCOperation.Update);
        lblMessage.Text = "Restriction details saved successfully";
        lblMessage.Visible = true;
        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
    }

    /// <summary>
    /// Used to close the screen.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Write("<script language='javascript'>window.submit = false;</script>");
        Response.Write("<script language='javascript'>window.close();</script>");
    }
}

