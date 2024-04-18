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
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using System.IO;

namespace ACSManager.Control
{    

    public partial class AGVDetail : DevExpress.XtraEditors.XtraUserControl
    {
        public static AGVDetail agvDetail;
        public bool IS_DEBUG_FILE_EXIST(string fname)
        {
            bool rval = false;
            try
            {
                string DBG_FileName = AppDomain.CurrentDomain.BaseDirectory + @"\" + fname;

                if (File.Exists(DBG_FileName))
                    rval = true;
            }
            catch (Exception ee)
            {

            }
            return rval;
        }

        Timer timer1 = null;
        Dictionary<string, string> NodeNames = new Dictionary<string, string>();
        public AGVDetail()
        {
            InitializeComponent();
            agvDetail = this;

            timer1 = new Timer();
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            timer1.Start();

            reloadAGVDetail();
        }

        private void AGVDetail_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                cadtp.UpdateCommandReset();

                if (Program.USER_DATA.UserGroup == "MANAGER" || Program.USER_DATA.UserGroup == "MASTER")
                {
                }
                else
                {
                    btn_dropjob.Enabled = false;
                    simpleButton1.Enabled = false;
                    btn_charge.Enabled = false;
                    btn_auto_drive.Enabled = false;
                    btn_manual_drive.Enabled = false;
                }

