/*
File Name:	AssetWriteoff.aspx.cs

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

public partial class AssetWriteoff : System.Web.UI.Page
{
    private DataTable _dtRights;

    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 Dec 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "AssetWriteoff.aspx";
            Response.Redirect("Login.aspx");
        }
       
        if (IsPostBack!= true)
        {
           Response.Cache.SetCacheability(HttpCacheability.NoCache);
           CheckDocumentDetails();
           FillReasonDropDown();
           this.lblAssetID.Text = Session["AssetID"].ToString();
        }

        //if Selected asset is a Enclosure with child assets than
        //check whether all these child assets are decommissioned than allow to decommission this asset 
        //else throw error messge.
        if (!string.IsNullOrEmpty(Session["AssetID"].ToString()))
        {
            AssetWriteoffBAL awBAL = new AssetWriteoffBAL();
            awBAL.AssetID = int.Parse(Session["AssetID"].ToString());
            DataRow dr = awBAL.FillAssetDetails();
            if (dr != null)
            {
                if (Convert.ToBoolean(dr["IsParent"].ToString()))
                {
                    if (!awBAL.CheckChildAssetsWriteOffStatus(int.Parse(Session["AssetID"].ToString())))
                    {
                        Response.Write("<script language='javascript'>validNavigation = true;alert('Asset can not be written off as active child assets exists');</script>");
                        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
                    }
                }
            }

        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvMusterReason.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtReason.MaxLength.ToString());
                
    }

    /// <summary>
    /// Used to Init the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Write Off Asset";

    }

    /// <summary>
    /// Used to validate the Restriction Status.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckDocumentDetails()
    {
        AssetWriteoffBAL objAssetWriteoffBAL = new AssetWriteoffBAL();
        objAssetWriteoffBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        DataRow dr = objAssetWriteoffBAL.FillAssetDetails();
        bool IsRestricted = false;
        if (dr != null)
        {
            IsRestricted = Convert.ToBoolean(dr["IsWriteOff"]);
        }
        if (IsRestricted == true)
        {
            Response.Write("<script language='javascript'>validNavigation = true;alert('Asset already Written Off by another user');</script>");
            Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
            return false;
        }
        return true;
    }

    private void FillReasonDropDown()
    {
        MusterReasonBAL musterBAL = new MusterReasonBAL();
        DataSet ds = musterBAL.retrieve();
        DataTable dtReason = ds.Tables[0];

        DataRow dr = dtReason.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtReason.Rows.InsertAt(dr, 0);

        if (dtReason.Rows.Count > 0)
        {
            this.ddlMusterReason.DataSource = dtReason;
            this.ddlMusterReason.DataValueField = dtReason.Columns["MusterReasonID"].ToString();
            this.ddlMusterReason.DataTextField = dtReason.Columns["MusterReason"].ToString();
            this.ddlMusterReason.DataBind();
        }
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
        AssetWriteoffBAL objAssetWriteoffBAL = new AssetWriteoffBAL();
        objAssetWriteoffBAL.AssetID = Convert.ToInt32(this.lblAssetID.Text);
        Session["AssetID"] = this.lblAssetID.Text;
        if (Session["UserID"] != null)
            objAssetWriteoffBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objAssetWriteoffBAL.UpdatedBy = 0;
        objAssetWriteoffBAL.RestrictionReason = txtReason.Text.Trim();
        //4.5.0.2
        objAssetWriteoffBAL.ReasonID = Convert.ToInt32(this.ddlMusterReason.SelectedValue);
        objAssetWriteoffBAL.Persist(DALCOperation.Update);
        lblMessage.Text = "Asset Writeoff details saved successfully";
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
        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
    }
}

