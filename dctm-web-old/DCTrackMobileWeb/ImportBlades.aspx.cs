/*
File Name   :	ImportAsset.aspx

Description :	Used to Import the assets

Date created:	29 Aug 2011

Modification History:
***********************
CR		Name			Date			Description
New		Nayana M	    29/08/2011		File has been created.
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

public partial class ImportBlades : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.ManufacturerBAL objMfg;
    private iAssetTrack.BAL.AssetModelBAL objAM;
    private iAssetTrackBAL.AssetGroupBAL objAT;
    private iAssetTrack.BAL.AssetBAL objAsset;
    private iAssetTrack.BAL.OwnerBAL objOwner;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
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
        Session["PageHeader"] = "Import Blade Assets";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ImportBlades.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Import Blades' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Import Blades' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Import Blades' and Rights = '" + "Delete" + "'").Length != 0)
        {
            //ibDelete.Visible = true;
        }
        else
        {
            //ibDelete.Visible = false;
        }

        this.grdAsset.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());



        if (!IsPostBack)
        {
            Session["WebUploadFilePathBlades"] = null;
            populateBU();
            BindDummyRow();
            ibExportToExcel.Visible = false;
            Session["FileIDBlades"] = null;
            Session["tempFileIDBlades"] = null;
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

        //create folder if doesn't exists
        string savePath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/");
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        //guid for xml file ... 
        string fileID = Guid.NewGuid().ToString();
        if (HttpContext.Current.Session["FileIDBlades"] == null)
            HttpContext.Current.Session["FileIDBlades"] = fileID;
        String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssfff");
        //string fileName = ajaxFUAsset.FileName;
        string name = FileName.Substring(0, FileName.LastIndexOf("."));
        string extn = FileName.Substring(FileName.LastIndexOf(".") + 1);
        string assetFileName = name + "_" + timeStamp + "." + extn;

        string pathToCheck = savePath + assetFileName;
        // Create a temporary file name to use for checking duplicates.
        if (File.Exists(HttpContext.Current.Session["WebUploadFilePathBlades"].ToString() + FileName))
        {
            File.Copy(HttpContext.Current.Session["WebUploadFilePathBlades"].ToString() + FileName, savePath + assetFileName, true);
            File.Delete(HttpContext.Current.Session["WebUploadFilePathBlades"].ToString() + FileName);
        }

        try
        {
            dtData = importExcelData(savePath + assetFileName);
            if (dtData != null && dtData.Rows.Count > 0)
            {
                dsData.Tables.Add(dtData);
                if (HttpContext.Current.Session["ImportFilePathBlades"] == null)
                    HttpContext.Current.Session["ImportFilePathBlades"] = savePath + assetFileName;
                else
                {
                    HttpContext.Current.Session["ImportFilePathBlades"] = HttpContext.Current.Session["ImportFilePathBlades"].ToString() + "," + savePath + assetFileName;
                }
                return dsData.GetXml();
            }
            else
            {
                string msg = "Blades sheet not exists in " + FileName + " file.";
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
        //int assetTypeCount;
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
            dsExcel.Tables["Blades"].Columns.Add(dcID);
            dsExcel.Tables["Blades"].Columns.Add(dcStatus);
            dsExcel.Tables["Blades"].Columns.Add(dcreason);
            // put empty spaces or 0 for null data, in order to make them serailze
            int id = 0;
            id = 1;
            foreach (DataRow dr in dsExcel.Tables["Blades"].Rows)
            {

                for (int colIdx = 0; colIdx < dsExcel.Tables["Blades"].Columns.Count; colIdx++)
                {
                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "parentassettag")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "parentserialno")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "owner")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    //Change Serial no text to upper case
                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "serialNumber")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    //Change Manufacturer text to upper case
                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "manufacturer")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    //Change Model text to upper case 
                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "model")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    // adds zero in t0 columns and space to string columns,
                    // this is to aviod null values being skipped by xml 
                    // serialization.
                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "position")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName.ToLower() == "serialnumber")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }

                    if (dsExcel.Tables["Blades"].Columns[colIdx].ColumnName != "ID" &&
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
                if (HttpContext.Current.Session["FileIDBlades"] != null)
                {
                    strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileIDBlades"].ToString() + ".xml";
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
                            int curRowsCount = dsExcel.Tables["Blades"].Rows.Count + 1;
                            foreach (DataRow dr in dsPrevExcelData.Tables[0].Rows)
                            {
                                DataRow newRow = dsExcel.Tables["Blades"].NewRow();
                                newRow["Position"] = dr["Position"].ToString().Trim();
                                newRow["AssetTag"] = dr["AssetTag"].ToString().Trim();
                                newRow["SerialNumber"] = dr["SerialNumber"].ToString().Trim();
                                newRow["Manufacturer"] = dr["Manufacturer"].ToString().Trim();
                                newRow["Model"] = dr["Model"].ToString().Trim();
                                newRow["HostName"] = dr["HostName"].ToString().Trim();
                                newRow["AssetName"] = dr["AssetName"].ToString().Trim();
                                newRow["ParentSerialNo"] = dr["ParentSerialNo"].ToString().Trim();
                                newRow["ParentAssetTag"] = dr["ParentAssetTag"].ToString().Trim();
                                newRow["ExternalID"] = dr["ExternalID"].ToString().Trim();
                                newRow["Owner"] = dr["Owner"].ToString().Trim();
                                newRow["Orientation"] = " ";
                                newRow["ID"] = curRowsCount++;
                                newRow["Status"] = "";
                                newRow["Reason"] = "";

                                dsExcel.Tables["Blades"].Rows.Add(newRow);
                            }
                            dsExcel.AcceptChanges();
                            XmlTextWriter xtw = new XmlTextWriter(strFile, null);
                            dsExcel.WriteXml(xtw);
                            xtw.Close();
                        }
                        SerializeExcelDataSetToXml(dsExcel);
                    }
                    else
                    {
                        // first time 
                        XmlTextWriter xtw = new XmlTextWriter(strFile, null);
                        dsExcel.WriteXml(xtw);
                        xtw.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
                //lblFileUploadStatus.Text = ex.Message;
                //lblFileUploadStatus.Visible = true;
            }

            dtExcel = dsExcel.Tables["Blades"];
            var recCount = 0;

            //Manufacturer Count
            dtManufacturer = new DataView(dtExcel).ToTable(true, new string[] { "Manufacturer" });
            recCount = (from cr in dtManufacturer.AsEnumerable()
                        where cr.Field<string>("Manufacturer").Trim() != ""
                        group cr by new
                        {
                            Manufacturer = cr.Field<String>("Manufacturer").ToLower(),
                        }
                            into grp
                        select grp.First()).Count();
            manufacturerCount = (int)recCount;

            DataRow drFS5 = dtFileStats.NewRow();
            drFS5["ID"] = "5";
            drFS5["Name"] = "No of Manufacturers found";
            drFS5["Value"] = manufacturerCount.ToString();
            dtFileStats.Rows.Add(drFS5);

            //Model Count
            dtModel = new DataView(dtExcel).ToTable(true, new string[] { "Model", "Manufacturer" });
            recCount = (from cr in dtModel.AsEnumerable()
                        where cr.Field<string>("Model").Trim() != ""
                        group cr by new
                        {
                            Model = cr.Field<String>("Model").ToLower(),
                        }
                            into grp
                        select grp.First()).Count();
            modelCount = (int)recCount;

            DataRow drFS6 = dtFileStats.NewRow();
            drFS6["ID"] = "6";
            drFS6["Name"] = "No of Blade Models found";
            drFS6["Value"] = modelCount.ToString();
            dtFileStats.Rows.Add(drFS6);

            ////Asset Type Count
            //dtAssetType = new DataView(dtExcel).ToTable(true, new string[] { "AssetType" });
            //recCount = (from cr in dtAssetType.AsEnumerable()
            //            where cr.Field<string>("AssetType").Trim() != ""
            //            group cr by new
            //            {
            //                AssetType = cr.Field<String>("AssetType").ToLower(),
            //            }
            //                into grp
            //                select grp.First()).Count();

            //assetTypeCount = (int)recCount;

            //DataRow drFS7 = dtFileStats.NewRow();
            //drFS7["ID"] = "7";
            //drFS7["Name"] = "No of Blade Types found";
            //drFS7["Value"] = assetTypeCount.ToString();
            //dtFileStats.Rows.Add(drFS7);

            //Asset Count
            assetCount = dtExcel.Rows.Count;

            DataRow drFS8 = dtFileStats.NewRow();
            drFS8["ID"] = "8";
            drFS8["Name"] = "Total No of Blade Assets";
            drFS8["Value"] = assetCount.ToString();
            dtFileStats.Rows.Add(drFS8);
            dtFileStats.AcceptChanges();

        }
        return dtFileStats;
    }

    protected void ShowFileData()
    {

        int assetCount;
        //int assetTypeCount;
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
        DataTable dtRack = new DataTable();
        DataTable dtAssetType = new DataTable();
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

        //Manufacturer Count
        dtMfg = new DataView(dtExcel).ToTable(true, new string[] { "Manufacturer" });
        recCount = (from cr in dtMfg.AsEnumerable()
                    where cr.Field<string>("Manufacturer").Trim() != ""
                    select cr).Count();
        manufacturerCount = (int)recCount;

        DataRow drFS5 = dtFileStats.NewRow();
        drFS5["ID"] = "5";
        drFS5["Name"] = "No of Manufacturers found";
        drFS5["Value"] = manufacturerCount.ToString();
        dtFileStats.Rows.Add(drFS5);

        //Model Count
        dtAssetModel = new DataView(dtExcel).ToTable(true, new string[] { "Model", "Manufacturer" });
        recCount = (from cr in dtAssetModel.AsEnumerable()
                    where cr.Field<string>("Model").Trim() != ""
                    select cr).Count();
        modelCount = (int)recCount;

        DataRow drFS6 = dtFileStats.NewRow();
        drFS6["ID"] = "6";
        drFS6["Name"] = "No of Blade Asset Models found";
        drFS6["Value"] = modelCount.ToString();
        dtFileStats.Rows.Add(drFS6);

        ////Asset Type Count
        //dtAssetType = new DataView(dtExcel).ToTable(true, new string[] { "AssetType" });
        //recCount = (from cr in dtAssetType.AsEnumerable()
        //            where cr.Field<string>("AssetType").Trim() != ""
        //            select cr).Count();
        //assetTypeCount = (int)recCount;

        //DataRow drFS7 = dtFileStats.NewRow();
        //drFS7["ID"] = "7";
        //drFS7["Name"] = "No of Blade Asset Types found";
        //drFS7["Value"] = assetTypeCount.ToString();
        //dtFileStats.Rows.Add(drFS7);

        //Asset Count
        assetCount = dtExcel.Rows.Count;

        DataRow drFS8 = dtFileStats.NewRow();
        drFS8["ID"] = "8";
        drFS8["Name"] = "Total No of Blade Assets";
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

                    case "Blades$":

                        sheet = "Blades$";
                        //OleDbCommand cmdDel = new OleDbCommand(" Delete from [" + sheet + "] " + 
                        //    " where HostName IS  NULL AND SerialNumber IS  NULL AND Site IS  NULL AND Manufacturer IS  NULL " +
                        //    " AND Model IS  NULL AND AssetType IS  NULL AND Room IS  NULL AND Row IS  NULL " +
                        //    " AND Rack IS  NULL AND StartPosition IS  NULL AND NoofRus IS  NULL AND [Rack/Stand] IS  NULL ",conn);
                        //cmdDel.CommandType = CommandType.Text;
                        //int count = cmdDel.ExecuteNonQuery();

                        cmd = new OleDbCommand("SELECT * FROM [" + sheet + "] ", conn);
                        cmd.CommandType = CommandType.Text;
                        outputTable = new DataTable("Blades");
                        output.Tables.Add(outputTable);
                        new OleDbDataAdapter(cmd).Fill(outputTable);
                        //delete blank rows
                        output.Tables["Blades"].BeginLoadData();
                        (from cr in output.Tables["Blades"].AsEnumerable()
                         where cr.Field<string>("HostName") == null && cr.Field<string>("SerialNumber") == null &&
                         cr.Field<string>("Manufacturer") == null &&
                         cr.Field<string>("Model") == null &&
                         cr.Field<string>("Position") == null && cr.Field<string>("Owner") == null
                         && cr.Field<string>("Orientation") == null
                         select cr).ToList().ForEach(cr => cr.Delete());
                        output.Tables["Blades"].EndLoadData();
                        output.Tables["Blades"].AcceptChanges();
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
        Session["WebUploadFilePathBlades"] = e.FolderPath;
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        bool bIsBUInserted = false;

        bIsBUInserted = insertBU();

        if (bIsBUInserted == true)
        {
            //Manufacturers
            insertManufacturer();
            //Models
            insertModel();
            // Asset Types
            insertAssetType();
            //Owner/Custodian
            insertOwner();
            // Assets
            insertAssets();
            if (Session["FileIDBlades"] != null)
                Session["tempFileIDBlades"] = Session["FileIDBlades"].ToString();
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
        try
        {
            //Get the AssetTypeIDs and AssetTypes into table dtAllAssetTypeNames
            var allOwners = (from rows in dsAllOwners.Tables[0].AsEnumerable()
                             orderby rows.Field<int>("OwnerID")
                             select new
                             {
                                 OwnerID = rows.Field<int>("OwnerID"),
                                 OwnerName = rows.Field<string>("OwnerLastName").Trim() + "," + rows.Field<string>("OwnerFirstName").Trim(),
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
            //Get the new AssetTypes from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();
            DataTable dtTable = new DataTable();
            DataTable dtResult = new DataTable();
            DataTable dtOwnersInExcel = new DataTable();
            DataTable dtOwnersResult = new DataTable();
            dtTable = dsExcelData.Tables["Blades"];



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
            ImportBlades.SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    private void ResetAll()
    {
        Session["WebUploadFilePathBlades"] = null;
        ibExportToExcel.Visible = true;
        Session["FileIDBlades"] = null;
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
        book.Worksheets.Add("Blades");

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

    public void insertModel()
    {
        string modelName = string.Empty;
        string mfgName = string.Empty;
        Dictionary<int, string> dictModelInserted = new Dictionary<int, string>();
        //Retreive all the models from the database.
        DataSet dsAllModels = new DataSet();
        DataTable dtAllModelNames = new DataTable();
        objAM = new iAssetTrack.BAL.AssetModelBAL();

        dsAllModels = objAM.retrieve();

        try
        {
            //Get the modelIDs and ModelName into table dtAllModelNames
            var allModels = (from rows in dsAllModels.Tables[0].AsEnumerable()
                             where rows.Field<bool>("IsBlade") == true
                             orderby rows.Field<int>("ModelID")
                             select new
                             {
                                 modelID = rows.Field<int>("ModelID"),
                                 modelName = rows.Field<string>("ModelName"),
                                 MfgID = rows.Field<int>("MfgID"),

                             });
            dtAllModelNames = LINQToDataTable(allModels, "dtModel");
            if (dtAllModelNames.Columns.Count == 0)
            {
                dtAllModelNames.Columns.Add("modelID", typeof(int));
                dtAllModelNames.Columns.Add("modelName", typeof(string));
                dtAllModelNames.Columns.Add("MfgID", typeof(int));

                dtAllModelNames.AcceptChanges();
            }
            //Get the new models from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();

            dsExcelData.Tables.Add(dtAllModelNames);
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

    public void insertAssetType()
    {
        string assetTypeName = string.Empty;
        Dictionary<int, string> dictAssetTypesInserted = new Dictionary<int, string>();

        //Retreive all the AssetTypes from the database.
        DataSet dsAllATs = new DataSet();
        DataTable dtAllATNames = new DataTable();
        objAT = new iAssetTrackBAL.AssetGroupBAL();
        dsAllATs = objAT.retrieveAllAssetGroup();
        try
        {
            //Get the AssetTypeIDs and AssetTypes into table dtAllAssetTypeNames
            var allATs = (from rows in dsAllATs.Tables[0].AsEnumerable()
                          where rows.Field<string>("AssetGroup").ToLower().Contains("blade") || rows.Field<string>("AssetGroup").ToLower().Contains("module")
                          orderby rows.Field<int>("AssetGroupID")
                          select new
                          {
                              AssetGroupID = rows.Field<int>("AssetGroupID"),
                              AssetGroup = rows.Field<string>("AssetGroup")

                          });
            dtAllATNames = LINQToDataTable(allATs, "dtAssetType");
            if (dtAllATNames.Columns.Count == 0)
            {
                dtAllATNames.Columns.Add("AssetGroupID", typeof(int));
                dtAllATNames.Columns.Add("AssetGroup", typeof(string));

                dtAllATNames.AcceptChanges();
            }
            //Get the new AssetTypes from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();

            dsExcelData.Tables.Add(dtAllATNames);
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
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
                //lblMessage.Text = ex.Message;
                //lblMessage.Visible = true;
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

    public void insertAssets()
    {
        #region Get all tables from dataset
        string assetName = string.Empty;
        string mfgName = string.Empty;
        string modelName = string.Empty;
        string assetType = string.Empty;
        string buName = txtBU.Text;
        string serialNo = string.Empty;
        string ownerName = string.Empty;
        string mountType = string.Empty;
        string ownerLName = string.Empty;
        string ownerFName = string.Empty;
        string orientation = string.Empty;
        bool isBlade = false;
        int bladeRCount = 0;
        int bladeCCount = 0;
        int enclFRCount = 0;
        int enclFCCount = 0;
        int enclRRCount = 0;
        int enclRCCount = 0;
        int startPos;
        int assetTypeID = 0;
        int assetModelID = 0;
        int ownerID;
        int buID;
        string assetTag = string.Empty;
        string hostName = string.Empty;
        //Get all tables from the Import Asset Excel
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtBusinessUnit = new DataTable();
        DataTable dtAssetType = new DataTable();
        DataTable dtAssetModel = new DataTable();
        DataTable dtAsset = new DataTable();
        DataTable dtMfg = new DataTable();
        DataTable dtOwner = new DataTable();
        //Result tables
        DataTable dtBUResult = new DataTable();
        DataTable dtATResult = new DataTable();
        DataTable dtAMResult = new DataTable();
        DataTable dtMResult = new DataTable();
        DataTable dtOwnerResult = new DataTable();

        objAsset = new iAssetTrack.BAL.AssetBAL();

        dtAsset = dsExcelData.Tables["Blades"];
        dtBusinessUnit = dsExcelData.Tables["dtBusinessUnit"];
        dtAssetModel = dsExcelData.Tables["dtModel"];
        dtAssetType = dsExcelData.Tables["dtAssetType"];
        dtMfg = dsExcelData.Tables["dtManufacturer"];
        dtOwner = dsExcelData.Tables["dtOwner"];

        #endregion

        foreach (DataRow dr in dtAsset.Rows)
        {
            if (!string.IsNullOrEmpty(dr["ParentSerialNo"].ToString().Trim()) || !string.IsNullOrEmpty(dr["ParentAssetTag"].ToString().Trim()))
            {

                if (!string.IsNullOrEmpty(dr["Manufacturer"].ToString().Trim()) && !string.IsNullOrEmpty(dr["Model"].ToString().Trim()))
                {

                    try
                    {
                        #region Assign Variables

                        mfgName = (dr["Manufacturer"].ToString()).Trim();
                        modelName = (dr["Model"].ToString()).Trim();
                        hostName = dr["HostName"].ToString().Trim();
                        assetName = dr["AssetName"].ToString().Trim();
                        assetTag = dr["AssetTag"].ToString().Trim();
                        serialNo = dr["SerialNumber"].ToString();
                        orientation = dr["Orientation"].ToString();

                        if (orientation == "NULL" || orientation == null || orientation.Trim() == string.Empty)
                            orientation = "Front";

                        if (!string.IsNullOrEmpty(dr["Position"].ToString().Trim()))
                        {
                            startPos = int.Parse(dr["Position"].ToString().Trim());
                        }
                        else
                        {
                            startPos = 0;
                        }


                        #endregion

                        //Serial No check
                        Regex regx = new Regex(@"^[\w0-9\-\.]+(\s?[\w0-9\-\.]+)*$");
                        if (regx.IsMatch(serialNo.Trim()) && serialNo.Trim().Length <= 100)
                        {
                            //no need to check host as selected host name must be in host table.
                            //Asset Name check
                            if (string.IsNullOrEmpty(assetName.Trim()) || (regx.IsMatch(assetName.Trim()) && assetName.Trim().Length <= 100))
                            {
                                //currently excel import excel supports single host names only.
                                if (string.IsNullOrEmpty(hostName.Trim()) || (regx.IsMatch(hostName.Trim()) && hostName.Trim().Length <= 100))
                                {

                                    regx = new Regex(@"^[A-Za-z0-9]+$");
                                    if (string.IsNullOrEmpty(assetTag.Trim()) || (regx.IsMatch(assetTag.Trim()) && assetTag.Trim().Length <= 24))
                                    {
                                        #region Get Required IDs

                                        //1. Get the MfgID
                                        var assetMfgResult = from mRow in dtMfg.AsEnumerable()
                                                             where mRow.Field<string>("MfgName").Equals(mfgName, StringComparison.InvariantCultureIgnoreCase)
                                                             select mRow;
                                        int mfgID;
                                        if (assetMfgResult.Count() > 0)
                                        {
                                            dtMResult = assetMfgResult.CopyToDataTable();
                                            mfgID = Convert.ToInt32(dtMResult.Rows[0]["MfgID"]);
                                        }
                                        else
                                        {
                                            mfgID = 0;
                                        }
                                        //2. Get the ModelID
                                        var assetModelResult = from amRow in dtAssetModel.AsEnumerable()
                                                               where amRow.Field<string>("ModelName").Equals(modelName, StringComparison.InvariantCultureIgnoreCase)
                                                               && amRow.Field<string>("MfgID").Equals(mfgID.ToString(), StringComparison.InvariantCultureIgnoreCase)

                                                               select amRow;
                                        if (assetModelResult.Count() > 0)
                                        {
                                            dtAMResult = assetModelResult.CopyToDataTable();
                                            assetModelID = Convert.ToInt32(dtAMResult.Rows[0]["ModelID"]);

                                        }
                                        else
                                        {
                                            assetModelID = 0;
                                        }

                                        //3. Asset Model
                                        //Get below Info from Model
                                        // Mount Type, ModelType/Asset Type, No Of RUs
                                        AssetModelBAL modelBAL = new AssetModelBAL();
                                        if (assetModelID > 0)
                                        {
                                            modelBAL.ModelID = assetModelID;
                                            DataSet dsModel = modelBAL.retrieve();
                                            if (dsModel != null && dsModel.Tables.Count > 0 && dsModel.Tables[0].Rows.Count > 0)
                                            {
                                                isBlade = Convert.ToBoolean(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_IS_BLADE]);
                                                assetType = dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_TYPE].ToString();
                                                mountType = dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_MOUNT_TYPE].ToString();

                                                if (isBlade)
                                                {
                                                    if (!string.IsNullOrEmpty(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_ROW_COUNT].ToString()))
                                                        bladeRCount = Convert.ToInt16(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_ROW_COUNT].ToString());
                                                    else
                                                        bladeRCount = 0;

                                                    if (!string.IsNullOrEmpty(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_ROW_COUNT].ToString()))
                                                        bladeCCount = Convert.ToInt16(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_BLADE_COL_COUNT].ToString());
                                                    else
                                                        bladeCCount = 0;
                                                }

                                                var assetTypeResult = from atRow in dtAssetType.AsEnumerable()
                                                                      where atRow.Field<string>("AssetGroup").Equals(assetType, StringComparison.InvariantCultureIgnoreCase)

                                                                      select atRow;
                                                dtATResult = assetTypeResult.CopyToDataTable();
                                                assetTypeID = Convert.ToInt32(dtATResult.Rows[0]["AssetGroupID"]);
                                            }
                                        }

                                        //4. Get the BusinessUnitID
                                        var buIDResult = from buRow in dtBusinessUnit.AsEnumerable()
                                                         where buRow.Field<string>("BusinessUnit").Equals(buName, StringComparison.InvariantCultureIgnoreCase)
                                                         select buRow;
                                        dtBUResult = buIDResult.CopyToDataTable();
                                        buID = Convert.ToInt32(dtBUResult.Rows[0]["BusinessUnitID"]);


                                        //5. Get parent Asset details
                                        AssetBAL objGetParent = new AssetBAL();
                                        DataRow drParent = objGetParent.GetParentAssetDetails(dr["ParentSerialNo"].ToString().Trim(), dr["ParentAssetTag"].ToString().Trim());
                                        if (bool.Parse(Session["TenantUser"].ToString()))
                                        {
                                            ArrayList assignedAssets = new ArrayList();
                                            UserBAL objUser = new UserBAL();
                                            objUser.UserID = Convert.ToInt32(Session["UserID"]);
                                            DataSet dsTenant = objUser.retrieveTenantDetails();
                                            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
                                            {
                                                TenantBAL objTenant = new TenantBAL();
                                                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                                                DataSet dsTA = objTenant.retrieveAssetAssignmentList();
                                                if (dsTA.Tables.Count > 0 && dsTA.Tables[0].Rows.Count > 0)
                                                {
                                                    foreach (DataRow drA in dsTA.Tables[0].Rows)
                                                    {
                                                        if (!assignedAssets.Contains((drA[DBFields.DBFIELD_ASSETID].ToString())))
                                                            assignedAssets.Add(drA[DBFields.DBFIELD_ASSETID].ToString());
                                                    }
                                                }
                                                if (assignedAssets.Count > 0)
                                                {
                                                    if (!assignedAssets.Contains(drParent[DBFields.DBFIELD_ASSETID].ToString()))
                                                        drParent = null;
                                                }
                                                else
                                                {
                                                    drParent = null;
                                                }

                                            }

                                        }
                                        //get parent enclosure details like row X column count
                                        if (drParent != null)
                                        {
                                            int parentmodelID = Convert.ToInt32(drParent[DBFields.DBFIELD_MODELID].ToString());

                                            AssetModelBAL parentModelBAL = new AssetModelBAL();
                                            parentModelBAL.ModelID = parentmodelID;
                                            DataSet dsParentModel = parentModelBAL.retrieve();
                                            if (dsParentModel != null && dsParentModel.Tables.Count > 0 && dsParentModel.Tables[0].Rows.Count > 0)
                                            {
                                                if (!string.IsNullOrEmpty(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_ROW_COUNT].ToString()))
                                                    enclFRCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_ROW_COUNT].ToString());
                                                else
                                                    enclFRCount = 0;
                                                if (!string.IsNullOrEmpty(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_COL_COUNT].ToString()))
                                                    enclFCCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_FRONT_COL_COUNT].ToString());
                                                else
                                                    enclFCCount = 0;
                                                if (!string.IsNullOrEmpty(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_ROW_COUNT].ToString()))
                                                    enclRRCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_ROW_COUNT].ToString());
                                                else
                                                    enclRRCount = 0;

                                                if (!string.IsNullOrEmpty(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_COL_COUNT].ToString()))
                                                    enclRCCount = Convert.ToInt16(dsParentModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_ENCL_REAR_COL_COUNT].ToString());
                                                else
                                                    enclRCCount = 0;
                                            }
                                        }
                                        #endregion
                                        //Owner
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


                                                //external id
                                                string externalId = dr["ExternalID"].ToString();

                                                if (assetModelID > 0)
                                                {
                                                    if (isBlade)
                                                    {
                                                        if (bladeCCount > 0 && bladeRCount > 0)
                                                        {
                                                            if (drParent != null && drParent.ItemArray.Length > 0)
                                                            {
                                                                if ((orientation.ToLower().CompareTo("front") == 0 && enclFRCount > 0 && enclFCCount > 0) ||
                                                                    (orientation.ToLower().CompareTo("rear") == 0 && enclRRCount > 0 && enclRCCount > 0))
                                                                {


                                                                    if (startPos > 0)
                                                                    {
                                                                        // need to vertify whethert this blade can be placed in selected parent
                                                                        int orientationID = GetOrientationID(orientation);
                                                                        if (orientationID > 0 && (orientation.ToLower().CompareTo("front") == 0 || orientation.ToLower().CompareTo("rear") == 0))
                                                                        {
                                                                            bool positionexists = false;
                                                                            if (orientation.ToLower().CompareTo("front") == 0)
                                                                            {
                                                                                positionexists = bladePositionExists(int.Parse(drParent["AssetID"].ToString()), orientation,
                                                                                    bladeRCount, bladeCCount, enclFRCount, enclFCCount, startPos);
                                                                            }
                                                                            else
                                                                            {
                                                                                positionexists = bladePositionExists(int.Parse(drParent["AssetID"].ToString()), orientation,
                                                                                    bladeRCount, bladeCCount, enclRRCount, enclRCCount, startPos);
                                                                            }

                                                                            if (positionexists)
                                                                            {
                                                                                objAsset = new iAssetTrack.BAL.AssetBAL();
                                                                                objAsset.AssetID = 0;
                                                                                objAsset.RefNumber = serialNo;
                                                                                objAsset.HostName = hostName;
                                                                                objAsset.AssetName = assetName;
                                                                                objAsset.StartPos = startPos;
                                                                                objAsset.NoOfRUs = 0;
                                                                                objAsset.AssetTypeId = assetTypeID;
                                                                                objAsset.ModelID = assetModelID;
                                                                                if (drParent != null && !string.IsNullOrEmpty(drParent["LastSeenLocationID"].ToString()))
                                                                                {
                                                                                    objAsset.LastSeenLocationID = int.Parse(drParent["LastSeenLocationID"].ToString());
                                                                                    objAsset.DefaultLocationID = int.Parse(drParent["LastSeenLocationID"].ToString());
                                                                                }
                                                                                else
                                                                                {
                                                                                    objAsset.LastSeenLocationID = int.Parse(drParent["LastSeenLocationID"].ToString());
                                                                                    objAsset.DefaultLocationID = int.Parse(drParent["LastSeenLocationID"].ToString());
                                                                                }

                                                                                objAsset.TechID = 0;
                                                                                objAsset.RackOrStand = mountType;
                                                                                objAsset.BusinessUnitID = int.Parse(drParent["BusinessUnitID"].ToString());
                                                                                objAsset.PrimarySiteID = int.Parse(drParent["PrimarySiteID"].ToString());
                                                                                objAsset.AssetCreatedDate = DateTime.Now;
                                                                                objAsset.AssetCreatedBy = Convert.ToInt32(Session["UserID"]);
                                                                                objAsset.CurrentOwnerID = ownerID;
                                                                                objAsset.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                                                                                objAsset.OS = string.Empty;
                                                                                objAsset.CPU = string.Empty;
                                                                                objAsset.CPUCount = 0;
                                                                                objAsset.CPUCore = string.Empty;
                                                                                objAsset.IsImport = true;
                                                                                objAsset.SerialNoModelCheck = (Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ImportAssetUniqueFilter"].ToString()) == 1 ? true : false);
                                                                                objAsset.AssetTAG = assetTag;
                                                                                objAsset.Orientation = "Front";
                                                                                objAsset.ApplicationsList = "";
                                                                                objAsset.AssetParentID = int.Parse(drParent["AssetID"].ToString());
                                                                                objAsset.IsParent = false;
                                                                                objAsset.ExternalID = externalId;
                                                                                objAsset.Persist(DALCOperation.Update);
                                                                                if (objAsset.Result == -1)
                                                                                {
                                                                                    dr["Status"] = "Failed";
                                                                                    CommonBAL objCommon = new CommonBAL();
                                                                                    MessageBAL objMessage = new MessageBAL();
                                                                                    objMessage.MessageCode = objAsset.MessageCode;

                                                                                    if (!string.IsNullOrEmpty(objAsset.MessageCode))
                                                                                    {
                                                                                        string displayMsg = objMessage.retrieve();
                                                                                        if (!string.IsNullOrEmpty(displayMsg))
                                                                                            dr["Reason"] = displayMsg;
                                                                                        else
                                                                                            dr["Reason"] = objAsset.MessageCode;
                                                                                    }

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
                                                                                dr["Reason"] = "Bay position not available";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            dr["Status"] = "Failed";
                                                                            dr["Reason"] = "Orientation must be Front or Rear";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dr["Status"] = "Failed";
                                                                        dr["Reason"] = "Bay Position is not defined";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dr["Status"] = "Failed";
                                                                    dr["Reason"] = "Parent Enclosure row-column details missing";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dr["Status"] = "Failed";
                                                                dr["Reason"] = "Parent Asset not found.";
                                                            }

                                                        }
                                                        else
                                                        {
                                                            dr["Status"] = "Failed";
                                                            dr["Reason"] = "Blade model row-column details mising";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        dr["Status"] = "Failed";
                                                        dr["Reason"] = "Asset Model is not Blade/Module type";
                                                    }
                                                }
                                                else
                                                {
                                                    dr["Status"] = "Failed";
                                                    dr["Reason"] = "Asset Model not defined in the system";
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
                                        dr["Reason"] = "Asset Tag is not in correct format";
                                    }
                                }
                                else
                                {
                                    dr["Status"] = "Failed";
                                    dr["Reason"] = "Host Name is not in correct format";
                                }
                            }
                            else
                            {
                                dr["Status"] = "Failed";
                                dr["Reason"] = "Asset Name is not in correct format";
                            }
                        }
                        else
                        {
                            dr["Status"] = "Failed";
                            dr["Reason"] = "Serial Number is not in correct format";
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
                    dr["Reason"] = "Manufacturer & Model details missing";
                }

            }
            else
            {
                dr["Status"] = "Failed";
                dr["Reason"] = "Either Parent Serial No or Parent Asset Tag is required.";
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
                drFS1["Name"] = "Total Blade Assets found";
                drFS1["Value"] = totalAssets.ToString();
                dtUploadStats.Rows.Add(drFS1);
                //Success Count
                DataRow drFS2 = dtUploadStats.NewRow();
                drFS2["ID"] = "2";
                drFS2["Name"] = "No of Blade Assets imported successfully";
                drFS2["Value"] = successAssets.ToString();
                dtUploadStats.Rows.Add(drFS2);

                //Failure Count
                DataRow drFS3 = dtUploadStats.NewRow();
                drFS3["ID"] = "3";
                drFS3["Name"] = "No of Blade Assets failed to import";
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
            if (HttpContext.Current.Session["FileIDBlades"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileIDBlades"].ToString() + ".xml";

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
            if (Session["FileIDBlades"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["FileIDBlades"].ToString() + ".xml");
                if (File.Exists(strFile))
                {
                    // Read the content of the file into a DataSet
                    XmlTextReader xtr = new XmlTextReader(strFile);
                    ds = new DataSet();
                    ds.ReadXml(xtr);
                    xtr.Close();
                }
            }
            else if (Session["tempFileIDBlades"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["tempFileIDBlades"].ToString() + ".xml");
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
            lblFileUploadStatus.Text = "Import file is corrupted, check error log for more information";
            lblFileUploadStatus.Visible = true;
        }
        return ds;
    }

    private int GetOrientationID(string Orientation)
    {
        OrientationBAL oBAL = new OrientationBAL();
        DataSet dsOrientation = oBAL.retrieve();
        int id = 0;
        if (dsOrientation != null && dsOrientation.Tables.Count > 0 && dsOrientation.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow drO in dsOrientation.Tables[0].Rows)
            {
                if (drO[DBFields.DBFIELD_ORIENTATION_NAME].ToString().ToLower().CompareTo(Orientation.ToLower()) == 0)
                {
                    id = Convert.ToInt32(drO[DBFields.DBFIELD_ORIENTATION_ID].ToString());
                    break;
                }
            }
        }

        return id;
    }

    private bool bladePositionExists(int ParentAssetID, string Orientation, int BladeRowCount, int BladeColCount, int EnclRowCount, int EnclColCount, int StartPos)
    {
        bool retval = false;
        bool throwError = false;
        if (BladeRowCount > EnclRowCount || BladeColCount > EnclColCount)
        {
            throwError = true;
        }
        else
        {
            for (int enclr = 1; enclr <= EnclRowCount; enclr++)
            {
                if (StartPos <= EnclColCount * enclr)
                {
                    for (int bc = 1; bc <= BladeColCount; bc++)
                    {
                        if ((StartPos + bc - 1) > (EnclColCount * enclr))
                        {
                            throwError = true;
                        }

                    }
                }
            }
        }

        if (!throwError)
        {
            List<int> enclPos = GetEnclPositions(ParentAssetID, Orientation);

            for (int r = 0; r < BladeRowCount; r++)
            {
                for (int c = 0; c < BladeColCount; c++)
                {
                    if (!enclPos.Contains((StartPos + (EnclColCount * r) + c)))
                    {
                        throwError = true;
                        break;
                    }
                }
            }
        }

        if (throwError)
            retval = false;
        else
            retval = true;

        return retval;
    }


    private List<int> GetEnclPositions(int EnclID, string Orientation)
    {
        string positions = string.Empty;
        List<int> arrPos = new List<int>();

        //if asset is blade and parent selected (in case of rack, parent selection is compulsory)
        AssetBAL assetBAL = new AssetBAL();
        assetBAL.AssetID = Convert.ToInt32(EnclID);
        DataSet dsAsset = assetBAL.retrieveEnclPositions();
        if (dsAsset != null && dsAsset.Tables.Count > 0 && dsAsset.Tables[0].Rows.Count > 0)
        {
            switch (Orientation)
            {
                case "Front":
                    positions = dsAsset.Tables[0].Rows[0][DBFields.DBFIELD_FRONT_POSITIONS].ToString();
                    break;
                case "Rear":
                    positions = dsAsset.Tables[0].Rows[0][DBFields.DBFIELD_REAR_POSITIONS].ToString();
                    break;

            }

            if (!string.IsNullOrEmpty(positions))
            {
                for (int i = 1; i <= positions.Length; i++)
                {
                    if (positions[i - 1] == 'P' || positions[i - 1] == 'V')
                        arrPos.Add(i);
                }
            }
        }

        return arrPos;
    }

    #endregion
}



