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
using ACSManager.data.MSSQL;
using ACSManager.data.MSSQL.MapDesignTableAdapters;

namespace ACSManager.Control
{
    public partial class ChargePointManage : DevExpress.XtraEditors.XtraUserControl
    {
        Color bk1 = Color.Black;
        Color bk2 = Color.Black;
        public const string AGV_WAIT_GROUP = "Waiting_Area";

        private DataTable sourceTable = null;

        public MapDesign.Node_Extension_JoinDataTable nodeExtensionTable = null;
        

        #region Constructor, Load
        public ChargePointManage()
        {
            InitializeComponent();
        }

        private void ChargePointManage_Load(object sender, EventArgs e)
        {
            //this.DataReWrite();
            this.DataRefresh();

            if (Program.USER_DATA.UserGroup == "MANAGER" || Program.USER_DATA.UserGroup == "MASTER")
            {
            }
            else
            {
            }
        }
        #endregion

        #region Event Function
        private void ChargePointManage_Enter(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void ChargePointManage_Leave(object sender, EventArgs e)
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

                string category = "";// View.GetRowCellDisplayText(e.RowHandle, View.Columns["comm_result"]);
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

        class NodeExtension
        {
            public string nodeName;
            public string nodeExtValue;
        }

        private IEnumerable<NodeExtension> GetNodeExtention(string sKey)
        {
            var query = from nejt in this.nodeExtensionTable
                        where nejt.CODE == sKey
                        select new NodeExtension() { nodeName = nejt.NODE_NAME, nodeExtValue = nejt.VALUE };
            return query;
        }


        void DataRefresh()
        {

            Group_Node_JoinTableAdapter gnjoinApt = new Group_Node_JoinTableAdapter();
            MapDesign.Group_Node_JoinDataTable waiting_group = gnjoinApt.GetGroupNode(Setting.MAP_ID, AGV_WAIT_GROUP, "");

            var result = waiting_group.AsEnumerable();
            var tb = result.Any() ? result.CopyToDataTable() : waiting_group.Clone();
            sourceTable = tb;

            if (sourceTable != null && sourceTable.Rows.Count > 0)
            {
                InsertNumber(ref sourceTable);

                Node_Extension_JoinTableAdapter nodeExtension = new Node_Extension_JoinTableAdapter();
                MapDesign.Node_Extension_JoinDataTable nodeExtData = nodeExtension.GetNodeExtension(Setting.MAP_ID, "", "isCharging");

                sourceTable.Columns.Add("isCharging", typeof(string));
                //sourceTable.Columns.Add("reg_date", typeof(DateTime));
                //sourceTable.Columns.Add("response_time", typeof(int));

                



                foreach (DataRow dr in sourceTable.Rows)
                {
                    if (nodeExtData.AsEnumerable().Where(x => x.Field<string>("NODE_NAME") == dr["NODE_NAME"].ToString() && x.Field<string>("VALUE") =="True" ).Count()> 0)
                    {
                        dr["isCharging"] = "True";
                    }
                    else
                    {
                        dr["isCharging"] = "False";
                    }
                }
            }
            /*
            Node_Extension_JoinTableAdapter nodJoinExtAdt = new Node_Extension_JoinTableAdapter();
            this.nodeExtensionTable = nodJoinExtAdt.GetNodeExtension(Setting.MAP_ID, "", "isCharging");

            foreach (MapDesign.Group_Node_JoinRow gnjrow in globalRouteInfo.group_node_join.Where(x => x.P_GROUP_NAME.Equals(Program.AGV_WAIT_GROUP) && x.C_GROUP_NAME.Equals(this.waiting_area)))
            {
                this.m_wait_place_list.Add(gnjrow.NODE_NAME);
            }
            */

            gridControl2.DataSource = sourceTable;
            gridView2.BestFitColumns();


        }

        void DataReWrite()
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

        void initCheckBox()
        {
            //RepositoryCheckItem
        }

        public void PrintGrid()
        {
            this.gridControl2.ShowPrintPreview();
        }


        void threadRefrashAGVList()
        {
            try
            {
                // 그리드를 돌면서 agv 이름을 가져오고 그 이름으로 히스토리의 값을 가져 온다.
                for (int i = 0; i < gridView2.RowCount; i++)
                {
                    /*
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
                    */

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("변경 내용을 저장 하시겠습니까?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                string sCharging = "";
                int iNode_id = 0;
                string sCode = "isCharging";

                node_extensionTableAdapter nodext = new node_extensionTableAdapter();

                try
                {
                    for (int i = 0; i < gridView2.RowCount; i++)
                    {


                        bool isChecked = Convert.ToBoolean(this.gridView2.GetRowCellValue(i, "isCharging"));
                        sCharging = isChecked ? "True" : "False";

                        //gridView2.column
                        iNode_id = int.Parse(gridView2.GetRowCellValue(i, "NODE_ID").ToString());

                        nodext.UpdateExtCodeValue(sCharging, iNode_id, sCode);

                    }

                    this.DataRefresh();


                }
                catch (Exception ex)
                {
                    TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
                }
            }

            XtraMessageBox.Show("저장 완료", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
