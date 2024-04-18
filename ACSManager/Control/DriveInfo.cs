using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ACSManager.Control
{
    public partial class DriveInfo : DevExpress.XtraEditors.XtraUserControl
    {
        private DataTable sourceTable = null;

        #region Constructor, Load
        public DriveInfo()
        {
            InitializeComponent();
        }

        private void DriveInfo_Load(object sender, EventArgs e)
        {
            dateStart.DateTime = DateTime.Now.AddDays(-20);
            dateEnd.DateTime = DateTime.Now;

            ReLoadInfo();
            if (Program.USER_DATA.UserGroup == "MANAGER" || Program.USER_DATA.UserGroup == "MASTER")
            {

            }
            else
            {
                btnMCSCOMPLET.Enabled = false;
                btnMCSEMR.Enabled = false;
            }
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
                MCSDB.AGVEmergency(txt_MCSID.Text);
                XtraMessageBox.Show("MCS Emergency Send", "MCS Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MCSDB.AGVComplete(txt_MCSID.Text);
                XtraMessageBox.Show("MCS Compelete Send", "MCS Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {

            }
            //XtraMessageBox.Show("MSC Return Value : " + result.RESULT + "   " + result.ERRMSG, "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.Column.FieldName == "v_from" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["route_info"].ToString();
                    if (ft.Contains("-"))
                    {
                        string[] frArr = ft.Split("-".ToCharArray());
                        e.Value = frArr[0];
                    }
                }

                if (e.Column.FieldName == "v_to" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["route_info"].ToString();
                    if (ft.Contains("-"))
                    {
                        string[] frArr = ft.Split("-".ToCharArray());
                        e.Value = frArr[1];
                    }
                }


                if (e.Column.FieldName == "v_status" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["status"].ToString();

                    if (ft.Contains("EMPTY"))
                        e.Value = "Empty Cart";

                    else if (ft.Contains("LOAD"))
                        e.Value = "Material Cart";

                    else if (ft.Contains("WAIT"))
                        e.Value = "Waiting";

                    else
                        e.Value = ft;
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
            object selectObj = gridView1.GetRow(gridView1.FocusedRowHandle);
            if (sourceTable != null && sourceTable.Rows.Count > 0)
            {
                string id = (((DataRowView)selectObj)["job_id"].ToString());
                txt_MCSID.Text = id;
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object selectObj = gridView1.GetRow(gridView1.FocusedRowHandle);
            if(sourceTable != null && sourceTable.Rows.Count > 0)
            {
                string id = (((DataRowView)selectObj)["job_id"].ToString());
                txt_MCSID.Text = id;
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReLoadInfo();
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReLoadInfo();
        }
        #endregion

        #region Function

        private void ReLoadInfo()
        {
            try
            {
                DateTime sdate = new DateTime(int.Parse(dateStart.DateTime.ToString("yyyy")), int.Parse(dateStart.DateTime.ToString("MM")), int.Parse(dateStart.DateTime.ToString("dd")));
                DateTime edate = new DateTime(int.Parse(dateEnd.DateTime.AddDays(1).ToString("yyyy")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("MM")), int.Parse(dateEnd.DateTime.AddDays(1).ToString("dd")));

                DataSet1MTableAdapters.tb_route_historyTableAdapter adtroute = new DataSet1MTableAdapters.tb_route_historyTableAdapter();
                var tbroute = adtroute.GetDataBy(sdate, edate);

                //DataSet1MTableAdapters.m_map_nodeTableAdapter adtNodes = new DataSet1MTableAdapters.m_map_nodeTableAdapter();
                //var tbNodes = adtNodes.GetDataByMapID(Setting.MAP_ID);

                
                var result = tbroute.AsEnumerable();
                var tb = result.Any() ? result.CopyToDataTable() : tbroute.Clone();
                sourceTable = tb;
                
                
                Filter1();

                Filter2();

                InsertNumber(ref sourceTable);
                gridControl1.DataSource = sourceTable;
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

            gridView1.BestFitColumns();
        }

        /// <summary>
        /// 번호 입력
        /// </summary>
        /// <param name="dt"></param>
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

        /// <summary>
        /// 메뉴얼과 작업지시 구분용
        /// </summary>
        private void Filter1()
        {
            try
            {
                if (sourceTable != null && sourceTable.Rows.Count > 0)
                {
                    switch (comboBoxEdit1.SelectedIndex)
                    {
                        case 0:
                            return;
                        case 1:
                            {
                                var result = sourceTable.AsEnumerable()
                                    .Where(p => p.Field<string>("job_id").StartsWith("M") && !p.Field<string>("job_id").Equals(""));


                                var tb = result.Any() ? result.CopyToDataTable() : sourceTable.Clone();
                                sourceTable = tb;
                            }
                            break;
                        case 2:
                            {
                                var result = sourceTable.AsEnumerable()
                                              .Where(p => p.Field<string>("job_id").StartsWith("NU") && !p.Field<string>("job_id").Equals(""));

                                var tb = result.Any() ? result.CopyToDataTable() : sourceTable.Clone();
                                sourceTable = tb;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //현재 구분할 방법이 없어서 패스
        private void Filter2()
        {
        }

        public void PrintGrid()
        {
            gridControl1.Refresh();
            gridControl1.ShowPrintPreview();
        }
        #endregion
    }
}
