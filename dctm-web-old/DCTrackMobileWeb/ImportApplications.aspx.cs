/*
File Name   :	ImportApplications.aspx

Description :	Used to Import the Application details

Date created:	23 Oct 2014

*/

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Xml;
using System.Reflection;
using iAssetTrack.BAL;
using iAssetTrack.DALC;
using System.Data.SqlClient;
using iAssetTrackBAL;
using System.Drawing;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.Web.UI.EditorControls;
using System.Web.Services;
using System.Web.Hosting;
using System.Text.RegularExpressions;

public partial class ImportApplications : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.UserBAL objUser;
    private iAssetTrack.BAL.AssetBAL objAsset;
    private iAssetTrack.BAL.ApplicationCriticalityBAL objAC;
    private iAssetTrack.BAL.ApplicationTypeBAL objAT;
    private iAssetTrack.BAL.DivisionBAL objDiv;
    private iAssetTrack.BAL.ApplicationBAL objAppl;
    private iAssetTrack.BAL.CommonBAL objCommon;
    private iAssetTrack.BAL.ManufacturerBAL objMfg;
    private iAssetTrack.BAL.OwnerBAL objOwner;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    private string fileID;
    #endregion

    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdAsset.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdAsset_ItemCommand);
        pagerControl = grdAsset.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdAsset.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Import Applications";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ImportApplications.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Import Applications' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Import Applications' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        this.grdAsset.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        if (!IsPostBack)
        {
            Session["WebUploadFilePathApps"] = null;
            populateBU();
            BindDummyRow();
            ibExportToExcel.Visible = false;
            Session["FileIDApps"] = null;
            Session["tempFileIDApps"] = null;
        }
        else
        {
            populateGrid();
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {

    }

    #endregion

    # region for WebMethod to show file stats using Ajax

    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("ID");
        dummy.Columns.Add("Name");
        dummy.Columns.Add("Value");
        dummy.Rows.Add();
        grdFileStats.DataSource = dummy;
        grdFileStats.DataBind();
    }


    [WebMethod(EnableSession = true)]
    public static string ShowFileStats(string FileName)
    {
        DataTable dtData = new DataTable();
        DataSet dsData = new DataSet();

        //create folder if doesn't exists-- kjb
        string savePath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/");
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        //guid for xml file ... by kjb on 04 Nov 2011
        string fileID = Guid.NewGuid().ToString();
        if (HttpContext.Current.Session["FileIDApps"] == null)
            HttpContext.Current.Session["FileIDApps"] = fileID;
        String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssfff");
        //string fileName = ajaxFUAsset.FileName;
        string name = FileName.Substring(0, FileName.LastIndexOf("."));
        string extn = FileName.Substring(FileName.LastIndexOf(".") + 1);
        string assetFileName = name + "_" + timeStamp + "." + extn;

        string pathToCheck = savePath + assetFileName;
        // Create a temporary file name to use for checking duplicates.
        if (File.Exists(HttpContext.Current.Session["WebUploadFilePathApps"].ToString() + FileName))
        {
            File.Copy(HttpContext.Current.Session["WebUploadFilePathApps"].ToString() + FileName, savePath + assetFileName, true);
            File.Delete(HttpContext.Current.Session["WebUploadFilePathApps"].ToString() + FileName);
        }
        try
        {
            dtData = importExcelData(savePath + assetFileName);
            if (dtData != null && dtData.Rows.Count > 0)
            {
                dsData.Tables.Add(dtData);
                if (HttpContext.Current.Session["ImportFilePathApps"] == null)
                    HttpContext.Current.Session["ImportFilePathApps"] = savePath + assetFileName;
                else
                {
                    HttpContext.Current.Session["ImportFilePathApps"] = HttpContext.Current.Session["ImportFilePathApps"].ToString() + "," + savePath + assetFileName;
                }
                return dsData.GetXml();
            }
            else
            {
                string msg = "Applications sheet not exists in " + FileName + " file.";
                HttpContext.Current.Response.StatusCode = 501;
                HttpContext.Current.Response.StatusDescription = msg;
                return msg;
            }
        }
        catch (Exception ex)
        {
            string msg = ex.Message + " error in " + FileName + " file.";
            HttpContext.Current.Response.StatusCode = 501;
            HttpContext.Current.Response.StatusDescription = msg;
            return msg;
        }
    }

    private static DataTable importExcelData(string filePath)
    {
        int assetCount;
        int assetTypeCount;
        int manufacturerCount;
        int modelCount;
        DataSet dsExcel = new DataSet();
        DataTable dtExcel = new DataTable();
        DataTable dtSite = new DataTable();
        DataTable dtRoom = new DataTable();
        DataTable dtRow = new DataTable();
        DataTable dtRack = new DataTable();
        DataTable dtAssetType = new DataTable();
        DataTable dtManufacturer = new DataTable();
        DataTable dtModel = new DataTable();
        DataTable dtFileStats = new DataTable();
        DataColumn col1 = new DataColumn("ID");
        DataColumn col2 = new DataColumn("Name");
        DataColumn col3 = new DataColumn("Value");
        dtFileStats.Columns.AddRange(new DataColumn[] { col1, col2, col3 });
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();
        dsExcel = ImportExcel(filePath);
        if (dsExcel.Tables.Count > 0)
        {
            // add two extra columns to this table to specify Status, reason
            DataColumn dcStatus = new DataColumn("Status", typeof(string));
            DataColumn dcreason = new DataColumn("Reason", typeof(string));
            DataColumn dcID = new DataColumn("ID", typeof(int));
            dsExcel.Tables[0].Columns.Add(dcID);
            dsExcel.Tables[0].Columns.Add(dcStatus);
            dsExcel.Tables[0].Columns.Add(dcreason);
            // put empty spaces or 0 for null data, in order to make them serailze
            int id = 0;
            id = 1;
            foreach (DataRow dr in dsExcel.Tables[0].Rows)
            {

                for (int colIdx = 0; colIdx < dsExcel.Tables[0].Columns.Count; colIdx++)
                {
                    //Application
                    if (dsExcel.Tables[0].Columns[colIdx].ColumnName.ToLower() == "application")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    //Change Application Crticality text to upper case
                    if (dsExcel.Tables[0].Columns[colIdx].ColumnName.ToLower() == "appcriticality")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    //app type
                    if (dsExcel.Tables[0].Columns[colIdx].ColumnName.ToLower() == "apptype")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    //Change App Status text to upper case 
                    if (dsExcel.Tables[0].Columns[colIdx].ColumnName.ToLower() == "appstatus")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    if (dsExcel.Tables[0].Columns[colIdx].ColumnName.ToLower() == "owner")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }
                    if (dsExcel.Tables[0].Columns[colIdx].ColumnName != "ID" &&
                         string.IsNullOrEmpty(dr[colIdx].ToString()))
                    {
                        dr[colIdx] = " ";
                    }
                }
                dr["ID"] = id++;
            }
            dsExcel.AcceptChanges();
            //Serialize Asset Excel dataset to xml
            String strFile;
            try
            {
                if (HttpContext.Current.Session["FileIDApps"] != null)
                {
                    strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileIDApps"].ToString() + ".xml";
                    if (File.Exists(strFile) && new FileInfo(strFile).Length > 0)
                    {
                        //already file exists with data
                        // Read the content of the file into a DataSet
                        XmlTextReader xtr = new XmlTextReader(strFile);
                        DataSet dsPrevExcelData = new DataSet();
                        dsPrevExcelData.ReadXml(xtr);
                        xtr.Close();
                        if (dsPrevExcelData.Tables.Count > 0 && dsPrevExcelData.Tables[0].Rows.Count > 0)
                        {
                            // now merge Data
                            //Merge Base table
                            int curRowsCount = dsExcel.Tables[0].Rows.Count + 1;
                            foreach (DataRow dr in dsPrevExcelData.Tables[0].Rows)
                            {
                                DataRow newRow = dsExcel.Tables[0].NewRow();
                                newRow["Application"] = dr["Application"].ToString();
                                newRow["AppCriticality"] = dr["AppCriticality"].ToString();
                                newRow["AppType"] = dr["AppType"].ToString();
                                newRow["AppStatus"] = dr["AppStatus"].ToString();
                                newRow["Owner"] = dr["Owner"].ToString();
                                newRow["Vendor"] = dr["Vendor"].ToString();
                                newRow["ID"] = curRowsCount++;
                                newRow["Status"] = "";
                                newRow["Reason"] = "";

                                dsExcel.Tables[0].Rows.Add(newRow);
                            }
                            dsExcel.AcceptChanges();
                            XmlTextWriter xtw = new XmlTextWriter(strFile, null);
                            dsExcel.WriteXml(xtw);
                            xtw.Close();
                        }
                    }
                    else
                    {
                        // first time 
                        XmlTextWriter xtw = new XmlTextWriter(strFile, null);
                        dsExcel.WriteXml(xtw);
                        xtw.Close();
                    }
                }
                SerializeExcelDataSetToXml(dsExcel);
            }
            catch (Exception ex)
            {
                throw ex;
                //lblFileUploadStatus.Text = ex.Message;
                //lblFileUploadStatus.Visible = true;
            }
            dsExcel.AcceptChanges();
            dtExcel = dsExcel.Tables[0];
            var recCount = 0;

            //AppCriticality Count
            dtManufacturer = new DataView(dtExcel).ToTable(true, new string[] { "AppCriticality" });
            recCount = (from cr in dtManufacturer.AsEnumerable()
                        where cr.Field<string>("AppCriticality").Trim() != ""
                        group cr by new
                        {
                            AppCriticality = cr.Field<String>("AppCriticality").ToLower()
                        }
                            into grp
                        select grp.First()).Count();
            manufacturerCount = (int)recCount;

            DataRow drFS5 = dtFileStats.NewRow();
            drFS5["ID"] = "5";
            drFS5["Name"] = "No of Application Criticalities found";
            drFS5["Value"] = manufacturerCount.ToString();
            dtFileStats.Rows.Add(drFS5);

            //App Type Count
            dtModel = new DataView(dtExcel).ToTable(true, new string[] { "AppType" });
            recCount = (from cr in dtModel.AsEnumerable()
                        where cr.Field<string>("AppType").Trim() != ""
                        group cr by new
                        {
                            AppType = cr.Field<String>("AppType").ToLower()
                        }
                            into grp
                        select grp.First()).Count();
            modelCount = (int)recCount;

            DataRow drFS6 = dtFileStats.NewRow();
            drFS6["ID"] = "6";
            drFS6["Name"] = "No of Application Types found";
            drFS6["Value"] = modelCount.ToString();
            dtFileStats.Rows.Add(drFS6);

            //App Status Count
            dtAssetType = new DataView(dtExcel).ToTable(true, new string[] { "AppStatus" });
            recCount = (from cr in dtAssetType.AsEnumerable()
                        where cr.Field<string>("AppStatus").Trim() != ""
                        group cr by new
                        {
                            AppStatus = cr.Field<String>("AppStatus").ToLower()
                        }
                            into grp
                        select grp.First()).Count();
            assetTypeCount = (int)recCount;

            DataRow drFS7 = dtFileStats.NewRow();
            drFS7["ID"] = "7";
            drFS7["Name"] = "No of Application Statuses found";
            drFS7["Value"] = assetTypeCount.ToString();
            dtFileStats.Rows.Add(drFS7);

            //App Division Count
            dtAssetType = new DataView(dtExcel).ToTable(true, new string[] { "Owner" });
            recCount = (from cr in dtAssetType.AsEnumerable()
                        where cr.Field<string>("Owner").Trim() != ""
                        group cr by new
                        {
                            Owner = cr.Field<String>("Owner").ToLower()
                        }
                            into grp
                        select grp.First()).Count();
            assetTypeCount = (int)recCount;


            DataRow drFS1 = dtFileStats.NewRow();
            drFS1["ID"] = "8";
            drFS1["Name"] = "No of Unique Owner/Custodians found";
            drFS1["Value"] = assetTypeCount.ToString();
            dtFileStats.Rows.Add(drFS1);

            //Asset Count
            assetCount = dtExcel.Rows.Count;

            DataRow drFS8 = dtFileStats.NewRow();
            drFS8["ID"] = "9";
            drFS8["Name"] = "Total No of Applications";
            drFS8["Value"] = assetCount.ToString();
            dtFileStats.Rows.Add(drFS8);
            dtFileStats.AcceptChanges();

            //ibCreate.Visible = true;
            //ibCreate.Enabled = true;
        }
        return dtFileStats;
    }

    protected void ShowFileData()
    {

        int assetCount;
        int assetTypeCount;
        int manufacturerCount;
        int modelCount;
        DataSet dsExcelData = new DataSet();
        DataTable dtExcel = new DataTable();
        DataTable dtSite = new DataTable();
        DataTable dtAsset = new DataTable();
        DataTable dtBusinessUnit = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable dtRoom = new DataTable();
        DataTable dtRow = new DataTable();
        DataTable dtAppCri = new DataTable();
        DataTable dtAppType = new DataTable();
        DataTable dtAppStatus = new DataTable();
        DataTable dtOwner = new DataTable();
        DataTable dtMfg = new DataTable();
        DataTable dtAssetModel = new DataTable();
        DataTable dtFileStats = new DataTable();
        dtFileStats.Columns.Add("ID");
        dtFileStats.Columns.Add("Name");
        dtFileStats.Columns.Add("Value");
        dtFileStats.AcceptChanges();
        var recCount = 0;
        dsExcelData = DeserializeDataSource();
        dtExcel = dsExcelData.Tables[0];

        //AppCriticality Count
        dtAppCri = new DataView(dtExcel).ToTable(true, new string[] { "AppCriticality" });
        recCount = (from cr in dtAppCri.AsEnumerable()
                    where cr.Field<string>("AppCriticality").Trim() != ""
                    select cr).Count();
        manufacturerCount = (int)recCount;

        DataRow drFS5 = dtFileStats.NewRow();
        drFS5["ID"] = "5";
        drFS5["Name"] = "No of Application Criticalities found";
        drFS5["Value"] = manufacturerCount.ToString();
        dtFileStats.Rows.Add(drFS5);

        //App Type Count
        dtAppType = new DataView(dtExcel).ToTable(true, new string[] { "AppType" });
        recCount = (from cr in dtAppType.AsEnumerable()
                    where cr.Field<string>("AppType").Trim() != ""
                    select cr).Count();
        modelCount = (int)recCount;

        DataRow drFS6 = dtFileStats.NewRow();
        drFS6["ID"] = "6";
        drFS6["Name"] = "No of Application Types found";
        drFS6["Value"] = modelCount.ToString();
        dtFileStats.Rows.Add(drFS6);

        //App Status Count
        dtAppStatus = new DataView(dtExcel).ToTable(true, new string[] { "AppStatus" });
        recCount = (from cr in dtAppStatus.AsEnumerable()
                    where cr.Field<string>("AppStatus").Trim() != ""
                    select cr).Count();
        assetTypeCount = (int)recCount;

        DataRow drFS7 = dtFileStats.NewRow();
        drFS7["ID"] = "7";
        drFS7["Name"] = "No of Application Statuses found";
        drFS7["Value"] = assetTypeCount.ToString();
        dtFileStats.Rows.Add(drFS7);

        //Owner Count
        dtOwner = new DataView(dtExcel).ToTable(true, new string[] { "Owner" });
        recCount = (from cr in dtOwner.AsEnumerable()
                    where cr.Field<string>("Owner").Trim() != ""
                    select cr).Count();
        assetTypeCount = (int)recCount;


        DataRow drFS1 = dtFileStats.NewRow();
        drFS1["ID"] = "8";
        drFS1["Name"] = "No of Owner/Custodians found";
        drFS1["Value"] = assetTypeCount.ToString();
        dtFileStats.Rows.Add(drFS1);

        //Application Count
        assetCount = dtExcel.Rows.Count;

        DataRow drFS8 = dtFileStats.NewRow();
        drFS8["ID"] = "9";
        drFS8["Name"] = "Total No of Applications";
        drFS8["Value"] = assetCount.ToString();
        dtFileStats.Rows.Add(drFS8);
        dtFileStats.AcceptChanges();

        grdFileStats.DataSource = dtFileStats;
        grdFileStats.DataBind();

        FileStats.Visible = true;

    }

    public static DataSet ImportExcel(string FileName)
    {
        //string HDR = hasHeaders ? "Yes" : "No";
        //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=1\"";
        //string strConn = "Provider =Microsoft.ACE.OLEDB.12.0; Data Source =" + FileName + "; Extended Properties =\"Excel 12.0 Xml;HDR=YES;IMEX=1";
        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0 Xml;IMEX=1;HDR=YES;\" ";
        DataSet output = new DataSet();

        using (OleDbConnection conn = new OleDbConnection(strConn))
        {
            conn.Open();

            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            string sheet = string.Empty;
            OleDbCommand cmd = null; ;
            DataTable outputTable;

            foreach (DataRow row in dt.Rows)
            {
                switch (row["TABLE_NAME"].ToString())
                {
                    case "Applications$":
                        sheet = "Applications$";
                        //if (sheet.Equals("Applications$", StringComparison.InvariantCultureIgnoreCase))
                        //{
                        //OleDbCommand cmdDel = new OleDbCommand(" Delete from [" + sheet + "] " + 
                        //    " where HostName IS  NULL AND SerialNumber IS  NULL AND Site IS  NULL AND Manufacturer IS  NULL " +
                        //    " AND Model IS  NULL AND Equipmenttype IS  NULL AND Room IS  NULL AND Row IS  NULL " +
                        //    " AND Rack IS  NULL AND StartPosition IS  NULL AND NoofRus IS  NULL AND [Rack/Stand] IS  NULL ",conn);
                        //cmdDel.CommandType = CommandType.Text;
                        //int count = cmdDel.ExecuteNonQuery();

                        cmd = new OleDbCommand("SELECT * FROM [" + sheet + "] ", conn);
                        cmd.CommandType = CommandType.Text;
                        outputTable = new DataTable(sheet);
                        output.Tables.Add(outputTable);
                        new OleDbDataAdapter(cmd).Fill(outputTable);
                        //delete blank rows
                        output.Tables[0].BeginLoadData();
                        (from cr in output.Tables[0].AsEnumerable()
                         where cr.Field<string>("Application") == null && cr.Field<string>("AppCriticality") == null &&
                         cr.Field<string>("AppType") == null &&
                         cr.Field<string>("AppStatus") == null && cr.Field<string>("Owner") == null &&
                         cr.Field<string>("Vendor") == null
                         select cr).ToList().ForEach(cr => cr.Delete());
                        output.Tables[0].EndLoadData();
                        output.Tables[0].AcceptChanges();
                        break;
                }
            }
        }
        return output;
    }

    # endregion

    # region Event-Handlers

    protected void wuImportAsset_OnUploadFinished(object sender, UploadFinishedEventArgs e)
    {
        Session["WebUploadFilePathApps"] = e.FolderPath;
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        bool bIsBUInserted = false;

        bIsBUInserted = insertBU();

        if (bIsBUInserted == true)
        {
            //App Criticality
            insertAppCriticality();
            //App Types
            insertAppTypes();
            // Owner
            insertOwner();
            //Vendor
            insertManufacturer();
            // Assets
            insertApps();
            if (Session["FileIDApps"] != null)
                Session["tempFileIDApps"] = Session["FileIDApps"].ToString();
            //populate grid
            populateGrid();
            ibCreate.Enabled = false;
            FileStats.Visible = true;
            FileStats.Style.Add("display", "block");
            UploadStats.Style.Add("display", "block");
            ShowFileData();
            ResetAll();
        }
    }

    public void insertOwner()
    {
        string ownerName = string.Empty;
        Dictionary<int, string> dictOwnersInserted = new Dictionary<int, string>();
        string ownerFName = string.Empty;
        string ownerLName = string.Empty;

        //Retreive all the Owner details from the database.
        DataSet dsAllOwners = new DataSet();
        DataTable dtAllOwners = new DataTable();
        objOwner = new iAssetTrack.BAL.OwnerBAL();
        dsAllOwners = objOwner.retrieve();

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            string tenantAssignedOwners = string.Empty;
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                DataSet dsTD = objTenant.retrieveOwnerAssignmentList();
                if (dsTD.Tables.Count > 0 && dsTD.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsTD.Tables[0].Rows)
                    {
                        tenantAssignedOwners = tenantAssignedOwners + dr[DBFields.DBFIELD_OWNER_ID].ToString() + ",";
                    }
                    tenantAssignedOwners = tenantAssignedOwners.Trim(',');

                }
                if (!string.IsNullOrEmpty(tenantAssignedOwners))
                {
                    DataTable tempTable = dsAllOwners.Tables[0].Clone();
                    foreach (DataRow dr in dsAllOwners.Tables[0].Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("OwnerID IN (" + tenantAssignedOwners + ")");
                    dsAllOwners.Tables[0].Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow dr in filteredRows)
                        {
                            dsAllOwners.Tables[0].Rows.Add(dr.ItemArray);
                        }

                    }
                }
                else
                {
                    dsAllOwners.Tables[0].Rows.Clear();
                }

            }

        }

        try
        {
            //Get the AssetTypeIDs and AssetTypes into table dtAllAssetTypeNames
            var allOwners = (from rows in dsAllOwners.Tables[0].AsEnumerable()
                             orderby rows.Field<int>("OwnerID")
                             select new
                             {
                                 OwnerID = rows.Field<int>("OwnerID"),
                                 OwnerName = rows.Field<string>("OwnerLastName") + "," + rows.Field<string>("OwnerFirstName"),
                                 Email = rows.Field<string>("Email")

                             });
            dtAllOwners = LINQToDataTable(allOwners, "dtOwner");
            if (dtAllOwners.Columns.Count == 0)
            {
                dtAllOwners.Columns.Add("OwnerID", typeof(int));
                dtAllOwners.Columns.Add("OwnerName", typeof(string));
                dtAllOwners.Columns.Add("Email", typeof(string));
                dtAllOwners.AcceptChanges();
            }
            //Get the new Owners from the Import Applications Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();
            DataTable dtTable = new DataTable();
            DataTable dtResult = new DataTable();
            DataTable dtOwnersInExcel = new DataTable();
            DataTable dtOwnersResult = new DataTable();
            dtTable = dsExcelData.Tables[0];//Applications Table



            dtOwnersInExcel = new DataView(dtTable).ToTable(true, new string[] { "Owner" });

            //Check if the Asset Type already exist in database.
            foreach (DataRow dr in dtOwnersInExcel.Rows)
            {
                ownerName = dr["Owner"].ToString().Trim();
                if (ownerName == "NULL" || ownerName == "N/A" || ownerName == null || ownerName == string.Empty)
                {
                    ownerName = "";
                }

                if (!string.IsNullOrEmpty(ownerName.Trim()))
                {

                    if (ownerName.Contains(','))
                    {
                        ownerLName = ownerName.Split(',')[0];
                        ownerFName = ownerName.Split(',')[1];
                    }
                    else if (!ownerName.Contains(','))
                    {
                        ownerLName = "";
                        ownerFName = ownerName;
                    }
                    //Owner First and Last names must be less than 50 char length
                    //A-Za-z0-9, Single space and dot
                    Regex regx = new Regex(@"^[A-Za-z0-9\.]+(\s{1}[A-Za-z0-9\.]+)*\s{0,1}$");
                    if (string.IsNullOrEmpty(ownerLName.Trim()) || (regx.IsMatch(ownerLName.Trim()) && ownerLName.Trim().Length <= 50))
                    {
                        if (string.IsNullOrEmpty(ownerFName.Trim()) || (regx.IsMatch(ownerFName.Trim()) && ownerFName.Trim().Length <= 50))
                        {
                            ownerName = ownerLName.Trim() + "," + ownerFName.Trim();

                            IEnumerable<DataRow> drResult = from ownerRow in dtAllOwners.AsEnumerable()
                                                            where ownerRow.Field<string>("OwnerName").Equals(ownerName, StringComparison.InvariantCultureIgnoreCase)
                                                            select ownerRow;
                            //if the Owner Name is not present in database
                            if (drResult.Count() == 0)
                            {
                                //Insert the Owner Name
                                try
                                {
                                    objOwner = new iAssetTrack.BAL.OwnerBAL();
                                    objOwner.FirstName = ownerFName.Trim();
                                    objOwner.LastName = ownerLName.Trim();
                                    objOwner.Email = "";
                                    objOwner.Status = 1;
                                    objOwner.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                    objOwner.Persist(DALCOperation.Insert);
                                    dictOwnersInserted.Add(objOwner.OwnerID, objOwner.LastName + "," + objOwner.FirstName);
                                }
                                catch (Exception ex)
                                {
                                    ExceptionPolicy.HandleException(ex, "errDCTrack");
                                }
                            }
                            //if the Owner already exist in database
                            else
                            {
                                //Owner Already exists.
                                dtOwnersResult = drResult.CopyToDataTable();
                                objOwner = new iAssetTrack.BAL.OwnerBAL();
                                objOwner.OwnerID = Convert.ToInt32(dtOwnersResult.Rows[0]["OwnerID"]);
                                objOwner.FirstName = ownerFName.Trim();
                                objOwner.LastName = ownerLName.Trim();
                                objOwner.Status = 1;
                                objOwner.Email = dtOwnersResult.Rows[0]["Email"].ToString();
                                objOwner.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                objOwner.Persist(DALCOperation.Insert);
                            }
                        }
                        else
                        {
                            //Owner first Name is not in correct format
                        }
                    }
                    else
                    {
                        //Owner Last Name is not in correct format
                    }

                }
            }

            //Loop through the Dictionary(with the OwnerIDs and Owner Details) of the Owner inserted
            if (dictOwnersInserted.Values.Count != 0)
            {
                foreach (KeyValuePair<int, string> pair in dictOwnersInserted)
                {
                    DataRow dr = dtAllOwners.NewRow();
                    dr["OwnerID"] = pair.Key;
                    dr["OwnerName"] = pair.Value;
                    dr["Email"] = ""; // email will be always empty as there is no field in import exccel to capture email id of Owner.
                    dtAllOwners.Rows.Add(dr);
                }
            }
            dsExcelData.Tables.Add(dtAllOwners);
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            ImportApplications.SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    private void ResetAll()
    {
        Session["WebUploadFilePathApps"] = null;
        ibExportToExcel.Visible = true;
        Session["FileIDApps"] = null;
        uploadButton.Style.Add("display", "none");
    }

    protected void grdAsset_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {

    }

    protected void grdAsset_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdAsset.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAsset.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdAsset.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAsset.Behaviors.Paging.PageIndex);
        }

    }

    protected void grdAsset_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {

    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        //Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        //this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        //Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        //this.eExporter.Export(this.grdAsset, wBook);

        WebControl[] array = new WebControl[1];
        array[0] = grdAsset;

        //define the workbook and some worksheets. 
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        Infragistics.Documents.Excel.Workbook book = new Infragistics.Documents.Excel.Workbook(excelFormat);
        book.Worksheets.Add("Applications");

        //first export the grids to each worksheet. Custom export mode is required for that.
        this.eExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;

        this.eExporter.Export(grdAsset, book.Worksheets[0]);

        //change the export mode back to Download
        this.eExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;

        //export the workbook and pass it an empty array (as the grids are already in the worksheets).
        this.eExporter.Export(book, 0, 0, new WebControl[0]);

    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {

    }

    #endregion

    #region "User Defined Methods"

    public void insertAppCriticality()
    {
        string CriticalityName = string.Empty;
        ArrayList alACToInsert = new ArrayList();
        Dictionary<int, string> dictACInserted = new Dictionary<int, string>();

        try
        {
            //Retreive all the manufacturer from the database.
            DataSet dsAllACs = new DataSet();
            DataTable dtAllACNames = new DataTable();
            objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();
            dsAllACs = objAC.retrieve();
            //Get the App Criticality IDs and names into table dtAllACNames
            var query = (from rows in dsAllACs.Tables[0].AsEnumerable()
                         orderby rows.Field<int>("ApplCriticalityID")
                         select new
                         {
                             ApplCriticalityID = rows.Field<int>("ApplCriticalityID"),
                             ApplCriticality = rows.Field<string>("ApplCriticality")
                         });
            dtAllACNames = LINQToDataTable(query, "dtAC");
            if (dtAllACNames.Columns.Count == 0)
            {
                dtAllACNames.Columns.Add("ApplCriticalityID", typeof(int));
                dtAllACNames.Columns.Add("ApplCriticality", typeof(string));
                dtAllACNames.AcceptChanges();
            }
            //Get the new manufacturer from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();
            DataTable dtTable = new DataTable();
            DataTable dtACsInExcel = new DataTable();
            dtTable = dsExcelData.Tables[0];
            dtACsInExcel = new DataView(dtTable).ToTable(true, new string[] { "AppCriticality" });
            //Check if the manufacturer already exist in database.
            DataRow[] duplicateRows = new DataRow[dtACsInExcel.Rows.Count - 1];
            int idx = 0;
            ArrayList arrAC = new ArrayList();
            foreach (DataRow dr in dtACsInExcel.Rows)
            {
                if (!arrAC.Contains(dr[0].ToString().ToLower().Trim()))
                {
                    arrAC.Add(dr[0].ToString().ToLower());
                }
                else
                {
                    duplicateRows[idx++] = dr;
                }
            }
            arrAC = null;
            foreach (DataRow dr in duplicateRows)
            {
                if (dr != null)
                    dtACsInExcel.Rows.Remove(dr);
            }
            foreach (DataRow dr in dtACsInExcel.Rows)
            {
                CriticalityName = dr[0].ToString();

                Regex regx = new Regex(@"^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");

                if (!string.IsNullOrEmpty(CriticalityName.Trim()) && regx.IsMatch(CriticalityName.Trim()) && CriticalityName.Trim().Length <= 50)
                {
                    IEnumerable<DataRow> drResult = from mfgRow in dtAllACNames.AsEnumerable()
                                                    where mfgRow.Field<string>("ApplCriticality").Equals(CriticalityName.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                    select mfgRow;
                    //if the Manufacturer is not present in database
                    if (drResult.Count() == 0)
                    {
                        //Insert the App Criticality
                        try
                        {
                            objAC = new iAssetTrack.BAL.ApplicationCriticalityBAL();
                            objAC.ApplCriticality = CriticalityName;
                            objAC.Description = CriticalityName;
                            objAC.Status = 1;
                            objAC.CreatedBy = Convert.ToInt32(Session["UserID"]);
                            objAC.Persist(DALCOperation.Insert);
                            dictACInserted.Add(objAC.ApplCriticalityID, objAC.ApplCriticality);
                        }
                        catch { }
                    }
                    else
                    {
                    }
                }
            }

            //Loop through the Dictionary(with the ManufacturerIds and Manufacturers) of the Manufacturer inserted
            if (dictACInserted.Values.Count != 0)
            {
                foreach (KeyValuePair<int, string> pair in dictACInserted)
                {
                    DataRow dr = dtAllACNames.NewRow();
                    dr["ApplCriticalityID"] = pair.Key;
                    dr["ApplCriticality"] = pair.Value;
                    dtAllACNames.Rows.Add(dr);
                }
            }
            dsExcelData.Tables.Add(dtAllACNames);
            dsExcelData.AcceptChanges();
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void insertAppTypes()
    {
        string typeName = string.Empty;
        Dictionary<int, string> dictATInserted = new Dictionary<int, string>();
        //Retreive all the types from the database.
        DataSet dsAllATs = new DataSet();
        DataTable dtAllATNames = new DataTable();
        objAT = new iAssetTrack.BAL.ApplicationTypeBAL();
        dsAllATs = objAT.retrieve();

        try
        {
            //Get the modelIDs and ModelName into table dtAllModelNames
            var allATs = (from rows in dsAllATs.Tables[0].AsEnumerable()
                          orderby rows.Field<int>("ApplTypeID")
                          select new
                          {
                              ApplTypeID = rows.Field<int>("ApplTypeID"),
                              ApplType = rows.Field<string>("ApplType")
                          });
            dtAllATNames = LINQToDataTable(allATs, "dtAT");
            if (dtAllATNames.Columns.Count == 0)
            {
                dtAllATNames.Columns.Add("ApplTypeID", typeof(int));
                dtAllATNames.Columns.Add("ApplType", typeof(string));
                dtAllATNames.AcceptChanges();
            }
            //Get the new models from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();
            DataTable dtTable = new DataTable();
            DataTable dtResult = new DataTable();
            DataTable dtResultBU = new DataTable();
            DataTable dtATsInExcel = new DataTable();
            dtTable = dsExcelData.Tables[0];

            string strBU = txtBU.Text;

            dtATsInExcel = new DataView(dtTable).ToTable(true, new string[] { "AppType" });

            //Check if the Asset Model already exist in database.
            foreach (DataRow dr in dtATsInExcel.Rows)
            {
                typeName = dr["AppType"].ToString().Trim();

                Regex regx = new Regex(@"^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");

                if (!string.IsNullOrEmpty(typeName.Trim()) && regx.IsMatch(typeName.Trim()) && typeName.Trim().Length <= 50)
                {

                    IEnumerable<DataRow> drResult = from modelRow in dtAllATNames.AsEnumerable()
                                                    where modelRow.Field<string>("ApplType").Equals(typeName.Trim(),
                                                    StringComparison.CurrentCultureIgnoreCase)
                                                    select modelRow;

                    //if the App Type is not present in database
                    if (drResult.Count() == 0)
                    {
                        //Insert the Asset Model
                        try
                        {
                            objAT = new iAssetTrack.BAL.ApplicationTypeBAL();
                            objAT.ApplType = typeName;
                            objAT.Description = typeName;
                            objAT.Status = 1;
                            objAT.CreatedBy = Convert.ToInt32(Session["UserID"]);

                            objAT.Persist(DALCOperation.Insert);
                            DataRow nDr = dtAllATNames.NewRow();
                            nDr["ApplTypeID"] = objAT.ApplTypelID;
                            nDr["ApplType"] = objAT.ApplType;
                            dtAllATNames.Rows.Add(nDr);
                            dtAllATNames.AcceptChanges();

                        }
                        catch (Exception ex)
                        {
                            dr["Status"] = "Failed";
                            dr["Reason"] = ex.Message;
                            ExceptionPolicy.HandleException(ex, "errDCTrack");
                        }
                    }
                    //if the Asset Model already exist in database
                    else
                    {
                        //App Type Already exists.
                    }
                }
            }


            dsExcelData.Tables.Add(dtAllATNames);
            dsExcelData.AcceptChanges();
            //Serialize the dataset dsExcel data - with 6 tables
            //(Excel data, BudsinessUnit table, Site table, Location table, Manufacturer table and Model table)
            SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public bool insertBU()
    {

        string strBUName = string.Empty;
        bool bIsBUInserted = false;
        ArrayList alInsertedBU = new ArrayList();
        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        DataTable dtAllBUNames = new DataTable();
        DataSet dsAllBU = objBU.retrieve();
        //Get the BusinessUnitIDs and BusinessUnitNames into table dtAllBUNames
        var query = (from rows in dsAllBU.Tables[0].AsEnumerable()
                     orderby rows.Field<int>("BusinessUnitID")
                     select new
                     {
                         BusinessUnitID = rows.Field<int>("BusinessUnitID"),
                         BusinessUnit = rows.Field<string>("BusinessUnit")
                     });
        dtAllBUNames = LINQToDataTable(query, "dtBusinessUnit");
        if (dtAllBUNames.Columns.Count == 0)
        {
            dtAllBUNames.Columns.Add("BusinessUnitID", typeof(int));
            dtAllBUNames.Columns.Add("BusinessUnit", typeof(string));
            dtAllBUNames.AcceptChanges();
        }
        // strBUName = txtBU.Text;
        strBUName = txtBU.Text;
        IEnumerable<DataRow> drResult = from row in dtAllBUNames.AsEnumerable()
                                        where row.Field<string>("BusinessUnit").Equals(strBUName, StringComparison.InvariantCultureIgnoreCase)
                                        select row;

        if (drResult.Count() == 0)
        {
            try
            {
                //objBU.BusinessUnit = txtBU.Text;
                //objBU.Description = txtBU.Text;
                objBU.BusinessUnit = txtBU.Text;
                objBU.Description = txtBU.Text;
                objBU.Status = 1;
                objBU.FromIA = true;
                objBU.CreatedBy = Convert.ToInt32(Session["UserID"]);
                objBU.CoPrefix = "1";
                objBU.Persist(DALCOperation.Insert);
                bIsBUInserted = true;
                alInsertedBU.Add(objBU.BusinessUnitID);
                alInsertedBU.Add(objBU.BusinessUnit);
            }
            catch (Exception ex)
            {
                //    lblMessage.Text = ex.Message;
                //    lblMessage.Visible = true;
                bIsBUInserted = false;
                ExceptionPolicy.HandleException(ex, "errDCTrack");
                throw ex;
            }
        }
        else if (drResult.Count() > 0)
        {
            bIsBUInserted = true;
        }

        //TODO: if alInsertedBU is null or empty
        if (alInsertedBU.Count != 0)
        {
            DataRow dr = dtAllBUNames.NewRow();
            dr["BusinessUnitID"] = alInsertedBU[0];
            dr["BusinessUnit"] = alInsertedBU[1];
            dtAllBUNames.Rows.Add(dr);
        }

        //Get the Import Asset Excel data and add a new table BusinessUnit to the dataset
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        dsExcelData.Tables.Add(dtAllBUNames);
        dsExcelData.AcceptChanges();
        //Serialize the dataset dsExcel data - with 2 tables(Excel data and BusinessUnit table)
        SerializeExcelDataSetToXml(dsExcelData);
        return bIsBUInserted;
    }

    public void insertManufacturer()
    {
        string manufacturerName = string.Empty;
        ArrayList alMfgToInsert = new ArrayList();
        Dictionary<int, string> dictMfgInserted = new Dictionary<int, string>();

        try
        {
            //Retreive all the manufacturer from the database.
            DataSet dsAllMfgs = new DataSet();
            DataTable dtAllMfgNames = new DataTable();
            objMfg = new iAssetTrack.BAL.ManufacturerBAL();
            dsAllMfgs = objMfg.retrieve();
            //Get the mfgIDs and MfgName into table dtAllMfgNames
            var query = (from rows in dsAllMfgs.Tables[0].AsEnumerable()
                         orderby rows.Field<int>("MfgID")
                         select new
                         {
                             mfgID = rows.Field<int>("MfgID"),
                             mfgName = rows.Field<string>("mfgName")
                         });
            dtAllMfgNames = LINQToDataTable(query, "dtManufacturer");
            if (dtAllMfgNames.Columns.Count == 0)
            {
                dtAllMfgNames.Columns.Add("mfgID", typeof(int));
                dtAllMfgNames.Columns.Add("mfgName", typeof(string));
                dtAllMfgNames.AcceptChanges();
            }
            //Get the new manufacturer from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();
            DataTable dtTable = new DataTable();
            DataTable dtMfgsInExcel = new DataTable();
            dtTable = dsExcelData.Tables[0];
            dtMfgsInExcel = new DataView(dtTable).ToTable(true, new string[] { "Vendor" });
            //Check if the manufacturer already exist in database.
            DataRow[] duplicateRows = new DataRow[dtMfgsInExcel.Rows.Count - 1];
            int idx = 0;
            ArrayList arrMfg = new ArrayList();
            foreach (DataRow dr in dtMfgsInExcel.Rows)
            {
                if (!arrMfg.Contains(dr[0].ToString().ToLower().Trim()))
                {
                    arrMfg.Add(dr[0].ToString().ToLower());
                }
                else
                {
                    duplicateRows[idx++] = dr;
                }
            }
            arrMfg = null;
            foreach (DataRow dr in duplicateRows)
            {
                if (dr != null)
                    dtMfgsInExcel.Rows.Remove(dr);
            }
            foreach (DataRow dr in dtMfgsInExcel.Rows)
            {
                manufacturerName = dr[0].ToString();

                Regex regx = new Regex(@"^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");

                if (!string.IsNullOrEmpty(manufacturerName.Trim()) && regx.IsMatch(manufacturerName.Trim()) && manufacturerName.Trim().Length <= 50)
                {
                    IEnumerable<DataRow> drResult = from mfgRow in dtAllMfgNames.AsEnumerable()
                                                    where mfgRow.Field<string>("mfgName").Equals(manufacturerName.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                    select mfgRow;
                    //if the Manufacturer is not present in database
                    if (drResult.Count() == 0)
                    {
                        //Insert the Manufacturers
                        try
                        {
                            objMfg = new iAssetTrack.BAL.ManufacturerBAL();
                            objMfg.MfgName = manufacturerName;
                            objMfg.Description = manufacturerName;
                            objMfg.Status = 1;
                            objMfg.CreatedBy = Convert.ToInt32(Session["UserID"]);
                            objMfg.Persist(DALCOperation.Insert);
                            dictMfgInserted.Add(objMfg.MFGID, objMfg.MfgName);
                        }
                        catch { }
                    }
                    //if the Manufacturer already exist in database
                    else
                    {
                        //Manufacturer Already exists.
                    }
                }
            }

            //Loop through the Dictionary(with the ManufacturerIds and Manufacturers) of the Manufacturer inserted
            if (dictMfgInserted.Values.Count != 0)
            {
                foreach (KeyValuePair<int, string> pair in dictMfgInserted)
                {
                    DataRow dr = dtAllMfgNames.NewRow();
                    dr["MfgID"] = pair.Key;
                    dr["mfgName"] = pair.Value;
                    dtAllMfgNames.Rows.Add(dr);
                }
            }
            dsExcelData.Tables.Add(dtAllMfgNames);
            dsExcelData.AcceptChanges();
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void insertApps()
    {

        #region Get all tables from dataset

        string criticalityName = string.Empty;
        string typeName = string.Empty;
        string ownerName = string.Empty;
        string vendor = string.Empty;
        string buName = txtBU.Text;
        string appStatus = string.Empty;
        string appName = string.Empty;
        string ownerLName = string.Empty;
        string ownerFName = string.Empty;
        int appStatusID = 0;
        int typeID;
        int criticalityID;
        int buID;
        int mfgID;
        int ownerID;
        //Get all tables from the Import Asset Excel
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtBusinessUnit = new DataTable();
        DataTable dtAT = new DataTable();
        DataTable dtAC = new DataTable();
        DataTable dtOwner = new DataTable();
        DataTable dtMfg = new DataTable();
        DataTable dtApps = new DataTable();
        //Result tables
        DataTable dtBUResult = new DataTable();
        DataTable dtATResult = new DataTable();
        DataTable dtACResult = new DataTable();
        DataTable dtDivResult = new DataTable();
        DataTable dtMfgResult = new DataTable();
        DataTable dtOwnerResult = new DataTable();
        dtApps = dsExcelData.Tables[0];
        dtBusinessUnit = dsExcelData.Tables["dtBusinessUnit"];
        dtAC = dsExcelData.Tables["dtAC"];
        dtAT = dsExcelData.Tables["dtAT"];
        dtOwner = dsExcelData.Tables["dtOwner"];
        dtMfg = dsExcelData.Tables["dtManufacturer"];

        #endregion

        foreach (DataRow dr in dtApps.Rows)
        {
            if (!string.IsNullOrEmpty(dr["Application"].ToString().Trim()))
            {
                appName = dr["Application"].ToString().Trim();
                Regex regx = new Regex(@"^[\w\-\.&,:]+(\s{1}[\w\-\.&,\:]+)*\s{0,1}$");

                if (regx.IsMatch(appName.Trim()) && appName.Trim().Length <= 250)
                {

                    if (!string.IsNullOrEmpty(dr["AppCriticality"].ToString().Trim()))
                    {
                        try
                        {

                            if (!string.IsNullOrEmpty(dr["AppType"].ToString().Trim()))
                            {
                                #region Assign Variables

                                criticalityName = (dr["AppCriticality"].ToString()).Trim();
                                typeName = (dr["AppType"].ToString()).Trim();
                                string mfgName = dr["Vendor"].ToString().Trim();
                                #endregion

                                regx = new Regex(@"^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");

                                if (regx.IsMatch(criticalityName.Trim()) && criticalityName.Trim().Length <= 50)
                                {

                                    regx = new Regex(@"^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");

                                    if (regx.IsMatch(typeName.Trim()) && typeName.Trim().Length <= 50)
                                    {

                                        regx = new Regex(@"^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");

                                        if (string.IsNullOrEmpty(mfgName) || (regx.IsMatch(mfgName.Trim()) && mfgName.Trim().Length <= 50))
                                        {

                                            ownerName = (dr["Owner"].ToString()).Trim();

                                            if (ownerName.Contains(','))
                                            {
                                                ownerLName = ownerName.Split(',')[0];
                                                ownerFName = ownerName.Split(',')[1];
                                            }
                                            else if (!ownerName.Contains(','))
                                            {
                                                ownerLName = "";
                                                ownerFName = ownerName;
                                            }

                                            //Owner First and Last names must be less than 50 char length
                                            //A-Za-z0-9, Single space and dot
                                            regx = new Regex(@"^[A-Za-z0-9\.]+(\s{1}[A-Za-z0-9\.]+)*\s{0,1}$");
                                            if (string.IsNullOrEmpty(ownerLName.Trim()) || (regx.IsMatch(ownerLName.Trim()) && ownerLName.Trim().Length <= 50))
                                            {
                                                if (string.IsNullOrEmpty(ownerFName.Trim()) || (regx.IsMatch(ownerFName.Trim()) && ownerFName.Trim().Length <= 50))
                                                {
                                                    //owner Name = lastname,first name
                                                    ownerName = ownerLName.Trim() + "," + ownerFName.Trim();

                                                    appStatus = (dr["AppStatus"].ToString()).Trim();
                                                    if (string.IsNullOrEmpty(appStatus))
                                                        appStatus = "Others";

                                                    appStatusID = GetAppStatusID(appStatus);

                                                    if (appStatusID > 0)
                                                    {

                                                        #region Get Required IDs

                                                        //1. Get the Criticality of the App

                                                        var acResult = from atRow in dtAC.AsEnumerable()
                                                                       where atRow.Field<string>("ApplCriticality").Equals(criticalityName, StringComparison.InvariantCultureIgnoreCase)
                                                                       select atRow;

                                                        dtACResult = acResult.CopyToDataTable();
                                                        criticalityID = Convert.ToInt32(dtACResult.Rows[0]["ApplCriticalityID"]);

                                                        //2. Get the App Type
                                                        var atResult = from mRow in dtAT.AsEnumerable()
                                                                       where mRow.Field<string>("ApplType").Equals(typeName, StringComparison.InvariantCultureIgnoreCase)
                                                                       select mRow;
                                                        if (atResult.Count() > 0)
                                                        {
                                                            dtATResult = atResult.CopyToDataTable();
                                                            typeID = Convert.ToInt32(dtATResult.Rows[0]["ApplTypeID"]);
                                                        }
                                                        else
                                                        {
                                                            typeID = 0;
                                                        }



                                                        //3. Get Vendor details

                                                        var mfgResult = from mRow in dtMfg.AsEnumerable()
                                                                        where mRow.Field<string>("MfgName").Equals(mfgName, StringComparison.InvariantCultureIgnoreCase)
                                                                        select mRow;
                                                        if (mfgResult.Count() > 0)
                                                        {
                                                            dtMfgResult = mfgResult.CopyToDataTable();
                                                            mfgID = Convert.ToInt32(dtMfgResult.Rows[0]["MfgID"]);
                                                        }
                                                        else
                                                        {
                                                            mfgID = 0;
                                                        }
                                                        //4.Owner
                                                        //Owner
                                                        if (dtOwner != null && dtOwner.Rows.Count > 0)
                                                        {

                                                            var ownerResult = from ownerRow in dtOwner.AsEnumerable()
                                                                              where ownerRow.Field<string>("OwnerName").Equals(ownerName, StringComparison.InvariantCultureIgnoreCase)
                                                                              select ownerRow;
                                                            if (ownerResult.Count() != 0)
                                                            {
                                                                dtOwnerResult = ownerResult.CopyToDataTable();
                                                                if (dtOwnerResult != null && dtOwnerResult.Rows.Count > 0)
                                                                {
                                                                    ownerID = Convert.ToInt32(dtOwnerResult.Rows[0]["OwnerID"]);
                                                                }
                                                                else
                                                                {
                                                                    ownerID = 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ownerID = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ownerID = 0;
                                                        }
                                                        //5. Get the BusinessUnitID
                                                        var buIDResult = from buRow in dtBusinessUnit.AsEnumerable()
                                                                         where buRow.Field<string>("BusinessUnit").Equals(buName, StringComparison.InvariantCultureIgnoreCase)
                                                                         select buRow;
                                                        dtBUResult = buIDResult.CopyToDataTable();
                                                        buID = Convert.ToInt32(dtBUResult.Rows[0]["BusinessUnitID"]);
                                                        # endregion

                                                        if (typeID > 0)
                                                        {
                                                            if (criticalityID > 0)
                                                            {
                                                                if (ownerID > 0)
                                                                {
                                                                    objAppl = new ApplicationBAL();
                                                                    objAppl.ApplName = dr["Application"].ToString();
                                                                    objAppl.Description = dr["Application"].ToString();
                                                                    objAppl.ApplCriticality = criticalityID;
                                                                    objAppl.OwnerID = ownerID;
                                                                    objAppl.ApplTypeID = typeID;
                                                                    objAppl.AppStatusID = appStatusID;
                                                                    objAppl.ApplManageID = mfgID;
                                                                    objAppl.Status = 1;
                                                                    objAppl.BUID = buID;

                                                                    int intAppl = 0;
                                                                    objAppl.ApplID = 0;
                                                                    intAppl = objAppl.exists();

                                                                    objAppl.CreatedBy = Convert.ToInt32(Session["UserID"]);

                                                                    if (intAppl != -1 && intAppl != 0)
                                                                        objAppl.ApplID = intAppl;

                                                                    if (intAppl != -1)
                                                                    {
                                                                        objAppl.Persist(DALCOperation.Insert);
                                                                    }
                                                                    else
                                                                    {
                                                                        //Application already exists
                                                                        dr["Status"] = "Failed";
                                                                        dr["Reason"] = "Application already exists with same Appl. Type and Owner.";
                                                                    }


                                                                    if (objAppl.ApplID < 0)
                                                                    {
                                                                        dr["Status"] = "Failed";
                                                                        dr["Reason"] = objAsset.MessageCode;
                                                                    }
                                                                    else
                                                                    {
                                                                        dr["Status"] = "Success";
                                                                        dr["Reason"] = "Success";
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr["Status"] = "Failed";
                                                                    dr["Reason"] = "Owner details missing";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dr["Status"] = "Failed";
                                                                dr["Reason"] = "Application criticality not defined in the system";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            dr["Status"] = "Failed";
                                                            dr["Reason"] = "Application type not defined in the system";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dr["Status"] = "Failed";
                                                        dr["Reason"] = "Application status not defined in the system";
                                                    }
                                                }
                                                else
                                                {
                                                    //Owner first Name is not in correct format
                                                    dr["Status"] = "Failed";
                                                    dr["Reason"] = "Owner First Name is not in correct format";
                                                }
                                            }
                                            else
                                            {
                                                //Owner Last Name is not in correct format
                                                dr["Status"] = "Failed";
                                                dr["Reason"] = "Owner Last Name is not in correct format";
                                            }
                                        }
                                        else
                                        {
                                            dr["Status"] = "Failed";
                                            dr["Reason"] = "Vendor is not in correct format";
                                        }
                                    }
                                    else
                                    {
                                        dr["Status"] = "Failed";
                                        dr["Reason"] = "Application type is not in correct format";
                                    }
                                }
                                else
                                {
                                    dr["Status"] = "Failed";
                                    dr["Reason"] = "Application criticality is not in correct format";
                                }
                            }
                            else
                            {
                                dr["Status"] = "Failed";
                                dr["Reason"] = "Application type is missing.";
                            }

                        }
                        catch (Exception ex)
                        {
                            dr["Status"] = "Failed";
                            dr["Reason"] = ex.Message;
                        }
                    }
                    else
                    {
                        dr["Status"] = "Failed";
                        dr["Reason"] = "Application Criticality is missing.";
                    }

                }
                else
                {
                    dr["Status"] = "Failed";
                    dr["Reason"] = "Application name is not in correct format";
                }
            }
            else
            {
                dr["Status"] = "Failed";
                dr["Reason"] = "Application name is missing.";
            }
        }
        //Serialize the dataset dsExcel data - with 8 tables
        //(Excel data, BudsinessUnit table, Site table, Location table, Manufacturer table, Model table, 
        //AssetType table and Asset Table)
        dsExcelData.AcceptChanges();
        SerializeExcelDataSetToXml(dsExcelData);
    }

    private void populateBU()
    {
        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        DataSet dsBU = objBU.retrieve();
        if (dsBU.Tables[0].Rows.Count == 1)
        {
            DataRow dr = dsBU.Tables[0].Rows[0];
            txtBU.Text = dr[DBFields.DBFIELD_BUSINESSUNIT].ToString();
            txtBU.Enabled = false;

        }
        else if (dsBU.Tables[0].Rows.Count > 1)
        {
            lblMessage.Text = "Multiple records returned.Only one Company should exist.";
            lblMessage.Visible = true;
            txtBU.Enabled = false;
        }
        else
        {
            lblMessage.Text = "Company is undefined.";
            lblMessage.Visible = true;
            txtBU.Enabled = true;
        }


    }

    private void populateGrid()
    {
        DataSet ds = DeserializeDataSource();
        if (ds != null)
        {
            //grdAsset.ClearDataSource();
            grdAsset.DataSource = ds.Tables[0];
            grdAsset.DataBind();

            // upload summary
            //total models
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable _assets = ds.Tables[0];
                int totalAssets = _assets.Rows.Count;
                int successAssets = (from cr in _assets.AsEnumerable()
                                     where cr.Field<string>("Status") == "Success"
                                     select cr).Count();
                int failedAssets = (totalAssets - successAssets);

                //grdUploadStatus -- show upload stats
                DataTable dtUploadStats = new DataTable();
                dtUploadStats.Columns.Add("ID");
                dtUploadStats.Columns.Add("Name");
                dtUploadStats.Columns.Add("Value");

                //Total Count
                DataRow drFS1 = dtUploadStats.NewRow();
                drFS1["ID"] = "1";
                drFS1["Name"] = "Total Applications found";
                drFS1["Value"] = totalAssets.ToString();
                dtUploadStats.Rows.Add(drFS1);
                //Success Count
                DataRow drFS2 = dtUploadStats.NewRow();
                drFS2["ID"] = "2";
                drFS2["Name"] = "No of Applications imported successfully";
                drFS2["Value"] = successAssets.ToString();
                dtUploadStats.Rows.Add(drFS2);

                //Failure Count
                DataRow drFS3 = dtUploadStats.NewRow();
                drFS3["ID"] = "3";
                drFS3["Name"] = "No of Application(s) failed to import";
                drFS3["Value"] = failedAssets.ToString();
                dtUploadStats.Rows.Add(drFS3);

                grdUploadStatus.DataSource = dtUploadStats;
                grdUploadStatus.DataBind();
                //Excel icon
                ibExportToExcel.Visible = true;


            }
        }
    }

    //Generic Function to convert the LINQ query result to the DataTable
    public DataTable LINQToDataTable<T>(IEnumerable<T> varlist, string tblName)
    {
        DataTable dtReturn = new DataTable(tblName);
        // column names 
        PropertyInfo[] oProps = null;
        if (varlist == null) return dtReturn;
        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others 
            //will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }
            DataRow dr = dtReturn.NewRow();
            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }
            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }

    public static void SerializeExcelDataSetToXml(DataSet ds)
    {
        String strFile;
        try
        {
            if (HttpContext.Current.Session["FileIDApps"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileIDApps"].ToString() + ".xml";

                XmlTextWriter xtw = new XmlTextWriter(strFile, null);
                ds.WriteXml(xtw);
                xtw.Close();
            }
        }
        catch (Exception ex)
        {
            throw ex;
            //lblFileUploadStatus.Text = ex.Message;
            //lblFileUploadStatus.Visible = true;
        }
    }

    DataSet DeserializeDataSource()
    {
        String strFile;
        DataSet ds = null;
        try
        {
            if (Session["FileIDApps"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["FileIDApps"].ToString() + ".xml");
                if (File.Exists(strFile))
                {
                    // Read the content of the file into a DataSet
                    XmlTextReader xtr = new XmlTextReader(strFile);
                    ds = new DataSet();
                    ds.ReadXml(xtr);
                    xtr.Close();
                }
            }
            else if (Session["tempFileIDApps"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["tempFileIDApps"].ToString() + ".xml");
                if (File.Exists(strFile))
                {
                    // Read the content of the file into a DataSet
                    XmlTextReader xtr = new XmlTextReader(strFile);
                    ds = new DataSet();
                    ds.ReadXml(xtr);
                    xtr.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            lblFileUploadStatus.Text = "Upload failed, check with Administrator";
            lblFileUploadStatus.Visible = true;
        }
        return ds;
    }

    private int GetAppStatusID(string AppStatus)
    {
        AppStatusBAL appStatusBAL = new AppStatusBAL();
        DataSet dsappStatus = appStatusBAL.retrieve();
        int id = 0;
        if (dsappStatus != null && dsappStatus.Tables.Count > 0 && dsappStatus.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow drAS in dsappStatus.Tables[0].Rows)
            {
                if (drAS[DBFields.DBFIELD_APP_STATUS].ToString().ToLower().CompareTo(AppStatus.ToLower()) == 0)
                {
                    id = Convert.ToInt32(drAS[DBFields.DBFIELD_APPLSTATUS_ID].ToString());
                    break;
                }
            }
        }

        return id;
    }

    #endregion
}



