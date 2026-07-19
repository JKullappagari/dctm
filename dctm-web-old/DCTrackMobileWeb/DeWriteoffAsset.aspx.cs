/*
File Name:	DeWriteoffAsset.aspx.cs

Description:	Used to De Restrict Document .	

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

public partial class DeWriteoffAsset : System.Web.UI.Page
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

        txtReason.Attributes.Add("onkeypress", "doKeypress(this," + txtReason.MaxLength.ToString() + ");");
        txtReason.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtReason.MaxLength.ToString() + ");");
        txtReason.Attributes.Add("onpaste", "doPaste(this," + txtReason.MaxLength.ToString() + ");");

                
    }
    /// <summary>
    /// Used to init the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Reinstate Asset";
    }

    /// <summary>
    /// Used to validate the De-Restrict Documents.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckDocumentDetails()
    {

        AssetDeWriteoffBAL objDocumentDeWriteoffBAL = new AssetDeWriteoffBAL();
        objDocumentDeWriteoffBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        DataRow dr = objDocumentDeWriteoffBAL.FillAssetDetails();
        bool IsWriteOff = false;
        if (dr != null)
        {
            IsWriteOff = Convert.ToBoolean(dr["IsWriteOff"]);
        }
        if (IsWriteOff)
        {
            dr = objDocumentDeWriteoffBAL.FillWriteoffDetails();
            if (dr != null)
            {
                txtRestrictDate.Text = dr["StatusDate"].ToString();
                txtRestrictReason.Text = dr["Comments"].ToString();
                txtRestrictedBy.Text = dr["LoginName"].ToString();
            }
        }
        else
        {
            Response.Write("<script language='javascript'>validNavigation = true;alert('Asset already Reinstated by another user');</script>");
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
        AssetDeWriteoffBAL objDocumentDeWriteoffBAL = new AssetDeWriteoffBAL();
        objDocumentDeWriteoffBAL.AssetID = Convert.ToInt32(this.lblAssetID.Text);
        Session["AssetID"] = this.lblAssetID.Text;
        if (Session["UserID"] != null)
            objDocumentDeWriteoffBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objDocumentDeWriteoffBAL.UpdatedBy = 0;
        objDocumentDeWriteoffBAL.Reason = txtReason.Text;
        objDocumentDeWriteoffBAL.Persist(DALCOperation.Update);
        lblMessage.Text = "Reinstate details saved successfully";
        lblMessage.Visible = true;
        Session["ReinstateAction"] = "Reinstate";
        Response.Write("<script language='javascript'>window.close();</script>");
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

