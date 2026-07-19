/*
File Name   :	GroupModuleRights.aspx.cs

Description :	create User Group

Date created:	01 Oct 2006

Modification History:
***********************
CR		Name			Date			Description
New		srinivas	01/10/2006		File has been created.
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

public partial class ASPX_GrpModuleRights : System.Web.UI.Page
{
    #region "Declarations"
    private iAssetTrack.BAL.GroupBAL objGroup;
    private iAssetTrack.BAL.GroupModuleRightBAL objGroupModuleRight;
    private DataTable dtRight;
    private DataTable dtGroupRight;
    private iAssetTrack.BAL.CommonBAL objCommon;
    //private SessionUser sessionUser;
    private string sStr = string.Empty;
    DataTable _dtRights;
    private const string PROP_NODES_EXPANDED = "NodesExpanded";
    private string tenantAssignedGroups = string.Empty;
    #endregion

    public bool NodesExpanded
    {
        get
        {
            return (ViewState[PROP_NODES_EXPANDED] != null ? Convert.ToBoolean(ViewState[PROP_NODES_EXPANDED].ToString()) : false);
        }
        set
        {
            ViewState[PROP_NODES_EXPANDED] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        this.ddlGroup.Focus();

        DataSet dsGroup;


        Session["PageHeader"] = "Group Module Access Rights Assignment";
        Session["PageUser"] = System.Configuration.ConfigurationManager.AppSettings["LoginUser"];
        Session["PageTime"] = System.Configuration.ConfigurationManager.AppSettings["LoginTime"];
        if (Session["Message"] != null)
        {
            lblMessage.Visible = true;
            lblMessage.Text = Session["Message"].ToString();
        }
        else

            lblMessage.Visible = false;
        Session["Message"] = "";

        _dtRights = (DataTable)(Session["Rights"]);

        bool blfoundPage = false;

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "GroupModuleRightsAssignment.aspx";
            Response.Redirect("Login.aspx");
        }

        if (_dtRights.Select("Module = 'Group Module Access Rights Assignment' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        //if (_dtRights.Select("Module = '" + Page.Title + "' and Rights = '" + "Create" + "'").Length != 0)
        //{
        //    ibCreate.Enabled = true;
        //}
        //else
        //{
        //    ibCreate.Enabled = false;
        //}

        //if (_dtRights.Select("Module = '" + Page.Title + "' and Rights = '" + "Delete" + "'").Length != 0)
        //{
        //    //dltModuleRight.Enabled = true;
        //    wdtGrpModuleRights.Enabled = true;
        //}
        //else
        //{
        //    //dltModuleRight.Enabled = false;
        //    wdtGrpModuleRights.Enabled = false;
        //}

        if (_dtRights.Select("Module = '" + Page.Title + "' and Rights = '" + "Assign Rights" + "'").Length != 0)
        {
            //dltModuleRight.Enabled = true;
            wdtGrpModuleRights.Enabled = true;
            ibCreate.Enabled = true;
        }
        else
        {
            //dltModuleRight.Enabled = false;
            wdtGrpModuleRights.Enabled = false;
            ibCreate.Enabled = false;
        }

        if (!IsPostBack)
        {
            try
            {
                this.NodesExpanded = true;

                if (bool.Parse(Session["TenantUser"].ToString()))
                {
                    UserBAL objUser = new UserBAL();
                    objUser.UserID = Convert.ToInt32(Session["UserID"]);
                    DataSet dsTenant = objUser.retrieveTenantDetails();
                    if (dsTenant.Tables.Count > 0 && dsTenant.Tables[0].Rows.Count > 0)
                    {
                        tenantAssignedGroups = dsTenant.Tables[0].Rows[0][DBFields.DBFIELD_TENANT_ASSIGNEDGROUPS].ToString();
                    }

                    objGroup = new iAssetTrack.BAL.GroupBAL();
                    dsGroup = objGroup.retrieve();
                    DataTable dtGroup = dsGroup.Tables[0];

                    DataTable tempTable = dtGroup.Clone();
                    foreach (DataRow dr in dtGroup.Rows)
                    {
                        tempTable.Rows.Add(dr.ItemArray);
                    }
                    DataRow[] filteredRows = tempTable.Select("GroupID IN (" + tenantAssignedGroups + ")");
                    dtGroup.Rows.Clear();
                    if (filteredRows != null && filteredRows.Length > 0)
                    {
                        foreach (DataRow dr in filteredRows)
                        {
                            dtGroup.Rows.Add(dr.ItemArray);
                        }

                    }

                    objCommon = new CommonBAL();
                    objCommon.setDataSource(ddlGroup, dtGroup, "-Select-");

                }
                else
                {
                    objGroup = new iAssetTrack.BAL.GroupBAL();
                    dsGroup = objGroup.retrieve();
                    DataTable dtGroup = dsGroup.Tables[0];
                    objCommon = new CommonBAL();
                    objCommon.setDataSource(ddlGroup, dtGroup, "-Select-");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "errDCTrack");
                throw ex;
            }
        }
    }

    /// <summary>
    /// assign module rights to group
    /// </summary>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        DataRow[] drSecRightModule;
        objGroupModuleRight = new iAssetTrack.BAL.GroupModuleRightBAL();
        objGroupModuleRight.GroupID = Convert.ToInt32(ddlGroup.SelectedValue);
        DataTable dtSecRightModule = ((DataTable)(ViewState["Module_Right"]));
        try
        {
            int intMainModuleCount = wdtGrpModuleRights.Nodes.Count;
            for (int i = 0; i < intMainModuleCount; i++)
            {
                int intModuleCount = wdtGrpModuleRights.Nodes[i].Nodes.Count;
                for (int j = 0; j < intModuleCount; j++)
                {
                    drSecRightModule = dtSecRightModule.Select("ModuleID = " + wdtGrpModuleRights.Nodes[i].Nodes[j].Key.ToString());
                    objGroupModuleRight.ModuleID = Convert.ToInt32(wdtGrpModuleRights.Nodes[i].Nodes[j].Key);
                    objGroupModuleRight.Delimiters = ";";
                    string strRightsIds = "";
                    int intRightsCount = wdtGrpModuleRights.Nodes[i].Nodes[j].Nodes.Count;
                    for (int k = 0; k < intRightsCount; k++)
                    {
                        if (wdtGrpModuleRights.Nodes[i].Nodes[j].Nodes[k].CheckState == Infragistics.Web.UI.CheckBoxState.Checked)
                        {
                            strRightsIds += wdtGrpModuleRights.Nodes[i].Nodes[j].Nodes[k].Key.ToString() + ";";
                        }
                    }
                    objGroupModuleRight.RightsIDs = strRightsIds;
                    objGroupModuleRight.Status = 1;
                    objGroupModuleRight.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    objGroupModuleRight.Persist(DALCOperation.Insert);
                }
            }

            App_Code.CSqlSiteMapProvider.RefreshSiteMap();

            objCommon = new CommonBAL();
            Session["Message"] = objCommon.displayMessage(MessageCodes.GEN_S_ASSIGNED);
            Response.Redirect("GroupModuleRightsAssignment.aspx");
            lblMessage.Visible = true;
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
            //lblMessage.Text = ex.Message;
            //lblMessage.Visible = true;
            //lblMessage.ForeColor =  System.Drawing.Color.Red;
        }


    }
    /// <summary>
    /// build data to grid
    /// </summary>
    private void doBuild()
    {
        wdtGrpModuleRights.Nodes.Clear();
        try
        {
            iAssetTrack.BAL.GroupModuleRightBAL objGroupModuleRight = new iAssetTrack.BAL.GroupModuleRightBAL();

            objGroupModuleRight.GroupModuleRightsID = 1;
            DataSet dsGroupModuleRight = objGroupModuleRight.retrieve();
            # region commented old code
            //dsGroupModuleRight.Relations.Add("Rel_MainModule", dsGroupModuleRight.Tables[2].Columns["MainModuleID"], dsGroupModuleRight.Tables[0].Columns["MainModuleID"]);
            //dsGroupModuleRight.Relations.Add("Rel_Module", dsGroupModuleRight.Tables[0].Columns["ModuleID"], dsGroupModuleRight.Tables[1].Columns["ModuleID"]);
            //this.wdtGrpModuleRights.DataSource = dsGroupModuleRight.Tables[2].DefaultView;
            //this.wdtGrpModuleRights.Levels[0].RelationName = "Rel_MainModule";
            //this.wdtGrpModuleRights.Levels[0].ColumnName = "MainModule";
            //this.wdtGrpModuleRights.Levels[1].RelationName = "Rel_Module";
            //this.wdtGrpModuleRights.Levels[1].ColumnName = "Module";
            //this.wdtGrpModuleRights.Levels[2].ColumnName = "Description";
            //this.wdtGrpModuleRights.Levels[0].LevelKeyField = "MainModuleID";
            //this.wdtGrpModuleRights.Levels[1].LevelKeyField = "ModuleID";
            //this.wdtGrpModuleRights.Levels[2].LevelKeyField = "RightsID";
            //this.wdtGrpModuleRights.DataMember = "tblMainModule";
            //this.wdtGrpModuleRights.DataBind();
            # endregion

            foreach (DataRow drMain in dsGroupModuleRight.Tables[2].Rows)
            {
                DataTreeNode parentNode = new DataTreeNode();
                parentNode.ImageUrl = "./images/ig_treeNews.gif";
                parentNode.Text = drMain["MainModule"].ToString();
                parentNode.Key = drMain["MainModuleID"].ToString();
                wdtGrpModuleRights.Nodes.Add(parentNode);
                foreach (DataRow drModule in dsGroupModuleRight.Tables[0].Rows)
                {
                    if (parentNode.Key.CompareTo(drModule["MainModuleID"].ToString()) == 0)
                    {
                        DataTreeNode moduleNode = new DataTreeNode();
                        moduleNode.ImageUrl = "./images/ig_treeNews.gif";
                        moduleNode.Text = drModule["Module"].ToString();
                        moduleNode.Key = drModule["ModuleID"].ToString();
                        parentNode.Nodes.Add(moduleNode);

                        foreach (DataRow drRights in dsGroupModuleRight.Tables[1].Rows)
                        {
                            if (moduleNode.Key.CompareTo(drRights["ModuleID"].ToString()) == 0)
                            {
                                DataTreeNode rightsNode = new DataTreeNode();
                                rightsNode.ImageUrl = "./images/ig_treeNews.gif";
                                rightsNode.Text = drRights["Description"].ToString();
                                rightsNode.Key = drRights["RightsID"].ToString();
                                moduleNode.Nodes.Add(rightsNode);
                            }
                        }

                    }
                }
            }
            lblMessage.Visible = true;
            lblMessage.Text = wdtGrpModuleRights.Nodes.Count.ToString() + "dfsdfsdf";

            DataTable dtGroupModuleRight = dsGroupModuleRight.Tables[0];
            dtRight = dsGroupModuleRight.Tables[1];
            ViewState["Module_Right"] = dtGroupModuleRight;

        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
            //lblMessage.Text = ex.Message;
            //lblMessage.Visible = true;
        }

        finally
        {

        }
    }
    /// <summary>
    /// edit rights
    /// </summary>
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            objGroup = new GroupBAL();
            if (ddlGroup.SelectedIndex == 0)
            {
                clearFields();
            }
            else
            {
                objGroup.GroupID = Convert.ToInt32(ddlGroup.SelectedValue.ToString());
                DataSet dsGroup = objGroup.retrieve();
                DataTable dtGroup = dsGroup.Tables[0];
                dtGroupRight = dsGroup.Tables[1];
                doBuild();
                checkRights(dtGroupRight);
                lblMessage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            throw ex;
            //lblMessage.Text = ex.Message; 
        }
    }
    /// <summary>
    /// check rights
    /// </summary>
    private void checkRights(DataTable dtGroupRight)
    {
        DataTable dtSecRightModule = ((DataTable)(ViewState["Module_Right"]));
        DataRow[] drSecRightModule;

        int intMainModuleCount = wdtGrpModuleRights.Nodes.Count;
        for (int i = 0; i < intMainModuleCount; i++)
        {
            int intModuleCount = wdtGrpModuleRights.Nodes[i].Nodes.Count;
            for (int j = 0; j < intModuleCount; j++)
            {
                drSecRightModule = dtSecRightModule.Select("ModuleID = " + wdtGrpModuleRights.Nodes[i].Nodes[j].Key.ToString());
                bool bolFlag = false;
                int intTemp = 0;
                for (int Temp = 0; Temp <= dtGroupRight.Rows.Count - 1; Temp++)
                {
                    if (dtGroupRight.Rows[Temp]["ModuleID"].ToString() == wdtGrpModuleRights.Nodes[i].Nodes[j].Key.ToString())
                    {
                        bolFlag = true;
                        intTemp = Temp;
                    }
                    if (bolFlag == true)
                    {
                        int intRightsCount = wdtGrpModuleRights.Nodes[i].Nodes[j].Nodes.Count;
                        for (int k = 0; k < intRightsCount; k++)
                        {
                            if (wdtGrpModuleRights.Nodes[i].Nodes[j].Nodes[k].Key.ToString() == dtGroupRight.Rows[intTemp]["RightsID"].ToString())
                            {
                                wdtGrpModuleRights.Nodes[i].Nodes[j].Nodes[k].CheckState = Infragistics.Web.UI.CheckBoxState.Checked;
                            }

                        }
                    }
                }

            }
        }
    }
    /// <summary>
    /// clear fields
    /// </summary>
    private void clearFields()
    {

        lblMessage.Visible = false;
        lblMessage.Text = "";
        ddlGroup.SelectedIndex = 0;
        wdtGrpModuleRights.Nodes.Clear();
    }
    /// <summary>
    /// reset fileds
    /// </summary>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        lblMessage.Visible = false;
        lblMessage.Text = "";
        ddlGroup.SelectedIndex = 0;
        wdtGrpModuleRights.Nodes.Clear();
    }
    protected void wibExpandClose_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        foreach (DataTreeNode node in wdtGrpModuleRights.AllNodes)
        {
            //if (node.ParentNode == null)
            //{
            //    DataTreeNode parent = node;
            //    if (parent.Expanded)
            //        parent.Expanded = false;
            //    else
            //        parent.Expanded = true;

            //    foreach (DataTreeNode nodeC in wdtGrpModuleRights.AllNodes)
            //    {
            //        if (nodeC.ParentNode != null && nodeC.ParentNode == parent)
            //        {
            //            nodeC.Expanded = parent.Expanded;
            //        }
            //    }
            //}
            node.Expanded = this.NodesExpanded;
        }
        this.NodesExpanded = !this.NodesExpanded;
    }
}
