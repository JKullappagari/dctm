/*
File Name   :	AssetModel.aspx.cs

Description :	Used to create Asset Model

Date created:	23 June 2011

Modification History:
***********************
CR		Name			    Date			Description
New		Jagadeesh Babu K	23 June 2011	File has been created.
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
using Infragistics.Web.UI.NavigationControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

public partial class AssetModel : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.AssetModelBAL objAM;
    private iAssetTrack.BAL.ManufacturerBAL objMfg;
    private iAssetTrack.BAL.MountTypeBAL objMountType;
    private iAssetTrack.BAL.AirFlowDirectionBAL objAFDirection;
    private iAssetTrack.BAL.CommonBAL objCommon;
    DataTable _dtRights;
    protected int totalRecordCount = 0;
    private iAssetTrack_WebDataGrid_Paging_CustomerPagerControl pagerControl;
    private const string PROP_FILTERCOUNT = "FilteredCount";
    protected string ddlMfgName = string.Empty;
    protected string ddlMfgID = string.Empty;
    protected string ddlBusinessUnitVal = string.Empty;
    protected const string PROP_PDF = "PDF";
    protected bool chkExists = false;
    #endregion

    public float PowerDeratedFactor
    {
        get
        {
            return (ViewState[PROP_PDF] != null ? float.Parse(ViewState[PROP_PDF].ToString()) : 0.0f);
        }
        set
        {
            ViewState[PROP_PDF] = value;
        }
    }

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

        grdModel.ItemCommand += new Infragistics.Web.UI.GridControls.ItemCommandEventHandler(grdModel_ItemCommand);
        pagerControl = grdModel.Behaviors.Paging.PagerTemplateContainerTop.FindControl("CustomerPager") as iAssetTrack_WebDataGrid_Paging_CustomerPagerControl;
        if (pagerControl != null)
            pagerControl.PageChanged += new EventHandler<PageChangedEventArgs>(currentPageControl_PageChanged);
    }
    void currentPageControl_PageChanged(object sender, PageChangedEventArgs e)
    {
        this.grdModel.Behaviors.Paging.PageIndex = e.PageNumber;
        populateGrid();
    }
    protected void grdModel_DataBound(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            pagerControl.SetupPageList(this.grdModel.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdModel.Behaviors.Paging.PageIndex);
        }


        Control postbackControlInstance = null;
        string postbackControlName = this.Request.Params.Get("__EVENTTARGET");
        if (postbackControlName != null && postbackControlName != string.Empty)
            postbackControlInstance = this.FindControl(postbackControlName);
        if (postbackControlInstance != null && postbackControlInstance.ID == "PagerPageList")
        {
            //do nothing
            //pagerControl.SetupPageList(this.grdModel.Behaviors.Paging.PageCount);
        }
        else
        {
            pagerControl.SetupPageList(this.grdModel.Behaviors.Paging.PageCount);
            pagerControl.SetCurrentPageNumber(grdModel.Behaviors.Paging.PageIndex);
            //pagerControl.SetCurrentPageNumber(grdModel.Behaviors.Paging.PageIndex);
            //pagerControl.SetupPageList(this.grdModel.Behaviors.Paging.PageCount);
        }

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        populateGrid();
        //if (chkExists)
        //{
        //    grdModel.Columns[6].Hidden = false;
        //    ibDelete.Visible = true;
        //}
    }
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageHeader"] = "Asset Model";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];

        lblMessage.Visible = false;

        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "AssetModel.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = _dtRights != null && _dtRights.Select("Module = 'Asset Model' and Rights = '" + "View" + "'").Length != 0;

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        if (_dtRights.Select("Module = 'Asset Model' and Rights = '" + "Create" + "'").Length != 0)
        {
            ibCreate.Enabled = true;
        }
        else
        {
            ibCreate.Enabled = false;
        }

        if (_dtRights.Select("Module = 'Asset Model' and Rights = '" + "Delete" + "'").Length != 0)
        {
            ibDelete.Visible = true;
        }
        else
        {
            ibDelete.Visible = false;
        }

        this.grdModel.Behaviors.Paging.PageSize = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());

        populateGrid();

        if (!IsPostBack)
        {
            Session["AssetModelID"] = null;
            populateManufacturerList();
            populateBusinessUnitList();
            ddlBusinessUnit.SelectedIndex = 1;
            ddlBusinessUnit.Enabled = false;
            PopulateTechCatList();
            populateAssetTypes();
            populateMountTypeList();
            populateAirFlowDirectionList();
            populateInputConnectorTypeList();
            populateOutputConnectorTypeList();
            EnableDisableEnclosureControls(false);
            EnableDisableBladeControls(false);
            EnableDisableRackControls();

            //Get Power Derated Factor (PDF)
            float pdf;
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PDF"].ToString()) && float.TryParse(ConfigurationManager.AppSettings["PDF"].ToString(), out pdf))
                {
                    if (pdf > 1)
                        this.PowerDeratedFactor = 1;
                    else
                        this.PowerDeratedFactor = pdf;
                }
                else
                {
                    this.PowerDeratedFactor = 1;
                }
            }
            catch
            {
                this.PowerDeratedFactor = 1;
            }

            txtModel.Focus();

        }
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }
        if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
            ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;

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
            string sMessage = objCommon.displayMessage(MessageCodes.ASSETMODEL_JS_DELETE);
            hdnMessage.Value = sMessage;
        }
        string errMsg = GetLocalResourceObject("revDesc1Resource1.ErrorMessage").ToString();
        revDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
        cvDesc.ErrorMessage = errMsg.Replace("{MAXLENGTH}", txtDesc.MaxLength.ToString());
    }

    /// <summary>
    /// Used to call upon grid page index changes.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdModel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //grdModel.PageIndex = e.NewPageIndex;
        //populateGrid();
    }

    /// <summary>
    /// Used to call upon grid row edits.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void grdModel_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    private void populateManufacturerList()
    {
        objMfg = new iAssetTrack.BAL.ManufacturerBAL();
        DataSet dsMfg = objMfg.retrieve();
        DataTable dtMfg = dsMfg.Tables[0];

        DataRow dr = dtMfg.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtMfg.Rows.InsertAt(dr, 0);

        ddlMfg.DataSource = dtMfg;
        ddlMfg.DataValueField = dtMfg.Columns[0].ToString();
        ddlMfg.DataTextField = dtMfg.Columns[1].ToString();
        ddlMfg.DataBind();
    }

    private void populateBusinessUnitList()
    {
        int intUserID = 0;
        if (Session["UserID"] != null)
            intUserID = Convert.ToInt32(Session["UserID"].ToString());

        FillDropDownListBU(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, ref ddlBusinessUnit, intUserID);
    }


    private void FillDropDownListBU(string strStoredProc, ref DropDownList ddl, int id)
    {

        //CommonBAL objCommonBAL = new CommonBAL();
        //ICommon svcBU = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        iAssetTrack.BAL.CommonBAL objCommon = new iAssetTrack.BAL.CommonBAL();

        DataTable dt = objCommon.FillDropDownListBU(strStoredProc, "-Select-", id);
        ddl.DataSource = dt;
        ddl.DataValueField = dt.Columns[0].ToString();
        ddl.DataTextField = dt.Columns[1].ToString();
        ddl.DataBind();

    }



    private void PopulateTechCatList()
    {
        TechCatBAL objTC = new TechCatBAL();
        DataSet dsTC = objTC.retrieve();
        DataTable dtTC = dsTC.Tables[0];

        DataRow dr = dtTC.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtTC.Rows.InsertAt(dr, 0);

        ddlTech.DataSource = dtTC;
        ddlTech.DataValueField = dtTC.Columns[0].ToString();
        ddlTech.DataTextField = dtTC.Columns[1].ToString();
        ddlTech.DataBind();
    }

    //private void LoadModels()
    //{

    //    AssetModelBAL objParentAM = new iAssetTrack.BAL.AssetModelBAL();

    //    //ManufacturerBAL objManf = new iAssetTrack.BAL.ManufacturerBAL();
    //    //DataSet dsManf = objManf.retrieve();
    //    //DataTable dtManf = dsManf.Tables[0];

    //    WebDataTree loctree = (WebDataTree)ddlModelList.Items[0].FindControl("TreeLocation");
    //    loctree.Nodes.Clear();
    //    // add Manufacturers
    //    DataTreeNode clearNode = new DataTreeNode();
    //    clearNode.Text = "No Model";
    //    clearNode.Value = "0";
    //    loctree.Nodes.Add(clearNode);
    //    //using (DataTableReader dtMFGRdr = dtManf.CreateDataReader())
    //    //{
    //    //    while (dtMFGRdr.Read())
    //    //    {

    //    DataTreeNode mfgNode = new DataTreeNode();
    //    mfgNode.Text = ddlMfg.SelectedItem.Text;
    //    mfgNode.Value = ddlMfg.SelectedItem.Value;
    //    mfgNode.Enabled = false;
    //    mfgNode.CssClass = ".TreeNodeDisabled";
    //    loctree.Nodes.Add(mfgNode);
    //    DataSet dsParentAM = objParentAM.retrieveByMfg(Int32.Parse(ddlMfg.SelectedItem.Value),
    //                            int.Parse(ddlBusinessUnit.SelectedItem.Value));
    //    DataTable dtParentAM = dsParentAM.Tables[0];
    //    using (DataTableReader dtModelByMfgRdr = dtParentAM.CreateDataReader())
    //    {
    //        while (dtModelByMfgRdr.Read())
    //        {
    //            DataTreeNode ModelNode = new DataTreeNode();
    //            ModelNode.Text = dtModelByMfgRdr.GetValue(1).ToString();
    //            ModelNode.Value = dtModelByMfgRdr.GetValue(0).ToString();
    //            //ModelNode.Enabled = false;
    //            //ModelNode.CssClass = ".TreeNodeDisabled";
    //            mfgNode.Nodes.Add(ModelNode);
    //            if (Int32.Parse(dtModelByMfgRdr.GetValue(2).ToString()) > 0) // child count of this model
    //                getChildModels(ModelNode, Convert.ToInt32(dtModelByMfgRdr.GetValue(0).ToString()));

    //        }
    //    }
    //    //    }

    //    //}




    //}

    //private void getChildModels(DataTreeNode node, int ModelID)
    //{
    //    AssetModelBAL objChildAM = new AssetModelBAL();
    //    DataSet dsChildAM = objChildAM.retrieveByParentModelID(ModelID);
    //    DataTable dtChildAM = dsChildAM.Tables[0];
    //    if (dtChildAM.Rows.Count > 0)
    //    {
    //        using (DataTableReader dtModelByModelRdr = dtChildAM.CreateDataReader())
    //        {
    //            while (dtModelByModelRdr.Read())
    //            {
    //                DataTreeNode ModelNode = new DataTreeNode();
    //                ModelNode.Text = dtModelByModelRdr.GetValue(1).ToString();
    //                ModelNode.Value = dtModelByModelRdr.GetValue(0).ToString();
    //                //ModelNode.Enabled = false;
    //                //ModelNode.CssClass = ".TreeNodeDisabled";
    //                node.Nodes.Add(ModelNode);
    //                getChildModels(ModelNode, Convert.ToInt32(dtModelByModelRdr.GetValue(0).ToString()));
    //            }
    //        }
    //    }

    //}

    /// <summary>
    /// Used to save information related BU.
    /// </summary>
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            objAM = new iAssetTrack.BAL.AssetModelBAL();

            objAM.ModelName = txtModel.Text.Trim();
            if (ddlMfg.SelectedIndex > 0)
                objAM.MfgID = Convert.ToInt32(ddlMfg.SelectedItem.Value);

            if (ddlBusinessUnit.SelectedIndex > 0)
                objAM.BUID = int.Parse(ddlBusinessUnit.SelectedItem.Value);

            if (Session["AssetModelID"] != null)
            {
                objAM.ModelID = (int)Session["AssetModelID"];
            }
            else
            {
                objAM.ModelID = objAM.exists();
            }

            int intModel = 0;
            objAM.ModelID = Session["AssetModelID"] == null ? intModel : (int)Session["AssetModelID"];
            intModel = objAM.exists();

            if (hdnSPCID.Value != "")
            {
                objAM.SPCID = Convert.ToInt32(hdnSPCID.Value);
            }

            objAM.Description = txtDesc.Text.Trim();
            objAM.Status = 1;
            objAM.CreatedBy = Convert.ToInt32(Session["UserID"]);

            if (ddlMfg.SelectedIndex > 0)
                objAM.MfgID = Convert.ToInt32(ddlMfg.SelectedItem.Value);

            if (ddlTech.SelectedIndex > 0)
                objAM.TechID = Convert.ToInt32(ddlTech.SelectedItem.Value);


            objAM.IsBlade = chkIsBlade.Checked;
            objAM.IsEnclosure = chkIsEnclosure.Checked;

            //ModelType
            if (ddlModelType.SelectedIndex > 0)
                objAM.ModelTypeID = int.Parse(ddlModelType.SelectedItem.Value);
            else
                objAM.ModelTypeID = 0;

            //Physical
            if (!string.IsNullOrEmpty(txtWidth.Text.Trim()))
                objAM.Width = float.Parse(txtWidth.Text);
            else
                objAM.Width = 0.0f;

            if (!string.IsNullOrEmpty(txtDepth.Text.Trim()))
                objAM.Depth = float.Parse(txtDepth.Text);
            else
                objAM.Depth = 0.0f;

            if (!string.IsNullOrEmpty(txtHeight.Text.Trim()))
                objAM.Height = float.Parse(txtHeight.Text);
            else
                objAM.Height = 0.0f;

            if (!string.IsNullOrEmpty(txtWeight.Text.Trim()))
                objAM.Weight = float.Parse(txtWeight.Text);
            else
                objAM.Weight = 0.0f;

            if (!string.IsNullOrEmpty(txtUHeight.Text.Trim()))
                objAM.UHeight = int.Parse(txtUHeight.Text);
            else
                objAM.UHeight = 0;

            //Power
            if (!string.IsNullOrEmpty(txtMaxPower.Text.Trim()))
                objAM.MaxPower = float.Parse(txtMaxPower.Text);
            else
                objAM.MaxPower = 0.0f;

            if (!string.IsNullOrEmpty(txtSSWatts.Text.Trim()))
                objAM.SteadyStatePower = float.Parse(txtSSWatts.Text);
            else
                objAM.SteadyStatePower = 0.0f;

            if (!string.IsNullOrEmpty(txtTotalPSUCount.Text.Trim()))
                objAM.TotalPSUCount = int.Parse(txtTotalPSUCount.Text);
            else
                objAM.TotalPSUCount = 0;

            if (!string.IsNullOrEmpty(txtReqPSUCount.Text.Trim()))
                objAM.RequiredPSUCount = int.Parse(txtReqPSUCount.Text);
            else
                objAM.RequiredPSUCount = 0;

            if (ddlCPDUSide.SelectedIndex > 0)
                objAM.ConnectorPDUSide = ddlCPDUSide.SelectedItem.Text;
            else
                objAM.ConnectorPDUSide = string.Empty;

            if (ddlCDevSide.SelectedIndex > 0)
                objAM.ConnectorDevSide = ddlCDevSide.SelectedItem.Text;
            else
                objAM.ConnectorDevSide = string.Empty;

            //Mounting
            if (ddlMountType.SelectedIndex > 0)
                objAM.MountTypeID = int.Parse(ddlMountType.SelectedItem.Value);
            else
                objAM.MountTypeID = 0;
            if (ddlAFDirection.SelectedIndex > 0)
                objAM.AFDirectionID = int.Parse(ddlAFDirection.SelectedItem.Value);
            else
                objAM.AFDirectionID = 0;

            //Rack specific
            if (ddlModelType.SelectedItem.Text.ToLower().CompareTo("rack") == 0)
            {
                if (!string.IsNullOrEmpty(txtRInternalDepth.Text))
                    objAM.RackInternalDepth = float.Parse(txtRInternalDepth.Text);
                else
                    objAM.RackInternalDepth = 0.0f;

                if (!string.IsNullOrEmpty(txtRInternalHeight.Text))
                    objAM.RackInternalHeight = float.Parse(txtRInternalHeight.Text);
                else
                    objAM.RackInternalHeight = 0.0f;

                if (!string.IsNullOrEmpty(txtRInternalWidth.Text))
                    objAM.RackInternalWidth = float.Parse(txtRInternalWidth.Text);
                else
                    objAM.RackInternalWidth = 0.0f;
            }
            else
            {
                objAM.RackInternalDepth = 0.0f;
                objAM.RackInternalHeight = 0.0f;
                objAM.RackInternalWidth = 0.0f;
            }


            //Enclosure specific
            if (chkIsEnclosure.Checked)
            {
                if (!string.IsNullOrEmpty(txtEnclFrontRowCount.Text))
                    objAM.EnclFrontRowCount = int.Parse(txtEnclFrontRowCount.Text);
                else
                    objAM.EnclFrontRowCount = 0;

                if (!string.IsNullOrEmpty(txtEnclFrontColCount.Text))
                    objAM.EnclFrontColCount = int.Parse(txtEnclFrontColCount.Text);
                else
                    objAM.EnclFrontColCount = 0;

                if (!string.IsNullOrEmpty(txtEnclRearRowCount.Text))
                    objAM.EnclRearRowCount = int.Parse(txtEnclRearRowCount.Text);
                else
                    objAM.EnclRearRowCount = 0;


                if (!string.IsNullOrEmpty(txtEnclRearColCount.Text))
                    objAM.EnclRearColCount = int.Parse(txtEnclRearColCount.Text);
                else
                    objAM.EnclRearColCount = 0;
            }


            if (chkIsBlade.Checked)
            {
                if (!string.IsNullOrEmpty(txtBladeRowCount.Text))
                    objAM.BladeRowCount = int.Parse(txtBladeRowCount.Text);
                else
                    objAM.BladeRowCount = 0;

                if (!string.IsNullOrEmpty(txtBladeColCount.Text))
                    objAM.BladeColCount = int.Parse(txtBladeColCount.Text);
                else
                    objAM.BladeColCount = 0;
            }

            if (ddlMountType.SelectedItem.Text.ToLower().CompareTo("vertical mount") == 0 && ddlModelType.SelectedItem.Text.ToLower().CompareTo("rack pdu-zero u") != 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Only Rack PDU-Zero U is allowed with mount type as Vertical mount";
                return;
            }

            if (ddlMountType.SelectedItem.Text.ToLower().CompareTo("rackmount") == 0
                && objAM.Width > 0.0f && objAM.Width > 445.6f)
            {
                objAM.Width = 445.5f;
            }


            if (intModel != -1 && intModel != 0)
                objAM.ModelID = intModel;

            if (intModel != -1)
            {
                objAM.Persist(DALCOperation.Insert);
                clearFields();

                if (Session["AssetModelID"] == null)
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
                grdModel.ClearDataSource();
                populateGrid();
                Session["AssetModelID"] = null;
            }
            else
            {
                objCommon = new CommonBAL();
                lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_I_EXISTS);
                lblMessage.Visible = true;
                populateGrid();
            }
        }
        catch (Exception ex)
        {
            //lblMessage.Text = ex.Message;
            //lblMessage.Visible = true;
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
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
        Session["AssetModelID"] = null;
        txtModel.Focus();
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
        int modelID;
        string strIDs;

        strIDs = "";
        foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdModel.Rows)
        {
            chkDelete = (CheckBox)(grdViewRow.Items[6].FindControl("chkDelete"));
            if (chkDelete.Checked == true)
            {
                modelID = Convert.ToInt32(((Label)(grdViewRow.Items[6].FindControl("lblDeleteID"))).Text);
                strIDs += Convert.ToString(modelID) + ",";
            }
        }

        if (strIDs != "")
        {
            strIDs = strIDs.Remove(strIDs.Length - 1, 1);
        }

        objAM = new iAssetTrack.BAL.AssetModelBAL();
        objAM.ModelIDs = strIDs;
        objAM.Status = 0;
        objAM.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
        objAM.Persist(DALCOperation.Delete);

        clearFields();

        objCommon = new CommonBAL();
        lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
        lblMessage.Visible = true;
        grdModel.ClearDataSource();
        populateGrid();
    }
    #endregion

    #region "User Defined Methods"
    /// <summary>
    /// Populate Business Unit data grid
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    //private void populateGrid()
    //{
    //    grdModel.ClearDataSource();
    //    grdModel.Rows.Clear();
    //    objAM = new iAssetTrack.BAL.AssetModelBAL();
    //    //grdModel.ClearDataSource();
    //    grdModel.DataSource = objAM.retrieve().Tables[0];
    //    grdModel.DataBind();
    //    applyRights();

    //}

    private void populateGrid()
    {
        //objAM = new iAssetTrack.BAL.AssetModelBAL();
        //grdModel.DataSource = objAM.retrieve().Tables[0];
        //grdModel.DataBind();
        objAM = new iAssetTrack.BAL.AssetModelBAL();
        objAM.BUID = Convert.ToInt16(Session["BUID"]);
        objAM.ModelID = 0;
        DataTable dtGrid = objAM.retrieve().Tables[0];
        totalRecordCount = dtGrid.Rows.Count;

        grdModel.Behaviors.Paging.Enabled = false;
        if (totalRecordCount == grdModel.Rows.Count)
            this.FilterCount = "";
        grdModel.Behaviors.Paging.Enabled = true;

        //grdModel.DataSource = dtGrid;
        //grdModel.DataBind();

        if (dtGrid.Rows.Count == 0)
        {
            grdModel.DataSource = dtGrid;
            grdModel.DataBind();
            grdModel.Columns[5].Hidden = true;
            ibDelete.Visible = false;
        }
        else //if (grdModel.Rows.Count > 0)
        {
            grdModel.DataSource = dtGrid;
            grdModel.DataBind();

            //if (_dtRights.Select("Rights = 'Map Technology Category' and Module = 'Asset Model'").Length != 0)
            //{
            //    grdModel.Columns[6].Hidden = false;
            //    ibMap.Visible = true;
            //    ddlTech.Enabled = true;
            //}
            //else
            //{
            //    grdModel.Columns[6].Hidden = true;
            //    ibMap.Visible = false;
            //    ddlTech.Enabled = false;
            //}

            if (_dtRights.Select("Rights = 'Delete' and Module = 'Asset Model'").Length != 0)
            {
                grdModel.Columns[6].Hidden = false;
                ibDelete.Visible = true;
            }
            else
            {
                grdModel.Columns[6].Hidden = true;
                ibDelete.Visible = false;
            }

            if (_dtRights.Select("Rights = 'Modify' and Module = 'Asset Model'").Length != 0)
            {
                grdModel.Columns[5].Hidden = false;
            }
            else
            {
                grdModel.Columns[5].Hidden = true;
            }

        }

        if (ibDelete.Visible == true)
        {
            int modelID;
            int iCount = 0;

            for (int i = 0; i < grdModel.Rows.Count; i++)
            {
                modelID = Convert.ToInt32(((Label)(grdModel.Rows[i].Items[6].FindControl("lblDeleteID"))).Text);

                grdModel.Rows[i].Items[6].FindControl("chkDelete").Visible = true;
                objCommon = new iAssetTrack.BAL.CommonBAL();

                DataSet dsCheck = objCommon.CheckBeforeDelete(DBFields.DBFIELD_MODELID, modelID.ToString(), 0);
                foreach (DataTable tblCheck in dsCheck.Tables)
                {
                    if (tblCheck.Rows[0][0].ToString() != "0")
                    {
                        grdModel.Rows[i].Items[6].FindControl("chkDelete").Visible = false;
                    }
                }
                if (grdModel.Rows[i].Items[6].FindControl("chkDelete").Visible == false)
                {
                    iCount++;
                }

            }

            if (iCount == grdModel.Rows.Count)
            {
                grdModel.Columns[6].Hidden = true;
            }
        }

        if (grdModel.Rows.Count > 0)
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

    /// <summary>
    /// Reset fields
    /// </summary>    
    /// <author>Venkatesan</author>
    /// <createdOn>27 March 2006</createdOn>
    private void clearFields()
    {
        txtModel.Text = "";
        txtDesc.Text = "";
        lblMessage.Visible = false;
        lblMessage.Text = "";
        ddlMfg.SelectedIndex = 0;
        ddlTech.SelectedIndex = 0;
        ddlModelType.SelectedIndex = 0;
        ddlMountType.SelectedIndex = 0;
        ddlAFDirection.SelectedIndex = 0;
        txtDepth.Text = "";
        txtWidth.Text = "";
        txtHeight.Text = "";
        txtUHeight.Text = "";
        txtWeight.Text = "";
        txtMaxPower.Text = "";
        txtSSWatts.Text = "";
        txtTotalPSUCount.Text = "";
        txtReqPSUCount.Text = "";
        ddlCDevSide.SelectedIndex = 0;
        ddlCPDUSide.SelectedIndex = 0;
        txtRInternalDepth.Text = "";
        txtRInternalHeight.Text = "";
        txtRInternalWidth.Text = "";
        chkIsBlade.Checked = false;
        chkIsEnclosure.Checked = false;
        EnableDisableControls();
        txtEnclFrontColCount.Text = "";
        txtEnclFrontRowCount.Text = "";
        txtEnclRearColCount.Text = "";
        txtEnclRearRowCount.Text = "";
        txtBladeColCount.Text = "";
        txtBladeRowCount.Text = "";
        txtHeight.Enabled = true;
        txtUHeight.Enabled = true;
        txtDepth.Enabled = true;
        txtWidth.Enabled = true;

        ddlModelType.Enabled = true;
        ddlMountType.Enabled = true;

        chkIsEnclosure.Enabled = true;
        chkIsBlade.Enabled = true;
    }
    #endregion
    protected void grdModel_ItemCommand(object sender, Infragistics.Web.UI.GridControls.HandleCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            clearFields();
            populateGrid();
            objAM = new iAssetTrack.BAL.AssetModelBAL();
            objAM.BUID = Convert.ToInt16(Session["BUID"]);
            objAM.ModelID = Convert.ToInt32(e.CommandArgument);
            DataSet dsBU = objAM.retrieve();
            DataRow dr = dsBU.Tables[0].Rows[0];
            txtModel.Text = dr[DBFields.DBFIELD_MODELNAME].ToString();
            txtDesc.Text = dr[DBFields.DBFIELD_DESCRIPTION].ToString();
            ddlMfg.SelectedValue = dr[DBFields.DBFIELD_MFGID].ToString();
            hdnSPCID.Value = dr[DBFields.DBFIELD_SPCID].ToString();

            if (ddlMfg != null && ddlMfg.Items.Count > 0)
            {
                ddlMfgName = ddlMfg.SelectedItem.Text;
                ddlMfgID = ddlMfg.SelectedItem.Value;
            }
            if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
                ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;

            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_TECHID].ToString()) && !(ddlTech.Items.Count == 1))
                ddlTech.SelectedValue = dr[DBFields.DBFIELD_TECHID].ToString();
            else
                ddlTech.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_BUSINESSUNITID].ToString()))
                ddlBusinessUnit.SelectedValue = dr[DBFields.DBFIELD_BUSINESSUNITID].ToString();
            else
                ddlBusinessUnit.SelectedIndex = 0;

            chkIsBlade.Checked = bool.Parse(dr[DBFields.DBFIELD_IS_BLADE].ToString()) == true ? true : false;
            chkIsEnclosure.Checked = bool.Parse(dr[DBFields.DBFIELD_IS_ENCLOSURE].ToString()) == true ? true : false;

            EnableDisableBladeControls(chkIsBlade.Checked);
            EnableDisableEnclosureControls(chkIsEnclosure.Checked);



            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_MODEL_TYPE_ID].ToString()))
                ddlModelType.SelectedValue = dr[DBFields.DBFIELD_MODEL_TYPE_ID].ToString();
            else
                ddlModelType.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_MODEL_MOUNT_TYPE_ID].ToString()))
                ddlMountType.SelectedValue = dr[DBFields.DBFIELD_MODEL_MOUNT_TYPE_ID].ToString();
            else
                ddlMountType.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_MODEL_AF_DIRECTION_ID].ToString()))
                ddlAFDirection.SelectedValue = dr[DBFields.DBFIELD_MODEL_AF_DIRECTION_ID].ToString();
            else
                ddlAFDirection.SelectedIndex = 0;


            txtDepth.Text = dr[DBFields.DBFIELD_MODEL_DEPTH].ToString();
            txtWidth.Text = dr[DBFields.DBFIELD_MODEL_WIDTH].ToString();

            //System won't allow to modify Height and UHeight fields if number of assets mapped to this model is more than zero
            if (int.Parse(dr[DBFields.DBFIELD_ASSET_COUNT].ToString()) > 0)
            {
                txtHeight.Enabled = false;
                txtUHeight.Enabled = false;
                txtWidth.Enabled = false;
                txtDepth.Enabled = false;
                ddlModelType.Enabled = false;
                ddlMountType.Enabled = false;

                revHeight.Enabled = false;
                rvHeight.Enabled = false;
                rfvHeight.Enabled = false;

                revWidth.Enabled = false;
                rfvWidth.Enabled = false;
                rvWidth.Enabled = false;

                revDepth.Enabled = false;
                rfvDepth.Enabled = false;
                rvDepth.Enabled = false;

                revUHeight.Enabled = false;
                rvUHeight.Enabled = false;
                rfvUHeight.Enabled = false;

                chkIsBlade.Checked = false;
                chkIsEnclosure.Enabled = false;
                chkIsBlade.Enabled = false;
                chkIsEnclosure.Enabled = false;



            }
            else
            {
                txtHeight.Enabled = true;
                txtUHeight.Enabled = true;
                txtWidth.Enabled = true;
                txtDepth.Enabled = true;

                revHeight.Enabled = true;
                rvHeight.Enabled = true;
                rfvHeight.Enabled = true;

                revWidth.Enabled = true;
                rfvWidth.Enabled = true;
                rvWidth.Enabled = true;

                revDepth.Enabled = true;
                rfvDepth.Enabled = true;
                rvDepth.Enabled = true;

                revUHeight.Enabled = true;
                rvUHeight.Enabled = true;
                rfvUHeight.Enabled = true;

            }
            EnableDisableRackControls();
            if (int.Parse(dr[DBFields.DBFIELD_ASSET_COUNT].ToString()) > 0)
            {
                txtRInternalDepth.Enabled = false;
                txtRInternalWidth.Enabled = false;
                txtRInternalHeight.Enabled = false;
            }

            txtHeight.Text = dr[DBFields.DBFIELD_MODEL_HEIGHT].ToString();
            txtUHeight.Text = dr[DBFields.DBFIELD_MODEL_UHEIGHT].ToString();

            txtWeight.Text = dr[DBFields.DBFIELD_MODEL_WEIGHT].ToString();
            txtMaxPower.Text = dr[DBFields.DBFIELD_MODEL_MAX_POWER].ToString();
            txtSSWatts.Text = dr[DBFields.DBFIELD_MODEL_SS_POWER].ToString();
            txtTotalPSUCount.Text = dr[DBFields.DBFIELD_MODEL_TOTAL_PSU_COUNT].ToString();
            txtReqPSUCount.Text = dr[DBFields.DBFIELD_MODEL_REQ_PSU_COUNT].ToString();
            CompareItem(ddlCDevSide, dr[DBFields.DBFIELD_MODEL_CONN_DEV_SIDE].ToString());
            CompareItem(ddlCPDUSide, dr[DBFields.DBFIELD_MODEL_CONN_PDU_SIDE].ToString());
            txtRInternalDepth.Text = dr[DBFields.DBFIELD_MODEL_RACK_INTERNAL_DEPTH].ToString();
            txtRInternalHeight.Text = dr[DBFields.DBFIELD_MODEL_RACK_INTERNAL_HEIGHT].ToString();
            txtRInternalWidth.Text = dr[DBFields.DBFIELD_MODEL_RACK_INTERNAL_WIDTH].ToString();

            if (chkIsEnclosure.Checked)
            {
                txtEnclFrontColCount.Text = dr[DBFields.DBFIELD_MODEL_ENCL_FRONT_COL_COUNT].ToString();
                txtEnclFrontRowCount.Text = dr[DBFields.DBFIELD_MODEL_ENCL_FRONT_ROW_COUNT].ToString();
                txtEnclRearColCount.Text = dr[DBFields.DBFIELD_MODEL_ENCL_REAR_COL_COUNT].ToString();
                txtEnclRearRowCount.Text = dr[DBFields.DBFIELD_MODEL_ENCL_REAR_ROW_COUNT].ToString();
                //if number of asset mapped to this enclosure is more than zero than 
                // disable  row X column counts
                if (int.Parse(dr[DBFields.DBFIELD_ASSET_COUNT].ToString()) > 0)
                {
                    txtEnclFrontRowCount.Enabled = false;
                    txtEnclFrontColCount.Enabled = false;
                    txtEnclRearRowCount.Enabled = false;
                    txtEnclRearColCount.Enabled = false;

                    revEnclFrontRowCount.Enabled = false;
                    rfvEnclFrontRowCount.Enabled = false;
                    rvEnclFrontRowCount.Enabled = false;

                    revEnclFrontColCount.Enabled = false;
                    rfvEnclFrontColCount.Enabled = false;
                    rvEnclFrontColCount.Enabled = false;

                    revEnclRearRowCount.Enabled = false;
                    rfvEnclRearRowCount.Enabled = false;
                    rvEnclRearRowCount.Enabled = false;

                    revEnclRearColCount.Enabled = false;
                    rfvEnclRearColCount.Enabled = false;
                    rvEnclRearColCount.Enabled = false;

                    chkIsEnclosure.Enabled = false;
                    chkIsBlade.Enabled = false;

                }

            }
            if (chkIsBlade.Checked)
            {
                txtBladeColCount.Text = dr[DBFields.DBFIELD_MODEL_BLADE_COL_COUNT].ToString();
                txtBladeRowCount.Text = dr[DBFields.DBFIELD_MODEL_BLADE_ROW_COUNT].ToString();

                // incase of blade model, all rack mount controls will be disabled.
                EnableDisableRackMountControls(false);

                //if number of asset mapped to this enclosure is more than zero than 
                // disable  row X column counts
                if (int.Parse(dr[DBFields.DBFIELD_ASSET_COUNT].ToString()) > 0)
                {
                    txtBladeRowCount.Enabled = false;
                    txtBladeColCount.Enabled = false;

                    revBladeRowCount.Enabled = false;
                    rfvBladeRowCount.Enabled = false;
                    rvBladeRowCount.Enabled = false;

                    revBladeColCount.Enabled = false;
                    rfvBladeColCount.Enabled = false;
                    rvBladeColCount.Enabled = false;

                    chkIsEnclosure.Enabled = false;
                    chkIsBlade.Enabled = false;
                }
            }

            Session["AssetModelID"] = objAM.ModelID;

            if (_dtRights.Select("Module = 'Asset Model' and Rights = '" + "Modify" + "'").Length != 0)
            {
                ibCreate.Enabled = true;
            }
            else
            {
                ibCreate.Enabled = false;
            }
            if (_dtRights.Select("Rights = 'Map Technology Category' and Module = 'Asset Model'").Length != 0)
            {
                ddlTech.Enabled = true;
            }
            else
            {
                ddlTech.Enabled = false;
            }



        }
        else if (e.CommandName == "Delete")
        {
            objAM = new iAssetTrack.BAL.AssetModelBAL();
            objAM.ModelIDs = Convert.ToString(e.CommandArgument);
            objAM.Status = 0;

            objAM.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
            objAM.Persist(DALCOperation.Delete);

            clearFields();

            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_DELETED);
            lblMessage.Visible = true;
            grdModel.ClearDataSource();
            populateGrid();
        }
    }

    protected void ibExportToExcel_Click(object sender, EventArgs e)
    {
        Infragistics.Documents.Excel.WorkbookFormat excelFormat = Infragistics.Documents.Excel.WorkbookFormat.Excel2007;
        this.eExporter.DataExportMode = Infragistics.Web.UI.GridControls.DataExportMode.AllDataInDataSource;
        Infragistics.Documents.Excel.Workbook wBook = new Infragistics.Documents.Excel.Workbook(excelFormat);
        this.eExporter.Export(this.grdModel, wBook);
    }
    protected void eExporter_CellExported(object sender, Infragistics.Web.UI.GridControls.ExcelCellExportedEventArgs e)
    {
        int iWSdex = e.Worksheet.Index;
        int iRdex = e.CurrentRowIndex;
        int iCdex = e.CurrentColumnIndex;
        e.Worksheet.Columns[4].Width = 1;
        e.Worksheet.Columns[5].Width = 1;
        e.Worksheet.Columns[6].Width = 1;
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
                if (iCdex == 6 && e.Worksheet.Rows[iRdex].Cells[iCdex].Value != null)
                {
                    e.Worksheet.Rows[iRdex].Cells[iCdex].Value = "";
                }

            }

        }
    }


    protected void ddlMfg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMfg != null && ddlMfg.Items.Count > 0)
        {
            ddlMfgName = ddlMfg.SelectedItem.Text;
            ddlMfgID = ddlMfg.SelectedItem.Value;
        }
    }



    protected void ibMap_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        CheckBox chkDelete;
        int modelID;
        string strIDs;

        try
        {
            strIDs = "";
            foreach (Infragistics.Web.UI.GridControls.GridRecord grdViewRow in grdModel.Rows)
            {
                chkDelete = (CheckBox)(grdViewRow.Items[6].FindControl("chkDelete"));
                if (chkDelete.Checked == true)
                {
                    modelID = Convert.ToInt32(((Label)(grdViewRow.Items[6].FindControl("lblDeleteID"))).Text);
                    strIDs += Convert.ToString(modelID) + ",";
                }
            }

            if (strIDs != "")
            {
                strIDs = strIDs.Remove(strIDs.Length - 1, 1);
            }

            objAM = new AssetModelBAL();
            objAM.ModelIDs = strIDs;
            objAM.LastModifiedBy = Convert.ToInt32(Session["UserID"]);
            if (ddlTech.SelectedIndex > 0)
                objAM.TechID = Convert.ToInt32(ddlTech.SelectedItem.Value);
            objAM.Persist(DALCOperation.Delete);
            clearFields();
            objCommon = new CommonBAL();
            lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            lblMessage.Visible = true;
            populateGrid();

            # region commented code
            //foreach (string id in strIDs.Split(','))
            //{
            //    AssetModelBAL objAM = new iAssetTrack.BAL.AssetModelBAL();
            //    DataSet dsAM = objAM.retrieveByModelID(Int32.Parse(id));

            //    //update 
            //    if (dsAM.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow dr = dsAM.Tables[0].Rows[0];
            //        objAM.ModelID = Int32.Parse(id);
            //        objAM.ModelName = dr[DBFields.DBFIELD_MODELNAME].ToString();
            //        objAM.Status = (dr[DBFields.DBFIELD_STATUS].ToString().ToLower().Equals("true") ? 1 : 0);
            //        objAM.CreatedBy = Convert.ToInt32(Session["UserID"]);
            //        if (!string.IsNullOrEmpty(dr[DBFields.DBFIELD_PARENTMODELID].ToString()))
            //            objAM.ParentModelID = Int32.Parse(dr[DBFields.DBFIELD_PARENTMODELID].ToString());
            //        else
            //            objAM.ParentModelID = 0;
            //        objAM.MfgID = Int32.Parse(dr[DBFields.DBFIELD_MFGID].ToString());
            //        if (!string.IsNullOrEmpty(dr["SpcID"].ToString().Trim()))
            //            objAM.SPCID = Int32.Parse(dr["SpcID"].ToString());
            //        else
            //            objAM.SPCID = 0;
            //        if (ddlTech.SelectedIndex > 0)
            //            objAM.TechID = Convert.ToInt32(ddlTech.SelectedItem.Value);

            //        objAM.Persist(DALCOperation.Insert);

            //    }

            //}
            //clearFields();
            //objCommon = new CommonBAL();
            //lblMessage.Text = objCommon.displayMessage(MessageCodes.GEN_S_UPDATED);
            //lblMessage.Visible = true;
            //grdModel.ClearDataSource();
            //populateGrid();
            # endregion

        }
        catch (Exception ex)
        {
            //lblMessage.Text = ex.Message;
            //lblMessage.Visible = true;
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
        }

    }

    protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlBusinessUnit != null && ddlBusinessUnit.Items.Count > 0)
            ddlBusinessUnitVal = ddlBusinessUnit.SelectedItem.Value;
    }
    protected void grdModel_DataFiltered(object sender, Infragistics.Web.UI.GridControls.FilteredEventArgs e)
    {
        //chkExists = false;
        // Cast the WebDataGrid DataSource to a DataTable  
        DataTable dt = this.grdModel.DataSource as DataTable;
        // Disable WebDataGrid Paging
        grdModel.Behaviors.Paging.Enabled = false;
        this.FilterCount = grdModel.Rows.Count.ToString();
        if (!string.IsNullOrEmpty(this.FilterCount))
        {
            if (totalRecordCount == int.Parse(this.FilterCount))
            {
                this.FilterCount = "";
            }
        }
        // Enable WebDataGrid Paging
        grdModel.Behaviors.Paging.Enabled = true;
        ////walk through each row and enable/disable ibDelete based on delete check box
        //int rowsCount = grdModel.Rows.Count;

        //foreach (Infragistics.Web.UI.GridControls.GridRecord rec in grdModel.Rows)
        //{
        //    if (rec.Items[6].FindControl("chkDelete").Visible)
        //    {
        //        chkExists = true;
        //        break;
        //    }
        //}
    }
    protected void grdModel_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
    {

    }
    protected void chkIsBlade_CheckedChanged(object sender, EventArgs e)
    {
        EnableDisableControls();
    }
    protected void chkIsEnclosure_CheckedChanged(object sender, EventArgs e)
    {
        EnableDisableControls();
    }

    protected void EnableDisableControls()
    {
        EnableDisableBladeControls(chkIsBlade.Checked);
        EnableDisableEnclosureControls(chkIsEnclosure.Checked);
    }

    protected void ddlModelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlModelType != null && ddlModelType.SelectedIndex > 0)
        {
            //Rack and Floor mounted Asset specific
            if (ddlModelType.SelectedItem.Text.ToLower().Contains("standalone") || ddlModelType.SelectedItem.Text.ToLower().CompareTo("rack") == 0)
            {
                CompareItem(ddlMountType, "Floor Standalone");
                chkIsBlade.Checked = false;
                chkIsEnclosure.Enabled = false;
                chkIsBlade.Enabled = false;
                chkIsEnclosure.Enabled = false;
                ddlMountType.Enabled = false;
                EnableDisableControls();
                EnableDisableRackControls();
            }
            else if (ddlModelType.SelectedItem.Text.ToLower().Contains("blade") || ddlModelType.SelectedItem.Text.ToLower().Contains("module"))
            {
                EnableDisableBladeControls(true);
                chkIsBlade.Checked = true;
                chkIsEnclosure.Checked = false;
                CompareItem(ddlMountType, "Enclosure Mount");
                EnableDisableRackControls();
                EnableDisableRackMountControls(false);
                EnableDisableEnclosureControls(false);
                ddlMountType.Enabled = false;
                chkIsEnclosure.Enabled = false;

            }
            else if (ddlModelType.SelectedItem.Text.ToLower().Contains("enclosure"))
            {
                EnableDisableEnclosureControls(true);
                chkIsEnclosure.Checked = true;
                chkIsBlade.Enabled = false;
                EnableDisableBladeControls(false);
                CompareItem(ddlMountType, "RackMount");
                ddlMountType.Enabled = false;
                EnableDisableRackControls();
                EnableDisableRackMountControls(true);
            }
            else if (ddlModelType.SelectedItem.Text.ToLower().CompareTo("rack pdu-zero u") == 0)
            {
                EnableDisableBladeControls(false);
                EnableDisableEnclosureControls(false);
                chkIsEnclosure.Checked = false;
                chkIsBlade.Enabled = false;
                EnableDisableBladeControls(false);
                CompareItem(ddlMountType, "Vertical Mount");
                ddlMountType.Enabled = false;
                EnableDisableRackControls();
                EnableDisableRackMountControls(true);
            }
            else
            {
                EnableDisableBladeControls(false);
                EnableDisableEnclosureControls(false);
                ddlMountType.Enabled = true;
                ddlMountType.SelectedIndex = 0;
                chkIsBlade.Checked = false;
                chkIsEnclosure.Checked = false;
                chkIsBlade.Enabled = false;
                chkIsEnclosure.Enabled = false;
                EnableDisableRackMountControls(true);
                EnableDisableRackControls();
            }
        }
    }



    protected void ddlMountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMountType != null && ddlMountType.Items.Count > 0)
        {
            if (ddlMountType.SelectedItem.Text.ToLower().CompareTo("rackmount") == 0 || ddlMountType.SelectedItem.Text.ToLower().CompareTo("stack in rack") == 0)
            {
                EnableDisableRackMountControls(true);
            }
            else if (ddlMountType.SelectedItem.Text.ToLower().CompareTo("vertical mount") == 0)
            {
                txtUHeight.Text = "";
                ddlCPDUSide.SelectedIndex = 0;
                ddlCDevSide.SelectedIndex = 0;
                txtTotalPSUCount.Text = "";
                txtReqPSUCount.Text = "";
                ddlAFDirection.SelectedIndex = 0;

                txtUHeight.Enabled = false;
                revUHeight.Enabled = false;
                rvUHeight.Enabled = false;
                rfvUHeight.Enabled = false;

                ddlCPDUSide.Enabled = true;
                rfvCPDUSide.Enabled = true;
                ddlCDevSide.Enabled = true;
                rfvCDevSide.Enabled = true;


                txtTotalPSUCount.Enabled = true;
                revTotalPSUCount.Enabled = true;
                rvTotalPSUCount.Enabled = true;
                rfvTotalPSUCount.Enabled = true;

                txtReqPSUCount.Enabled = true;
                revReqPSUCount.Enabled = true;
                rvReqPSUCount.Enabled = true;
                rfvReqPSUCount.Enabled = true;

                ddlAFDirection.Enabled = false;
                rfvAFDirection.Enabled = false;
            }
            else
            {
                EnableDisableRackMountControls(false);
            }
        }
    }

    protected void ddlAFDirection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAFDirection != null && ddlAFDirection.Items.Count > 0)
        {
        }
    }

    private void populateAssetTypes()
    {
        if (!ddlModelType.Visible) return;

        AssetGroupBAL dgrpBAL = new AssetGroupBAL();
        DataSet ds = dgrpBAL.retrieve();
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtAssetType = ds.Tables[0];
            DataRow dr = dtAssetType.NewRow();
            dr[0] = 0;
            dr[1] = "-Select-";
            dtAssetType.Rows.InsertAt(dr, 0);

            ddlModelType.DataSource = dtAssetType;
            ddlModelType.DataValueField = dtAssetType.Columns[0].ToString();
            ddlModelType.DataTextField = dtAssetType.Columns[1].ToString();
            ddlModelType.DataBind();
        }
    }

    private void EnableDisableRackControls()
    {
        if (ddlModelType != null && ddlModelType.SelectedIndex > 0 && ddlModelType.SelectedItem.Text.ToLower().CompareTo("rack") == 0)
        {
            txtRInternalDepth.Enabled = true;
            rvRInternalDepth.Enabled = true;
            revRInternalDepth.Enabled = true;
            rfvRInternalDepth.Enabled = true;
            cvIDepth.Enabled = true;

            txtRInternalHeight.Enabled = true;
            rvRInternalHeight.Enabled = true;
            revRInternalHeight.Enabled = true;
            rfvRInternalHeight.Enabled = true;
            cvIHeight.Enabled = true;

            txtRInternalWidth.Enabled = true;
            rvRInternalWidth.Enabled = true;
            revRInternalWidth.Enabled = true;
            rfvRInternalWidth.Enabled = true;
            cvIWidth.Enabled = true;

            txtRInternalWidth.Text = "482.6";// hardcoded as default value. 19 inch

            ddlCPDUSide.Enabled = false;
            rfvCPDUSide.Enabled = false;
            ddlCDevSide.Enabled = false;
            rfvCDevSide.Enabled = false;

            txtTotalPSUCount.Enabled = false;
            revTotalPSUCount.Enabled = false;
            rvTotalPSUCount.Enabled = false;
            rfvTotalPSUCount.Enabled = false;

            txtReqPSUCount.Enabled = false;
            revReqPSUCount.Enabled = false;
            rvReqPSUCount.Enabled = false;
            rfvReqPSUCount.Enabled = false;

            txtMaxPower.Enabled = false;
            revMaxPower.Enabled = false;
            rfvMaxPower.Enabled = false;
            rvMaxPower.Enabled = false;

            txtSSWatts.Enabled = false;
            revSSWatts.Enabled = false;
            rfvSSWatts.Enabled = false;
            rvSSWatts.Enabled = false;
            cvSPower.Enabled = false;


        }
        else
        {
            txtRInternalDepth.Enabled = false;
            rvRInternalDepth.Enabled = false;
            revRInternalDepth.Enabled = false;
            rfvRInternalDepth.Enabled = false;
            cvIDepth.Enabled = false;

            txtRInternalHeight.Enabled = false;
            rvRInternalHeight.Enabled = false;
            revRInternalHeight.Enabled = false;
            rfvRInternalHeight.Enabled = false;
            cvIHeight.Enabled = false;

            txtRInternalWidth.Enabled = false;
            rvRInternalWidth.Enabled = false;
            revRInternalWidth.Enabled = false;
            rfvRInternalWidth.Enabled = false;
            cvIWidth.Enabled = false;
            txtRInternalWidth.Text = "";

            ddlCPDUSide.Enabled = true;
            rfvCPDUSide.Enabled = true;
            ddlCDevSide.Enabled = true;
            rfvCDevSide.Enabled = true;

            txtTotalPSUCount.Enabled = true;
            revTotalPSUCount.Enabled = true;
            rvTotalPSUCount.Enabled = true;
            rfvTotalPSUCount.Enabled = true;

            txtReqPSUCount.Enabled = true;
            revReqPSUCount.Enabled = true;
            rvReqPSUCount.Enabled = true;
            rfvReqPSUCount.Enabled = true;

            txtMaxPower.Enabled = true;
            revMaxPower.Enabled = true;
            rfvMaxPower.Enabled = true;
            rvMaxPower.Enabled = true;

            txtSSWatts.Enabled = true;
            revSSWatts.Enabled = true;
            rfvSSWatts.Enabled = true;
            rvSSWatts.Enabled = true;
            cvSPower.Enabled = true;
        }
    }


    private void EnableDisableEnclosureControls(bool OnOff)
    {
        //Enable/Disable Enclosure Specific properties
        txtEnclFrontColCount.Enabled = OnOff;
        txtEnclFrontRowCount.Enabled = OnOff;
        txtEnclRearColCount.Enabled = OnOff;
        txtEnclRearRowCount.Enabled = OnOff;

        revEnclFrontColCount.Enabled = OnOff;
        revEnclFrontRowCount.Enabled = OnOff;
        revEnclRearColCount.Enabled = OnOff;
        revEnclRearRowCount.Enabled = OnOff;
        rvEnclFrontColCount.Enabled = OnOff;
        rvEnclFrontRowCount.Enabled = OnOff;
        rvEnclRearColCount.Enabled = OnOff;
        rvEnclRearRowCount.Enabled = OnOff;
        rfvEnclFrontColCount.Enabled = OnOff;
        rfvEnclFrontRowCount.Enabled = OnOff;
        rfvEnclRearColCount.Enabled = OnOff;
        rfvEnclRearRowCount.Enabled = OnOff;

        if (!OnOff)
        {
            txtEnclFrontRowCount.Text = "";
            txtEnclFrontColCount.Text = "";
            txtEnclRearRowCount.Text = "";
            txtEnclRearColCount.Text = "";
        }
    }

    private void EnableDisableBladeControls(bool OnOff)
    {
        //Enable/Disable Blade Specific properties

        if (!OnOff)
        {
            txtBladeColCount.Text = "";
            txtBladeRowCount.Text = "";
        }


        txtBladeColCount.Enabled = OnOff;
        txtBladeRowCount.Enabled = OnOff;

        revBladeColCount.Enabled = OnOff;
        revBladeRowCount.Enabled = OnOff;
        rvBladeColCount.Enabled = OnOff;
        rvBladeRowCount.Enabled = OnOff;
        rfvBladeColCount.Enabled = OnOff;
        rfvBladeRowCount.Enabled = OnOff;

    }

    private void EnableDisableRackMountControls(bool OnOff)
    {
        if (!OnOff)
        {
            txtUHeight.Text = "";
            ddlCPDUSide.SelectedIndex = 0;
            ddlCDevSide.SelectedIndex = 0;
            txtTotalPSUCount.Text = "";
            txtReqPSUCount.Text = "";
            ddlAFDirection.SelectedIndex = 0;

        }
        txtUHeight.Enabled = OnOff;
        revUHeight.Enabled = OnOff;
        rvUHeight.Enabled = OnOff;
        rfvUHeight.Enabled = OnOff;

        ddlCPDUSide.Enabled = OnOff;
        rfvCPDUSide.Enabled = OnOff;
        ddlCDevSide.Enabled = OnOff;
        rfvCDevSide.Enabled = OnOff;


        txtTotalPSUCount.Enabled = OnOff;
        revTotalPSUCount.Enabled = OnOff;
        rvTotalPSUCount.Enabled = OnOff;
        rfvTotalPSUCount.Enabled = OnOff;

        txtReqPSUCount.Enabled = OnOff;
        revReqPSUCount.Enabled = OnOff;
        rvReqPSUCount.Enabled = OnOff;
        rfvReqPSUCount.Enabled = OnOff;

        ddlAFDirection.Enabled = OnOff;
        rfvAFDirection.Enabled = OnOff;
    }

    private void populateMountTypeList()
    {
        objMountType = new iAssetTrack.BAL.MountTypeBAL();
        DataSet dsMountType = objMountType.retrieve();
        DataTable dtMountType = dsMountType.Tables[0];

        DataRow dr = dtMountType.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtMountType.Rows.InsertAt(dr, 0);

        ddlMountType.DataSource = dtMountType;
        ddlMountType.DataValueField = dtMountType.Columns[0].ToString();
        ddlMountType.DataTextField = dtMountType.Columns[1].ToString();
        ddlMountType.DataBind();
    }

    private void populateInputConnectorTypeList()
    {
        InputConnectorTypeBAL objICT = new iAssetTrack.BAL.InputConnectorTypeBAL();
        DataSet dsICT = objICT.retrieve();
        DataTable dtICT = dsICT.Tables[0];

        DataRow dr = dtICT.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtICT.Rows.InsertAt(dr, 0);

        ddlCDevSide.DataSource = dtICT;
        ddlCDevSide.DataValueField = dtICT.Columns[0].ToString();
        ddlCDevSide.DataTextField = dtICT.Columns[1].ToString();
        ddlCDevSide.DataBind();
    }

    private void populateOutputConnectorTypeList()
    {
        OutputConnectorTypeBAL objOCT = new iAssetTrack.BAL.OutputConnectorTypeBAL();
        DataSet dsOCT = objOCT.retrieve();
        DataTable dtOCT = dsOCT.Tables[0];

        DataRow dr = dtOCT.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtOCT.Rows.InsertAt(dr, 0);

        ddlCPDUSide.DataSource = dtOCT;
        ddlCPDUSide.DataValueField = dtOCT.Columns[0].ToString();
        ddlCPDUSide.DataTextField = dtOCT.Columns[1].ToString();
        ddlCPDUSide.DataBind();
    }

    private void populateAirFlowDirectionList()
    {
        objAFDirection = new iAssetTrack.BAL.AirFlowDirectionBAL();
        DataSet dsAFDirection = objAFDirection.retrieve();
        DataTable dtAFDirection = dsAFDirection.Tables[0];

        DataRow dr = dtAFDirection.NewRow();
        dr[0] = 0;
        dr[1] = "-Select-";
        dtAFDirection.Rows.InsertAt(dr, 0);

        ddlAFDirection.DataSource = dtAFDirection;
        ddlAFDirection.DataValueField = dtAFDirection.Columns[0].ToString();
        ddlAFDirection.DataTextField = dtAFDirection.Columns[1].ToString();
        ddlAFDirection.DataBind();
    }

    private void CompareItem(DropDownList ddl, string itemOrigin)
    {
        string itemToCompare = string.Empty;
        foreach (ListItem item in ddl.Items)
        {
            itemToCompare = item.Text.ToLower();
            if (itemOrigin.ToLower().CompareTo(itemToCompare) == 0)
            {
                ddl.ClearSelection();
                item.Selected = true;
            }
        }
    }

}
