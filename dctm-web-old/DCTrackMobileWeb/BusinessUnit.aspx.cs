/*
File Name   :	BusinessUnit.aspx.cs

Description :	Used to create business unit

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
using iAssetTrack.DALC;
using System.Data.SqlClient;
using iAssetTrackBAL;

public partial class BusinessUnit : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    #endregion

    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdBU.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdBU_ItemCommand);
        pagerControl = grdBU.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        if (pagerControl != null)
            pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdBU.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdBU_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdBU.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdBU.Behaviors.Paging.PageIndex);
        }

        Control postbackControlInstance = null;
        string postbackControlName = this.Request.Params.Get("__EVENTTARGET");
        if (postbackControlName != null && postbackControlName != string.Empty)
            postbackControlInstance = this.FindControl(postbackControlName);
        if (postbackControlInstance != null && postbackControlInstance.ID == "PagerPageList")
        {
            //do nothing
        }
        else
        {
            pagerControl.SetupPageList(this.grdBU.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdBU.Behaviors.Paging.PageIndex);
        }


    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Root Entity";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "BusinessUnit.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Root Entity'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Root Entity' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Root Entity' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdBU.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        populateGrid();

        if (!IsPostBack)
        {

            Session["BusinessUnitID"] = null;

        }
    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.BU_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
        txtDesc.Attributes.Add("onkeypress", "doKeypress(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onpaste", "doPaste(this," + txtDesc.MaxLength.ToString() + ");");
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdBU_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdBU.Pag = e.NewPageIndex;
        populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdBU_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //populateGrid();
        //objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        //objBU.BusinessUnitID = Convert.ToInt32(grdBU.DataKeys[Convert.ToInt32(e.NewEditIndex)].Value);
        //DataSet dsBU = objBU.retrieve();
        //DataRow dr = dsBU.Tables[0].Rows[0];
        //txtBU.Text = dr[DBFields.DBFIELD_BUSINESSUNIT].ToString();
        //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //txtCoPrefix.Text = dr[DBFields.DBFIELD_COPREFIX].ToString();
        //Session["BusinessUnitID"] = objBU.BusinessUnitID;
    }

    /// <summary>
    /// Used to save information related BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        objBU.BusinessUnit = txtBU.Text;

        objBU.Description = txtDesc.Text;

        objBU.Status = 1;
        objBU.CreatedBy = Convert.ToInt32(Session["UserID"]);
        objBU.CoPrefix = txtCoPrefix.Text;

        int intBU = 0;
        objBU.BusinessUnitID = Session["BusinessUnitID"] == null ? intBU : (int)Session["BusinessUnitID"];
        intBU = objBU.exists();

        if (intBU != -1 && intBU != 0)
            objBU.BusinessUnitID = intBU;

        if (intBU != -1)
        {
            objBU.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["BusinessUnitID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }


            lblMessage.Visible = true;
            grdBU.ClearDataSource();
            populateGrid();
            Session["BusinessUnitID"] = null;
        }
        else
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);
            lblMessage.Visible = true;
            populateGrid();
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
        Session["BusinessUnitID"] = null;
        txtBU.Focus();
        //Added - 13-Nov-2006
        //Check to Show/Hide the delete button
        populateGrid();
    }

    /// <summary>
    /// Used to delete information related specific BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int BUId;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdBU.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[4].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                BUId = Convert.ToInt32(((Label)(grdViewRow.Items[4].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(BUId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        objBU.BusinessUnitIDs = strIDs;
        objBU.Status = 0;
        objBU.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objBU.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdBU.ClearDataSource();
        populateGrid();
    }
    #endregion

    #region "User Defined Methods"
    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void populateGrid()
    {
        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        grdBU.DataSource = objBU.retrieve().Tables[0];
        grdBU.DataBind();

        if (grdBU.Rows.Count == 0)
        {
            grdBU.DataSource = null;
            grdBU.DataBind();
            grdBU.Columns[4].Hidden = true;
            ibDelete.Visible = false;
        }
        else if(grdBU.Rows.Count > 0)
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Root Entity'").Length != 0)
            {
                grdBU.Columns[4].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdBU.Columns[4].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Root Entity'").Length != 0)
            {
                grdBU.Columns[3].Hidden = false;
            }
            else
            {
                grdBU.Columns[3].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int BUId;
            int iCount = 0;

            for (int i = 0; i < grdBU.Rows.Count; i++)
            {
                BUId = Convert.ToInt32(((Label)(grdBU.Rows[i].Items[4].FindControl("lblDeleteID"))).Text);
                grdBU.Rows[i].Items[4].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_BUSINESSUNITID, BUId.ToString(),0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdBU.Rows[i].Items[4].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdBU.Rows[i].Items[4].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdBU.Rows.Count)
            {
                grdBU.Columns[4].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdBU.Rows.Count > 0)
        {
            ibExportToExcel.Enabled = true;
            ibExportToExcel.Visible = true;

        }
        else
        {
            ibExportToExcel.Enabled = false;
            ibExportToExcel.Visible = false;
        }


    }


    private void applyRights()
    {
        
    }

    /// <summary>
    /// Reset fields
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void clearFields()
    {
        txtBU.Text = "";
        txtDesc.Text = "";
        txtCoPrefix.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }
    #endregion
    protected void grdBU_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid(); DataSet dsBU = objBU.retrieve();
            DataRow dr = dsBU.Tables[0].Rows[0];
            txtBU.Text = dr[DBFields.DBFIELD_BUSINESSUNIT].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            txtCoPrefix.Text = dr[DBFields.DBFIELD_COPREFIX].ToString();
            Session["BusinessUnitID"] = objBU.BusinessUnitID;
            objBU = new iAssetTrack.BAL.BusinessUnitBAL();
            objBU.BusinessUnitID = Convert.ToInt32(e.CommandArgument);
           
        }
    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdBU, wBook);
    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[3].Width = 1;
        e.Worksheet.Columns[4].Width = 1;
        if (iWSdex == 0)
        {
            if (iRdex == 0)
            {

                if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }
                if (iCdex == 4 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }

            }

        }
    }


    protected void grdBU_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {


        //int buID;
        //buID = Convert.ToInt32(((Label)(e.Row.Items[4].FindControl("lblDeleteID"))).Text);

        //objCommon = new iAssetTrack.BAL.CommonBAL();

        //DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_BUSINESSUNITID, buID);
        //foreach (DataTable tblCheck in dsCheck.Tables)
        //{
        //    if (tblCheck.Rows[0][0].ToString() != "0")
        //    {
        //        e.Row.Items[4].FindControl("chkDelete").Visible = false;
        //    }
        //}

        //if (ibDelete.Visible == true)
        //{
        //    int BUId;
        //    int iCount = 0;

        //    //for (int i = 0; i < grdBU.Rows.Count; i++)
        //    //{
        //        BUId = Convert.ToInt32(((Label)(e.Row.Items[4].FindControl("lblDeleteID"))).Text);

        //        objCommon = new iAssetTrack.BAL.CommonBAL();

        //        DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_BUSINESSUNITID, BUId);
        //        foreach (DataTable tblCheck in dsCheck.Tables)
        //        {
        //            if (tblCheck.Rows[0][0].ToString() != "0")
        //            {
        //                e.Row.Items[4].FindControl("chkDelete").Visible = false;
        //            }
        //        }
        //        //if (e.Row.Items[4].FindControl("chkDelete").Visible == false)
        //        //{
        //        //    iCount += 1;
        //        //}

        //    //}
        //    //if (iCount == grdBU.Rows.Count)
        //    //{
        //    //    grdBU.Columns[4].Hidden = true;
        //    //    ibDelete.Visible = false;
        //    //}
        //}
    }

}
