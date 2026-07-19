/*
File Name   :	ImpotAssetModels.aspx

Description :	Used to Import the Asset Models

Date created:	19 May 2018

Modification History:
***********************
CR		Name			Date			Description
New		KJB             19 May 2018		File has been created.
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

public partial class ImportAssetModels : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.ManufacturerBAL objMfg;
    private iAssetTrack.BAL.AssetModelBAL objAM;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    #endregion

    #region " Page Event Methods "

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdAssetModel.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdAssetModel_ItemCommand);
        pagerControl = grdAssetModel.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdAssetModel.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Import Asset Models";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ImportAssetModels.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Import Asset Models' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Import Asset Models' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        this.grdAssetModel.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        if (!IsPostBack)
        {
            Session["WebUploadFilePathModels"] = null;
            populateBU();
            BindDummyRow();
            ibExportToExcel.Visible = false;
            Session["FileIDModels"] = null;
            Session["tempFileIDModels"] = null;
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
        if (HttpContext.Current.Session["FileIDModels"] == null)
            HttpContext.Current.Session["FileIDModels"] = fileID;
        String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssfff");
        //string fileName = ajaxFUAsset.FileName;
        string name = FileName.Substring(0, FileName.LastIndexOf("."));
        string extn = FileName.Substring(FileName.LastIndexOf(".") + 1);
        string assetFileName = name + "_" + timeStamp + "." + extn;

        string pathToCheck = savePath + assetFileName;
        // Create a temporary file name to use for checking duplicates.
        if (File.Exists(HttpContext.Current.Session["WebUploadFilePathModels"].ToString() + FileName))
        {
            File.Copy(HttpContext.Current.Session["WebUploadFilePathModels"].ToString() + FileName, savePath + assetFileName, true);
            File.Delete(HttpContext.Current.Session["WebUploadFilePathModels"].ToString() + FileName);
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
                string msg = "AssetModels sheet not exists in " + FileName + " file.";
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
        int modelCount;
        int manufacturerCount;
        DataSet dsExcel = new DataSet();
        DataTable dtExcel = new DataTable();
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
            dsExcel.Tables["AssetModels"].Columns.Add(dcID);
            dsExcel.Tables["AssetModels"].Columns.Add(dcStatus);
            dsExcel.Tables["AssetModels"].Columns.Add(dcreason);
            // put empty spaces or 0 for null data, in order to make them serailze
            int id = 0;
            id = 1;
            foreach (DataRow dr in dsExcel.Tables["AssetModels"].Rows)
            {

                for (int colIdx = 0; colIdx < dsExcel.Tables["AssetModels"].Columns.Count; colIdx++)
                {
                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "modelname")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "manufacturer")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "modeltype")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "mounttype")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "airflowdirection")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }
                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "Connector_Type_PDUSide")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }
                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "Connector_Type_DeviceSide")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }
                    }

                    /*
                     * Height_mm	Width_mm	Depth_mm	UHeight	Weight_kg	Max_Power_Watts	Total_PSUCount	Req_PSUCount	
                     * Connector_Type_PDUSide	Connector_Type_DeviceSide	IsBlade	IsEnclosure	Encl_Front_Row_Count	Encl_Front_Col_Count	
                     * Encl_Rear_Row_Count	Encl_Rear_Col_Count	Blade_Row_Count	Blade_Col_Count	MountType	AirFlowDirection	InternalWidth_Rack	
                     * Internal_Depth_Rack	Internal_Height_Rack
                     */

                    // adds zero in t0 columns and space to string columns,
                    // this is to aviod null values being skipped by xml 
                    // serialization.
                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "height_mm")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "width_mm")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "depth_mm")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "weight_kg")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "max_power_watts")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "total_psucount")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "req_psucount")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "uheight")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "encl_front_row_count")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "encl_front_col_count")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "encl_rear_row_count")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "encl_rear_col_count")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "blade_row_count")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "blade_col_count")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "internalwidth_rack")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "internal_depth_rack")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "internal_height_rack")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0.0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "isblade")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName.ToLower() == "isenclosure")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }

                    if (dsExcel.Tables["AssetModels"].Columns[colIdx].ColumnName != "ID" &&
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
                if (HttpContext.Current.Session["FileIDModels"] != null)
                {
                    strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileIDModels"].ToString() + ".xml";
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
                            int curRowsCount = dsExcel.Tables["AssetModels"].Rows.Count + 1;
                            foreach (DataRow dr in dsPrevExcelData.Tables[0].Rows)
                            {
                                /*
                                 * ModelName	Manufacturer	ModelType	Height_mm	Width_mm	Depth_mm	UHeight	Weight_kg	Max_Power_Watts	
                                 * Total_PSUCount	Req_PSUCount	Connector_Type_PDUSide	Connector_Type_DeviceSide	IsBlade	IsEnclosure	Encl_Front_Row_Count
                                 * Encl_Front_Col_Count	Encl_Rear_Row_Count	Encl_Rear_Col_Count	Blade_Row_Count	Blade_Col_Count	MountType	AirFlowDirection	
                                 * InternalWidth_Rack	Internal_Depth_Rack	Internal_Height_Rack
                                 */
                                DataRow newRow = dsExcel.Tables["AssetModels"].NewRow();
                                newRow["ModelName"] = dr["ModelName"].ToString().Trim();
                                newRow["Manufacturer"] = dr["Manufacturer"].ToString().Trim();
                                newRow["ModelType"] = dr["ModelType"].ToString().Trim();
                                newRow["Height_mm"] = dr["Height_mm"].ToString().Trim();
                                newRow["Width_mm"] = dr["Width_mm"].ToString().Trim();
                                newRow["Depth_mm"] = dr["Depth_mm"].ToString().Trim();
                                newRow["UHeight"] = dr["UHeight"].ToString().Trim();
                                newRow["Weight_kg"] = dr["Weight_kg"].ToString().Trim();
                                newRow["Max_Power_Watts"] = dr["Max_Power_Watts"].ToString().Trim();
                                newRow["Total_PSUCount"] = dr["Total_PSUCount"].ToString().Trim();
                                newRow["Req_PSUCount"] = dr["Req_PSUCount"].ToString().Trim();
                                newRow["Connector_Type_PDUSide"] = dr["Connector_Type_PDUSide"].ToString().Trim();
                                newRow["Connector_Type_DeviceSide"] = dr["Connector_Type_DeviceSide"].ToString().Trim();
                                newRow["IsBlade"] = dr["IsBlade"].ToString().Trim();
                                newRow["IsEnclosure"] = dr["IsEnclosure"].ToString().Trim();
                                newRow["Encl_Front_Row_Count"] = dr["Encl_Front_Row_Count"].ToString().Trim();
                                newRow["Encl_Front_Col_Count"] = dr["Encl_Front_Col_Count"].ToString().Trim();
                                newRow["Encl_Rear_Row_Count"] = dr["Encl_Rear_Row_Count"].ToString().Trim();
                                newRow["Encl_Rear_Col_Count"] = dr["Encl_Rear_Col_Count"].ToString().Trim();
                                newRow["Blade_Row_Count"] = dr["Blade_Row_Count"].ToString().Trim();
                                newRow["Blade_Col_Count"] = dr["Blade_Col_Count"].ToString().Trim();
                                newRow["MountType"] = dr["MountType"].ToString().Trim();
                                newRow["AirFlowDirection"] = dr["AirFlowDirection"].ToString().Trim();
                                newRow["InternalWidth_Rack"] = dr["InternalWidth_Rack"].ToString().Trim();
                                newRow["Internal_Depth_Rack"] = dr["Internal_Depth_Rack"].ToString().Trim();
                                newRow["Internal_Height_Rack"] = dr["Internal_Height_Rack"].ToString().Trim();

                                newRow["ID"] = curRowsCount++;
                                newRow["Status"] = "";
                                newRow["Reason"] = "";

                                dsExcel.Tables["AssetModels"].Rows.Add(newRow);
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

            dtExcel = dsExcel.Tables["AssetModels"];
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
            dtModel = new DataView(dtExcel).ToTable(true, new string[] { "ModelName", "Manufacturer" });
            recCount = (from cr in dtModel.AsEnumerable()
                        where cr.Field<string>("ModelName").Trim() != ""
                        group cr by new
                        {
                            Model = cr.Field<String>("ModelName").ToLower(),
                        }
                            into grp
                        select grp.First()).Count();
            modelCount = (int)recCount;

            DataRow drFS6 = dtFileStats.NewRow();
            drFS6["ID"] = "6";
            drFS6["Name"] = "No of Asset Models found";
            drFS6["Value"] = modelCount.ToString();
            dtFileStats.Rows.Add(drFS6);

            //Asset Type Count
            dtAssetType = new DataView(dtExcel).ToTable(true, new string[] { "ModelType" });
            recCount = (from cr in dtAssetType.AsEnumerable()
                        where cr.Field<string>("ModelType").Trim() != ""
                        group cr by new
                        {
                            AssetType = cr.Field<String>("ModelType").ToLower(),
                        }
                            into grp
                        select grp.First()).Count();

            int assetTypeCount = (int)recCount;

            DataRow drFS7 = dtFileStats.NewRow();
            drFS7["ID"] = "7";
            drFS7["Name"] = "No of Model Types found";
            drFS7["Value"] = assetTypeCount.ToString();
            dtFileStats.Rows.Add(drFS7);
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
        dtAssetModel = new DataView(dtExcel).ToTable(true, new string[] { "ModelName", "Manufacturer" });
        recCount = (from cr in dtAssetModel.AsEnumerable()
                    where cr.Field<string>("ModelName").Trim() != ""
                    select cr).Count();
        modelCount = (int)recCount;

        DataRow drFS6 = dtFileStats.NewRow();
        drFS6["ID"] = "6";
        drFS6["Name"] = "No of Asset Models found";
        drFS6["Value"] = modelCount.ToString();
        dtFileStats.Rows.Add(drFS6);

        //Asset Type Count
        dtAssetType = new DataView(dtExcel).ToTable(true, new string[] { "ModelType" });
        recCount = (from cr in dtAssetType.AsEnumerable()
                    where cr.Field<string>("ModelType").Trim() != ""
                    select cr).Count();
        int assetTypeCount = (int)recCount;

        DataRow drFS7 = dtFileStats.NewRow();
        drFS7["ID"] = "7";
        drFS7["Name"] = "No of Model Types found";
        drFS7["Value"] = assetTypeCount.ToString();
        dtFileStats.Rows.Add(drFS7);

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

                    case "AssetModels$":

                        sheet = "AssetModels$";

                        cmd = new OleDbCommand("SELECT * FROM [" + sheet + "] ", conn);
                        cmd.CommandType = CommandType.Text;
                        outputTable = new DataTable("AssetModels");
                        output.Tables.Add(outputTable);
                        new OleDbDataAdapter(cmd).Fill(outputTable);
                        //delete blank rows
                        output.Tables["AssetModels"].BeginLoadData();
                        (from cr in output.Tables["AssetModels"].AsEnumerable()
                         where cr.Field<string>("ModelName") == null && cr.Field<string>("Manufacturer") == null &&
                         cr.Field<string>("ModelType") == null &&
                         cr.Field<string>("MountType") == null
                         select cr).ToList().ForEach(cr => cr.Delete());
                        output.Tables["AssetModels"].EndLoadData();
                        output.Tables["AssetModels"].AcceptChanges();
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
        Session["WebUploadFilePathModels"] = e.FolderPath;
    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //Get Manufacturers
        GetManufacturers();
        //Get Model Types
        GetAssetTypes();

        GetDeviceConnectors();
        GetPDUConnectors();
        GetAirflowDirections();
        GetMountTypes();

        //Create Models
        insertModel();

        if (Session["FileIDModels"] != null)
            Session["tempFileIDModels"] = Session["FileIDModels"].ToString();
        //populate grid
        populateGrid();
        ibCreate.Enabled = false;
        FileStats.Visible = true;
        FileStats.Style.Add("display", "block");
        UploadStats.Style.Add("display", "block");
        ShowFileData();
        ResetAll();
    }

    private void ResetAll()
    {
        Session["WebUploadFilePathModels"] = null;
        ibExportToExcel.Visible = true;
        Session["FileIDModels"] = null;
        uploadButton.Style.Add("display", "none");
    }

    protected void grdAssetModel_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {

    }

    protected void grdAssetModel_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdAssetModel.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAssetModel.Behaviors.Paging.PageIndex);
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
            pagerControl.SetupPageList(this.grdAssetModel.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdAssetModel.Behaviors.Paging.PageIndex);
        }

    }

    protected void grdAssetModel_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {

    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        //Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        //this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        //Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        //this.eExporter.Export(this.grdAssetModel, wBook);

        WebControl[] array = new WebControl[1];
        array[0] = grdAssetModel;

        //define the workbook and some worksheets. 
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        Infragistics.Documents.Excel.Workbook book = new Infragistics.Documents.Excel.Workbook(excelFormat);
        book.Worksheets.Add("AssetModels");

        //first export the grids to each worksheet. Custom export mode is required for that.
        this.eExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;

        this.eExporter.Export(grdAssetModel, book.Worksheets[0]);

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

    public void GetAirflowDirections()
    {
        //Retreive all the AssetTypes from the database.
        DataSet dsAllAFs = new DataSet();
        DataTable dtAllAFNames = new DataTable();
        AirFlowDirectionBAL objAF = new AirFlowDirectionBAL();
        dsAllAFs = objAF.retrieve();
        try
        {
            //Get the AFDirectionIDs and AFDirection names into table dtAllAFNames
            var allATs = (from rows in dsAllAFs.Tables[0].AsEnumerable()
                          orderby rows.Field<int>("ID")
                          select new
                          {
                              ID = rows.Field<int>("ID"),
                              AirflowDirection = rows.Field<string>("AirflowDirection")

                          });
            dtAllAFNames = LINQToDataTable(allATs, "dtAirflowDirection");
            if (dtAllAFNames.Columns.Count == 0)
            {
                dtAllAFNames.Columns.Add("ID", typeof(int));
                dtAllAFNames.Columns.Add("AirflowDirection", typeof(string));

                dtAllAFNames.AcceptChanges();
            }
            //Get the new AssetTypes from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();

            dsExcelData.Tables.Add(dtAllAFNames);
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void GetMountTypes()
    {
        //Retreive all the AssetTypes from the database.
        DataSet dsAllMTs = new DataSet();
        DataTable dtAllMTNames = new DataTable();
        MountTypeBAL objMT = new MountTypeBAL();
        dsAllMTs = objMT.retrieve();
        try
        {
            //Get the AssetTypeIDs and AssetTypes into table dtAllAssetTypeNames
            var allATs = (from rows in dsAllMTs.Tables[0].AsEnumerable()
                          orderby rows.Field<int>("MountTypeID")
                          select new
                          {
                              MountTypeID = rows.Field<int>("MountTypeID"),
                              MountType = rows.Field<string>("MountType")

                          });
            dtAllMTNames = LINQToDataTable(allATs, "dtMountType");
            if (dtAllMTNames.Columns.Count == 0)
            {
                dtAllMTNames.Columns.Add("MountTypeID", typeof(int));
                dtAllMTNames.Columns.Add("MountType", typeof(string));

                dtAllMTNames.AcceptChanges();
            }
            //Get the new AssetTypes from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();

            dsExcelData.Tables.Add(dtAllMTNames);
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void GetDeviceConnectors()
    {
        //Retreive all the AssetTypes from the database.
        DataSet dsAllDCs = new DataSet();
        DataTable dtAllDCNames = new DataTable();
        InputConnectorTypeBAL objDC = new InputConnectorTypeBAL();
        dsAllDCs = objDC.retrieve();
        try
        {
            var allDCs = (from rows in dsAllDCs.Tables[0].AsEnumerable()
                          orderby rows.Field<int>("InputConnectorTypeID")
                          select new
                          {
                              InputConnectorTypeID = rows.Field<int>("InputConnectorTypeID"),
                              InputConnectorType = rows.Field<string>("InputConnectorType")

                          });
            dtAllDCNames = LINQToDataTable(allDCs, "dtDeviceConnectorType");
            if (dtAllDCNames.Columns.Count == 0)
            {
                dtAllDCNames.Columns.Add("InputConnectorTypeID", typeof(int));
                dtAllDCNames.Columns.Add("InputConnectorType", typeof(string));

                dtAllDCNames.AcceptChanges();
            }
            //Get the new AssetTypes from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();

            dsExcelData.Tables.Add(dtAllDCNames);
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void GetPDUConnectors()
    {
        //Retreive all the AssetTypes from the database.
        DataSet dsAllPCs = new DataSet();
        DataTable dtAllPCNames = new DataTable();
        OutputConnectorTypeBAL objPC = new OutputConnectorTypeBAL();
        dsAllPCs = objPC.retrieve();
        try
        {
            //Get the AssetTypeIDs and AssetTypes into table dtAllAssetTypeNames
            var allPCs = (from rows in dsAllPCs.Tables[0].AsEnumerable()
                          orderby rows.Field<int>("OutputConnectorTypeID")
                          select new
                          {
                              OutputConnectorTypeID = rows.Field<int>("OutputConnectorTypeID"),
                              OutputConnectorType = rows.Field<string>("OutputConnectorType")

                          });
            dtAllPCNames = LINQToDataTable(allPCs, "dtPDUConnectorType");
            if (dtAllPCNames.Columns.Count == 0)
            {
                dtAllPCNames.Columns.Add("OutputConnectorTypeID", typeof(int));
                dtAllPCNames.Columns.Add("OutputConnectorType", typeof(string));

                dtAllPCNames.AcceptChanges();
            }
            //Get the new AssetTypes from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();

            dsExcelData.Tables.Add(dtAllPCNames);
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void GetAssetTypes()
    {
        string assetTypeName = string.Empty;
        Dictionary<int, string> dictAssetTypesInserted = new Dictionary<int, string>();

        //Retreive all the AssetTypes from the database.
        DataSet dsAllATs = new DataSet();
        DataTable dtAllATNames = new DataTable();
        iAssetTrackBAL.AssetGroupBAL objAT = new iAssetTrackBAL.AssetGroupBAL();
        dsAllATs = objAT.retrieveAllAssetGroup();
        try
        {
            //Get the AssetTypeIDs and AssetTypes into table dtAllAssetTypeNames
            var allATs = (from rows in dsAllATs.Tables[0].AsEnumerable()
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

    public void GetManufacturers()
    {
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
        #region Get all tables from dataset
        string modelName = string.Empty;
        string mfgName = string.Empty;
        string modelType = string.Empty;
        string mountType = string.Empty;
        string airflowDirection = string.Empty;
        string connector_pdu_side = string.Empty;
        string connector_device_side = string.Empty;
        bool isBlade = false;
        bool isEnclosure = false;
        int bladeRCount = 0;
        int bladeCCount = 0;
        int enclFRCount = 0;
        int enclFCCount = 0;
        int enclRRCount = 0;
        int enclRCCount = 0;
        float width_mm = 0.0f;
        float depth_mm = 0.0f;
        float height_mm = 0.0f;
        float weight_kg = 0.0f;
        float max_power_watts = 0.0f;
        int total_psu_count;
        int req_psu_count;
        int uHeight;
        int assetTypeID = 0;
        float internal_rack_width = 0.0f;
        float internal_rack_depth = 0.0f;
        float internal_rack_height = 0.0f;
        int afDirectionID = 0;
        int mountTypeID = 0;

        //Get all tables from the Import Asset Excel
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtAssetType = new DataTable();
        DataTable dtAssetModel = new DataTable();
        DataTable dtMfg = new DataTable();
        DataTable dtAFDirection = new DataTable();
        DataTable dtMountType = new DataTable();
        DataTable dtPDUConnector = new DataTable();
        DataTable dtDeviceConnector = new DataTable();
        //Result tables
        DataTable dtATResult = new DataTable();
        DataTable dtAMResult = new DataTable();
        DataTable dtMResult = new DataTable();

        dtAssetModel = dsExcelData.Tables["AssetModels"];
        dtAssetType = dsExcelData.Tables["dtAssetType"];
        dtMfg = dsExcelData.Tables["dtManufacturer"];
        dtAFDirection = dsExcelData.Tables["dtAirflowDirection"];
        dtMountType = dsExcelData.Tables["dtMountType"];
        dtDeviceConnector = dsExcelData.Tables["dtDeviceConnectorType"];
        dtPDUConnector = dsExcelData.Tables["dtPDUConnectorType"];

        #endregion

        foreach (DataRow dr in dtAssetModel.Rows)
        {
            if (!string.IsNullOrEmpty(dr["ModelName"].ToString().Trim()) || !string.IsNullOrEmpty(dr["ModelName"].ToString().Trim()))
            {
                if (!string.IsNullOrEmpty(dr["Manufacturer"].ToString().Trim()) && !string.IsNullOrEmpty(dr["Manufacturer"].ToString().Trim()))
                {
                    if (!string.IsNullOrEmpty(dr["ModelType"].ToString().Trim()) && !string.IsNullOrEmpty(dr["ModelType"].ToString().Trim()))
                    {
                        if (!string.IsNullOrEmpty(dr["MountType"].ToString().Trim()) && !string.IsNullOrEmpty(dr["MountType"].ToString().Trim()))
                        {
                            try
                            {
                                #region Assign Variables

                                mfgName = (dr["Manufacturer"].ToString()).Trim();
                                modelName = (dr["ModelName"].ToString()).Trim();
                                modelType = dr["ModelType"].ToString().Trim();
                                mountType = dr["MountType"].ToString().Trim();
                                airflowDirection = dr["AirFlowDirection"].ToString().Trim();
                                connector_pdu_side = dr["Connector_Type_PDUSide"].ToString().Trim();
                                connector_device_side = dr["Connector_Type_DeviceSide"].ToString().Trim();


                                //BusinessUnit ID
                                BusinessUnitBAL buBAL = new BusinessUnitBAL();
                                buBAL.BusinessUnit = txtBU.Text.Trim();
                                int buID = 0;
                                if (buBAL.retrieve().Tables.Count > 0)
                                {
                                    buID = Convert.ToInt32(buBAL.retrieve().Tables[0].Select("BusinessUnit ='" + txtBU.Text.Trim() + "'")[0][0].ToString());
                                }
                                #endregion

                                if (!string.IsNullOrEmpty(dr["Width_mm"].ToString()) && float.TryParse(dr["Width_mm"].ToString(), out width_mm))
                                {
                                    if (!string.IsNullOrEmpty(dr["Depth_mm"].ToString()) && float.TryParse(dr["Depth_mm"].ToString(), out depth_mm))
                                    {
                                        if (!string.IsNullOrEmpty(dr["Height_mm"].ToString()) && float.TryParse(dr["Height_mm"].ToString(), out height_mm))
                                        {
                                            if (!string.IsNullOrEmpty(dr["Weight_kg"].ToString()) && float.TryParse(dr["Weight_kg"].ToString(), out weight_kg))
                                            {
                                                if (!string.IsNullOrEmpty(dr["UHeight"].ToString()) && int.TryParse(dr["UHeight"].ToString(), out uHeight))
                                                {
                                                    if (!string.IsNullOrEmpty(dr["Max_Power_Watts"].ToString()) && float.TryParse(dr["Max_Power_Watts"].ToString(), out max_power_watts))
                                                    {
                                                        if (!string.IsNullOrEmpty(dr["Total_PSUCount"].ToString()) && int.TryParse(dr["Total_PSUCount"].ToString(), out total_psu_count))
                                                        {
                                                            if (!string.IsNullOrEmpty(dr["Req_PSUCount"].ToString()) && int.TryParse(dr["Req_PSUCount"].ToString(), out req_psu_count))
                                                            {
                                                                //Model Name check
                                                                Regex regx = new Regex(@"^^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");
                                                                if (regx.IsMatch(modelName.Trim()) && modelName.Trim().Length <= 150)
                                                                {
                                                                    regx = new Regex(@"^[\w\-\.\:]+(\s{1}[\w\-\.\:]+)*\s{0,1}$");
                                                                    //Manufacturer check
                                                                    if (string.IsNullOrEmpty(mfgName.Trim()) || (regx.IsMatch(mfgName.Trim()) && mfgName.Trim().Length <= 50))
                                                                    {
                                                                        var assetMfgResult = from mRow in dtMfg.AsEnumerable()
                                                                                             where mRow.Field<string>("MfgName").Equals(mfgName, StringComparison.InvariantCultureIgnoreCase)
                                                                                             select mRow;
                                                                        int mfgID;
                                                                        if (assetMfgResult.Count() > 0)
                                                                        {
                                                                            //Model Type Check
                                                                            if (string.IsNullOrEmpty(modelType.Trim()) || (regx.IsMatch(modelType.Trim()) && modelType.Trim().Length <= 25))
                                                                            {
                                                                                var modeltypeResult = from mRow in dtAssetType.AsEnumerable()
                                                                                                      where mRow.Field<string>("Assetgroup").Equals(modelType.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                                                                      select mRow;
                                                                                int modelTypeID;
                                                                                if (modeltypeResult.Count() > 0)
                                                                                {

                                                                                    dtMResult = assetMfgResult.CopyToDataTable();
                                                                                    mfgID = Convert.ToInt32(dtMResult.Rows[0]["MfgID"]);

                                                                                    dtATResult = modeltypeResult.CopyToDataTable();
                                                                                    modelTypeID = Convert.ToInt32(dtATResult.Rows[0]["AssetGroupID"]);

                                                                                    if (depth_mm >= 1 && depth_mm <= 1500)
                                                                                    {
                                                                                        if (width_mm >= 1 && width_mm <= 1500)
                                                                                        {
                                                                                            if (height_mm >= 1 && height_mm <= 3000)
                                                                                            {
                                                                                                if (weight_kg >= 0 && weight_kg <= 2000)
                                                                                                {
                                                                                                    if (uHeight >= 1 && uHeight <= 68)
                                                                                                    {
                                                                                                        if (max_power_watts >= 0 && max_power_watts <= 10000)
                                                                                                        {
                                                                                                            if (total_psu_count >= 0 && total_psu_count <= 10)
                                                                                                            {
                                                                                                                if (req_psu_count >= 0 && req_psu_count <= 10 && req_psu_count <= total_psu_count)
                                                                                                                {
                                                                                                                    //Airflow Direction
                                                                                                                    if (!string.IsNullOrEmpty(airflowDirection))
                                                                                                                    {
                                                                                                                        var afDirectionResult = from mRow in dtAFDirection.AsEnumerable()
                                                                                                                                                where mRow.Field<string>("AirflowDirection").Equals(airflowDirection, StringComparison.InvariantCultureIgnoreCase)
                                                                                                                                                select mRow;

                                                                                                                        if (afDirectionResult.Count() > 0)
                                                                                                                        {
                                                                                                                            DataTable dtAF = afDirectionResult.CopyToDataTable();
                                                                                                                            afDirectionID = Convert.ToInt32(dtAF.Rows[0]["ID"]);
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            afDirectionID = 0;
                                                                                                                        }

                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        afDirectionID = 1;
                                                                                                                    }
                                                                                                                    if (afDirectionID > 0)
                                                                                                                    {
                                                                                                                        //Mount Type
                                                                                                                        if (!string.IsNullOrEmpty(mountType))
                                                                                                                        {
                                                                                                                            var mtResult = from mRow in dtMountType.AsEnumerable()
                                                                                                                                           where mRow.Field<string>("MountType").Equals(mountType, StringComparison.InvariantCultureIgnoreCase)
                                                                                                                                           select mRow;

                                                                                                                            if (mtResult.Count() > 0)
                                                                                                                            {
                                                                                                                                DataTable dtMT = mtResult.CopyToDataTable();
                                                                                                                                mountTypeID = Convert.ToInt32(dtMT.Rows[0]["MountTypeID"]);
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                mountTypeID = 0;
                                                                                                                            }

                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            mountTypeID = 0;
                                                                                                                        }

                                                                                                                        if (mountTypeID > 0)
                                                                                                                        {
                                                                                                                            //If Mount type is RackMount than asset width will be hard coded for 445.5
                                                                                                                            //as this only allowed size for rack mount to fit-in in the rack
                                                                                                                            if (isRackMountDevice(mountTypeID))
                                                                                                                                width_mm = 445.5f;
                                                                                                                            //Connector Types
                                                                                                                            if (!string.IsNullOrEmpty(connector_device_side))
                                                                                                                            {
                                                                                                                                var deviceCResult = from mRow in dtDeviceConnector.AsEnumerable()
                                                                                                                                                    where mRow.Field<string>("InputConnectorType").Equals(connector_device_side, StringComparison.InvariantCultureIgnoreCase)
                                                                                                                                                    select mRow;

                                                                                                                                if (deviceCResult.Count() > 0)
                                                                                                                                {
                                                                                                                                    DataTable dtDC = deviceCResult.CopyToDataTable();
                                                                                                                                    connector_device_side = dtDC.Rows[0]["InputConnectorType"].ToString();
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    connector_device_side = "Other";
                                                                                                                                }

                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                connector_device_side = "Other";
                                                                                                                            }

                                                                                                                            if (!string.IsNullOrEmpty(connector_pdu_side))
                                                                                                                            {
                                                                                                                                var pduCResult = from mRow in dtPDUConnector.AsEnumerable()
                                                                                                                                                 where mRow.Field<string>("OutputConnectorType").Equals(connector_pdu_side, StringComparison.InvariantCultureIgnoreCase)
                                                                                                                                                 select mRow;

                                                                                                                                if (pduCResult.Count() > 0)
                                                                                                                                {
                                                                                                                                    DataTable dtPC = pduCResult.CopyToDataTable();
                                                                                                                                    connector_pdu_side = dtPC.Rows[0]["OutputConnectorType"].ToString();
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    connector_pdu_side = "Other";
                                                                                                                                }

                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                connector_pdu_side = "Other";
                                                                                                                            }

                                                                                                                            bool validationError = false;
                                                                                                                            //Check blade and enclosure
                                                                                                                            if (modelType.ToLower().Contains("blade") || modelType.ToLower().Contains("module"))
                                                                                                                            {
                                                                                                                                if (!string.IsNullOrEmpty(dr["isBlade"].ToString()) && dr["isBlade"].ToString().Trim().CompareTo("1") == 0)
                                                                                                                                {
                                                                                                                                    isBlade = true;
                                                                                                                                    if (!mountType.ToLower().Contains("enclosure"))
                                                                                                                                    {
                                                                                                                                        validationError = true;
                                                                                                                                        dr["Status"] = "Failed";
                                                                                                                                        dr["Reason"] = "Blade model mount type must be enclosure mount";
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        if (!string.IsNullOrEmpty(dr["Blade_Row_Count"].ToString()) && !string.IsNullOrEmpty(dr["Blade_Col_Count"].ToString()) &&
                                                                                                                                            int.TryParse(dr["Blade_Row_Count"].ToString(), out bladeRCount) && int.TryParse(dr["Blade_Col_Count"].ToString(), out bladeCCount)
                                                                                                                                            && bladeCCount > 0 && bladeRCount > 0)
                                                                                                                                        {
                                                                                                                                            //do nothing
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            validationError = true;
                                                                                                                                            dr["Status"] = "Failed";
                                                                                                                                            dr["Reason"] = "Blade row and column counts missing";
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    validationError = true;
                                                                                                                                    dr["Status"] = "Failed";
                                                                                                                                    dr["Reason"] = "IsBlade must be 1 for a model of blade type";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else if (modelType.ToLower().Contains("enclosure"))
                                                                                                                            {
                                                                                                                                if (!string.IsNullOrEmpty(dr["isEnclosure"].ToString()) && dr["isEnclosure"].ToString().Trim().CompareTo("1") == 0)
                                                                                                                                {
                                                                                                                                    isEnclosure = true;
                                                                                                                                    if (!string.IsNullOrEmpty(dr["Encl_Front_Row_Count"].ToString()) && !string.IsNullOrEmpty(dr["Encl_Front_Col_Count"].ToString()) &&
                                                                                                                                           int.TryParse(dr["Encl_Front_Row_Count"].ToString(), out enclFRCount) && int.TryParse(dr["Encl_Front_Col_Count"].ToString(), out enclFCCount)
                                                                                                                                        && enclFRCount > 0 && enclFCCount > 0)
                                                                                                                                    {
                                                                                                                                        if (!string.IsNullOrEmpty(dr["Encl_Rear_Row_Count"].ToString()) && !string.IsNullOrEmpty(dr["Encl_Rear_Col_Count"].ToString()) &&
                                                                                                                                           int.TryParse(dr["Encl_Rear_Row_Count"].ToString(), out enclRRCount) && int.TryParse(dr["Encl_Rear_Col_Count"].ToString(), out enclRCCount)
                                                                                                                                            && enclRRCount > 0 && enclRCCount > 0)
                                                                                                                                        {
                                                                                                                                            //check mount type selected for enclosure model
                                                                                                                                            if (!mountType.ToLower().Contains("rackmount"))
                                                                                                                                            {
                                                                                                                                                validationError = true;
                                                                                                                                                dr["Status"] = "Failed";
                                                                                                                                                dr["Reason"] = "Enclosure model mount type must be rack mount";
                                                                                                                                            }
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            validationError = true;
                                                                                                                                            dr["Status"] = "Failed";
                                                                                                                                            dr["Reason"] = "Enclosure rear row and column data is missing";
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                    else
                                                                                                                                    {
                                                                                                                                        validationError = true;
                                                                                                                                        dr["Status"] = "Failed";
                                                                                                                                        dr["Reason"] = "Enclosure front row and column data is missing";
                                                                                                                                    }
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    validationError = true;
                                                                                                                                    dr["Status"] = "Failed";
                                                                                                                                    dr["Reason"] = "IsEnclosure must be 1 for a model of Enclosure type";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else if (modelType.ToLower().Contains("standalone"))
                                                                                                                            {
                                                                                                                                if (!mountType.ToLower().Contains("standalone"))
                                                                                                                                {
                                                                                                                                    validationError = true;
                                                                                                                                    dr["Status"] = "Failed";
                                                                                                                                    dr["Reason"] = "Standalone type model mount type must be standalone";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else if (modelType.ToLower().CompareTo("rack") == 0)
                                                                                                                            {
                                                                                                                                if (!mountType.ToLower().Contains("standalone"))
                                                                                                                                {
                                                                                                                                    validationError = true;
                                                                                                                                    dr["Status"] = "Failed";
                                                                                                                                    dr["Reason"] = "Standalone type model mount type must be standalone";
                                                                                                                                }
                                                                                                                                else if (!string.IsNullOrEmpty(dr["InternalWidth_Rack"].ToString()) && !string.IsNullOrEmpty(dr["Internal_Depth_Rack"].ToString()) &&
                                                                                                                                            !string.IsNullOrEmpty(dr["Internal_Height_Rack"].ToString()) &&
                                                                                                                                           float.TryParse(dr["InternalWidth_Rack"].ToString(), out internal_rack_width) && float.TryParse(dr["Internal_Depth_Rack"].ToString(), out internal_rack_depth)
                                                                                                                                            && float.TryParse(dr["Internal_Height_Rack"].ToString(), out internal_rack_height)
                                                                                                                                    && internal_rack_depth > 0 && internal_rack_height > 0 && internal_rack_width > 0)
                                                                                                                                {
                                                                                                                                    //do nothing
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    validationError = true;
                                                                                                                                    dr["Status"] = "Failed";
                                                                                                                                    dr["Reason"] = "Rack model must have internal depth,height and width details";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else if (modelType.ToLower().CompareTo("rack pdu-zero u") == 0)
                                                                                                                            {
                                                                                                                                if (!mountType.ToLower().Contains("vertical"))
                                                                                                                                {
                                                                                                                                    validationError = true;
                                                                                                                                    dr["Status"] = "Failed";
                                                                                                                                    dr["Reason"] = "Rack PDU-Zero U model mount type must be vertical mount";
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                //rack mount and Stack in rack models
                                                                                                                                if (!mountType.ToLower().Contains("rackmount"))
                                                                                                                                {
                                                                                                                                    validationError = true;
                                                                                                                                    dr["Status"] = "Failed";
                                                                                                                                    dr["Reason"] = "Mount type must be Rack mount";
                                                                                                                                }
                                                                                                                            }

                                                                                                                            if (!validationError)
                                                                                                                            {
                                                                                                                                float pdf;
                                                                                                                                float steadypower;
                                                                                                                                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PDF"].ToString()) && float.TryParse(ConfigurationManager.AppSettings["PDF"].ToString(), out pdf))
                                                                                                                                {
                                                                                                                                    steadypower = max_power_watts * pdf;
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    steadypower = max_power_watts;
                                                                                                                                }

                                                                                                                                objAM = new AssetModelBAL();

                                                                                                                                int modelID = 0;
                                                                                                                                objAM.ModelName = modelName;
                                                                                                                                objAM.ModelID = modelID;
                                                                                                                                objAM.MfgID = mfgID;
                                                                                                                                objAM.BUID = buID;

                                                                                                                                if (objAM.exists() == 0)
                                                                                                                                {
                                                                                                                                    PostModelData2DB(modelID, modelName, mfgID, buID, connector_pdu_side, connector_device_side, isBlade, isEnclosure, bladeRCount,
                                                                                                                                        bladeCCount, enclFRCount, enclFCCount, enclRRCount, enclRCCount, width_mm, depth_mm, height_mm, weight_kg,
                                                                                                                                        max_power_watts, total_psu_count, req_psu_count,
                                                                                                                                        uHeight, internal_rack_width, internal_rack_depth, internal_rack_height, afDirectionID, mountTypeID, modelTypeID, steadypower);
                                                                                                                                    dr["Status"] = "Success";
                                                                                                                                    dr["Reason"] = "Asset model created successfully";
                                                                                                                                }
                                                                                                                                else
                                                                                                                                {
                                                                                                                                    DataSet dsModels = objAM.retrieve();


                                                                                                                                    if (dsModels.Tables.Count > 0 && dsModels.Tables[0].Rows.Count > 0)
                                                                                                                                    {
                                                                                                                                        int assetCount = 0;
                                                                                                                                        DataRow[] rows = dsModels.Tables[0].Select(" ModelName = '" + modelName.Trim() + "' AND  "
                                                                                                                                            + " MfgID = " + mfgID + " AND BusinessUnitID = " + buID);
                                                                                                                                        if (rows.Length > 0 && int.TryParse(rows[0]["AssetCount"].ToString(), out assetCount) && assetCount <= 0)
                                                                                                                                        {
                                                                                                                                            modelID = int.Parse(rows[0]["ModelID"].ToString());
                                                                                                                                            //update models data since no assets with this model
                                                                                                                                            PostModelData2DB(modelID, modelName, mfgID, buID, connector_pdu_side, connector_device_side, isBlade, isEnclosure, bladeRCount,
                                                                                                                                                bladeCCount, enclFRCount, enclFCCount, enclRRCount, enclRCCount, width_mm, depth_mm, height_mm, weight_kg,
                                                                                                                                                max_power_watts, total_psu_count, req_psu_count,
                                                                                                                                                uHeight, internal_rack_width, internal_rack_depth, internal_rack_height, afDirectionID, mountTypeID, modelTypeID, steadypower);
                                                                                                                                            dr["Status"] = "Success";
                                                                                                                                            dr["Reason"] = "Asset model data updated";
                                                                                                                                        }
                                                                                                                                        else
                                                                                                                                        {
                                                                                                                                            //Model already exists with some assets associated with it.
                                                                                                                                            dr["Status"] = "Failed";
                                                                                                                                            dr["Reason"] = "Asset model details can not be updated";
                                                                                                                                        }
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }

                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            dr["Status"] = "Failed";
                                                                                                                            dr["Reason"] = "Mount Type details not correct";
                                                                                                                        }

                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        dr["Status"] = "Failed";
                                                                                                                        dr["Reason"] = "Airflow Direction details not correct";
                                                                                                                    }

                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    dr["Status"] = "Failed";
                                                                                                                    dr["Reason"] = "Required PSU count should be less than Total PSU count and value between 0 and 10";
                                                                                                                }

                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                dr["Status"] = "Failed";
                                                                                                                dr["Reason"] = "Total PSU count should be between 0 and 10";
                                                                                                            }


                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            dr["Status"] = "Failed";
                                                                                                            dr["Reason"] = "Max power should be between 0 and 10000";
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        dr["Status"] = "Failed";
                                                                                                        dr["Reason"] = "UHeight should be between 1 and 68";
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    dr["Status"] = "Failed";
                                                                                                    dr["Reason"] = "Weight should be between 0 and 2000";
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                dr["Status"] = "Failed";
                                                                                                dr["Reason"] = "Height should be between 1 and 3000";
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            dr["Status"] = "Failed";
                                                                                            dr["Reason"] = "Width should be between 1 and 1500";
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        dr["Status"] = "Failed";
                                                                                        dr["Reason"] = "Depth should be between 1 and 1500";
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    dr["Status"] = "Failed";
                                                                                    dr["Reason"] = "Model Type details not found in database";
                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                dr["Status"] = "Failed";
                                                                                dr["Reason"] = "Model Type details not in correct format";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            dr["Status"] = "Failed";
                                                                            dr["Reason"] = "Manufacturer details not found in database";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        dr["Status"] = "Failed";
                                                                        dr["Reason"] = "Manufacturer details not in correct format";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    dr["Status"] = "Failed";
                                                                    dr["Reason"] = "Serial Number is not in correct format";
                                                                }

                                                            }
                                                            else
                                                            {
                                                                dr["Status"] = "Failed";
                                                                dr["Reason"] = "Required PSU Count data is missing or not in correct format";
                                                            }

                                                        }
                                                        else
                                                        {
                                                            dr["Status"] = "Failed";
                                                            dr["Reason"] = "Total PSU Count data is missing or not in correct format";
                                                        }

                                                    }
                                                    else
                                                    {
                                                        dr["Status"] = "Failed";
                                                        dr["Reason"] = "Max Power Watts data is missing or not in correct format";
                                                    }

                                                }
                                                else
                                                {
                                                    dr["Status"] = "Failed";
                                                    dr["Reason"] = "UHeight data is missing or not in correct format";
                                                }

                                            }
                                            else
                                            {
                                                dr["Status"] = "Failed";
                                                dr["Reason"] = "Weight data is missing or not in correct format";
                                            }

                                        }
                                        else
                                        {
                                            dr["Status"] = "Failed";
                                            dr["Reason"] = "Height data is missing or not in correct format";
                                        }

                                    }
                                    else
                                    {
                                        dr["Status"] = "Failed";
                                        dr["Reason"] = "Depth data is missing or not in correct format";
                                    }

                                }
                                else
                                {
                                    dr["Status"] = "Failed";
                                    dr["Reason"] = "Width data is missing or not in correct format";
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
                            dr["Reason"] = "Mount Type details missing";
                        }

                    }
                    else
                    {
                        dr["Status"] = "Failed";
                        dr["Reason"] = "Model Type details missing";
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
                dr["Reason"] = "Model Name is required.";
            }
        }
        //Serialize the dataset dsExcel data - with 8 tables
        //(Excel data, BudsinessUnit table, Site table, Location table, Manufacturer table, Model table, 
        //AssetType table and Asset Table)
        dsExcelData.AcceptChanges();
        SerializeExcelDataSetToXml(dsExcelData);
    }

    private bool isRackMountDevice(int mountTypeId)
    {
        bool isRackMountDevice = false;

        try
        {
            MountTypeBAL objMT = new MountTypeBAL();
            DataSet dsAllMTs = objMT.retrieve();

            var mtResult = from mRow in dsAllMTs.Tables[0].AsEnumerable()
                           where mRow.Field<string>("MountType").Equals("RackMount", StringComparison.InvariantCultureIgnoreCase)
                           select mRow;

            if (mtResult.Count() > 0)
            {
                DataTable dtMT = mtResult.CopyToDataTable();
                int rmMountTypeID = Convert.ToInt32(dtMT.Rows[0]["MountTypeID"]);
                if (rmMountTypeID == mountTypeId)
                    isRackMountDevice = true;
            }
            else
            {
                isRackMountDevice = false;
            }
        }
        catch(Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }

        return isRackMountDevice;
    }

    private void PostModelData2DB(int modelID, string modelName, int mfgID, int buID, string connector_pdu_side, string connector_device_side, bool isBlade, bool isEnclosure, int bladeRCount, int bladeCCount, int enclFRCount, int enclFCCount, int enclRRCount, int enclRCCount, float width_mm, float depth_mm, float height_mm, float weight_kg, float max_power_watts, int total_psu_count, int req_psu_count, int uHeight, float internal_rack_width, float internal_rack_depth, float internal_rack_height, int afDirectionID, int mountTypeID, int modelTypeID, float steadypower)
    {
        objAM.ModelName = modelName;
        objAM.ModelID = modelID;
        objAM.MfgID = mfgID;
        objAM.BUID = buID;
        objAM.Description = modelName;
        objAM.Status = 1;
        objAM.CreatedBy = Convert.ToInt32(Session["UserID"]);
        objAM.TechID = 0;
        objAM.SPCID = 0;
        objAM.Comment = "";
        objAM.IsBlade = isBlade;
        objAM.IsEnclosure = isEnclosure;
        objAM.ModelTypeID = modelTypeID;
        objAM.Width = width_mm;
        objAM.Depth = depth_mm;
        objAM.Height = height_mm;
        objAM.UHeight = uHeight;
        objAM.Weight = weight_kg;
        objAM.MaxPower = max_power_watts;
        objAM.SteadyStatePower = steadypower;
        objAM.ConnectorDevSide = connector_device_side;
        objAM.ConnectorPDUSide = connector_pdu_side;
        objAM.TotalPSUCount = total_psu_count;
        objAM.RequiredPSUCount = req_psu_count;
        objAM.MountTypeID = mountTypeID;
        objAM.AFDirectionID = afDirectionID;
        objAM.RackInternalDepth = internal_rack_depth;
        objAM.RackInternalHeight = internal_rack_height;
        objAM.RackInternalWidth = internal_rack_width;
        objAM.EnclFrontRowCount = enclFRCount;
        objAM.EnclFrontColCount = enclFCCount;
        objAM.EnclRearRowCount = enclRRCount;
        objAM.EnclRearColCount = enclRCCount;
        objAM.BladeRowCount = bladeRCount;
        objAM.BladeColCount = bladeCCount;

        objAM.Persist(DALCOperation.Insert);
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
            //grdAssetModel.ClearDataSource();
            grdAssetModel.DataSource = ds.Tables[0];
            grdAssetModel.DataBind();

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
                drFS1["Name"] = "Total Asset models found";
                drFS1["Value"] = totalAssets.ToString();
                dtUploadStats.Rows.Add(drFS1);
                //Success Count
                DataRow drFS2 = dtUploadStats.NewRow();
                drFS2["ID"] = "2";
                drFS2["Name"] = "No of Asset models imported successfully";
                drFS2["Value"] = successAssets.ToString();
                dtUploadStats.Rows.Add(drFS2);

                //Failure Count
                DataRow drFS3 = dtUploadStats.NewRow();
                drFS3["ID"] = "3";
                drFS3["Name"] = "No of Asset models failed to import";
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
            if (HttpContext.Current.Session["FileIDModels"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileIDModels"].ToString() + ".xml";

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
            if (Session["FileIDModels"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["FileIDModels"].ToString() + ".xml");
                if (File.Exists(strFile))
                {
                    // Read the content of the file into a DataSet
                    XmlTextReader xtr = new XmlTextReader(strFile);
                    ds = new DataSet();
                    ds.ReadXml(xtr);
                    xtr.Close();
                }
            }
            else if (Session["tempFileIDModels"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["tempFileIDModels"].ToString() + ".xml");
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

    #endregion
}



