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
    public partial class UseRateCar : DevExpress.XtraEditors.XtraUserControl
    {
        public UseRateCar()
        {
            InitializeComponent();
        }

        public void PrintGrid()
        {
            gridControl1.Refresh();
            gridControl1.ShowPrintPreview();
        }

        private void UseRateCar_Load(object sender, EventArgs e)
        {
            dateStart.DateTime = DateTime.Now.AddDays(-1);
            dateEnd.DateTime = DateTime.Now;

            var carlist = new DataSet1MTableAdapters.tb_agvTableAdapter().GetData();
            var idlist = carlist.AsEnumerable().Select(p => p.agv_id).ToList();
            
            foreach (var id in idlist)
            {
                agv_id.Properties.Items.Add(id);
            }

            ReLoadInfo();
        }

        private void ReLoadInfo()
        {
            var urhAdt = new DataSet1MTableAdapters.tb_use_rate_hourTableAdapter();
            DataSet1M.tb_use_rate_hourDataTable urhTbl = urhAdt.GetDataBy(dateStart.DateTime.ToString("yyyy/MM/dd 00:00:00").Replace("/", "-"), dateEnd.DateTime.ToString("yyyy/MM/dd 23:00:00").Replace("/", "-"));

            if (Convert.ToInt32(dateTimePicker1.Text) >= 08 && Convert.ToInt32(dateTimePicker1.Text) <= 17)
            {
                var result = urhTbl.AsEnumerable().Where(p => p.title.Contains($" {dateTimePicker1.Text}"));
                var tb = result.Any() ? result.CopyToDataTable() : urhTbl.Clone();

                gridControl1.DataSource = tb;
            }
            else
            {
                gridControl1.DataSource = urhTbl;
            }

            // 20190315 수정 - 최고 가동률 표시
            DataSet1MTableAdapters.tb_peak_rateTableAdapter pradt = new DataSet1MTableAdapters.tb_peak_rateTableAdapter();

            string today = DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "-");
            int peakRate = (int)pradt.GetTodayPeakRate(today);

            labelPeakRate.Text = $"{today} Peak Rate: {peakRate}%";
            // ----------
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dateStart.DateTime > dateEnd.DateTime)
            {
                MessageBox.Show("Wrong serch condition ! ");
                return;
            }

            agv_id.Text = "";

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

        private void agv_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var urhAdt = new DataSet1MTableAdapters.tb_use_rate_hourTableAdapter();
                DataSet1M.tb_use_rate_hourDataTable table = urhAdt.GetDataBy(dateStart.DateTime.ToString("yyyy/MM/dd 00:00:00").Replace("/", "-"), dateEnd.DateTime.ToString("yyyy/MM/dd 23:00:00").Replace("/", "-"));
                
                var result = table.AsEnumerable().Where(p => p.agv_id == agv_id.Text);
                var tb = result.Any() ? result.CopyToDataTable() : table.Clone();

                gridControl1.DataSource = tb;
                gridView1.BestFitColumns();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }
    }
}
