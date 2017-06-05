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

namespace VirusScanner
{
    public partial class frmXoaFile : Form
    {
        public frmXoaFile(string path)
        {
            InitializeComponent();
            _path = path;
        }
        public frmXoaFile(List<string> dsPath, List<string>dsVirusName, string path) :this(path)
        {
            _dsPath = dsPath;
            _dsVirusName = dsVirusName;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }
        #region Worker
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(DsError.Count==0)
            {
                deleteAllFilesComplete = true;
                MyMessageBox.ShowMessage("Đã xóa các tệp tin thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MyMessageBox.ShowMessage("Có "+DsError.Count.ToString()+" tệp không xóa được !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (string item in _dsPath)
            {
                try
                {
                    File.Delete(item);
                }
                catch (Exception)
                {
                    DeleteAllFilesComplete = false;
                    DsError.Add(item);
                    DsFileNameError.Add(Path.GetFileName(item));
                    DsVirusError.Add(_dsVirusName[_dsPath.IndexOf(item)]);
                }
            }
        }
#endregion
        string _path;
        private bool deleteFileComplete = false;
        private bool deleteAllFilesComplete = false;
        List<string> _dsPath = new List<string>();
        List<string> _dsVirusName = new List<string>();
        List<string> dsError = new List<string>();
        List<string> dsFileNameError = new List<string>();
        List<string> dsVirusError = new List<string>();
        BackgroundWorker worker = new BackgroundWorker();
        public bool DeleteFileComplete { get => deleteFileComplete; set => deleteFileComplete = value; }
        public bool DeleteAllFilesComplete { get => deleteAllFilesComplete; set => deleteAllFilesComplete = value; }
        public List<string> DsError { get => dsError; set => dsError = value; }
        public List<string> DsFileNameError { get => dsFileNameError; set => dsFileNameError = value; }
        public List<string> DsVirusError { get => dsVirusError; set => dsVirusError = value; }

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
            if (_path != "")
            {
                try
                {
                    labelDeleting.Text = "Đang xóa file !";
                    File.Delete(_path);
                    DeleteFileComplete = true;
                    MyMessageBox.ShowMessage("Đã xóa file thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception)
                {
                    MyMessageBox.ShowMessage("Đã có lỗi trong quá trình xóa file !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DeleteFileComplete = false;
                    Close();
                }
            }
            else
            {
                labelDeleting.Text = "Đang xóa file !";
                worker.RunWorkerAsync();
            }
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
    }
}
