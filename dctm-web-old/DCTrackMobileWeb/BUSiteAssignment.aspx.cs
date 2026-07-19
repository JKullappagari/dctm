/*
File Name   :	BUSiteAssignment.aspx.cs

Description :	Used to assign site(s) for business unit

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

/// <summary>
/// Title       : Business Unit and Site Assignment
/// Purpose     : Assign Site(s) for Business Unit
/// Modified By : Venkatesan
/// </summary>
public partial class ASPX_BUSiteAssignment : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    //private iAssetTrack.BAL.SiteAccessSchemeBAL objSiteAccess;
    private iAssetTrack.BAL.BUSiteAssignmentBAL objBUSite;
    private iAssetTrack.BAL.CommonBAL objCommon;
    //DataTable _dtRights;

    //Order the First Column in Sequence for maximum efficiency
    private String[,] saModuleRightsControlList = new String[6, 3]
    {
        {"Create", "ibCreate", "Visible,Enabled"},
        {"Modify", "ibCreate", "Enabled"},
        {"Modify", "ibAllLeft", "Enabled"},
        {"Modify", "ibLeft", "Enabled"},
        {"Modify", "ibAllRight", "Enabled"},
        {"Modify", "ibRight", "Enabled"}
    };

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
        if (SiteMap.CurrentNode != null) Session["PageHeader"] = SiteMap.CurrentNode.Title;
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;


        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "BUSiteAssignment.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Assign Sites to Company' and Rights = '" + "View" + "'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Assign Sites to Company' and Rights = '" + "Assignment" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        ibAllLeft.Enabled = true;
        ibLeft.Enabled = true;
        ibAllRight.Enabled = true;
        ibRight.Enabled = true;

        //if (_dtRights.Select("Module = 'Business Unit and Site Assignment' and Rights = '" + "Modify" + "'").Length != 0)
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
            //PageControlAccess.CheckPageControlAccess(this, saModuleRightsControlList);
            //commented by kjb on 07 June 2012

            objBU = new iAssetTrack.BAL.BusinessUnitBAL();
            objBU.BusinessUnitID = Convert.ToInt16(Session["BUID"]);
            DataSet dsBU = objBU.retrieve();
            DataTable dtBU = dsBU.Tables[0];

            objCommon = new CommonBAL();
            objCommon.setDataSource(ddlBU, dtBU, "-Select-");
            ddlBU.SelectedIndex = 1;
            ddlBU.Enabled = false;
            populate();

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

        if (lbAvSite.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        int iSelected = 0;

        for (int i = 0; i < lbAvSite.Items.Count; i++)
        {
            if (lbAvSite.Items[i].Selected)
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

        for (int i = 0; i < lbAvSite.Items.Count; i++)
        {
            if (lbAvSite.Items[i].Selected)
            {
                iCurrentIndex = lbAsSite.Items.Count;
                ListItem liAsSite = new ListItem(lbAvSite.Items[i].Text, lbAvSite.Items[i].Value);
                lbAsSite.Items.Insert(iCurrentIndex, liAsSite);
            }
        }

        for (int i = lbAvSite.Items.Count - 1; i >= 0; i--)
        {
            if (lbAvSite.Items[i].Selected)
            {
                lbAvSite.Items.RemoveAt(i);
            }
        }

        for (int i = 0; i < lbAvSite.Items.Count; i++)
        {
            if (lbAvSite.Items[i].Selected)
            {
                lbAvSite.Items[i].Selected = false;
            }
        }


        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            if (lbAsSite.Items[i].Selected)
            {
                lbAsSite.Items[i].Selected = false;
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

        String csvSitesWithLocation = hdnSitesWithLocation.Value;
        String[] arrSitesWithLocation = csvSitesWithLocation.Split(',');


        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            if (lbAsSite.Items[i].Selected)
            {
                foreach (String siteID in arrSitesWithLocation)
                {
                    if (lbAsSite.Items[i].Value.Trim() == siteID)
                    {
                        lblMessage.Text = "Sites cannot be Un-Assigned from Company as it contains Locations";
                        lblMessage.Visible = true;
                        return;
                    }
                }
            }
        }

        int iCurrentIndex;
        if (lbAsSite.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        int iSelected = 0;
        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            if (lbAsSite.Items[i].Selected)
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

        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            if (lbAsSite.Items[i].Selected)
            {
                iCurrentIndex = lbAvSite.Items.Count;
                ListItem liAvSite = new ListItem(lbAsSite.Items[i].Text, lbAsSite.Items[i].Value);
                lbAvSite.Items.Insert(iCurrentIndex, liAvSite);
            }
        }

        for (int i = lbAsSite.Items.Count - 1; i >= 0; i--)
        {
            if (lbAsSite.Items[i].Selected)
            {
                lbAsSite.Items.RemoveAt(i);

            }
        }

        for (int i = 0; i < lbAvSite.Items.Count; i++)
        {
            if (lbAvSite.Items[i].Selected)
            {
                lbAvSite.Items[i].Selected = false;
            }
        }

        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            if (lbAsSite.Items[i].Selected)
            {
                lbAsSite.Items[i].Selected = false;
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
        if (lbAvSite.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }
        int iCurrentIndex;
        for (int i = 0; i < lbAvSite.Items.Count; i++)
        {
            iCurrentIndex = lbAsSite.Items.Count;
            ListItem liAsSite = new ListItem(lbAvSite.Items[i].Text, lbAvSite.Items[i].Value);
            lbAsSite.Items.Insert(iCurrentIndex, liAsSite);
        }

        lbAvSite.Items.Clear();

        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            if (lbAsSite.Items[i].Selected)
            {
                lbAsSite.Items[i].Selected = false;
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
        String csvSitesWithLocation = hdnSitesWithLocation.Value;
        String[] arrSitesWithLocation = csvSitesWithLocation.Split(',');

        //Select all on the right  and then move to left
        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            lbAsSite.Items[i].Selected = true;
        }

        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            if (lbAsSite.Items[i].Selected)
            {
                foreach (String siteID in arrSitesWithLocation)
                {
                    if (lbAsSite.Items[i].Value.Trim() == siteID)
                    {
                        lblMessage.Text = "Sites cannot be Un-Assigned from Company as one of them contains Locations";
                        lblMessage.Visible = true;
                        return;
                    }
                }
            }
        }

        int iCurrentIndex;
        if (lbAsSite.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            iCurrentIndex = lbAvSite.Items.Count;
            ListItem liAvSite = new ListItem(lbAsSite.Items[i].Text, lbAsSite.Items[i].Value);
            lbAvSite.Items.Insert(iCurrentIndex, liAvSite);

        }

        lbAsSite.Items.Clear();

        for (int i = 0; i < lbAvSite.Items.Count; i++)
        {
            if (lbAvSite.Items[i].Selected)
            {
                lbAvSite.Items[i].Selected = false;
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
        objBUSite = new iAssetTrack.BAL.BUSiteAssignmentBAL();
        objBUSite.BusinessUnitID = Convert.ToInt32(ddlBU.SelectedValue);
        //Commentd - 10-Nov-2006
        //objBUSite.SiteAccessID = Convert.ToInt32(ddlSiteAccess.SelectedValue);
        objBUSite.SiteAccessID = 0;

        string strSiteIds = "";

        for (int i = 0; i < lbAsSite.Items.Count; i++)
        {
            strSiteIds += lbAsSite.Items[i].Value + ";";
        }

        //if (strSiteIds == "")
        //{
        //    objCommon = new CommonBAL();
        //    lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
        //    lblMessage.Visible = true;
        //    return;
        //}


        objBUSite.SiteIDs = strSiteIds;
        objBUSite.Delimiters = ";";
        objBUSite.Status = 1;
        objBUSite.CreatedBy = Convert.ToInt32(Session["UserID"]);

        objBUSite.Persist(DALCOperation.Insert);
        //Commented - 11-Nov-06
        //clearFields();
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);

        lblMessage.Visible = true;
    }

    /// <summary>
    /// Used to clear form control value(s).
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        ddlBU.Focus();
        populate();
    }

    /// <summary>
    /// Used to call BU dropdown selection changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ddlBU_SelectedIndexChanged(object sender, EventArgs e)
    {
        populate();
    }
    #endregion

    #region "User Defined Methods"
    /// <summary>
    /// Populate Available and Assigned site information in the respective list box
    /// </summary>
    private void populate()
    {
        objBUSite = new iAssetTrack.BAL.BUSiteAssignmentBAL();

        objBUSite.BusinessUnitID = Convert.ToInt32(ddlBU.SelectedValue);
        //Commented - 10-Nov-2006  --Dummy value passed for Parameter
        //objBUSite.SiteAccessID = Convert.ToInt32(ddlSiteAccess.SelectedValue);
        objBUSite.SiteAccessID = 0;

        DataSet dsAvSite = objBUSite.retrieveAvailSites();
        lbAvSite.DataTextField = dsAvSite.Tables[0].Columns[1].ColumnName;
        lbAvSite.DataValueField = dsAvSite.Tables[0].Columns[0].ColumnName;
        lbAvSite.DataSource = dsAvSite.Tables[0].DefaultView;
        lbAvSite.DataBind();

        DataSet dsAsSite = objBUSite.retrieveAssignSite();
        lbAsSite.DataTextField = dsAsSite.Tables[0].Columns[1].ColumnName;
        lbAsSite.DataValueField = dsAsSite.Tables[0].Columns[0].ColumnName;
        lbAsSite.DataSource = dsAsSite.Tables[0].DefaultView;
        lbAsSite.DataBind();

        String strSitesWithLoc = "";

        foreach (DataRow dr in dsAsSite.Tables[0].Rows)
        {
            if (Convert.ToInt32(dr[2].ToString()) > 0)
            {
                strSitesWithLoc += dr[0].ToString() + ",";
            }
            //lbAsSite.Items.FindByValue(dr[0].ToString()).Enabled = (dr[2].ToString() == "0");
        }

        hdnSitesWithLocation.Value = strSitesWithLoc;

        /* Commented By Rajesh
        if (_dtRights.Select("Module = 'Business Unit and Site Assignment' and Rights = '" + "Modify" + "'").Length != 0)
        {
            ibAllLeft.Enabled = true;
            ibLeft.Enabled = true;
            ibAllRight.Enabled = true;
            ibRight.Enabled = true;
        }
        else
        {
            ibAllLeft.Enabled = false;
            ibLeft.Enabled = false;
            ibAllRight.Enabled = false;
            ibRight.Enabled = false;
        }
         */

    }

    /// <summary>
    /// Reset fields to default
    /// </summary>
    private void clearFields()
    {
        lblMessage.Visible = false;
        lblMessage.Text = "";
        //ddlBU.SelectedIndex = 0;// commented by kjb on 06 June 2012
        //ddlSiteAccess.SelectedIndex = 0;
        lbAsSite.Items.Clear();
        lbAvSite.Items.Clear();

        //if (lbAsSite.Items.Count == 0)
        //{
        //    ibLeft.Enabled = false;
        //    ibAllLeft.Enabled = false;
        //}
        //if (lbAvSite.Items.Count == 0)
        //{
        //    ibRight.Enabled = false;
        //    ibAllRight.Enabled = false;
        //}
    }
    #endregion
}