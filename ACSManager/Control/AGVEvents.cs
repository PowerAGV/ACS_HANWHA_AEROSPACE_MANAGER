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
using DevExpress.XtraPrinting;

namespace ACSManager.Control
{
    public partial class AGVEvents : DevExpress.XtraEditors.XtraUserControl
    {
        private DataTable sourceTable = new DataTable();
        private int last_count = 0;
        private Timer timer = new Timer();

        #region Constructor, Load
        public AGVEvents()
        {
            InitializeComponent();
        }

        private void AGVEvents_Load(object sender, EventArgs e)
        {
            dateStart.DateTime = DateTime.Now;
            dateEnd.DateTime = DateTime.Now;

            
            var carlist = new DataSet1MTableAdapters.tb_agvTableAdapter().GetData();
            var idlist = carlist.AsEnumerable().Select(p => p.agv_id).ToList();

            //idlist.ForEach(p => agv_id.Properties.Items.Add(p));
            foreach (var id in idlist)
            {
                agv_id.Properties.Items.Add(id);
            }
            

            ReLoadInfo();
        }
        #endregion

        #region Event Function
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            if (dateStart.DateTime > dateEnd.DateTime)
            {
                MessageBox.Show("Wrong serch condition ! ");
                return;
            }

            agv_id.Text = "";

            ReLoadInfo();
        }

        private void txteditAGVID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                ReLoadInfo();
            }
        }

        
        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            // v_dis
            try
            {
                
                GridView view = sender as GridView;
                if (e.Column.FieldName == "v_dis" && e.IsGetData) {


                    //DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    DataRow row = view.GetDataRow(e.ListSourceRowIndex);
                    //string ft = row["param"].ToString().Length >= 2 ? row["param"].ToString().Substring(0, 2) : row["param"].ToString();

                    e.Value = ChangeCodeToMessage(view.GetRowCellValue(e.ListSourceRowIndex, "param").ToString());
                    //e.Column.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                    //e.Column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;//  ColumnSortOrder.Ascending;
                }

            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }
        #endregion

        public static Dictionary<string, string> error_code = new Dictionary<string, string>();

        #region Function
        private void ReLoadInfo()
        {
            try
            {
                error_code.Clear();

                DataSet1MTableAdapters.tb_comm_codeTableAdapter cAdtp = new DataSet1MTableAdapters.tb_comm_codeTableAdapter();
                foreach (DataSet1M.tb_comm_codeRow crw in cAdtp.GetDataBy("AGV_Err"))
                {
                    if (!error_code.ContainsKey(crw.comm_code))
                    {
                        error_code.Add(crw.comm_code, crw.comm_name);
                    }
                }


                DateTime sdate = new DateTime(int.Parse(dateStart.DateTime.ToString("yyyy")), int.Parse(dateStart.DateTime.ToString("MM")), int.Parse(dateStart.DateTime.ToString("dd")));
                DateTime edate = new DateTime(int.Parse(dateEnd.DateTime.AddDays(1).ToString("yyyy")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("MM")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("dd")));

                DataSet1MTableAdapters.tb_avg_eventTableAdapter aedt = new DataSet1MTableAdapters.tb_avg_eventTableAdapter();
                var table = aedt.GetDataBy(sdate, edate);

                
                var result = table.AsEnumerable();
                var tb = result.Any() ? result.CopyToDataTable() : table.Clone();
                sourceTable = tb;
                

                if (sourceTable != null && sourceTable.Rows.Count > 0)
                    InsertNumber(ref sourceTable);

                // 알람 코드 DB를 이용 하여 변경
                foreach (DataRow row in sourceTable.Rows)
                {
                    if(row["param"].ToString().Length > 1)
                    {// error_code
                        string code = row["param"].ToString().Substring(0, 2);
                        if (error_code.ContainsKey(code))
                            row["param_nm"] = error_code[code];
                    }
                }


                gridControl1.DataSource = sourceTable;
                //gridView1.BestFitColumns();
                
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }



        public static string ChangeCodeToMessage(string cmd)
        {

            string message = "";

            if (error_code.ContainsKey(cmd))
            {
                message = error_code[cmd];
            }
            else
            {
                switch (cmd) {
                    case "32":
                        message = "비상정지";
                        break;
                    case "33":
                        message = "라인이탈";
                        break;
                    case "34":
                        message = "범퍼충돌";
                        break;
                    case "35":
                        message = "전진 주행중 장애물 감지";
                        break;
                    case "36":
                        message = "후진 주행중 장애물 감지";
                        break;
                    case "37":
                        message = "좌회전 중 장애물 감지";
                        break;
                    case "38":
                        message = "우회전 중 장애물 감지";
                        break;
                    case "39":
                        message = "경로 이탈";
                        break;
                    case "3B":
                        message = "회전 중 라인 이탈";
                        break;
                    case "3D":
                        message = "우측 주행 모터 이상";
                        break;
                    case "3E":
                        message = "좌측 주행 모터 이상";
                        break;
                    case "41":
                        message = "리프트 모터 이상";
                        break;
                    case "42":
                        message = "팔레트 미감지";
                        break;
                    case "43":
                        message = "팔레트 감지";
                        break;
                    case "4C":
                        message = "자동충전기 비상정지";
                        break;
                    case "4D":
                        message = "충전 동작 시간 초과";
                        break;
                    case "4E":
                        message = "충전 중 충전 이상";
                        break;
                    case "50":
                        message = "브레이크 스위치 해제 이상";
                        break;
                    case "52":
                        message = "리프트 동작 시간 초과";
                        break;
                    case "65":
                        message = "우측 모터드라이버 CAN 통신 이상";
                        break;
                    case "66":
                        message = "좌측 모터드라이버 CAN 통신 이상";
                        break;
                    case "67":
                        message = "리프트 모터드라이버 CAN 통신 이상";
                        break;
                    case "6E":
                        message = "컨베이어 비상정지";
                        break;
                    case "6F":
                        message = "컨베이어 동작 시간 초과";
                        break;
                    default:
                        message = "미등록 메시지";
                        break;
                }
            }
            return message;
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


        public void PrintGrid()
        {
            if (gridControl1.IsPrintingAvailable)
                gridControl1.ShowPrintPreview();
        }


        #endregion


        private void agv_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime sdate = new DateTime(int.Parse(dateStart.DateTime.ToString("yyyy")), int.Parse(dateStart.DateTime.ToString("MM")), int.Parse(dateStart.DateTime.ToString("dd")));
                DateTime edate = new DateTime(int.Parse(dateEnd.DateTime.AddDays(1).ToString("yyyy")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("MM")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("dd")));

                DataSet1MTableAdapters.tb_avg_eventTableAdapter aedt = new DataSet1MTableAdapters.tb_avg_eventTableAdapter();

                var table = aedt.GetDataBy(sdate, edate);

                
                var result = table.AsEnumerable().Where(p => p.avg_id == agv_id.Text);
                var tb = result.Any() ? result.CopyToDataTable() : table.Clone();
                sourceTable = tb;
                

                if (sourceTable != null && sourceTable.Rows.Count > 0)
                {
                    InsertNumber(ref sourceTable);
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

            gridControl1.DataSource = sourceTable;
            gridView1.BestFitColumns();
        }
    }
}
