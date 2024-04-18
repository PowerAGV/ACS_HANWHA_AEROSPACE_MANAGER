using ACSManager.Control;
using ACSManager.data.MSSQL;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.data.MSSQL;

namespace ACSManager
{
    public delegate void DataFilterEventHandler(string area);

    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private int index = 0;

        private Timer timer = new Timer();
        public DataFilterEventHandler DataFilterEvent;

        public static MapDesign.nodeDataTable nodTbl = null;

        public MainForm()
        {
            InitializeComponent();
            // Handling the QueryControl event that will populate all automatically generated Documents
            this.tabbedView1.QueryControl += tabbedView1_QueryControl;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            if (Program.USER_DATA.UserGroup != "MASTER" && Program.USER_DATA.UserGroup != "MANAGER")
            {
                barButtonItem7.Enabled = false;
                barButtonItem9.Enabled = false;
            }

            //barLabel1.Caption = "LOGIN : " + Program.USER_DATA.UserGroup;
            barLabel1.Caption = $"LOGIN: {Program.USER_DATA.UserID}({Program.USER_DATA.UserGroup})";

            timer.Tick += CurrentTimeTimer_Tick;
            timer.Interval = 1000;
            timer.Enabled = true;

            barButtonItem2.PerformClick();

            var riCB = (barEditItem1.Edit as RepositoryItemComboBox);
            riCB.Items.Clear();
            

            //var tb = new DataSet1MTableAdapters.m_node_groupTableAdapter().GetDataByMapID(Setting.MAP_ID);
            //var rows = tb.Select(p => p.group_name).Distinct();

            var rows = Setting.groupNodeJoinTbl.Where(x => x.P_GROUP_NAME == "Route_Area")
                                     .Select(x => x.C_GROUP_NAME).Distinct(); 
                
            riCB.Items.Add("ALL");
            foreach (var item in rows.ToList())
                riCB.Items.Add(item);

            barEditItem1.EditValue = riCB.Items[0];
            

            ShowAlert($"{Program.USER_DATA.UserID}님 환영합니다.");

            timer1.Tick += LogFileDelete_Tick;
            timer1.Interval = 1000 * 60 * 60; // 한시간
            timer1.Enabled = true;


        }

        #region Event Function
        void CurrentTimeTimer_Tick(object sender, EventArgs e)
        {
            this.barLabelDatetime.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }


        

        void LogFileDelete_Tick(object sender, EventArgs e)
        {
            TraceManager.RemoveFileByDays(int.Parse(Setting.LogSaveDay));
        }


