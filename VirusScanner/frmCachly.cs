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
    public partial class frmCachly : Form
    {
        public frmCachly(string path, string virusName)
        {
            InitializeComponent();
            _path = path;
            _virusName = virusName;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }
#region worker
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(EncryptFileComplete)
            {
                Database.CachLyRow row = App.DB.CachLy.NewCachLyRow();
                row.VirusName = _virusName;
                row.Path = _path;
                row.DateTime = DateTime.Now;
                App.DB.CachLy.AddCachLyRow(row);
                App.DB.AcceptChanges();
                App.DB.WriteXml(string.Format("{0}/.dat", Application.StartupPath));
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(row.VirusName);
                item.SubItems.Add(row.Path);
                item.SubItems.Add(row.DateTime.ToLongDateString());
                quarantine.listView.Items.Add(item);
                MyMessageBox.ShowMessage("Đã cách ly tệp tin thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MyMessageBox.ShowMessage("Đã có lỗi trong trong quá trình ly !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Encrypt(_path);
        }

        BackgroundWorker worker = new BackgroundWorker();
#endregion
        string _path;
        string _virusName;
        private bool encryptFileComplete = false;
        Quarantine quarantine = new Quarantine();
        public bool EncryptFileComplete { get => encryptFileComplete; set => encryptFileComplete = value; }

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
            labelCachly.Text = "Đang cách ly...";
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
        private void Encrypt(string inputFile)
        {
            try
            {

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(@"password");

                string cryptFile =Application.StartupPath+ "\\Quarantine\\"+ Path.GetFileName(inputFile) + ".VScan";
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
                File.Delete(inputFile);
                EncryptFileComplete = true;
            }

            catch (Exception ex)
            {
                EncryptFileComplete = false;
            }
        }
    }
}
