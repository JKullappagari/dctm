/*
File Name:	DeAssignRFIDCard.aspx.cs

Description:	Used to DeAssign RFID Card .	

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Atchuta 		27/03/2006		File has been created.
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

public partial class DeAssignRFIDCard : System.Web.UI.Page
{
    /// <summary>
    /// Used to load the page.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            lblAssetID.Text = Session["AssetID"].ToString();
            CheckAssetDetails();
        }

        txtReason.Attributes.Add("onkeypress", "doKeypress(this," + txtReason.MaxLength.ToString() + ");");
        txtReason.Attributes.Add("onbeforepaste", "doBeforePaste(this," + txtReason.MaxLength.ToString() + ");");
        txtReason.Attributes.Add("onpaste", "doPaste(this," + txtReason.MaxLength.ToString() + ");");

    }
    /// <summary>
    /// Used to validate the RFID Card Details.
    /// </summary>
    /// <returns>True if valid; Otherwise returns FALSE.</returns>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    private Boolean CheckAssetDetails()
    {
        AssetBarringBAL objAssetBarringBAL = new AssetBarringBAL();
        objAssetBarringBAL.AssetID = Convert.ToInt32(Session["AssetID"].ToString());
        DataRow dr = objAssetBarringBAL.FillAssetDetails();
        string strRfidCardNumber = Convert.IsDBNull(dr["CurrentRFIDCardNumber"]) ? "" : Convert.ToString(dr["CurrentRFIDCardNumber"]);
        if (strRfidCardNumber.Trim() == "" || strRfidCardNumber.Trim() == null)
        {
            Response.Write("<script language='javascript'>alert('Document's RFID Tag DeAssigned by another user');</script>");
            Response.Write("<script language='javascript'>window.close();</script>");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Used to save RFID Deassignment details to the database.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibCreate_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (CheckAssetDetails() == false)
            return;
        RFIDCardAssignmentBAL objRFIDCardAssignmentBAL = new RFIDCardAssignmentBAL();
        objRFIDCardAssignmentBAL.AssetID = Convert.ToInt32(lblAssetID.Text);
        Session["AssetID"] = lblAssetID.Text;
        objRFIDCardAssignmentBAL.RFIDCardNumber = "";// rfidCardNoTextBox.Text;// Convert.ToInt32(txtRFIDCardNo.Text);
        //objRFIDCardAssignmentBAL.RFIDExpiryDate="";
        objRFIDCardAssignmentBAL.Reason = txtReason.Text;
        objRFIDCardAssignmentBAL.Action = "RFID Tag DeAssigned";
        if (Session["UserID"] != null)
            objRFIDCardAssignmentBAL.UpdatedBy = Convert.ToInt32(Session["UserID"].ToString());
        else
            objRFIDCardAssignmentBAL.UpdatedBy = 0;
        objRFIDCardAssignmentBAL.Persist(DALCOperation.Update);

        CommonBAL objCommonBAL = new CommonBAL();
        if (objRFIDCardAssignmentBAL.IsAssetExists == 2)
        {
            lblMessage.Text = objCommonBAL.displayMessage("RFID_DEASSIGN_E_TAGNOTASSIGNED");//"RFID Card already assigned";
            lblMessage.Visible = true;
            return;
        }
        else if (objRFIDCardAssignmentBAL.IsAssetExists == 4)
        {
            lblMessage.Text = objCommonBAL.displayMessage("RFID_DEASSIGN_E_DOCISISSUED");// "Should not assign Returned or Lost card";
            lblMessage.Visible = true;
            return;
        }

        Response.Write("<script language='javascript'>window.close();</script>");
    }

    /// <summary>
    /// Used to close screen.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void ibReset_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Response.Write("<script language='javascript'>window.close();</script>");
    }
    /// <summary>
    /// Used to change the title.
    /// </summary>
    /// <author>Atchuta</author>
    /// <createdOn>27 March 2006</createdOn>
    protected void txtLabel_TextChanged(object sender, EventArgs e)
    {
        this.Title = txtLabel.Text;
    }
}
