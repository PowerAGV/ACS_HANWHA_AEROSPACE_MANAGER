using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ACSManager.FORM
{
    public partial class AGVStatus : DevExpress.XtraEditors.XtraForm
    {
        public string agv_id = "";

        public AGVStatus()
        {
            InitializeComponent();
        }

        private void AGVStatus_Load(object sender, EventArgs e)
        {
            txtAGV_ID.Text = agv_id;
            this.CenterToScreen();
            simpleButton1.PerformClick();
        }

        private void ReloadInfo()
        {
            try
            {
                var adtAGV = new DataSet1MTableAdapters.tb_agvTableAdapter();

                if (txtAGV_ID.Text != null && txtAGV_ID.Text != string.Empty)
                {
                    var result = adtAGV.GetDataByAgvID(txtAGV_ID.Text);
                    if (result != null && result.Rows.Count > 0)
                        SetAgvInformation(result.Rows[0] as DataSet1M.tb_agvRow, false);
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ReloadInfo();

        }

        public void SetAgvInformation(DataSet1M.tb_agvRow agv, bool nodeChange)
        {
            try
            {
                EventHandler eh2 = delegate
                {
                    if(!agv.Iscurrent_statusNull())
                    {
                        if (agv.current_status.Contains("LOAD"))
                        {
                            if (agv.drive_status.Contains("Transfer"))
                            {
                                lbl_agvStatus.Text = "Transfer";
                            }
                            else
                            {
                                lbl_agvStatus.Text = "Pre-Run";
                            }
                        }

                        else if (agv.current_status.Contains("OFF"))
                            lbl_agvStatus.Text = "OFF";
                        else if (agv.current_status.Contains("WAIT"))
                            lbl_agvStatus.Text = "Waiting";
                        else if (agv.current_status.Contains("Parking"))
                            lbl_agvStatus.Text = "Parking";
                        else
                            lbl_agvStatus.Text = agv.current_status;
                    }
                };
                lbl_agvStatus.Invoke(eh2);
                EventHandler eh3 = delegate
                {
                    try
                    {
                        lbl_agvDetail.Text = agv.stop_reason == null ? "" : agv.stop_reason;
                    }
                    catch (Exception)
                    {
                        lbl_agvDetail.Text = "";
                    }
                };
                lbl_agvDetail.Invoke(eh3);
                EventHandler eh4 = delegate
                {
                    try
                    {
                        lbl_agvLastResponse.Text = agv.last_ack_date == null ? "" : agv.last_ack_date.ToString("yyyy/MM/dd hh:mm ");
                    }
                    catch (Exception)
                    {
                        lbl_agvLastResponse.Text = "";
                    }
                };
                lbl_agvLastResponse.Invoke(eh4);

                EventHandler eh5 = delegate
                {
                    lbl_agvLocation.Text = agv.Iscurrent_nodeNull() ? "" : agv.current_node;
                };
                lbl_agvLocation.Invoke(eh5);

                EventHandler eh10 = delegate
                {
                    lbl_agvJobID.Text = agv.Iscurrent_job_idNull() ? "" : agv.current_job_id;
                };
                lbl_agvJobID.Invoke(eh10);

                EventHandler eh11 = delegate
                {
                    lbl_battery.Text = agv.Isbattery_statusNull() ? "" : agv.battery_status;
                };
                lbl_battery.Invoke(eh11);

                EventHandler eh13 = delegate
                {
                    lblTraffic.Text = agv.IstrafficNull() ? "" : agv.traffic;
                };
                lblTraffic.Invoke(eh13);
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }
    }


}