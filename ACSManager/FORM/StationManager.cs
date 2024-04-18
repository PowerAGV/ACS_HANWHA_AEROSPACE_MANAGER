using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.data.MSSQL;
using ACSManager.DataSet1MTableAdapters;
using DevExpress.XtraEditors.Repository;
using ACSManager.Control;

namespace ACSManager.FORM
{
    public partial class StationManager : DevExpress.XtraEditors.XtraForm
    {
        public int StationNo = 0;
        public int Port = 0;

        //public string m_agv_id = "";
        public int agv_type = 0;
        public int floor = 0;
        public string area = "";

        public string m_ip = "";
        public string m_ip2 = "";
        public int m_port = 0;
        
        //현재 안쓰는 변수이지만 db에 Notnull이라 일단 삭제 안함
        public long mapid = -1;

        public bool isNew = true;

        public delegate void FormSendDataHandler(Object obj);

        public event FormSendDataHandler FormSendEvent;



        #region 생성자 LOAD 함수

        public StationManager()
        {
            InitializeComponent();
        }

        private StationInfo sti;
        public StationManager(StationInfo ucl, int stationNo)
        {
            //StationInfo.Split(new char[] { ',' });
            sti = ucl;
            this.StationNo = stationNo;
            InitializeComponent();
        }

