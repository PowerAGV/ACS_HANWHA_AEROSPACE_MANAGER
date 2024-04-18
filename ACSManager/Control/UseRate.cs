using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace ACSManager.Control
{
    public partial class UseRate : DevExpress.XtraEditors.XtraUserControl
    {
        public UseRate()
        {
            InitializeComponent();
        }

        public void PrintGrid()
        {
            gridControl1.Refresh();
            gridControl1.ShowPrintPreview();
        }

        private void UseRate_Load(object sender, EventArgs e)
        {
            dateStart.DateTime = DateTime.Now.AddDays(-1);
            dateEnd.DateTime = DateTime.Now;

            ReLoadInfo();
        }

        private void ReLoadInfo()
        {
            try
            {
                DataSet1MTableAdapters.tb_use_rateTableAdapter ucadt = new DataSet1MTableAdapters.tb_use_rateTableAdapter();
                //string tss = dateStart.DateTime.ToString("yyyy/MM/dd");
                DataSet1M.tb_use_rateDataTable urtbl2 = ucadt.GetDataBy(dateStart.DateTime.ToString("yyyy/MM/dd").Replace("-", "/"), dateEnd.DateTime.AddDays(1).ToString("yyyy/MM/dd").Replace("-", "/"));

                gridControl1.DataSource = urtbl2;
            }
            catch (Exception)
            {

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dateStart.DateTime > dateEnd.DateTime)
            {
                MessageBox.Show("Wrong serch condition ! ");
                return;
            }

            ReLoadInfo();
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (e.Column.FieldName == "total_rate" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    int l_r = int.Parse(row["load_rate"].ToString());
                    int e_r = int.Parse(row["empty_rate"].ToString());
                    int w_r = int.Parse(row["load_rate"].ToString());
                    int a_r = int.Parse(row["load_rate"].ToString());
                    int t_r = l_r + e_r;
                    e.Value = t_r.ToString();                    
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }
    }
}
