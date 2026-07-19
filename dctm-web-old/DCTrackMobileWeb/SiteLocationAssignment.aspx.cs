/*
File Name   :	SiteLocationAssignment.aspx.cs

Description :	Used to assign location(s) to site

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Venkatesan M	27/03/2006		File has been created.
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
using System.Data.SqlClient;
using iAssetTrack.DALC;
using iAssetTrackBAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

/// <summary>
/// Title       : Site and Location Assignment
/// Purpose     : Assign Location(s) to a site
/// Modified By : Venkatesan
/// </summary>
public partial class ASPX_SiteLocationAssignment : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.SiteLocationAssignmentBAL objSiteLocation;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    #endregion

    #region "Page Event Methods"
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {


        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "SiteLocationAssignment.aspx";
            Response.Redirect("Login.aspx");
        }

        Session["PageHeader"] = "Site and Location Assignment";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Site and Location Assignment' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Site and Location Assignment' and Rights = '" + "Assignment" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        //ibAllLeft.Enabled = true;
        ibAllLeft.Visible = false;
        ibLeft.Enabled = true;
        ibAllRight.Enabled = true;
        ibRight.Enabled = true;

        //if (_dtRights.Select("Module = 'Site and Location Assignment' and Rights = '" + "Modify" + "'").Length != 0)
        //{
        //    ibAllLeft.Enabled = true;
        //    ibLeft.Enabled = true;
        //    ibAllRight.Enabled = true;
        //    ibRight.Enabled = true;
        //}
        //else
        //{
        //    ibAllLeft.Enabled = false;
        //    ibLeft.Enabled = false;
        //    ibAllRight.Enabled = false;
        //    ibRight.Enabled = false;
        //}

        if (!IsPostBack)
        {
            objBU = new iAssetTrack.BAL.BusinessUnitBAL();
            DataSet dsBU = objBU.retrieve();
            DataTable dtBU = dsBU.Tables[0];

            objCommon = new CommonBAL();
            objCommon.setDataSource(ddlBU, dtBU, "-Select-");
            ddlBU.SelectedIndex = 1;
            ddlBU.Enabled = false;

            populateSite();

        }
    }

    /// <summary>
    /// Used to assigned sites.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibRight_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        int iCurrentIndex;

        if (lbAvLocation.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        int iSelected = 0;

        for (int i = 0; i < lbAvLocation.Items.Count; i++)
        {
            if (lbAvLocation.Items[i].Selected)
            {
                iSelected = +1;
            }
        }

        if (iSelected == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        for (int i = 0; i < lbAvLocation.Items.Count; i++)
        {
            if (lbAvLocation.Items[i].Selected)
            {
                iCurrentIndex = lbAsLocation.Items.Count;
                ListItem liAsSite = new ListItem(lbAvLocation.Items[i].Text, lbAvLocation.Items[i].Value);
                lbAsLocation.Items.Insert(iCurrentIndex, liAsSite);
            }
        }

        for (int i = lbAvLocation.Items.Count - 1; i >= 0; i--)
        {
            if (lbAvLocation.Items[i].Selected)
            {
                lbAvLocation.Items.RemoveAt(i);
            }
        }

        for (int i = 0; i < lbAvLocation.Items.Count; i++)
        {
            if (lbAvLocation.Items[i].Selected)
            {
                lbAvLocation.Items[i].Selected = false;
            }
        }


        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                lbAsLocation.Items[i].Selected = false;
            }
        }
    }

    /// <summary>
    /// Used to de-assigned sites.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibLeft_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        int iCurrentIndex;
        if (lbAsLocation.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        int iSelected = 0;

        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                iSelected = +1;
            }
        }

        if (iSelected == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }
        else
        {
            //4.5.0.4
            int iSpecialRoomSelected = 0;
            if (iSpecialRoomSelected == 0)
            {
                for (int i = 0; i < lbAsLocation.Items.Count; i++)
                {
                    if (lbAsLocation.Items[i].Selected)
                    {
                        int locID = int.Parse(lbAsLocation.Items[i].Value);
                        SiteLocationAssignmentBAL slBAL = new SiteLocationAssignmentBAL();

                        if (slBAL.CheckSpecialRoom(locID))
                        {
                            objCommon = new CommonBAL();
                            lblMessage.Text = objCommon.displayMessage(MessageCodes.SPECIAL_ROOM_DEASSIGN_ERROR);
                            lblMessage.Visible = true;
                            return;
                        }
                    }
                }

            }
        }

        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                LocationBAL obLoc = new LocationBAL();
                obLoc.LocationID = Int32.Parse(lbAsLocation.Items[i].Value);
                int assetCount = obLoc.GetAssetCountByRoom();
                if (assetCount > 0)
                {
                    lblMessage.Text = "Room cannot be Un-Assigned from Site as it contains Assets";
                    lblMessage.Visible = true;
                    return;
                }
            }
        }

        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                iCurrentIndex = lbAvLocation.Items.Count;
                ListItem liAvSite = new ListItem(lbAsLocation.Items[i].Text, lbAsLocation.Items[i].Value);
                lbAvLocation.Items.Insert(iCurrentIndex, liAvSite);
            }
        }

        for (int i = lbAsLocation.Items.Count - 1; i >= 0; i--)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                lbAsLocation.Items.RemoveAt(i);

            }
        }

        for (int i = 0; i < lbAvLocation.Items.Count; i++)
        {
            if (lbAvLocation.Items[i].Selected)
            {
                lbAvLocation.Items[i].Selected = false;
            }
        }

        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                lbAsLocation.Items[i].Selected = false;
            }
        }
    }

    /// <summary>
    /// Used to assigned ALL sites.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibAllRight_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (lbAvLocation.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }
        int iCurrentIndex;
        for (int i = 0; i < lbAvLocation.Items.Count; i++)
        {
            iCurrentIndex = lbAsLocation.Items.Count;
            ListItem liAsSite = new ListItem(lbAvLocation.Items[i].Text, lbAvLocation.Items[i].Value);
            lbAsLocation.Items.Insert(iCurrentIndex, liAsSite);
        }

        lbAvLocation.Items.Clear();

        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                lbAsLocation.Items[i].Selected = false;
            }
        }
    }

    /// <summary>
    /// Used to de-assigned ALL sites.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibAllLeft_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        int iCurrentIndex;
        if (lbAsLocation.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        //4.5.0.4
        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            if (lbAsLocation.Items[i].Selected)
            {
                int locID = int.Parse(lbAsLocation.Items[i].Value);
                SiteLocationAssignmentBAL slBAL = new SiteLocationAssignmentBAL();

                if (slBAL.CheckSpecialRoom(locID))
                {
                    objCommon = new CommonBAL();
                    lblMessage.Text = objCommon.displayMessage(MessageCodes.SPECIAL_ROOM_DEASSIGN_ERROR);
                    lblMessage.Visible = true;
                    return;
                }
            }
        }


        for (int i = 0; i < lbAsLocation.Items.Count; i++)
        {
            iCurrentIndex = lbAvLocation.Items.Count;
            ListItem liAvSite = new ListItem(lbAsLocation.Items[i].Text, lbAsLocation.Items[i].Value);
            lbAvLocation.Items.Insert(iCurrentIndex, liAvSite);

        }

        lbAsLocation.Items.Clear();

        for (int i = 0; i < lbAvLocation.Items.Count; i++)
        {
            if (lbAvLocation.Items[i].Selected)
            {
                lbAvLocation.Items[i].Selected = false;
            }
        }
    }

    /// <summary>
    /// Used to assign sites for specific BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            objSiteLocation = new iAssetTrack.BAL.SiteLocationAssignmentBAL();
            objSiteLocation.BusinessUnitID = Convert.ToInt32(ddlBU.SelectedValue);
            //Commentd - 10-Nov-2006
            objSiteLocation.SiteID = Convert.ToInt32(ddlSite.SelectedValue);

            string StrLocationIDs = "";

            for (int i = 0; i < lbAsLocation.Items.Count; i++)
            {
                StrLocationIDs += lbAsLocation.Items[i].Value + ";";
            }

            objSiteLocation.LocationIDs = StrLocationIDs;
            objSiteLocation.Delimiters = ";";
            objSiteLocation.Status = 1;
            objSiteLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);

            objSiteLocation.Persist(DALCOperation.Insert);
            //Commented - 11-Nov-06
            //clearFields();
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);

            lblMessage.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
            //lblMessage.Text = ex.Message;
            //lblMessage.Visible = true;
        }
    }

    /// <summary>
    /// Used to clear form control value(s).
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        ddlSite.Focus();
        //populate();
    }

    /// <summary>
    /// Used to call BU dropdown selection changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ddlBU_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateSite();
    }
    #endregion

    #region "User Defined Methods"
    /// <summary>
    /// Populate Available and Assigned site information in the respective list box
    /// </summary>
    private void populate()
    {
        objSiteLocation = new iAssetTrack.BAL.SiteLocationAssignmentBAL();

        objSiteLocation.BusinessUnitID = Convert.ToInt32(ddlBU.SelectedValue);
        //populateSite();

        //Commented - 10-Nov-2006  --Dummy value passed for Parameter
        //objSiteLocation.SiteAccessID = Convert.ToInt32(ddlSiteAccess.SelectedValue);
        objSiteLocation.SiteID = Convert.ToInt32(ddlSite.SelectedValue);

        DataSet dsAvSite = objSiteLocation.retrieveAvailLocations();
        lbAvLocation.DataTextField = dsAvSite.Tables[0].Columns[1].ColumnName;
        lbAvLocation.DataValueField = dsAvSite.Tables[0].Columns[0].ColumnName;
        lbAvLocation.DataSource = dsAvSite.Tables[0].DefaultView;
        lbAvLocation.DataBind();

        DataSet dsAsSite = objSiteLocation.retrieveAssignLocation();
        lbAsLocation.DataTextField = dsAsSite.Tables[0].Columns[1].ColumnName;
        lbAsLocation.DataValueField = dsAsSite.Tables[0].Columns[0].ColumnName;
        lbAsLocation.DataSource = dsAsSite.Tables[0].DefaultView;
        lbAsLocation.DataBind();

        //if (_dtRights.Select("Module = 'Site and Location Assignment' and Rights = '" + "Modify" + "'").Length != 0)
        //{
        //    ibAllLeft.Enabled = true;
        //    ibLeft.Enabled = true;
        //    ibAllRight.Enabled = true;
        //    ibRight.Enabled = true;
        //}
        //else
        //{
        //    ibAllLeft.Enabled = false;
        //    ibLeft.Enabled = false;
        //    ibAllRight.Enabled = false;
        //    ibRight.Enabled = false;
        //}
    }

    private void populateSite()
    {
        if (Convert.ToInt32(ddlBU.SelectedValue) != 0)
        {
            objSite = new iAssetTrack.BAL.SitesBAL();
            DataSet dsSite = objSite.retrieveByBusinessUnitId(Convert.ToInt32(ddlBU.SelectedValue));
            DataTable dtSite = dsSite.Tables[0];
            objCommon = new CommonBAL();
            objCommon.setDataSource(ddlSite, dtSite, "-Select-");
        }
        else
        {
            objSite = new iAssetTrack.BAL.SitesBAL();
            DataSet dsSite = objSite.retrieveByBusinessUnitId(-1);
            DataTable dtSite = dsSite.Tables[0];
            objCommon = new CommonBAL();
            objCommon.setDataSource(ddlSite, dtSite, "-Select-");
        }
        lbAvLocation.Items.Clear();
        lbAsLocation.Items.Clear();

    }

    /// <summary>
    /// Reset fields to default
    /// </summary>
    private void clearFields()
    {
        lblMessage.Visible = false;
        lblMessage.Text = "";
        //ddlBU.SelectedIndex = 0; //commented by kjb on 06 June 2012
        ddlSite.SelectedIndex = 0;
        lbAsLocation.Items.Clear();
        lbAvLocation.Items.Clear();

        //if (lbAsLocation.Items.Count == 0)
        //{
        //    ibLeft.Enabled = false;
        //    ibAllLeft.Enabled = false;
        //}
        //if (lbAvLocation.Items.Count == 0)
        //{
        //    ibRight.Enabled = false;
        //    ibAllRight.Enabled = false;
        //}
    }
    #endregion
    protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
    {
        populate();
    }
}