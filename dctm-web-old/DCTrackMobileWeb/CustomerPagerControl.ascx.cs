using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class PageChangedEventArgs : EventArgs
{
    public PageChangedEventArgs(int pageNumber)
    {
        this._pageNumber = pageNumber;
    }

    private int _pageNumber;

    public int PageNumber
    {
        get { return this._pageNumber; }
    }
}

    public partial class iAssetTrack_WebDataGrid_Paging_CustomerPagerControl : System.Web.UI.UserControl
{

    public event EventHandler<PageChangedEventArgs> PageChanged;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void PagerPageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (PageChanged != null)
        {
            this.PageChanged(this, new PageChangedEventArgs(PagerPageList.SelectedIndex));
        }
    }

    public DropDownList PageList
    {
        get
        {
            return this.PagerPageList;
        }
    }

    public void SetCurrentPageNumber(int pageNumber)
    {
        if (pageNumber <= this.PagerPageList.Items.Count - 1)
        {
            this.PagerPageList.SelectedIndex = pageNumber;
        }
    }

    public void SetupPageList(int numberOfPages)
    {
        PagerPageList.Items.Clear();

        for (int pageNumber = 1; pageNumber <= numberOfPages; pageNumber++)
        {
            ListItem lstItem = new ListItem(pageNumber.ToString());
            this.PagerPageList.Items.Add(lstItem);
        }

        this.PageText.Text = numberOfPages.ToString();

    }


}
