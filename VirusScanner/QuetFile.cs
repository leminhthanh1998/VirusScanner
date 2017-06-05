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
using System.Threading;
using System.Diagnostics;

namespace VirusScanner
{
    public partial class QuetFile : Form
    {
        public QuetFile(string path)
        {
            InitializeComponent();
            _path = path;
            worker1.DoWork += Worker1_DoWork;
            worker1.RunWorkerCompleted += Worker1_RunWorkerCompleted;
            btnDelete.Enabled =  btnCachly.Enabled = false;
        }
        #region Worker
        private void Worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            watch.Stop();
            progressBar.animated = false;
            labelTime.Text ="Thời gian: " +watch.Elapsed.ToString("mm\\:ss\\.f");
            if (clamav.IsVirus)
            {
                labelTenVirus.Text = "1 - " + clamav.NameVirus;
                labelThongBao.Text = "Đã phát hiện virus !";
                btnDelete.Enabled = btnCachly.Enabled = true;
            }
            else labelThongBao.Text = "Tệp tin an toàn !";
        }

        private void Worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar.animated = true;
            clamav.Scan(_path);
        }

        BackgroundWorker worker1 = new BackgroundWorker();
#endregion
        string _path;
        ClamAvManager clamav = new ClamAvManager("127.0.0.1", 3310);
        Stopwatch watch = new Stopwatch();
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Lay thong tin file quet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuetFile_Load(object sender, EventArgs e)
        {
            labelFiledaquet.Text = _path;
            watch.Start();
            worker1.RunWorkerAsync();
        }

        /// <summary>
        /// Xoa file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            frmXoaFile deleteFile = new frmXoaFile(_path);
            deleteFile.ShowDialog();
            if(deleteFile.DeleteFileComplete)
            {
                Close();
            }
        }

        /// <summary>
        /// Huy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCachly_Click(object sender, EventArgs e)
        {
            frmCachly cachLyFile = new frmCachly(_path, clamav.NameVirus);
            cachLyFile.ShowDialog();
            if(cachLyFile.EncryptFileComplete)
            {
                Close();
            }
        }
    }
}
