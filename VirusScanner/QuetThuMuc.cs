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
using System.Diagnostics;

namespace VirusScanner
{
    public partial class QuetThuMuc : Form
    {
        public QuetThuMuc(string path)
        {
            InitializeComponent();
            _path = path;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }
        public QuetThuMuc(string[] localDisk, string path):this(path)
        {
            _localDisk = new string[localDisk.Length];
            _localDisk = localDisk;
            worker2.DoWork += Worker2_DoWork;
            worker2.RunWorkerCompleted += Worker2_RunWorkerCompleted;
        }


        #region Worker
        private void Worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            watch.Stop();
            progressBar.animated = false;
            if (DsVirusName.Count > 0)
            {
                DetectVirus detect = new DetectVirus(watch, DsVirusName, DsPathVirus, DsFileName);
                detect.ShowDialog();
                Close();
            }
            else { MyMessageBox.ShowMessage("Không tìm thấy virus !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); Close(); }
        }

        private void Worker2_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (string item in _localDisk)
            {
                ScanFolder(item);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            watch.Stop();
            progressBar.animated = false;
            if (DsVirusName.Count > 0)
            {
                DetectVirus detect = new DetectVirus(watch, DsVirusName, DsPathVirus, DsFileName);
                detect.ShowDialog();
                Close();
            }
            else { MyMessageBox.ShowMessage("Không tìm thấy virus !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); Close(); }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ScanFolder(_path);
        }
        BackgroundWorker worker = new BackgroundWorker();
        BackgroundWorker worker2 = new BackgroundWorker();
#endregion
        string _path;
        int dem = 0;
        long dem2 = 0;
        string[] _localDisk;
        private List<string> dsVirusName = new List<string>();
        private List<string> dsPathVirus = new List<string>();
        private List<string> dsFileName = new List<string>();
        ClamAvManager clamav = new ClamAvManager("127.0.0.1", 3310);
        Stopwatch watch = new Stopwatch();
        public List<string> DsVirusName { get => dsVirusName; set => dsVirusName = value; }
        public List<string> DsPathVirus { get => dsPathVirus; set => dsPathVirus = value; }
        public List<string> DsFileName { get => dsFileName; set => dsFileName = value; }

        private void ScanFolder(string location)
        {
            string[] files = Directory.GetFiles(location);
            string[] folder = Directory.GetDirectories(location);
            for (int i = 0; i < files.Length; i++)
            {
                labelFile.Invoke(new MethodInvoker(delegate { labelFile.Text = files[i]; }));
                clamav.Scan(files[i]);
                if(clamav.IsVirus)
                {
                    DsVirusName.Add(clamav.NameVirus);
                    DsPathVirus.Add(files[i]);
                    DsFileName.Add(Path.GetFileName(files[i]));
                    dem++;
                }
                labelDetect.Invoke(new MethodInvoker(delegate { labelDetect.Text = (dem).ToString(); }));
                labelFiledaquet.Invoke(new MethodInvoker(delegate { labelFiledaquet.Text = (dem2++).ToString(); }));
            }
            for (int i = 0; i < folder.Length; i++)
            {
                try
                {
                    ScanFolder(folder[i]);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        private void QuetThuMuc_Load(object sender, EventArgs e)
        {
            progressBar.animated = true;
            watch.Start();
            if (_path != "")
                worker.RunWorkerAsync();
            else worker2.RunWorkerAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
