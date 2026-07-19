using System;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using settings = System.Configuration.ConfigurationManager;
/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{
    public Utility()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetUrlRootPath()
    {
        string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
        if (protocol == null || protocol == "0")
            protocol = @"http://";
        else
            protocol = @"https://";

        string server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

        string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
        if (port == null || port == "80" || port == "443")
            port = "";
        else
            port = ":" + port;

        string virtualDir = HttpContext.Current.Request.ApplicationPath;

        return protocol + server + port + virtualDir;
    }

    public static int GetWorkPermitNumberLength()
    {
        int maxLength;
        if (!int.TryParse(ConfigurationManager.AppSettings["WorkPermitNumberLength"], out maxLength))
            maxLength = 6;
        return  maxLength;
    }

    public static int GetSiteID()
    {
        int siteID;

        if (!int.TryParse(ConfigurationManager.AppSettings["SiteID"], out siteID))
        {
            throw new ApplicationException(
                "The <SiteID> parameter is not defined correctly in the web.config!");
        }

        return siteID;
    }

    //HP: Added ValidatePassword method to validates the password for Windows complexity requirement.
    /// <summary>
    /// Validates the password for Windows complexity requirement when user changes password or Administrator 
    /// resets it.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static bool ValidatePassword(String inputString)
    {
        Regex regex;
        String AtleastOneNumericDigitRegex = @"(?=.*\d)";
        String AtleastOneLowerCaseCharacterRegex = @"(?=.*[a-z])";
        String AtleastOneUpperCaseCharacterRegex = @"(?=.*[A-Z])";
        String PasswordLengthRegex = @"^(?=.*).{{{0},{1}}}$";
        String NonAlphaNumericCharacters = @"`~!@#$%^&*()-_=+[]{}\|/;:',<.>?""";

        int mustBeAtleastCharacters = 6;                            // Default
        int allowedMaximumCharacters = 32;                          // Default
        bool mustContainAtleastOneUpperCaseCharacter = false;
        bool mustContainAtleastOneLowerCaseCharacter = false;
        bool mustContainAtleastOneNumericDigit = false;
        bool mustContainAtleastOneNonAlphabeticCharacter = false;
        bool mustNotContainAllorPartofUserAccountName = false;

        int.TryParse(settings.AppSettings["mustBeAtleastCharacters"].ToString(), out mustBeAtleastCharacters);

        int.TryParse(settings.AppSettings["allowedMaximumCharacters"].ToString(), out allowedMaximumCharacters);

        bool.TryParse(settings.AppSettings["mustContainAtleastOneUpperCaseCharacter"].ToString(),
            out mustContainAtleastOneUpperCaseCharacter);

        bool.TryParse(settings.AppSettings["mustContainAtleastOneLowerCaseCharacter"].ToString(),
            out mustContainAtleastOneLowerCaseCharacter);

        bool.TryParse(settings.AppSettings["mustContainAtleastOneNumericDigit"].ToString(),
            out mustContainAtleastOneNumericDigit);

        bool.TryParse(settings.AppSettings["mustContainAtleastOneNonAlphabeticCharacter"].ToString(),
            out mustContainAtleastOneNonAlphabeticCharacter);

        bool.TryParse(settings.AppSettings["mustNotContainAllorPartofUserAccountName"].ToString(),
            out mustNotContainAllorPartofUserAccountName);

        if (mustBeAtleastCharacters > 0)
        {
            regex = new Regex(String.Format(PasswordLengthRegex, mustBeAtleastCharacters, allowedMaximumCharacters),
                RegexOptions.CultureInvariant);
            if (!regex.IsMatch(inputString)) return false;
        }
        if (mustContainAtleastOneNumericDigit)
        {
            regex = new Regex(AtleastOneNumericDigitRegex, RegexOptions.CultureInvariant);
            if (!regex.IsMatch(inputString)) return false;
        }
        if (mustContainAtleastOneLowerCaseCharacter)
        {
            regex = new Regex(AtleastOneLowerCaseCharacterRegex, RegexOptions.CultureInvariant);
            if (!regex.IsMatch(inputString)) return false;
        }
        if (mustContainAtleastOneUpperCaseCharacter)
        {
            regex = new Regex(AtleastOneUpperCaseCharacterRegex, RegexOptions.CultureInvariant);
            if (!regex.IsMatch(inputString)) return false;
        }
        if (mustContainAtleastOneNonAlphabeticCharacter)
        {
            char[] NonAlphaNumericCharacterArray = NonAlphaNumericCharacters.ToCharArray();
            if (inputString.IndexOfAny(NonAlphaNumericCharacterArray) < 0)
                return false;
        }
        if (mustNotContainAllorPartofUserAccountName)
        {
            String searchFrom = System.Web.HttpContext.Current.Session["UserName"].ToString();
            int charPos = 0;
            while (charPos <= searchFrom.Length - 3)
            {
                String search = searchFrom.Substring(charPos, 3);
                if (inputString.ToLower().Trim().Contains(search.ToLower()))
                    return false;
                else
                    charPos++;
            }
        }
        return true;
    }
    /// <summary>
    /// Check whether input string is numeric
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static bool ISNumeric(string strToCheck)
    {
        return Regex.IsMatch(strToCheck,"^\\d+(\\.\\d+)?$");
    }
}