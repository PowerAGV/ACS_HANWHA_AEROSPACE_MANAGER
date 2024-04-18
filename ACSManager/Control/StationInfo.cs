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
using ACSManager.FORM;

namespace ACSManager.Control
{
    public partial class StationInfo : DevExpress.XtraEditors.XtraUserControl
    {

        ACSManager.FORM.StationManager stm;

        public StationInfo()
        {
            InitializeComponent();
        }

        public void StationInfo_Load(object sender, EventArgs e)
        {
            try
            {
                this.StationInfoSetting();

                timer1.Start();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        public void CallFunc(Object obj)
        {

            MessageBox.Show("호출");

        }

        private void ChangeStationExist()
        {
            foreach (Object ctlObj in this.pnlStation.Controls)
            {
                if(ctlObj is GroupBox)
                {
                    foreach(Object objChild in ((GroupBox)ctlObj).Controls)
                    {
                        if(objChild is Panel)
                        {
                            //"■" : "□"
                            if (((Label)objChild).Text == "■" || ((Label)objChild).Text == "□")
                            {

                            }
                        }
                    }
                }
            }
        }


        public void RefrashStation(bool isStart)
        {
            if (isStart)
            {
                this.timer1.Start();
                //this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

                this.StationInfoSetting();
            }
            else
            {
                this.timer1.Stop();
                //this.timer1.Tick -= new System.EventHandler(this.timer1_Tick);
            }
            //Thread mulThread = new Thread(delegate () { threadRefrashAgvList(); });
            //mulThread.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            // 1초마다 갱신 한다.
            //RefrashAgvList();
            RefrashStation(true);
        }


        public void StationInfoSetting()
        {
            this.pnlStation.Controls.Clear();

            string sStation = "";
            tb_station_InfoTableAdapter stinfo = new tb_station_InfoTableAdapter();
            DataSet1M.tb_station_InfoDataTable sdt = stinfo.GetStationInfo(sStation);

            DataView view = sdt.DefaultView;

            // 중복을 제외한 결과를 얻을 수 있다.
            // (쿼리문의 SELECT DISTINCT와 동일)
            DataTable distStation = view.ToTable(true, new string[] { "station" });


            string sPreStation = "";
            GroupBox grb = null;

            int igrpX = 0;
            int igrpY = 0;

            // 그룹박스 위에서 port panel의 x,y 좌표
            int iPLX = 56;
            int iPLY = 37;

            int iLoop = 0;

            // Station 내의 각 Port 판넬 넓이, 높이
            int iPNLSizeW = 34;
            int iPNLSizeH = 110;

            // port 번호 라벨의 위치
            int iLbLocX = 7;
            int iLbLocY = 42;

            // Port 의 아래 isExist 표시 라벨 위치
            int iExistLocX = 7;
            int iExistLocY = 79;

            // Port 의 아래 isExist 표시 넓이 , 높이
            int iLbSizeW = 9;
            int iLbSizeH = 9;

            Panel pnl = null;
            Label lblPort = null;
            Label lblExistFlag = null;

            int iGrpBox_Create_Index = 0;

            // GroupBox 생성
            int[,] iStationLocation = new int[3,3];
            iStationLocation[0, 0] = 293;
            iStationLocation[0, 1] = 394;

            iStationLocation[1, 0] = 36;
            iStationLocation[1, 1] = 106;

            iStationLocation[2, 0] = 631;
            iStationLocation[2, 1] = 106;



            foreach (DataRow dr in sdt.Rows)
            {
                

                if (sPreStation == dr["station"].ToString())
                {
                    iLoop++;

                    pnl = new Panel();
                    pnl.Size = new System.Drawing.Size(iPNLSizeW, iPNLSizeH);
                    pnl.Location = new System.Drawing.Point(iPLX * iLoop, iPLY);
                    if (dr["valid"].ToString() == "Y")
                    {
                        pnl.BackColor = dr["kind"].ToString() == "Empty" ? Color.White : Color.Orange;
                    }
                    else
                    {
                        pnl.BackColor =  Color.Gray;
                    }
                    pnl.Click += Pnl_Click;

                    grb.Controls.Add(pnl);
                    grb.Tag = dr["Station"].ToString();


                    lblPort = CreatePortLabel();
                    lblPort.Text = dr["Port"].ToString();
                    lblPort.Location = new System.Drawing.Point(iLbLocX, iLbLocY);
                    lblPort.Name = $"Port_{dr["Port"]}";
                    lblPort.Size = new System.Drawing.Size(iLbSizeW, iLbSizeH);

                    pnl.Controls.Add(lblPort);

                    lblExistFlag = this.CreateOutFlagLabel();
                    lblExistFlag.Text = dr["isExist"].ToString() == "1" ? "■" : "□";
                    lblExistFlag.ForeColor = dr["isExist"].ToString() == "1" ? Color.Red : Color.Black;
                    lblExistFlag.Location = new System.Drawing.Point(iExistLocX, iExistLocY);
                    lblExistFlag.Name = $"Port_{dr["Port"]}";
                    lblExistFlag.Size = new System.Drawing.Size(iLbSizeW, iLbSizeH);

                    pnl.Controls.Add(lblExistFlag);

                    pnl.Tag = dr["StationNO"].ToString();


                }
                else
                {

                    iLoop = 1;


                    Int32.TryParse(dr["X"].ToString(), out igrpX);
                    Int32.TryParse(dr["Y"].ToString(), out igrpY);

                    try
                    {
                        if (igrpX == 0)
                        {
                            igrpX = iStationLocation[iGrpBox_Create_Index, 0];
                            igrpX = iStationLocation[iGrpBox_Create_Index, 1];
                        }
                    }
                    catch {
                        
                    }
                    finally
                    {
                        iGrpBox_Create_Index++;
                    }


                    grb = new GroupBox();
                    grb.Location = new System.Drawing.Point(igrpX, igrpY);
                    grb.Size = new System.Drawing.Size(550, 183);
                    grb.Text = dr["station"].ToString();
                    grb.Paint += groupBox_Paint;

                    pnl = new Panel();
                    pnl.Size = new System.Drawing.Size(iPNLSizeW, iPNLSizeH);
                    pnl.Location = new System.Drawing.Point(iPLX * iLoop, iPLY);
                    if (dr["valid"].ToString() == "Y")
                    {
                        pnl.BackColor = dr["kind"].ToString() == "Empty" ? Color.White : Color.Orange;
                    }
                    else
                    {
                        pnl.BackColor = Color.Gray;
                    }

                    pnl.Click += Pnl_Click;
                    pnl.Tag = dr["StationNO"].ToString();
                    grb.Controls.Add(pnl);

                    this.pnlStation.Controls.Add(grb);

                    lblPort = CreatePortLabel();
                    lblPort.Text = dr["Port"].ToString();
                    lblPort.Location = new System.Drawing.Point(iLbLocX, iLbLocY);
                    lblPort.Name = $"Port_{dr["Port"]}";
                    lblPort.Size = new System.Drawing.Size(iLbSizeW, iLbSizeH);

                    pnl.Controls.Add(lblPort);



                    lblExistFlag = this.CreateOutFlagLabel();
                    lblExistFlag.Text = dr["isExist"].ToString()=="1" ? "■": "□";
                    lblExistFlag.ForeColor = dr["isExist"].ToString() == "1" ? Color.Red : Color.Black;
                    lblExistFlag.Location = new System.Drawing.Point(iExistLocX, iExistLocY);
                    lblExistFlag.Name = $"Port_{dr["Port"]}";
                    lblExistFlag.Size = new System.Drawing.Size(iLbSizeW, iLbSizeH);

                    pnl.Controls.Add(lblExistFlag);

                    
                }




                sPreStation = dr["station"].ToString();
            }


            
            this.grpExample.Paint += groupBox_Paint;


        }

        
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Red, Color.Blue);
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private Label CreatePortLabel()
        {
            Label lblport = new Label();
            lblport.AutoSize = true;
            lblport.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            return lblport;
        }


        private Label CreateOutFlagLabel()
        {
            Label lblOutFlag = new Label();
            lblOutFlag.AutoSize = true;
            lblOutFlag.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            return lblOutFlag;
        }


        public void SetAgvInformation(DataSet1M.tb_agvRow agv,bool nodeChange)
        {
            try
            {
                /*
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
                    var tbRoute = new DataSet1MTableAdapters.tb_final_routeTableAdapter().GetDataByID(agv.agv_id);

                    if(tbRoute != null && tbRoute.Rows.Count > 0)
                    {
                        gridControl1.DataSource = tbRoute.OrderBy(p => p.sequance);
                        gridView1.BestFitColumns();
                    }
                    
                };
                lbl_Traffic.Invoke(eh5);
                */


            }
            catch(Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        #region EVENT FUCTION
        private void Pnl_Click(object sender, EventArgs e)
        {
            try
            {
                this.RefrashStation(false);
                // 팝업창 오픈
                //XtraMessageBox.Show("Station Port 수정", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                string[] sKey = ((Panel)sender).Tag.ToString().Split(new char[] { ',' });

                StationManager sm = new StationManager(this,int.Parse(((Panel)sender).Tag.ToString()));
                sm.ShowDialog();


            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

        }


        private void btnCreateStation_Click(object sender, EventArgs e)
        {
            this.RefrashStation(false);

            StationManager sm = new StationManager(this,0);
            sm.ShowDialog();
        }




        #endregion

       
    }



}
