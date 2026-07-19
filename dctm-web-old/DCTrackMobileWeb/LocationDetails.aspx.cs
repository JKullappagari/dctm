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
using Infragistics.Web.UI.GridControls;
using iAssetTrackBAL;

public partial class LocationDetails : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.LocationBAL objLocation;
    private iAssetTrack.BAL.AssetBAL objAsset;
    // (Debasish) private iAssetTrack.BAL.CommonBAL objCommon;
    private int exportedRows = 0;

    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    #endregion

    #region "Page Event Methods"
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        Infragistics.Web.UI.GridControls.WebDataGrid grAssets = (Infragistics.Web.UI.GridControls.WebDataGrid)uwtLocationTab.Tabs[1].FindControl("grdAssetDetails");

        if (grdAssetDetails != null)
            pagerControl = grdAssetDetails.Behaviors.Paging.PagerTemplateContainerBottom.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        else
            pagerControl = grAssets.Behaviors.Paging.PagerTemplateContainerBottom.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;

        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);

    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdAssetDetails.Behaviors.Paging.PageIndex = e.PageNumber;
    }
    protected void grdAssetDetails_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdAssetDetails.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAssetDetails.Behaviors.Paging.PageIndex);
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
            pagerControl.SetCurrentPageNumber(grdAssetDetails.Behaviors.Paging.PageIndex);


    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "LocationDetails";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "Sites.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;
        if (_dtRights.Select("Module = 'Location'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        int intLocationID = 0;
        if (Request.QueryString.Get("LocationID") != null)
        {
            intLocationID = Convert.ToInt32(Request.QueryString.Get("LocationID").ToString());
            Session["LocationID"] = Request.QueryString.Get("LocationID");
        }
        else
            Response.Redirect("Location.aspx");

        PopulateAssetDetails();

        if (!this.IsPostBack)
        {
            populateLocationDetails();
            populateGrid();
            //ibCancel.Attributes.Add("onClick", "javascript:history.back(); return false;");
        }

    }
    protected void ibAddChild_Command(object sender, CommandEventArgs e)
    {

    }
    protected void ibCancel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Redirect("Location.aspx");
    }
    #endregion

    #region "User Defined Methods"
    private void populateGrid()
    {
        objLocation = new iAssetTrack.BAL.LocationBAL();
        grdChildLocations.DataSource = objLocation.GetLocationBYLocation(Convert.ToInt32(Session["LocationID"].ToString()));
        grdChildLocations.DataBind();


    }
    private void populateLocationDetails()
    {
        int locationID;
        string location, description, locationType, ipAddress, locPath;
        string isExitDoor;

        objLocation = new iAssetTrack.BAL.LocationBAL();
        locationID = Convert.ToInt32(Session["LocationID"].ToString());
        objLocation.LocationID = locationID;
        DataRow drLocation = objLocation.retrieve().Tables[0].Rows[0];

        location = drLocation[DBFields.DBFIELD_LOCATION].ToString();
        description = drLocation[DBFields.DBFIELD_LOCATION_DESC].ToString();
        isExitDoor = (bool)drLocation[DBFields.DBFIELD_ISEXITDOOR] ? "YES" : "NO";
        locationType = drLocation[DBFields.DBFIELD_LOCATIONTYPE].ToString();
        ipAddress = drLocation[DBFields.DBFIELD_IPADDRESS].ToString();
        //Location Path
        objLocation.LocationID = locationID;
        int loggedInUserId = string.IsNullOrEmpty(Session["LoggedInUser"].ToString()) ? 0 : int.Parse(Session["LoggedInUser"].ToString());
        DataSet dsLocPath = objLocation.retrieveLocationPath(loggedInUserId);
        if (dsLocPath.Tables.Count > 0 && dsLocPath.Tables[0].Rows.Count > 0)
        {
            locPath = dsLocPath.Tables[0].Rows[0]["LocationPath"].ToString();
        }
        else
        {
            locPath = string.Empty;
        }

        lblLocNameVal.Text = location;
        lblDescval.Text = description;
        lblextDrVal.Text = isExitDoor;
        lblLoctypeval.Text = locationType;
        lblPlocval.Text = locPath;
        lblipval.Text = ipAddress;
    }

    private void PopulateAssetDetails()
    {
        objAsset = new iAssetTrack.BAL.AssetBAL();
        grdAssetDetails.DataSource = objAsset.GetAssetByLocationId(Convert.ToInt32(Session["LocationID"].ToString()));
        grdAssetDetails.DataBind();


    }
    #endregion

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        wBook.Worksheets.Clear();
        Infragistics.Documents.Excel.Worksheet wSheet = wBook.Worksheets.Add("Child Location Details");
        Infragistics.Documents.Excel.Worksheet wSheet2 = wBook.Worksheets.Add("Asset Details");
        this.eExporter.ExportMode = ExportMode.Custom;
        exportedRows = 0;
        this.eExporter.RowExported += eExporter_RowExported;
        this.eExporter.Export(this.grdChildLocations, wSheet);
        this.eExporter.Export(this.grdAssetDetails, wSheet2, ++exportedRows, 0);
        string tmpName = System.IO.Path.GetTempFileName();
        wBook.Save(tmpName);
        byte[] fileContents = System.IO.File.ReadAllBytes(tmpName);
        string downloadFileExt = "xlsx";//(this.rblExcelFormat.SelectedValue == "2003") ? "xls" : "xlsx";
        string downloadFileName = "LocationDetails" + "." + downloadFileExt;
        Response.AddHeader("Content-Disposition", "attachment; filename=" + downloadFileName);
        Response.ContentType = "application/vnd.ms-excel";
        Response.OutputStream.Write(fileContents, 0, fileContents.Length);
        Response.End();

    }
    protected void eExporter_RowExported(object sender, ExcelRowExportedEventArgs e)
    {
        // exportedRows++;

    }
    protected void eExporter_CellExported(object sender, ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[2].Width = 4000;
        if (iWSdex == 0)
        {
            if (iRdex != 0)
            {

                if (iCdex == 2 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    string str = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
                    char[] sep = { '<', '>' };
                    Array a = str.Split(sep);
                    if (a.Length > 2)
                        e.Worksheet.Rows[iRdex].Cells[iCdex].Value = a.GetValue(2);

                }

            }
            else
            {

            }
        }
        else
        {
            if (iRdex != 0)
            {

                if (iCdex == 2 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    string str = e.Worksheet.Rows[iRdex].Cells[iCdex].Value.ToString();
                    char[] sep = { '<', '>' };
                    Array a = str.Split(sep);
                    if (a.Length > 2)
                        e.Worksheet.Rows[iRdex].Cells[iCdex].Value = a.GetValue(2);

                }

            }
            else
            {

            }
        }


    }

}
