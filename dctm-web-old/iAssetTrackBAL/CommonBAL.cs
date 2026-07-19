/*
File Name   :	CommonBAL.cs

Description :	Business Logic layer for Common Functions

Date created:	27 March 2006

Modification History:
***********************
CR		Name			Date			Description
New		Murugan K	27/03/2006		File has been created.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Web;
using System.Data.OleDb;
using System.Security.Cryptography;
using iAssetTrackBAL;
using iAssetTrack.DALC;
using System.Web.UI.WebControls;


namespace iAssetTrack.BAL
{
    public class CommonBAL
    {
        #region Common
        //* BAL Name:  CommonBAL
        //* Author :  Murugan K
        //* Date	  :  10:27 AM 10/12/2006
        //* Use : Application common functions.
        //* Caller: 
        #endregion

        #region Declarations
        private int intUserID;
        private int intBusinessUnitID;
        #endregion

        #region Properties
            public int UserID
            {
                get { return this.intUserID; }
                set { this.intUserID = value; }
            }
        public int BusinessUnitID
        {
            get { return this.intBusinessUnitID; }
            set { this.intBusinessUnitID = value; }
        }
        #endregion

        /// <summary>
        /// Retrive Business Unit List by user ID
        /// </summary>
        /// <params>UserID</params>
        /// <returns>Dataset</returns>
        public DataSet BusinessUnitListByUserID()
        {
            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_BUSINESSUNIT_LISTBYUSERID, DALCResultType.DataSet);
            criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, UserID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            return (ds);
        }

        /// <summary>
        /// Generic fill drop down list
        /// </summary>
        /// <param name="strStoredProc"></param>
        /// <returns>Data Table</returns>
        public DataTable FillDropDownList(string strStoredProc)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(strStoredProc, DALCResultType.DataSet);
            //criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, pIntSiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            dr[0] = 0;
            dr[1] = "-- Select --";
            //ds.Tables[0].Rows.Add(dr);
            ds.Tables[0].Rows.InsertAt(dr, 0);

            return ds.Tables[0];
        }
        public DataTable FillDropDownLst(string strStoredProc)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(strStoredProc, DALCResultType.DataSet);
            //criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, pIntSiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            dr[0] = 0;
            dr[1] = "All";
            //dr[2] = "All";
            //ds.Tables[0].Rows.Add(dr);
            ds.Tables[0].Rows.InsertAt(dr, 0);

            return ds.Tables[0];
        }

        /// <summary>
        /// Generic fill drop down list
        /// </summary>
        /// <param name="strStoredProc"></param>
        /// <param name="strEmptyValue"></param>
        /// <returns>Data Table</returns>
        public DataTable FillDropDownList(string strStoredProc, string strEmptyValue)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(strStoredProc, DALCResultType.DataSet);
            //criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, pIntSiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            dr[0] = 0;
            dr[1] = strEmptyValue;//"-- Select --";
            //ds.Tables[0].Rows.Add(dr);
            ds.Tables[0].Rows.InsertAt(dr, 0);

            return ds.Tables[0];
        }

        public DataTable FillDropDownListSingleParam(string strStoredProc, string strEmptyValue)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(strStoredProc, DALCResultType.DataSet);
            //criteria.AddInParameter(Parameters.PARAM_SITEID, DbType.Int32, pIntSiteID);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            //dr[0] = 0;
            dr[0] = strEmptyValue;//"-- Select --";
            //ds.Tables[0].Rows.Add(dr);
            ds.Tables[0].Rows.InsertAt(dr, 0);

            return ds.Tables[0];
        }


        

        /// <summary>
        /// Generic fill drop down list by business unit
        /// </summary>
        /// <param name="strStoredProc"></param>
        /// <param name="strEmptyValue"></param>
        /// <param name="id"></param>
        /// <returns>Data Table</returns>
        public DataTable FillDropDownListBU(string strStoredProc, string strEmptyValue, int id)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(strStoredProc, DALCResultType.DataSet);
            criteria.AddInParameter( Parameters.PARAM_USERID, DbType.Int32, id);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            if (strEmptyValue != "")
            {
                DataRow dr;
                dr = ds.Tables[0].NewRow();
                dr[0] = 0;
                dr[1] = strEmptyValue;//"-- Select --";
                //ds.Tables[0].Rows.Add(dr);
                ds.Tables[0].Rows.InsertAt(dr, 0);
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// Generic fill drop down list
        /// </summary>
        /// <param name="strStoredProc"></param>
        /// <param name="strEmptyValue"></param>
        /// <param name="id"></param>
        /// <returns>Data Table</returns>
        public DataTable FillDropDownList(string strStoredProc, string strEmptyValue, int id)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(strStoredProc, DALCResultType.DataSet);
            
            criteria.AddInParameter(Parameters.PARAM_BUSINESSUNITID, DbType.Int32, id);            
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            dr[0] = 0;
            dr[1] = strEmptyValue;//"-- Select --";
            //ds.Tables[0].Rows.Add(dr);
            ds.Tables[0].Rows.InsertAt(dr, 0);

            return ds.Tables[0];
        }

        /// <summary>
        /// Generic fill drop down list
        /// </summary>
        /// <param name="strStoredProc"></param>
        /// <param name="strEmptyValue"></param>
        /// <param name="id"></param>
        /// <param name="strParameter"></param>
        /// <returns>Data Table</returns>
        public DataTable FillDropDownList(string strStoredProc, string strEmptyValue, int id, string strParamter)
        {
            DALCCommandHelper criteria = new DALCCommandHelper(strStoredProc, DALCResultType.DataSet);
            
            criteria.AddInParameter(strParamter, DbType.Int32, id);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            Dictionary<string, object> output = criteria.OutputParameters;
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            dr[0] = 0;
            dr[1] = strEmptyValue;//"-- Select --";
            //ds.Tables[0].Rows.Add(dr);
            ds.Tables[0].Rows.InsertAt(dr, 0);

            return ds.Tables[0];
        }


 
        /// <summary>
        /// Add Empty Row function has been used to add empty row to dropdown list.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>DataRow</returns>
        public DataRow AddEmptyRow(DataRow dr)
        {
            dr[0] = "0";
            dr[1] = "--Select--";
            return dr;
        }

        /// <summary>
        /// Common set datasouce function has been used to fill infragistics drop down list.
        /// </summary>
        /// <param name="wco"></param>
        /// <param name="dt"></param>
        /// <param name="isBlankRow"></param>
        /// <param name="isSearch"></param>
        //public  void setDataSource(Infragistics.WebUI.WebCombo.WebCombo wco, DataTable dt, bool isBlankRow,bool isSearch)
        //{
        //    if (isBlankRow)
        //        dt.Rows.InsertAt(this.AddEmptyRow(dt.NewRow()), 0);
        //    if (isSearch)
        //    {
        //        wco.Rows[0].Cells[0].Value = "(All)";
        //        wco.Rows[0].Cells[1].Value = "(All)";
        //    }
        //    wco.DataTextField = dt.Columns[1].ColumnName;
        //    wco.DataValueField = dt.Columns[0].ColumnName;
        //    wco.DataSource = dt;
        //    wco.DataBind();
        //    wco.Columns[0].Hidden = true;
        //    wco.Columns[1].Hidden = true;
            
        //}
        //J00008 by kjb on 01 Oct 2012


        /// <summary>
        /// Common set data source function has been used to fill infragistics dorp down list with empty row.
        /// </summary>
        /// <param name="wdl"></param>
        /// <param name="dt"></param>
        /// <param name="strText"></param>
        public void setDataSource(System.Web.UI.WebControls.DropDownList wdl, DataTable dt, String strText)
        {
            
            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = strText;
            dt.Rows.InsertAt(dr, 0);
               
            wdl.DataTextField = dt.Columns[1].ColumnName;
            wdl.DataValueField = dt.Columns[0].ColumnName;
            wdl.DataSource = dt;
            wdl.DataBind();

        }
        ///// <summary>
        ///// Common set data source function has been used to fill infragistics dorp down list with empty row.
        ///// </summary>
        ///// <param name="wdl"></param>
        ///// <param name="dt"></param>
        ///// <param name="strText"></param>
        //public void setDataSourceInfra(Infragistics.Web.UI.ListControls.WebDropDown wdl, DataTable dt, String strText)
        //{

        //    DataRow dr = dt.NewRow();
        //    dr[0] = "0";
        //    dr[1] = strText;
        //    dt.Rows.InsertAt(dr, 0);

        //    wdl.TextField = dt.Columns[1].ColumnName;
        //    wdl.ValueField = dt.Columns[0].ColumnName;
        //    wdl.DataSource = dt;
        //    wdl.DataBind();

        //}

        /// <summary>
        /// Common set data source function has been used to fill infragistics dorp down list with empty row.
        /// </summary>
        /// <param name="wdl"></param>
        /// <param name="dt"></param>
        /// <param name="strText"></param>
        public void setDataSourceInfra(DropDownList wdl, DataTable dt, String strText)
        {

            DataRow dr = dt.NewRow();
            dr[0] = "0";
            dr[1] = strText;
            dt.Rows.InsertAt(dr, 0);

            wdl.DataTextField = dt.Columns[1].ColumnName;
            wdl.DataValueField = dt.Columns[0].ColumnName;
            wdl.DataSource = dt;
            wdl.DataBind();

        }

        /// <summary>
        /// Display Error Messagae
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <returns>string</returns>
        public string displayMessage(string MessageCode)
        {
            iAssetTrack.BAL.MessageBAL objMessage = new MessageBAL();
            objMessage.MessageCode = MessageCode;
            return (objMessage.retrieve());
        }

        /// <summary>
        /// Export to Microsoft Excel file.
        /// </summary>
        /// <param name="grdExport"></param>
        /// <param name="exporter"></param>
        /// <param name="dt"></param>
        /// <param name="downloadName"></param>
        /// <returns>Excel File</returns>
        //public static void ExportExcel(
        //    Infragistics.WebUI.UltraWebGrid.UltraWebGrid grdExport,
        //    Infragistics.WebUI.UltraWebGrid.ExcelExport.UltraWebGridExcelExporter exporter,
        //    DataTable dt, string downloadName)
        //{
        //    try
        //    {
        //        Infragistics.WebUI.UltraWebGrid.UltraWebGrid grdHeader = new Infragistics.WebUI.UltraWebGrid.UltraWebGrid("grdHeader");
        //        grdHeader.DisplayLayout.AutoGenerateColumns = false;
        //        grdHeader.ID = grdExport.Caption;

        //        grdHeader.Bands[0].HeaderStyle.BackColor = System.Drawing.Color.FromArgb(60, 123, 194);
        //        grdHeader.Bands[0].HeaderStyle.Font.Name = "Verdana";
        //        grdHeader.Bands[0].HeaderStyle.Font.Size = System.Web.UI.WebControls.FontUnit.Small;
        //        grdHeader.Bands[0].HeaderStyle.Font.Bold = true;
        //        grdHeader.Bands[0].RowStyle.Font.Name = "Verdana";
        //        grdHeader.Bands[0].HeaderStyle.ForeColor = Color.White;
        //        grdHeader.Bands[0].RowStyle.Font.Size = System.Web.UI.WebControls.FontUnit.XXSmall;

        //        foreach (Infragistics.WebUI.UltraWebGrid.UltraGridColumn grdCol in grdExport.Bands[0].Columns)
        //        {
        //            if (!grdCol.Hidden && grdCol.BaseColumnName.Length != 0)
        //            {
        //                if (grdCol.Header.Caption != "")
        //                {
        //                    if (grdCol.Header.Caption.Trim().Length != 0)
        //                        grdHeader.Bands[0].Columns.Add(grdCol);
        //                }
        //            }
        //        }

        //        grdHeader.DataSource = dt;
        //        grdHeader.DataBind();
        //        /*
        //        int pgIndex = grdExport.DisplayLayout.Pager.CurrentPageIndex;
        //        grdExport.DisplayLayout.Pager.PageSize = dt.Rows.Count;
        //        grdExport.DisplayLayout.Pager.CurrentPageIndex = 1;
        //        grdExport.DataSource = dt;
        //        grdExport.DataBind();
        //        */
        //        exporter.DownloadName = downloadName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
        //        //exporter.Export(grdHeader);
        //    }
        //    catch (Exception)
        //    {
        //        //System.Diagnostics.Debug.WriteLine("");
        //    }
        //}

        //J00009 by kjb on 01 Oct 2012

       /// <summary>
        /// To encrypt any given string using md5 hash services
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns>string</returns>
        public string GetHashValue(string strInput) 
        {
            byte[] salt = {1};

            //salt = System.Text.Encoding.ASCII.GetBytes("1");

            String hashPass = CryptographyUtil.ComputeHash(strInput, "MD5", salt);
            return (hashPass);
        }


        public string GetSHA256HashValue(string strInput)
        {
            //byte[] salt = { 652 };

            byte[] intBytes = BitConverter.GetBytes(652);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            byte[] salt = intBytes;

            String hashVal = CryptographyUtil.ComputeHash(strInput, "SHA256", salt);
            return (hashVal);

        }

        /// <summary>
        /// Checking refrential intagrity check before deleting records from table.
        /// </summary>
        /// <param name="strColumn"></param>
        /// <param name="intValue"></param>
        /// <returns>DataSet</returns>
        /// <Modified by>kjb on 02 sep 2011</Modified>
        public DataSet CheckBeforeDelete(string strColumn, string intValue,int IsGuid)
        {

            DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_CHECKBEFOREDELETE, DALCResultType.DataSet);

            criteria.AddInParameter(Parameters.PARAM_COLUMN, DbType.String, strColumn);
            criteria.AddInParameter(Parameters.PARAM_VALUE, DbType.String, intValue);
            criteria.AddInParameter(Parameters.PARAM_IS_GUID_FLAG, DbType.Int32, IsGuid);
            
            // int parameter changed to string to accomodate HostID (GUID)

            //criteria.AddOutParameter("@count", DbType.Int32,0,5);

            DataSet ds = (DataSet)criteria.ExecuteCommand();

            Dictionary<string, object> output = criteria.OutputParameters;
            //return() - Records
            return (ds); 


        }

        

        /// <summary>
        /// Retrive Site list by user ID
        /// </summary>
        /// <param name="Uid"></param>
        /// <returns>DataSet</returns>
        /// Debasish (Redundant) and also the constant SP_Site_List_By_User
        //public DataSet GetSiteListByUserID(int Uid)
        //{
        //    DataSet ds=null;
        //    try
        //    {
            
        //       DALCCommandHelper criteria = new DALCCommandHelper(StoredProcedures.SP_Site_List_By_User, DALCResultType.DataSet);
        //       criteria.AddInParameter(Parameters.PARAM_USERID, DbType.Int32, Uid);
        //     //criteria.AddOutParameter("@count", DbType.Int32,0,5);
        //       ds = (DataSet)criteria.ExecuteCommand();
        //        Dictionary<string, object> output = criteria.OutputParameters;
        //     }
        //    catch
        //    {
        //        throw;
        //    }

        //    //return() - Records
        //    return (ds); 

        //}


        /* <Usage>
          string   plainText          = txtEncrypt.Text;    // original plaintext        
                string passPhrase = "P@s5pr@se";        // can be any string Pas5pr@se
                string saltValue = "s@1tV@1ue";        // can be any string s@1tValue
                string hashAlgorithm = "MD5";             // can be "SHA1" / "MD5"
                int      passwordIterations = 2;                  // can be any number
                string   initVector         = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
                int      keySize            = 256;                // can be 192 or 128
        
        
                string  cipherText = this.Encrypt(plainText,
                                                    passPhrase,
                                                    saltValue,
                                                    hashAlgorithm,
                                                    passwordIterations,
                                                    initVector,
                                                    keySize);
                this.txtDecrypt.Text = cipherText;
         
         */

        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm
        /// and returns a base64-encoded result.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be encrypted.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this function we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this function we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be 
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Encrypted value formatted as a base64-encoded string.
        /// </returns>

        public string Encrypt(string plainText,
                                 string passPhrase,
                                 string saltValue,
                                 string hashAlgorithm,
                                 int passwordIterations,
                                 string initVector,
                                 int keySize)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }



        /*Usage
         string cipherText = txtDecrypt.Text;    // original plaintext        
                string passPhrase = "P@s5pr@se";        // can be any string
                string saltValue = "s@1tV@1ue";        // can be any string
                string hashAlgorithm = "MD5";          // can be "SHA1" / "MD5"
                int passwordIterations = 2;                  // can be any number
                string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
                int keySize = 256;                // can be 192 or 128

                string plainText = this.Decrypt(cipherText,
                                                        passPhrase,
                                                        saltValue,
                                                        hashAlgorithm,
                                                        passwordIterations,
                                                        initVector,
                                                        keySize);
                this.txtEncrypt.Text = plainText;
         
         */


        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this function we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this function we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt
        /// logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of
        /// the Encrypt function which was called to generate the
        /// ciphertext.
        /// </remarks>
        public string Decrypt(string cipherText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            // Return decrypted string.   
            return plainText;
        }

       
    }
}
