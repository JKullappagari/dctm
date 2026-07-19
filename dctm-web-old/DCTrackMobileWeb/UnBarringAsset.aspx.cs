/*
File Name:	UnBarringAsset.aspx.cs

Description:	Used to UnBar the Document .	

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

public partial class UnBarringAsset : System.Web.UI.Page
{
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Venkatesh</author>
    /// <createdOn>29 September 2006</createdOn>
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
        cvReason.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtUnBarredReason.MaxLength.ToString());

    }


    /// <summary>
    /// Used to Init the page.
    /// </summary>
    /// <author>Venkatesh</author>
    /// <createdOn>29 September 2006</createdOn>
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Remove Asset Restriction";

    }

    /// <summary>
    /// Used to while page was activated.
    /// </summary>
    /// <author>Venkatesh</author>
    /// <createdOn>29 September 2006</createdOn>
    protected void Page_Activate(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Un Barring Documents";
    }
    /// <summary>
    /// Used to validate the Unbarring Status.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckDocumentDetails()
    {
        AssetUnBarringBAL objAssetUnBarringBAL = new AssetUnBarringBAL();
        objAssetUnBarringBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        DataRow dr = objAssetUnBarringBAL.FillAssetDetails();
        bool IsNotbarred= false;
        if (dr != null)
        {
            IsNotbarred = Convert.IsDBNull(dr["BarredStartDate"]);
            if (IsNotbarred != true)
            {
                txtBarredFromDate.Text = dr["BarredStartDate"].ToString();
                txtBarredToDate.Text = dr["BarredEndDate"].ToString();
            }
        }
        if (IsNotbarred != true)
        {
            dr = objAssetUnBarringBAL.FillBarringDetails();
            if (dr != null)
            {
                txtBarredReason.Text = dr["Comments"].ToString();
                txtBarredBy.Text = dr["LoginName"].ToString();
                txtBarredDate.Text = dr["StatusDate"].ToString();

            }
        }
        else
        {
            Response.Write("<script language='javascript'>validNavigation = true;alert('Document already UnBarred by another user');</script>");
            Response.Write("<script language='javascript'>window.close();</script>");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Used to save unbarring details to the database.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>    
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (CheckDocumentDetails() == false)
            return;
        //CheckDocumentDetails();
        AssetUnBarringBAL objAssetUnBarringBAL = new AssetUnBarringBAL();
        Session["AssetID"] = this.lblAssetID.Text.Trim();
        objAssetUnBarringBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        if (Session["UserID"] != null)
            objAssetUnBarringBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objAssetUnBarringBAL.UpdatedBy = 0;
        objAssetUnBarringBAL.UnBarredReason = txtUnBarredReason.Text;
        objAssetUnBarringBAL.Persist(DALCOperation.Update);
        lblMessage.Text = "Un-Barring details saved successfully";
        lblMessage.Visible = true;
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

