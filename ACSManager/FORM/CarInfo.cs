﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.data.MSSQL;

namespace ACSManager.FORM
{
    public partial class CarInfo : DevExpress.XtraEditors.XtraForm
    {
        public string m_agv_id = "";
        public int agv_type = 0;
        public int floor = 0;
        public string area = "";
        public string waiting = "";


        public string m_ip = "";
        public string m_ip2 = "";
        public int m_port = 0;
        
        //현재 안쓰는 변수이지만 db에 Notnull이라 일단 삭제 안함
        public long mapid = -1;

        public bool isNew = true;

        #region 생성자 LOAD 함수

        public CarInfo()
        {
            InitializeComponent();
        }

        private void CarInfo_Load(object sender, EventArgs e)
        {
            // init route area
            // get apply map

            txtIP.Properties.Mask.EditMask =
                "([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}";
            this.txtIP.Properties.Mask.IgnoreMaskBlank = false;
            this.txtIP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;

            try
            {
                //TODO 현재 그룹때문에 늦게 틀어져서 막아놓음 나중에 쓸때 수정


                mapid = Setting.MAP_ID;

                //DataSet1MTableAdapters.m_node_groupTableAdapter nadp = new DataSet1MTableAdapters.m_node_groupTableAdapter();
                //DataSet1M.m_node_groupDataTable ndtbl = nadp.GetDataByMapID(mapid);

                List<string> gnames = new List<string>();
                // Group BY
                foreach (MapDesign.Group_Node_JoinRow rrow in Setting.groupNodeJoinTbl.Where(x=>x.P_GROUP_NAME=="Route_Area"))
                {
                    if (gnames.FindIndex(x => x == rrow.C_GROUP_NAME) == -1)
                        gnames.Add(rrow.C_GROUP_NAME);

                }
                List<string> apply_group = new List<string>();
                apply_group.Add("ALL");
                apply_group.Add("DCAP_LH");
                apply_group.Add("DCAP_RH");
                apply_group.Add("SCAP_LH");
                apply_group.Add("SCAP_RH");
                apply_group.Add("SPARE");

                foreach (string gname in gnames)
                {
                    //if(apply_group.Contains(gname))
                    if(gname.Contains("ALL") || gname.Contains("ONLY"))
                        cmb_area.Properties.Items.Add(gname);
                }
                if (gnames.Count > 0)
                    cmb_area.SelectedIndex = 0;

                gnames.Clear();
                foreach (MapDesign.Group_Node_JoinRow rrow in Setting.groupNodeJoinTbl.Where(x => x.P_GROUP_NAME == "Waiting_Area"))
                {
                    if (gnames.FindIndex(x => x == rrow.C_GROUP_NAME) == -1)
                        gnames.Add(rrow.C_GROUP_NAME);
                }
                foreach (string gname in gnames)
                {
                    this.cmb_waiting.Properties.Items.Add(gname);
                }
                if (gnames.Count > 0)
                    this.cmb_waiting.SelectedIndex = 0;



                if (!isNew)
                {// 편집일 경우
                    DataSet1MTableAdapters.tb_agvTableAdapter atadp = new DataSet1MTableAdapters.tb_agvTableAdapter();
                    DataSet1M.tb_agvDataTable agvDtbl = atadp.GetDataByAgvID(m_agv_id);
                    foreach (DataSet1M.tb_agvRow agvRow in agvDtbl)
                    {
                        m_agv_id = txtAGVID.Text = agvRow.agv_id;
                        agv_type = agvRow.agv_area;

                        floor = agv_type;
                        cmbFloor.SelectedIndex = agv_type;

                        m_ip = txtIP.Text = agvRow.ip;
                        txtPort.Text = agvRow.port.ToString();
                        m_port = agvRow.port;
                        mapid = Setting.MAP_ID;
                        area = cmb_area.Text = agvRow.route_area;
                        waiting = cmb_waiting.Text = agvRow.waiting_area;

                        /*
                        if(agvRow.agv_area == 1 || agvRow.agv_area == 3)
                            cmbAGVType.SelectedIndex = 0;
                        else if(agvRow.agv_area == 2)
                            cmbAGVType.SelectedIndex =1;
                        else
                            cmbAGVType.SelectedIndex = 2;
                        */
                        txtAGVID.ReadOnly = true;
                    }
                }
                else
                {
                    txtAGVID.Text = "";
                    cmbAGVType.SelectedIndex = 0;
                    cmbFloor.SelectedIndex = 0;

                    //TODO 나중에 풀어야함
                    //cmb_area.SelectedIndex = 0;
                    txtIP.Text = "";
                    txtPort.Text = "55321";
                }
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
                if (txtAGVID.Text.Length < 2)
                {
                    MessageBox.Show("Check AGV ID", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }
                else if (cmbFloor.SelectedIndex < 0)
                {
                    MessageBox.Show("Check AGV ID", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }
                else if (cmb_area.Text.Length < 1)
                {
                    MessageBox.Show("Check Area Information", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }

                //m_ip
                else if (txtIP.Text.Length < 2)
                {
                    MessageBox.Show("Check IP(P/C)", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                    return;
                }
                else
                {
                    if(MessageBox.Show("Do you want to add or modify?", "ACS", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        m_agv_id = txtAGVID.Text;
                        //agv_type = cmbAGVType.SelectedIndex;
                        floor = cmbFloor.SelectedIndex;
                        area = cmb_area.Text;
                        waiting = cmb_waiting.Text;
                        m_ip = txtIP.Text;
                        //m_ip2 = txtIP.Text;
                        m_port = int.Parse(txtPort.Text);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }

                //TODO 나중에 풀어야함
                ////AGV TYPE
                //if (cmbAGVType.SelectedIndex < 0)
                //{
                //    MessageBox.Show("check agv type", "acs", MessageBoxButtons.OK, MessageBoxIcon.None);
                //    return;
                //}

                //Floor
                //if (cmbFloor.SelectedIndex < 0)
                //{
                //    MessageBox.Show("Check AGV ID", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                //    return;
                //}

                ////TODO 나중에 풀어야함
                //if (cmb_area.Text.Length < 1)
                //{
                //    MessageBox.Show("Check Area Information", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                //    return;
                //}

                ////m_ip
                //if (txtIP.Text.Length < 2)
                //{
                //    MessageBox.Show("Check IP(P/C)", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                //    return;
                //}

                //TODO 나중에 풀어야함
                //m_ip2
                //if (txtIP2.Text.Length < 2)
                //{
                //    MessageBox.Show("Check IP(D/S)", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                //    return;
                //}

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


                //m_agv_id = txtAGVID.Text;
                ////agv_type = cmbAGVType.SelectedIndex;
                //floor = cmbFloor.SelectedIndex;
                //area = cmb_area.Text;
                //m_ip = txtIP.Text;
                ////m_ip2 = txtIP.Text;
                //m_port = int.Parse(txtPort.Text);

                //this.DialogResult = DialogResult.OK;
                //this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Fail", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Function
        public void SetEditMode(string agv_id)
        {
            isNew = false;
            m_agv_id = agv_id;
        }

        #endregion

        private void cmbFloor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmb_area.Properties.Items.Clear();
            //cmb_area.Text = "";

            //if (cmbFloor.SelectedItem.ToString() == "1 Floor")
            //{
            //    List<string> gnames = new List<string>();
            //    foreach (DataSet1M.m_node_groupRow rrow in ndtbl)
            //    {
            //        if (gnames.FindIndex(x => x == rrow.group_name) == -1)
            //            gnames.Add(rrow.group_name);

            //    }
            //    foreach (string gname in gnames)
            //    {
            //        cmb_area.Properties.Items.Add(gname);
            //    }
            //}
        }
    }
}