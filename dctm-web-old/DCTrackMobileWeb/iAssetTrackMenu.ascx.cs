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
//using iDocTrack.Remoting.Shared;

public partial class Controls_iAssetTrackMenu : System.Web.UI.UserControl
{
    #region "Declarations"

    //private iAssetTrack.BAL.CommonBAL objCommon;
    //private iAssetTrack.BAL.UserBAL objUser;
    //Hashtable hsmenu;
#endregion
    
    #region "security methods"
    const string accessDenied="~/ASPX/AccessDeniedPage.aspx";
    const string centraltositeurl="~/ASPX/CentralToSite.aspx";
    string[] centralRedirect={"Manual Clock In/Clock Out","Non Resident Worker Entry","Non Resident Worker Entry","Gantry Offline Attendance Reconciliation", "View Worker Attendance", "View Worker 2428 Hours Attendance"};

    string[] centralAccessDenied ={"Non Resident Worker Exit","Attendance Reconciliation", "Create Requisition", "Requisition View", "Assign Workers to Borrow Requisition", "Return Borrowed Workers"};

    string[] siteRedirect ={ "Worker Preallocation", "View SSIC List", "View Work Order Settlement Amount List", "Subcontractor TLWF and TFWF Monthly Count", "Create Indemnity", "Maintenance", "Masters", "SubContractor", "Assignment", "Scheme", "Block", "Create/Search JobCard", "Create Job Card Without Work Order", "Search Worker", "Create Worker", "Blanket Insurance", "Search Subcontractor", "Create Subcontractor", "Link Subcontractors", "Indemnity View", "Requisition View", "Approve Borrowed Workers", "Group", "Group Module Access Rights Assignment", "Manage Users", "User Search", "View Linked Subcontractors" };
    string[] siteAccessDenied ={ "Worker Preallocation", "View Project List", "View Cost Center List", "View Project Subcode List", "View Subcontractor List", "View Trade List", "View Work Order Detail List", "View Work Order Header List", "View Work Order Invoice Amount List", "View Work Order Settlement Amount List", "Create Indemnity", "Approve Requisition View", "Job Card Submission", "Attendance Reconciliation", "Create Requisition", "Requisition View", "Assign Workers to Borrow Requisition", "Return Borrowed Workers", "Associate Job Card to Work Order", "Download Worker Data", "Upload Worker Data", "Site Specific RFID Card Upload", "ViewWorkPermitDetails.aspx", "ViewSSIC.aspx" };
    string[] externalAccessDenied ={ "View SSIC List", "View Work Order Settlement Amount List", "Subcontractor TLWF and TFWF Monthly Count", "Associate Job Card to Work Order", "View Project List", "View Cost Center List", "View Project Subcode List", "View Subcontractor List", "View Trade List", "View Work Order Detail List", "View Work Order Header List", "View Work Order Invoice Amount List", "View Work Order Settlement Amount List", "Maintenance", "Masters", "SubContractor", "Assignment", "Scheme", "Block", "Create/Search JobCard", "Create Job Card Without Work Order", "Manual Clock In/Clock Out", "Non Resident Worker Entry", "Non Resident Worker Exit", "Gantry Offline Attendance Reconciliation", "Blanket Insurance", "Link Subcontractors", "Approve Requisition View", "Group", "Group Module Access Rights Assignment", "Manage Users", "User Search", "Approve Borrowed Workers", "Download Worker Data", "Upload Worker Data", "Subcontractor Category", "Site Specific RFID Card Upload", "View Linked Subcontractors", "View Worker Attendance", "View Worker 2428 Hours Attendance" };
    //"Create Indemnity","Indemnity View"
    private bool ExternalAccessDenied(string menu)
    {
        foreach(string search in externalAccessDenied)
        {
            if(search==menu)
            {
                return true;
            }
        }
        return false;

    }

