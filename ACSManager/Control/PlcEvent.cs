/*
* © SYSCON 
* © TEAM :        SoftWare3
* @ Start Date :  2024.03.19
* @ Project :     AJIN GUEO
* @ Source :      PlcEvent.cs
*/

#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;

using ACSManager.data.MSSQL;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.DataSetPLCTableAdapters;
using ACSManager.DataSet1MTableAdapters;

using DevExpress.XtraDiagram;
#endregion

namespace ACSManager.Control
{

    public partial class PlcEvent : XtraUserControl
    {

        public PlcEvent()
        {
            InitializeComponent();
        }


        private void PlcEvent_Load(object sender, EventArgs e)
        {
            changeDataLabel();
        }

        private void OnTimer(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            try
            {
                changeDataLabel();
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
            finally
            {
                timer1.Enabled = true;
            }
        }


        void changeDataLabel()
        {
            try
            {


                tb_plc_subLine_CommTableAdapter sublineCommAdt = new tb_plc_subLine_CommTableAdapter();
                tb_plc_mainLine_CommTableAdapter mainlineCommAdt = new tb_plc_mainLine_CommTableAdapter();
                tb_plc_subLine_InfoTableAdapter sublineInfoAdt = new tb_plc_subLine_InfoTableAdapter();
                tb_plc_mainLine_InfoTableAdapter mainlineInfoAdt = new tb_plc_mainLine_InfoTableAdapter();

                int SubCommOK = 0;
                int SubAInfoOK = 0;
                int SubBInfoOK = 0;
                int MainCommOK = 0;
                int MainAInfoOK = 0;
                int MainBInfoOK = 0;

                foreach (DataSetPLC.tb_plc_subLine_CommRow Row in sublineCommAdt.GetData())
                {
                    changeLabelColor(Row.isAuto, Row.isManual, A1);
                    changeLabelColor(Row.abNormal, Row.emergency, A2);

                    changeLabelColor(Row.isAuto, Row.isManual, B1);
                    changeLabelColor(Row.abNormal, Row.emergency, B2);

                    changeLabelColor(Row.isAuto, Row.isManual, C1);
                    changeLabelColor(Row.abNormal, Row.emergency, C2);

                    changeLabelColor(Row.isAuto, Row.isManual, D1);
                    changeLabelColor(Row.abNormal, Row.emergency, D2);

                    if (Row.isAuto == 1 && Row.isManual == 0 && Row.abNormal == 1 && Row.emergency == 1)
                    {
                        SubCommOK = 1;
                    }
                }

                foreach (DataSetPLC.tb_plc_subLine_InfoRow Row in sublineInfoAdt.GetData())
                {
                    if (Row.node == "0000240") // Sub A
                    {
                        changeLabelColor(Row.pltON, 0, A3); //취출
                        changeLabelColor(Row.callAGV, 0, A4);

                        changeLabelColor(Row.pltON, 1, B3); //투입
                        changeLabelColor(Row.callAGV, 0, B4);


                        if (Row.pltON == 1 && Row.callAGV == 1) SubAInfoOK = 1;
                        else if (Row.pltON == 0 && Row.callAGV == 1) SubAInfoOK = 2;
                    }
                    else if (Row.node == "0000540")// Sub B
                    {
                        changeLabelColor(Row.pltON, 0, C3); //취출
                        changeLabelColor(Row.callAGV, 0, C4);

                        changeLabelColor(Row.pltON, 1, D3); //투입
                        changeLabelColor(Row.callAGV, 0, D4);

                        if (Row.pltON == 1 && Row.callAGV == 1) SubBInfoOK = 1;
                        else if (Row.pltON == 0 && Row.callAGV == 1) SubBInfoOK = 2;
                    }
                }

                foreach (DataSetPLC.tb_plc_mainLine_CommRow Row in mainlineCommAdt.GetData())
                { // Main Comm
                    changeLabelColor(Row.isAuto, Row.isManual, E1);
                    changeLabelColor(Row.abNormal, Row.emergency, E2);

                    changeLabelColor(Row.isAuto, Row.isManual, F1);
                    changeLabelColor(Row.abNormal, Row.emergency, F2);

                    changeLabelColor(Row.isAuto, Row.isManual, G1);
                    changeLabelColor(Row.abNormal, Row.emergency, G2);

                    changeLabelColor(Row.isAuto, Row.isManual, H1);
                    changeLabelColor(Row.abNormal, Row.emergency, H2);

                    if (Row.isAuto == 1 && Row.isManual == 0 && Row.abNormal == 1 && Row.emergency == 1)
                    {
                        MainCommOK = 1;
                    }
                }

                foreach (DataSetPLC.tb_plc_mainLine_InfoRow Row in mainlineInfoAdt.GetData())
                {
                    if (Row.node == "0000120") // Main A
                    {
                        changeLabelColor(Row.pltON, 0, E3); // 취출
                        changeLabelColor(Row.callAGV, 0, E4);

                        changeLabelColor(Row.pltON, 1, F3); //투입
                        changeLabelColor(Row.callAGV, 0, F4);

                        if (Row.pltON == 1 && Row.callAGV == 1) MainAInfoOK = 1;
                        else if (Row.pltON == 0 && Row.callAGV == 1) MainAInfoOK = 2;
                    }
                    else if (Row.node == "0000420") // Main B
                    {
                        changeLabelColor(Row.pltON, 0, G3); //취출
                        changeLabelColor(Row.callAGV, 0, G4);

                        changeLabelColor(Row.pltON, 1, H3); //투입
                        changeLabelColor(Row.callAGV, 0, H4);

                        if (Row.pltON == 1 && Row.callAGV == 1) MainBInfoOK = 1;
                        else if (Row.pltON == 0 && Row.callAGV == 1) MainBInfoOK = 2;
                    }
                }

                changeLabelColor(0, 0, A5);
                changeLabelColor(0, 0, B5);
                changeLabelColor(0, 0, C5);
                changeLabelColor(0, 0, D5);
                changeLabelColor(0, 0, E5);
                changeLabelColor(0, 0, F5);
                changeLabelColor(0, 0, G5);
                changeLabelColor(0, 0, H5);

                if (SubCommOK == 1 && SubAInfoOK == 1) changeLabelColor(1, 0, A5);
                else if (SubCommOK == 1 && SubAInfoOK == 2) changeLabelColor(1, 0, B5);

                if (SubCommOK == 1 && SubBInfoOK == 1) changeLabelColor(1, 0, C5);
                else if (SubCommOK == 1 && SubBInfoOK == 2) changeLabelColor(1, 0, D5);

                if (MainCommOK == 1 && MainAInfoOK == 1) changeLabelColor(1, 0, E5);
                else if (SubCommOK == 1 && MainAInfoOK == 2) changeLabelColor(1, 0, F5);

                if (MainCommOK == 1 && MainBInfoOK == 1) changeLabelColor(1, 0, G5);
                else if (SubCommOK == 1 && MainBInfoOK == 2) changeLabelColor(1, 0, H5);


                changeLabelColor(0, 0, O1);
                changeLabelColor(0, 0, O2);
                changeLabelColor(0, 0, O3);
                changeLabelColor(0, 0, O4);
                changeLabelColor(0, 0, O5);
                changeLabelColor(0, 0, O6);
                changeLabelColor(0, 0, O7);
                changeLabelColor(0, 0, O8);

                tb_mcs_commandTableAdapter mcsAdt = new tb_mcs_commandTableAdapter();
                foreach (DataSet1M.tb_mcs_commandRow Row in mcsAdt.GetDataByJOB())
                {
                    if (Row.FROM_LOCATOR == "0000240" && Row.TO_LOCATOR == "0000120") changeLabelColor(1, 0, O1);       // sub A -> main A
                    else if (Row.FROM_LOCATOR == "0000240" && Row.TO_LOCATOR == "0000420") changeLabelColor(1, 0, O2);  // sub A -> main B
                    else if (Row.FROM_LOCATOR == "0000540" && Row.TO_LOCATOR == "0000120") changeLabelColor(1, 0, O3);  // sub B -> main A
                    else if (Row.FROM_LOCATOR == "0000540" && Row.TO_LOCATOR == "0000420") changeLabelColor(1, 0, O4);  // sub B -> main B
                    else if (Row.FROM_LOCATOR == "0000120" && Row.TO_LOCATOR == "0000240") changeLabelColor(1, 0, O5);  // main A -> sub A
                    else if (Row.FROM_LOCATOR == "0000120" && Row.TO_LOCATOR == "0000540") changeLabelColor(1, 0, O6);  // main A -> sub B
                    else if (Row.FROM_LOCATOR == "0000420" && Row.TO_LOCATOR == "0000240") changeLabelColor(1, 0, O7);  // main B -> sub A
                    else if (Row.FROM_LOCATOR == "0000420" && Row.TO_LOCATOR == "0000540") changeLabelColor(1, 0, O8);  // main B -> sub B
                }
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }

        void changeLabelColor(int Condition, int Condition2, Label lb)
        {
            if (Condition == 1) lb.BackColor = Color.Lime;
            else lb.BackColor = SystemColors.ControlDark;

            if (lb.Tag != null)
            {
                if (lb.Tag.ToString() == "1") //자동,수동
                {
                    if (Condition == 1 && Condition2 == 0)
                    {
                        lb.BackColor = Color.Lime;
                        lb.Text = "자동";
                    }
                    else
                    {
                        lb.BackColor = SystemColors.ControlDark;
                        lb.Text = "수동";
                    }
                }

                else if (lb.Tag.ToString() == "2") //정상,비상
                {
                    if (Condition2 == 0)
                    {
                        lb.BackColor = Color.Red;
                        lb.Text = "비상";
                    }
                    else if (Condition == 0)
                    {
                        lb.BackColor = Color.Red;
                        lb.Text = "이상";
                    }
                    if (Condition == 1 && Condition2 == 1)
                    {
                        lb.BackColor = Color.Lime;
                        lb.Text = "정상";
                    }
                }

                else if (lb.Tag.ToString() == "3") //팔레트 ON, 팔레트OFF
                {
                    if (Condition == 1 && Condition2 ==0)
                    {
                        lb.BackColor = Color.Lime;
                    }
                    else if(Condition == 0 && Condition2 == 0)
                    {
                        lb.BackColor = SystemColors.ControlDark;
                    }
                    else if (Condition == 1 && Condition2 == 1)
                    {
                        lb.BackColor = SystemColors.ControlDark;
                    }
                    else if (Condition == 0 && Condition2 == 1)
                    {
                        lb.BackColor = Color.Lime;
                    }
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e) //SUB A
        {
            if (XtraMessageBox.Show($"Do you want to Notouch signal completion?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("SUB_LINE_A", "NOTOUCH", 0);
                plcEventAdt.InsertPlcReport("SUB_LINE_A", "LOADOUT", 0);
                plcEventAdt.InsertPlcReport("SUB_LINE_A", "UNLOADOUT", 0);

                TraceManager.AddLog($"Cilck Send Notouch signal completion --> 'SUB_LINE_A' ");
                XtraMessageBox.Show($"Send complete", "AcsConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e) //SUB B
        {
            if (XtraMessageBox.Show($"Do you want to Notouch signal completion?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("SUB_LINE_B", "NOTOUCH", 0);
                plcEventAdt.InsertPlcReport("SUB_LINE_B", "LOADOUT", 0);
                plcEventAdt.InsertPlcReport("SUB_LINE_B", "UNLOADOUT", 0);

                TraceManager.AddLog($"Cilck Send Notouch signal completion --> 'SUB_LINE_B' ");
                XtraMessageBox.Show($"Send complete", "AcsConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e) //MAIN A
        {
            if (XtraMessageBox.Show($"Do you want to Notouch signal completion?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("MAIN_LINE_A", "NOTOUCH", 0);
                plcEventAdt.InsertPlcReport("MAIN_LINE_A", "LOADOUT", 0);
                plcEventAdt.InsertPlcReport("MAIN_LINE_A", "UNLOADOUT", 0);

                TraceManager.AddLog($"Cilck Send Notouch signal completion --> 'MAIN_LINE_A' ");
                XtraMessageBox.Show($"Send complete", "AcsConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e) //MAIN B
        {
            if (XtraMessageBox.Show($"Do you want to Notouch signal completion?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "NOTOUCH", 0);
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "LOADOUT", 0);
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "UNLOADOUT", 0);

                TraceManager.AddLog($"Cilck Send Notouch signal completion --> 'MAIN_LINE_A' ");
                XtraMessageBox.Show($"Send complete", "AcsConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void button5_Click(object sender, EventArgs e) //차단바 UP
        {
            if (XtraMessageBox.Show($"Do you want to open blockingBar?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_blockingBarTableAdapter BarAdt = new tb_blockingBarTableAdapter();
                BarAdt.UpdateBarMode(1);

                TraceManager.AddLog($"Cilck Send open blockingBar signal completion");
                XtraMessageBox.Show($"Send complete", "AcsConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e) //차단바 DOWN
        {
            if (XtraMessageBox.Show($"Do you want to close brokingBar?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_blockingBarTableAdapter BarAdt = new tb_blockingBarTableAdapter();
                BarAdt.UpdateBarMode(2);

                TraceManager.AddLog($"Cilck Send close blockingBar signal completion");
                XtraMessageBox.Show($"Send complete", "AcsConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }




        //////////////////////// TEST
        private void button7_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show($"Do you want to change auto brokingBar?", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_blockingBarTableAdapter BarAdt = new tb_blockingBarTableAdapter();
                BarAdt.UpdateBarMode(0);

                TraceManager.AddLog($"Cilck Send Auto blockingBar signal completion");
                XtraMessageBox.Show($"Send complete", "AcsConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show($"check subA notouch", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("SUB_LINE_A", "NOTOUCH", 1);
                plcEventAdt.InsertPlcReport("SUB_LINE_A", "LOADOUT", 1);
                plcEventAdt.InsertPlcReport("SUB_LINE_A", "UNLOADOUT", 1);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show($"check subB notouch", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("SUB_LINE_B", "NOTOUCH", 1);
                plcEventAdt.InsertPlcReport("SUB_LINE_B", "LOADOUT", 1);
                plcEventAdt.InsertPlcReport("SUB_LINE_B", "UNLOADOUT", 1);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show($"check mainA notouch", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "NOTOUCH", 2);
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "LOADOUT", 2);
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "UNLOADOUT", 2);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show($"check mainB notouch", "AcsConfig", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                tb_plc_eventTableAdapter plcEventAdt = new tb_plc_eventTableAdapter();
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "NOTOUCH", 2);
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "LOADOUT", 2);
                plcEventAdt.InsertPlcReport("MAIN_LINE_B", "UNLOADOUT", 2);
            }
        }


    }
}

