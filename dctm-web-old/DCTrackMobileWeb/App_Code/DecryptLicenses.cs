using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
namespace App_Code
{
    /// <summary>
    /// Summary description for DecryptLicenses
    /// </summary>
    public class DecryptLicenses
    {
        public DecryptLicenses()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static LicenseData GetTenantData()
        {
            LicenseData licenseData = null;
            FileStream stream = null;
            try
            {
                string licenseFile = string.Empty;
                foreach (FileInfo file in new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory + "\\Tenant\\").GetFiles("*.encrypt"))
                {
                    licenseFile = file.FullName;
                }
                if (!string.IsNullOrEmpty(licenseFile))
                {
                    FileInfo fi = new FileInfo(licenseFile);
                    stream = new FileStream(licenseFile, FileMode.Open);
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, Convert.ToInt32(stream.Length));

                    byte[] decryptBytes = App_Code.Crypto.Decrypt("DCTr1ckDCTr1ckM0bileM0bileDCTr1ckM0bile", bytes);
                    string jsonString = Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

                    licenseData = JsonConvert.DeserializeObject<LicenseData>(jsonString);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                    stream.Close();

            }

            return licenseData;
        }


    }

    public class ColSettings
    {
        public string companyname { get; set; }
        public string nooftenants { get; set; }
        public string licensemode { get; set; }
        public string licensevalidthru { get; set; }
    }

    public class LicenseData
    {
        public ColSettings colSettings { get; set; }
    }
}