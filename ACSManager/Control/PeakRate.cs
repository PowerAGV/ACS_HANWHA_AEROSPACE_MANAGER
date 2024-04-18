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
    public partial class PeakRate : DevExpress.XtraEditors.XtraUserControl
    {
        public PeakRate()
        {
            InitializeComponent();
        }

        public void PrintGrid()
        {
            gridControl1.Refresh();
            gridControl1.ShowPrintPreview();
        }

        private void PeakRate_Load(object sender, EventArgs e)
        {
            dateStart.DateTime = DateTime.Now.AddDays(-1);
            dateEnd.DateTime = DateTime.Now;

            ReLoadInfo();
        }

        private void ReLoadInfo()
        {
            try
            {
                string sDate = dateStart.DateTime.ToString("yyyy/MM/dd").Replace("/", "-");
                string eDate = dateEnd.DateTime.ToString("yyyy/MM/dd").Replace("/", "-");

                DataSet1MTableAdapters.tb_peak_rateTableAdapter pradt = new DataSet1MTableAdapters.tb_peak_rateTableAdapter();
                DataSet1M.tb_peak_rateDataTable prtbl = pradt.GetPeakRate(sDate, eDate);
                
                gridControl1.DataSource = prtbl;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} - {e.StackTrace}");
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
                
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }
    }
}