    private char CentralAccessDenied(string menu)
    {
        char type='N';
        foreach(string search in centralAccessDenied)
        {
            if(search==menu)
            {
                type='A';
                return type;
            }
        }

        foreach(string redirect in centralRedirect)
        {
                if(redirect==menu)
                {
                    type='R';
                    return type;
                }
           

        }
        return type;
        
    }

    private char SiteAccessDenied(string menu)
    {
        char type='N';
        foreach(string search in siteAccessDenied)
        {
            if(search==menu)
            {
                type='A';
                return type;
            }
        }

        foreach(string redirect in siteRedirect)
        {
                if(redirect==menu)
                {
                    type='R';
                    return type;
                }
           

        }
        return type;
        
    }

    //private void AssignMenu()
    //{           
    //    hsmenu=new Hashtable();
    //    hsmenu.Add("Maintenance","~/ASPX/Maintenance.aspx");
    //    //jobcard
    //    hsmenu.Add("Job Card Search","~/aspx/JobcardSearch.aspx");
    //    hsmenu.Add("Create Job Card Without Work Order","");
    //    hsmenu.Add("Job Card Submission","~/aspx/JobCardSubmission_New.aspx");
    //    hsmenu.Add("Assign Work Order to Job Card","~/aspx/WorkOrderSearch.aspx");
        

    //    //worker
    //    hsmenu.Add("Search Worker","~/ASPX/WorkerSearch.aspx");
    //    hsmenu.Add("Create Worker","~/ASPX/WorkerCreation.aspx");
    //    hsmenu.Add("Worker Pre-allocation","~/aspx/WorkerPreAlloc1.aspx");
    //    hsmenu.Add("Manual Clock In/Clock Out","~/ASPX/ManualClockInClockOut.aspx");
    //    hsmenu.Add("Non Resident Worker Entry","~/ASPX/NonResidentWorkerEntry.aspx");
    //    hsmenu.Add("Non Resident Worker Exit","~/ASPX/NonResidentWorkerExit.aspx");
    //    hsmenu.Add("Gantry Offline Attendance Reconciliation","~/ASPX/GantryOfflineAttendanceReconciliation.aspx");
        
    //    //subcontractor
    //    hsmenu.Add("Blanket Insurance","~/ASPX/BlanketInsurance.aspx");
    //    hsmenu.Add("Search Subcontractor","~/ASPX/SubcontractorSearch.aspx");
    //    hsmenu.Add("Create Subcontractor","~/ASPX/SubcontractorCreation.aspx");
    //    hsmenu.Add("Link Subcontractors","~/ASPX/SubcontractorLink.aspx");
        
    //    //Borrowing
    //    hsmenu.Add("Create Indemnity","~/aspx/IndemnityDetails.aspx");
    //    hsmenu.Add("Indemnity View","~/aspx/IndemnityView.aspx");
    //    hsmenu.Add("Create Requisition","~/aspx/BorrowingRequisition.aspx");
    //    hsmenu.Add("Requisition View","~/aspx/BorrowingRequistionView.aspx");
    //    hsmenu.Add("Approve Requisition View","~/aspx/BorrowingRequistionApproveView.aspx");
    //    hsmenu.Add("Assign Workers To Borrowing Requistion","~/aspx/AssignWorkersToBorrowingRequistion.aspx");
    //    hsmenu.Add("Approve  Borrowing Workers","~/aspx/ApproveBorrowingAssignWorkersList.aspx");
    //    hsmenu.Add("Return Borrowing Workers","~/aspx/ReturnBorrowingWorkersList.aspx");
    //    //User Administration
    //    hsmenu.Add("Group","Group.aspx");
    //    hsmenu.Add("Group Module Access Rights Assignment","GroupModuleRights.aspx");
    //    hsmenu.Add("Manage Users","ManageUsers.aspx");
    //    hsmenu.Add("User Search","UserSearch.aspx");
    //    hsmenu.Add("Reconciliation","~/ASPX/Reconciliation.aspx");
     

