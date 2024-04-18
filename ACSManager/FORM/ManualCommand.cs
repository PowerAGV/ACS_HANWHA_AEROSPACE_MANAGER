using ACSManager.DataSet1MTableAdapters;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACSManager.FORM
{
    public partial class ManualCommand : Form
    {
        public ManualCommand()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Close();
        }

        bool RH_Full = false;
        bool RH_Empty = false;
        bool LH_Full = false;
        bool LH_Empty = false;

        void GetMidDepotStatus()
        {
            DataSet1MTableAdapters.tb_sw_plcTableAdapter swAdtp = new DataSet1MTableAdapters.tb_sw_plcTableAdapter();
            foreach (DataSet1M.tb_sw_plcRow srow in swAdtp.GetData())
            {
                LH_Empty = srow.depoLHEmpty.Trim() == "1" ? true : false;
                LH_Full = srow.depoLHFull.Trim() == "1" ? true : false;
                RH_Empty = srow.depoRHEmpty.Trim() == "1" ? true : false;
                RH_Full = srow.depoRHFull.Trim() == "1" ? true : false;
                break;
            }
        }

        string sOrderID = "";
        void GetNweOrderID()
        {
            tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
            mcs.GetOrderID("", ref sOrderID);
        }
        private void RB_F_Click(object sender, EventArgs e)
        {
            GetMidDepotStatus();
            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (RH_Empty)
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0003524", "0005303");
                    XtraMessageBox.Show("명령 생성 성공", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("RH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void RA_F_Click(object sender, EventArgs e)
        {
            GetMidDepotStatus();

            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (RH_Empty)
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0003524", "0005103");
                    XtraMessageBox.Show("명령 생성 성공", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("RH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ManualCommand_Load(object sender, EventArgs e)
        {

        }
        
/*
RH B 체움
0003524 - 0005303
RH A 체움 
0003524 - 0005103
LH A 체움
0003544 - 0004703
LH B 체움
0003544 - 0004503
*/
       
        private void LA_F_Click(object sender, EventArgs e)
        {
            GetMidDepotStatus();

            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (LH_Empty)
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0003544", "0004703");
                    XtraMessageBox.Show("명령 생성 성공", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("LH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LB_F_Click(object sender, EventArgs e)
        {
            GetMidDepotStatus();
            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (LH_Empty)
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0003544", "0004503");
                    XtraMessageBox.Show("명령 생성 성공", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("LH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        /*
        RH B 비움
        0005303 - 0000107
        RH A 비움 
        0005103 - 0000107
        LH A 비움
        0004703 - 0000607
        LH B 비움
        0004503 - 0000607
        */
        private void RB_E_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (RH_Full)
                {
                    XtraMessageBox.Show("LH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0005303", "0000107");
                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void RA_E_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (RH_Full)
                {
                    XtraMessageBox.Show("LH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0005103", "0000107");
                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LA_E_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (LH_Full)
                {
                    XtraMessageBox.Show("LH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0004703", "0000607");
                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LB_E_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("명령을 생성 하시겠습니까?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (LH_Full)
                {
                    XtraMessageBox.Show("LH 대차 확인 필요", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    GetNweOrderID();
                    tb_mcs_commandTableAdapter mcs = new tb_mcs_commandTableAdapter();
                    mcs.InsertJobByACSManager(sOrderID, "0004503", "0000607");
                    XtraMessageBox.Show("Command Trancefer Success", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
