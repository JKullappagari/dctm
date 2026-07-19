/*
File Name   :	DeviceReg.aspx.cs

Description :	Used to create Device in the DB

Date created:	05 May 2012

Modification History:
***********************
CR		Name			    Date			Description
New		Jagadeesh Babu K	05/05/2012	File has been created.
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
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.DeviceBAL objDev;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
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

    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdDevice.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdDevice_ItemCommand);
        pagerControl = grdDevice.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdDevice.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdDevice_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdDevice.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdDevice.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdDevice.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdDevice.Behaviors.Paging.PageIndex);
        }


    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Device Registration";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "DeviceReg.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Register Device' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Register Device' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Register Device' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdDevice.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        populateGrid();

        if (!IsPostBack)
        {
            Session["devID"] = null;
            //Load Sites
            populateSite();
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
            string sMessage = objCommon.displayMessage(MessageCodes.DEV_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    protected void grdDevice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdDevice.Pag = e.NewPageIndex;
        populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    protected void grdDevice_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    /// <summary>
    /// Used to save information related Device.
    /// </summary>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objDev = new iAssetTrack.BAL.DeviceBAL();
        int intID = 0;
        objDev.DeviceID = txtDeviceID.Text.Trim();
        objDev.DeviceName = txtDeviceName.Text.Trim();
        objDev.ID = Session["devID"] == null ? intID : (int)Session["devID"];
        intID = objDev.exists();

        if (intID != -1 && intID != 0)
            objDev.ID = intID;
        objDev.DeviceID = txtDeviceID.Text;
        objDev.DeviceName = txtDeviceName.Text;
        objDev.CreatedBy = Convert.ToInt32(Session["UserID"]);
        objDev.SiteID = Convert.ToInt32(ddlSite.SelectedValue);
        objDev.Status = (rdoActiveInt.Checked ? 1 : 0);

        if (intID != -1)
        {
            objDev.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["devID"] == null)
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
            grdDevice.ClearDataSource();
            populateGrid();
            Session["devID"] = null;
        }
        else
        {
            objCommon = new CommonBAL();
           // lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);//commented on 28-feb-2013
            lblMessage.Text = "Device ID or DeviceName is already in use";//Added on 28-feb-2013
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
        Session["devID"] = null;
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
        int id;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdDevice.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[5].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                id = Convert.ToInt32(((Label)(grdViewRow.Items[5].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(id) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objDev = new iAssetTrack.BAL.DeviceBAL();
        objDev.IDs = strIDs;
        objDev.Status = 0;
        objDev.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objDev.Persist(DALCOperation.Delete);


        clearFields();
        objCommon = new CommonBAL();
        //lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED); 
        // commented and added below line by kjb on 16 may 2012(Now it will show disabled message instead of deleted)
        lblMessage.Text = "Disabled successfully";
        lblMessage.Visible = true;
        grdDevice.ClearDataSource();
        populateGrid();
    }
    #endregion

    #region "User Defined Methods"
    /// <summary>
    /// Populate Device data grid
    /// </summary>    
    private void populateGrid()
    {
        //objDev = new iAssetTrack.BAL.DeviceBAL();
        //grdDevice.DataSource = objDev.retrieve().Tables[0];
        //grdDevice.DataBind();
        objDev = new iAssetTrack.BAL.DeviceBAL();
        DataTable dtGrid = objDev.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        grdDevice.DataSource = dtGrid;
        grdDevice.DataBind();

        grdDevice.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdDevice.Rows.Count)
            this.FilterCount = "";
        grdDevice.Behaviors.Paging.Enabled = true;

        if (grdDevice.Rows.Count == 0)
        {
            grdDevice.DataSource = dtGrid;
            grdDevice.DataBind();
            grdDevice.Columns[5].Hidden = true;
            ibDelete.Visible = false;
        }
        else if (grdDevice.Rows.Count > 0)
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Register Device'").Length != 0)
            {
                grdDevice.Columns[5].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdDevice.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Register Device'").Length != 0)
            {
                grdDevice.Columns[4].Hidden = false;
            }
            else
            {
                grdDevice.Columns[4].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int id;
            int iCount = 0;

            for (int i = 0; i < grdDevice.Rows.Count; i++)
            {
                id = Convert.ToInt32(((Label)(grdDevice.Rows[i].Items[5].FindControl("lblDeleteID"))).Text);
                grdDevice.Rows[i].Items[5].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();
                //Special case
                // don't show check box for devices which are alrwady disabled
                if (grdDevice.Rows[i].Items[3].Text.CompareTo("Disabled") == 0)
                {
                    grdDevice.Rows[i].Items[5].FindControl("chkDelete").Visible = false;
                }

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_ID, id.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdDevice.Rows[i].Items[5].FindControl("chkDelete").Visible = false;
                    }
                }

                if (grdDevice.Rows[i].Items[5].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdDevice.Rows.Count)
            {
                grdDevice.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdDevice.Rows.Count > 0)
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
    private void clearFields()
    {
        txtDeviceID.Text = "";
        txtDeviceName.Text = "";
        ddlSite.SelectedIndex = 0;
        lblMessage.Visible = false;
        lblMessage.Text = "";
        txtDeviceID.Enabled = true;
        txtDeviceName.Enabled = true;
        txtDeviceID.Focus();
        rdoDisabledInt.Checked = false;
        rdoActiveInt.Checked = true;
    }
    #endregion
    protected void grdDevice_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objDev.ID = Convert.ToInt32(e.CommandArgument);
            DataSet dsDev = objDev.retrieve();
            DataRow dr = dsDev.Tables[0].Rows[0];
            txtDeviceID.Text = dr[DBFields.DBFIELD_DEVICEID].ToString();
            txtDeviceName.Text = dr[DBFields.DBFIELD_DEVICENAME].ToString();
            ddlSite.SelectedValue = dr[DBFields.DBFIELD_SITEID].ToString();
            Session["devID"] = objDev.ID;
            txtDeviceID.Enabled = false;
            txtDeviceName.Enabled = false;
            if (Convert.ToBoolean(dr[DBFields.DBFIELD_STATUS].ToString()))
            {
                rdoActiveInt.Checked = true;
            }
            else
            {
                rdoDisabledInt.Checked = true;
            }
            //objBU = new iAssetTrack.BAL.DeviceBAL();
            //objBU.BusinessUnitID = Convert.ToInt32(e.CommandArgument);


            if (_dtRights.Select("Module = 'Register Device' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdDevice, wBook);
    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[4].Width = 1;
        e.Worksheet.Columns[5].Width = 1;
        if (iWSdex == 0)
        {
            if (iRdex == 0)
            {

                if (iCdex == 4 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }
                if (iCdex == 5 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }

            }

        }
    }


    private void populateSite()
    {
        objSite = new iAssetTrack.BAL.SitesBAL();
        DataSet dsSite = objSite.retrieve();
        DataTable dtSite = dsSite.Tables[0];
        objCommon = new CommonBAL();
        objCommon.setDataSource(ddlSite, dtSite, "-Select-");
    }

    protected void grdDevice_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {

    }
    protected void grdDevice_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdDevice.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdDevice.Behaviors.Paging.Enabled = false;

        this.FilterCount = grdDevice.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdDevice.Behaviors.Paging.Enabled = true;
    }

}
