/*
File Name   :	ApplicationType.aspx.cs

Description :	Used to create Application Type

Date created:	04 Aug 2011

Modification History:
***********************
CR		Name			Date			Description
New		Nayana M	    04/08/2011		File has been created.
*/

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using Infragistics.Web.UI.GridControls;

public partial class ApplicationType : System.Web.UI.Page
{
    #region "Declarations"

    private ApplicationTypeBAL objAT;
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private const string PROP_FILTERCOUNT = "FilteredCount";
    #endregion
    public string FilterCount
    {
        get
        {
            return (ViewState[PROP_FILTERCOUNT] != null ? ViewState[PROP_FILTERCOUNT].ToString() : "");
        }
        set
        {
            ViewState[PROP_FILTERCOUNT] = value;
        }
    }
    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdApplType.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdApplType_ItemCommand);
        pagerControl = grdApplType.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Application Type";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ApplicationType.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Application Type' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Application Type' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Application Type' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdApplType.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["ApplicationTypeID"] = null;
        }
    }
    //Added on 19Apr2013
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.APPTYPE_JS_DELETE);
            hdnMessage.Value = sMessage;
        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objAT = new iAssetTrack.BAL.ApplicationTypeBAL();
        objAT.ApplType = txtApplType.Text.Trim();
        objAT.Description = txtDesc.Text.Trim();
        objAT.Status = 1;
        objAT.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intApplType = 0;
        objAT.ApplTypelID = Session["ApplicationTypeID"] == null ? intApplType : (int)Session["ApplicationTypeID"];
        intApplType = objAT.exists();

        if (intApplType != -1 && intApplType != 0)
            objAT.ApplTypelID = intApplType;

        if (intApplType != -1)
        {
            objAT.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["ApplicationTypeID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }
            
            Session["ApplicationTypeID"] = null;
            lblMessage.Visible = true;
            grdApplType.ClearDataSource();
            populateGrid();
        }
        else
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);
            lblMessage.Visible = true;
            populateGrid();
        }
    }

    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        txtApplType.Focus();
        Session["SiteID"] = null;
        populateGrid();
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int AssetTypeId;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdApplType.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                AssetTypeId = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(AssetTypeId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objAT = new iAssetTrack.BAL.ApplicationTypeBAL();
        objAT.ApplTypeIDs = strIDs;
        objAT.Status = 0;
        objAT.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objAT.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdApplType.ClearDataSource();
        populateGrid();
    }

    protected void grdApplType_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdApplType.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdApplType.Behaviors.Paging.PageIndex);
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
            //pagerControl.SetCurrentPageNumber(grdApplType.Behaviors.Paging.PageIndex);
            pagerControl.SetupPageList(this.grdApplType.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdApplType.Behaviors.Paging.PageIndex);
        }
    }

    protected void grdApplType_ItemCommand(object sender, HandleCommandEventArgs e)
    {
        //if (e.CommandName == "Edit")
        //{
        //    populateGrid();
        objAT = new iAssetTrack.BAL.ApplicationTypeBAL();
        //    objAT.ApplTypelID= Convert.ToInt32(e.CommandArgument);
        //    DataSet dsApplType = objAT.retrieve();
        //    DataRow dr = dsApplType.Tables[0].Rows[0];
        //    txtApplType.Text = dr[DBFields.DBFIELD_APPLTYPE].ToString();
        //    txtDesc.Text = dr[DBFields.DBFIELD_APPLTYPEDESC].ToString();
        //    Session["ApplicationTypeID"] = objAT.ApplTypelID;
        //}
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrack.BAL.ApplicationTypeBAL objEdit = new iAssetTrack.BAL.ApplicationTypeBAL();
            //iAssetTrack.BAL.SitesBAL objEdit = new iAssetTrack.BAL.SitesBAL();
          
            objEdit.ApplTypelID = Convert.ToInt32(e.CommandArgument);
            DataSet dsApplType = objEdit.retrieve();
            DataRow dr = dsApplType.Tables[0].Rows[0];
            txtApplType.Text = dr[DBFields.DBFIELD_APPLTYPE].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_APPLTYPEDESC].ToString();
            Session["ApplicationTypeID"] = objEdit.ApplTypelID;

            if (_dtRights.Select("Module = 'Application Type' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
        else if (e.CommandName == "Page")
        {
            pagerControl.SetCurrentPageNumber(grdApplType.Behaviors.Paging.PageIndex);
        }
    }

    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[2].Width = 1;
        e.Worksheet.Columns[3].Width = 1;
        if (iRdex == 0)
        {
            if (iCdex == 2 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
            {
                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
            }
            if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
            {
                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
            }
        }
    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdApplType, wBook);
    }

    #endregion

    #region "User Defined Methods"

    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdApplType.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }

    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    private void populateGrid()
    {

        //grdApplType.Rows.Clear();
        //objAT = new iAssetTrack.BAL.ApplicationTypeBAL();

        //grdApplType.DataSource = objAT.retrieve().Tables[0];
        //grdApplType.DataBind();

        objAT = new iAssetTrack.BAL.ApplicationTypeBAL();
        DataTable dtGrid = objAT.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        grdApplType.DataSource = dtGrid;
        grdApplType.DataBind();

        grdApplType.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdApplType.Rows.Count)
            this.FilterCount = "";
        grdApplType.Behaviors.Paging.Enabled = true;

        if (grdApplType.Rows.Count == 0)
        {
            grdApplType.DataSource = dtGrid;
            grdApplType.DataBind();
            grdApplType.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Application Type'").Length != 0)
            {
                grdApplType.Columns[3].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdApplType.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Application Type'").Length != 0)
            {
                grdApplType.Columns[2].Hidden = false;
            }
            else
            {
                grdApplType.Columns[2].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int ApplTypeID;
            int iCount = 0;

            for (int i = 0; i < grdApplType.Rows.Count; i++)
            {
                ApplTypeID = Convert.ToInt32(((Label)(grdApplType.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);

                objCommon = new iAssetTrack.BAL.CommonBAL();

                //TODO: Check the DBFields.DBFIELD_APPLTYPEID
                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_APPLTYPEID, ApplTypeID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdApplType.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdApplType.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdApplType.Rows.Count)
            {
                grdApplType.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdApplType.Rows.Count > 0)
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
    protected void grdApplType_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdApplType.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdApplType.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdApplType.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdApplType.Behaviors.Paging.Enabled = true;
    }
    /// <summary>
    /// Reset fields
    /// </summary>    
    private void clearFields()
    {
        txtApplType.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }


    #endregion
}
