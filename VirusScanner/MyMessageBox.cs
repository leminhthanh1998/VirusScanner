using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusScanner
{
    class MyMessageBox
    {
        public static System.Windows.Forms.DialogResult ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxButtons button, System.Windows.Forms.MessageBoxIcon icon)
        {
            System.Windows.Forms.DialogResult dlgResult = System.Windows.Forms.DialogResult.None;
            if (button == System.Windows.Forms.MessageBoxButtons.OK)
            {
                using (ThongBao frmOk = new ThongBao())
                {
                    frmOk.Title = caption;
                    frmOk.Message = message;
                    switch (icon)
                    {
                        case System.Windows.Forms.MessageBoxIcon.Information:
                            frmOk.MessageIcon = Properties.Resources.Information_96;
                            break;
                        case System.Windows.Forms.MessageBoxIcon.Error:
                            frmOk.MessageIcon = Properties.Resources.Error_96;
                            break;
                        case System.Windows.Forms.MessageBoxIcon.Question:
                            frmOk.MessageIcon = Properties.Resources.Ask_Question_96;
                            break;
                    }
                    dlgResult = frmOk.ShowDialog();
                }
            }
            
            return dlgResult;
        }
    }
}
