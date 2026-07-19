/*
File Name   :	Host.aspx.cs

Description :	Used to create business unit

Date created:	01 Sep 2011

Modification History:
***********************
CR		Name			    Date			Description
New		Jagadeesh Babu K	01/09/2011		File has been created.
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
using Infragistics.Web.UI.GridControls;

public partial class GlobalParameters : System.Web.UI.Page
{
    #region "Declarations"
   
    private iAssetTrackBAL.GlobalParametersBAL objGlobalParameters;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    #endregion

    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdGlobalParameters.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdGlobalParameters_ItemCommand);
        pagerControl = grdGlobalParameters.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdGlobalParameters.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdGlobalParameters_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdGlobalParameters.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdGlobalParameters.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdGlobalParameters.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdGlobalParameters.Behaviors.Paging.PageIndex);

        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Global Parameters";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "GlobalParameters.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Global Parameters'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (_dtRights.Select("Module = 'Global Parameters' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }
        if (_dtRights.Select("Module = 'Global Parameters' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'GlobalParameters' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdGlobalParameters.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        populateGrid();

        if (!IsPostBack)
        {

            Session["Sno"] = null;
            populateUOM();
            populatePerUOM();
            populateMeasure();
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
            string sMessage = objCommon.displayMessage(MessageCodes.GlobalParams_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
        txtDesc.Attributes.Add("onkeypress", "doKeypress(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onpaste", "doPaste(this," + txtDesc.MaxLength.ToString() + ");");
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
   
    protected void grdGlobalParameters_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdGlobalParameters.PageIndex = e.NewPageIndex;
        populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
   
    protected void grdGlobalParameters_RowEditing(object sender, GridViewEditEventArgs e)
    {
       
    }

    /// <summary>
    /// Used to save information related BU.
    /// </summary>
    
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
       

        objGlobalParameters = new GlobalParametersBAL();

        objGlobalParameters.SPCVariable = txtSPCVariable.Text;
        objGlobalParameters.SPCValue = txtSPCValue.Text;
        objGlobalParameters.UOMID = Convert.ToInt16(ddlUOMID.SelectedValue);
        objGlobalParameters.PerUOMID = Convert.ToInt16(ddlPerUOMID.SelectedValue);
        objGlobalParameters.MeasureID = Convert.ToInt16(ddlMeasureID.SelectedValue);

       
        objGlobalParameters.Comment = txtDesc.Text;
        objGlobalParameters.Status = 1;
        objGlobalParameters.CreatedBy = Convert.ToInt32(Session["UserID"]);

        string strGL = string.Empty;
        objGlobalParameters.SNo = Session["SNo"] == null ? strGL : (string)Session["SNo"];
        strGL = objGlobalParameters.exists().ToString();
     


        if (strGL != "-1" && strGL != "0")
            objGlobalParameters.SNo = strGL;
        if (strGL != "-1")
        {
            objGlobalParameters.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["SNo"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }

            Session["SNo"] = null;
            lblMessage.Visible = true;
            grdGlobalParameters.ClearDataSource();
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
    /// <summary>
    /// Used to clear form control value(s).
    /// </summary>
    /// <author>Jagadeesh</author>
    /// <createdOn>01 Sep 2011</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        Session["SNo"] = null;
      
        populateGrid();
    }

    /// <summary>
    /// Used to delete information related specific GL.
    /// </summary>
   
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        string hostID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdGlobalParameters.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[7].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                hostID = ((Label)(grdViewRow.Items[7].FindControl("lblDeleteID"))).Text;
                strIDs += "'" + hostID + "',";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

         objGlobalParameters = new GlobalParametersBAL();
        objGlobalParameters.SNos = strIDs;
        objGlobalParameters.Status = 0;
        objGlobalParameters.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objGlobalParameters.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdGlobalParameters.ClearDataSource();
        populateGrid();
    }
    #endregion

    #region "User Defined Methods"
   

    private void populateGrid()
    {
        //objHost = new HostBAL();
        //grdHost.DataSource = objHost.retrieve().Tables[0];
        //grdHost.DataBind();
        //objHost = new HostBAL();
        objGlobalParameters = new GlobalParametersBAL();
        DataTable dtGrid = objGlobalParameters.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;


        if (dtGrid.Rows.Count == 0)
        {
            //grdHost.DataSource = null;
            grdGlobalParameters.DataSource = dtGrid;
            grdGlobalParameters.DataBind();
            grdGlobalParameters.Columns[7].Hidden = true;
            ibDelete.Visible = false;
        }
        else// if (grdHost.Rows.Count > 0)
        {
            grdGlobalParameters.DataSource = dtGrid;
            grdGlobalParameters.DataBind();
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Host'").Length != 0)
            {
                grdGlobalParameters.Columns[7].Hidden = false;
                ibDelete.Visible = true;

            }
            else
            {
                grdGlobalParameters.Columns[7].Hidden = true;
                ibDelete.Visible = false;
            }
            if (_dtRights.Select("Rights = 'Modify' and Module = 'Host'").Length != 0)
            {
                grdGlobalParameters.Columns[2].Hidden = false;
            }
            else
            {
                grdGlobalParameters.Columns[2].Hidden = true;
            }




            if (ibDelete.Visible == true)
            {
                string HostId;
                int iCount = 0;

                for (int i = 0; i < grdGlobalParameters.Rows.Count; i++)
                {
                    //HostId = Convert.ToInt32(((Label)(grdHost.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);
                    HostId = ((Label)(grdGlobalParameters.Rows[i].Items[7].FindControl("lblDeleteID"))).Text;
                    grdGlobalParameters.Rows[i].Items[7].FindControl("chkDelete").Visible = true;
                    objCommon = new iAssetTrack.BAL.CommonBAL();

                    DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_SNo, HostId, 1);
                    foreach (DataTable tblCheck in dsCheck.Tables)
                    {
                        if (tblCheck.Rows[0][0].ToString() != "0")
                        {
                            grdGlobalParameters.Rows[i].Items[7].FindControl("chkDelete").Visible = false;
                        }
                    }
                    if (grdGlobalParameters.Rows[i].Items[7].FindControl("chkDelete").Visible == false)
                    {
                        iCount += 1;
                    }

                }

                if (iCount == grdGlobalParameters.Rows.Count)
                {
                    grdGlobalParameters.Columns[7].Hidden = true;
                    ibDelete.Visible = false;
                }
            }
            if (grdGlobalParameters.Rows.Count > 0)
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
    }
    private void populateUOM()
    {
        objGlobalParameters = new GlobalParametersBAL();
        DataSet dsUOM = objGlobalParameters.retrieveUOMPerUOM();
        DataTable dtUOM = dsUOM.Tables[0];

        objCommon = new CommonBAL();
        objCommon.setDataSourceInfra(ddlUOMID, dtUOM, "-Select-");
    }

    private void populatePerUOM()
    {
        objGlobalParameters = new GlobalParametersBAL();
        DataSet dsPerUOM = objGlobalParameters.retrieveUOMPerUOM();
        DataTable dtPerUOM = dsPerUOM.Tables[1];

        objCommon = new CommonBAL();
        objCommon.setDataSourceInfra(ddlPerUOMID, dtPerUOM, "-Select-");
    }
    private void populateMeasure()
    {
        objGlobalParameters = new GlobalParametersBAL();
        DataSet dsMeasure = objGlobalParameters.retrieveMeasure();
        DataTable dtMeasure = dsMeasure.Tables[0];

        objCommon = new CommonBAL();
        objCommon.setDataSourceInfra(ddlMeasureID, dtMeasure, "-Select-");
    }


    /// <summary>
    /// Reset fields
    /// </summary>    
   
    private void clearFields()
    {
        txtSPCVariable.Text = "";
        txtSPCValue.Text = "";
        ddlUOMID.SelectedIndex = -1;
        ddlPerUOMID.SelectedIndex = -1;
        ddlMeasureID.SelectedIndex = -1;
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }
    #endregion
    protected void grdGlobalParameters_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
       
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrackBAL.GlobalParametersBAL objEdit = new iAssetTrackBAL.GlobalParametersBAL();
            objEdit.SNo = Convert.ToString(e.CommandArgument);
            DataSet dsGL = objEdit.retrieve();
            DataRow dr = dsGL.Tables[0].Rows[0];
            txtSPCVariable.Text = dr[DBFields.DBFIELD_SPCVariable].ToString();
            txtSPCValue.Text = dr[DBFields.DBFIELD_SPCValue].ToString();
            ddlUOMID.SelectedValue = dr[DBFields.DBFIELD_UOMID].ToString();
            ddlPerUOMID.SelectedValue = dr[DBFields.DBFIELD_PerUOMID].ToString();
            ddlMeasureID.SelectedValue = dr[DBFields.DBFIELD_MeasureID].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_Comment].ToString();
            Session["SNo"] = objEdit.SNo;
            if (_dtRights.Select("Module = 'Host' and Rights = '" + "Modify" + "'").Length != 0)
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
            pagerControl.SetCurrentPageNumber(grdGlobalParameters.Behaviors.Paging.PageIndex);
        }

    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdGlobalParameters, wBook);
    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        //e.Worksheet.Columns[2].Width = 1;
        e.Worksheet.Columns[7].Width = 1;
        e.Worksheet.Columns[6].Width = 1;
     
        if (iWSdex == 0)
        {
            if (iRdex == 0)
            {

                if (iCdex == 6 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }
                if (iCdex == 7 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


                }

            }

        }
    }
    protected void grdGlobalParameters_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdGlobalParameters.DataSource as DataTable;

        // Disable WebDataGrid Paging
        grdGlobalParameters.Behaviors.Paging.Enabled = false;

        hdnFilterCount.Value = grdGlobalParameters.Rows.Count.ToString();


        // Enable WebDataGrid Paging
        grdGlobalParameters.Behaviors.Paging.Enabled = true;
    }

    protected void grdGlobalParameters_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {
        
    }
}
