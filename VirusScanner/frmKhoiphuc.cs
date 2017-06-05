using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace VirusScanner
{
    public partial class frmKhoiphuc : Form
    {
        public frmKhoiphuc(string path, string originPath)
        {
            InitializeComponent();
            _path = path;
            _originPath = originPath;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }
#region worker
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(DecryptFileComplete)
            {
                
                MyMessageBox.ShowMessage("Đã khôi phục tệp tin thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MyMessageBox.ShowMessage("Đã có lỗi trong quá trình khôi phục !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Encrypt(_path, _originPath);
        }

        BackgroundWorker worker = new BackgroundWorker();
#endregion
        string _path;
        string _originPath;
        private bool decryptFileComplete = false;
        public bool DecryptFileComplete { get => decryptFileComplete; set => decryptFileComplete = value; }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        
        /// <summary>
        /// Xac nhan xoa file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYes_Click(object sender, EventArgs e)
        {
            labelCachly.Text = "Đang khôi phục...";
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Huy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Ma hoa file de cach ly
        /// </summary>
        /// <param name="input"></param>
        private void Encrypt(string inputFile, string outputFile)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@"password");

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);
                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
                File.Delete(inputFile);
                DecryptFileComplete = true;
            }

            catch (Exception ex)
            {
                DecryptFileComplete = false;
            }
        }
    }
}