        private void CarInfo_Load(object sender, EventArgs e)
        {
            // init route area
            // get apply map

            //txtNode.Properties.Mask.EditMask =
            //    "([0-9])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}";
            //this.txtNode.Properties.Mask.IgnoreMaskBlank = false;
            //this.txtNode.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;

            try
            {
                //TODO 현재 그룹때문에 늦게 틀어져서 막아놓음 나중에 쓸때 수정


                mapid = Setting.MAP_ID;

                this.isNew = (this.StationNo == 0);

                //DataSet1MTableAdapters.m_node_groupTableAdapter nadp = new DataSet1MTableAdapters.m_node_groupTableAdapter();
                //DataSet1M.m_node_groupDataTable ndtbl = nadp.GetDataByMapID(mapid);


                // Station 정보
                //Dictionary<int, string> dStation = new Dictionary<int,string>();
                cboStationName.Properties.Items.Clear();
                cboDesStation.Properties.Items.Clear();

                for ( int i = 0; i<Setting.Station_Count; i++)
                {
                    //dStation.Add(i, $"{Setting.Station_Name}{i + 1}");
                    this.cboStationName.Properties.Items.Add($"{Setting.Station_Name}{i + 1}");
                    this.cboDesStation.Properties.Items.Add($"{Setting.Station_Name}{i + 1}");

                }
                cboStationName.IsModified = false;
                cboDesStation.IsModified = false;



                cboPort.IsModified = false;
                cboDesPort.IsModified = false;

                // Port 정보
                //Dictionary<int, string> dPort = new Dictionary<int, string>();
                for (int i = 0; i < Setting.Port_Count; i++)
                {
                    //dPort.Add(i + 1, $"{i + 1}");
                    cboPort.Properties.Items.Add($"{i + 1}");
                    cboDesPort.Properties.Items.Add($"{i + 1}");

                }





                //cboPort.Properties.Items.Add(dPort);

                // Empty,Full
                //Dictionary<int, string> dEFGubun = new Dictionary<int, string>();
                //dEFGubun.Add(0, "Empty");
                //dEFGubun.Add(1, "Full");


                //cboFEGubun.DataBindings.Add(new Binding("", dEFGubun, "Value"));

                cboFEGubun.Properties.Items.Add($"Empty");
                cboFEGubun.Properties.Items.Add($"Full");
                cboFEGubun.IsModified = false;











                //cboFEGubun.Properties.Items.Add("Empty");
                //cboFEGubun.Properties.Items.Add("Full");


                cboValid.Properties.Items.Add("Y");
                cboValid.Properties.Items.Add("N");
                cboValid.IsModified = false;


                


                //List<string> gnames = new List<string>();
                DataSet1MTableAdapters.tb_station_InfoTableAdapter staionAdt = new DataSet1MTableAdapters.tb_station_InfoTableAdapter();
                //staionAdt.GetStationDetail(this.StationNO);

                foreach(DataSet1M.tb_station_InfoRow srow in staionAdt.GetStationInfoDetail(this.StationNo))
                {
                    
                    this.txtStationNo.Text = srow["StationNo"].ToString();
                    this.cboStationName.EditValue = srow["Station"].ToString();
                    this.cboPort.EditValue = srow["Port"].ToString();
                    this.cboFEGubun.EditValue = srow["Kind"].ToString();
                    //this.txtStationNO.Text = srow["isExist"].ToString();
                    this.txtNode.Text = srow["Node"].ToString();
                    this.cboValid.EditValue = srow["Valid"].ToString();
                    this.cboDesStation.EditValue = srow["subStation"].ToString();
                    this.cboDesPort.EditValue = srow["subPort"].ToString();

                    isNew = false;
                }

                // Group BY
                //foreach (MapDesign.Group_Node_JoinRow rrow in Setting.groupNodeJoinTbl.Where(x=>x.P_GROUP_NAME=="Route_Area"))
                //{
                //    if (gnames.FindIndex(x => x == rrow.C_GROUP_NAME) == -1)
                //        gnames.Add(rrow.C_GROUP_NAME);

                //}
                //foreach (string gname in gnames)
                //{
                //    cboFEGubun.Properties.Items.Add(gname);
                //}
                //if (gnames.Count > 0)
                //    cboFEGubun.SelectedIndex = 0;

                if (isNew)
                {   // 편집일 경우
                    cboStationName.SelectedIndex = 0;
                    cboPort.SelectedIndex = 0;
                    cboFEGubun.SelectedIndex = 0;
                    cboValid.SelectedIndex = 0;

                    //TODO 나중에 풀어야함
                    //cmb_area.SelectedIndex = 0;
                    txtNode.Text = "";
                    cboDesStation.SelectedIndex = 0;
                    cboDesPort.SelectedIndex = 0;

                
                }
                this.btnDelete.Visible = !isNew;

            }
            catch (Exception ee)
            {
                MessageBox.Show("Dialog Init Fail", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

            this.CenterToScreen();
        }
        #endregion

        #region Event Function
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboStationName.SelectedIndex < 0)
                {
                    MessageBox.Show("Station 정보를 선택하세요", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }

                if (cboPort.SelectedIndex < 0)
                {
                    MessageBox.Show("Port 정보를 선택하세요", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }
                if (cboFEGubun.Text.Length < 1)
                {
                    MessageBox.Show("대차 종류를 선택 하세요.", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }

                //m_ip
                if (txtNode.Text.Length !=7)
                {
                    MessageBox.Show("위치 정보를 정확히 입력하세요.", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }
                if(MessageBox.Show("추가 또는 수정 작업을 진행 하시겠습니까?", "ACS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string subStation = this.cboDesStation.EditValue.ToString();
                    int subPort = int.Parse(this.cboDesPort.EditValue.ToString());

                    tb_station_InfoTableAdapter stationAdt = new tb_station_InfoTableAdapter();
                    DataSet1M.tb_station_InfoDataTable stTbl = stationAdt.GetSubStationNo(subStation, subPort);
                    int subStationNo = 0;
                    if (stTbl.Rows.Count > 0)
                    {
                        subStationNo = int.Parse(stTbl.Rows[0]["StationNo"].ToString());
                    }



                    if (this.isNew)
                    {
                        stationAdt.InsertStationInfo(
                                                      this.cboStationName.EditValue.ToString()
                                                    , int.Parse(this.cboPort.EditValue.ToString())
                                                    , this.cboFEGubun.EditValue.ToString()
                                                    , this.txtNode.Text
                                                    , this.cboValid.EditValue.ToString()
                                                    , subStationNo
                                                );

                    }
                    else
                    {
                        stationAdt.UpdateStationInfo(
                                                    this.cboFEGubun.EditValue.ToString() 
                                                    ,this.txtNode.Text 
                                                    ,this.cboValid.EditValue.ToString()
                                                    , subStationNo
                                                    , this.StationNo
                                                    
                                                );
                    }
                        
                    
                }


                if (isNew)
                {// 중복 확인 에디트 시에는 필요 없음.
                    //DataSet1TableAdapters.tb_agvTableAdapter tAgv = new DataSet1TableAdapters.tb_agvTableAdapter();
                    //long aCnt = (long)tAgv.ISInAGV(txtAGVID.Text);
                    //if (aCnt != 0)
                    //{
                    //    MessageBox.Show("Alady Exist AGV ID", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    //    return;
                    //}
                }


            }
            catch (Exception ee)
            {
                MessageBox.Show("Fail", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }



        
        private void btn_cancel_Click(object sender, EventArgs e)
        {

            this.sti.RefrashStation(true);

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Function
        public void SetEditMode(string agv_id)
        {
            isNew = false;

        }

        #endregion

        private void cmbFloor_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("삭제 작업을 진행 하시겠습니까?", "ACS", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    tb_station_InfoTableAdapter stationAdt = new tb_station_InfoTableAdapter();
                    stationAdt.DeleteStationInfo(StationNo);

                    MessageBox.Show("삭제 되었습니다.", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);

                    this.sti.RefrashStation(true);

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fail", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
                }
            }
        }
    }
}