        void ribMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item.Tag.ToString() != string.Empty)
            {
                switch (e.Item.Tag.ToString())
                {
                    case "ACSManager.Control.CarInfoList":
                        index = 0;
                        break;
                    case "ACSManager.Control.DriveInfo":
                        index = 1;
                        break;
                    case "ACSManager.Control.AGVEvents":
                        index = 2;
                        break;
                    case "ACSManager.Control.MCSInfo":
                        index = 3;
                        break;
                    case "ACSManager.Control.UseRateCar":
                        index = 4;
                        break;
                    case "ACSManager.Control.PeakRate":
                        index = 5;
                        break;
                    case "ACSManager.Control.MapControl":
                        index = 6;
                        break;
                    case "ACSManager.Control.ChargePointManage":
                        index = 7;
                        break;
                    case "ACSManager.Control.OrderCreation":
                        index = 8;
                        break;
                    case "ACSManager.Control.ACSConfigUI":
                        index = 9;
                        break;
                    case "ACSManager.Control.PlcEvent":
                        index = 10;
                        break;
                    case "ACSManager.Control.ChangeStation":
                        index = 11;
                        break;
                }

                //해당 항목 활성화
            }

            if (e.Item.Tag.ToString() =="Print")
            {
                switch (index)
                {
                    case 0:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.CarInfoList");
                            var Info = documentManager1.View.Documents[result].Control as CarInfoList;
                            Info.PrintGrid();
                            return;
                        }
                    case 1:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.DriveInfo");
                            var Info = documentManager1.View.Documents[result].Control as DriveInfo;
                            Info.PrintGrid();
                            return;
                        }
                    case 2:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.AGVEvents");
                            var Info = documentManager1.View.Documents[result].Control as AGVEvents;
                            Info.PrintGrid();
                            return;
                        }
                    case 3:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.MCSInfo");
                            var Info = documentManager1.View.Documents[result].Control as MCSInfo;
                            Info.PrintGrid();
                            return;
                        }
                    case 4:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.UseRateCar");
                            var Info = documentManager1.View.Documents[result].Control as UseRateCar;
                            Info.PrintGrid();
                            return;
                        }
                    case 5:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.PeakRate");
                            var Info = documentManager1.View.Documents[result].Control as PeakRate;
                            Info.PrintGrid();
                            return;
                        }
                    case 6:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.MapControl");
                            var Info = documentManager1.View.Documents[result].Control as MapControl;
                            //Info.Show();
                            return;
                        }
                    case 7:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.ChargePointManage");
                            var Info = documentManager1.View.Documents[result].Control as ChargePointManage;
                            Info.PrintGrid();
                            return;
                        }
                    case 8:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.OrderCreation");
                            var Info = documentManager1.View.Documents[result].Control as StationInfo;
                            //Info.PrintGrid();
                            return;
                        }
                    case 9:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.ACSConfigUI");
                            var Info = documentManager1.View.Documents[result].Control as ACSConfigUI;
                            //Info.PrintGrid();
                            return;
                        }
                    case 10:
                        {
                            var result = documentManager1.View.Documents.FindIndex(p => p.ControlName == "ACSManager.Control.PlcEvent");
                            var Info = documentManager1.View.Documents[result].Control as PlcEvent;
                            //Info.PrintGrid();
                            return;
                        }
                    default:
                        return;
                }
            }


            ActivateDocument(e.Item.Caption, e.Item.Tag.ToString());
        }

        private void btnFloorChange_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show("Do you want to chage Floor?", "", MessageBoxButtons.OKCancel))
            {
                Setting.FLOOR = 1;

                var table = new mapTableAdapter().GetCurrentMap();

                if (table != null && table.Rows.Count > 0)
                {
                    Setting.MAP_ID = (long)table.Rows[0]["id"];
                }

                var riCB = (barEditItem1.Edit as RepositoryItemComboBox);
                riCB.Items.Clear();
                if (Setting.FLOOR == 1)
                {
                    //var tb = new DataSet1MTableAdapters.m_node_groupTableAdapter().GetDataByMapID(Setting.MAP_ID);
                    //var rows = tb.Select(p => p.group_name).Distinct();

                    var rows = Setting.groupNodeJoinTbl.Where(x => x.P_GROUP_NAME == "Route_Area")
                                             .Select(x => new { x.C_GROUP_NAME }).Distinct();


                    riCB.Items.Add("ALL");
                    foreach (var item in rows.ToList())
                        riCB.Items.Add(item);

                    barEditItem1.EditValue = riCB.Items[0];
                }
                else
                {
                    //var tb = new DataSet1MTableAdapters.m_node_groupTableAdapter().GetDataByMapID(Setting.MAP_ID);
                    //var rows = tb.Select(p => p.group_name).Distinct();

                    var rows = Setting.groupNodeJoinTbl.Where(x => x.P_GROUP_NAME == "Route_Area")
                                             .Select(x => new { x.C_GROUP_NAME }).Distinct();


                    riCB.Items.Add("ALL");
                    foreach (var item in rows.ToList())
                        riCB.Items.Add(item);

                    barEditItem1.EditValue = riCB.Items[0];
                }

                documentManager1.View.Controller.CloseAll();
                barLabel1.Caption = "LOGIN : " + Program.USER_DATA.UserGroup + " / " + "FLOOR : " + Setting.FLOOR;
                barButtonItem2.PerformClick();
            }
        }

        // Assigning a required content for each auto generated Document
        void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {
            if (e.Control == null)
            {
                if (e.Document.Tag == null || e.Document.Tag.ToString() == string.Empty)
                {
                    return;
                }

                var con = Activator.CreateInstance(Type.GetType(e.Document.Tag.ToString()));

                if (con == null)
                {
                    
                }
                else
                {
                    e.Control = con as DevExpress.XtraEditors.XtraUserControl;
                }
            }
            
        }

        public void cbxAreaChanged(object sender, EventArgs e)
        {
            var riCB = (barEditItem1.Edit as RepositoryItemComboBox);
            DataFilterEvent(barEditItem1.EditValue.ToString());
        }
        #endregion

        #region Function
        public void ActivateDocument(string caption, string name)
        {
            // Form일시 폼으로 화면에 띄움
            var con = Activator.CreateInstance(Type.GetType(name));
            if (con as DevExpress.XtraEditors.XtraUserControl == null)
            {
                (con as XtraForm).Show();
                return;
            }


            BaseDocument document = null;

            //기존에 활성화 된 화면이 있는지 확인하여 있으면 그 화면창 뜨게함
            foreach (BaseDocument doc in documentManager1.View.Documents)
            {
                if (doc.ControlName == name)
                {
                    document = doc;
                }
            }

            //없으면 탭 새로 만들어서 띄움
            if (document == null)
            {
                if (caption.Contains("\r\n"))
                {
                    caption = caption.Replace("\r\n", string.Empty);
                }
                document = tabbedView1.AddDocument(caption, name);
                document.Tag = name;
            }

            tabbedView1.Controller.Activate(document);
        }

        //옆의 박스
        public void ShowAlert(string text, string caption = null, Form owner = null, AlertLevel level = AlertLevel.Info)
        {
            if (owner == null) owner = this;

            customAlertControl1.AppearanceCaption.ForeColor = Color.White;
            customAlertControl1.AppearanceText.ForeColor = Color.White;
            customAlertControl1.AppearanceText.Font = new Font("맑은고딕", 12);

            var backColor = Color.RoyalBlue;
            switch (level)
            {
                case AlertLevel.Info:
                    backColor = Color.RoyalBlue;
                    break;
                case AlertLevel.Warning:
                    backColor = Color.DarkOrange;
                    break;
                case AlertLevel.Error:
                    backColor = Color.Crimson;
                    break;
                default:
                    backColor = Color.RoyalBlue;
                    break;
            }

            if (owner.InvokeRequired)
            {
                owner.BeginInvoke(new Action(() =>
                {
                    
                    customAlertControl1.Show(owner, caption, text, null, null, null, backColor);
                }));
            }
            else
            {
                customAlertControl1.Show(owner, caption, text, null, null, null, backColor);
            }
        }

        public enum AlertLevel { Info, Warning, Error }
        #endregion

       
    }
}
