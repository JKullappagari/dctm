/*
File Name:	DeWriteoffAsset.aspx.cs

Description:	Used to Reinstate document .	

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

using iAssetTrack.BAL;
using iAssetTrack.DALC;

public partial class DeRestrictAsset : System.Web.UI.Page
{
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (IsPostBack!= true)
        {
              Response.Cache.SetCacheability(HttpCacheability.NoCache);
              this.lblAssetID.Text = Session["AssetID"].ToString();
              CheckDocumentDetails();
        }

    }


    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvReason.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtReason.MaxLength.ToString());

    }

    /// <summary>
    /// Used to init the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "De-Restrict Asset";
    }

    /// <summary>
    /// Used to validate the De-Restrict Documents.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckDocumentDetails()
    {

        AssetDeRestrictionBAL objDocumentDeRestrictBAL = new AssetDeRestrictionBAL();
        objDocumentDeRestrictBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        DataRow dr = objDocumentDeRestrictBAL.FillAssetDetails();
        bool IsPermRestrict = false;
        if (dr != null)
        {
            IsPermRestrict = Convert.ToBoolean(dr["IsPermRestrict"]);
        }
        if (IsPermRestrict)
        {
            dr = objDocumentDeRestrictBAL.FillRestrictionDetails();
            if (dr != null)
            {
                txtRestrictDate.Text = dr["StatusDate"].ToString();
                txtRestrictReason.Text = dr["Comments"].ToString();
                txtRestrictedBy.Text = dr["LoginName"].ToString();
            }
        }
        else
        {
            Response.Write("<script language='javascript'>validNavigation = true;alert('Asset already De Restricted by another user');</script>");
            Response.Write("<script language='javascript'>window.close();</script>");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Used to save De Black listing details to the database.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
   protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (CheckDocumentDetails() == false)
            return;
        AssetDeRestrictionBAL objDocumentDeRestrictBAL = new AssetDeRestrictionBAL();
        objDocumentDeRestrictBAL.AssetID = Convert.ToInt32(this.lblAssetID.Text);
        Session["AssetID"] = this.lblAssetID.Text;
        if (Session["UserID"] != null)
            objDocumentDeRestrictBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objDocumentDeRestrictBAL.UpdatedBy = 0;
        objDocumentDeRestrictBAL.Reason = txtReason.Text.Trim();
        objDocumentDeRestrictBAL.Persist(DALCOperation.Update);
        lblMessage.Text = "De-Restriction details saved successfully";
        lblMessage.Visible = true;
        Session["RestrictAction"] = "Restrict";
        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
    }

    /// <summary>
    /// Used to close screen.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Write("<script language='javascript'>window.close();</script>");
    }
}

