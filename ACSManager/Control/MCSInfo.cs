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
using ACSManager.FORM;
using ACSManager.data.MSSQL.MapDesignTableAdapters;

namespace ACSManager.Control
{
    public partial class MCSInfo : DevExpress.XtraEditors.XtraUserControl
    {

        private DataTable sourceTable = null;

        private Timer timer = new Timer();
        private int last_count = 0;
        private string route_area = "ALL";

        public DataFilterEventHandler DataFilterEvent;
        Dictionary<string, string> m_locname = new Dictionary<string, string>();
        #region Constructor, Load
        public MCSInfo()
        {
            InitializeComponent();
            //DataSet1MTableAdapters.m_map_linkTableAdapter adt = new DataSet1MTableAdapters.m_map_linkTableAdapter();
            //var table = adt.GetDataByMapID(742);

            //foreach (var item in table)
            //{
            //    int a = int.Parse(item.strbeginitemtag.Replace("NODE",""));
            //}
            Node_Extension_JoinTableAdapter nodJoinExtAdt = new Node_Extension_JoinTableAdapter();
            var nodeExtensionTable = nodJoinExtAdt.GetNodeExtension(Setting.MAP_ID, "", "AName");

            foreach (data.MSSQL.MapDesign.Node_Extension_JoinRow nex in nodeExtensionTable)
            {
                m_locname.Add(nex.NODE_NAME, nex.VALUE);
            }


        }

        private void MCSInfo_Load(object sender, EventArgs e)
        {
            dateStart.DateTime = DateTime.Now;
            dateEnd.DateTime = DateTime.Now;

            route_area = Program.main_process.barEditItem1.EditValue.ToString();
            ReLoadInfo();

            gridView1.BestFitColumns();
            if (Program.USER_DATA.UserGroup == "MANAGER" || Program.USER_DATA.UserGroup == "MASTER")
            {
            }
            else
            {
                btn_complete.Enabled = false;
                btn_emergency.Enabled = false;
            }
            //last_count = sourceTable.AsEnumerable().Count();

            //타이머 안씀
            timer.Interval = 3000;
            timer.Tick += delegate
            {
                try
                {
                    //DateTime sdate = new DateTime(int.Parse(dateStart.DateTime.ToString("yyyy")), int.Parse(dateStart.DateTime.ToString("MM")), int.Parse(dateStart.DateTime.ToString("dd")));
                    //DateTime edate = new DateTime(int.Parse(dateEnd.DateTime.AddDays(1).ToString("yyyy")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("MM")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("dd")));

                    int count;

                    count = (int)new DataSet1MTableAdapters.tb_mcs_commandTableAdapter().GetRowCount(dateStart.DateTime, dateEnd.DateTime, "1%");

                    if (count != last_count)
                    {
                        Program.main_process.ShowAlert("Received new command");
                        last_count = (int)count;
                        simpleButton1.PerformClick();
                    }
                }
                catch (Exception)
                {
                }
            };

            Program.main_process.DataFilterEvent += new DataFilterEventHandler(ChangedArea);

            //timer.Enabled = true;
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
            ReLoadInfo();
        }

