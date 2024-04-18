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
    public partial class CommunicationStatus : DevExpress.XtraEditors.XtraUserControl
    {
        Color bk1 = Color.Black;
        Color bk2 = Color.Black;

        private DataTable sourceTable = null;

        #region Constructor, Load
        public CommunicationStatus()
        {
            InitializeComponent();
        }

        private void CommunicationStatus_Load(object sender, EventArgs e)
        {
            RefrashDatas();

            if (Program.USER_DATA.UserGroup == "MANAGER" || Program.USER_DATA.UserGroup == "MASTER")
            {
            }
            else
            {
            }
        }
        #endregion

        #region Event Function
        private void CommunicationStatus_Enter(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void CommunicationStatus_Leave(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task mytask2 = Task.Run(() =>
            {
                threadRefrashAGVList();
            });

            gridView2.BestFitColumns();
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                if (bk1 == Color.Black)
                {
                    bk1 = e.Appearance.BackColor;
                    bk2 = e.Appearance.BackColor2;
                }

                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["comm_result"]);
                if (category == "Error")
                {
                    if (e.Column.FieldName == "agv_id")
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.BackColor2 = Color.DarkRed;
                    }
                }
                else
                {
                    if (e.Column.FieldName == "agv_id")
                    {
                        e.Appearance.BackColor = bk1;
                        e.Appearance.BackColor2 = bk2;
                    }
                }
            }
        }
        #endregion

        #region Function
        void RefrashDatas()
        {
            try
            {
                DataSet1MTableAdapters.tb_agvTableAdapter agvAdp = new DataSet1MTableAdapters.tb_agvTableAdapter();
                var tbAGVip = agvAdp.GetData();

                
                var result = tbAGVip.AsEnumerable();
                var tb = result.Any() ? result.CopyToDataTable() : tbAGVip.Clone();
                sourceTable = tb;
                

                //추가 필터 넣으면 됨

                if (sourceTable != null && sourceTable.Rows.Count > 0)
                {
                    InsertNumber(ref sourceTable);
                }
                
                sourceTable.Columns.Add("comm_result", typeof(string));
                sourceTable.Columns.Add("reg_date", typeof(DateTime));
                sourceTable.Columns.Add("response_time", typeof(int));
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

            gridControl2.DataSource = sourceTable;
            gridView2.BestFitColumns();
        }


        int ChangeGrid1CellAGV(int rCnt, string fildname,string fldnm,string fldnm2, string value,string v2,string v3)
        {
            //string gVal = gridView2.GetRowCellValue(rCnt, fildname) == null ? "" : gridView2.GetRowCellValue(rCnt, fildname).ToString();
            //if (gVal != value)
            {
                if (gridControl2.InvokeRequired)
                {
                    gridControl2.Invoke(new MethodInvoker(delegate ()
                    {
                        gridView2.SetRowCellValue(rCnt, fildname, value);
                        gridView2.SetRowCellValue(rCnt, fldnm, v2);
                        gridView2.SetRowCellValue(rCnt, fldnm2, v3);
                    }));
                }
                else
                {
                    gridView2.SetRowCellValue(rCnt, fildname, value);
                }

                return 1; // 변경 된 내용이 있다면 증가하도록 함.
            }
        }

        void threadRefrashAGVList()
        {
            try
            {
                // 그리드를 돌면서 agv 이름을 가져오고 그 이름으로 히스토리의 값을 가져 온다.
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    string agvid = gridView2.GetRowCellValue(i, "agv_id").ToString();
                    string response_time = gridView2.GetRowCellValue(i, "response_time") == null ? "" : gridView2.GetRowCellValue(i, "response_time").ToString();
                    string comm_result = gridView2.GetRowCellValue(i, "comm_result") == null ? "" : gridView2.GetRowCellValue(i, "comm_result").ToString();
                    if (comm_result == "Error")
                        comm_result = "0";
                    else
                        comm_result = "1";

                    DataSet1MTableAdapters.tb_agv_comm_historyTableAdapter hisAgvAdp = new DataSet1MTableAdapters.tb_agv_comm_historyTableAdapter();
                    DataSet1M.tb_agv_comm_historyDataTable tblAgvHis = hisAgvAdp.GetDataByAGVID(agvid);
                    foreach (DataSet1M.tb_agv_comm_historyRow ritem in tblAgvHis)
                    {
                        if (response_time != ritem.reg_date.ToString("yyyy/MM/dd hh:mm:dd") || comm_result != ritem.comm_result.ToString() || response_time != ritem.response_time.ToString())
                        {// 하나라도 다르면 삽입
                            ChangeGrid1CellAGV(i, "reg_date", "response_time", "comm_result", ritem.reg_date.ToString("yyyy/MM/dd hh:mm:ss"), ritem.response_time.ToString(), ritem.comm_result == 1 ? "Normal" : "Error");
                        }
                        break;//TODO 확인 필요
                    }
                }
            }
            catch (Exception ex)
            {

                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }

        private void InsertNumber(ref DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!dt.Columns.Contains("no"))
                    dt.Columns.Add(new DataColumn("no", typeof(int)));


                int index = 1;

                foreach (DataRow row in dt.Rows)
                {
                    row["no"] = index;
                    index++;
                }
            }
        }
        #endregion
    }
}
