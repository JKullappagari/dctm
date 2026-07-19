/*
File Name:	MusterAsset.aspx.cs

Description:	Used to Muster the Document.

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
using iAssetTrackBAL;
using iAssetTrack.BAL;
using iAssetTrack.DALC;

public partial class MusterAsset : System.Web.UI.Page
{
    private string _Action;
    private DataTable _dtRights;

    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "AssetWriteoff.aspx";
            Response.Redirect("Login.aspx");
        }

        if (Request.QueryString.Get("Action") != null)
        {
            _Action = Request.QueryString.Get("Action").ToString();
            Session["MusterAction"] = _Action;

            if (_Action.ToLower().CompareTo("decomm") == 0)
            {
                Session["PageHeader"] = "Decommission Asset";
                wgbBarredDetails.Text = "Enter Decommissioning  Details";
                lblPeriod.Text = "Decommission Date*";
                Page.Title = "Decommission Asset";
                rfvFromDate.ErrorMessage = "<br>Select Decommission Date";
                rvBarredFromDate.ErrorMessage = "Invalid Decommission date - Can be Today or 1 day before";
            }
            else if (_Action.ToLower().CompareTo("recomm") == 0)
            {
                Session["PageHeader"] = "Recommission Asset";
                wgbBarredDetails.Text = "Enter Recommissioning  Details";
                wgbBarredDetails.Text = "Enter Recommissioning  Details";
                lblPeriod.Text = "Recommission Date*";
                Page.Title = "Recommission Asset";
                rfvFromDate.ErrorMessage = "<br>Select Recommission Date";
                rvBarredFromDate.ErrorMessage = "Invalid Recommission date - Can be Today or 1 day before";
            }
        }

        //if Selected asset is a Enclosure with child assets than
        //check whether all these child assets are decommissioned than allow to decommission this asset 
        //else throw error messge.
        if (!string.IsNullOrEmpty(Session["AssetID"].ToString()))
        {
            AssetMusterBAL amBAL = new AssetMusterBAL();
            amBAL.AssetID = int.Parse(Session["AssetID"].ToString());
            DataRow dr = amBAL.FillAssetDetails();
            if (dr != null)
            {
                if (Convert.ToBoolean(dr["IsParent"].ToString()))
                {
                    if (!amBAL.CheckChildAssetsDecomStatus(int.Parse(Session["AssetID"].ToString())))
                    {
                        Response.Write("<script language='javascript'>validNavigation = true;alert('Asset can not be Decommissioned as non decommissioned child assets exists');</script>");
                        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
                    }
                }
            }

        }

        if (!this.IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //rvBarredFromDate.MaximumValue = DateTime.Now.AddYears(+1).ToShortDateString();//DateTime.Now.AddYears(+10).ToShortDateString();
            //rvBarredFromDate.MinimumValue = DateTime.Now.ToShortDateString();

            rvBarredFromDate.MaximumValue = DateTime.Now.ToShortDateString();//DateTime.Now.AddYears(+10).ToShortDateString();
            rvBarredFromDate.MinimumValue = DateTime.Now.AddDays(-1).ToShortDateString();//DateTime.Now.AddYears(+10).ToShortDateString();

            CheckDocumentDetails();

            FillMusteringReasonDropDown();

        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvMusterReason.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtMusterReason.MaxLength.ToString());
    }
    /// <summary>
    /// Used to Init the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Decommission Asset";
    }

    /// <summary>
    /// Used to validate the Barring Status.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckDocumentDetails()
    {
        if (Session["MusterAction"].ToString().ToLower().CompareTo("decomm") == 0)
        {
            AssetMusterBAL objAssetMusterBAL = new AssetMusterBAL();
            objAssetMusterBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
            DataRow dr = objAssetMusterBAL.FillAssetDetails();
            bool IsNotMustered = false;
            if (dr != null)
            {
                IsNotMustered = Convert.IsDBNull(dr["ExpiryDate"]);

            }
            if (IsNotMustered != true)
            {
                Response.Write("<script language='javascript'>validNavigation = true;alert('Asset already Decommissioned by another user');</script>");
                Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
                return false;

            }
            else
            {

                if (Convert.IsDBNull(dr["ExpiryDate"]) == false)
                {
                    if (Convert.ToDateTime(Convert.ToDateTime(dr["ExpiryDate"]).ToShortDateString()) <= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        Response.Write("<script language='javascript'>validNavigation = true;alert('Asset already Decommissioned by another user');</script>");
                        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
                        return false;
                    }
                }
            }
            return true;
        }
        else
        {
            return true;
        }
    }


    private void FillMusteringReasonDropDown()
    {
        MusterReasonBAL musterBAL = new MusterReasonBAL();
        DataSet ds = musterBAL.retrieve();

        DataRow dr = ds.Tables[0].NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        ds.Tables[0].Rows.InsertAt(dr, 0);

        if (ds.Tables[0].Rows.Count > 0)
        {

            this.ddlMusterReason.DataSource = ds.Tables[0];
            this.ddlMusterReason.DataValueField = ds.Tables[0].Columns["MusterReasonID"].ToString();
            this.ddlMusterReason.DataTextField = ds.Tables[0].Columns["MusterReason"].ToString();
            this.ddlMusterReason.DataBind();
        }

    }

    /// <summary>
    /// Used to save Barring details to the database.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (CheckDocumentDetails()==false)
            return;
        AssetMusterBAL objAssetMusterBAL = new AssetMusterBAL();
        if (Session["AssetID"] != null)
            objAssetMusterBAL.AssetID = Convert.ToInt32(Session["AssetID"]);
        if (Session["UserID"] != null)
            objAssetMusterBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objAssetMusterBAL.UpdatedBy = 0;
        objAssetMusterBAL.MusterReason = txtMusterReason.Text.Trim();
        objAssetMusterBAL.MusterReasonID = Convert.ToInt32(this.ddlMusterReason.SelectedValue);
        objAssetMusterBAL.ExpiryDate = Convert.ToDateTime(wdcExpiryDate.Text);
        objAssetMusterBAL.Action = (!string.IsNullOrEmpty(Session["MusterAction"].ToString())? Session["MusterAction"].ToString(): _Action);
        objAssetMusterBAL.Persist(DALCOperation.Update);
        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
    }

    /// <summary>
    /// Used to close the screen.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
    }
}

