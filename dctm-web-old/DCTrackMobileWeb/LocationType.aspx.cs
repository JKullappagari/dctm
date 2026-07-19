/*
File Name   :	LocationType.aspx.cs

Description :	Used to maintain LocationTypes

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
using Infragistics.Web.UI.GridControls;

public partial class LocationType : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.LocationTypeBAL objLocationType;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    #endregion

    #region "Page Event Methods"
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdLocationType.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdLocationType_ItemCommand);
        pagerControl = grdLocationType.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdLocationType.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdLocationType_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdLocationType.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdLocationType.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdLocationType.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdLocationType.Behaviors.Paging.PageIndex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
    }
    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "LocationType";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "LocationType.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Location Type'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Location Type' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Location Type' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdLocationType.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        populateGrid();
        if (!IsPostBack)
        {
            Session["LocationTypeID"] = null;

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
            string sMessage = objCommon.displayMessage(MessageCodes.LCTYPE_JS_DELETE);
            this.hdnMessage.Value = sMessage;
        }
        txtDesc.Attributes.Add("onkeypress", "doKeypress(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtDesc.MaxLength.ToString() + ");");
        txtDesc.Attributes.Add("onpaste", "doPaste(this," + txtDesc.MaxLength.ToString() + ");");
    }

    /// <summary>
    /// Used to save Master data for LocationType.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        objLocationType = new iAssetTrack.BAL.LocationTypeBAL();
        objLocationType.LocationType = txtLocationType.Text;
        objLocationType.Description = txtDesc.Text;
        objLocationType.IsStorageType = Convert.ToInt32(rdoStorageType.SelectedValue.ToString());
        objLocationType.IsRfidLocation = Convert.ToInt32(rdoRfidLocation.SelectedValue.ToString());
        objLocationType.Status = 1;
        objLocationType.CreatedBy = Convert.ToInt32(Session["UserID"]);

        int intLocationType = 0;
        objLocationType.LocationTypeID = Session["LocationTypeID"] == null ? intLocationType : (int)Session["LocationTypeID"];
        intLocationType = objLocationType.exists();

        if (intLocationType != -1 && intLocationType != 0)
            objLocationType.LocationTypeID = intLocationType;

        if (intLocationType != -1)
        {
            objLocationType.Persist(DALCOperation.Insert);
            clearFields();
            if (Session["LocationTypeID"] == null)
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_INSERTED);
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            }

            Session["LocationTypeID"] = null;
            lblMessage.Visible = true;
            grdLocationType.ClearDataSource();
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
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        clearFields();
        txtLocationType.Focus();
        Session["LocationTypeID"] = null;
        //Added - 13-Nov-2006
        //Check to Show/Hide the delete button
        populateGrid();
    }

    /// <summary>
    /// Used to delete information related to specific LocationType.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int LocationTypeId;
        string strIDs;

        strIDs = "";

        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdLocationType.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[5].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                LocationTypeId = Convert.ToInt32(((Label)(grdViewRow.Items[5].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(LocationTypeId) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objLocationType = new iAssetTrack.BAL.LocationTypeBAL();
        objLocationType.LocationTypeIDs = strIDs;
        objLocationType.Status = 0;
        objLocationType.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objLocationType.Persist(DALCOperation.Delete);
        clearFields();
        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdLocationType.ClearDataSource();
        populateGrid();
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdLocationType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdLocationType.PageIndex = e.NewPageIndex;
        //populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdLocationType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //populateGrid();
        //iAssetTrack.BAL.LocationTypeBAL objEdit = new iAssetTrack.BAL.LocationTypeBAL();
        //objEdit.LocationTypeID = Convert.ToInt32(grdLocationType.DataKeys[Convert.ToInt32(e.NewEditIndex)].Value);
        //DataSet dsLocationType = objEdit.retrieve();
        //DataRow dr = dsLocationType.Tables[0].Rows[0];
        //txtLocationType.Text = dr[DBFields.DBFIELD_LOCATIONTYPE].ToString();
        //txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
        //Session["LocationTypeID"] = objEdit.LocationTypeID;
    }
    #endregion

    #region "User Defined Methods"
    private void populateGrid()
    {
        objLocationType = new iAssetTrack.BAL.LocationTypeBAL();

        grdLocationType.DataSource = objLocationType.retrieve().Tables[0];
        grdLocationType.DataBind();

        if (grdLocationType.Rows.Count == 0)
        {
            grdLocationType.DataSource = null;
            grdLocationType.DataBind();
            grdLocationType.Columns[5].Hidden = true;
            ibDelete.Visible = false;
        }
        else if (grdLocationType.Rows.Count > 0)
        {
            if (_dtRights.Select("Rights = 'Delete' and Module = 'Location Type'").Length != 0)
            {
                grdLocationType.Columns[5].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdLocationType.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }
            if (_dtRights.Select("Rights = 'Modify' and Module = 'Location Type'").Length != 0)
            {
                grdLocationType.Columns[3].Hidden = false;
            }
            else
            {
                grdLocationType.Columns[3].Hidden = true;
            }
        }

        for (int i = 0; i < grdLocationType.Rows.Count; i++)
        {
            string YesNo = ((Label)grdLocationType.Rows[i].Items[2].FindControl("IsStorageType")).Text.ToLower();
            string YesNo1 = ((Label)grdLocationType.Rows[i].Items[3].FindControl("IsRfidLocation")).Text.ToLower();

            if (YesNo.Equals("true") || YesNo.Equals("yes"))
                ((Label)(grdLocationType.Rows[i].Items[2].FindControl("IsStorageType"))).Text = "Yes";
            else
                ((Label)(grdLocationType.Rows[i].Items[2].FindControl("IsStorageType"))).Text = "No";

            if (YesNo1.Equals("true") || YesNo1.Equals("yes"))
                ((Label)(grdLocationType.Rows[i].Items[3].FindControl("IsRfidLocation"))).Text = "Yes";
            else
                ((Label)(grdLocationType.Rows[i].Items[3].FindControl("IsRfidLocation"))).Text = "No";

        }

        if (ibDelete.Visible == true)
        {
            int LocationTypeId;
            int iCount = 0;

            for (int i = 0; i < grdLocationType.Rows.Count; i++)
            {
                LocationTypeId = Convert.ToInt32(((Label)(grdLocationType.Rows[i].Items[5].FindControl("lblDeleteID"))).Text);
                grdLocationType.Rows[i].Items[5].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_LOCATIONTYPEID, LocationTypeId.ToString(),0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdLocationType.Rows[i].Items[5].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdLocationType.Rows[i].Items[5].FindControl("chkDelete").Visible == false)
                {
                    iCount += 1;
                }


            }

            if (iCount == grdLocationType.Rows.Count)
            {
                grdLocationType.Columns[5].Hidden = true;
                ibDelete.Visible = false;
            }


        }
        if (grdLocationType.Rows.Count > 0)
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

    private void clearFields()
    {
        txtLocationType.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
        rdoRfidLocation.Items[1].Selected = true;
        rdoStorageType.Items[1].Selected = true;

    }
    #endregion
    protected void grdLocationType_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            populateGrid();
            iAssetTrack.BAL.LocationTypeBAL objEdit = new iAssetTrack.BAL.LocationTypeBAL();
            objEdit.LocationTypeID = Convert.ToInt32(e.CommandArgument);
            DataSet dsLocationType = objEdit.retrieve();
            DataRow dr = dsLocationType.Tables[0].Rows[0];
            txtLocationType.Text = dr[DBFields.DBFIELD_LOCATIONTYPE].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            rdoRfidLocation.SelectedValue =(dr[DBFields.DBFIELD_RFID_LOC].ToString().ToLower() == "true" ? "1":"0");
            rdoStorageType.SelectedValue = (dr[DBFields.DBFIELD_STORAGE_LOC].ToString().ToLower() == "true" ? "1":"0");
            Session["LocationTypeID"] = objEdit.LocationTypeID;
        }
    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdLocationType, wBook);

    }
    protected void eExporter_CellExported(object sender, ExcelCellExportedEventArgs e)
    {
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[4].Width = 1;
        e.Worksheet.Columns[5].Width = 1;

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
        // IsStorage

        if (iCdex == 2 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
        {
            string strVal = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
            if (strVal.ToLower().Contains("<span") && strVal.ToLower().Contains("span>"))
                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = strVal.Replace("</span>", "").Remove(strVal.IndexOf('<'), strVal.IndexOf('>') + 1);
        }
        //IsRFID
        if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
        {
            string strVal = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
            if (strVal.ToLower().Contains("<span") && strVal.ToLower().Contains("span>"))
                e.Worksheet.Rows[iRdex].Cells[iCdex].Value = strVal.Replace("</span>", "").Remove(strVal.IndexOf('<'), strVal.IndexOf('>') + 1);
        }

    }
    protected void grdLocationType_InitializeRow(object sender, RowEventArgs e)
    {
        //if (ibDelete.Visible == true)
        //{
        //    int LocationTypeId;
        //    int iCount = 0;

        //    //for (int i = 0; i < grdLocationType.Rows.Count; i++)
        //    //{
        //        LocationTypeId = Convert.ToInt32(((Label)(e.Row.Items[5].FindControl("lblDeleteID"))).Text);

        //        objCommon = new iAssetTrack.BAL.CommonBAL();

        //        DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_LOCATIONTYPEID, LocationTypeId);
        //        foreach (DataTable tblCheck in dsCheck.Tables)
        //        {
        //            if (tblCheck.Rows[0][0].ToString() != "0")
        //            {
        //                e.Row.Items[5].FindControl("chkDelete").Visible = false;
        //            }
        //        }
        //    //    if (e.Row.Items[5].FindControl("chkDelete").Visible == false)
        //    //    {
        //    //        iCount += 1;
        //    //    }

        //    //}
        //    //if (iCount == grdLocationType.Rows.Count)
        //    //{
        //    //    grdLocationType.Columns[5].Hidden = true;
        //    //    ibDelete.Visible = false;
        //    //}
        //}

    }
}
