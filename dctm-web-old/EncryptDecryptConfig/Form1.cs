using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptDecryptConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtConfigFilePath.Text = openFileDialog1.FileName;
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableControls(false);
                lblMessage.Text = "";
                if (!string.IsNullOrEmpty(txtConfigFilePath.Text.Trim()))
                {
                    if (!txtConfigFilePath.Text.Trim().Contains("encrypt"))
                    {
                        FileInfo fi = new FileInfo(txtConfigFilePath.Text.Trim());
                        if (Directory.Exists(fi.DirectoryName))
                        {
                            if (File.Exists(txtConfigFilePath.Text.Trim()))
                            {
                                Crypto.EncryptFile(txtConfigFilePath.Text.Trim(),
                                txtConfigFilePath.Text.Trim() + "." + txtCompany.Text.Trim().ToLower() +  ".encrypt", "DCTr1ckDCTr1ckM0bileM0bileDCTr1ckM0bile");
                                lblMessage.Text = "Encryption Successful";
                            }
                            else
                            {
                                MessageBox.Show("File not found", "Encrypt-Decrypt Config");
                                txtConfigFilePath.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Directory not found", "Encrypt-Decrypt Config");
                            txtConfigFilePath.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select a Config file", "Encrypt-Decrypt Config");
                        txtConfigFilePath.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured!" + " Error Msg:" + ex.Message);
            }
            finally
            {
                EnableDisableControls(true);
            }

        }

        private void EnableDisableControls(bool OnOff)
        {
            txtConfigFilePath.Enabled = OnOff;
            btnBrowse.Enabled = OnOff;
            btnEncrypt.Enabled = OnOff;
            btnDecrypt.Enabled = OnOff;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableControls(false);
                lblMessage.Text = "";
                if (!string.IsNullOrEmpty(txtConfigFilePath.Text.Trim()))
                {
                    if (txtConfigFilePath.Text.Trim().Contains("encrypt"))
                    {

                        FileInfo fi = new FileInfo(txtConfigFilePath.Text.Trim());
                        if (Directory.Exists(fi.DirectoryName))
                        {
                            if (File.Exists(txtConfigFilePath.Text.Trim()))
                            {
                                Crypto.DecryptFile(txtConfigFilePath.Text.Trim(),
                                txtConfigFilePath.Text.Trim().Replace(".encrypt", ""), "DCTr1ckDCTr1ckM0bileM0bileDCTr1ckM0bile");
                                lblMessage.Text = "Decryption Successful";
                            }
                            else
                            {
                                MessageBox.Show("File not found", "Encrypt-Decrypt Config");
                                txtConfigFilePath.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Directory not found", "Encrypt-Decrypt Config");
                            txtConfigFilePath.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select a Encrypted file", "Encrypt-Decrypt Config");
                        txtConfigFilePath.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured!" + " Error Msg:" + ex.Message);
            }
            finally
            {
                EnableDisableControls(true);
            }
        }
    }

}