                DataSet1MTableAdapters.tb_acs_configTableAdapter acsConfigAdtp = new DataSet1MTableAdapters.tb_acs_configTableAdapter();
                DataSet1M.tb_acs_configDataTable tbAcsConfig = acsConfigAdtp.GetUseAcs();
                foreach (DataSet1M.tb_acs_configRow acsConfigRow in tbAcsConfig)
                {
                    if (acsConfigRow.acs_use == 1)
                    {
                        btn_UseAcsSelected.Text = "ACS 사용중";
                        btn_UseAcsSelected.Appearance.BackColor = Color.FromArgb(0, 255, 255, 255);
                        btn_UseAcsSelected.Appearance.ForeColor = Color.Black;
                        btn_UseAcsSelected.Tag = "1";
                    }

                    if (acsConfigRow.acs_use == 0)
                    {
                        btn_UseAcsSelected.Text = "ACS 정지중";
                        btn_UseAcsSelected.Appearance.BackColor = Color.Red;
                        btn_UseAcsSelected.Appearance.ForeColor = Color.Red;
                        btn_UseAcsSelected.Tag = "0";
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        public void SetAgvInformation(DataSet1M.tb_agvRow agv,bool nodeChange)
        {
            try
            {
                EventHandler eh1 = delegate
                {
                    lbl_agvName.Text = agv.agv_id;
                };
                lbl_agvName.Invoke(eh1);

                EventHandler eh2 = delegate
                {
                    lbl_agvFromTo.Text = agv.Iscurrent_routeNull() ? "" : agv.current_route;
                };
                lbl_agvFromTo.Invoke(eh2);

                EventHandler eh3 = delegate
                {
                    lbl_agvJobID.Text = agv.Iscurrent_job_idNull() ? "" : agv.current_job_id;
                };
                lbl_agvJobID.Invoke(eh3);

                EventHandler eh4 = delegate
                {
                    lbl_Traffic.Text = agv.IstrafficNull() ? "" : agv.traffic;
                };
                lbl_Traffic.Invoke(eh4);

                EventHandler eh5 = delegate
                {
                    gridControl1.DataSource = null;
                    
                    //DataTable tbRoute = new DataSet1MTableAdapters.tb_final_routeTableAdapter().GetDataByID(agv.agv_id);
                    var tbRoute = new DataSet1MTableAdapters.tb_final_routeTableAdapter().GetDataByID(agv.agv_id);

                    if (tbRoute != null && tbRoute.Rows.Count > 0)
                    {
                        gridControl1.DataSource = tbRoute.OrderBy(p => p.sequance);
                        gridView1.BestFitColumns();
                    }
                    
                };
                lbl_Traffic.Invoke(eh5);


            }
            catch(Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        #region EVENT FUCTION
        private void btn_manual_drive_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_agvName.Text.Length < 2)
                {
                    XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(txtFrom.Text.Length < 2 || txtTo.Text.Length < 2)
                {
                    XtraMessageBox.Show("Input From-To information", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                long mapid = Setting.MAP_ID;
                
                
                int fCnt = Setting.nodTbl.Where(x => x.name == txtFrom.Text).Count();
                int nCnt = Setting.nodTbl.Where(x => x.name == txtTo.Text).Count(); 
                if (fCnt == 0)
                {
                    XtraMessageBox.Show("Not Exist From Node", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (nCnt == 0)
                {
                    XtraMessageBox.Show("Not Exist To Node", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }



                if (XtraMessageBox.Show("Do you Want to Move selected AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    DataSet1MTableAdapters.tb_test_ipssTableAdapter taps = new DataSet1MTableAdapters.tb_test_ipssTableAdapter();
                    int inum = (Int32)taps.GetManualDriveCount(lbl_agvName.Text);
                    if (inum != 0)
                    {
                        if (XtraMessageBox.Show("Previous Drive Informatin Is Exist. Do you want to Manual drive", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                        // Auto Selection 주행이 있다면 삭제
                        taps.DeleteManualJob(lbl_agvName.Text);

                        DataSet1MTableAdapters.tb_manager_commandTableAdapter mcdap = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                        // manaual command 명령이 있다면 삭제
                        mcdap.DeleteCommand(lbl_agvName.Text);
                    }

                    // 수동 주행 다시 insert
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "MU", txtFrom.Text, txtTo.Text, 0);



                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btn_manual_stop_Click(object sender, EventArgs e)
        {
            try
            {
                if(lbl_agvName.Text.Length < 2)
                {
                    XtraMessageBox.Show("Select AGV","AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                cadtp.InsertQuery(lbl_agvName.Text,"ST","","",0);
                //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btn_liftup_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_agvName.Text.Length < 2)
                {
                    XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                cadtp.InsertQuery(lbl_agvName.Text, "LU", "", "", 0);
                //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btn_lift_down_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_agvName.Text.Length < 2)
                {
                    XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                cadtp.InsertQuery(lbl_agvName.Text, "LD", "", "", 0);
                //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btn_resume_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_agvName.Text.Length < 2)
                {
                    XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (XtraMessageBox.Show("교통통제를 해제하시겠습니까?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        TraceManager.AddLog(string.Format("btn_resume Click"));

                        DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                        cadtp.InsertQuery(lbl_agvName.Text, "RE", "", "", 0);
                        //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                        XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btn_dropjob_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_agvName.Text.Length < 2)
                {
                    XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (XtraMessageBox.Show("Do you Want to cancel job order?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        //bool isNotStopHold = true;

                        TraceManager.AddLog(string.Format("btn_dropjob_Click  Click"));


                        //DataSet1MTableAdapters.tb_stop_hold_flagTableAdapter StopHoldAdt = new DataSet1MTableAdapters.tb_stop_hold_flagTableAdapter();


                        //foreach (DataSet1M.tb_stop_hold_flagRow stopHoldRow in StopHoldAdt.GetDataByAgvid(lbl_agvName.Text))
                        //{
                        //    TraceManager.AddLog($"btn_dropjob_Click     StopHold    agv_id : {lbl_agvName.Text} flag : {stopHoldRow.stop_hold_flag}  ");

                        //    // stop_hold_flag가 1인 경우 stop_hold_flag만 0으로 변경
                        //    if (stopHoldRow.stop_hold_flag == 1)
                        //    {
                        //        // flag를 0으로 변경
                        //        StopHoldAdt.UpdateStopHoldFlag(0, lbl_agvName.Text);

                        //        // 에러 코드 초기화 ( 충전소에 사람이 개입했음으로 초기화)
                        //        DataSet1MTableAdapters.tb_agvTableAdapter agvAdt = new DataSet1MTableAdapters.tb_agvTableAdapter();
                        //        agvAdt.UpdateOnlyStatus("", "", lbl_agvName.Text);

                        //        // StopHold 시작 시간 초기화
                        //        StopHoldAdt.UpdateStartDate(DateTime.Now.AddYears(100), lbl_agvName.Text);

                        //        // 작업 취소 동작 안하도록 만듬
                        //        isNotStopHold = false;
                        //    }
                        //}


                        // stop_hold_flag가 0인 경우 작업 취소 동작
                        //if (isNotStopHold)
                        //{
                            DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                            cadtp.InsertQuery(lbl_agvName.Text, "CN", "", "", 0);
                            //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운

                        //}

                        XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                //DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                //cadtp.InsertQuery(lbl_agvName.Text, "CN", "", "", 0);
                ////ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                //XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("Do you Want to complete job order?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "CM", "", "", 0);
                    //ST/RE/CN/MU/LU/LD/CM 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운 / 완료
                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            //DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
            //cadtp.InsertQuery(lbl_agvName.Text, "CM", "", "", 0);
            ////ST/RE/CN/MU/LU/LD/CM 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운 / 완료
            //XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_EmStop_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Doy you wnat to ALL AGV Stop?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

        }

        private void btn_charge_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("Do you Want to move AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "CH", "", "", 0);
                    //ST/RE/CN/MU/LU/LD/CM 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운 / 완료
                }
            }
            //DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
            //cadtp.InsertQuery(lbl_agvName.Text, "CH", "", "", 0);
            ////ST/RE/CN/MU/LU/LD/CM 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운 / 완료
            //XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_auto_drive_Click(object sender, EventArgs e)
        {
            try
            {


                if (txtFrom.Text.Length < 2 || txtTo.Text.Length < 2)
                {
                    XtraMessageBox.Show("Input From-To information", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //long mapid = Setting.MAP_ID;
                int fCnt = Setting.nodTbl.Where(x => x.name == txtFrom.Text).Count();
                int nCnt = Setting.nodTbl.Where(x => x.name == txtTo.Text).Count();

                if (fCnt == 0)
                {
                    XtraMessageBox.Show("Not Exist From Node", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (nCnt == 0)
                {
                    XtraMessageBox.Show("Not Exist To Node", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (XtraMessageBox.Show("Do you Want to Move AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    //DataSet1MTableAdapters.tb_test_ipssTableAdapter taps = new DataSet1MTableAdapters.tb_test_ipssTableAdapter();

                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery("", "MU", txtFrom.Text, txtTo.Text, 0);



                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }
        #endregion

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("Do you Want to move AGV to charger?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "GW", "", "", 0);
                }
            }
        }

        

        private void btnChargeStart_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Trim().Equals(""))
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("Do you Want to charge in selected AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "CO", "", "", 0);
                    //ST/RE/CN/MU/LU/LD/CM 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운 / 완료
                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnChargeEnd_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Trim().Equals(""))
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("Do you Want to stop charge in selected AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "CX", "", "", 0);
                    //ST/RE/CN/MU/LU/LD/CM 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운 / 완료
                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void RH_A_Click(object sender, EventArgs e)
        {
            DataSet1MTableAdapters.tb_plc_eventTableAdapter eAdtp = new DataSet1MTableAdapters.tb_plc_eventTableAdapter();
            eAdtp.InsertPLCEvent("0005101","M_RTP_R");
            XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void RH_B_Click(object sender, EventArgs e)
        {
            DataSet1MTableAdapters.tb_plc_eventTableAdapter eAdtp = new DataSet1MTableAdapters.tb_plc_eventTableAdapter();
            eAdtp.InsertPLCEvent("0005301", "M_RTP_R");
            XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LH_A_Click(object sender, EventArgs e)
        {
            DataSet1MTableAdapters.tb_plc_eventTableAdapter eAdtp = new DataSet1MTableAdapters.tb_plc_eventTableAdapter();
            eAdtp.InsertPLCEvent("0004701", "M_RTP_R");
            XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LH_B_Click(object sender, EventArgs e)
        {
            DataSet1MTableAdapters.tb_plc_eventTableAdapter eAdtp = new DataSet1MTableAdapters.tb_plc_eventTableAdapter();
            eAdtp.InsertPLCEvent("0004501", "M_RTP_R");
            XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("출발 방지 AGV를 해제할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "PS", "", "", 0);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            //try
            //{ // RH LH AGV CALL 상황 표현
            //    DataSet1MTableAdapters.tb_sw_plcTableAdapter swAdtp = new DataSet1MTableAdapters.tb_sw_plcTableAdapter();
            //    foreach (DataSet1M.tb_sw_plcRow srow in swAdtp.GetData())
            //    {
            //        //RH_A.Appearance.ForeColor = srow.rhFullOKA.Trim() == "1" || srow.rhEmptyOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        //RH_B.Appearance.ForeColor = srow.rhFullOKB.Trim() == "1" || srow.rhEmptyOKB.Trim() == "1" ? Color.Red : Color.Black;
            //        //LH_A.Appearance.ForeColor = srow.lhFullOKA.Trim() == "1" || srow.lhEmptyOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        //LH_B.Appearance.ForeColor = srow.lhFullOKB.Trim() == "1" || srow.lhEmptyOKB.Trim() == "1" ? Color.Red : Color.Black;

            //        RH_A.Appearance.ForeColor = srow.rhFullOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        RH_B.Appearance.ForeColor = srow.rhFullOKB.Trim() == "1" ? Color.Red : Color.Black;
            //        LH_A.Appearance.ForeColor = srow.lhFullOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        LH_B.Appearance.ForeColor = srow.lhFullOKB.Trim() == "1" ? Color.Red : Color.Black;

            //        break;
            //    }

            //}
            //catch (Exception ee)
            //{
            //    TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            //}

            //timer1.Enabled = true;  
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            return;
            timer2.Enabled = false;

            //try
            //{ // RH LH AGV CALL 상황 표현
            //    DataSet1MTableAdapters.tb_sw_plcTableAdapter swAdtp = new DataSet1MTableAdapters.tb_sw_plcTableAdapter();
            //    foreach (DataSet1M.tb_sw_plcRow srow in swAdtp.GetData())
            //    {
            //        //RH_A.Appearance.ForeColor = srow.rhFullOKA.Trim() == "1" || srow.rhEmptyOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        //RH_B.Appearance.ForeColor = srow.rhFullOKB.Trim() == "1" || srow.rhEmptyOKB.Trim() == "1" ? Color.Red : Color.Black;
            //        //LH_A.Appearance.ForeColor = srow.lhFullOKA.Trim() == "1" || srow.lhEmptyOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        //LH_B.Appearance.ForeColor = srow.lhFullOKB.Trim() == "1" || srow.lhEmptyOKB.Trim() == "1" ? Color.Red : Color.Black;

            //        RH_A.Appearance.ForeColor = srow.rhFullOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        RH_B.Appearance.ForeColor = srow.rhFullOKB.Trim() == "1" ? Color.Red : Color.Black;
            //        LH_A.Appearance.ForeColor = srow.lhFullOKA.Trim() == "1" ? Color.Red : Color.Black;
            //        LH_B.Appearance.ForeColor = srow.lhFullOKB.Trim() == "1" ? Color.Red : Color.Black;

            //        break;
            //    }

            //}
            //catch (Exception ee)
            //{
            //    TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            //}
            

            timer2.Enabled = true;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            // 비움 체움
            ManualCommand m = new ManualCommand();
            m.ShowDialog();

        }

        private void cmbFromName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = cmbFromName.SelectedIndex;
            // NodeNames
            if (NodeNames.ContainsValue(cmbFromName.Text))
            {
                string find_node = NodeNames.FirstOrDefault(x => x.Value == cmbFromName.Text).Key;
                txtTo.Text = txtFrom.Text = find_node;
                cmbToName.SelectedIndex = sel;
            }

        }

        private void cmbToName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = cmbToName.SelectedIndex;
            // NodeNames
            if (NodeNames.ContainsValue(cmbToName.Text))
            {
                string find_node = NodeNames.FirstOrDefault(x => x.Value == cmbToName.Text).Key;
                txtTo.Text = find_node;
            }

        }

        private void L_F_R_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L1", "", "", 0);
                }
            }
        }

        private void L_F_L_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L2", "", "", 0);
                }
            }
        }

        private void U_F_R_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L3", "", "", 0);
                }
            }
        }

        private void U_F_L_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L4", "", "", 0);
                }
            }
        }

        private void L_B_R_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L5", "", "", 0);
                }
            }
        }

        private void L_B_L_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L6", "", "", 0);
                }
            }
        }

        private void U_B_R_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L7", "", "", 0);
                }
            }
        }

        private void U_B_L_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L8", "", "", 0);
                }
            }
        }

        private void L_A_R_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "L9", "", "", 0);
                }
            }
        }

        private void U_A_R_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "LA", "", "", 0);
                }
            }
        }

