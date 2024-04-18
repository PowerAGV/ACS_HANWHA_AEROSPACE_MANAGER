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
using System.Data.SqlClient;
using System.Configuration;

namespace ACSManager.Control
{
    public partial class ACSConfigUI : DevExpress.XtraEditors.XtraUserControl
    {
        public ACSConfigUI()
        {
            InitializeComponent();
        }

        private void ACSConfig_Load(object sender, EventArgs e)
        {
            try
            {// input config
                DataSet1MTableAdapters.tb_acs_configTableAdapter adp = new DataSet1MTableAdapters.tb_acs_configTableAdapter();
                DataSet1M.tb_acs_configDataTable tbl =  adp.GetData();
                foreach(DataSet1M.tb_acs_configRow rw in tbl)
                {
                    txtPort.Text = rw.evt_port.ToString();
                    txtAgvAliveCritical.Text = rw.agv_time_out_interval.ToString();
                    txtAliveInterval.Text = rw.alive_interval.ToString();
                    txtBatery.Text = rw.betery_limite.ToString();
                    txtNodeChangeInterval.Text = rw.last_node_interval.ToString();
                    txtTimeOut.Text = rw.comm_timeout.ToString();
                    txtBatteryRecommended.Text = rw.battery_Recommended.ToString();
                    txtDatabaseLog.Text = rw.db_log_delete_interval.ToString();
                    //txtStraightCount.Text = rw.traffic_straight_count.ToString();
                    //txtCurveCount.Text = rw.traffic_curve_count.ToString();
                    txtBatterytlimit.Text = rw.battery_high.ToString();
                    txtBatteryStartCharge.Text = rw.battery_low.ToString();
                    
                    break;
                }
            }
            catch(Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {// input config
                DataSet1MTableAdapters.tb_acs_configTableAdapter adp = new DataSet1MTableAdapters.tb_acs_configTableAdapter();
/*
evt_port = @evt_port, 
betery_limite = @betery_limite, 
alive_interval = @alive_interval, 
last_node_interval = @last_node_interval, 
agv_time_out_interval = @agv_time_out_interval, 
comm_timeout = @comm_timeout,
battery_Recommended = @battery_Recommended
db_log_delete_interval = @db_log_delete_interval
reg_date = getdate(), 
*/


                adp.UpdateQuery
                    (
                     int.Parse(txtPort.Text)
                    , int.Parse(txtBatery.Text)
                    , int.Parse(txtAliveInterval.Text)
                    , int.Parse(txtNodeChangeInterval.Text)
                    , int.Parse(txtAgvAliveCritical.Text)
                    , int.Parse(txtTimeOut.Text)
                    , int.Parse(txtBatteryRecommended.Text)
                    , int.Parse(txtDatabaseLog.Text)
                    , int.Parse(txtStraightCount.Text)
                    , int.Parse(txtCurveCount.Text)
                    , int.Parse(txtBatteryStartCharge.Text)
                    , int.Parse(txtBatterytlimit.Text)

                    ); 
                MessageBox.Show("환경설정 저장 완료", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);

            }
            catch (Exception ee)
            {
                MessageBox.Show("환경설정 저장 실패", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        // 20210306
        private void btnAGVLocationCation_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("AGV 주행 위치변경 진행 하시겠습니까?", "ACS", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string connetionString = ConfigurationManager.ConnectionStrings["ACSManager.Properties.Settings.acsMSSqlConnectionString"].ConnectionString; //ConfigurationSettings.AppSettings["MCS_MSSQL_ConnectionString"].ToString();
                SqlConnection connection = null;
                SqlCommand command = null;
                string sql = "dbo.SP_AgvLocation_Change";


                try
                {
                    connection = new SqlConnection(connetionString);
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    command.CommandType = CommandType.StoredProcedure;


                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();

                    MessageBox.Show("AGV 위치변경 저장 완료", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);

                }
                catch (SqlException sex)
                {
                    //PO_RETCODE = "Error";
                    TraceManager.AddLog(string.Format("{0}r\n{1}", sex.StackTrace, sex.Message));
                }
                catch (Exception ex)
                {
                    //PO_RETCODE = "Error";
                    TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                    if (connection != null)
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                        connection = null;
                    }
                }
            }
        }
    }
}
