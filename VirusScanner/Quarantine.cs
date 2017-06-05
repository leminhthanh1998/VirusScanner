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
    public partial class Quarantine : Form
    {
        public Quarantine()
        {
            InitializeComponent();
        }

        //Load danh sach 
        private void Quarantine_Load(object sender, EventArgs e)
        {
            listView.Items.Clear();
            string path = string.Format("{0}/.dat", Application.StartupPath);
            App.DB.Clear();
            if (File.Exists(path))
                App.DB.ReadXml(path);
            foreach (Database.CachLyRow row in App.DB.CachLy)
            {
                listView.Items.Add(new ListViewItem(new string[] { row.VirusName, row.DateTime.ToLongDateString(), row.Path }));
            }
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
                string originPath = listView.Items[listView.FocusedItem.Index].SubItems[2].Text;
                string path = Application.StartupPath + "\\Quarantine\\" + Path.GetFileName(originPath)+".VScan";
                frmXoaFile deleteFile = new frmXoaFile(path);
                deleteFile.ShowDialog();
                if (deleteFile.DeleteFileComplete)
                {
                    for (int i = listView.SelectedItems.Count; i > 0; i--)
                    {
                        ListViewItem item = listView.SelectedItems[i - 1];
                        App.DB.CachLy.Rows[item.Index].Delete();
                        listView.Items[item.Index].Remove();
                    }
                    App.DB.AcceptChanges();
                    App.DB.WriteXml(string.Format("{0}/.dat", Application.StartupPath));
                }
            }
            else MyMessageBox.ShowMessage("Bạn chưa chọn tệp cần xóa !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 0)
            {
                string originPath = listView.Items[listView.FocusedItem.Index].SubItems[2].Text;
                string path = Application.StartupPath + "\\Quarantine\\" + Path.GetFileName(originPath) + ".VScan";
                frmKhoiphuc restoreFile = new frmKhoiphuc(path, originPath);
                restoreFile.ShowDialog();
                if (restoreFile.DecryptFileComplete)
                {
                    for (int i = listView.SelectedItems.Count; i > 0; i--)
                    {
                        ListViewItem item = listView.SelectedItems[i - 1];
                        App.DB.CachLy.Rows[item.Index].Delete();
                        listView.Items[item.Index].Remove();
                    }
                    App.DB.AcceptChanges();
                    App.DB.WriteXml(string.Format("{0}/.dat", Application.StartupPath));
                }
            }
            else MyMessageBox.ShowMessage("Bạn chưa chọn tệp cần khôi phục !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
