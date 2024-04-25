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
using ACSManager.FORM;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using DevExpress.XtraEditors.Repository;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.data.MSSQL;
using Newtonsoft.Json.Linq;

namespace ACSManager.Control
{
    public partial class CarInfoList : DevExpress.XtraEditors.XtraUserControl, IDisposable
    {
        private Color bk1 = Color.Black;
        private Color bk2 = Color.Black;

        private string select_agv = "";
        private string route_area = "ALL";

        public delegate void ChangedMoveArea(string area);

        #region Constructor, Load
        public CarInfoList()
        {
            InitializeComponent();
        }

        Dictionary<string, string> m_locname = new Dictionary<string, string>();
        private void CarInfoList_Load(object sender, EventArgs e)
        {
            //진행중이던거 완료 되면 화면 다시 띄우게 하기 위해서 추가
            Task.WaitAll();
            LoadAgvList();
            Program.main_process.DataFilterEvent += new DataFilterEventHandler(ChangedArea);


            Node_Extension_JoinTableAdapter nodJoinExtAdt = new Node_Extension_JoinTableAdapter();
            var nodeExtensionTable = nodJoinExtAdt.GetNodeExtension(Setting.MAP_ID, "", "AName");

            foreach (data.MSSQL.MapDesign.Node_Extension_JoinRow nex in nodeExtensionTable)
            {
                m_locname.Add(nex.NODE_NAME, nex.VALUE);
            }

            //dateTimePickerLogDate.Value = DateTime.Now;

            if (Program.USER_DATA.UserGroup == "MANAGER" || Program.USER_DATA.UserGroup == "MASTER")
            {

            }
            else
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnModify.Enabled = false;
            }

            timer1.Start();
        }
        #endregion

