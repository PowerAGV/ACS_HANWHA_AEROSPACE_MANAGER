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

namespace ACSManager.FORM
{
    public partial class UserControl : DevExpress.XtraEditors.XtraForm
    {
        public UserControl()
        {
            InitializeComponent();
        }

        void RepladUser()
        {
            try
            {
                DataSet1MTableAdapters.tb_userTableAdapter adtUsr = new DataSet1MTableAdapters.tb_userTableAdapter();
                DataSet1M.tb_userDataTable dtbl = adtUsr.GetData();
                gridControl1.DataSource = dtbl;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text.Length < 3)
                {
                    XtraMessageBox.Show("Please ID Check", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtPass.Text.Length < 3)
                {
                    XtraMessageBox.Show("Please PASSWORD Check", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataSet1MTableAdapters.tb_userTableAdapter adtUsr = new DataSet1MTableAdapters.tb_userTableAdapter();
                long findCon = (long)adtUsr.ScalarQuery(txtID.Text);
                if (findCon != 0)
                {
                    XtraMessageBox.Show("Alady exist ID", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                adtUsr.InsertQuery(txtID.Text, txtPass.Text, cmbType.Text);
                XtraMessageBox.Show("Process OK!", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Text = "";
                txtPass.Text = "";
                cmbType.SelectedIndex = 0;
                RepladUser();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

        }

        private void UserControl_Load(object sender, EventArgs e)
        {
            RepladUser();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {// delete 
            try
            {
                if (txtID.Text.Length < 3)
                {
                    XtraMessageBox.Show("Please Select User", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                DataSet1MTableAdapters.tb_userTableAdapter adtUsr = new DataSet1MTableAdapters.tb_userTableAdapter();
                long findCon = (long)adtUsr.ScalarQuery(txtID.Text);
                if (findCon == 0)
                {
                    XtraMessageBox.Show("ID is not exist", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (XtraMessageBox.Show("Are you sure you want to delete account?", "AGV Manager", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {

                    adtUsr.DeleteQuery(txtID.Text);
                    XtraMessageBox.Show("Process OK!", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                RepladUser();
                txtID.Text = "";
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            object selectObj = gridView1.GetRow(gridView1.FocusedRowHandle);
            string sId = (((DataRowView)selectObj)[1].ToString());
            txtID.Text = sId;
        }
    }
}