using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirusScanner
{
    public partial class ThongBao : Form
    {
        public ThongBao()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public Image MessageIcon
        {
            get { return pictureBox.Image; }
            set { pictureBox.Image = value; }
        }
        public string Message
        {
            get { return labelMessage.Text; }
            set { labelMessage.Text = value; }
        }
        public string Title
        {
            get { return labelTieude.Text; }
            set { labelTieude.Text = value; }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