        private void L_A_L_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "LB", "", "", 0);
                }
            }
        }

        private void U_A_L_Click(object sender, EventArgs e)
        {
            if (lbl_agvName.Text.Length < 2)
            {
                XtraMessageBox.Show("Select AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (XtraMessageBox.Show("컨베이어를 작동 할까요?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery(lbl_agvName.Text, "LC", "", "", 0);
                }
            }
        }

        private void chkFront_CheckedChanged(object sender, EventArgs e)
        {
            // UPDATE tb_sw_plc
            // SET rhAGVCallA = :rhAGVCallA
            

        }

        private void LEFT_GO_Click(object sender, EventArgs e)
        {
            try
            {

                if (XtraMessageBox.Show("Do you Want to Send Forward route?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery("", "X2", txtFrom.Text, txtTo.Text, 0);

                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void RIGHT_GO_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                cadtp.InsertQuery("", "YX", txtFrom.Text, txtTo.Text, 0);

                XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void allStart_Click(object sender, EventArgs e)
        {
            try
            {

                if (XtraMessageBox.Show("Do you Want to Command Start?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery("", "XX", txtFrom.Text, txtTo.Text, 0);

                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void allPause_Click(object sender, EventArgs e)
        {
            try
            {

                if (XtraMessageBox.Show("Do you Want to Command Pause ALL AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery("", "XZ", txtFrom.Text, txtTo.Text, 0);

                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void allResum_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("Do you Want to Command Resum ALL AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery("", "XT", txtFrom.Text, txtTo.Text, 0);

                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("Do you Want to Send Reverse route?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery("", "X3", txtFrom.Text, txtTo.Text, 0);

                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                cadtp.InsertQuery("", "X1", txtFrom.Text, txtTo.Text, 0);

                XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {

                if (XtraMessageBox.Show("Do you Want to Command Reverse Start?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                    cadtp.InsertQuery("", "ZZ", txtFrom.Text, txtTo.Text, 0);

                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //ST/RE/CN/MU/LU/LD 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운
                }
            }
            catch (Exception ee)
            {
                XtraMessageBox.Show("Command Trancefer fail", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btn_UseAcsSelected_Click(object sender, EventArgs e)
        {
            string showMessage = "";
            if (btn_UseAcsSelected.Tag.ToString() == "0")
            {
                showMessage = "사용";
            }
            else if (btn_UseAcsSelected.Tag.ToString() == "1")
            {
                showMessage = "정지";
            }
            else
            {
                showMessage = "????";
            }

            if (XtraMessageBox.Show($"ACS를 {showMessage} 하시겠습니까?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataSet1MTableAdapters.tb_acs_configTableAdapter acsConfigAdtp = new DataSet1MTableAdapters.tb_acs_configTableAdapter();

                if (btn_UseAcsSelected.Tag.ToString() == "1")
                {
                    btn_UseAcsSelected.Text = "ACS 정지중";
                    btn_UseAcsSelected.Appearance.BackColor = Color.Red;
                    btn_UseAcsSelected.Appearance.ForeColor = Color.Red;
                    btn_UseAcsSelected.Tag = "0";
                    acsConfigAdtp.UpdateAcsUse(0);
                }
                else if (btn_UseAcsSelected.Tag.ToString() == "0")
                {
                    btn_UseAcsSelected.Text = "ACS 사용중";
                    btn_UseAcsSelected.Appearance.BackColor = Color.FromArgb(0, 255, 255, 255);
                    btn_UseAcsSelected.Appearance.ForeColor = Color.Black;
                    btn_UseAcsSelected.Tag = "1";
                    acsConfigAdtp.UpdateAcsUse(1);
                }
            }
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Do you Want to change AGV?", "AGV Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataSet1MTableAdapters.tb_manager_commandTableAdapter cadtp = new DataSet1MTableAdapters.tb_manager_commandTableAdapter();
                cadtp.InsertQuery("", "52", "", "", 0);
                //ST/RE/CN/MU/LU/LD/CM 정지 / 재운행 / 잡취소 / 메뉴얼운행 / 리프트업 / 리프트 다운 / 완료
                XtraMessageBox.Show("Command Change Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        public void reloadAGVDetail()
        {
            cmbFromName.Properties.Items.Clear();
            cmbToName.Properties.Items.Clear();
            NodeNames.Clear();

            cmbFromName.Properties.Items.Add("Select name");
            cmbToName.Properties.Items.Add("Select name");
            cmbFromName.SelectedIndex = 0;
            cmbToName.SelectedIndex = 0;
            // Load Name
            Node_Extension_JoinTableAdapter nodJoinExtAdt = new Node_Extension_JoinTableAdapter();
            var nodeExtensionTable = nodJoinExtAdt.GetNodeExtension(Setting.MAP_ID, "", "AName");
            foreach (data.MSSQL.MapDesign.Node_Extension_JoinRow nex in nodeExtensionTable)
            {
                //nex.VALUE.
                NodeNames.Add(nex.NODE_NAME, nex.VALUE);
                cmbFromName.Properties.Items.Add(nex.VALUE);
                cmbToName.Properties.Items.Add(nex.VALUE);
            }


            {
                List<string> tVal = NodeNames.Values.ToList();
                tVal.Sort();
                foreach (string k in tVal)
                {
                    string kk = NodeNames.FirstOrDefault(x => x.Value == k).Key;
                    Console.WriteLine("(\"" + k + "\",\"" + kk + "\")");
                }
            }
        }
    }



}
