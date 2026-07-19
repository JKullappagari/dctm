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
using System.ComponentModel;
using System.Drawing.Design;
using iAssetTrack.DALC;
using System.Drawing;
using iAssetTrack.BAL;
using iAssetTrackBAL;
using Infragistics.Web.UI.NavigationControls;
using Infragistics.Web.UI.GridControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.WebUI.WebDataInput;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WebForms;
using Infragistics.Web.UI.EditorControls;

public partial class AssetDataExcel : System.Web.UI.Page
{
    DataTable _dtRights;
    protected string srcSite;
    protected string dstSite;

    // J00007 by kjb on 25 Sep 2012
    # region new control declaration
    protected Label lblErrorMessage;
    protected CheckBoxList chkColumns;
    protected WebImageButton ibUp, ibDown;
    protected CheckBox chkSelectAll;
    # endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        // J00007 by kjb on 25 Sep 2012
        # region New controls initialization
        lblErrorMessage = (Label)this.wpSearchOptions.Groups[0].Items[0].FindControl("lblErrorMessage");
        chkColumns = (CheckBoxList)this.wpSearchOptions.Groups[0].Items[0].FindControl("chkColumns");
        ibUp = (WebImageButton)this.wpSearchOptions.Groups[0].Items[0].FindControl("ibUp");
        ibDown = (WebImageButton)this.wpSearchOptions.Groups[0].Items[0].FindControl("ibDown");
        chkSelectAll = (CheckBox)this.wpSearchOptions.Groups[0].Items[0].FindControl("chkSelectAll");

        # endregion

        Session["PageHeader"] = "Asset Data - Excel";
        _dtRights = (DataTable)(Session["Rights"]);

        if (_dtRights == null)
        {
            Session["RedirectUrl"] = "AssetDataExcel.aspx";
            Response.Redirect("Login.aspx");
        }

        bool blfoundPage = false;

        if (_dtRights.Select("Module = 'Asset Data - Excel' and Rights = '" + "View" + "'").Length != 0)
        {
            blfoundPage = true;
        }

        if (blfoundPage == false)
        {
            Response.Redirect("AccessDeniedPage.aspx");
            return;
        }
        if (!IsPostBack)
        {

        }
    }


    protected string CheckedColumns()
    {
        string checkedColumns = string.Empty;

        foreach (ListItem item in chkColumns.Items)
        {
            if (item.Selected)
            {
                checkedColumns = checkedColumns + item.Value + ",";
            }
        }


        return checkedColumns.Trim(',');

    }



    protected void btnShowReport_Click(object sender, EventArgs e)
    {

        ReportParameter[] parameters = new ReportParameter[2];

        //parameters
        try
        {
            lblErrorMessage.Visible = false;
            lblErrorMessage.Enabled = false;
            lblErrorMessage.Text = string.Empty;


            parameters[0] = new ReportParameter("columnList", CheckedColumns());
            parameters[1] = new ReportParameter("pIntLoggedInUserId",Session["UserID"].ToString());


            ReportViewer1.ShowParameterPrompts = false;

            string user = ConfigurationManager.AppSettings["ReportServerUser"];

            string pass = ConfigurationManager.AppSettings["ReportServerPass"];

            string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

            ReportViewer1.ServerReport.ReportServerCredentials = new ReportCredentials(user, pass, domain);

            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            ReportViewer1.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportServerUrl"]);
            ReportViewer1.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportServerFolder"] + "AssetDataExcel";
            ReportViewer1.ServerReport.SetParameters(parameters);
            ReportViewer1.ServerReport.Refresh();
            //ReportViewer1.Visible = true;
            //wpSearchOptions.Expanded = false;// J00007 by kjb on 25 Sep 2012
            wpSearchOptions.Groups[0].Expanded = false;

            //downoload as excel
            string mimeType, encoding, extension, deviceInfo;
            string[] streamids;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string format = "Excel"; //Desired format goes here (PDF, Excel, or Image)
            deviceInfo = "<DeviceInfo>" + "<SimplePageHeaders>True</SimplePageHeaders>" + "</DeviceInfo>";

            byte[] bytes = ReportViewer1.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            Response.Clear();
            if (format == "PDF")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-disposition", "filename=AssetDataExcel.pdf");
            }

            else if (format == "Excel")
            {
                Response.ContentType = "application/excel";
                Response.AddHeader("Content-disposition", "filename=AssetDataExcel.xls");
            }

            Response.OutputStream.Write(bytes, 0, bytes.Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.Flush();
            Response.Close();
        }
        catch (Exception ex)
        {
            ExceptionPolicy.HandleException(ex, "errDCTrack");
            lblErrorMessage.Visible = true;
            lblErrorMessage.Enabled = true;
            lblErrorMessage.Text = "Error in processing report parameters";
        }
        finally
        {
            if (parameters != null)
                parameters = null;
        }



    }

    protected void ibUp_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        // only if the first item isn't the current one
        if (chkColumns.SelectedIndex > 0)
        {
            // add a duplicate item up in the listbox
            chkColumns.Items.Insert(chkColumns.SelectedIndex - 1, chkColumns.Items[chkColumns.SelectedIndex].Text);
            // make it the current item
            chkColumns.SelectedIndex = (chkColumns.SelectedIndex - 2);
            // delete the old occurrence of this item
            chkColumns.Items.RemoveAt(chkColumns.SelectedIndex + 2);
        }
    }

    protected void ibDown_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        // only if the last item isn't the current one
        if ((chkColumns.SelectedIndex != -1) && (chkColumns.SelectedIndex < chkColumns.Items.Count - 1))
        {
            // add a duplicate item down in the listbox
            chkColumns.Items.Insert(chkColumns.SelectedIndex + 2, chkColumns.Items[chkColumns.SelectedIndex].Text);
            // make it the current item
            chkColumns.SelectedIndex = chkColumns.SelectedIndex + 2;
            // delete the old occurrence of this item
            chkColumns.Items.RemoveAt(chkColumns.SelectedIndex - 2);
        }
    }

    protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs args)
    {
        bool checkUncheck = chkSelectAll.Checked;
        foreach (ListItem item in chkColumns.Items)
        {
            item.Selected = checkUncheck;
        }
    }

    protected void chkColumns_OnSelectedIndexChanged(object sender, EventArgs args)
    {
        var selected = chkColumns.Items.Cast<ListItem>().Where(x => x.Selected);

        if (chkColumns.Items.Count == selected.Count())
        {
            chkSelectAll.Checked = true;
        }
        else
        {
            chkSelectAll.Checked = false;
        }
    }


}
