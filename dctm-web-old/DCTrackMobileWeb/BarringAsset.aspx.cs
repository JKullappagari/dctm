/*
File Name:	BarringAsset.aspx.cs

Description:	Used to Bar the Document for a period .	

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

public partial class BarringAsset : System.Web.UI.Page
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
            rvBarredFromDate.MaximumValue = DateTime.Now.AddYears(+1).ToShortDateString();//DateTime.Now.AddYears(+10).ToShortDateString();
            rvBarredFromDate.MinimumValue = DateTime.Now.ToShortDateString();
            CheckDocumentDetails();
        }

        txtBarredReason.Attributes.Add("onkeypress", "doKeypress(this," + txtBarredReason.MaxLength.ToString() + ");");
        txtBarredReason.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtBarredReason.MaxLength.ToString() + ");");
        txtBarredReason.Attributes.Add("onpaste", "doPaste(this," + txtBarredReason.MaxLength.ToString() + ");");
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvReason.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtBarredReason.MaxLength.ToString());

    }

    /// <summary>
    /// Used to Init the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Restrict Asset for a period";
    }

    /// <summary>
    /// Used to validate the Barring Status.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckDocumentDetails()
    {
        AssetBarringBAL objAssetBarringBAL = new AssetBarringBAL();
        objAssetBarringBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        DataRow dr = objAssetBarringBAL.FillAssetDetails();
        bool IsNotbarred = false;
        if (dr != null)
        {
            IsNotbarred = Convert.IsDBNull(dr["BarredStartDate"]);

        }
        if (IsNotbarred != true)
        {
            Response.Write("<script language='javascript'>validNavigation = true;alert('Asset already restricted by another user');</script>");
            Response.Write("<script language='javascript'>window.close();</script>");
            return false;

        }
        else
        {

            if (Convert.IsDBNull(dr["BarredStartDate"]) == false)
            {
                if (Convert.ToDateTime(Convert.ToDateTime(dr["BarredEndDate"]).ToShortDateString()) >= Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {
                    Response.Write("<script language='javascript'>validNavigation = true;alert('Asset already restricted by another user');</script>");
                    Response.Write("<script language='javascript'>window.close();</script>");
                    return false;
                }
            }
        }
        return true;
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
        AssetBarringBAL objAssetBarringBAL = new AssetBarringBAL();
        if (Session["AssetID"] != null)
            objAssetBarringBAL.AssetID = Convert.ToInt32(Session["AssetID"]);
        if (Session["UserID"] != null)
            objAssetBarringBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objAssetBarringBAL.UpdatedBy = 0;
        objAssetBarringBAL.BarredReason = txtBarredReason.Text.Trim();
        objAssetBarringBAL.BarredFromDate = Convert.ToDateTime(wdcBarredFromDate.Text);
        objAssetBarringBAL.BarredToDate = Convert.ToDateTime(wdcBarredToDate.Text);
        objAssetBarringBAL.Persist(DALCOperation.Update);
        Response.Write("<script language='javascript'>window.opener.location.reload(false);window.close();</script>");
    }

    /// <summary>
    /// Used to close the screen.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Write("<script language='javascript'>window.close();</script>");
    }
}

