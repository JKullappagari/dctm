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
using Infragistics.Web.UI.GridControls;

public partial class ImportAsset : System.Web.UI.Page, IPostBackEventHandler
{
    #region "Declarations"
    private iAssetTrack.BAL.BusinessUnitBAL objBU;
    private iAssetTrack.BAL.SitesBAL objSite;
    private iAssetTrack.BAL.BUSiteAssignmentBAL objBUSite;
    private iAssetTrack.BAL.LocationBAL objLocation;
    private iAssetTrack.BAL.SiteLocationAssignmentBAL objSiteLocation;
    private iAssetTrack.BAL.ManufacturerBAL objMfg;
    private iAssetTrack.BAL.AssetModelBAL objAM;
    private iAssetTrackBAL.AssetGroupBAL objAT;
    private iAssetTrack.BAL.AssetBAL objAsset;
    private iAssetTrack.BAL.OwnerBAL objOwner;
    DataTable _dtRights;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl, pagerControlRacks;
    #endregion

    #region " Page Event Methods "



    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdAsset.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdAsset_ItemCommand);
        grdRacks.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdRacks_ItemCommand);

        pagerControl = grdAsset.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);

        pagerControlRacks = grdRacks.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPagerRack") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        pagerControlRacks.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControlRacks_PageChanged);

    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdAsset.Behaviors.Paging.PageIndex = e.PageNumber;
        populateAssetGrid();
    }

    void currentPageControlRacks_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdRacks.Behaviors.Paging.PageIndex = e.PageNumber;
        populateRackGrid();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Import Asset";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "ImportAsset.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Import Asset' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Import Asset' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Import Asset' and Rights = '" + "Delete" + "'").Length != 0)
        {
            //ibDelete.Visible = true;
        }
        else
        {
            //ibDelete.Visible = false;
        }

        this.grdAsset.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        this.grdRacks.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());


        if (!IsPostBack)
        {

            Session["WebUploadFilePath"] = null;
            //Session["WebUploadFileName"] = null;

            populateBU();

            BindDummyRow();

            ibExportToExcel.Visible = false;
            //TODO: Check
            Session["FileID"] = null;
            Session["tempFileID"] = null;
            //if (ajaxFUAsset.HasFile)
            //{
            //    ajaxFUAsset.FileContent.Flush();
            //    ClearContents((Control)ajaxFUAsset);

            //}

            //if (string.IsNullOrEmpty(ajaxFUAsset.FileName))
            //{
            //    ibCreate.Enabled = false;
            //    ibUpload.Enabled = false;
            //}
        }
        else
        {
            populateAssetGrid();
            populateRackGrid();
        }



    }

    /// <summary>
    /// Used to call once page loading complete.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_LoadComplete(object sender, EventArgs e)
    {

    }

    #endregion

    # region Methods
    private void ClearContents(Control control)
    {
        for (var i = 0; i < Session.Keys.Count; i++)
        {
            if (Session.Keys[i].Contains(control.ClientID))
            {
                Session.Remove(Session.Keys[i]);
                break;
            }
        }
    }


    # endregion

    # region File Stats using dopostback -- commented

    public void RaisePostBackEvent(string eventArgument)
    {
        //UploadFile();
    }

    # endregion

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
        if (HttpContext.Current.Session["FileID"] == null)
            HttpContext.Current.Session["FileID"] = fileID;
        String timeStamp = (DateTime.Now).ToString("yyyyMMddHHmmssfff");
        //string fileName = ajaxFUAsset.FileName;
        string name = FileName.Substring(0, FileName.LastIndexOf("."));
        string extn = FileName.Substring(FileName.LastIndexOf(".") + 1);
        string assetFileName = name + "_" + timeStamp + "." + extn;

        string pathToCheck = savePath + assetFileName;
        // Create a temporary file name to use for checking duplicates.
        // Check to see if a file already exists with the
        // same name as the file to upload.        
        //if (System.IO.File.Exists(pathToCheck))
        //{
        //    int counter = 1;
        //    while (System.IO.File.Exists(pathToCheck))
        //    {
        //        // if a file with this name already exists,
        //        // prefix the filename with a number.
        //        tempfileName = counter.ToString() + assetFileName;
        //        pathToCheck = savePath + tempfileName;
        //        counter++;
        //    }
        //    assetFileName = tempfileName;


        //    // Notify the user that the file name was changed.
        //    lblDuplicateFile.Text = "A file with the same name already exists." +
        //    "<br />Your file was saved as " + assetFileName;
        //}

        if (File.Exists(HttpContext.Current.Session["WebUploadFilePath"].ToString() + FileName))
        {
            File.Copy(HttpContext.Current.Session["WebUploadFilePath"].ToString() + FileName, savePath + assetFileName, true);
            File.Delete(HttpContext.Current.Session["WebUploadFilePath"].ToString() + FileName);

        }

        try
        {
            dtData = importExcelData(savePath + assetFileName);
            if (dtData != null && dtData.Rows.Count > 0)
            {
                dsData.Tables.Add(dtData);

                if (HttpContext.Current.Session["ImportFilePath"] == null)
                    HttpContext.Current.Session["ImportFilePath"] = savePath + assetFileName;
                else
                {
                    HttpContext.Current.Session["ImportFilePath"] = HttpContext.Current.Session["ImportFilePath"].ToString() + "," + savePath + assetFileName;
                }
                return dsData.GetXml();
            }
            else
            {
                string msg = "Assets sheet not exists in " + FileName + " file.";
                HttpContext.Current.Response.StatusCode = 501;
                HttpContext.Current.Response.StatusDescription = msg;
                //HttpContext.Current.Response.End();
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
        int siteCount;
        int roomCount;
        int rowCount;
        int rackCount;
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
            dcreason.MaxLength = 1000;
            DataColumn dcID = new DataColumn("ID", typeof(int));
            //Asset Sheet
            dsExcel.Tables["Assets"].Columns.Add(dcID);
            dsExcel.Tables["Assets"].Columns.Add(dcStatus);
            dsExcel.Tables["Assets"].Columns.Add(dcreason);


            // put empty spaces or 0 for null data, in order to make them serailze
            int id = 0;
            id = 1;
            foreach (DataRow dr in dsExcel.Tables["Assets"].Rows)
            {

                for (int colIdx = 0; colIdx < dsExcel.Tables["Assets"].Columns.Count; colIdx++)
                {
                    //Change Serial no text to upper case
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "serialNumber")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }

                    }
                    //asset tag
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "assettag")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }

                    }
                    //asset name
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "assetname")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }

                    }

                    //host name
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "hostname")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }

                    }
                    //Change Site text to upper case
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "site")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    //room
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "room")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }

                    }
                    //row
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "row")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }

                    }
                    //rack
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "rack")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }

                    }

                    //owner
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "owner")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim();
                        }

                    }


                    //Change Manufacturer text to upper case
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "manufacturer")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }
                    //Change Model text to upper case 
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "model")
                    {
                        if (!string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = dr[colIdx].ToString().Trim().ToUpper();
                        }
                    }


                    //
                    // adds zero in t columns and space t ostring columns,
                    // this is to aviod null values being skipped by xml 
                    // serialization.
                    if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "startposition")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }

                    else if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName.ToLower() == "serialnumber")
                    {
                        if (string.IsNullOrEmpty(dr[colIdx].ToString().Trim()))
                        {
                            dr[colIdx] = "0";
                        }
                    }
                    else if (dsExcel.Tables["Assets"].Columns[colIdx].ColumnName != "ID" &&
                        string.IsNullOrEmpty(dr[colIdx].ToString()))
                    {
                        dr[colIdx] = " ";
                    }

                }
                dr["ID"] = id++;
            }


            // make sure RackTag and ExternalID columns of racks sheet are available
            DataColumn dcRackTag = new DataColumn("RackTag", typeof(string));
            DataColumn dcExternalID = new DataColumn("ExternalID", typeof(string));
            DataColumn dcSerialNo = new DataColumn("SerialNumber", typeof(string));
            DataColumn dcMfg = new DataColumn("Manufacturer", typeof(string));
            DataColumn dcModel = new DataColumn("Model", typeof(string));
            DataColumn rackStatus = new DataColumn("Status", typeof(string));
            DataColumn rackReason = new DataColumn("Reason", typeof(string));
            DataColumn rackID = new DataColumn("ID", typeof(int));

            if (!dsExcel.Tables["Racks"].Columns.Contains("SerialNumber"))
                dsExcel.Tables["Racks"].Columns.Add(dcSerialNo);

            if (!dsExcel.Tables["Racks"].Columns.Contains("Manufacturer"))
                dsExcel.Tables["Racks"].Columns.Add(dcMfg);

            if (!dsExcel.Tables["Racks"].Columns.Contains("Model"))
                dsExcel.Tables["Racks"].Columns.Add(dcModel);

            if (!dsExcel.Tables["Racks"].Columns.Contains("RackTag"))
                dsExcel.Tables["Racks"].Columns.Add(dcRackTag);

            if (!dsExcel.Tables["Racks"].Columns.Contains("ExternalID"))
                dsExcel.Tables["Racks"].Columns.Add(dcExternalID);

            //Racks Sheet
            dsExcel.Tables["Racks"].Columns.Add(rackID);
            dsExcel.Tables["Racks"].Columns.Add(rackStatus);
            dsExcel.Tables["Racks"].Columns.Add(rackReason);
            id = 1;
            foreach (DataRow dr in dsExcel.Tables["Racks"].Rows)
            {
                if (!string.IsNullOrEmpty(dr["Site"].ToString()))
                    dr["Site"] = dr["Site"].ToString().Trim();
                if (!string.IsNullOrEmpty(dr["Room"].ToString()))
                    dr["Room"] = dr["Room"].ToString().Trim();
                if (!string.IsNullOrEmpty(dr["Row"].ToString()))
                    dr["Row"] = dr["Row"].ToString().Trim();
                else
                    dr["Row"] = "";

                if (!string.IsNullOrEmpty(dr["Rack"].ToString()))
                    dr["Rack"] = dr["Rack"].ToString().Trim();
                else
                    dr["Rack"] = "";

                if (!string.IsNullOrEmpty(dr["Manufacturer"].ToString()))
                    dr["Manufacturer"] = dr["Manufacturer"].ToString().Trim();
                if (!string.IsNullOrEmpty(dr["Model"].ToString()))
                    dr["Model"] = dr["Model"].ToString().Trim();
                if (!string.IsNullOrEmpty(dr["SerialNumber"].ToString()))
                    dr["SerialNumber"] = dr["SerialNumber"].ToString().Trim();


                if (string.IsNullOrEmpty(dr["SerialNumber"].ToString()))
                    dr["SerialNumber"] = "";
                if (string.IsNullOrEmpty(dr["Manufacturer"].ToString()))
                    dr["Manufacturer"] = "";
                if (string.IsNullOrEmpty(dr["Model"].ToString()))
                    dr["Model"] = "";

                if (!string.IsNullOrEmpty(dr["RackTag"].ToString().Trim()))
                    dr["RackTag"] = dr["RackTag"].ToString().Trim().ToUpper();
                else
                    dr["RackTag"] = "";

                if (string.IsNullOrEmpty(dr["ExternalID"].ToString()))
                    dr["ExternalID"] = "";

                dr["Status"] = " ";
                dr["Reason"] = " ";
                dr["ID"] = id++;
            }

            dsExcel.AcceptChanges();


            //Serialize Asset Excel dataset to xml
            String strFile;
            try
            {
                if (HttpContext.Current.Session["FileID"] != null)
                {
                    strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileID"].ToString() + ".xml";
                    if (File.Exists(strFile) && new FileInfo(strFile).Length > 0)
                    {
                        //already file exists with data
                        // Read the content of the file into a DataSet
                        XmlTextReader xtr = new XmlTextReader(strFile);
                        DataSet dsPrevExcelData = new DataSet();
                        dsPrevExcelData.ReadXml(xtr);
                        xtr.Close();
                        if (dsPrevExcelData.Tables.Count > 0 && dsPrevExcelData.Tables["Assets"].Rows.Count > 0)
                        {
                            // Merge Asset Data
                            //Merge Base table
                            int curRowsCount = dsExcel.Tables["Assets"].Rows.Count + 1;
                            foreach (DataRow dr in dsPrevExcelData.Tables["Assets"].Rows)
                            {
                                DataRow newRow = dsExcel.Tables["Assets"].NewRow();
                                newRow["StartPosition"] = dr["StartPosition"].ToString().Trim();
                                newRow["AssetTag"] = dr["AssetTag"].ToString().Trim();
                                newRow["SerialNumber"] = dr["SerialNumber"].ToString().Trim();
                                newRow["Manufacturer"] = dr["Manufacturer"].ToString().Trim();
                                newRow["Model"] = dr["Model"].ToString().Trim();
                                newRow["HostName"] = dr["HostName"].ToString().Trim();
                                newRow["AssetName"] = dr["AssetName"].ToString().Trim();
                                newRow["Site"] = dr["Site"].ToString().Trim();
                                newRow["Room"] = dr["Room"].ToString().Trim();
                                newRow["Row"] = dr["Row"].ToString().Trim();
                                newRow["Rack"] = dr["Rack"].ToString().Trim();
                                newRow["Owner"] = dr["Owner"].ToString().Trim();
                                newRow["Orientation"] = dr["Orientation"].ToString().Trim();
                                newRow["ExternalID"] = dr["ExternalID"].ToString().Trim();
                                newRow["ID"] = curRowsCount++;
                                newRow["Status"] = "";
                                newRow["Reason"] = "";

                                dsExcel.Tables["Assets"].Rows.Add(newRow);
                            }

                            // Merge Racks Data
                            //Merge Base table
                            int curRackRowsCount = dsExcel.Tables["Racks"].Rows.Count + 1;
                            foreach (DataRow dr in dsPrevExcelData.Tables["Racks"].Rows)
                            {
                                DataRow newRow = dsExcel.Tables["Racks"].NewRow();

                                newRow["Site"] = dr["Site"].ToString().Trim();
                                newRow["Room"] = dr["Room"].ToString().Trim();
                                newRow["Row"] = dr["Row"].ToString().Trim();
                                newRow["Rack"] = dr["Rack"].ToString().Trim();

                                if (!string.IsNullOrEmpty(dr["SerialNumber"].ToString().Trim()))
                                    newRow["SerialNumber"] = dr["SerialNumber"].ToString().Trim();
                                else
                                    newRow["SerialNumber"] = "";

                                if (!string.IsNullOrEmpty(dr["Manufacturer"].ToString().Trim()))
                                    newRow["Manufacturer"] = dr["Manufacturer"].ToString().Trim();
                                else
                                    newRow["Manufacturer"] = "";

                                if (!string.IsNullOrEmpty(dr["Model"].ToString().Trim()))
                                    newRow["Model"] = dr["Model"].ToString().Trim();
                                else
                                    newRow["Model"] = "";

                                if (!string.IsNullOrEmpty(dr["RackTag"].ToString().Trim()))
                                    newRow["RackTag"] = dr["RackTag"].ToString().Trim();
                                else
                                    newRow["RackTag"] = "";

                                if (!string.IsNullOrEmpty(dr["ExternalID"].ToString().Trim()))
                                    newRow["ExternalID"] = dr["ExternalID"].ToString().Trim();
                                else
                                    newRow["ExternalID"] = "";

                                newRow["ID"] = curRackRowsCount++;
                                newRow["Status"] = "";
                                newRow["Reason"] = "";

                                dsExcel.Tables["Racks"].Rows.Add(newRow);
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


            dtExcel = dsExcel.Tables["Assets"];
            var recCount = 0;
            //Site Count
            dtSite = new DataView(dtExcel).ToTable(true, new string[] { "Site" });
            recCount = (from cr in dtSite.AsEnumerable()
                        where cr.Field<string>("Site").Trim() != ""
                        group cr by new
                        {
                            Site = cr.Field<String>("Site").ToLower()
                        }
                            into grp
                        select grp.First()).Count();
            siteCount = (int)recCount;

            DataRow drFS1 = dtFileStats.NewRow();
            drFS1["ID"] = "1";
            drFS1["Name"] = "No of Sites found";
            drFS1["Value"] = siteCount.ToString();
            dtFileStats.Rows.Add(drFS1);
            //Rooms Count
            dtRoom = new DataView(dtExcel).ToTable(true, new string[] { "Room", "Site" });
            recCount = (from cr in dtRoom.AsEnumerable()
                        where cr.Field<string>("Room").Trim() != ""
                        group cr by new
                        {
                            Site = cr.Field<String>("Site").ToLower(),
                            Room = cr.Field<String>("Room").ToLower()
                        }
                            into grp
                        select grp.First()).Count();

            roomCount = (int)recCount;

            DataRow drFS2 = dtFileStats.NewRow();
            drFS2["ID"] = "2";
            drFS2["Name"] = "No of Rooms found";
            drFS2["Value"] = roomCount.ToString();
            dtFileStats.Rows.Add(drFS2);

            //Rows Count
            dtRow = new DataView(dtExcel).ToTable(true, new string[] { "Row", "Room", "Site" });
            recCount = (from cr in dtRow.AsEnumerable()
                        where cr.Field<string>("Row").Trim() != ""
                        group cr by new
                        {
                            Site = cr.Field<String>("Site").ToLower(),
                            Row = cr.Field<String>("Row").ToLower(),
                            Room = cr.Field<String>("Room").ToLower()
                        }
                            into grp
                        select grp.First()).Count();
            rowCount = (int)recCount;

            DataRow drFS3 = dtFileStats.NewRow();
            drFS3["ID"] = "3";
            drFS3["Name"] = "No of Rows found";
            drFS3["Value"] = rowCount.ToString();
            dtFileStats.Rows.Add(drFS3);
            //Racks Count
            dtRack = new DataView(dtExcel).ToTable(true, new string[] { "Rack", "Row", "Room", "Site" });
            recCount = (from cr in dtRack.AsEnumerable()
                        where cr.Field<string>("Rack").Trim() != ""
                        group cr by new
                        {
                            Site = cr.Field<String>("Site").ToLower(),
                            Row = cr.Field<String>("Row").ToLower(),
                            Room = cr.Field<String>("Room").ToLower(),
                            Rack = cr.Field<String>("Rack").ToLower()
                        }
                            into grp
                        select grp.First()).Count();
            rackCount = (int)recCount;

            DataRow drFS4 = dtFileStats.NewRow();
            drFS4["ID"] = "4";
            drFS4["Name"] = "No of Racks found";
            drFS4["Value"] = rackCount.ToString();
            dtFileStats.Rows.Add(drFS4);


            //Manufacturer Count
            dtManufacturer = new DataView(dtExcel).ToTable(true, new string[] { "Manufacturer" });
            recCount = (from cr in dtManufacturer.AsEnumerable()
                        where cr.Field<string>("Manufacturer").Trim() != ""
                        group cr by new
                        {
                            Manufacturer = cr.Field<String>("Manufacturer").ToLower()
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
                            Model = cr.Field<String>("Model").ToLower()
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
            //dtAssetType = new DataView(dtExcel).ToTable(true, new string[] { "AssetType" });
            //recCount = (from cr in dtAssetType.AsEnumerable()
            //            where cr.Field<string>("AssetType").Trim() != ""
            //            group cr by new
            //           {
            //               AssetType = cr.Field<String>("AssetType").ToLower()
            //           }
            //                into grp
            //                select grp.First()).Count();

            //assetTypeCount = (int)recCount;

            //DataRow drFS7 = dtFileStats.NewRow();
            //drFS7["ID"] = "7";
            //drFS7["Name"] = "No of Asset Types found";
            //drFS7["Value"] = assetTypeCount.ToString();
            //dtFileStats.Rows.Add(drFS7);

            //Asset Count
            assetCount = dtExcel.Rows.Count;

            DataRow drFS8 = dtFileStats.NewRow();
            drFS8["ID"] = "8";
            drFS8["Name"] = "Total No of Assets";
            drFS8["Value"] = assetCount.ToString();
            dtFileStats.Rows.Add(drFS8);
            dtFileStats.AcceptChanges();

            //ibCreate.Visible = true;
            //ibCreate.Enabled = true;
        }
        return dtFileStats;
    }

    public static DataSet ImportExcel(string FileName)
    {
        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0 Xml;IMEX=1;HDR=YES;\" ";
        DataSet output = new DataSet();

        using (OleDbConnection conn = new OleDbConnection(strConn))
        {
            conn.Open();

            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            int tableNameColIndex = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToLower().CompareTo("table_name") == 0)
                {
                    tableNameColIndex = i;
                    break;
                }
            }

            string sheet = string.Empty;
            OleDbCommand cmd = null; ;
            DataTable outputTable;
            foreach (DataRow row in dt.Rows)
            {
                switch (row["Table_NAME"].ToString())
                {
                    case "Assets$":
                        sheet = "Assets$";
                        //if (sheet.Equals("Assets$", StringComparison.InvariantCultureIgnoreCase))
                        //{
                        //OleDbCommand cmdDel = new OleDbCommand(" Delete from [" + sheet + "] " + 
                        //    " where HostName IS  NULL AND SerialNumber IS  NULL AND Site IS  NULL AND Manufacturer IS  NULL " +
                        //    " AND Model IS  NULL AND AssetType IS  NULL AND Room IS  NULL AND Row IS  NULL " +
                        //    " AND Rack IS  NULL AND StartPosition IS  NULL AND NoofRus IS  NULL AND [Rack/Stand] IS  NULL ",conn);
                        //cmdDel.CommandType = CommandType.Text;
                        //int count = cmdDel.ExecuteNonQuery();

                        using (cmd = new OleDbCommand("SELECT * FROM [" + sheet + "] ", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            outputTable = new DataTable("Assets");
                            output.Tables.Add(outputTable);
                            new OleDbDataAdapter(cmd).Fill(outputTable);
                            //delete blank rows
                            output.Tables["Assets"].BeginLoadData();
                            (from cr in output.Tables["Assets"].AsEnumerable()
                             where cr.Field<string>("HostName") == null && cr.Field<string>("SerialNumber") == null &&
                             cr.Field<string>("Site") == null && cr.Field<string>("Manufacturer") == null &&
                             cr.Field<string>("Model") == null &&
                             cr.Field<string>("Room") == null && cr.Field<string>("Row") == null &&
                             cr.Field<string>("Rack") == null && cr.Field<string>("StartPosition") == null &&
                             cr.Field<string>("Orientation") == null && cr.Field<string>("Owner") == null
                             select cr).ToList().ForEach(cr => cr.Delete());
                            output.Tables["Assets"].EndLoadData();
                            output.Tables["Assets"].AcceptChanges();
                        }
                        break;
                    //}
                    //else if (sheet.Equals("Racks$", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    case "Racks$":
                        sheet = "Racks$";
                        cmd = new OleDbCommand("SELECT * FROM [" + sheet + "] ", conn);
                        cmd.CommandType = CommandType.Text;
                        outputTable = new DataTable("Racks");
                        output.Tables.Add(outputTable);
                        new OleDbDataAdapter(cmd).Fill(outputTable);
                        //delete blank rows
                        output.Tables["Racks"].BeginLoadData();
                        (from cr in output.Tables["Racks"].AsEnumerable()
                         where cr.Field<string>("Site") == null && cr.Field<string>("Room") == null &&
                         cr.Field<string>("Rack") == null &&
                         cr.Field<string>("Manufacturer") == null &&
                         cr.Field<string>("Model") == null &&
                         cr.Field<string>("RackTag") == null &&
                         cr.Field<string>("ExternalID") == null
                         select cr).ToList().ForEach(cr => cr.Delete());
                        output.Tables["Racks"].EndLoadData();
                        output.Tables["Racks"].AcceptChanges();
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

        Session["WebUploadFilePath"] = e.FolderPath;
        //Session["WebUploadFileName"] = e.FileName;

    }

    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        bool bIsBUInserted = false;
        bool bAreSitesAssigned = false;
        ArrayList alSitesInserted = new ArrayList();


        // inserts company
        bIsBUInserted = insertBU();

        if (bIsBUInserted == true)
        {
            DataSet dsExcel = new DataSet();
            dsExcel = DeserializeDataSource();
            /*   Manufacturer and Model data will be retrived from DB 
             * No Creation of Manufacturer and Model from Excel data
             */
            //Manufacturers
            insertManufacturer("Racks");
            //Models
            insertModel("Racks");

            if (dsExcel.Tables.Contains("Racks") && dsExcel.Tables["Racks"].Rows.Count > 0)
            {
                /*
                 * Site, Room, Row and Rack will be created based on Racks Sheet data
                 * If already exists, system will just update the information
                 * If a rack referenced in Assets sheet and not defined in Racks sheet than 
                 * Asset creation will fail, Location details in Assets sheet is just for refernce.
                 */
                //-----------//Racks Sheet

                //this section is not executed for Tenant User, that means tenant user can't create new site
                alSitesInserted = insertSites("Racks");
                if (alSitesInserted.Count > 0)
                {
                    //If the new sites are inserted then Assign the sites to the Business unit.
                    bAreSitesAssigned = assignSites2BU(alSitesInserted);
                }

                //locations
                insertLocations("Racks");
            }

            if (dsExcel.Tables.Contains("Assets") && dsExcel.Tables["Assets"].Rows.Count > 0)
            {
                //get Site and Location details from database 
                if (!dsExcel.Tables.Contains("Racks"))
                {
                    GetSiteandLocationDetails();
                }


                //-----------//Assets Sheet
                //alSitesInserted = insertSites("Assets");
                //if (alSitesInserted.Count > 0)
                //{
                //    //If the new sites are inserted then Assign the sites to the Business unit.
                //    bAreSitesAssigned = assignSites2BU(alSitesInserted);
                //}

                //locations
                //Locations must be created using Racks Sheet only - 23June2017
                //insertLocations("Assets");
                ////Manufacturers
                //insertManufacturer("Assets");
                ////Models
                //insertModel("Assets");


                /*
                 * Asset Type/ Model Type data from DB will be loaded and No Creation 
                 * 
                 */
                // Asset Types
                insertAssetType();
                /*Owner/ Custodian
                 * Created or update of owner information
                 */
                insertOwner();
                /*Assets
                 * Import assets based data in Assets sheet
                 */
                insertAssets();

                //populate grid
                if (Session["FileID"] != null)
                    Session["tempFileID"] = Session["FileID"].ToString();
                populateAssetGrid();
                populateRackGrid();
                ibCreate.Enabled = false;
                FileStats.Visible = true;
                FileStats.Style.Add("display", "block");
                UploadStats.Style.Add("display", "block");
                ShowFileData();
                ResetAll();
            }
        }
    }

    private void GetSiteandLocationDetails()
    {
        //Sites
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataSet dsAllLocations = new DataSet();
        DataTable dtAllLocNames = new DataTable();
        DataTable dtSL = new DataTable();
        //Retreive all the sites from the database.
        DataSet dsAllSites = new DataSet();
        DataTable dtAllSiteNames = new DataTable();
        objSite = new iAssetTrack.BAL.SitesBAL();
        dsAllSites = objSite.retrieveAllSites();
        //Get the SiteIDs and SiteName into table dtAllSiteNames
        var query = (from rows in dsAllSites.Tables[0].AsEnumerable()
                     orderby rows.Field<int>("SiteID")
                     select new
                     {
                         siteID = rows.Field<int>("SiteID"),
                         Site = rows.Field<string>("Site")
                     });
        dtAllSiteNames = LINQToDataTable(query, "dtSite");
        if (dtAllSiteNames.Columns.Count == 0)
        {
            dtAllSiteNames.Columns.Add("siteID", typeof(int));
            dtAllSiteNames.Columns.Add("Site", typeof(string));
            dtAllSiteNames.AcceptChanges();
        }

        if (!dsExcelData.Tables.Contains(dtAllSiteNames.TableName))
            dsExcelData.Tables.Add(dtAllSiteNames);

        dsExcelData.AcceptChanges();

        //Locations
        //Retreive All locations and add it to the excel dataset

        DataTable dtSite = dsExcelData.Tables["dtSite"];
        objLocation = new iAssetTrack.BAL.LocationBAL();
        dsAllLocations = objLocation.retrieveAllLocations();
        //get SiteLocationAssignment details
        SiteLocationAssignmentBAL objSL = new SiteLocationAssignmentBAL();
        //objSL.BusinessUnitID = 0;
        //objSL.SiteID = 0;
        dtSL = objSL.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            string tenantAssignedLocations = string.Empty;
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedLocations = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDLOCATIONS].ToString();
            }

            DataTable tempTable = dsAllLocations.Tables[0].Clone();
            foreach (DataRow dr in dsAllLocations.Tables[0].Rows)
            {
                tempTable.Rows.Add(dr.ItemArray);
            }
            DataRow[] filteredRows = tempTable.Select("LocationID IN (" + tenantAssignedLocations + ")");
            dsAllLocations.Tables[0].Rows.Clear();
            if (filteredRows != null && filteredRows.Length > 0)
            {
                foreach (DataRow dr in filteredRows)
                {
                    dsAllLocations.Tables[0].Rows.Add(dr.ItemArray);
                }

            }

        }

        var query1 = (from rows in dsAllLocations.Tables[0].AsEnumerable()
                      orderby rows.Field<int>("LocationID")
                      select new
                      {
                          LocationID = rows.Field<int>("LocationID"),
                          Location = rows.Field<string>("Location"),
                          LocationType = rows.Field<string>("LocationType"),
                          Site = ((from crSL in dtSL.AsEnumerable()
                                   where crSL.Field<int>("LocationID") == rows.Field<int>("LocationID")
                                   select crSL).Count() > 0 ?
                                  (from crSL in dtSL.AsEnumerable()
                                   where crSL.Field<int>("LocationID") == rows.Field<int>("LocationID")
                                   select crSL.Field<string>("Site")).First() : " "),
                          ParentLocationID = Convert.ToString(rows.Field<Nullable<int>>("ParentLocationID")),
                          Manufacturer = rows.Field<string>("Manufacturer"),
                          Model = rows.Field<string>("Model"),
                          SerialNumber = rows.Field<string>("SerialNumber"),
                          ModelID = rows.Field<Nullable<int>>("ModelID"),
                          ExternalID = rows.Field<string>("ExternalID"),
                          TagID = rows.Field<string>("TagID")
                      });
        dtAllLocNames = LINQToDataTable(query1, "dtLocation");

        if (dsExcelData.Tables.Contains("dtLocation"))
            dsExcelData.Tables.Remove("dtLocation");

        if (dtAllLocNames.Columns.Count == 0)
        {
            dtAllLocNames.Columns.Add("LocationID", typeof(int));
            dtAllLocNames.Columns.Add("Location", typeof(string));
            dtAllLocNames.Columns.Add("LocationType", typeof(string));
            dtAllLocNames.Columns.Add("Site", typeof(string));
            dtAllLocNames.Columns.Add("ParentLocationID", typeof(int));
            dtAllLocNames.Columns.Add("ExternalID", typeof(string));
            dtAllLocNames.Columns.Add("Manufacturer", typeof(string));
            dtAllLocNames.Columns.Add("Model", typeof(string));
            dtAllLocNames.Columns.Add("TagID", typeof(string));
            dtAllLocNames.Columns.Add("SerialNumber", typeof(string));
            dtAllLocNames.Columns.Add("ModelID", typeof(int));

            dtAllLocNames.AcceptChanges();
        }
        //Get the new locations from the Import Asset Excel
        if (!dsExcelData.Tables.Contains(dtAllLocNames.TableName))
            dsExcelData.Tables.Add(dtAllLocNames);

        dsExcelData.AcceptChanges();
        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
    }

    private void ResetAll()
    {
        Session["WebUploadFilePath"] = null;
        ibExportToExcel.Visible = true;
        Session["FileID"] = null;
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

        //this.eExporter.WorkbookFormat = excelFormat;
        //this.eExporter.Export(true, this.grdAsset, this.grdRacks);

        WebControl[] array = new WebControl[2];
        array[0] = grdAsset;
        array[1] = grdRacks;

        //define the workbook and some worksheets. 
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        Infragistics.Documents.Excel.Workbook book = new Infragistics.Documents.Excel.Workbook(excelFormat);
        book.Worksheets.Add("Assets");
        book.Worksheets.Add("Racks");

        //first export the grids to each worksheet. Custom export mode is required for that.
        this.eExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;

        this.eExporter.Export(grdAsset, book.Worksheets[0]);
        this.eExporter.Export(grdRacks, book.Worksheets[1]);

        //change the export mode back to Download
        this.eExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;

        //export the workbook and pass it an empty array (as the grids are already in the worksheets).
        this.eExporter.Export(book, 0, 0, new WebControl[0]);

    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        //int iWSdex = e.Worksheet.Index;
        //int iRdex = e.CurrentRowIndex;
        //int iCdex = e.CurrentColumnIndex;
        //e.Worksheet.Columns[3].Width = 1;
        //e.Worksheet.Columns[4].Width = 1;
        //if (iWSdex == 0)
        //{
        //    if (iRdex == 0)
        //    {

        //        if (iCdex == 3 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
        //        {
        //            e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


        //        }
        //        if (iCdex == 4 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
        //        {
        //            e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";


        //        }

        //    }

        //}
    }

    #endregion


    #region "User Defined Methods"

    protected void ShowFileData()
    {
        int siteCount;
        int roomCount;
        int rowCount;
        int rackCount;
        int assetCount;
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
        dtExcel = dsExcelData.Tables["Assets"];


        //Site Count
        dtSite = new DataView(dtExcel).ToTable(true, new string[] { "Site" });
        recCount = (from cr in dtSite.AsEnumerable()
                    where cr.Field<string>("Site").Trim() != ""
                    select cr).Count();
        siteCount = (int)recCount;

        DataRow drFS1 = dtFileStats.NewRow();
        drFS1["ID"] = "1";
        drFS1["Name"] = "No of Sites found";
        drFS1["Value"] = siteCount.ToString();
        dtFileStats.Rows.Add(drFS1);
        //Rooms Count
        dtRoom = new DataView(dtExcel).ToTable(true, new string[] { "Room" });
        recCount = (from cr in dtRoom.AsEnumerable()
                    where cr.Field<string>("Room").Trim() != ""
                    select cr).Count();
        roomCount = (int)recCount;

        DataRow drFS2 = dtFileStats.NewRow();
        drFS2["ID"] = "2";
        drFS2["Name"] = "No of Rooms found";
        drFS2["Value"] = roomCount.ToString();
        dtFileStats.Rows.Add(drFS2);

        //Rows Count
        dtRow = new DataView(dtExcel).ToTable(true, new string[] { "Row" });
        recCount = (from cr in dtRow.AsEnumerable()
                    where cr.Field<string>("Row").Trim() != ""
                    select cr).Count();
        rowCount = (int)recCount;

        DataRow drFS3 = dtFileStats.NewRow();
        drFS3["ID"] = "3";
        drFS3["Name"] = "No of Rows found";
        drFS3["Value"] = rowCount.ToString();
        dtFileStats.Rows.Add(drFS3);
        //Racks Count
        dtRack = new DataView(dtExcel).ToTable(true, new string[] { "Rack" });
        recCount = (from cr in dtRack.AsEnumerable()
                    where cr.Field<string>("Rack").Trim() != ""
                    select cr).Count();
        rackCount = (int)recCount;

        DataRow drFS4 = dtFileStats.NewRow();
        drFS4["ID"] = "4";
        drFS4["Name"] = "No of Racks found";
        drFS4["Value"] = rackCount.ToString();
        dtFileStats.Rows.Add(drFS4);


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
        drFS6["Name"] = "No of Asset Models found";
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
        //drFS7["Name"] = "No of Asset Types found";
        //drFS7["Value"] = assetTypeCount.ToString();
        //dtFileStats.Rows.Add(drFS7);

        //Asset Count
        assetCount = dtExcel.Rows.Count;

        DataRow drFS8 = dtFileStats.NewRow();
        drFS8["ID"] = "8";
        drFS8["Name"] = "Total No of Assets";
        drFS8["Value"] = assetCount.ToString();
        dtFileStats.Rows.Add(drFS8);
        dtFileStats.AcceptChanges();

        grdFileStats.DataSource = dtFileStats;
        grdFileStats.DataBind();

        FileStats.Visible = true;

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
        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
        return bIsBUInserted;
    }

    public ArrayList insertSites(string TableName)
    {
        string siteName = string.Empty;
        ArrayList alSitesToInsert = new ArrayList();
        Dictionary<int, string> dictSitesInserted = new Dictionary<int, string>();
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        //Retreive all the sites from the database.
        DataSet dsAllSites = new DataSet();
        DataTable dtAllSiteNames = new DataTable();
        objSite = new iAssetTrack.BAL.SitesBAL();
        dsAllSites = objSite.retrieveAllSites();
        //Get the SiteIDs and SiteName into table dtAllSiteNames
        var query = (from rows in dsAllSites.Tables[0].AsEnumerable()
                     orderby rows.Field<int>("SiteID")
                     select new
                     {
                         siteID = rows.Field<int>("SiteID"),
                         Site = rows.Field<string>("Site")
                     });
        dtAllSiteNames = LINQToDataTable(query, "dtSite");
        if (dtAllSiteNames.Columns.Count == 0)
        {
            dtAllSiteNames.Columns.Add("siteID", typeof(int));
            dtAllSiteNames.Columns.Add("Site", typeof(string));
            dtAllSiteNames.AcceptChanges();
        }
        //Get the new sites from the Import Asset Excel

        DataTable dtTable = new DataTable();
        DataTable dtSitesInExcel = new DataTable();
        DataTable dtSiteResult = new DataTable();
        dtTable = dsExcelData.Tables[TableName];
        dtSitesInExcel = new DataView(dtTable).ToTable(true, new string[] { "Site" });
        //Check if the site already exist in database.
        if (!bool.Parse(Session["TenantUser"].ToString()))
        {
            //Site creation is allowed when current user is not tenant user.
            foreach (DataRow dr in dtSitesInExcel.Rows)
            {
                if (!string.IsNullOrEmpty(dr["Site"].ToString().Trim()))
                {
                    siteName = dr["Site"].ToString();
                    Regex regx = new Regex(@"^[\w0-9\-\.]+([\w0-9\-\.]+)*$");
                    if (regx.IsMatch(siteName.Trim()) && siteName.Trim().Length <= 25)
                    {
                        IEnumerable<DataRow> drResult = from siteRow in dtAllSiteNames.AsEnumerable()
                                                        where siteRow.Field<string>("Site").Equals(siteName, StringComparison.InvariantCultureIgnoreCase)
                                                        select siteRow;

                        //if the site is not present in database
                        if (drResult.Count() == 0)
                        {
                            //Insert the sites
                            try
                            {
                                objSite = new iAssetTrack.BAL.SitesBAL();
                                objSite.Site = siteName;
                                objSite.Description = siteName;
                                objSite.Status = 1;
                                objSite.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                objSite.Persist(DALCOperation.Insert);
                                dictSitesInserted.Add(objSite.SiteID, objSite.Site);
                                alSitesToInsert.Add(siteName);
                            }
                            catch (Exception ex)
                            {
                                ExceptionPolicy.HandleException(ex, "errDCTrack");
                            }
                        }
                        //if the site already exist in database
                        else
                        {
                            try
                            {
                                //Site Already exists.
                                dtSiteResult = drResult.CopyToDataTable();
                                objSite = new iAssetTrack.BAL.SitesBAL();
                                objSite.SiteID = Convert.ToInt32(dtSiteResult.Rows[0]["siteID"]);
                                objSite.Site = siteName;
                                objSite.Description = siteName;
                                objSite.Status = 1;
                                objSite.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                objSite.Persist(DALCOperation.Update);
                                alSitesToInsert.Add(siteName);
                            }
                            catch (Exception ex)
                            {
                                ExceptionPolicy.HandleException(ex, "errDCTrack");
                            }

                        }
                    }
                }
            }
        }
        //Loop through the Dictionary(with the siteIds and Sites) of the sites inserted
        if (dictSitesInserted.Values.Count != 0)
        {
            foreach (KeyValuePair<int, string> pair in dictSitesInserted)
            {
                DataRow dr = dtAllSiteNames.NewRow();
                dr["SiteID"] = pair.Key;
                dr["site"] = pair.Value;
                dtAllSiteNames.Rows.Add(dr);
            }
        }
        if (!dsExcelData.Tables.Contains(dtAllSiteNames.TableName))
            dsExcelData.Tables.Add(dtAllSiteNames);
        else
        {
            var idsNotInNew = dsExcelData.Tables["dtSite"].AsEnumerable().Select(r => int.Parse(r.Field<string>("siteID")))
                            .Except(dtAllSiteNames.AsEnumerable().Select(r => r.Field<int>("siteID")));
            var remData = (from row in dsExcelData.Tables["dtSite"].AsEnumerable()
                           join siteID in idsNotInNew
                           on int.Parse(row.Field<string>("siteID")) equals siteID
                           select row);
            if (remData.Count() > 0)
            {
                DataTable remDataTable = remData.CopyToDataTable();

                foreach (DataRow remRow in remDataTable.Rows)
                {
                    dsExcelData.Tables["dtSite"].Rows.Add(remRow.ItemArray);
                }
            }
        }
        dsExcelData.AcceptChanges();
        //Serialize the dataset dsExcel data - with 3 tables(Excel data, BudsinessUnit table and Site table)
        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
        //Return inserted sites
        return alSitesToInsert;

    }

    public bool assignSites2BU(ArrayList alSitesToAssign)
    {
        bool bIsAssigned = false;
        ArrayList siteIDs = new ArrayList();
        objBU = new iAssetTrack.BAL.BusinessUnitBAL();
        objBUSite = new iAssetTrack.BAL.BUSiteAssignmentBAL();

        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtBU = new DataTable();
        DataTable dtSites = new DataTable();
        DataTable dtResultBU = new DataTable();
        DataTable dtResultSite = new DataTable();
        dtBU = dsExcelData.Tables["dtBusinessUnit"];
        dtSites = dsExcelData.Tables["dtSite"];
        string strBU = txtBU.Text;

        //Retrive the BusinessUnitID
        IEnumerable<DataRow> resultBU = from BURow in dtBU.AsEnumerable()
                                        where BURow.Field<string>("BusinessUnit").Equals(strBU,
                                        StringComparison.InvariantCultureIgnoreCase)
                                        select BURow;
        if (resultBU.Count() > 0)
        {
            dtResultBU = resultBU.CopyToDataTable();
        }
        //Retrive the SIteIDs
        foreach (string site in alSitesToAssign)
        {
            IEnumerable<DataRow> resultSite = from SiteRow in dtSites.AsEnumerable()
                                              where SiteRow.Field<string>("Site").Equals(site, StringComparison.InvariantCultureIgnoreCase)
                                              select SiteRow;
            if (resultSite.Count() > 0)
            {
                dtResultSite = resultSite.CopyToDataTable();
            }
            siteIDs.Add(Convert.ToInt32(dtResultSite.Rows[0][DBFields.DBFIELD_SITEID]));
        }

        if (dtResultBU.Rows.Count != 0 && siteIDs.Count != 0)
        {
            try
            {
                objBUSite.BusinessUnitID = Convert.ToInt32(dtResultBU.Rows[0][DBFields.DBFIELD_BUSINESSUNITID]);
                objBUSite.SiteAccessID = 0;
                ArrayList strSiteIds = new ArrayList();
                // get already assigned sites
                DataSet dsAssignedSites = objBUSite.retrieveAssignSite();
                if (dsAssignedSites != null && dsAssignedSites.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsAssignedSites.Tables[0].Rows)
                    {
                        strSiteIds.Add(dr["SiteID"].ToString());
                    }
                }
                //add new Site ids with the existing ones.
                for (int i = 0; i < siteIDs.Count; i++)
                {
                    if (!strSiteIds.Contains(siteIDs[i].ToString()))
                    {
                        strSiteIds.Add(siteIDs[i].ToString());
                    }
                }
                string[] arrIDs = (string[])strSiteIds.ToArray(typeof(string));
                objBUSite.SiteIDs = string.Join(";", arrIDs);
                objBUSite.Delimiters = ";";
                objBUSite.Status = 1;
                objBUSite.CreatedBy = Convert.ToInt32(Session["UserID"]);
                objBUSite.Persist(DALCOperation.Insert);
                bIsAssigned = true;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "errDCTrack");
            }
        }
        return bIsAssigned;
    }

    public void insertLocations(string TableName)
    {
        //Retreive All locations and add it to the excel dataset
        DataSet dsAllLocations = new DataSet();
        DataTable dtAllLocNames = new DataTable();
        DataTable dtSL = new DataTable();
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtSite = dsExcelData.Tables["dtSite"];
        objLocation = new iAssetTrack.BAL.LocationBAL();
        dsAllLocations = objLocation.retrieveAllLocations();
        //get SiteLocationAssignment details
        SiteLocationAssignmentBAL objSL = new SiteLocationAssignmentBAL();
        //objSL.BusinessUnitID = 0;
        //objSL.SiteID = 0;
        dtSL = objSL.retrieve().Tables[0];

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            string tenantAssignedLocations = string.Empty;
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantAssignedLocations = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDLOCATIONS].ToString();
            }

            DataTable tempTable = dsAllLocations.Tables[0].Clone();
            foreach (DataRow dr in dsAllLocations.Tables[0].Rows)
            {
                tempTable.Rows.Add(dr.ItemArray);
            }

            DataRow[] filteredRows0 = tempTable.Select("LocationID IN (" + tenantAssignedLocations + ")");

            //Get Parent - Rows
            ArrayList parentLocIds0 = new ArrayList();
            foreach (DataRow dr in filteredRows0)
            {
                if (!string.IsNullOrEmpty(dr["ParentLocationID"].ToString()) && dr["ParentLocationID"].ToString().CompareTo("0") != 0)
                {
                    if (!parentLocIds0.Contains(dr["ParentLocationID"].ToString()))
                        parentLocIds0.Add(dr["ParentLocationID"].ToString());
                }
            }
            DataRow[] filteredRows1 = tempTable.Select("LocationID IN (" + string.Join(",", parentLocIds0.ToArray()) + ")");
            //Get Parent - Rooms
            ArrayList parentLocIds1 = new ArrayList();
            foreach (DataRow dr in filteredRows1)
            {
                if (!string.IsNullOrEmpty(dr["ParentLocationID"].ToString()) && dr["ParentLocationID"].ToString().CompareTo("0") != 0)
                {
                    if (!parentLocIds1.Contains(dr["ParentLocationID"].ToString()))
                        parentLocIds1.Add(dr["ParentLocationID"].ToString());
                }
            }
            DataRow[] filteredRows2 = null;
            if (parentLocIds1.Count > 0)
            {
                filteredRows2 = tempTable.Select("LocationID IN (" + string.Join(",", parentLocIds1.ToArray()) + ")");
            }

            dsAllLocations.Tables[0].Rows.Clear();
            if (filteredRows0 != null && filteredRows0.Length > 0)
            {
                foreach (DataRow dr in filteredRows0)
                {
                    dsAllLocations.Tables[0].Rows.Add(dr.ItemArray);
                }
            }
            if (filteredRows1 != null && filteredRows1.Length > 0)
            {
                foreach (DataRow dr in filteredRows1)
                {
                    if (dsAllLocations.Tables[0].Select(" LocationID = " + dr["LocationID"].ToString()).Length < 1)
                        dsAllLocations.Tables[0].Rows.Add(dr.ItemArray);
                }
            }
            if (filteredRows2 != null && filteredRows2.Length > 0)
            {
                foreach (DataRow dr in filteredRows2)
                {
                    if (dsAllLocations.Tables[0].Select(" LocationID = " + dr["LocationID"].ToString()).Length < 1)
                        dsAllLocations.Tables[0].Rows.Add(dr.ItemArray);
                }
            }

        }

        var query = (from rows in dsAllLocations.Tables[0].AsEnumerable()
                     orderby rows.Field<int>("LocationID")
                     select new
                     {
                         LocationID = rows.Field<int>("LocationID"),
                         Location = rows.Field<string>("Location"),
                         LocationType = rows.Field<string>("LocationType"),
                         Site = ((from crSL in dtSL.AsEnumerable()
                                  where crSL.Field<int>("LocationID") == rows.Field<int>("LocationID")
                                  select crSL).Count() > 0 ?
                                 (from crSL in dtSL.AsEnumerable()
                                  where crSL.Field<int>("LocationID") == rows.Field<int>("LocationID")
                                  select crSL.Field<string>("Site")).First() : " "),
                         ParentLocationID = Convert.ToString(rows.Field<Nullable<int>>("ParentLocationID")),
                         Manufacturer = rows.Field<string>("Manufacturer"),
                         Model = rows.Field<string>("Model"),
                         SerialNumber = rows.Field<string>("SerialNumber"),
                         ModelID = rows.Field<Nullable<int>>("ModelID"),
                         ExternalID = rows.Field<string>("ExternalID"),
                         TagID = rows.Field<string>("TagID")
                     });
        dtAllLocNames = LINQToDataTable(query, "dtLocation");

        if (dsExcelData.Tables.Contains("dtLocation"))
            dsExcelData.Tables.Remove("dtLocation");

        if (dtAllLocNames.Columns.Count == 0)
        {
            dtAllLocNames.Columns.Add("LocationID", typeof(int));
            dtAllLocNames.Columns.Add("Location", typeof(string));
            dtAllLocNames.Columns.Add("LocationType", typeof(string));
            dtAllLocNames.Columns.Add("Site", typeof(string));
            dtAllLocNames.Columns.Add("ParentLocationID", typeof(int));
            dtAllLocNames.Columns.Add("ExternalID", typeof(string));
            dtAllLocNames.Columns.Add("Manufacturer", typeof(string));
            dtAllLocNames.Columns.Add("Model", typeof(string));
            dtAllLocNames.Columns.Add("TagID", typeof(string));
            dtAllLocNames.Columns.Add("SerialNumber", typeof(string));
            dtAllLocNames.Columns.Add("ModelID", typeof(int));

            dtAllLocNames.AcceptChanges();
        }
        //Get the new locations from the Import Asset Excel
        if (!dsExcelData.Tables.Contains(dtAllLocNames.TableName))
            dsExcelData.Tables.Add(dtAllLocNames);
        else
        {
            var idsNotInNew = dsExcelData.Tables["dtLocation"].AsEnumerable().Select(r => int.Parse(r.Field<string>("LocationID")))
                            .Except(dtAllLocNames.AsEnumerable().Select(r => r.Field<int>("LocationID")));
            var remData = (from row in dsExcelData.Tables["dtLocation"].AsEnumerable()
                           join locationID in idsNotInNew
                           on int.Parse(row.Field<string>("LocationID")) equals locationID
                           select row);
            if (remData.Count() > 0)
            {
                DataTable remDataTable = remData.CopyToDataTable();

                foreach (DataRow remRow in remDataTable.Rows)
                {
                    dsExcelData.Tables["dtLocation"].Rows.Add(remRow.ItemArray);
                }
            }
        }




        dsExcelData.AcceptChanges();
        //Serialize the dataset dsExcel data - with 4 tables(Excel data,BusinessUnit table,Site table and Location Table)
        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            string tenantType = string.Empty;
            //tenant user
            // for tenant user, based on tenant type insert of Room ,Row and Rack are allowed.
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantType = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_TYPE].ToString();
            }
            switch (tenantType.ToLower())
            {
                case "room":
                    insertRooms(TableName);
                    insertRows(TableName);
                    insertRacks(TableName);
                    break;
                case "row":
                    insertRows(TableName);
                    insertRacks(TableName);
                    break;
                case "rack":
                    insertRacks(TableName);
                    break;
            }
        }
        else
        {
            // Non tenant user
            insertRooms(TableName);
            insertRows(TableName);
            insertRacks(TableName);

        }
    }

    public void insertRooms(string TableName)
    {

        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtTable = new DataTable();
        DataTable dtRooms = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable drLocResult = new DataTable();
        dtTable = dsExcelData.Tables[TableName];
        dtLocation = dsExcelData.Tables["dtLocation"];

        Dictionary<int, string> dictRoomsInserted = new Dictionary<int, string>();
        Dictionary<string, string> siteLocAssign = new Dictionary<string, string>();

        if (dtLocation == null)
        {
            dtLocation = new DataTable("dtLocation");
            dtLocation.Columns.Add("LocationID", typeof(int));
            dtLocation.Columns.Add("Location", typeof(string));
            dtLocation.Columns.Add("LocationType", typeof(string));
            dtLocation.Columns.Add("Site", typeof(string));
            dtLocation.Columns.Add("ParentLocationID", typeof(int));
            dtLocation.Columns.Add("Manufacturer", typeof(string));
            dtLocation.Columns.Add("Model", typeof(string));
            dtLocation.Columns.Add("TagID", typeof(int));
            dtLocation.Columns.Add("SerialNumber", typeof(string));
            dtLocation.Columns.Add("ModelID", typeof(int));
            dtLocation.AcceptChanges();

        }

        //Get the distinct RowIDs with all the columns from the excel
        IEnumerable<DataRow> query = (from rooms in dtTable.AsEnumerable()
                                      group rooms by new
                                      {
                                          Room = rooms.Field<String>("Room"),
                                          Site = rooms.Field<String>("Site")
                                      }
                                          into grp
                                      select grp.First());

        // Create a table from the query.
        if (query.Count() > 0)
            dtRooms = query.CopyToDataTable();

        // Location type value, get from DB
        LocationTypeBAL obLT = new LocationTypeBAL();
        DataSet dsLT = obLT.retrieve();
        int locationTypeID = 0;
        var locationTypeList = (from lt in dsLT.Tables[0].AsEnumerable()
                                where lt.Field<string>("LocationType").Equals("Room",
                                StringComparison.InvariantCultureIgnoreCase)
                                select lt.Field<int>("LocationTypeID")
                                   );
        if (locationTypeList.Count() > 0)
            locationTypeID = (int)locationTypeList.First();

        try
        {
            foreach (DataRow dr in dtRooms.Rows)
            {
                string roomName = dr["Room"].ToString();
                // if room name contains dispose or decom keywords than creation will be cancelled.
                if (!roomName.Trim().ToLower().Contains("dispose") && !roomName.Trim().ToLower().Contains("decom"))
                {
                    Regex regx = new Regex(@"^[\w0-9\-\.]+(\s?[\w0-9\-\.]+)*$");
                    if (regx.IsMatch(roomName.Trim()) && roomName.Trim().Length <= 50)
                    {
                        string siteName = dr["Site"].ToString();
                        if (!string.IsNullOrEmpty(roomName.Trim()) &&
                               !string.IsNullOrEmpty(siteName.Trim()))
                        {
                            if (dtLocation.Rows.Count > 0)
                            {
                                IEnumerable<DataRow> drResult = from locRoom in dtLocation.AsEnumerable()
                                                                where locRoom.Field<string>("Location").Equals(roomName,
                                                                StringComparison.InvariantCultureIgnoreCase) &&
                                                                locRoom.Field<string>("LocationType").Equals("Room",
                                                                StringComparison.InvariantCultureIgnoreCase) &&
                                                                 locRoom.Field<string>("Site").Equals(siteName,
                                                                StringComparison.InvariantCultureIgnoreCase)
                                                                select locRoom;


                                if (!isLocationCreateAllowed(roomName, "room"))
                                    continue;

                                if (drResult.Count() == 0)
                                {
                                    //Hardcoded values for IpAddress, IsExitDoor, IsCheckOutLocation
                                    //ParentLocationID = 0 for Room
                                    //LocationTypeId for LocationType - "Room" is 4
                                    objLocation = new iAssetTrack.BAL.LocationBAL();
                                    objLocation.Location = roomName;
                                    objLocation.Description = roomName;
                                    objLocation.IpAddress = string.Empty;
                                    objLocation.ParentLocationID = 0;
                                    objLocation.LocationTypeID = locationTypeID;
                                    objLocation.IsExitDoor = 0;
                                    objLocation.IsCheckOutLocation = 1;
                                    objLocation.Status = 1;
                                    objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                    objLocation.ExternalID = "";
                                    objLocation.Persist(DALCOperation.Insert);
                                    // SiteName ## LocationName, to make sure that site-location combination unique.
                                    dictRoomsInserted.Add(objLocation.LocationID, siteName + "##" + objLocation.Location);

                                    if (siteLocAssign.ContainsKey(siteName))
                                    {
                                        siteLocAssign[siteName] = siteLocAssign[siteName] + objLocation.LocationID.ToString() + ";";
                                    }
                                    else
                                    {
                                        siteLocAssign.Add(siteName, objLocation.LocationID.ToString() + ";");
                                    }

                                }
                                else
                                {
                                    drLocResult = drResult.CopyToDataTable();
                                    LocationBAL objL = new LocationBAL();
                                    if (objL.HasChildNodes(Convert.ToInt32(drLocResult.Rows[0]["LocationID"])) == 0)
                                    {
                                        //Location "room" already exists
                                        objLocation = new iAssetTrack.BAL.LocationBAL();
                                        objLocation.LocationID = Convert.ToInt32(drLocResult.Rows[0]["LocationID"]);
                                        objLocation.Location = roomName;
                                        objLocation.Description = siteName + " " + roomName;
                                        objLocation.IpAddress = string.Empty;
                                        objLocation.ParentLocationID = 0;
                                        objLocation.LocationTypeID = (int)locationTypeID;
                                        objLocation.IsExitDoor = 0;
                                        objLocation.IsCheckOutLocation = 1;
                                        objLocation.Status = 1;
                                        objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                        objLocation.ExternalID = "";
                                        objLocation.Persist(DALCOperation.Update);

                                        dictRoomsInserted.Add(objLocation.LocationID, siteName + "##" + objLocation.Location);

                                        SiteLocationAssignmentBAL objSLAssignBAL = new SiteLocationAssignmentBAL();

                                        DataSet dsSL = objSLAssignBAL.retrieve();

                                        int mapExists = ((from cr in dsSL.Tables[0].AsEnumerable()
                                                          where cr.Field<int>("LocationID") == Convert.ToInt32(drLocResult.Rows[0]["LocationID"])
                                                          select cr).Count());
                                        if (mapExists != 1)
                                        {
                                            if (siteLocAssign.ContainsKey(siteName))
                                            {
                                                siteLocAssign[siteName] = siteLocAssign[siteName] + objLocation.LocationID.ToString() + ";";
                                            }
                                            else
                                            {
                                                siteLocAssign.Add(siteName, objLocation.LocationID.ToString() + ";");
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!isLocationCreateAllowed(roomName, "room"))
                                    continue;

                                //Hardcoded values for IpAddress, IsExitDoor, IsCheckOutLocation
                                //ParentLocationID = 0 for Room
                                //LocationTypeId for LocationType - "Room" is 4
                                objLocation = new iAssetTrack.BAL.LocationBAL();
                                objLocation.Location = roomName;
                                objLocation.Description = roomName;
                                objLocation.IpAddress = string.Empty;
                                objLocation.ParentLocationID = 0;
                                objLocation.LocationTypeID = (int)locationTypeID;
                                objLocation.IsExitDoor = 0;
                                objLocation.IsCheckOutLocation = 1;
                                objLocation.Status = 1;
                                objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                objLocation.Persist(DALCOperation.Insert);
                                // SiteName ## LocationName, to make sure that site-location combination unique.
                                dictRoomsInserted.Add(objLocation.LocationID, siteName + "##" + objLocation.Location);

                                if (siteLocAssign.ContainsKey(siteName))
                                {
                                    siteLocAssign[siteName] = siteLocAssign[siteName] + objLocation.LocationID.ToString() + ";";
                                }
                                else
                                {
                                    siteLocAssign.Add(siteName, objLocation.LocationID.ToString() + ";");
                                }

                            }

                        }
                    }
                }
            }
        }
        catch (Exception ex) { ExceptionPolicy.HandleException(ex, "errDCTrack"); }


        //Loop through the Dictionary(with the LocationIDs and Locations) of the Locations/Rooms inserted
        if (dictRoomsInserted.Values.Count != 0)
        {
            foreach (KeyValuePair<int, string> pair in dictRoomsInserted)
            {
                int cnt = 0;
                if (dtLocation.Rows.Count > 0)
                {

                    if (dtLocation.Columns["LocationID"].DataType == typeof(System.Int32))
                    {
                        var locCount = (from cr in dtLocation.AsEnumerable()
                                        where cr.Field<int>("LocationID") == pair.Key
                                        select cr);
                        cnt = locCount.Count();
                    }
                    else
                    {
                        var locCount = (from cr in dtLocation.AsEnumerable()
                                        where cr.Field<string>("LocationID") == pair.Key.ToString()
                                        select cr);
                        cnt = locCount.Count();
                    }

                }
                else
                {
                    cnt = 0;
                }
                if (cnt == 0)
                {
                    DataRow dr = dtLocation.NewRow();
                    dr["LocationID"] = pair.Key;
                    // to get location name
                    dr["Site"] = pair.Value.Replace("##", "~").Split('~')[0];
                    dr["Location"] = pair.Value.Replace("##", "~").Split('~')[1];
                    dr["LocationType"] = "Room";
                    dr["ParentLocationID"] = "0";

                    dtLocation.Rows.Add(dr);
                }
                else
                {
                    foreach (DataRow dr in dtLocation.Rows)
                    {
                        if (Convert.ToInt32(dr["LocationID"].ToString()) == pair.Key)
                        {
                            dr["Site"] = pair.Value.Replace("##", "~").Split('~')[0];
                            dr["LocationType"] = "Room";
                            dtLocation.AcceptChanges();
                        }
                    }
                }

            }
            dtLocation.AcceptChanges();
        }
        if (!dsExcelData.Tables.Contains("dtLocation"))
        {
            dsExcelData.Tables.Add(dtLocation);
        }
        dsExcelData.AcceptChanges();
        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
        assignRooms2Site(siteLocAssign);
    }

    private bool isLocationCreateAllowed(string LocationName, string LocationType)
    {
        int tenantTypeSize;
        int currentTenantTypeSize = 0;
        bool isAllowed = true;
        string tenantType = string.Empty;

        if (bool.Parse(Session["TenantUser"].ToString()))
        {
            //tenant user
            // for tenant user, based on tenant type insert of Room ,Row and Rack are allowed.
            UserBAL objUser = new UserBAL();
            objUser.UserID = Convert.ToInt32(Session["UserID"]);
            DataSet dsTenant = objUser.retrieveTenantDetails();
            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
            {
                tenantTypeSize = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_TYPE_SIZE].ToString());
                tenantType = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_TYPE].ToString();

                //check current tenant type size
                TenantBAL objTenant = new TenantBAL();
                objTenant.TenantId = Convert.ToInt32(dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ID].ToString());
                //0 means no location specified or NULL
                DataSet dsTL = objTenant.retrieveLocationAssignmentList(0);
                if (dsTL.Tables.Count > 0 && dsTL.Tables[0].Rows.Count > 0)
                {
                    DataRow[] filteredRows = null;
                    if (tenantType.ToLower().CompareTo("room") == 0)
                    {
                        filteredRows = dsTL.Tables[0].Select("LocationTypeID = 1 AND Location <> '" + LocationName.Trim() + "'");
                    }
                    else if (tenantType.ToLower().CompareTo("row") == 0)
                    {
                        filteredRows = dsTL.Tables[0].Select("LocationTypeID = 2 AND Location <> '" + LocationName.Trim() + "'");
                    }
                    else if (tenantType.ToLower().CompareTo("rack") == 0)
                    {
                        filteredRows = dsTL.Tables[0].Select("LocationTypeID = 3 AND Location <> '" + LocationName.Trim() + "'");
                    }

                    if (filteredRows != null)
                        currentTenantTypeSize = filteredRows.Length;
                    else
                        currentTenantTypeSize = 0;
                }

                switch (tenantType.ToLower())
                {
                    case "room":
                        if(tenantType.ToLower().CompareTo(LocationType) == 0)
                        {
                            if (currentTenantTypeSize >= tenantTypeSize)
                                isAllowed = false;
                        }
                        
                        break;
                    case "row":
                        if (tenantType.ToLower().CompareTo(LocationType) == 0)
                        {
                            if (currentTenantTypeSize >= tenantTypeSize)
                                isAllowed = false;
                        }
                        else if (LocationType.CompareTo("room") == 0)
                        {
                            isAllowed = false;
                        }
                        break;
                    case "rack":
                        if (tenantType.ToLower().CompareTo(LocationType) == 0)
                        {
                            if (currentTenantTypeSize >= tenantTypeSize)
                                isAllowed = false;
                        }
                        else if (LocationType.CompareTo("room") == 0 || LocationType.CompareTo("row") == 0)
                        {
                            isAllowed = false;
                        }
                        break;
                }
            }
        }
        return isAllowed;
    }

    public void insertRows(string TableName)
    {
        //Insert Rows
        string rowName = string.Empty;
        string roomName = string.Empty;
        string siteName = string.Empty;
        //Get the new rooms from the Import Asset Excel
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtTable = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable drLocResult = new DataTable();
        DataTable dtRows = new DataTable();
        dtTable = dsExcelData.Tables[TableName];
        dtLocation = dsExcelData.Tables["dtLocation"];
        Dictionary<int, string> dictRowsInserted = new Dictionary<int, string>();



        try
        {
            //Get the distinct RowIDs with all the columns from the excel
            IEnumerable<DataRow> query = (from rows in dtTable.AsEnumerable()
                                          group rows by new
                                          {
                                              Site = rows.Field<String>("Site"),
                                              Row = rows.Field<String>("Row"),
                                              Room = rows.Field<String>("Room")
                                          }
                                              into grp
                                          select grp.First());

            if (query.Count() > 0)
                // Create a table from the query.
                dtRows = query.CopyToDataTable();

            // Location type value, get from DB
            LocationTypeBAL obLT = new LocationTypeBAL();
            DataSet dsLT = obLT.retrieve();
            var locationTypeID = (int)(from lt in dsLT.Tables[0].AsEnumerable()
                                       where lt.Field<string>("LocationType").Equals("Row",
                                       StringComparison.InvariantCultureIgnoreCase)
                                       select lt.Field<int>("LocationTypeID")).First();


            //TODO: Check if the location already exist
            foreach (DataRow dr in dtRows.Rows)
            {
                rowName = dr["Row"].ToString().Trim();
                roomName = dr["Room"].ToString().Trim();
                siteName = dr["Site"].ToString().Trim();
                if (!string.IsNullOrEmpty(roomName.Trim()))
                {

                    if (!string.IsNullOrEmpty(rowName.Trim()))
                    {
                        // if row name contains dispose or decom keywords than creation will be cancelled.
                        if (!rowName.Trim().ToLower().Contains("dispose") && !rowName.Trim().ToLower().Contains("decom"))
                        {
                            Regex regx = new Regex(@"^[\w0-9\-\.]+(\s?[\w0-9\-\.]+)*$");
                            if (regx.IsMatch(rowName) && rowName.Trim().Length <= 50)
                            {
                                int intParent;
                                //Get the Parent LocationID of the Row.
                                var ParentLocIDResult = from roomRow in dtLocation.AsEnumerable()
                                                        where roomRow.Field<string>("Location").Equals(roomName,
                                                        StringComparison.InvariantCultureIgnoreCase) &&
                                                        roomRow.Field<string>("LocationType").Equals("Room",
                                                        StringComparison.InvariantCultureIgnoreCase) &&
                                                        roomRow.Field<string>("Site").Equals(siteName,
                                                        StringComparison.InvariantCultureIgnoreCase)

                                                        select roomRow;
                                if (ParentLocIDResult.Count() > 0)
                                    dtResult = ParentLocIDResult.CopyToDataTable();
                                if (dtResult.Rows.Count != 0)
                                {
                                    intParent = Convert.ToInt32(dtResult.Rows[0]["LocationID"]);
                                }
                                else
                                {
                                    intParent = 0;
                                }

                                if (intParent > 0)
                                {
                                    IEnumerable<DataRow> drResult = from locRow in dtLocation.AsEnumerable()
                                                                    where locRow.Field<string>("Location").Equals(rowName, StringComparison.InvariantCultureIgnoreCase) &&
                                                                    locRow.Field<string>("LocationType").Equals("Row", StringComparison.InvariantCultureIgnoreCase) &&
                                                                    int.Parse(locRow.Field<string>("ParentLocationID")) == intParent
                                                                    select locRow;

                                    if (!isLocationCreateAllowed(rowName, "row"))
                                        continue;
                                    if (!(drResult.Count() > 0))
                                    {
                                        //Hardcoded values for IpAddress, IsExitDoor, IsCheckOutLocation
                                        objLocation = new iAssetTrack.BAL.LocationBAL();
                                        objLocation.Location = rowName;
                                        objLocation.Description = rowName;
                                        objLocation.IpAddress = string.Empty;
                                        objLocation.ParentLocationID = intParent;
                                        objLocation.LocationTypeID = locationTypeID;
                                        objLocation.IsExitDoor = 0;
                                        objLocation.IsCheckOutLocation = 1;
                                        objLocation.Status = 1;
                                        objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                        objLocation.ExternalID = "";
                                        objLocation.Persist(DALCOperation.Insert);
                                        dictRowsInserted.Add(objLocation.LocationID, siteName + "##" + intParent.ToString() + "##" +
                                                objLocation.Location);
                                    }
                                    else
                                    {
                                        drLocResult = drResult.CopyToDataTable();
                                        LocationBAL objL = new LocationBAL();
                                        if (objL.HasChildNodes(Convert.ToInt32(drLocResult.Rows[0]["LocationID"])) == 0)
                                        {
                                            // Row Already exist
                                            //Hardcoded values for IpAddress, IsExitDoor, IsCheckOutLocation
                                            objLocation = new iAssetTrack.BAL.LocationBAL();
                                            objLocation.Location = rowName;
                                            objLocation.LocationID = Convert.ToInt32(drLocResult.Rows[0]["LocationID"]);
                                            objLocation.Description = rowName;
                                            objLocation.IpAddress = string.Empty;
                                            objLocation.ParentLocationID = intParent;
                                            objLocation.LocationTypeID = locationTypeID;
                                            objLocation.IsExitDoor = 0;
                                            objLocation.IsCheckOutLocation = 1;
                                            objLocation.Status = 1;
                                            objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                            objLocation.ExternalID = "";
                                            objLocation.Persist(DALCOperation.Update);
                                        }
                                    }
                                }
                                else
                                {
                                    //parent details not found.
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }

        //Loop through the Dictionary(with the LocationIDs and Locations) of the Locations/Rooms inserted
        if (dictRowsInserted.Values.Count != 0)
        {
            foreach (KeyValuePair<int, string> pair in dictRowsInserted)
            {
                DataRow dr = dtLocation.NewRow();
                dr["LocationID"] = pair.Key;
                dr["Site"] = pair.Value.Replace("##", "~").Split('~')[0];
                dr["ParentLocationID"] = pair.Value.Replace("##", "~").Split('~')[1];
                dr["Location"] = pair.Value.Replace("##", "~").Split('~')[2];
                dr["LocationType"] = "Row";

                dtLocation.Rows.Add(dr);
            }
            dtLocation.AcceptChanges();
        }
        dsExcelData.AcceptChanges();
        //Serialize the dataset dsExcel data - with 4 tables(Excel data,BusinessUnit table,Site table and Location Table)
        //Add new Rows inserted - in table Location.
        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
    }

    public void assignRooms2Site(Dictionary<string, string> dictRooms)
    {
        //Get the Sites from the serialized Import Asset Excel
        DataTable dtTable = new DataTable();
        DataSet dsExcelData = new DataSet();
        DataTable dtBU = new DataTable();
        DataTable dtSite = new DataTable();
        DataTable dtResultSite = new DataTable();
        DataTable dtResultBU = new DataTable();
        dsExcelData = DeserializeDataSource();
        dtSite = dsExcelData.Tables["dtSite"];
        dtBU = dsExcelData.Tables["dtBusinessUnit"];

        string strBU = txtBU.Text;


        try
        {
            dtTable = dsExcelData.Tables[0];



            //Loop through the Dictionary(with the LocationIDs and Locations) of the Locations/Rooms inserted
            foreach (KeyValuePair<string, string> pair in dictRooms)
            {
                objSiteLocation = new iAssetTrack.BAL.SiteLocationAssignmentBAL();
                string StrLocationIDs = "";
                //int roomLocID = pair.Key;
                string siteName = pair.Key;
                //StrLocationIDs = pair.Value;
                ////Get the distinct RowIDs with all the columns from the excel
                //var site = (from cr in dtTable.AsEnumerable()
                //            where cr.Field<string>("Room").Equals(pair.Value, StringComparison.InvariantCultureIgnoreCase)
                //            select cr.Field<string>("Site")).First();

                //string siteName = (string)site;


                IEnumerable<DataRow> result = from siteRow in dtSite.AsEnumerable()
                                              where siteRow.Field<string>("Site").Equals(siteName, StringComparison.InvariantCultureIgnoreCase)
                                              select siteRow;
                if (result.Count() > 0)
                    dtResultSite = result.CopyToDataTable();

                //Retrive the BusinessUnitID
                IEnumerable<DataRow> resultBU = from BURow in dtBU.AsEnumerable()
                                                where BURow.Field<string>("BusinessUnit").Equals(strBU, StringComparison.InvariantCultureIgnoreCase)
                                                select BURow;
                if (resultBU.Count() > 0)
                    dtResultBU = resultBU.CopyToDataTable();

                if (dtResultSite.Rows.Count != 0 && dtResultBU.Rows.Count != 0)
                {
                    objSiteLocation.BusinessUnitID = Convert.ToInt32(dtResultBU.Rows[0]["BusinessUnitID"]);
                    objSiteLocation.SiteID = Convert.ToInt32(dtResultSite.Rows[0]["SiteID"]);
                    // get already assigned Locations
                    DataSet dsAssignedLocs = objSiteLocation.retrieveAssignLocation();
                    if (dsAssignedLocs != null && dsAssignedLocs.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsAssignedLocs.Tables[0].Rows)
                        {
                            StrLocationIDs += dr["LocationID"].ToString() + ";";
                        }
                    }
                    // add new ones
                    StrLocationIDs += pair.Value;

                    objSiteLocation.BusinessUnitID = Convert.ToInt32(dtResultBU.Rows[0]["BusinessUnitID"]);
                    objSiteLocation.SiteID = Convert.ToInt32(dtResultSite.Rows[0]["SiteID"]);
                    objSiteLocation.LocationIDs = StrLocationIDs;
                    objSiteLocation.Delimiters = ";";
                    objSiteLocation.Status = 1;
                    objSiteLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    objSiteLocation.Persist(DALCOperation.Insert);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void insertRacks(string TableName)
    {
        //Insert Racks
        string rackTagValue = string.Empty;  //*V3.8-Added on 21Oct2013-By Amar Vidya 
        string rackName = string.Empty;
        string rowName = string.Empty;
        string roomName = string.Empty;
        string siteName = string.Empty;
        string parentLoc = string.Empty;
        string manufacturer = string.Empty;
        string model = string.Empty;
        int parentLocID = 0;
        string serialnumber = string.Empty;
        string externalID = string.Empty;
        int modelID = 0;
        int mfgID = 0;
        string rackStatus = string.Empty;
        string rackReason = string.Empty;
        //Get the new rooms from the Import Asset Excel
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtTable = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable dtResult = new DataTable();
        DataTable drLocResult = new DataTable();
        DataTable dtRacks = new DataTable();
        DataTable dtAssetModel = new DataTable();
        DataTable dtMfg = new DataTable();
        dtAssetModel = dsExcelData.Tables["dtModel"];
        dtMfg = dsExcelData.Tables["dtManufacturer"];
        DataTable dtAMResult = new DataTable();
        DataTable dtMResult = new DataTable();


        dtTable = dsExcelData.Tables[TableName];
        dtLocation = dsExcelData.Tables["dtLocation"];
        Dictionary<int, string> dictRacksInserted = new Dictionary<int, string>();

        if (dtLocation != null)
        {
            if (!dtLocation.Columns.Contains("Manufacturer"))
                dtLocation.Columns.Add("Manufacturer", typeof(string));

            if (!dtLocation.Columns.Contains("Model"))
                dtLocation.Columns.Add("Model", typeof(string));

            if (!dtLocation.Columns.Contains("TagID"))
                dtLocation.Columns.Add("TagID", typeof(string));

            if (!dtLocation.Columns.Contains("SerialNumber"))
                dtLocation.Columns.Add("SerialNumber", typeof(string));

            if (!dtLocation.Columns.Contains("ModelID"))
                dtLocation.Columns.Add("ModelID", typeof(int));

            if (!dtLocation.Columns.Contains("ExternalID"))
                dtLocation.Columns.Add("ExternalID", typeof(string));
            dtLocation.AcceptChanges();
        }

        IEnumerable<DataRow> query = null;
        if (TableName.ToLower().CompareTo("assets") == 0)
        {
            query = (from racks in dtTable.AsEnumerable()
                     group racks by new
                     {
                         Site = racks.Field<String>("Site"),
                         Room = racks.Field<String>("Room"),
                         Row = racks.Field<String>("Row"),
                         Rack = racks.Field<String>("Rack"),
                         RackTag = "",
                         Manufacturer = "",
                         Model = "",
                         SerialNumber = ""
                     }
                         into grp
                     select grp.First());

        }
        else
        {
            query = (from racks in dtTable.AsEnumerable()
                     group racks by new
                     {
                         Site = racks.Field<String>("Site"),
                         Room = racks.Field<String>("Room"),
                         Row = racks.Field<String>("Row"),
                         Rack = racks.Field<String>("Rack"),
                         RackTag = racks.Field<String>("RackTag"),
                         Manufacturer = racks.Field<String>("Manufacturer"),
                         Model = racks.Field<String>("Model"),
                         SerialNumber = racks.Field<string>("SerialNumber"),
                         ExternalID = racks.Field<string>("ExternalID"),
                         Status = Convert.ToString(racks.Field<string>("Status")),
                         Reason = Convert.ToString(racks.Field<string>("Reason")),
                         ID = Convert.ToString(racks.Field<string>("ID"))
                     }
                         into grp
                     select grp.First());
        }

        if (query.Count() > 0)
            // Create a table from the query.
            dtRacks = query.CopyToDataTable();


        // Location type value, get from DB
        LocationTypeBAL obLT = new LocationTypeBAL();
        DataSet dsLT = obLT.retrieve();
        var locationTypeID = (int)(from lt in dsLT.Tables[0].AsEnumerable()
                                   where lt.Field<string>("LocationType").Equals("Rack",
                                   StringComparison.InvariantCultureIgnoreCase)
                                   select lt.Field<int>("LocationTypeID")).First();

        try
        {
            foreach (DataRow dr in dtRacks.Rows)
            {
                rackStatus = string.Empty;
                rackReason = string.Empty;

                rackName = dr["Rack"].ToString().Trim();
                rowName = dr["Row"].ToString().Trim();
                roomName = dr["Room"].ToString().Trim();
                siteName = dr["Site"].ToString().Trim();

                // if row name contains dispose or decom keywords than creation will be cancelled.
                if (!rackName.Trim().ToLower().Contains("dispose") && !rackName.Trim().ToLower().Contains("decom"))
                {
                    Regex regx = new Regex(@"^[\w0-9\-\.]+(\s?[\w0-9\-\.]+)*$");
                    if (regx.IsMatch(rackName) && rackName.Trim().Length <= 50)
                    {

                        if (TableName.ToLower().CompareTo("racks") == 0)
                        {
                            if (!string.IsNullOrEmpty(dr["SerialNumber"].ToString()))
                                serialnumber = dr["SerialNumber"].ToString();
                            else
                                serialnumber = "";

                            if (!string.IsNullOrEmpty(dr["Manufacturer"].ToString()))
                                manufacturer = dr["Manufacturer"].ToString();
                            else
                                manufacturer = "";
                            if (!string.IsNullOrEmpty(dr["Model"].ToString()))
                                model = dr["Model"].ToString();
                            else
                                model = "";

                            rackTagValue = dr["RackTag"].ToString();

                        }
                        else
                        {
                            manufacturer = "";
                            model = "";
                            serialnumber = "";
                            rackTagValue = "";
                        }

                        if (!string.IsNullOrEmpty(dr["ExternalID"].ToString()))
                            externalID = dr["ExternalID"].ToString();
                        else
                            externalID = "";

                        //Serial No check
                        regx = new Regex(@"^[\w0-9\-\.]+([\w0-9\-\.]+)*$");
                        if (string.IsNullOrEmpty(serialnumber.Trim()) || (regx.IsMatch(serialnumber.Trim()) && serialnumber.Trim().Length <= 50))
                        {
                            regx = new Regex(@"^[A-Za-z0-9]+$");
                            if (string.IsNullOrEmpty(rackTagValue.Trim()) || (regx.IsMatch(rackTagValue.Trim()) && rackTagValue.Trim().Length <= 24))
                            {

                                if (!string.IsNullOrEmpty(rackName.Trim()) && !string.IsNullOrEmpty(roomName.Trim()) &&
                                        !string.IsNullOrEmpty(siteName.Trim()))
                                {
                                    //Parent Row (if no parent row, than rack will be mapped to the room)
                                    if (!string.IsNullOrEmpty(rowName.Trim()))
                                    {
                                        var parentRoom = (from cr in dtLocation.AsEnumerable()
                                                          where cr.Field<string>("Location").Equals(roomName.Trim(),
                                                          StringComparison.InvariantCultureIgnoreCase) &&
                                                          cr.Field<string>("LocationType").Equals("Room",
                                                         StringComparison.InvariantCultureIgnoreCase) &&
                                                         cr.Field<string>("Site").Equals(siteName.Trim(),
                                                         StringComparison.InvariantCultureIgnoreCase)
                                                          select cr.Field<string>("LocationID"));
                                        var parentRoomID = "0";
                                        if (parentRoom.Count() > 0)
                                            parentRoomID = parentRoom.First();

                                        var parentLocationRow = from rackRow in dtLocation.AsEnumerable()
                                                                where rackRow.Field<string>("Location").Equals(rowName.Trim(),
                                                                StringComparison.InvariantCultureIgnoreCase) &&
                                                                rackRow.Field<string>("LocationType").Equals("Row",
                                                                StringComparison.InvariantCultureIgnoreCase) &&
                                                                rackRow.Field<string>("Site").Equals(siteName.Trim(),
                                                                StringComparison.InvariantCultureIgnoreCase) &&
                                                                rackRow.Field<string>("ParentLocationID").Equals(parentRoomID.ToString())

                                                                select rackRow;
                                        dtResult.Rows.Clear();
                                        if (parentLocationRow.Count() > 0)
                                            dtResult = parentLocationRow.CopyToDataTable();
                                        if (dtResult.Rows.Count != 0)
                                        {
                                            parentLocID = Convert.ToInt32(dtResult.Rows[0]["LocationID"]);
                                        }
                                        else
                                            parentLocID = 0;
                                    }
                                    else if (!string.IsNullOrEmpty(roomName.Trim()))
                                    {
                                        var parentLocationRoom = (from rackRoom in dtLocation.AsEnumerable()
                                                                  where rackRoom.Field<string>("Location").Equals(roomName.Trim(),
                                                                 StringComparison.InvariantCultureIgnoreCase) &&
                                                                 rackRoom.Field<string>("LocationType").Equals("Room",
                                                                 StringComparison.InvariantCultureIgnoreCase) &&
                                                                 rackRoom.Field<string>("Site").Equals(siteName.Trim(),
                                                                 StringComparison.InvariantCultureIgnoreCase)
                                                                  select rackRoom);

                                        dtResult.Rows.Clear();
                                        if (parentLocationRoom.Count() > 0)
                                            dtResult = parentLocationRoom.CopyToDataTable();
                                        if (dtResult.Rows.Count != 0)
                                        {
                                            parentLocID = Convert.ToInt32(dtResult.Rows[0]["LocationID"]);
                                        }
                                    }

                                    if (parentLocID > 0)
                                    {
                                        IEnumerable<DataRow> drResult = from locRow in dtLocation.AsEnumerable()
                                                                        where locRow.Field<string>("Location").Equals(rackName.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                                        locRow.Field<string>("LocationType").Equals("Rack", StringComparison.InvariantCultureIgnoreCase) &&
                                                                    locRow.Field<string>("Site").Equals(siteName,
                                                                    StringComparison.InvariantCultureIgnoreCase) &&
                                                                    locRow.Field<string>("ParentLocationID").CompareTo(parentLocID.ToString()) == 0
                                                                        select locRow;
                                        if (dtMfg != null)
                                        {
                                            // Get the MfgID
                                            var assetMfgResult = from mRow in dtMfg.AsEnumerable()
                                                                 where mRow.Field<string>("MfgName").Trim().Equals(manufacturer.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                                 select mRow;

                                            if (assetMfgResult.Count() > 0)
                                            {
                                                dtMResult = assetMfgResult.CopyToDataTable();
                                                mfgID = Convert.ToInt32(dtMResult.Rows[0]["MfgID"]);
                                            }
                                            else
                                            {
                                                mfgID = 0;
                                            }
                                        }

                                        if (dtAssetModel != null)
                                        {
                                            //2. Get the ModelID
                                            var assetModelResult = from amRow in dtAssetModel.AsEnumerable()
                                                                   where amRow.Field<string>("ModelName").Trim().Equals(model.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                                   && amRow.Field<string>("MfgID").Equals(mfgID.ToString(), StringComparison.InvariantCultureIgnoreCase)

                                                                   select amRow;
                                            if (assetModelResult.Count() > 0)
                                            {
                                                dtAMResult = assetModelResult.CopyToDataTable();
                                                modelID = Convert.ToInt32(dtAMResult.Rows[0]["ModelID"]);

                                            }
                                            else
                                            {
                                                modelID = 0;

                                            }
                                        }
                                        //if modeltype is not of type "rack" than rack won't be created
                                        if (modelID > 0)
                                        {
                                            AssetModelBAL modelBAL = new AssetModelBAL();
                                            modelBAL.ModelID = modelID;
                                            DataSet dsModel = modelBAL.retrieve();
                                            if (dsModel != null && dsModel.Tables.Count > 0 && dsModel.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_TYPE].ToString().ToLower().CompareTo("rack") != 0)
                                                {
                                                    modelID = 0;
                                                    rackStatus = "Failed";
                                                    rackReason = "Rack Model must be of type Rack";
                                                }
                                            }
                                        }

                                        if (isLocationCreateAllowed(rackName, "rack"))
                                        {
                                            // rack will be inserted, only if row or room exists as parent
                                            if (!string.IsNullOrEmpty(rowName.Trim()) || !string.IsNullOrEmpty(roomName.Trim()))
                                            {
                                                if (drResult.Count() == 0)
                                                {
                                                    // if modelid = 0 than rack creation will be cancelled.
                                                    if (modelID > 0)
                                                    {
                                                        //Hardcoded values for IpAddress, IsExitDoor, IsCheckOutLocation
                                                        objLocation = new iAssetTrack.BAL.LocationBAL();
                                                        objLocation.Location = rackName;
                                                        objLocation.Description = rackName;
                                                        objLocation.IpAddress = string.Empty;
                                                        objLocation.ParentLocationID = parentLocID;
                                                        objLocation.LocationTypeID = (int)locationTypeID;
                                                        objLocation.IsExitDoor = 0;
                                                        objLocation.IsCheckOutLocation = 1;
                                                        objLocation.Status = 1;
                                                        objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                                        objLocation.ExternalID = externalID;
                                                        if (TableName.ToLower().CompareTo("assets") != 0)
                                                        {
                                                            objLocation.Manufacturer = manufacturer;
                                                            objLocation.Model = model;
                                                            objLocation.TagID = rackTagValue;
                                                            objLocation.SerialNumber = serialnumber;
                                                            objLocation.ModelID = modelID;
                                                            if (modelID > 0)
                                                            {
                                                                AssetModelBAL modelBAL = new AssetModelBAL();
                                                                modelBAL.ModelID = modelID;
                                                                DataSet dsModel = modelBAL.retrieve();
                                                                if (dsModel != null && dsModel.Tables.Count > 0 && dsModel.Tables[0].Rows.Count > 0)
                                                                    objLocation.Height = Convert.ToInt32(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_UHEIGHT].ToString());
                                                                else
                                                                    objLocation.Height = 0;
                                                            }
                                                            else
                                                                objLocation.Height = 0;
                                                        }
                                                        else
                                                        {
                                                            objLocation.Manufacturer = "";
                                                            objLocation.Model = "";
                                                            objLocation.TagID = "";
                                                            objLocation.SerialNumber = "";
                                                            objLocation.ModelID = 0;
                                                            objLocation.Height = 0;
                                                        }



                                                        objLocation.Persist(DALCOperation.Insert);
                                                        //dictRacksInserted.Add(objLocation.LocationID, objLocation.Location);
                                                        dictRacksInserted.Add(objLocation.LocationID, siteName + "##" + parentLocID.ToString() + "##" +
                                                               objLocation.Location + "##" + objLocation.Manufacturer + " ##" + objLocation.Model + "##" + objLocation.TagID + "##");

                                                        rackStatus = "Success";
                                                        rackReason = "Rack created";
                                                    }
                                                    else
                                                    {
                                                        //model details
                                                        if (rackReason.ToString().CompareTo("Rack Model must be of type Rack") != 0)
                                                        {
                                                            rackStatus = "Failed";
                                                            rackReason = "Rack Model not defined in the system";
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    //check if any assets are already placed in this rack
                                                    //if assets exists than cancel rack update.
                                                    LocationBAL objL = new LocationBAL();
                                                    drLocResult = drResult.CopyToDataTable();
                                                    if (objL.HasChildNodes(Convert.ToInt32(drLocResult.Rows[0]["LocationID"])) == 0)
                                                    {
                                                        if (modelID > 0)
                                                        {
                                                            //Rack already exists

                                                            if (string.IsNullOrEmpty(drLocResult.Rows[0]["Manufacturer"].ToString()))
                                                                manufacturer = "";
                                                            else
                                                                manufacturer = drLocResult.Rows[0]["Manufacturer"].ToString();

                                                            if (string.IsNullOrEmpty(drLocResult.Rows[0]["Model"].ToString()))
                                                                model = "";
                                                            else
                                                                model = drLocResult.Rows[0]["Model"].ToString();

                                                            if (dtMfg != null)
                                                            {
                                                                // Get the MfgID
                                                                var assetMfgResult = from mRow in dtMfg.AsEnumerable()
                                                                                     where mRow.Field<string>("MfgName").Trim().Equals(manufacturer.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                                                     select mRow;

                                                                if (assetMfgResult.Count() > 0)
                                                                {
                                                                    dtMResult = assetMfgResult.CopyToDataTable();
                                                                    mfgID = Convert.ToInt32(dtMResult.Rows[0]["MfgID"]);
                                                                }
                                                                else
                                                                {
                                                                    mfgID = 0;
                                                                }
                                                            }

                                                            if (dtAssetModel != null)
                                                            {
                                                                //2. Get the ModelID
                                                                var assetModelResult = from amRow in dtAssetModel.AsEnumerable()
                                                                                       where amRow.Field<string>("ModelName").Trim().Equals(model.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                                                       && amRow.Field<string>("MfgID").Equals(mfgID.ToString(), StringComparison.InvariantCultureIgnoreCase)

                                                                                       select amRow;
                                                                if (assetModelResult.Count() > 0)
                                                                {
                                                                    dtAMResult = assetModelResult.CopyToDataTable();
                                                                    modelID = Convert.ToInt32(dtAMResult.Rows[0]["ModelID"]);

                                                                }
                                                                else
                                                                {
                                                                    modelID = 0;

                                                                }
                                                            }


                                                            if (string.IsNullOrEmpty(drLocResult.Rows[0]["SerialNumber"].ToString()))
                                                                serialnumber = "";
                                                            else
                                                                serialnumber = drLocResult.Rows[0]["SerialNumber"].ToString();

                                                            if (string.IsNullOrEmpty(drLocResult.Rows[0]["TagID"].ToString()))
                                                                rackTagValue = "";
                                                            else
                                                                rackTagValue = drLocResult.Rows[0]["TagID"].ToString();
                                                            //Rack Already exist
                                                            //Hardcoded values for IpAddress, IsExitDoor, IsCheckOutLocation

                                                            objLocation = new iAssetTrack.BAL.LocationBAL();
                                                            objLocation.Location = rackName;
                                                            objLocation.LocationID = Convert.ToInt32(drLocResult.Rows[0]["LocationID"]);
                                                            objLocation.Description = rackName;
                                                            objLocation.IpAddress = string.Empty;
                                                            objLocation.ParentLocationID = parentLocID;
                                                            objLocation.LocationTypeID = (int)locationTypeID;
                                                            objLocation.IsExitDoor = 0;
                                                            objLocation.IsCheckOutLocation = 1;
                                                            objLocation.Status = 1;
                                                            objLocation.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                                            objLocation.Manufacturer = manufacturer;
                                                            objLocation.Model = model;
                                                            objLocation.SerialNumber = serialnumber;
                                                            objLocation.ModelID = modelID;
                                                            objLocation.TagID = rackTagValue;
                                                            objLocation.ExternalID = externalID;
                                                            //Height
                                                            if (modelID > 0)
                                                            {
                                                                AssetModelBAL modelBAL = new AssetModelBAL();
                                                                modelBAL.ModelID = modelID;
                                                                DataSet dsModel = modelBAL.retrieve();
                                                                if (dsModel != null && dsModel.Tables.Count > 0 && dsModel.Tables[0].Rows.Count > 0)
                                                                    objLocation.Height = Convert.ToInt32(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_UHEIGHT].ToString());
                                                                else
                                                                    objLocation.Height = 0;
                                                            }
                                                            else
                                                                objLocation.Height = 0;

                                                            objLocation.Persist(DALCOperation.Update);

                                                            rackStatus = "Success";
                                                            rackReason = "Rack details updated";
                                                        }
                                                        else
                                                        {
                                                            //rackStatus = "Failed";
                                                            //rackReason = "Model not defined in the system";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        rackStatus = "Not Updated";
                                                        rackReason = "Asset(s) exists.";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            rackStatus = "Failed";
                                            rackReason = "Tenant type size is exceeded.";
                                        }
                                    }
                                    else
                                    {
                                        rackStatus = "Failed";
                                        rackReason = "Parent location details not found.";
                                    }
                                }

                            }
                            else
                            {
                                //rack tag value
                                rackStatus = "Failed";
                                rackReason = "Rack Tag is not in correct format.";
                            }
                        }
                        else
                        {
                            //serial no
                            rackStatus = "Failed";
                            rackReason = "Serial Number is not in correct format";
                        }
                    }
                    else
                    {
                        //rack name format
                        rackStatus = "Failed";
                        rackReason = "Rack Name is not in correct format";
                    }
                }
                else
                {
                    //no dispose or decom
                    rackStatus = "Failed";
                    rackReason = "Rack Name with Dispose or Decom keywords is not allowed";
                }

                DataRow[] drRack = dtTable.Select(" Site = '" + siteName.Trim() + "' AND Room = '" + roomName.Trim() + "' AND Row = '" + rowName.Trim() + "' AND Rack='" +
                        rackName.Trim() + "'");

                drRack[0]["Status"] = rackStatus;
                drRack[0]["Reason"] = rackReason;


                //Serialize the dataset dsExcel data - with 4 tables(Excel data,BusinessUnit table,Site table and Location Table)
                //Add new Racks inserted - in table Location.
                if (dictRacksInserted != null && dictRacksInserted.Values.Count != 0)
                {
                    foreach (KeyValuePair<int, string> pair in dictRacksInserted)
                    {
                        DataRow drow = dtLocation.NewRow();
                        drow["LocationID"] = pair.Key;
                        drow["Site"] = pair.Value.Replace("##", "~").Split('~')[0];
                        drow["ParentLocationID"] = pair.Value.Replace("##", "~").Split('~')[1];
                        drow["Location"] = pair.Value.Replace("##", "~").Split('~')[2];
                        drow["Manufacturer"] = pair.Value.Replace("##", "~").Split('~')[3];
                        drow["Model"] = pair.Value.Replace("##", "~").Split('~')[4];
                        drow["TagID"] = pair.Value.Replace("##", "~").Split('~')[5];
                        drow["LocationType"] = "Rack";

                        dtLocation.Rows.Add(drow);
                    }
                    dtLocation.AcceptChanges();
                }
                dsExcelData.AcceptChanges();
                dictRacksInserted.Clear();
            }
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            //lblStatus.Text = ex.Message;
        }

        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
    }

    public void insertManufacturer(string TableName)
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

            if (!dsExcelData.Tables.Contains("dtManufacturer"))
            {
                dsExcelData.Tables.Add(dtAllMfgNames);
            }
            dsExcelData.AcceptChanges();
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void insertModel(string TableName)
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

            if (!dsExcelData.Tables.Contains("dtModel"))
                dsExcelData.Tables.Add(dtAllModelNames);
            dsExcelData.AcceptChanges();
            //Serialize the dataset dsExcel data - with 6 tables
            //(Excel data, BudsinessUnit table, Site table, Location table, Manufacturer table and Model table)
            ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
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
            ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void insertOwner()
    {
        string ownerName = string.Empty;
        string ownerFName = string.Empty;
        string ownerLName = string.Empty;
        Dictionary<int, string> dictOwnersInserted = new Dictionary<int, string>();

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
            //Get the new Owners from the Import Asset Excel
            DataSet dsExcelData = new DataSet();
            dsExcelData = DeserializeDataSource();
            DataTable dtTable = new DataTable();
            DataTable dtResult = new DataTable();
            DataTable dtOwnersInExcel = new DataTable();
            DataTable dtOwnersResult = new DataTable();
            dtTable = dsExcelData.Tables["Assets"];



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
            dsExcelData.AcceptChanges();
            //Serialize the dataset dsExcel data - with 5 tables
            //(Excel data, BudsinessUnit table,Site table, Location tabl and Manufacturer table)
            ImportAsset.SerializeExcelDataSetToXml(dsExcelData);
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
        }
    }

    public void insertAssets()
    {

        #region Retreive All Asset

        objAsset = new iAssetTrack.BAL.AssetBAL();
        #endregion

        #region Get all tables from dataset

        string assetName = string.Empty;
        string mfgName = string.Empty;
        string modelName = string.Empty;
        string assetType = string.Empty;
        string rackName = string.Empty;
        string rowName = string.Empty;
        string roomName = string.Empty;
        string locName = string.Empty;
        string strApplications = string.Empty;
        string ownerName = string.Empty;
        string ownerFName = string.Empty;
        string ownerLName = string.Empty;
        string buName = txtBU.Text;
        string siteName = string.Empty;
        string serialNo = string.Empty;
        string mountType = string.Empty;
        int startPos;
        int noOfRUs = 0;
        int assetTypeID = 0;
        int assetModelID;
        int ownerID;
        int locationId = 0;
        int buID;
        int siteID;
        string externalID;
        string assetTag = string.Empty;
        string hostName = string.Empty;
        string Orientation = string.Empty;
        string locType = string.Empty;

        //Get all tables from the Import Asset Excel
        DataSet dsExcelData = new DataSet();
        dsExcelData = DeserializeDataSource();
        DataTable dtBusinessUnit = new DataTable();
        DataTable dtSite = new DataTable();
        DataTable dtLocation = new DataTable();
        DataTable dtAssetType = new DataTable();
        DataTable dtAssetModel = new DataTable();
        DataTable dtAsset = new DataTable();
        DataTable dtMfg = new DataTable();
        DataTable dtOwner = new DataTable();
        //Result tables
        DataTable dtBUResult = new DataTable();
        DataTable dtSiteResult = new DataTable();
        DataTable dtLocResult = new DataTable();
        DataTable dtATResult = new DataTable();
        DataTable dtAMResult = new DataTable();
        DataTable dtMResult = new DataTable();
        DataTable dtOwnerResult = new DataTable();

        dtAsset = dsExcelData.Tables["Assets"];
        dtBusinessUnit = dsExcelData.Tables["dtBusinessUnit"];
        dtSite = dsExcelData.Tables["dtSite"];
        dtLocation = dsExcelData.Tables["dtLocation"];
        dtAssetModel = dsExcelData.Tables["dtModel"];
        dtAssetType = dsExcelData.Tables["dtAssetType"];
        dtMfg = dsExcelData.Tables["dtManufacturer"];
        dtOwner = dsExcelData.Tables["dtOwner"];

        #endregion

        foreach (DataRow dr in dtAsset.Rows)
        {
            // site, room, manufacturer and Model are mandatory fields, if any of these are not
            //exist than insert will fail.
            if (!string.IsNullOrEmpty(dr["Site"].ToString().Trim()) &&
                !string.IsNullOrEmpty(dr["Room"].ToString().Trim())
                )
            {
                if (!string.IsNullOrEmpty(dr["Manufacturer"].ToString().Trim()) &&
                !string.IsNullOrEmpty(dr["Model"].ToString().Trim())
                )
                {
                    try
                    {
                        #region Assign Variables
                        mfgName = (dr["Manufacturer"].ToString()).Trim();
                        modelName = (dr["Model"].ToString()).Trim();
                        roomName = (dr["Room"].ToString()).Trim();
                        rackName = (dr["Rack"].ToString()).Trim();
                        rowName = (dr["Row"].ToString()).Trim();
                        siteName = (dr["Site"].ToString()).Trim();
                        hostName = dr["HostName"].ToString().Trim();
                        assetName = dr["AssetName"].ToString().Trim();

                        if (!string.IsNullOrEmpty(dr["StartPosition"].ToString().Trim()))
                        {
                            startPos = int.Parse(dr["StartPosition"].ToString().Trim());
                        }
                        else
                        {
                            startPos = 0;
                        }

                        assetTag = dr["AssetTag"].ToString().Trim();

                        //v3.8: if nothing specified for Orientation than Front will be taken automatically.
                        if (string.IsNullOrEmpty(dr["Orientation"].ToString().Trim()))
                        {
                            Orientation = "Front";
                        }
                        else
                        {
                            Orientation = dr["Orientation"].ToString().Trim();
                        }
                        serialNo = dr["SerialNumber"].ToString();
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
                                                             where mRow.Field<string>("MfgName").Equals(mfgName.Trim(), StringComparison.InvariantCultureIgnoreCase)
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
                                                               where amRow.Field<string>("ModelName").Equals(modelName.Trim(), StringComparison.InvariantCultureIgnoreCase)
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
                                                noOfRUs = Convert.ToInt32(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_UHEIGHT].ToString());
                                                assetType = dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_TYPE].ToString();
                                                mountType = dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_MOUNT_TYPE].ToString();

                                                var assetTypeResult = from atRow in dtAssetType.AsEnumerable()
                                                                      where atRow.Field<string>("AssetGroup").Equals(assetType, StringComparison.InvariantCultureIgnoreCase)

                                                                      select atRow;
                                                dtATResult = assetTypeResult.CopyToDataTable();
                                                assetTypeID = Convert.ToInt32(dtATResult.Rows[0]["AssetGroupID"]);
                                            }
                                        }

                                        //4. Get the LocationID
                                        if (!string.IsNullOrEmpty(dr["Rack"].ToString().Trim()))
                                        {
                                            locName = dr["Rack"].ToString().Trim();
                                            locType = "Rack";
                                            string parentID = string.Empty;
                                            if (string.IsNullOrEmpty(rowName.Trim()))
                                            {
                                                try
                                                {
                                                    var location = (from cr in dtLocation.AsEnumerable()
                                                                    where cr.Field<string>("Location").Equals(roomName.Trim(),
                                                                    StringComparison.InvariantCultureIgnoreCase) &&
                                                                    cr.Field<string>("LocationType").Equals("Room",
                                                                   StringComparison.InvariantCultureIgnoreCase) &&
                                                                   cr.Field<string>("Site").Equals(siteName.Trim(),
                                                                   StringComparison.InvariantCultureIgnoreCase)

                                                                    select cr.Field<string>("LocationID"));
                                                    var id = "0";
                                                    if (location.Count() > 0)
                                                        id = location.First();

                                                    if (id != null && !string.IsNullOrEmpty(id))
                                                        parentID = id.ToString();
                                                    else
                                                        parentID = "0";
                                                }
                                                catch
                                                {
                                                    parentID = "0";
                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    var rowParent = (from cr in dtLocation.AsEnumerable()
                                                                     where cr.Field<string>("Location").Equals(roomName.Trim(),
                                                                     StringComparison.InvariantCultureIgnoreCase) &&
                                                                     cr.Field<string>("LocationType").Equals("Room",
                                                                    StringComparison.InvariantCultureIgnoreCase) &&
                                                                    cr.Field<string>("Site").Equals(siteName.Trim(),
                                                                    StringComparison.InvariantCultureIgnoreCase)
                                                                     select cr.Field<string>("LocationID"));

                                                    var rowParentid = "0";
                                                    if (rowParent.Count() > 0)
                                                        rowParentid = rowParent.First();

                                                    string rowParentLocID = string.Empty;
                                                    if (rowParentid != null && !string.IsNullOrEmpty(rowParentid))
                                                        rowParentLocID = rowParentid.ToString();
                                                    else
                                                        rowParentLocID = "0";

                                                    var loc = (from cr in dtLocation.AsEnumerable()
                                                               where cr.Field<string>("Location").Equals(rowName.Trim(),
                                                               StringComparison.InvariantCultureIgnoreCase) &&
                                                               cr.Field<string>("LocationType").Equals("Row",
                                                              StringComparison.InvariantCultureIgnoreCase) &&
                                                              cr.Field<string>("Site").Equals(siteName.Trim(),
                                                              StringComparison.InvariantCultureIgnoreCase) &&
                                                               cr.Field<string>("ParentLocationiD").Equals(rowParentLocID, StringComparison.InvariantCultureIgnoreCase)
                                                               select cr.Field<string>("LocationID"));
                                                    var id = "0";
                                                    if (loc.Count() > 0)
                                                        id = loc.First();

                                                    if (id != null && !string.IsNullOrEmpty(id))
                                                        parentID = id.ToString();
                                                    else
                                                        parentID = "0";
                                                }
                                                catch
                                                {
                                                    parentID = "0";
                                                }
                                            }

                                            var locIDResult = from locRow in dtLocation.AsEnumerable()
                                                              where locRow.Field<string>("Location").Equals(locName.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("LocationType").Equals("Rack", StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("Site").Equals(siteName.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("ParentLocationiD").Equals(parentID, StringComparison.InvariantCultureIgnoreCase)

                                                              select locRow;
                                            if (locIDResult.Count() > 0)
                                            {
                                                dtLocResult = locIDResult.CopyToDataTable();
                                                locationId = Convert.ToInt32(dtLocResult.Rows[0]["LocationID"]);
                                            }
                                            else
                                            {
                                                locationId = 0;
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(dr["Row"].ToString().Trim()))
                                        {
                                            locName = dr["Row"].ToString();
                                            locType = "Row";
                                            string parentID = string.Empty;
                                            try
                                            {
                                                var loc = (from cr in dtLocation.AsEnumerable()
                                                           where cr.Field<string>("Location").Equals(roomName.Trim(),
                                                           StringComparison.InvariantCultureIgnoreCase) &&
                                                           cr.Field<string>("LocationType").Equals("Room",
                                                          StringComparison.InvariantCultureIgnoreCase) &&
                                                          cr.Field<string>("Site").Equals(siteName.Trim(),
                                                          StringComparison.InvariantCultureIgnoreCase)
                                                           select cr.Field<string>("LocationID"));
                                                var id = "0";
                                                if (loc.Count() > 0)
                                                    id = loc.First();

                                                if (id != null && !string.IsNullOrEmpty(id))
                                                    parentID = id.ToString();
                                                else
                                                    parentID = "0";
                                            }
                                            catch
                                            {
                                                parentID = "0";
                                            }
                                            //Get the LocationID
                                            var locIDResult = from locRow in dtLocation.AsEnumerable()
                                                              where locRow.Field<string>("Location").Equals(locName.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("LocationType").Equals("Row", StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("Site").Equals(siteName.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("ParentLocationiD").Equals(parentID, StringComparison.InvariantCultureIgnoreCase)

                                                              select locRow;
                                            if (locIDResult.Count() > 0)
                                            {
                                                dtLocResult = locIDResult.CopyToDataTable();
                                                locationId = Convert.ToInt32(dtLocResult.Rows[0]["LocationID"]);
                                            }
                                            else
                                            {
                                                locationId = 0;
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(dr["Room"].ToString().Trim()))
                                        {
                                            locName = dr["Room"].ToString();
                                            locType = "Room";
                                            //Get the LocationID
                                            var locIDResult = from locRow in dtLocation.AsEnumerable()
                                                              where locRow.Field<string>("Location").Equals(locName.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("LocationType").Equals("Room", StringComparison.InvariantCultureIgnoreCase) &&
                                                              locRow.Field<string>("Site").Equals(siteName.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                              select locRow;
                                            if (locIDResult.Count() > 0)
                                            {
                                                dtLocResult = locIDResult.CopyToDataTable();
                                                locationId = Convert.ToInt32(dtLocResult.Rows[0]["LocationID"]);
                                            }
                                            else
                                            {
                                                locationId = 0;
                                            }
                                        }
                                        // this validation is applicable for Tenants only.
                                        bool isTenantLocation = true;
                                        if (bool.Parse(Session["TenantUser"].ToString()))
                                        {
                                            string tenantAssignedLocations = string.Empty;
                                            UserBAL objUser = new UserBAL();
                                            objUser.UserID = Convert.ToInt32(Session["UserID"]);
                                            DataSet dsTenant = objUser.retrieveTenantDetails();
                                            if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
                                            {
                                                tenantAssignedLocations = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDLOCATIONS].ToString();
                                            }
                                            if (tenantAssignedLocations.Contains(locationId.ToString()))
                                                isTenantLocation = true;
                                            else
                                                isTenantLocation = false;
                                        }

                                        //5. Get the BusinessUnitID
                                        var buIDResult = from buRow in dtBusinessUnit.AsEnumerable()
                                                         where buRow.Field<string>("BusinessUnit").Equals(buName, StringComparison.InvariantCultureIgnoreCase)
                                                         select buRow;
                                        dtBUResult = buIDResult.CopyToDataTable();
                                        buID = Convert.ToInt32(dtBUResult.Rows[0]["BusinessUnitID"]);

                                        //6.Get the SiteID
                                        var siteIDResult = from siteRow in dtSite.AsEnumerable()
                                                           where siteRow.Field<string>("Site").Equals(siteName, StringComparison.InvariantCultureIgnoreCase)
                                                           select siteRow;
                                        dtSiteResult = siteIDResult.CopyToDataTable();
                                        siteID = Convert.ToInt32(dtSiteResult.Rows[0]["SiteID"]);

                                        //7. ExternalID
                                        externalID = dr["ExternalID"].ToString();

                                        //8. Owner
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

                                        if (isTenantLocation)
                                        {
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

                                                    #endregion

                                                    if (locationId > 0)
                                                    {

                                                        if (assetModelID > 0)
                                                        {

                                                            int orientationID = GetOrientationID(Orientation);
                                                            if (orientationID > 0)
                                                            {
                                                                LocationBAL locBAL = new LocationBAL();
                                                                if (!locBAL.IsPartOfDisposeDecomRooms(locationId))
                                                                {
                                                                    modelBAL.ModelID = Convert.ToInt32(assetModelID.ToString());
                                                                    DataSet dsModel = modelBAL.retrieve();
                                                                    if (dsModel != null && dsModel.Tables.Count > 0 && dsModel.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        float modelWidth = float.Parse(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_WIDTH].ToString());
                                                                        float modelDepth = float.Parse(dsModel.Tables[0].Rows[0][DBFields.DBFIELD_MODEL_DEPTH].ToString());
                                                                        locBAL = new LocationBAL();
                                                                        bool posValidation = false;

                                                                        if (locType.CompareTo("Rack") == 0)
                                                                        {
                                                                            if (mountType.CompareTo("Vertical Mount") != 0)
                                                                            {
                                                                                if (startPos == 0)
                                                                                {
                                                                                    posValidation = false;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (locBAL.ValidateRackPosition(locationId, 0, startPos, noOfRUs, orientationID, modelWidth, modelDepth))
                                                                                    {
                                                                                        posValidation = true;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        posValidation = false;
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                posValidation = true;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            posValidation = true;
                                                                            //start position must be zero for equipment placed in row or room.
                                                                            startPos = 0;
                                                                        }

                                                                        if (posValidation)
                                                                        {
                                                                            objAsset = new iAssetTrack.BAL.AssetBAL();
                                                                            objAsset.AssetID = 0;
                                                                            objAsset.RefNumber = serialNo.Trim();
                                                                            objAsset.HostName = hostName;
                                                                            objAsset.AssetName = assetName;
                                                                            objAsset.StartPos = startPos;
                                                                            objAsset.NoOfRUs = noOfRUs;
                                                                            objAsset.AssetTypeId = assetTypeID;
                                                                            objAsset.ModelID = assetModelID;
                                                                            objAsset.LastSeenLocationID = locationId;
                                                                            objAsset.DefaultLocationID = locationId;
                                                                            objAsset.TechID = 0;
                                                                            objAsset.RackOrStand = mountType;
                                                                            objAsset.BusinessUnitID = buID;
                                                                            objAsset.PrimarySiteID = siteID;
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
                                                                            objAsset.Orientation = Orientation;
                                                                            objAsset.ExternalID = externalID;
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
                                                                            dr["Reason"] = "Selected Position is in use or not available";
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        dr["Status"] = "Failed";
                                                                        dr["Reason"] = "Incorrect Model Details, check again";
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    dr["Status"] = "Failed";
                                                                    dr["Reason"] = "Asset can't be placed in Decom or Dispose Room";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                dr["Status"] = "Failed";
                                                                dr["Reason"] = "Incorrect Orientation";
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
                                                        dr["Status"] = "Failed";
                                                        dr["Reason"] = "Location information is incorrect,check again";
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
                                            dr["Reason"] = "Asset location is not a tenant location";
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
                    dr["Reason"] = "Manufacturer and Model are mandatory, one of these values is missing";
                }

            }
            else
            {
                dr["Status"] = "Failed";
                dr["Reason"] = "Site and Room are mandatory, one of these values is missing";
            }
        }

        //Serialize the dataset dsExcel data - with 8 tables
        //(Excel data, BudsinessUnit table, Site table, Location table, Manufacturer table, Model table, 
        //AssetType table and Asset Table)
        dsExcelData.AcceptChanges();
        ImportAsset.SerializeExcelDataSetToXml(dsExcelData);


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

    private void populateAssetGrid()
    {
        DataSet ds = DeserializeDataSource();
        if (ds != null)
        {
            //grdAsset.ClearDataSource();
            grdAsset.DataSource = ds.Tables["Assets"];
            grdAsset.DataBind();

            // upload summary
            //total models
            if (ds.Tables.Contains("Assets") && ds.Tables["Assets"].Rows.Count > 0)
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
                drFS1["Name"] = "Total Assets found";
                drFS1["Value"] = totalAssets.ToString();
                dtUploadStats.Rows.Add(drFS1);
                //Success Count
                DataRow drFS2 = dtUploadStats.NewRow();
                drFS2["ID"] = "2";
                drFS2["Name"] = "No of Assets imported successfully";
                drFS2["Value"] = successAssets.ToString();
                dtUploadStats.Rows.Add(drFS2);

                //Failure Count
                DataRow drFS3 = dtUploadStats.NewRow();
                drFS3["ID"] = "3";
                drFS3["Name"] = "No of Assets failed to import";
                drFS3["Value"] = failedAssets.ToString();
                dtUploadStats.Rows.Add(drFS3);

                grdUploadStatus.DataSource = dtUploadStats;
                grdUploadStatus.DataBind();
                //Excel icon
                ibExportToExcel.Visible = true;


            }
        }
    }

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
            if (HttpContext.Current.Session["FileID"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadPath"].ToString() + "/") + HttpContext.Current.Session["FileID"].ToString() + ".xml";
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
            if (Session["FileID"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["FileID"].ToString() + ".xml");
                if (File.Exists(strFile))
                {
                    // Read the content of the file into a DataSet
                    XmlTextReader xtr = new XmlTextReader(strFile);
                    ds = new DataSet();
                    ds.ReadXml(xtr);
                    xtr.Close();
                }
            }
            else if (Session["tempFileID"] != null)
            {
                //strFile = Server.MapPath("~/AssetData/" + Session.SessionID + ".xml");
                strFile = Server.MapPath("~/AssetData/" + Session["tempFileID"].ToString() + ".xml");
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
            //lblFileUploadStatus.Text = ex.Message;
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            lblFileUploadStatus.Text = "Import file is corrupted, check error log for more information";
            lblFileUploadStatus.Visible = true;
        }
        return ds;
    }

    protected void grdRacks_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {

    }

    protected void grdRacks_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControlRacks.SetupPageList(this.grdRacks.Behaviors.Paging.PageCount);
            pagerControlRacks.SetCurrentPageNumber(grdRacks.Behaviors.Paging.PageIndex);
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
            pagerControlRacks.SetupPageList(this.grdRacks.Behaviors.Paging.PageCount);
            pagerControlRacks.SetCurrentPageNumber(grdRacks.Behaviors.Paging.PageIndex);
        }

    }

    protected void grdRacks_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {

    }

    private void populateRackGrid()
    {
        DataSet ds = DeserializeDataSource();
        if (ds != null)
        {
            //grdAsset.ClearDataSource();
            grdRacks.DataSource = ds.Tables["Racks"];
            grdRacks.DataBind();
        }
    }

    #endregion
}



