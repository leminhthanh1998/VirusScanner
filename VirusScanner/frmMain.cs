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

namespace VirusScanner
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            if (clamav.GetVer() != "Chưa kích hoạt ClamAV")
                labelEngine.Text = "Database: " + clamav.GetVer();
            else
            {
                labelEngine.Text = "Chưa kích hoạt ClamAV";
                MyMessageBox.ShowMessage("Bạn chưa kích hoạt ClamAV !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        ClamAvManager clamav = new ClamAvManager("127.0.0.1", 3310);

        string path;

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Quet tep tin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuickScan_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "All files (*.*)|*.*";
            open.FilterIndex = 1;
            if(open.ShowDialog()==DialogResult.OK)
            {
                path = open.FileName;
                QuetFile scanFile = new QuetFile(path);
                scanFile.ShowDialog();
            }
        }

        /// <summary>
        /// Quet thu muc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomScan_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = true;
            if(folder.ShowDialog()==DialogResult.OK)
            {
                QuetThuMuc scanfolder = new QuetThuMuc(folder.SelectedPath);
                scanfolder.ShowDialog();
            }
        }

        private void btnFullScan_Click(object sender, EventArgs e)
        {
            string[] drives = Directory.GetLogicalDrives();
            QuetThuMuc scanfolder = new QuetThuMuc(drives, "");
            scanfolder.ShowDialog();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            Quarantine quanrantine = new Quarantine();
            quanrantine.ShowDialog();
        }
    }
}
