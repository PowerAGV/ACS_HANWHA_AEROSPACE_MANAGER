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
using ACSManager.DataSet1MTableAdapters;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.data.MSSQL;


using ACSManager.FORM;

namespace ACSManager.Control
{
    public partial class OrderCreation : DevExpress.XtraEditors.XtraUserControl
    {

  
        public OrderCreation()
        {
            InitializeComponent();
        }

        public void OrderCreation_Load(object sender, EventArgs e)
        {
            try
            {     
                timer1.Start();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }




    }
}