        private void btnMCSEMR_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_MCSID.Text.Length < 2)
                {
                    XtraMessageBox.Show("Invalid W/O No.", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (DialogResult.OK == XtraMessageBox.Show("Do you want Job Cancel?", "MCS Work Order", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    string result = MCSDB.AGVEmergency(txt_MCSID.Text);
                    XtraMessageBox.Show($"Job Canceled\n Job : {result}", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                ReLoadInfo();
            }
            catch (Exception)
            {
            }
        }

        private void btnMCSCOMPLET_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_MCSID.Text.Length < 2)
                {
                    XtraMessageBox.Show("Invalid W/O No.", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (DialogResult.OK == XtraMessageBox.Show("Do you want Job Complete? ", "MCS Work Order", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    MCSReturnValue result = MCSDB.AGVArrived(txt_MCSID.Text);
                    XtraMessageBox.Show($"Job Completed\n Job : {result}", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                ReLoadInfo();
            }
            catch (Exception)
            {
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                //현재 선택된 ROW의 ORDER_ID를 획득
                if (txt_MCSID.Text.Length < 2)
                {
                    XtraMessageBox.Show("Invalid W/O No.", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (DialogResult.OK == XtraMessageBox.Show("Do you want Job Reset? ", "MCS Work Order", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    //DB 업데이트
                    DataSet1MTableAdapters.tb_mcs_commandTableAdapter adapter = new DataSet1MTableAdapters.tb_mcs_commandTableAdapter();
                    adapter.UpdateStatus((int)MCSDB.JosStatus.wait, txt_MCSID.Text);
                    XtraMessageBox.Show($"Job Reset\n Job : {txt_MCSID.Text}", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //재검색
                ReLoadInfo();
            }
            catch (Exception)
            {
            }
        }

        private void gridView1_CustomUnboundColumnData_1(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                DataTable dt = new DataTable();

                if (e.Column.FieldName == "txn_date" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["TXN_DATE"].ToString();
                    try
                    {
                        e.Value = DateTime.ParseExact(ft, "yyyyMMddHHmmss", null);
                    }
                    catch (Exception b)
                    {

                    }
                }


                if (e.Column.FieldName == "status" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["STATUS"].ToString();
                    if (ft == "0")
                        e.Value = "대기중"; //"Waiting"
                    else if (ft == "1")
                        e.Value = "예약됨";// Reserved
                    else if (ft == "2")
                        e.Value = "작업시작"; //Departured
                    else if (ft == "3")
                        e.Value = "작업완료"; //Completed
                    else if (ft == "4")
                        e.Value = "작업취소"; //MCS Cancel
                    else if (ft == "5")
                        e.Value = "작업 재요청";// REQ Start
                    else if (ft == "6")
                        e.Value = "작업취소"; // ACS Cancel
                    else
                        e.Value = "None";
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            ChangedRow();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ChangedRow();
        }
        #endregion

        #region Function
        private void ReLoadInfo()
        {
            DateTime sdate = new DateTime(int.Parse(dateStart.DateTime.ToString("yyyy")), int.Parse(dateStart.DateTime.ToString("MM")), int.Parse(dateStart.DateTime.ToString("dd")));
            DateTime edate = new DateTime(int.Parse(dateEnd.DateTime.AddDays(1).ToString("yyyy")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("MM")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("dd")));

            try
            {
                DataSet1MTableAdapters.tb_mcs_commandTableAdapter radt = new DataSet1MTableAdapters.tb_mcs_commandTableAdapter();
                DataSet1M.tb_mcs_commandDataTable tbmcs = null;
                if (route_area != "ALL")
                    tbmcs = radt.GetDatabyMapid(sdate, edate, route_area);
                else
                    tbmcs = radt.GetDataByMapIDALL(sdate, edate);


                var resulrt = tbmcs.AsEnumerable();
                var tb = resulrt.Any() ? resulrt.CopyToDataTable() : tbmcs.Clone();
                sourceTable = tb;


                //foreach(DataRow row in sourceTable.Rows)
                //{
                //    row["ORDER_ID"] = row["ORDER_ID"].ToString().Split('-')[1];
                //}
                //추가 필터 넣으면 됨
                InsertNumber(ref sourceTable);

                gridControl1.DataSource = sourceTable;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
            }

            gridView1.BestFitColumns();
        }

        private void InsertNumber(ref DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!dt.Columns.Contains("no"))
                    dt.Columns.Add(new DataColumn("no", typeof(int)));

                int index = dt.Rows.Count;




                foreach (DataRow row in dt.Rows)
                {
                    if (m_locname.ContainsKey(row["FROM_LOCATOR"].ToString()))
                        row["FROM_GUBUN"] = m_locname[row["FROM_LOCATOR"].ToString()];
                    if (m_locname.ContainsKey(row["TO_LOCATOR"].ToString()))
                        row["TO_GUBUN"] = m_locname[row["TO_LOCATOR"].ToString()];

                    row["no"] = index;
                    index--;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ChangedRow()
        {
            try
            {
                string id = "";

                if (sourceTable == null && sourceTable.Rows.Count <= 0)
                    return;

                object selectObj = gridView1.GetRow(gridView1.FocusedRowHandle);

                if (selectObj != null)
                {
                    id = (((DataRowView)selectObj)["ORDER_ID"].ToString());
                    txt_MCSID.Text = id;


                    InitTextBox();

                    var tag_result = sourceTable.AsEnumerable().Where(p => p["ORDER_ID"].ToString() == id).ToList()[0]["TAG_RESULT"]?.ToString();
                    switch (tag_result)
                    {
                        case "Tagging Error":
                            tag_result = "Cart Tag Error";
                            break;
                        case "Tag Mismatch ":
                            tag_result = "Cart Tag Mismatch";
                            break;
                        default:
                            break;
                    }



                    txtTagResult.Text = tag_result;
                    txtTagName.Text = sourceTable.AsEnumerable().Where(p => p["ORDER_ID"].ToString() == id).ToList()[0]["TAG_READ"]?.ToString();

                    DataSet1MTableAdapters.tb_mcs_messageTableAdapter adapter = new DataSet1MTableAdapters.tb_mcs_messageTableAdapter();
                    var table = adapter.GetDataByID(id);

                    if (table != null && table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            string message = row["message"].ToString();

                            switch (message)
                            {
                                case "ORDER_ASSIGN":
                                    txtOrderAssign.Text = row["status"].ToString();
                                    break;
                                case "ORDER_START":
                                    txtOrderStart.Text = row["status"].ToString();
                                    break;
                                case "ORDER_COMPLETE":
                                    txtOrderComplete.Text = row["status"].ToString();
                                    break;
                                case "REQUEST_START":
                                    txtReqStart.Text = row["status"].ToString();
                                    break;
                                case "REQUEST_EMPTY":
                                    txtReqEmpty.Text = row["status"].ToString();
                                    break;
                                case "DISMATCH_TARGET":
                                    txtDismatch.Text = row["status"].ToString();
                                    break;
                                case "NOTEXISTS_TARGET":
                                    txtNotExist.Text = row["status"].ToString();
                                    break;
                            }

                        }
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void InitTextBox()
        {
            txtOrderAssign.Text = "";
            txtOrderStart.Text = "";
            txtOrderComplete.Text = "";
            txtReqStart.Text = "";
            txtReqEmpty.Text = "";
            txtNotExist.Text = "";
            txtDismatch.Text = "";
            txtTagName.Text = "";
            txtTagResult.Text = "";
        }

        public void PrintGrid()
        {
            gridControl1.Refresh();
            gridControl1.ShowPrintPreview();
        }
        #endregion

        private void btnStatusRefresh_Click(object sender, EventArgs e)
        {
            ChangedRow();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ReLoadInfo();

            if (sourceTable != null)
            {
                var table = sourceTable.AsEnumerable()
                .Where(P => P["STATUS"].ToString() != "3" && P["STATUS"].ToString() != "4" && P["STATUS"].ToString() != "6");

                if (table != null && table.Count() > 0)
                    gridControl1.DataSource = table.CopyToDataTable();
                else
                    gridControl1.DataSource = sourceTable.Clone();
            }

            InitTextBox();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            var row = gridView1.GetRow(gridView1.FocusedRowHandle);
            string agv_id = (row as DataRowView).Row["agv_id"].ToString();


            if (agv_id != null && agv_id != "")
            {
                AGVStatus agvStatus = new AGVStatus();
                agvStatus.agv_id = agv_id;
                agvStatus.ShowDialog();
            }

        }

        private void MCSInfo_Leave(object sender, EventArgs e)
        {
            //if (timer.Enabled)
            //    timer.Stop();
        }

        private void MCSInfo_Enter(object sender, EventArgs e)
        {
            //if (!timer.Enabled)
            //    timer.Start();
        }

        private void ChangedArea(string area)
        {
            route_area = area;
            ReLoadInfo();
        }
    }
}