    //}
    
       
    #endregion

    #region " Page Event Methods "

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    //HP: Do not display menu if user password had expired. Search for showMenu string in this page to view 
    //    //HP: where value is set. This is applicable only for external users.
    //    bool showMenu = true;
        
    //    //if (null != Session["UserType"])
    //    //    if (Session["UserType"].Equals("E"))
    //    //        if ((bool)Session["pwdExpired"]) showMenu = false;
       

    //    string mode=ConfigurationManager.AppSettings["Mode"];
    //  //  string path=ConfigurationManager.AppSettings["CentralWebAppRootPathUrl"];
        
    //    DataTable _dtRights = (DataTable)(Session["Rights"]);
    //    if (_dtRights == null) Response.Redirect("Login.aspx");
    //    foreach (Item mnuBar in uwMenu.Items)
    //    {
    //        int menuCount=0;
    //        bool foundRows = false;
    //        if (_dtRights.Select("Module = '" + mnuBar.Text + "'").Length != 0)
    //        {
    //            foundRows = true;
    //        }
    //       foreach (Item mnuItem in mnuBar.Items)
    //        {
    //           // mnuItem.TargetUrl="";
    //            if (_dtRights.Select("Module = '" + mnuItem.Text + "'").Length != 0)
    //            {
    //                foundRows = true;
    //                mnuItem.Hidden = false;
    //                mnuItem.Enabled = showMenu;
    //            }
    //            else
    //                mnuItem.Hidden = true;

    //            if (mnuItem.Text == "Create Subcontractor")
    //            {
    //                if (_dtRights.Select("Module = 'Create Subcontractor' AND Rights = 'Create'").Length != 0)
    //                {
    //                    foundRows = true;
    //                    mnuItem.Hidden = false;
    //                    mnuItem.Enabled = showMenu;
    //                }
    //                else
    //                    mnuItem.Hidden = true;
    //            }
                
    //            foreach (Item subItem in mnuItem.Items)
    //            {
    //                if (_dtRights.Select("Module = '" + subItem.Text + "'").Length != 0)
    //                {
    //                    foundRows = true;
    //                    subItem.Hidden = false;
    //                    subItem.Enabled = showMenu;
    //                }
    //                else
    //                    subItem.Hidden = true;        
    //            }
                    
              
    //            if(Convert.ToInt32(mode)==3)
    //            {
    //                if(ExternalAccessDenied(mnuItem.Text))
    //                {
    //                    mnuItem.Hidden = true;
    //                     menuCount=menuCount+1;
    //                }
    //            }

    //            if(Convert.ToInt32(mode)==1)
    //            {
    //              char type=CentralAccessDenied(mnuItem.Text); 
    //              if(type=='A')
    //              {
    //                  mnuItem.Hidden = true;
    //                   menuCount=menuCount+1;
    //              }
    //              if(type=='R')
    //              {
    //                //string targeturl= mnuItem.TargetUrl;
    //            //   Session["CentralToSiteUrl"]=mnuItem.TargetUrl;
    //               mnuItem.Hidden=true;
    //               menuCount=menuCount+1;

    //              }

    //            }

    //            if(Convert.ToInt32(mode)==2)
    //            {
    //                char type=SiteAccessDenied(mnuItem.Text);
    //                if(type=='A')
    //                {
    //                    mnuItem.Hidden = true;
    //                     menuCount=menuCount+1;

    //                }

    //                if(type=='R')
    //                {
    //                    mnuItem.Hidden=true;
    //                     menuCount=menuCount+1;
    //                }
    //            }

    //        }
            

    //        if (foundRows == true)
    //        {
    //            mnuBar.Hidden = false;
    //            mnuBar.Enabled = showMenu;
    //        }
    //        else
    //        {
    //            mnuBar.Hidden = true;
    //        }
    //        if (mnuBar.Text == "Home" || mnuBar.Text == "Logout")
    //        {
    //            mnuBar.Hidden = false;
    //        } 


