using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ACSManager.Control
{
    public partial class InsertAP : DevExpress.XtraEditors.XtraForm
    {
        public InsertAP()
        {
            InitializeComponent();
        }

        private void InsertAP_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // 저장
            if(txtApIp.Text.Length < 1 || txtApName.Text.Length < 1)
            {
                XtraMessageBox.Show("Invalied Input Value .", "ACS.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                DataSet1MTableAdapters.tb_ap_infoTableAdapter apAdt = new DataSet1MTableAdapters.tb_ap_infoTableAdapter();
                apAdt.InsertQuery(txtApName.Text, txtApIp.Text);
                Close();
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("Save fail " + ex.Message, "ACS.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}