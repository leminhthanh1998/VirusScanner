using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace VirusScanner
{
    public partial class DetectVirus : Form
    {
        public DetectVirus(Stopwatch watch, List<string> dsVirusName, List<string> dsPath, List<string>dsFileName)
        {
            InitializeComponent();
            _watch = watch;
            _dsVirusName = dsVirusName;
            _dsPath = dsPath;
            _dsFileName = dsFileName;
        }
        Stopwatch _watch = new Stopwatch();
        List<string> _dsVirusName = new List<string>();
        List<string> _dsPath = new List<string>();
        List<string> _dsFileName = new List<string>();

        /// <summary>
        /// Load listview danh sach virus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetectVirus_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < _dsVirusName.Count; i++)
            {
                listView.Items.Add(new ListViewItem(new string[] { _dsVirusName[i], _dsFileName[i], _dsPath[i] }));
            }
            labelTime.Text= "Thời gian: " + _watch.Elapsed.ToString("mm\\:ss\\.f");
            labelDetectFile.Text = "Số tệp được phát hiện: " + _dsFileName.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Xoa file duoc chon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 0)
            {
                frmXoaFile deleteFile = new frmXoaFile(_dsPath[listView.FocusedItem.Index]);
                deleteFile.ShowDialog();
                if (deleteFile.DeleteFileComplete)
                {
                    _dsPath.RemoveAt(listView.FocusedItem.Index);
                    for (int i = listView.SelectedItems.Count; i > 0; i--)
                    {
                        ListViewItem item = listView.SelectedItems[i - 1];
                        listView.Items[item.Index].Remove();
                    }
                }
            }
            else MyMessageBox.ShowMessage("Bạn chưa chọn tệp cần xóa !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Xoa tat ca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            frmXoaFile deleteFiles = new frmXoaFile(_dsPath,_dsVirusName, "");
            deleteFiles.ShowDialog();
            if (deleteFiles.DeleteAllFilesComplete)
                Close();
            else
            {
                listView.Items.Clear();
                for (int i = 0; i < deleteFiles.DsError.Count; i++)
                {
                    listView.Items.Add(new ListViewItem(new string[] { deleteFiles.DsVirusError[i], deleteFiles.DsFileNameError[i], deleteFiles.DsError[i] }));
                }
            }
        }

        private void btnCachly_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 0)
            {
                frmCachly deleteFile = new frmCachly(_dsPath[listView.FocusedItem.Index], _dsVirusName[listView.FocusedItem.Index]);
                deleteFile.ShowDialog();
                if (deleteFile.EncryptFileComplete)
                {
                    _dsPath.RemoveAt(listView.FocusedItem.Index); _dsVirusName.RemoveAt(listView.FocusedItem.Index);
                    for (int i = listView.SelectedItems.Count; i > 0; i--)
                    {
                        ListViewItem item = listView.SelectedItems[i - 1];
                        listView.Items[item.Index].Remove();
                    }
                }
            }
            else MyMessageBox.ShowMessage("Bạn chưa chọn tệp cần cách ly !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