    //         if(Convert.ToInt32(mode)==2)
    //         {
    //             if(mnuBar.Text == "Reports")
    //             {
    //                    mnuBar.Hidden = true;
    //             }
    //         }

    //         if (Convert.ToInt32(mode) == 2)
    //         {
    //             if (mnuBar.Text == "Subcontractor")
    //             {
    //                 mnuBar.Hidden = true;
    //             }
    //         }
            
            
            
    //        //if (mnuBar.Text == "Logout")
    //        //{
    //        //    if (Session["UserType"].ToString().Contains("I"))
    //        //    {
    //        //        mnuBar.Hidden = true;
    //        //    }
    //        //}
            


    //        if ((mnuBar.Text != "Home") && (mnuBar.Text != "Logout") && (mnuBar.Text != "Reports") && (mnuBar.Text != "Archival"))
    //         {
    //                if(mnuBar.Items.Count==menuCount)
    //                {
    //                mnuBar.Hidden=true;

    //                }
    //         }

    //        if(mnuBar.Text == "Job Card")
    //        {
    //            if(Convert.ToInt32(mode)==2)
    //            {
    //                mnuBar.Hidden=true;
    //            }
             
    //        }

    //        if(mnuBar.Text =="View")
    //        {
    //            if(Convert.ToInt32(mode)==2)
    //            {
    //                mnuBar.Hidden=true;
    //            }
          
    //        }

    //        if (mnuBar.Text == "Maris")
    //        {
    //            if (Convert.ToInt32(mode) == 2)
    //            {
    //                mnuBar.Hidden = true;
    //            }

    //        }
    //     }
       
        
    //    if (!IsPostBack)
    //    {
    //        this.uwMenu.TopSelectedStyle.CssClass = "SelectedStyle";
    //        this.setSelected();
    //    }
    //}

    /* commneted by Arif
    private void MaintainSite(int mode,string module)
    {
        if(mode==1)
        {
             if(module=="Maintenance")
             {

             }
        }
        if(mode==2)
        {
             if(module=="Maintenance")
             {
                Response.Redirect("a.aspx");

             }


        }
        if(mode==3)
        {

        }

    }
    Commented By Arif */

    //protected void uwMenu_MenuItemClicked(object sender, Infragistics.WebUI.UltraWebNavigator.WebMenuItemEventArgs e)
    //{
             
    //    if (e.Item.Text.Equals("Create Job Card Without Work Order"))
    //    {
    //        Session["WoNumber"] = null;
    //        Session["JobCardID"] = null;
    //        Response.Redirect("JobcardCreation.aspx");
    //    }
    //    if (e.Item.Text.Equals("Search Subcontractor"))
    //    {
    //        Session["SubcontractorSearchCriteria"] = null;
    //        Response.Redirect("SubcontractorSearch.aspx");
    //    }
    //    if (e.Item.Text.Equals("Search Worker"))
    //    {
    //        Session["WorkerSearchCriteria"] = null;
    //        Response.Redirect("WorkerSearch.aspx");
    //    }
    //    if (e.Item.Text.Equals("Job Costing Search"))
    //    {
    //        Session["JobCostingSearchCriteria"] = null;
    //        Response.Redirect("JobCostingSearch.aspx");
    //    }

