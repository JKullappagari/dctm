using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Infragistics.WebUI.WebControls;

/// <summary>
/// Summary description for PageControlAccess
/// </summary>
public class PageControlAccess
{
    public PageControlAccess()
    {
    }

    public static void CheckPageControlAccess(Page pPage, String[,] saModuleRightsControlList)
    {
        Control mpContentPlaceHolder;
        mpContentPlaceHolder = pPage.Master.FindControl("Master_ContentPlaceHolder");

        String sModule = "";

        bool pageRightsFound = false;
        

        if (SiteMap.CurrentNode != null)
        {
            pPage.Session["SiteMapNode"] = SiteMap.CurrentNode;
            sModule = SiteMap.CurrentNode.Title;
            pageRightsFound = true;

        }
        else
        {
            // Rajesh : If the SiteMap Node is not found that means the user does not have any permission on the page
        }

        String sRight = "";
        int iLength = 0;

        DataTable _dtRights;

        _dtRights = (DataTable)(HttpContext.Current.Session["Rights"]);

        if (_dtRights == null)
        {
            HttpContext.Current.Session["RedirectUrl"] = SiteMap.CurrentNode.Url;
            HttpContext.Current.Response.Redirect("Login.aspx");
        }

        if (_dtRights.Select("Module = '" + sModule + "'").Length == 0)
        {
            HttpContext.Current.Response.Redirect("AccessDeniedPage.aspx");
            return;
        }

        bool hasRights = false;

        for (int i = 0; i <= saModuleRightsControlList.GetUpperBound(0); i++)
        {

            if (!sRight.Equals(saModuleRightsControlList[i, 0]))
            {
                sRight = saModuleRightsControlList[i, 0];

                //iLength = _dtRights.Select("Module = '" + sModule + "' and Rights = '" + sRight + "'").Length;

                iLength = _dtRights.Select("Module = '" + sModule + "' and " + GetRightsQueryCriteria(sRight)).Length;

                hasRights = (iLength > 0);
                hasRights = hasRights && pageRightsFound;
            }


            String sControl = saModuleRightsControlList[i, 1];

            String[] arrControlPath = sControl.Split('.');

            Control pageControl = FindControlRecursive(mpContentPlaceHolder, arrControlPath[0]);

            String props = saModuleRightsControlList[i, 2];
            bool setVisibleProp = props.Contains("Visible");
            bool setEnabledProp = props.Contains("Enabled");

            if (pageControl != null)
            {
                Type typ = pageControl.GetType();


                if (pageControl is WebControl)
                {
                    //if (pageControl is Infragistics.WebUI.UltraWebTab.UltraWebTab)
                    //{
                    //    if (arrControlPath.Length == 2)
                    //    {
                    //        Infragistics.WebUI.UltraWebTab.UltraWebTab tab = (Infragistics.WebUI.UltraWebTab.UltraWebTab)pageControl;
                    //        if (setVisibleProp) tab.Tabs.FromKey(arrControlPath[1]).Visible = tab.Tabs.FromKey(arrControlPath[1]).Visible & hasRights;                           
                    //    }
                    //}
                    //else
                    //{

                    //    WebControl webControl = (WebControl)pageControl;
                    //    if (setVisibleProp) webControl.Visible = webControl.Visible & hasRights;
                    //    if (setEnabledProp) webControl.Enabled = webControl.Enabled & hasRights;
                    //}
                    //J00006 by kjb on 01 Oct 2012
                }
                else if (pageControl is HtmlControl)
                {
                    HtmlControl htmlControl = (HtmlControl)pageControl;
                    if (setVisibleProp) htmlControl.Visible = htmlControl.Visible & hasRights;
                    if (setEnabledProp) htmlControl.Disabled = htmlControl.Disabled & (!hasRights);
                } 
                else if (typ.BaseType.FullName.StartsWith("Infragistics"))
                {
                    WebControlBase infragisticsControl = (WebControlBase)pageControl;
                    if (setVisibleProp) infragisticsControl.Visible = infragisticsControl.Visible & hasRights;
                    if (setEnabledProp) infragisticsControl.Enabled = infragisticsControl.Enabled & hasRights;
                }
            }
        }
    }


    private static String GetRightsQueryCriteria(String pRight)
    {
        if (pRight.Contains(","))
        {
            String[] arrRights = pRight.Split(',');
            String strQuery = "Rights IN ('" + String.Join("','", arrRights) + "')";
            return strQuery;
        }
        else { return "Rights = '" + pRight.Trim() + "'"; }
    }

    public static Control FindControlRecursive(Control Root, string Id)
    {
        if (Root.ID == Id) return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindControlRecursive(Ctl, Id);
            if (FoundCtl != null) return FoundCtl;

        }
        return null;
    }



    public static bool IsAuthorized(Page pPage, String pRight)
    {
        String sModule = "";

        DataTable _dtRights = (DataTable)(HttpContext.Current.Session["Rights"]);

        if (SiteMap.CurrentNode != null)
        {
            pPage.Session["SiteMapNode"] = SiteMap.CurrentNode;
            sModule = SiteMap.CurrentNode.Title;

        }
        else
        {
            // Rajesh : If the SiteMap Node is not found that means the user does not have any permission on the page
            //SiteMapNode prevselection = (SiteMapNode)pPage.Session["SiteMapNode"];
            //sModule = prevselection.Title;
        }


        
        int iLength = _dtRights.Select("Module = '" + sModule + "' and Rights = '" + pRight + "'").Length;

        return (iLength > 0);
        

    }
}