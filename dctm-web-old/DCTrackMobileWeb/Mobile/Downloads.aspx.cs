using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;

public partial class Mobile_Downloads : System.Web.UI.Page
{

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);


    }

    public void Clicked(object sender, EventArgs e)
    {
        LinkButton button = (LinkButton)sender;
        string buttonId = button.ID;
        string preReqFile = string.Empty;
        string userPlatform = GetUserPlatform(HttpContext.Current.Request);

        if (userPlatform.ToLower().Contains("android"))
        {
            preReqFile = buttonId + ".APK";
        }
        else if (userPlatform.ToLower().Contains("windows"))
        {
            preReqFile = buttonId + ".CAB";
        }

        fileDownload(preReqFile, Server.MapPath("~/Mobile/Downloads/" + preReqFile));

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //pageWidth = Page.Request.Browser.ScreenPixelsWidth;

        if (Session["LoggedInUser"] == null)
        {
            Response.Redirect("~/Mobile/MobileLogin.aspx");
        }

        //check number of files and list them all with lnkbuttons for download
        DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Mobile/Downloads"));
        //based on user platform, show appropriate files
        // for Windows Mobile / Windows CE show CAB files
        // for Android platform show APK files.
        string userPlatform = GetUserPlatform(HttpContext.Current.Request);
        if (userPlatform.ToLower().Contains("android"))
        {
            //get all APK files
            foreach (FileInfo file in dir.GetFiles("*.apk"))
            {
                LinkButton lb = new LinkButton();
                lb.ID = file.Name.ToLower().Replace(".apk", "").ToUpper();
                lb.Text = file.Name;
                lb.Attributes.Add("target", "_blank");
                lb.Click += new EventHandler(Clicked);
                PlaceHolder1.Controls.Add(lb);
                //Add This
                PlaceHolder1.Controls.Add(new LiteralControl("<br />"));
            }
        }
        else if(userPlatform.ToLower().Contains("windows"))
        {
            //get all cab files only.
            foreach (FileInfo file in dir.GetFiles("*.cab"))
            {
                LinkButton lb = new LinkButton();
                lb.ID = file.Name.ToLower().Replace(".cab", "").ToUpper();
                lb.Text = file.Name;
                lb.Click += new EventHandler(Clicked);
                PlaceHolder1.Controls.Add(lb);
                //Add This
                PlaceHolder1.Controls.Add(new LiteralControl("<br />"));
            }
        }


    }


   

    public String GetUserPlatform(HttpRequest request)
    {
        var ua = request.UserAgent;

        if (ua.Contains("Android"))
            return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

        if (ua.Contains("iPad"))
            return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

        if (ua.Contains("iPhone"))
            return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

        if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
            return "Kindle Fire";

        if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
            return "Black Berry";

        if (ua.Contains("Windows Phone"))
            return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

        if (ua.Contains("Mac OS"))
            return "Mac OS";

        if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
            return "Windows XP";

        if (ua.Contains("Windows NT 6.0"))
            return "Windows Vista";

        if (ua.Contains("Windows NT 6.1"))
            return "Windows 7";

        if (ua.Contains("Windows NT 6.2"))
            return "Windows 8";

        if (ua.Contains("Windows NT 6.3"))
            return "Windows 8.1";

        if (ua.Contains("Windows NT 10"))
            return "Windows 10";

        if (ua.Contains("Windows CE"))
            return "Windows CE";

        //fallback to basic platform:
        return request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
    }

    public String GetMobileVersion(string userAgent, string device)
    {
        var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
        var version = string.Empty;

        foreach (var character in temp)
        {
            var validCharacter = false;
            int test = 0;

            if (Int32.TryParse(character.ToString(), out test))
            {
                version += character;
                validCharacter = true;
            }

            if (character == '.' || character == '_')
            {
                version += '.';
                validCharacter = true;
            }

            if (validCharacter == false)
                break;
        }

        return version;
    }



    private void fileDownload(string fileName, string fileUrl)
    {
        Page.Response.Clear();
        bool success = ResponseFile(Page.Request, Page.Response, fileName, fileUrl, 1024000);
        if (!success)
            Response.Write("Downloading Error!");
        Page.Response.End();

    }
    public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
    {
        try
        {
            FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(myFile);
            try
            {
                _Response.AddHeader("Accept-Ranges", "bytes");
                _Response.Buffer = false;
                long fileLength = myFile.Length;
                long startBytes = 0;

                int pack = 102400; //10K bytes
                int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;
                if (_Request.Headers["Range"] != null)
                {
                    _Response.StatusCode = 206;
                    string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                    startBytes = Convert.ToInt64(range[1]);
                }
                _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                if (startBytes != 0)
                {
                    _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                }


                _Response.AddHeader("Content-Length", myFile.Length.ToString());
                _Response.AddHeader("Connection", "Keep-Alive");
                _Response.ContentType = "application/octet-stream";
                _Response.AddHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8) + "\"");
                //_Response.BinaryWrite(((MemoryStream)(br.BaseStream)).ToArray());
                br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                for (int i = 0; i < maxCount; i++)
                {
                    if (_Response.IsClientConnected)
                    {
                        _Response.BinaryWrite(br.ReadBytes(pack));
                        Thread.Sleep(sleep);
                    }
                    else
                    {
                        i = maxCount;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                br.Close();
                myFile.Close();
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
}