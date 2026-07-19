/*
File Name   :	BUDivAssignment.aspx.cs

Description :	Used to assign Division(s) for business unit

Date created:	05 Aug 2011

Modification History:
***********************
CR		Name			Date			Description
New		Nayana M	05/08/2011		File has been created.
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


public partial class BUDivAssignment : System.Web.UI.Page
{

    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.BUDivAssignmentBAL objBUDiv;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;

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

    #endregion

    #region "Page Event Methods"
    /// <summary>
    /// Used to load the page.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = SiteMap.CurrentNode.Title;
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "BUDivAssignment.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Company and Division Assignment' and Rights = '" + "View" + "'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Company and Division Assignment' and Rights = '" + "Assignment" + "'").Length != 0)
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

        if (!IsPostBack)
        {
            //PageControlAccess.CheckPageControlAccess(this, saModuleRightsControlList);
            objBU = new iAssetTrack.BAL.BusinessUnitBAL();
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
    /// Used to assign Divisions.
    /// </summary>
    protected void ibRight_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        int iCurrentIndex;

        if (lbAvDivision.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        int iSelected = 0;

        for (int i = 0; i < lbAvDivision.Items.Count; i++)
        {
            if (lbAvDivision.Items[i].Selected)
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

        for (int i = 0; i < lbAvDivision.Items.Count; i++)
        {
            if (lbAvDivision.Items[i].Selected)
            {
                iCurrentIndex = lbAsDivision.Items.Count;
                ListItem liAsDivision = new ListItem(lbAvDivision.Items[i].Text, lbAvDivision.Items[i].Value);
                lbAsDivision.Items.Insert(iCurrentIndex, liAsDivision);
            }
        }

        for (int i = lbAvDivision.Items.Count - 1; i >= 0; i--)
        {
            if (lbAvDivision.Items[i].Selected)
            {
                lbAvDivision.Items.RemoveAt(i);
            }
        }

        for (int i = 0; i < lbAvDivision.Items.Count; i++)
        {
            if (lbAvDivision.Items[i].Selected)
            {
                lbAvDivision.Items[i].Selected = false;
            }
        }


        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            if (lbAsDivision.Items[i].Selected)
            {
                lbAsDivision.Items[i].Selected = false;
            }
        }
    }

    /// <summary>
    /// Used to de-assign Divisions.
    /// </summary>
    protected void ibLeft_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        String csvDivisionsWithAppl = hdnDivisionsWithAppl.Value;
        String[] arrDivisionsWithAppl = csvDivisionsWithAppl.Split(',');


        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            if (lbAsDivision.Items[i].Selected)
            {
                foreach (String divID in arrDivisionsWithAppl)
                {
                    if (lbAsDivision.Items[i].Value.Trim() == divID)
                    {
                        lblMessage.Text = "Divisions cannot be Un-Assigned from Company as Custodian's mapped to Division";
                        lblMessage.Visible = true;
                        return;
                    }
                }
            }
        }

        int iCurrentIndex;
        if (lbAsDivision.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        int iSelected = 0;
        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            if (lbAsDivision.Items[i].Selected)
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

        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            if (lbAsDivision.Items[i].Selected)
            {
                iCurrentIndex = lbAvDivision.Items.Count;
                ListItem liAvDivision = new ListItem(lbAsDivision.Items[i].Text, lbAsDivision.Items[i].Value);
                lbAvDivision.Items.Insert(iCurrentIndex, liAvDivision);
            }
        }

        for (int i = lbAsDivision.Items.Count - 1; i >= 0; i--)
        {
            if (lbAsDivision.Items[i].Selected)
            {
                lbAsDivision.Items.RemoveAt(i);

            }
        }

        for (int i = 0; i < lbAvDivision.Items.Count; i++)
        {
            if (lbAvDivision.Items[i].Selected)
            {
                lbAvDivision.Items[i].Selected = false;
            }
        }

        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            if (lbAsDivision.Items[i].Selected)
            {
                lbAsDivision.Items[i].Selected = false;
            }
        }

    }

    /// <summary>
    /// Used to assign All Divisions.
    /// </summary>
    protected void ibAllRight_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (lbAvDivision.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }
        int iCurrentIndex;
        for (int i = 0; i < lbAvDivision.Items.Count; i++)
        {
            iCurrentIndex = lbAsDivision.Items.Count;
            ListItem liAsDivision = new ListItem(lbAvDivision.Items[i].Text, lbAvDivision.Items[i].Value);
            lbAsDivision.Items.Insert(iCurrentIndex, liAsDivision);
        }

        lbAvDivision.Items.Clear();

        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            if (lbAsDivision.Items[i].Selected)
            {
                lbAsDivision.Items[i].Selected = false;
            }
        }
    }

    /// <summary>
    /// Used to de-assign All Divisions.
    /// </summary>
    protected void ibAllLeft_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        String csvDivisionsWithAppl = hdnDivisionsWithAppl.Value;
        String[] arrDivisionsWithAppl = csvDivisionsWithAppl.Split(',');
        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            if (lbAsDivision.Items[i].Selected)
            {
                foreach (String siteID in arrDivisionsWithAppl)
                {
                    if (lbAsDivision.Items[i].Value.Trim() == siteID)
                    {
                        lblMessage.Text = "Divisions cannot be Un-Assigned from Company as it Custodian's mapped to Division";
                        lblMessage.Visible = true;
                        return;
                    }
                }
            }
        }

        int iCurrentIndex;
        if (lbAsDivision.Items.Count == 0)
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_LIST);
            lblMessage.Visible = true;
            return;
        }

        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            iCurrentIndex = lbAvDivision.Items.Count;
            ListItem liAvDivision = new ListItem(lbAsDivision.Items[i].Text, lbAsDivision.Items[i].Value);
            lbAvDivision.Items.Insert(iCurrentIndex, liAvDivision);

        }

        lbAsDivision.Items.Clear();

        for (int i = 0; i < lbAvDivision.Items.Count; i++)
        {
            if (lbAvDivision.Items[i].Selected)
            {
                lbAvDivision.Items[i].Selected = false;
            }
        }
    }

    /// <summary>
    /// Used to assign Divisions for specific BU.
    /// </summary>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objBUDiv = new iAssetTrack.BAL.BUDivAssignmentBAL();
        objBUDiv.BusinessUnitID = Convert.ToInt32(ddlBU.SelectedValue);
        objBUDiv.DivAccessID = 0;

        string strDivIds = "";

        for (int i = 0; i < lbAsDivision.Items.Count; i++)
        {
            strDivIds += lbAsDivision.Items[i].Value + ";";
        }

        objBUDiv.DivisionIDs = strDivIds;
        objBUDiv.Delimiters = ";";
        objBUDiv.Status = 1;
        objBUDiv.CreatedBy = Convert.ToInt32(Session["UserID"]);

        objBUDiv.Persist(DALCOperation.Insert);
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);

        lblMessage.Visible = true;
    }

    /// <summary>
    /// Used to clear form control value(s).
    /// </summary>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        ddlBU.Focus();
        populate();
    }

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
        objBUDiv = new iAssetTrack.BAL.BUDivAssignmentBAL();

        objBUDiv.BusinessUnitID = Convert.ToInt32(ddlBU.SelectedValue);
        objBUDiv.DivAccessID = 0;

        DataSet dsAvSite = objBUDiv.retrieveAvailDivisions();
        lbAvDivision.DataTextField = dsAvSite.Tables[0].Columns[1].ColumnName;
        lbAvDivision.DataValueField = dsAvSite.Tables[0].Columns[0].ColumnName;
        lbAvDivision.DataSource = dsAvSite.Tables[0].DefaultView;
        lbAvDivision.DataBind();

        DataSet dsAsDivision = objBUDiv.retrieveAssignDivision();
        lbAsDivision.DataTextField = dsAsDivision.Tables[0].Columns[1].ColumnName;
        lbAsDivision.DataValueField = dsAsDivision.Tables[0].Columns[0].ColumnName;
        lbAsDivision.DataSource = dsAsDivision.Tables[0].DefaultView;
        lbAsDivision.DataBind();

        String strDivisionsWithAppl = "";

        foreach (DataRow dr in dsAsDivision.Tables[0].Rows)
        {
            if (Convert.ToInt32(dr[2].ToString()) > 0)
            {
                strDivisionsWithAppl += dr[0].ToString() + ",";
            }
        }

        hdnDivisionsWithAppl.Value = strDivisionsWithAppl;
    }


    /// <summary>
    /// Reset fields to default
    /// </summary>
    private void clearFields()
    {
        lblMessage.Visible = false;
        lblMessage.Text = "";
        //ddlBU.SelectedIndex = 0; //commented by kjb on 06 June 2012
        lbAsDivision.Items.Clear();
        lbAvDivision.Items.Clear();

        //if (lbAsDivision.Items.Count == 0)
        //{
        //    ibLeft.Enabled = false;
        //    ibAllLeft.Enabled = false;
        //}
        //if (lbAvDivision.Items.Count == 0)
        //{
        //    ibRight.Enabled = false;
        //    ibAllRight.Enabled = false;
        //}
    }
    #endregion

}
