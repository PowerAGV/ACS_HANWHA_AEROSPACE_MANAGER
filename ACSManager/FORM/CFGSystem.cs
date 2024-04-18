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
    public partial class CFGSystem : DevExpress.XtraEditors.XtraForm
    {
        DataSet1M.tb_charge_stationDataTable tbBatteryStation;

        public CFGSystem()
        {
            InitializeComponent();
        }

        private void CFGSystem_Load(object sender, EventArgs e)
        {
            //LoadBatteryData();
            LoadTimeData();

            txtIP.Properties.Mask.EditMask =
                "([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}";

            txtNode.Properties.Mask.EditMask = "[0-9][0-9][0-9][0-9][0-9][0-9][0-9]";

            txtStartHour.Properties.Mask.EditMask = "[0-2][0-9]";
            txtEndHour.Properties.Mask.EditMask = "[0-2][0-9]";

            txtStartMin.Properties.Mask.EditMask = "[0-5][0-9]";
            txtEndMin.Properties.Mask.EditMask = "[0-5][0-9]";
           

        }

        private void LoadBatteryData()
        {
            tbBatteryStation = new DataSet1MTableAdapters.tb_charge_stationTableAdapter().GetData();

            foreach(var row in tbBatteryStation)
            {
                cbxStationName.Properties.Items.Add(row.StationName);
            }
            int index = 0;

            txtCheckTime.Text = tbBatteryStation[index].CheckInterval.ToString();
            txtBatteryLimit.Text = tbBatteryStation[index].PowerLimit.ToString();
            txtIP.Text = tbBatteryStation[index].StationIP;
            txtPort.Text = tbBatteryStation[index].StationPORT.ToString();
            txtNode.Text = tbBatteryStation[index].node;

            cbxStationName.SelectedIndex = index;
        }

        private void LoadTimeData()
        {
            var acsconfig = new DataSet1MTableAdapters.tb_acs_configTableAdapter().GetData();

            var ss = acsconfig.FirstOrDefault();

            txtStartHour.Text = ss.day_start_time.Hours.ToString();
            txtStartMin.Text = ss.day_start_time.Minutes.ToString();

            txtEndHour.Text = ss.day_end_time.Hours.ToString();
            txtEndMin.Text = ss.day_end_time.Minutes.ToString();
        }

        private void cbxStationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbxStationName.SelectedIndex;

            txtCheckTime.Text = tbBatteryStation[index].CheckInterval.ToString();
            txtBatteryLimit.Text = tbBatteryStation[index].PowerLimit.ToString();
            txtIP.Text = tbBatteryStation[index].StationIP;
            txtPort.Text = tbBatteryStation[index].StationPORT.ToString();
            txtNode.Text = tbBatteryStation[index].node;

            cbxStationName.SelectedIndex = index;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want save?", "SAVE", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var id = tbBatteryStation.AsEnumerable().FirstOrDefault(p => p.StationName == cbxStationName.Text).StationID;

                    var adpBatteryStation = new DataSet1MTableAdapters.tb_charge_stationTableAdapter();
                    adpBatteryStation.UpdateQuery(txtIP.Text, int.Parse(txtPort.Text), int.Parse(txtCheckTime.Text), int.Parse(txtBatteryLimit.Text), txtNode.Text, id);

                    var config = new DataSet1MTableAdapters.tb_acs_configTableAdapter();
                    config.UpdateDayTime(txtStartHour.Text + ":" + txtStartMin.Text, txtEndHour.Text + ":" + txtEndMin.Text);

                    DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ee)
            {
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}