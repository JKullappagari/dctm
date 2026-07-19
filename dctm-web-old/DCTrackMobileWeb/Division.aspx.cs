/*
File Name   :	Division.aspx.cs

Description :	Used to create Division

Date created:	05 Aug 2011

Modification History:
***********************
CR		Name			Date			Description
New		Nayana M	    05/08/2011		File has been created.
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

public partial class Division : System.Web.UI.Page
{
    #region "Declarations"

    private iAssetTrack.BAL.DivisionBAL objDiv;
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private const string PROP_FILTERCOUNT = "FilteredCount";

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
    #endregion

    private string tenantAssignedDivisions = string.Empty;
    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdDivision.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdDivision_ItemCommand);
        pagerControl = grdDivision.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    //Added on 19Apr2013
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Division";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Division.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Division' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Division' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Division' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdDivision.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["DivisionID"] = null;
        }
    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (ibDelete.Enabled)
        {
            iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
            string sMessage = objCommon.displayMessage(MessageCodes.Division_JS_DELETE);
            hdnMessage.Value = sMessage;
        }

        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());

    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objDiv = new iAssetTrack.BAL.DivisionBAL();
        objDiv.Division = txtDivision.Text;
        objDiv.Description = txtDesc.Text;
        objDiv.Status = 1;
        objDiv.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intDivision = 0;
        objDiv.DivisionID = Session["DivisionID"] == null ? intDivision : (int)Session["DivisionID"];
        intDivision = objDiv.exists();

        if (intDivision != -1 && intDivision != 0)
            objDiv.DivisionID = intDivision;

        if (intDivision != -1)
        {
            objDiv.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["DivisionID"] == null)
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
            grdDivision.ClearDataSource();
            populateGrid();
            Session["DivisionID"] = null;
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
        Session["DivisionID"] = null;
        txtDivision.Focus();
        populateGrid();
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int DivisionID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdDivision.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                DivisionID = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(DivisionID) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objDiv = new iAssetTrack.BAL.DivisionBAL();
        objDiv.DivisionIDs = strIDs;
        objDiv.Status = 0;
        objDiv.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objDiv.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdDivision.ClearDataSource();
        populateGrid();
    }


    protected void grdDivision_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdDivision.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdDivision.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdDivision.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdDivision.Behaviors.Paging.PageIndex);
        }
        //pagerControl.SetCurrentPageNumber(grdDivision.Behaviors.Paging.PageIndex);
    }

    protected void grdDivision_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objDiv = new iAssetTrack.BAL.DivisionBAL();
            objDiv.DivisionID = Convert.ToInt32(e.CommandArgument);
            DataSet dsDiv = objDiv.retrieve();
            DataRow dr = dsDiv.Tables[0].Rows[0];
            txtDivision.Text = dr[DBFields.DBFIELD_DIVISION].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DIVISIONDESC].ToString();
            Session["DivisionID"] = objDiv.DivisionID;

            if (_dtRights.Select("Module = 'Division' and Rights = '" + "Modify" + "'").Length != 0)
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
            pagerControl.SetCurrentPageNumber(grdDivision.Behaviors.Paging.PageIndex);
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

    protected void ibExportToExcel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdDivision, wBook);
    }


    #endregion

    #region "User Defined Methods"

    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdDivision.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }

    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    private void populateGrid()
    {
        objDiv = new iAssetTrack.BAL.DivisionBAL();
        DataTable dtGrid = objDiv.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            tenantAssignedDivisions = string.Empty;
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                DataSet dsTD = objTenant.retrieveDivisionAssignmentList();
                if (dsTD.Tables.Count > 0 && dsTD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsTD.Tables[0].Rows)
                    {
                        tenantAssignedDivisions = tenantAssignedDivisions + dr[DBFields.DBFIELD_DIVISIONID].ToString() + ",";
                    }
                    tenantAssignedDivisions = tenantAssignedDivisions.Trim(',');

                }
                if (!string.IsNullOrEmpty(tenantAssignedDivisions))
                {
                    DataTable tempTable = dtGrid.Clone();
                    foreach (DataRow dr in dtGrid.Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("DivisionID IN (" + tenantAssignedDivisions + ")");
                    dtGrid.Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow dr in filteredRows)
                        {
                            dtGrid.Rows.Add(dr.ItemArray);
                        }

                    }
                }
                else
                {
                    dtGrid.Rows.Clear();
                }
            
            }

        }

        totalRecordCount = dtGrid.Rows.Count;
        grdDivision.DataSource = dtGrid;
        grdDivision.DataBind();

        grdDivision.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdDivision.Rows.Count)
            this.FilterCount = "";
        grdDivision.Behaviors.Paging.Enabled = true;

        if (grdDivision.Rows.Count == 0)
        {
            grdDivision.DataSource = dtGrid;
            grdDivision.DataBind();
            grdDivision.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Division'").Length != 0)
            {
                grdDivision.Columns[3].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdDivision.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Division'").Length != 0)
            {
                grdDivision.Columns[2].Hidden = false;
            }
            else
            {
                grdDivision.Columns[2].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int DivisionID;
            int iCount = 0;

            for (int i = 0; i < grdDivision.Rows.Count; i++)
            {
                DivisionID = Convert.ToInt32(((Label)(grdDivision.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);

                objCommon = new iAssetTrack.BAL.CommonBAL();

                //TODO: Check the DBFields.DBFIELD_APPLDIVISIONID
                //DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_APPLDIVISIONID, DivisionID.ToString(), 0);
                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_DIVISIONID, DivisionID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdDivision.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdDivision.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdDivision.Rows.Count)
            {
                grdDivision.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdDivision.Rows.Count > 0)
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
    protected void grdDivision_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdDivision.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdDivision.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdDivision.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdDivision.Behaviors.Paging.Enabled = true;
    }

    /// <summary>
    /// Reset fields
    /// </summary>    
    private void clearFields()
    {
        txtDivision.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }


    #endregion
}
