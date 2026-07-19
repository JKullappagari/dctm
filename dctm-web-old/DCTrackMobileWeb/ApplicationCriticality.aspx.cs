/*
File Name   :	ApplicationCriticality.aspx.cs

Description :	Used to create Application Criticality

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
using System.Drawing;
using Microsoft.Security.Application;


public partial class ApplicationCriticality : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.ApplicationCriticalityBAL objAC;
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
        grdApplCriticality.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdApplCriticality_ItemCommand);
        pagerControl = grdApplCriticality.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Application Criticality";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ApplicationCriticality.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Application Criticality' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }


        if (_dtRights.Select("Module = 'Application Criticality' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Application Criticality' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }


        this.grdApplCriticality.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["ApplCriticalityID"] = null;
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
            string sMessage = objCommon.displayMessage(MessageCodes.APPCRITI_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }
    //Added on 19Apr2013
    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();
        objAC.ApplCriticality = txtApplCriticality.Text.Trim();
        objAC.Description = txtDesc.Text.Trim();
        objAC.Status = 1;
        objAC.CreatedBy = Convert.ToInt32(Session["UserID"]);
        //-->v3.9
        objAC.BackColorCode = Request.Form["backColorVal"].ToString();
        objAC.ForeColorCode = GetForeColor(objAC.BackColorCode);


        int intApplCriticality = 0;
        objAC.ApplCriticalityID = Session["ApplCriticalityID"] == null ? intApplCriticality : (int)Session["ApplCriticalityID"];
        intApplCriticality = objAC.exists();

        if (intApplCriticality != -1 && intApplCriticality != 0)
            objAC.ApplCriticalityID = intApplCriticality;

        if (intApplCriticality != -1)
        {
            objAC.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["ApplCriticalityID"] == null)
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
            grdApplCriticality.ClearDataSource();
            populateGrid();
            Session["ApplCriticalityID"] = null;
        }
        else
        {
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);
            lblMessage.Visible = true;
            populateGrid();
        }
    }

    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int ApplCriticalityID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdApplCriticality.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[3].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                ApplCriticalityID = Convert.ToInt32(((Label)(grdViewRow.Items[3].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(ApplCriticalityID) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();
        objAC.ApplCriticalityIDs = strIDs;
        objAC.Status = 0;
        objAC.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objAC.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdApplCriticality.ClearDataSource();
        populateGrid();
    }

    protected void grdApplCriticality_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdApplCriticality.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdApplCriticality.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdApplCriticality.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdApplCriticality.Behaviors.Paging.PageIndex);
        }
        //pagerControl.SetCurrentPageNumber(grdApplCriticality.Behaviors.Paging.PageIndex);
    }

    protected void grdApplCriticality_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();
            objAC.ApplCriticalityID = Convert.ToInt32(e.CommandArgument);
            DataSet dsAC = objAC.retrieve();
            DataRow dr = dsAC.Tables[0].Rows[0];
            txtApplCriticality.Text = dr[DBFields.DBFIELD_APPLCRITICALITY].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_APPLCRITICALITYDESC].ToString();

            string scrpt = "<script> document.getElementById('backColorVal').value = '" + dr[DBFields.DBFIELD_APPLCRITICALITy_BACK_COLOR].ToString() + "';</script>";
            var script = HttpUtility.JavaScriptStringEncode(scrpt, false).Replace("\\u003c", "<").Replace("\\u003e", ">").Replace("\\u0027", "'");
            ClientScript.RegisterStartupScript(typeof(string), "textvaluesetter", script,false);

            //backColorVal.Value = dr[DBFields.DBFIELD_APPLCRITICALITy_BACK_COLOR].ToString();

            Session["ApplCriticalityID"] = objAC.ApplCriticalityID;

            if (_dtRights.Select("Module = 'Application Criticality' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
        }
    }

    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        Session["ApplCriticalityID"] = null;
        txtApplCriticality.Focus();
        //-->v3.9
        var scrpt = "document.getElementById('backColorVal').value = '" + "" + "';";
        var script = HttpUtility.JavaScriptStringEncode(scrpt, false).Replace("\\u0027", "'");

        ClientScript.RegisterStartupScript(typeof(string), "textvaluesetter", script, true);

        populateGrid();
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
        this.eExporter.Export(this.grdApplCriticality, wBook);
    }


    #endregion

    #region "User Defined Methods"

    private string GetForeColor(string backColor)
    {
        int d = 0;

        Color color = (Color)System.Drawing.ColorTranslator.FromHtml("#" + backColor);

        // Counting the perceptive luminance - human eye favors green color... 
        double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

        if (a < 0.5)
            d = 0; // bright colors - black font
        else
            d = 255; // dark colors - white font

        return ColorTranslator.ToHtml(Color.FromArgb(d, d, d)).Trim('#');

    }

    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdApplCriticality.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }

    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    private void populateGrid()
    {

        grdApplCriticality.Rows.Clear();
        //objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();

        //grdApplCriticality.DataSource = objAC.retrieve().Tables[0];
        //grdApplCriticality.DataBind();
        objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();
        DataTable dtGrid = objAC.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;
        grdApplCriticality.DataSource = dtGrid;
        grdApplCriticality.DataBind();


        if (grdApplCriticality.Rows.Count == 0)
        {
            grdApplCriticality.DataSource = dtGrid;
            grdApplCriticality.DataBind();
            grdApplCriticality.Columns[3].Hidden = true;
            ibDelete.Visible = false;
        }
        else
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Application Criticality'").Length != 0)
            {
                grdApplCriticality.Columns[3].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdApplCriticality.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Application Criticality'").Length != 0)
            {
                grdApplCriticality.Columns[2].Hidden = false;
            }
            else
            {
                grdApplCriticality.Columns[2].Hidden = true;
            }
        }

        if (ibDelete.Visible == true)
        {
            int ApplCriticalityID;
            int iCount = 0;

            for (int i = 0; i < grdApplCriticality.Rows.Count; i++)
            {
                ApplCriticalityID = Convert.ToInt32(((Label)(grdApplCriticality.Rows[i].Items[3].FindControl("lblDeleteID"))).Text);

                objCommon = new iAssetTrack.BAL.CommonBAL();

                //TODO: Check the DBFields.DBFIELD_APPLCRITICALITYID
                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_APPLCRITICALITYID, ApplCriticalityID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdApplCriticality.Rows[i].Items[3].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdApplCriticality.Rows[i].Items[3].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }

            }
            if (iCount == grdApplCriticality.Rows.Count)
            {
                grdApplCriticality.Columns[3].Hidden = true;
                ibDelete.Visible = false;
            }
        }
        if (grdApplCriticality.Rows.Count > 0)
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

    protected void grdApplCriticality_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdApplCriticality.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdApplCriticality.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdApplCriticality.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdApplCriticality.Behaviors.Paging.Enabled = true;
    }
    /// <summary>
    /// Reset fields
    /// </summary>    
    private void clearFields()
    {
        txtApplCriticality.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
    }

    #endregion


}
