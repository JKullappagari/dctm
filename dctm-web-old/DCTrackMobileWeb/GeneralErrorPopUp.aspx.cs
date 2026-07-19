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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

public partial class GeneralErrorPopup : System.Web.UI.Page
{
    private Exception ex;
    private string ExceptionBody;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ex = (Exception)Application["ErrorClass"];

            //ex = Server.GetLastError().GetBaseException();
            if (ex != null)
            {
                this.LogException(ex);
                if (ex.InnerException != null)
                {
                    foreach (DictionaryEntry de in ex.InnerException.Data)
                    {
                        lblMsg.Text += "<br>";
                        lblMsg.Text += de.Key + "   " + de.Value;
                    }
                    ExceptionBody = Server.HtmlEncode(ex.Message);
                    //ex.Data.Add(ExceptionData.UserCredentials.ToString(), (Hashtable)Session["userCredentials"]);
                    //ExceptionPolicy.HandleException(ex, ExceptionPolicies.LoggedExceptionPolicy.ToString());
                }
            }
        } 
    }
    protected void lnkHome_Click(object sender, EventArgs e)
    {
        Session["Clicked"] = "";
        Response.Redirect("MainPage.aspx");
    }
    protected void lnkViewErrReport_Click(object sender, EventArgs e)
    {
        litErrorText.Text = " ";
        if(ex==null)
            ex = (Exception)Application["ErrorClass"];
        if (ex != null)
        {
            if (ex.InnerException != null)
            {
                //litErrorText.Text += ex.Data["ExceptionData.ExceptionMessage"].ToString();
                litErrorText.Text += "<BR><B>Error Message : " + ex.InnerException.Message + "</B><BR>";
                //litErrorText.Text += ex.StackTrace.Replace(ControlChars.CrLf, "<BR>");
                litErrorText.Text += ex.ToString();
            }
            else
            {
                //litErrorText.Text += ex.Data["ExceptionData.ExceptionMessage"].ToString();
                litErrorText.Text += "<BR><B>Error Message : " + ex.Message + "</B><BR>";
                //litErrorText.Text += ex.StackTrace.Replace(ControlChars.CrLf, "<BR>");
                litErrorText.Text += ex.ToString();
            }
        }
        else
        {
            litErrorText.Text += "<B>Error Message : Unable to catch the exception</B><BR>";
        }
    }
    protected void lnkSendErrReport_Click(object sender, EventArgs e)
    {

    }
    private void LogException(Exception ex)
    {
        try
        {
            //* Commented on 07-Oct-2013
            //string fileName = Server.MapPath(".\\Logs");
            //fileName += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";            
            //StreamWriter sw;
            //if (!File.Exists(fileName))
            //{
            //    sw = File.CreateText(fileName);
            //    sw.WriteLine("Created log file");
            //    sw.WriteLine("Date and time accessed : " + DateTime.Now.ToString());
            //    sw.WriteLine("Logged in user : " + Session["UserName"].ToString());
            //    sw.WriteLine("------------------------------------------------------");
            //    sw.WriteLine("Error message : " + ex.Message);
            //    sw.WriteLine("Stack trace : " + ex.ToString());
            //    sw.WriteLine("------------------------------------------------------");
            //    sw.Close();                
            //}
            //else
            //{
            //    sw = new StreamWriter(fileName, true);
            //    sw.WriteLine("Appending to existing log file");
            //    sw.WriteLine("Date and time accessed : " + DateTime.Now.ToString());
            //    sw.WriteLine("Logged in user : " + Session["UserName"].ToString());
            //    sw.WriteLine("------------------------------------------------------");
            //    sw.WriteLine("Error message : " + ex.Message);
            //    sw.WriteLine("Stack trace : " + ex.ToString());
            //    sw.WriteLine("------------------------------------------------------");
            //    sw.Close();                
            //}//*
            ExceptionPolicy.HandleException(ex, "errDCTrack");// Added on 07-Oct-2013
        }
        catch (Exception dex)
        {
            litErrorText.Text += "<BR><B>Error Message : " + dex.Message + "</B><BR>";
        }
    }
}
