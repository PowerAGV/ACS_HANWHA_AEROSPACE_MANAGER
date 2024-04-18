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

namespace ACSManager
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
                //adp.UpdateQuery(int.Parse(txtPort.Text),int.Parse(txtBatery.Text),int.Parse(txtAliveInterval.Text),int.Parse(txtNodeChangeInterval.Text),int.Parse(txtAgvAliveCritical.Text), int.Parse(txtTimeOut.Text));
                MessageBox.Show("Value Change success", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);

            }
            catch (Exception ee)
            {
                MessageBox.Show("Value Change fail", "ACS", MessageBoxButtons.OK, MessageBoxIcon.None);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

        }
    }
}