        #region Function
        private void LoadAgvList()
        {
            try
            {
                DataSet1MTableAdapters.tb_agvTableAdapter tAgv = new DataSet1MTableAdapters.tb_agvTableAdapter();
                DataSet1M.tb_agvDataTable dtAgv = tAgv.GetDataByMapID();
                DataTable dt;

                foreach (DataSet1M.tb_agvRow item in dtAgv)
                {
                    if (item.current_status.ToString().Equals("OFF"))
                    {
                        item.stop_reason = "";
                        item.battery_status = "";
                        item.current_job_id = "";
                        item.tag_id = "";
                        
                    }
                    else
                    {
                        if (item.current_job_id == "" || item.tag_id == "") // 명령, 카트번호 같이 표시되게
                        {
                            item.current_job_id = "";
                            item.tag_id = "";
                        }
                    }

                    if (item.current_node.EndsWith("000"))
                    {
                        item.stop_reason = "Not Initialized";
                    }
                }

                var result = dtAgv.AsEnumerable();


                if (route_area == "ALL")
                    dt = result.Any() ? result.CopyToDataTable() : dtAgv.Clone();
                else
                {
                    var result1 = result.Where(p => p.route_area == route_area);
                    dt = result1.Any() ? result1.CopyToDataTable() : dtAgv.Clone();
                }

                gridControl1.Invoke(new MethodInvoker(delegate ()
                {
                    gridControl1.DataSource = dt;
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            gridView1.BestFitColumns();
        }

        int ChangeGridCell(int rCnt, string fildname, string value)
        {
            if (gridView1.GetRowCellValue(rCnt, fildname).ToString() != value)
            {
                if (gridControl1.InvokeRequired)
                {
                    gridControl1.Invoke(new MethodInvoker(delegate ()
                    {
                        gridView1.SetRowCellValue(rCnt, fildname, value);
                    }));
                }
                else
                {
                    gridView1.SetRowCellValue(rCnt, fildname, value);
                }

                return 1; // 변경 된 내용이 있다면 증가하도록 함.
            }
            return 0;
        }


        void threadRefrashAgvList()
        {
            try
            {
                DataSet1MTableAdapters.tb_agvTableAdapter refAdt = new DataSet1MTableAdapters.tb_agvTableAdapter();
                DataSet1M.tb_agvDataTable dtAGV2 = null;  //refAdt.GetDataByMapID();

                if (route_area == "ALL")
                {
                    dtAGV2 = refAdt.GetDataByMapID();
                }
                else
                {
                    var result = refAdt.GetDataByMapID().Where(p => p.route_area == route_area);
                    result.CopyToDataTable(dtAGV2, LoadOption.PreserveChanges);
                }


                foreach (DataSet1M.tb_agvRow agvItem in dtAGV2)
                {
                    int rhandle = IsExist(agvItem.agv_id);
                    if (rhandle == -1 && gridView1.DataSource != null)
                    {// 추가

                        LoadAgvList(); // 추가가 되면 갱신 하고 리턴
                        return;
                    }
                    else
                    {// 변경된 값만 셀을 바꾼다.

                        
                        if (agvItem.current_status.ToString().Equals("OFF"))
                        {
                            agvItem.stop_reason = "";
                            agvItem.battery_status = "";
                            agvItem.current_job_id = "";
                            agvItem.tag_id = "";
                            agvItem.agv_desc = "";
                            agvItem.traffic = "";
                        }
                        else
                        {
                            if (agvItem.current_job_id == "") //|| agvItem.tag_id == "" // 명령, 카트번호 같이 표시되게
                            {
                                agvItem.current_job_id = "";
                                agvItem.tag_id = "";
                            }
                        }

                        if (agvItem.current_node.EndsWith("000"))
                        {
                            agvItem.stop_reason = "Not Initialized";
                        }

                        if (!agvItem.Iscurrent_statusNull())
                            ChangeGridCell(rhandle, "current_status", agvItem.current_status);
                        if (!agvItem.Isstop_reasonNull())
                            ChangeGridCell(rhandle, "stop_reason", agvItem.stop_reason);
                        if (!agvItem.Isbattery_statusNull())
                            ChangeGridCell(rhandle, "battery_status", agvItem.battery_status);
                        if (!agvItem.Iscurrent_nodeNull())
                            ChangeGridCell(rhandle, "current_node", agvItem.current_node);
                        if (!agvItem.Iscurrent_routeNull())
                            ChangeGridCell(rhandle, "current_route", agvItem.current_route);
                        if (!agvItem.Iscurrent_job_idNull())
                        {
                            if(agvItem.current_job_id != "")
                                ChangeGridCell(rhandle, "current_job_id", "작업 중");
                            else
                                ChangeGridCell(rhandle, "current_job_id", "");
                        }
                            

                        ChangeGridCell(rhandle, "traffic", agvItem.traffic.ToString());

                        ChangeGridCell(rhandle, "agv_desc", agvItem.agv_desc.ToString());

                        ChangeGridCell(rhandle, "available", agvItem.available.ToString());

                        ChangeGridCell(rhandle, "drive_status", agvItem.Isdrive_statusNull() ? "" : agvItem.drive_status);

                        ChangeGridCell(rhandle, "tag_id", agvItem.tag_id);

                        ChangeGridCell(rhandle, "route_area", agvItem.Isroute_areaNull() ? "" : agvItem.route_area);
                    }
                }
                dtAGV2.Dispose();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

        }

        private void RefrashAgvList()
        {

            Task mytask = Task.Run(() =>
            {
                threadRefrashAgvList();
            });
            //Thread mulThread = new Thread(delegate () { threadRefrashAgvList(); });
            //mulThread.Start();
        }


        private int IsExist(string agvID)
        {
            if (agvID == null)
                return -1;


            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (gridView1.GetRowCellValue(i, "agv_id").ToString() == agvID)
                    return i;
            }

            return -1;
        }


        public void PrintGrid()
        {
            gridControl1.ShowPrintPreview();
        }

        /*
        AGV
        STATUS
        BATTERY
        S_UP
        S_DN
        LIFT
        F_NODE
        TAG_INFO
        JOB
        AUTO/MAN
        ANGLE
        NODE
        LINK_ID
        US/FR
        */

        public string GetGQStatus(string gqStatus, string skey)
        {
            string [] properties = gqStatus.Split(new char[] { ';' });
            string svalue = "";
            string[] porperty = new string[2];
            foreach (string sproperty in properties)
            {

                porperty = sproperty.Split(new char[] { '=' });
                if (porperty[0].Trim() ==skey)
                {
                    svalue = porperty[1];
                    break;
                }
            }

            //string svalue = query.FirstOrDefault().ToString().Split(new char[] { '=' })[1];

            
            return svalue.Trim();
        }

        #endregion

        #region Event Function
        private void btnAdd_ItemClick(object sender, EventArgs e)
        {
            try
            {

                CarInfo mCar = new CarInfo();
                if (mCar.ShowDialog() == DialogResult.OK)
                {
                    DataSet1MTableAdapters.tb_agvTableAdapter tAgv = new DataSet1MTableAdapters.tb_agvTableAdapter();
                    // insert and refrash
                    tAgv.InsertAGV( mCar.m_agv_id, mCar.area,mCar.waiting, mCar.m_ip, mCar.m_port, mCar.floor);
                    LoadAgvList();

                    MessageBox.Show("AGV Data Created", "ACS", MessageBoxButtons.OK);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Insert Fail 1", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selItm = gridView1.GetSelectedRows();
                object obj = gridView1.GetRow(gridView1.FocusedRowHandle);
                if (MessageBox.Show("Do you want to delete?", "ACS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (selItm.Length > 0)
                    {
                        string agv_id = (obj as DataRowView).Row[0].ToString();
                        DataSet1MTableAdapters.tb_agvTableAdapter tAgv = new DataSet1MTableAdapters.tb_agvTableAdapter();
                        tAgv.DeleteQuery(agv_id);
                    }

                }
                LoadAgvList();

            }
            catch (Exception ee)
            {
                MessageBox.Show("Delete Fail", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                // insert and refrash
                long mapid = Setting.MAP_ID;
                //mapTableAdapter madp = new mapTableAdapter();
                //foreach(MapDesign.mapRow mrow in madp.GetCurrentMap())
                //{
                //    mapid = (long)mrow["id"];
                //}
                

                int[] selItm = gridView1.GetSelectedRows();
                object obj = gridView1.GetRow(gridView1.FocusedRowHandle);
                if (selItm.Length > 0)
                {
                    string agv_id = (obj as DataRowView).Row[0].ToString();
                    CarInfo mCar = new CarInfo();
                    mCar.SetEditMode(agv_id);
                    if (mCar.ShowDialog() == DialogResult.OK)
                    {
                        DataSet1MTableAdapters.tb_agvTableAdapter tAgv = new DataSet1MTableAdapters.tb_agvTableAdapter();
                        tAgv.UpdateQuery( mCar.area, mCar.waiting,mCar.m_ip, mCar.m_port, mCar.floor, mCar.m_agv_id);
                        LoadAgvList();

                        MessageBox.Show("AGV Data Changed", "ACS", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Delete Fail", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                object selectObj = gridView1.GetRow(gridView1.FocusedRowHandle);// (gridView1.FocusedRowHandle);
                select_agv = (((DataRowView)selectObj)[0].ToString());

                DataSet1MTableAdapters.tb_agvTableAdapter tAgv = new DataSet1MTableAdapters.tb_agvTableAdapter();
                DataSet1M.tb_agvDataTable dtAgv = tAgv.GetDataByAgvID(select_agv);

                foreach (DataSet1M.tb_agvRow agvItem in dtAgv)
                {
                    //agvDetail1.SetAgvInformation(agvItem, true);
                    plcView1.SetAgvInformation(agvItem, true);
                    break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 1초마다 갱신 한다.
            RefrashAgvList();
        }


        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                //if (bk1 == Color.Black)
                //{
                //    bk1 = e.Appearance.BackColor;
                //    bk2 = e.Appearance.BackColor2;
                //}

                //string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns["current_status"]);
                //if (category.Contains("EMPTY") || category.Contains("LOAD") || category.ToUpper().Contains("Lif") || category.ToUpper().Contains("WAIT") || category.ToUpper().Contains("COM"))
                //{
                //    e.Appearance.BackColor = bk1;
                //    e.Appearance.BackColor2 = bk2;
                //}
                //else
                //{
                //    if (e.Column.FieldName == "agv_id")
                //    {
                //        e.Appearance.BackColor = Color.Red;
                //        e.Appearance.BackColor2 = Color.DarkRed;
                //    }
                //} //181125 빨간색 싫다고 요청하여 색깔 변경 주석처리
            }
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                if (e.Column.FieldName == "v_use" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["agv_area"].ToString();

                    if (ft.Contains("1"))
                        e.Value = "1F";
                    else if (ft.Contains("2"))
                        e.Value = "2F-TUB";
                    else if (ft.Contains("3"))
                        e.Value = "2F-GER";
                    else
                        e.Value = "NONE";



                }
                if (e.Column.FieldName == "v_from" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["current_route"].ToString();
                    if (ft.Contains("-"))
                    {
                        string[] frArr = ft.Split("-".ToCharArray());
                        if (m_locname.ContainsKey(frArr[0]))
                        {
                            e.Value = m_locname[frArr[0]];
                        }
                        else
                            e.Value = frArr[0];
                        //e.Value = frArr[0];
                    }
                }
                if (e.Column.FieldName == "v_to" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["current_route"].ToString();
                    if (ft.Contains("-"))
                    {
                        string[] frArr = ft.Split("-".ToCharArray());
                        if(m_locname.ContainsKey(frArr[1]))
                        {
                            e.Value = m_locname[frArr[1]];
                        }
                        else
                            e.Value = frArr[1];
                    }
                }
                if (e.Column.FieldName == "v_status" && e.IsGetData)
                {
                    DataRow row = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string ft = row["current_status"].ToString();
                    string ft2 = row["drive_status"].ToString();
                    
                        e.Value = ft2;
                    //if (ft.Contains("EMPTY"))
                    //    e.Value = "Pre-Run";
                    /*
                    if (ft.Contains("LOAD"))
                    {
                        if (ft2.Contains("Transfer"))
                        {
                            e.Value = "Transfer";
                        }
                        else
                        {
                            e.Value = "Pre-Run";
                        }
                    }

                    else if (ft.Contains("OFF"))
                        e.Value = "OFF";
                    else if (ft.Contains("WAIT"))
                        e.Value = "Waiting";
                    else if (ft.Contains("Parking"))
                        e.Value = "Parking";
                    else
                        e.Value = ft;
                    if (ft2.Contains("Parking"))
                    {
                        e.Value = "Parking";
                    }
                    */
                }
                if (e.Column.FieldName == "v_mode" && e.IsGetData)
                {
                    DataRow row     = gridView1.GetDataRow(e.ListSourceRowIndex);
                    string agv_desc = row["agv_desc"].ToString();
                    string sMode    = this.GetGQStatus(agv_desc,"AUTO/MAN");
                    e.Value         = sMode;

                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
                System.Diagnostics.Debug.WriteLine(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }
        #endregion



        private void btn_CSV_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Task.WaitAll();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            ofd.Title = "Select AGV CSV File";
            ofd.ValidateNames = false;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() != DialogResult.OK) return;

            var lines = File.ReadAllLines(ofd.FileName)?.ToList();

            //객체로 변환
            lines.RemoveAt(0); //컬럼 삭제
            var list_AGV = new List<AGV>();

            foreach (string obj in lines)
            {
                var values = obj.Split(',');
                list_AGV.Add(new AGV
                {
                    ID = values[0],
                    Route_Area = values[1],
                    Waiting_Area = values[2],
                    IP = values[3],
                    PORT = int.Parse(values[4]),
                    Available = int.Parse(values[5]),
                    Area = int.Parse(values[6])
                });
            }

            if (list_AGV.Count() > 0)
            {
                var adtAGV = new DataSet1MTableAdapters.tb_agvTableAdapter();
                var tbAGV = adtAGV.GetData();

                if (tbAGV != null && tbAGV.Rows.Count > 0)
                {
                    foreach (var obj in list_AGV)
                    {
                        var row = tbAGV.AsEnumerable().Where(p => p.agv_id == obj.ID); //번호 있는지 확인
                        if (row.Count() != 0)
                            adtAGV.DeleteQuery(row.ElementAt(0).agv_id); //있으면 삭제시킴

                        adtAGV.InsertAGV(obj.ID, obj.Route_Area,obj.Waiting_Area, obj.IP, obj.PORT, obj.Area);//삽입
                    }
                }
                MessageBox.Show("Insert Complete");
                //완료되었습니다. 메세지 박스 띄우기
            }
            else
            {
                MessageBox.Show("Not Exist Data");
                //데이터가 없습니다.
            }

            //다시 다 갱신

            timer1.Start();
        }


        private void btnLogDown_Click(object sender, EventArgs e)
        {
            //if (dateTimePickerLogDate.Value > DateTime.Today.Date)
            //{
            //    dateTimePickerLogDate.Value = DateTime.Today.Date;
            //}

            //DateTime selectedDate = dateTimePickerLogDate.Value;
            //string Date = selectedDate.ToShortDateString().Replace("-", "");

            //string Folder = GetDownloadsPath();
            ////Folder = Folder + @"/ACS Engine Logs/" + DateTime.Today.ToString("yyyyMMdd");
            //Folder = Folder + @"/ACS Engine Logs/" + Date;
            //if (!Directory.Exists(Folder))
            //{
            //    Directory.CreateDirectory(Folder);
            //}


            ////string Date = DateTime.Today.ToString("yyyyMMdd");
            //string url = string.Empty;
            //string agv_id = string.Empty;
            //int moveArea = 0;


            //object selectObj = gridView1.GetRow(gridView1.FocusedRowHandle);// (gridView1.FocusedRowHandle);
            //agv_id = (((DataRowView)selectObj)["agv_id"].ToString());
            //moveArea = (int)((DataRowView)selectObj)["agv_area"];

            ////agv_id랑 route_area를 가져와야하는뎅


            //url = @"http://192.168.1.2/ACS_LOG/Trace/" + Date + "/";
            ////url = @"http://127.0.0.1/ACS_LOG/Trace/" + Date + "/";
            //download(url + "Trace_" + Date + ".log", Folder);

            //agv_id = string.Format("{0:D3}", agv_id) + "_" + Date;
            //download(url + agv_id + ".log", Folder);
            

            //MessageBox.Show(agv_id.Split('_')[0] + " AGV Log Download Completed", this.Text, MessageBoxButtons.OK);

            //Process.Start(Folder);
        }

        private static Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);

        public string GetDownloadsPath()
        {
            if (Environment.OSVersion.Version.Major < 6) throw new NotSupportedException();
            IntPtr pathPtr = IntPtr.Zero;
            try
            {
                SHGetKnownFolderPath(ref FolderDownloads, 0, IntPtr.Zero, out pathPtr);
                return Marshal.PtrToStringUni(pathPtr);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pathPtr);
            }
        }

        private void download(string url, string Folder)
        {
            Cursor cursor = Cursors.WaitCursor;
            try
            {

                string filename = url.Substring(url.LastIndexOf("/") + 1);

                WebRequest http = WebRequest.Create(url);

                byte[] buf = new byte[32768];
                using (Stream input = http.GetResponse().GetResponseStream())
                {
                    using (FileStream output = new FileStream(Folder + "/" + filename, FileMode.Append))
                    {
                        int bytesRead = 0;
                        while ((bytesRead = input.Read(buf, 0, buf.Length)) > 0)
                        {
                            output.Write(buf, 0, bytesRead);
                            output.Flush();
                        }

                        input.Close();
                        output.Flush();
                        output.Close();
                        //
                    }
                }
            }
            catch (Exception ex)
            {
                cursor = Cursors.Default;

                //MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK);
            }
            cursor = Cursors.Default;
        }

        private void ChangedArea(string area)
        {
            route_area = area;
            LoadAgvList();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            //try
            //{ // 데폿 상황 표현
            //    DataSet1MTableAdapters.tb_sw_plcTableAdapter swAdtp = new DataSet1MTableAdapters.tb_sw_plcTableAdapter();
            //    foreach(DataSet1M.tb_sw_plcRow srow in swAdtp.GetData())
            //    {
            //        CEL.Text = srow.depoLHEmpty.Trim() == "1" ? "O" : "X";
            //        CFL.Text = srow.depoLHFull.Trim() == "1" ? "O" : "X";
            //        CER.Text = srow.depoRHEmpty.Trim() == "1" ? "O" : "X";
            //        CFR.Text = srow.depoRHFull.Trim() == "1" ? "O" : "X";
            //        break;
            //    }

            //}
            //catch(Exception ee)
            //{
            //    TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            //}

            //timer2.Enabled = true;
        }
    }

    public class AGV
    {
        public string ID { get; set; } = "000";
        public string Route_Area { get; set; } = "ALL";

        public string Waiting_Area { get; set; } = "";
        public string IP { get; set; }
        public int PORT { get; set; } = 55321;
        public int Available { get; set; } = 0;
        public int Area { get; set; } = 0;
    }

}