    //    // Commented by Rajesh
    //    //if (e.Item.Text.Equals("Logout"))
    //    //{
    //    //    this.AuditLogout();
    //    //    if (Session["UserType"].ToString().Contains("I"))
    //    //    {
    //    //        Session.Abandon();
    //    //        //Response.Write("<script language='javascript'>self.close();</script>");
    //    //        //Response.Write("<script language='javascript'>window.open('close.htm', '_self');</script>"); working ie7.
    //    //        Response.Write("<script language='javascript'>var obj_window = window.open('close.htm', '_self', 'width=650,height=525,status=no,resizable=no,top='+((screen.height-575)/2)+',left='+((screen.width-650)/2)+',dependent=yes,alwaysRaised=yes');obj_window.opener = window;obj_window.focus();</script>");
    //    //        //Response.Write("<script language='javascript'>var windowObject = window.self; windowObject.opener = window.self; windowObject.close();</script>");
    //    //        Response.End();
    //    //    }
    //    //    Session.Abandon();
    //    //    FormsAuthentication.SignOut();
    //    //    FormsAuthentication.RedirectToLoginPage();
    //    //    Response.Redirect("Login.aspx");

    //    //}
    //    // Commented by Rajesh
        

    //    ////switch (e.Item.Tag.ToString())
    //    ////{
    //    ////    case "Home":
    //    ////        Session["Clicked"] = null;
    //    ////        Response.Redirect("Default.aspx");
    //    ////        break;
    //    ////    case "Masters":
    //    ////        Session["Clicked"] = "Masters";
    //    ////        Session["SelGrpIndex"] = null;
    //    ////        Session["SelItem"] = null;
    //    ////        Response.Redirect("Masters.aspx");
    //    ////        break;
    //    ////    case "Maintenance":
    //    ////        Session["Clicked"] = "Maintenance";
    //    ////        Session["SelGrpIndex"] = null;
    //    ////        Session["SelItem"] = null;
    //    ////        Response.Redirect("MaintenancePage.aspx");
    //    ////        break;
    //    ////    case "Reports":
    //    ////        break;
    //    ////    case "Logout":
    //    ////        Response.Redirect("Login.aspx");
    //    ////        break;
    //    ////    default:
    //    ////        Session["Clicked"] = null;
    //    ////        Response.Redirect("Default.aspx");
    //    ////        break;
    //    ////}
    //}
    #endregion

    #region " User Defined Methods "
    private void AuditLogout()
    {
       

        string mode=ConfigurationManager.AppSettings["Mode"];

        if(Convert.ToInt32(mode)!=3)
        {
             AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
             objAudit.UserID = Convert.ToInt32(Session["UserID"]);
             objAudit.AuditType = 1;
             //System.Net.IPHostEntry TmpEntry = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]);
             //objAudit.IP = TmpEntry.AddressList[0].ToString();
             objAudit.IP = Request.ServerVariables["REMOTE_ADDR"];
             objAudit.Persist(DALCOperation.Insert);


        }
        //else
        //{
        //        ICommon svc = (ICommon)RemotingHelper.CreateProxy(typeof(ICommon));
        //        AuditLoginLogoutBAL objAudit = new AuditLoginLogoutBAL();
        //        //objAudit.UserID = Convert.ToInt32(Session["UserID"]);
        //        //objAudit.AuditType = 0;
        //        //System.Net.IPHostEntry TmpEntry = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]);
        //       // objAudit.IP = TmpEntry.AddressList[0].ToString();
        //       // objAudit.Persist(DALCOperation.Insert);          
        //        //svc.AuditLogin(Convert.ToInt32(Session["UserID"]),1,TmpEntry.AddressList[0].ToString());
        //        svc.AuditLogin(Convert.ToInt32(Session["UserID"]), 1, Request.ServerVariables["REMOTE_ADDR"]);
        //}


    }
    private void setSelected()
    {
        //string select=string.Empty;
        //if(Session["Clicked"] !=null)
        //    select = Session["Clicked"].ToString();
        //switch (select)
        //{
        //    case "Masters":
        //        this.uwMenu.Items[1].CssClass = "SelectedStyle";
        //        break;
        //    case "Maintenance":
        //        this.uwMenu.Items[2].CssClass = "SelectedStyle";
        //        break;
        //    default:
        //        this.uwMenu.Items[0].CssClass = "SelectedStyle";
        //        break;
        //}
    }
    #endregion
}
