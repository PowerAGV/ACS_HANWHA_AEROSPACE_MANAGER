/*
* © SYSCON 
* © TEAM :        SoftWare3
* @ Start Date :  2024.03.01
* @ Project :     AJIN GUEO
* @ Source :      PLCView.cs
*/

#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Windows;
using static ACSManager.DataDefine;

using ACSManager.data.MSSQL;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.DataSetPLCTableAdapters;
using ACSManager.DataSet1MTableAdapters;

using DevExpress.Skins;
using DevExpress.XtraDiagram.Base;
using DevExpress.Utils.Serializing;
using DevExpress.XtraDiagram.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.ButtonPanel;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraDiagram;
using DevExpress.Diagram.Core;

using ImVader;
using ImVader.Algorithms.ShortestPaths;
#endregion

namespace ACSManager.Control
{
    public partial class PLCView : XtraUserControl
    {
        #region value

        long map_id = 0;
        bool isworking_AGV = false;
        List<agvInfo> m_agv = new List<agvInfo>();
        List<MakeGUI> arrayGUI = new List<MakeGUI>();

        List<DiagramShape> subLineComm = new List<DiagramShape>();
        List<DiagramShape> subLineInfoA = new List<DiagramShape>();
        List<DiagramShape> subLineInfoB = new List<DiagramShape>();
        List<DiagramShape> mainLineComm = new List<DiagramShape>();
        List<DiagramShape> mainLineInfoA = new List<DiagramShape>();
        List<DiagramShape> mainLineInfoB = new List<DiagramShape>();

        DiagramShape shape_subline_Auto = new DiagramShape();
        DiagramShape shape_subline_Manual = new DiagramShape();
        DiagramShape shape_subline_Abnormal = new DiagramShape();
        DiagramShape shape_subline_Emergency = new DiagramShape();

        DiagramShape shape_mainline_Auto = new DiagramShape();
        DiagramShape shape_mainline_Manual = new DiagramShape(); 
        DiagramShape shape_mainline_Abnormal = new DiagramShape(); 
        DiagramShape shape_mainline_Emergency = new DiagramShape(); 

        DiagramShape shape_sublineA_pltOn = new DiagramShape(); 
        DiagramShape shape_sublineA_CallAGV = new DiagramShape(); 
        DiagramShape shape_sublineA_CarInfo = new DiagramShape(); 
        DiagramShape shape_sublineA_pltPartOn = new DiagramShape(); 
        DiagramShape shape_sublineA_pltPartCnt = new DiagramShape();
        
        DiagramShape shape_sublineB_pltOn = new DiagramShape(); 
        DiagramShape shape_sublineB_CallAGV = new DiagramShape(); 
        DiagramShape shape_sublineB_CarInfo = new DiagramShape(); 
        DiagramShape shape_sublineB_pltPartOn = new DiagramShape(); 
        DiagramShape shape_sublineB_pltPartCnt = new DiagramShape();

        DiagramShape shape_mainlineA_pltOn = new DiagramShape();
        DiagramShape shape_mainlineA_CallAGV = new DiagramShape();
        DiagramShape shape_mainlineA_CarInfo = new DiagramShape();
        DiagramShape shape_mainlineA_pltPartOn = new DiagramShape();
        DiagramShape shape_mainlineA_pltPartCnt = new DiagramShape();

        DiagramShape shape_mainlineB_pltOn = new DiagramShape();
        DiagramShape shape_mainlineB_CallAGV = new DiagramShape();
        DiagramShape shape_mainlineB_CarInfo = new DiagramShape();
        DiagramShape shape_mainlineB_pltPartOn = new DiagramShape();
        DiagramShape shape_mainlineB_pltPartCnt = new DiagramShape();

        DiagramShape shape_blockingBar_R = new DiagramShape();
        DiagramShape shape_blockingBar_L = new DiagramShape();
        
        DiagramShape shape_Aram_PltSitDown_SubA = new DiagramShape();
        DiagramShape shape_Aram_PltSitDown_SubB = new DiagramShape();
        DiagramShape shape_Aram_PltSitDown_MainA = new DiagramShape();
        DiagramShape shape_Aram_PltSitDown_MainB = new DiagramShape();

        public List<NodeInfo> m_listNodeDate = new List<NodeInfo>();
        public List<LinkInfo> m_listLinkDate = new List<LinkInfo>();
        
        public List<NodeInfo> m_listTempNodeDate = new List<NodeInfo>();
        public List<LinkInfo> m_listTempLinkDate = new List<LinkInfo>();
        #endregion

        MakeGUI makeGUI = new MakeGUI();

        public PLCView()
        {
            InitializeComponent();
        }

        private void PLCView_Load(object sender, EventArgs e)
        {
            //Map
            LoadMapInfo();
            LoadMapLink(map_id);
            LoadMapNode(map_id);

            //Node,Link
            AddNodeFromList();
            AddLinkFromList();
            
            loadGUI();
            
            //AGV
            reloadAgvInfo();
            
            DIAGRAM_GUI.FitToDrawing();  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            try
            {
                changeData_DiagramShape();
                reloadAgvInfo();
                displayAGV();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
            timer1.Enabled = true;
        }

        #region Map

        void LoadMapInfo()
        {
            globalRouteInfo.nodes.Clear();
            globalRouteInfo.m_linkes.Clear();

            this.map_id = Setting.MAP_ID; //맵ID 받아오기

            if (map_id == 0) //맵ID 없을때
            {
                Console.WriteLine("지정된 맵이 없습니다.");
                return;
            }

            Link_Node_JoinTableAdapter lnjAdt = new Link_Node_JoinTableAdapter();
            Setting.linkNodeJoinTbl = lnjAdt.GetLinkNode(map_id);

            foreach (MapDesign.Link_Node_JoinRow rw in Setting.linkNodeJoinTbl)
            {
                int f = globalRouteInfo.nodes.FindIndex(x => x == rw.FROM_NODE_NAME);
                int t = globalRouteInfo.nodes.FindIndex(x => x == rw.TO_NODE_NAME);

                if (f == -1) globalRouteInfo.nodes.Add(rw.FROM_NODE_NAME);
                if (t == -1) globalRouteInfo.nodes.Add(rw.TO_NODE_NAME);

                f = globalRouteInfo.nodes.FindIndex(x => x == rw.FROM_NODE_NAME);
                t = globalRouteInfo.nodes.FindIndex(x => x == rw.TO_NODE_NAME);

                if (f != -1 && t != -1)
                {
                    int revaeseAngle = 0;
                    Angle CA = new Angle(180);
                    Angle LA = new Angle(rw.ANGLE);
                    Angle a3 = CA + LA;
                    revaeseAngle = (int)a3.Degrees360;
                    if (revaeseAngle == 360)
                        revaeseAngle = 0;

                    // sensor on off 판단 넣기
                    bool FromTo_isSensorOnOff = false;
                    bool ToFrom_isSensorOnOff = false;

                    if (rw.DIRECTION == $"{LinkDirection.TwoWay}")
                    {
                        globalRouteInfo.m_linkes.Add(new LinkInfoD(f, t, (int)rw.ANGLE, rw.SPEED.ToString(), FindNodeNameFromIndex(f), FindNodeNameFromIndex(t), FromTo_isSensorOnOff, rw.LINK_ID.ToString()));
                        globalRouteInfo.m_linkes.Add(new LinkInfoD(t, f, revaeseAngle, rw.SPEED.ToString(), FindNodeNameFromIndex(t), FindNodeNameFromIndex(f), ToFrom_isSensorOnOff, rw.LINK_ID.ToString()));
                    }
                    else if (rw.DIRECTION == $"{LinkDirection.OneWay}")
                    {
                        globalRouteInfo.m_linkes.Add(new LinkInfoD(f, t, (int)rw.ANGLE, rw.SPEED.ToString(), FindNodeNameFromIndex(f), FindNodeNameFromIndex(t), ToFrom_isSensorOnOff, rw.LINK_ID.ToString()));
                    }
                }
            }

            globalRouteInfo.m_path = new DirectedListGraph<int, WeightedEdge>(globalRouteInfo.nodes.Count + 2); // 안전을 위해 2 추가

            foreach (MapDesign.Link_Node_JoinRow rw in Setting.linkNodeJoinTbl)
            {
                //rw.angle

                int f = globalRouteInfo.nodes.FindIndex(x => x == rw.FROM_NODE_NAME);
                int d = globalRouteInfo.nodes.FindIndex(x => x == rw.TO_NODE_NAME);

                int fcnt = globalRouteInfo.nodes.FindAll(x => x == rw.FROM_NODE_NAME).Count();
                int dcnt = globalRouteInfo.nodes.FindAll(x => x == rw.TO_NODE_NAME).Count();

                int fadd = 0;
                int dadd = 0;

                if (fcnt > 2)
                    fadd = 100;
                if (dcnt > 2)
                    dadd = 100;

                // TwoWay 양방향
                if (rw.DIRECTION == $"{LinkDirection.TwoWay}")
                {
                    globalRouteInfo.m_path.AddEdge(new WeightedEdge(f, d, rw.LENGTH + fadd));
                    globalRouteInfo.m_path.AddEdge(new WeightedEdge(d, f, rw.LENGTH + dadd));
                }
                else if (rw.DIRECTION == $"{LinkDirection.OneWay}")
                {
                    globalRouteInfo.m_path.AddEdge(new WeightedEdge(f, d, rw.LENGTH + dadd));
                }
            }
        }

        public bool LoadMapLink(long MapID)
        {
            bool bRet = false;

            try
            {
                Link_Node_JoinTableAdapter lnjAdt = new Link_Node_JoinTableAdapter();

                MapDesign.Link_Node_JoinDataTable tb = lnjAdt.GetLinkNode(MapID);
                int nRowCnt = tb.Rows.Count;

                for (int nIndex = 0; nIndex < nRowCnt; nIndex++)
                {
                    MapDesign.Link_Node_JoinRow row = (MapDesign.Link_Node_JoinRow)tb.Rows[nIndex];

                    LinkInfo LinkInfoTemp = new LinkInfo();
                    LinkInfoTemp.strId = row.LINK_ID.ToString();
                    LinkInfoTemp.Direction = row.DIRECTION;
                    LinkInfoTemp.nSpeed = int.Parse(row.SPEED.ToString());
                    LinkInfoTemp.strFrom_Node = row.FROM_NODE_NAME;
                    LinkInfoTemp.strTo_Node = row.TO_NODE_NAME;

                    if (row.LENGTH == 0)
                    {
                        LinkInfoTemp.nLength = 1000;
                    }
                    else
                    {
                        LinkInfoTemp.nLength = row.LENGTH; // 사이간격 확대용
                    }
                    LinkInfoTemp.nAngle = row.ANGLE;

                    m_listTempLinkDate.Add(LinkInfoTemp); //데이터 추가
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return bRet;
        }

        public bool LoadMapNode(long lMapID)
        {
            bool bRet = false;

            try
            {
                foreach (MapDesign.nodeRow row in Setting.nodTbl)
                {

                    NodeInfo NodeInfoTemp = new NodeInfo();
                    NodeInfoTemp.sNode_Name = row.name;
                    NodeInfoTemp.sType = row.node_type;
                    NodeInfoTemp.ptPos = DBStringToPointFloat((int)row.x, (int)row.y);
                    NodeInfoTemp.NodeObj = null;

                    m_listTempNodeDate.Add(NodeInfoTemp);
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return bRet;
        }
        #endregion

        #region Node/Link

        public bool AddNodeFromList()
        {
            bool bRet = false;

            try
            {
                for (int nIndex = 0; nIndex < m_listTempNodeDate.Count; nIndex++)
                {
                    NodeInfo NodeInfoTemp = new NodeInfo();
                    NodeInfoTemp.Clear();


                    PointFloat ptTemp = m_listTempNodeDate[nIndex].ptPos;

                    // 반지름 수치 15 보정
                    ptTemp.X = ptTemp.X + 15;
                    ptTemp.Y = ptTemp.Y + 15;

                    NodeInfoTemp.ptPos = ptTemp;
                    NodeInfoTemp.sNode_Name = m_listTempNodeDate[nIndex].sNode_Name;
                    NodeInfoTemp.sType = m_listTempNodeDate[nIndex].sType;

                    //노드그리기
                    NodeInfoTemp.NodeObj = AddNodeOnDisplay(ref DIAGRAM_GUI, NodeInfoTemp);
                    m_listNodeDate.Add(NodeInfoTemp);
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return bRet;
        }

        public bool AddLinkFromList()
        {
            bool bRet = false;

            try
            {
                for (int nIndex = 0; nIndex < m_listTempLinkDate.Count; nIndex++)
                {
                    DiagramConnector dcon = new DiagramConnector();

                    if (0.0f == m_listTempLinkDate[nIndex].ptPos1.X &&
                        0.0f == m_listTempLinkDate[nIndex].ptPos1.Y &&
                        0.0f == m_listTempLinkDate[nIndex].ptPos2.X &&
                        0.0f == m_listTempLinkDate[nIndex].ptPos2.Y)
                    {
                        dcon.Type = ConnectorType.Straight;
                    }
                    else
                    {
                        dcon.Type = ConnectorType.Curved;
                        IList<DevExpress.Utils.PointFloat> lstPoints = new List<DevExpress.Utils.PointFloat>();
                        lstPoints.Add(m_listTempLinkDate[nIndex].ptPos1);
                        lstPoints.Add(m_listTempLinkDate[nIndex].ptPos2);
                        PointCollection Points = new PointCollection(lstPoints);
                        dcon.Points = Points;
                    }

                    dcon.Appearance.BackColor = Color.Red;
                    dcon.Appearance.BorderColor = m_listTempLinkDate[nIndex].ColorLink;
                    dcon.Tag = m_listTempLinkDate[nIndex].strId;
                    dcon.BeginItem = GetNodeItemInDiagram(DIAGRAM_GUI, m_listTempLinkDate[nIndex].strFrom_Node);
                    dcon.EndItem = GetNodeItemInDiagram(DIAGRAM_GUI, m_listTempLinkDate[nIndex].strTo_Node);
                    dcon.CanResize = true;
                    dcon.BeginArrow = ArrowDescriptions.ClosedDot;
                    dcon.EndArrow = ArrowDescriptions.ClosedDot;
                    dcon.Appearance.BorderSize = 1;
                    dcon.CanMove = false;
                    dcon.CanDragBeginPoint = false;
                    dcon.CanDragEndPoint = false;
                    dcon.CanEdit = false;


                    if ($"{LinkDirection.TwoWay}" == m_listTempLinkDate[nIndex].Direction)
                    {
                        dcon.BeginArrow = ArrowDescriptions.Filled90;
                        dcon.EndArrow = ArrowDescriptions.Filled90;
                    }
                    else if ($"{LinkDirection.OneWay}" == m_listTempLinkDate[nIndex].Direction)
                    {
                        dcon.BeginArrow = ArrowDescriptions.ClosedDot;
                        dcon.EndArrow = ArrowDescriptions.Filled90;
                        //dcon.BeginArrow = ArrowDescriptions.Filled90;
                        //dcon.EndArrow = ArrowDescriptions.ClosedDot;
                    }
                    else
                    {
                        dcon.BeginArrow = ArrowDescriptions.ClosedDot;
                        dcon.EndArrow = ArrowDescriptions.ClosedDot;
                    }

                    DIAGRAM_GUI.Items.Add(dcon);

                    LinkInfo linkInfoTemp = new LinkInfo();
                    linkInfoTemp.Clear();
                    linkInfoTemp.strFrom_Node = m_listTempLinkDate[nIndex].strFrom_Node; // dcon.BeginItem;
                    linkInfoTemp.strTo_Node = m_listTempLinkDate[nIndex].strTo_Node; // dcon.EndItem;

                    linkInfoTemp.strId = m_listTempLinkDate[nIndex].strId;
                    linkInfoTemp.Direction = m_listTempLinkDate[nIndex].Direction;

                    linkInfoTemp.ColorLink = m_listTempLinkDate[nIndex].ColorLink;
                    linkInfoTemp.nSpeed = m_listTempLinkDate[nIndex].nSpeed;
                    linkInfoTemp.nLength = m_listTempLinkDate[nIndex].nLength;
                    linkInfoTemp.ptPos1 = m_listTempLinkDate[nIndex].ptPos1;
                    linkInfoTemp.ptPos2 = m_listTempLinkDate[nIndex].ptPos2;
                    linkInfoTemp.nAngle = m_listTempLinkDate[nIndex].nAngle;
                    linkInfoTemp.sensorL = m_listTempLinkDate[nIndex].sensorL;
                    linkInfoTemp.sensorR = m_listTempLinkDate[nIndex].sensorR;

                    m_listLinkDate.Add(linkInfoTemp);
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return bRet;
        }
        #endregion


        void loadGUI()
        {
            // ▼ subLine Comm
            make_subLine_Frame();
            make_subLine_Auto();
            make_subLine_Manual();
            make_subLine_Abnormal();
            make_subLine_Emergency();

            // ▼ subLineA Info
            make_subLineA_CallAGV();
            make_subLineA_pltOn();
            make_subLineA_CarInfo();
            make_subLineA_pltPartOn();
            make_subLineA_pltPartCnt();

            // ▼ subLineB Info
            make_subLineB_CallAGV();
            make_subLineB_pltOn();
            make_subLineB_CarInfo();
            make_subLineB_pltPartOn();
            make_subLineB_pltPartCnt();

            // ▼ mainLine Comm
            make_mainLine_Frame();
            make_mainLine_Auto();
            make_mainLine_Manual();
            make_mainLine_Abnormal();
            make_mainLine_Emergency();

            // ▼ mainLineA Info
            make_mainLineA_CallAGV();
            make_mainLineA_pltOn();
            make_mainLineA_CarInfo();
            make_mainLineA_pltPartOn();
            make_mainLineA_pltPartCnt();

            // ▼ mainLineB Info
            make_mainLineB_CallAGV();
            make_mainLineB_pltOn();
            make_mainLineB_CarInfo();
            make_mainLineB_pltPartOn();
            make_mainLineB_pltPartCnt();

            // ▼ brokingBar
            make_blockingBar_R();
            make_blockingBar_L();

            // ▼ pltSitDownAram
            make_Aram_PltSitDown_SubA();
            make_Aram_PltSitDown_SubB();
            make_Aram_PltSitDown_MainA();
            make_Aram_PltSitDown_MainB();

            foreach (MakeGUI GUI in arrayGUI)
            {
                DiagramShape ds = GUI.ds;
                DIAGRAM_GUI.Items.Add(ds);
            }

            for (int idx = 0; idx < DIAGRAM_GUI.Items.Count; idx++)
            {
                if (DIAGRAM_GUI.Items[idx].GetType().Name == "DiagramShape")
                {
                    DiagramShape ds = new DiagramShape();
                    ds = (DiagramShape)DIAGRAM_GUI.Items[idx];

                    ds.Appearance.Font = new Font("Tahoma", 4);
                    ds.Appearance.ForeColor = Color.Black;

                    string dsTag = ds.Tag.ToString();

                    if (dsTag.Contains("subLine_Auto")) shape_subline_Auto = ds;
                    else if (dsTag.Contains("subLine_Manual")) shape_subline_Manual = ds;
                    else if (dsTag.Contains("subLine_Abnormal")) shape_subline_Abnormal = ds;
                    else if (dsTag.Contains("subLine_Emergency")) shape_subline_Emergency = ds;

                    else if (dsTag.Contains("mainLine_Auto")) shape_mainline_Auto = ds;
                    else if (dsTag.Contains("mainLine_Manual")) shape_mainline_Manual = ds;
                    else if (dsTag.Contains("mainLine_Abnormal")) shape_mainline_Abnormal = ds;
                    else if (dsTag.Contains("mainLine_Emergency")) shape_mainline_Emergency = ds;

                    else if (dsTag.Contains("subLineA_pltOn")) shape_sublineA_pltOn = ds;
                    else if (dsTag.Contains("subLineA_CallAGV")) shape_sublineA_CallAGV = ds;
                    else if (dsTag.Contains("subLineA_CarInfo")) shape_sublineA_CarInfo = ds;
                    else if (dsTag.Contains("subLineA_pltPartOn")) shape_sublineA_pltPartOn = ds;
                    else if (dsTag.Contains("subLineA_pltPartCnt")) shape_sublineA_pltPartCnt = ds;

                    else if (dsTag.Contains("subLineB_pltOn")) shape_sublineB_pltOn = ds;
                    else if (dsTag.Contains("subLineB_CallAGV")) shape_sublineB_CallAGV = ds;
                    else if (dsTag.Contains("subLineB_CarInfo")) shape_sublineB_CarInfo = ds;
                    else if (dsTag.Contains("subLineB_pltPartOn")) shape_sublineB_pltPartOn = ds;
                    else if (dsTag.Contains("subLineB_pltPartCnt")) shape_sublineB_pltPartCnt = ds;

                    else if (dsTag.Contains("mainLineA_pltOn")) shape_mainlineA_pltOn = ds;
                    else if (dsTag.Contains("mainLineA_CallAGV")) shape_mainlineA_CallAGV = ds;
                    else if (dsTag.Contains("mainLineA_CarInfo")) shape_mainlineA_CarInfo = ds;
                    else if (dsTag.Contains("mainLineA_pltPartOn")) shape_mainlineA_pltPartOn = ds;
                    else if (dsTag.Contains("mainLineA_pltPartCnt")) shape_mainlineA_pltPartCnt = ds;

                    else if (dsTag.Contains("mainLineB_pltOn")) shape_mainlineB_pltOn = ds;
                    else if (dsTag.Contains("mainLineB_CallAGV")) shape_mainlineB_CallAGV = ds;
                    else if (dsTag.Contains("mainLineB_CarInfo")) shape_mainlineB_CarInfo = ds;
                    else if (dsTag.Contains("mainLineB_pltPartOn")) shape_mainlineB_pltPartOn = ds;
                    else if (dsTag.Contains("mainLineB_pltPartCnt")) shape_mainlineB_pltPartCnt = ds;

                    else if (dsTag.Contains("blockingBar_R")) shape_blockingBar_R = ds;
                    else if (dsTag.Contains("blockingBar_L")) shape_blockingBar_L = ds;

                    else if (dsTag.Contains("Aram_PltSitDown_SubA")) shape_Aram_PltSitDown_SubA = ds;
                    else if (dsTag.Contains("Aram_PltSitDown_SubB")) shape_Aram_PltSitDown_SubB = ds;
                    else if (dsTag.Contains("Aram_PltSitDown_MainA")) shape_Aram_PltSitDown_MainA = ds;
                    else if (dsTag.Contains("Aram_PltSitDown_MainB")) shape_Aram_PltSitDown_MainB = ds;

                    if (dsTag.Contains("Auto")) ds.Content = "자동";
                    else if (dsTag.Contains("Manual")) ds.Content = "수동";
                    else if (dsTag.Contains("Abnormal")) ds.Content = "정상";
                    else if (dsTag.Contains("Emergency")) ds.Content = "비상";
                    else if (dsTag.Contains("Frame")) ds.Content = "";
                    else if (dsTag.Contains("CallAGV")) ds.Content = "AGV호출";
                    else if (dsTag.Contains("CarInfo")) ds.Content = "차종";
                    else if (dsTag.Contains("pltOn")) ds.Content = "팔레트 ON";
                    else if (dsTag.Contains("pltPartOn")) ds.Content = "팔레트 파트ON";
                    else if (dsTag.Contains("blocking")) ds.Content = "차단바";
                    else if (dsTag.Contains("PltSitDown")) ds.Content = "안착 불량";
                    else ds.Content = dsTag;
                }
            }
        }
        
        #region Timer
        void changeData_DiagramShape()
        {
            try
            {
                tb_plc_subLine_CommTableAdapter sublineCommAdt = new tb_plc_subLine_CommTableAdapter();
                tb_plc_mainLine_CommTableAdapter mainlineCommAdt = new tb_plc_mainLine_CommTableAdapter();
                tb_plc_subLine_InfoTableAdapter sublineInfoAdt = new tb_plc_subLine_InfoTableAdapter();
                tb_plc_mainLine_InfoTableAdapter mainlineInfoAdt = new tb_plc_mainLine_InfoTableAdapter();
                tb_blockingBarTableAdapter blockingBarAdt = new tb_blockingBarTableAdapter();
                tb_plc_pltSitDownTableAdapter pltSitDownAdt = new tb_plc_pltSitDownTableAdapter();

                foreach (DataSetPLC.tb_plc_subLine_CommRow Row in sublineCommAdt.GetDataByNolock())
                { // Sub Comm
                    changeShapeColor(Row.isAuto, shape_subline_Auto);
                    changeShapeColor(Row.isManual, shape_subline_Manual);
                    changeShapeColor(Row.abNormal, shape_subline_Abnormal);
                    changeShapeColor(Row.emergency, shape_subline_Emergency);
                }
                foreach (DataSetPLC.tb_plc_mainLine_CommRow Row in mainlineCommAdt.GetDataByNolock())
                { // Main Comm
                    changeShapeColor(Row.isAuto, shape_mainline_Auto);
                    changeShapeColor(Row.isManual, shape_mainline_Manual);
                    changeShapeColor(Row.abNormal, shape_mainline_Abnormal);
                    changeShapeColor(Row.emergency, shape_mainline_Emergency);
                }
                foreach (DataSetPLC.tb_plc_subLine_InfoRow Row in sublineInfoAdt.GetDataByNolock())
                {
                    if(Row.node == "0000240") // Sub A
                    {
                        changeShapeColor(Row.pltON, shape_sublineA_pltOn);
                        changeShapeColor(Row.callAGV, shape_sublineA_CallAGV);
                        changeShapeText(Row.carInfo, shape_sublineA_CarInfo);
                        changeShapeColor(Row.pltPartON, shape_sublineA_pltPartOn);
                        changeShapeText(Row.pltPartCnt.ToString(), shape_sublineA_pltPartCnt);
                    }
                    else if(Row.node == "0000540")// Sub B
                    {
                        changeShapeColor(Row.pltON, shape_sublineB_pltOn);
                        changeShapeColor(Row.callAGV, shape_sublineB_CallAGV);
                        changeShapeText(Row.carInfo, shape_sublineB_CarInfo);
                        changeShapeColor(Row.pltPartON, shape_sublineB_pltPartOn);
                        changeShapeText(Row.pltPartCnt.ToString(), shape_sublineB_pltPartCnt);
                    }
                }
                foreach (DataSetPLC.tb_plc_mainLine_InfoRow Row in mainlineInfoAdt.GetDataByNolock())
                {
                    if (Row.node == "0000120") // Main A
                    {
                        changeShapeColor(Row.pltON, shape_mainlineA_pltOn);
                        changeShapeColor(Row.callAGV, shape_mainlineA_CallAGV);
                        changeShapeText(Row.carInfo, shape_mainlineA_CarInfo);
                        changeShapeColor(Row.pltPartON, shape_mainlineA_pltPartOn);
                        changeShapeText(Row.pltPartCnt.ToString(), shape_mainlineA_pltPartCnt);
                    }
                    else if (Row.node == "0000420") // Main B
                    {
                        changeShapeColor(Row.pltON, shape_mainlineB_pltOn);
                        changeShapeColor(Row.callAGV, shape_mainlineB_CallAGV);
                        changeShapeText(Row.carInfo, shape_mainlineB_CarInfo);
                        changeShapeColor(Row.pltPartON, shape_mainlineB_pltPartOn);
                        changeShapeText(Row.pltPartCnt.ToString(), shape_mainlineB_pltPartCnt);
                    }
                }
                foreach (DataSet1M.tb_blockingBarRow Row in blockingBarAdt.GetData())
                {
                    changeShapeColor(Row.bleakBarMode, shape_blockingBar_R);
                    changeShapeColor(Row.bleakBarMode, shape_blockingBar_L);
                }
                foreach (DataSetPLC.tb_plc_pltSitDownRow Row in pltSitDownAdt.GetData())
                {
                    if (Row.Area == "SUB_LINE_A")
                    {
                        changeShapeColor(Row.Signal, shape_Aram_PltSitDown_SubA);
                    }
                    else if (Row.Area == "SUB_LINE_B")
                    {
                        changeShapeColor(Row.Signal, shape_Aram_PltSitDown_SubB);
                    }
                    else if (Row.Area == "MAIN_LINE_A")
                    {
                        changeShapeColor(Row.Signal, shape_Aram_PltSitDown_MainA);
                    }
                    else if (Row.Area == "MAIN_LINE_B")
                    {
                        changeShapeColor(Row.Signal, shape_Aram_PltSitDown_MainB);
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        void reloadAgvInfo()
        {
            try
            {
                DataSet1MTableAdapters.tb_agvTableAdapter tagv = new DataSet1MTableAdapters.tb_agvTableAdapter();
                DataSet1M.tb_agvDataTable agTbl = tagv.GetData();
                lock (m_agv)
                {
                    foreach (DataSet1M.tb_agvRow item in agTbl)
                    {
                        int fidx = m_agv.FindIndex(x => x.agv_id == item.agv_id);
                        if (fidx != -1)
                        {
                            m_agv[fidx].fromto = item.current_route;
                            m_agv[fidx].location = item.current_node;
                            m_agv[fidx].status = item.current_status;
                            //m_agv[fidx].angle = int.Parse(GetGQStatus(item.agv_desc, "ANGLE")); AGV에서 각도값 안올려줌
                            m_agv[fidx].lift = GetGQStatus(item.agv_desc, "LIFT");

                            string agv_desc = item.agv_desc;
                            string agv_Status = this.GetGQStatus(agv_desc, "ACS AGV Status");
                            if (agv_Status != "Charge" && agv_Status != "Prepared")
                            {
                                isworking_AGV = true;
                            }
                            else
                            {
                                isworking_AGV = false;
                            }
                        }
                        else
                        {
                            m_agv.Add(new agvInfo(item.agv_id, item.current_node, item.current_route, 0, item.current_status, "DOWN"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }

        int agv_degree = 0;
        //string preLocation = "";
        //int pre_agv_degree = 270;
        private object changeDegree = new object();
        void displayAGV()
        {
            foreach (agvInfo item in m_agv)
            {
                if (item.status != "OFF")
                {
                    if (isExist(item.agv_id))
                    {
                        SetAgvPoint(item.agv_id, item.lift, agv_degree);
                    }
                    else
                    {
                        makecar(item.agv_id, GetNodePosition(item.location), agv_degree);
                    }
                }
             
            }
        }
        #endregion


        //-------------------------------------------Method-----------------------------------------//
        public DiagramShape AddNodeOnDisplay(ref DevExpress.XtraDiagram.DiagramControl diagramControl, NodeInfo nodeData)
        {
            DiagramShape diagramItem = null;

            try
            {
                diagramItem = new DiagramShape();
                diagramItem.Height = 20;//20
                diagramItem.Width = 25;//20
                diagramItem.Position = new DevExpress.Utils.PointFloat(nodeData.ptPos.X - 10, nodeData.ptPos.Y - 10);

                diagramItem.Shape = BasicShapes.Ellipse;

                //▼ 랙방에 해당할 때
                if(nodeData.sType == "BackwardStation" || nodeData.sType == "ForwardStation")
                {
                    diagramItem.Shape = BasicShapes.Rectangle;
                    diagramItem.Height = 25;
                    diagramItem.Width = 30;
                }
                diagramItem.Appearance.BackColor = GetNodeVisibleColor(nodeData.sType); // 배경색
                diagramItem.Appearance.ForeColor = GetNodeVisibleColor(nodeData.sType); // 전경색

                //▼ 설정 세팅
                diagramItem.CanResize   = false;
                diagramItem.Tag         = nodeData.sNode_Name;
                diagramItem.CanMove     = false;
                diagramItem.CanDelete   = false;
                diagramItem.CanCopy     = false;
                diagramItem.CanEdit     = false;
                diagramItem.CanResize   = false;
                diagramItem.CanSelect   = false;

                //text설정
                //diagramItem.Content = GetNodeVisibleContent(nodeData.sType);

                diagramControl.Items.Add(diagramItem); //다이어그램 컨트롤 아이템에 추가
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
            return diagramItem;
        }
        
        public Color GetNodeVisibleColor(string NodeType)
        {
            Color colorNode = Color.PowderBlue;

            switch (NodeType)
            {
                case "Void":
                    colorNode = Color.Silver;
                    break;
                case "Index":
                    colorNode = Color.PowderBlue;
                    break;
                case "ForwardStation":
                    colorNode = Color.DarkOrange;
                    break;
                case "BackwardStation":
                    colorNode = Color.PowderBlue;
                    break;
                default:
                    colorNode = Color.PowderBlue;
                    break;
            }
            return colorNode;
        }

        public string GetNodeVisibleContent(string nNodeType)
        {
            string strContent = "I";

            switch (nNodeType)
            {
                case "Void":
                    strContent = "V";
                    break;
                case "Index":
                    strContent = "I";
                    break;
                case "BackwardStation":
                    strContent = "B";
                    break;
                case "ForwardStation":
                    strContent = "F";
                    break;

                default:
                    strContent = "I";
                    break;
            }
            return strContent;
        }

        public DiagramShape GetNodeItemInDiagram(DiagramControl diagramControl, string strTag)
        {
            int nItemIndex = -1;
            DiagramShape diagramItem = null;
            //strTag = strTag + "NODE";

            try
            {
                nItemIndex = diagramControl.Items.FindIndex(t => t.Tag.ToString() == strTag);

                diagramItem = new DiagramShape();
                diagramItem = (DiagramShape)diagramControl.Items[nItemIndex];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not find Node " + strTag);
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return diagramItem;
        }

        void makecar(string carname, PointF pos, float ang)
        {
            DiagramShapeEx nitm = null;

            nitm = new DiagramShapeEx(BasicShapes.Pentagon, pos.X, pos.Y, 35, 35, carname) { ImagePath = "car.png" };

            nitm.Tag = carname;
            nitm.Angle = ang;
            nitm.CanResize = false;
            nitm.CanRotate = false;
            nitm.CanMove = false;

            nitm.Appearance.Font = new Font("Tahoma", 6);
            nitm.Appearance.BackColor = Color.White;
            nitm.Appearance.ForeColor = Color.Black;

            DIAGRAM_GUI.Items.Add(nitm);
        }

        bool isExist(string agvID)
        {
            int idx = DIAGRAM_GUI.Items.FindIndex(x => x.Tag.ToString() == agvID);
            if (idx != -1)
                return true;
            else
                return false;
        }

        void SetAgvPoint(string agvID, string lift, int angle)
        {
            int fidx = DIAGRAM_GUI.Items.FindIndex(x => x.Tag.ToString() == agvID);

            if (fidx != -1)
            {
                agvInfo av = m_agv.Find(x => x.agv_id == agvID);
                DIAGRAM_GUI.Items[fidx].Position = new PointFloat(GetNodePosition(av.location));
                DIAGRAM_GUI.Items[fidx].Angle = angle;

                if (av.status != "WAIT" && lift == "UP") DIAGRAM_GUI.Items[fidx].Appearance.BackColor = Color.Gold;
                else if (av.status != "WAIT" && lift == "DOWN")DIAGRAM_GUI.Items[fidx].Appearance.BackColor = Color.PaleGoldenrod;
                else if (av.status == "WAIT" && isworking_AGV) DIAGRAM_GUI.Items[fidx].Appearance.BackColor = Color.PaleGoldenrod;
                else if (av.status == "WAIT" && !isworking_AGV) DIAGRAM_GUI.Items[fidx].Appearance.BackColor = Color.Gray;
                else if (av.status == "CHARGE") DIAGRAM_GUI.Items[fidx].Appearance.BackColor = Color.Purple;

            }
        }

        void Tab2_ArrayConnectComponet()
        {
            
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            Image imgData = null;

            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                ms.Write(byteArrayIn, 0, byteArrayIn.Length);
                imgData = Image.FromStream(ms, true);//Exception occurs here
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return imgData;
        }

        public string GetGQStatus(string gqStatus, string skey)
        {
            string[] properties = gqStatus.Split(new char[] { ';' });
            string svalue = "";
            string[] porperty = new string[2];
            foreach (string sproperty in properties)
            {

                porperty = sproperty.Split(new char[] { '=' });
                if (porperty[0].Trim() == skey)
                {
                    svalue = porperty[1];
                    break;
                }
            }
            return svalue.Trim();
        }

        PointF GetNodePosition(string nodeID)
        {
            PointF pt = new PointF(0, 0);
            for (int nIndex = 0; nIndex < DIAGRAM_GUI.Items.Count; nIndex++)
            {
                if ("DiagramShape" == DIAGRAM_GUI.Items[nIndex].GetType().Name)
                {
                    DiagramShape diagramItem = null;
                    diagramItem = new DiagramShape();
                    diagramItem = (DiagramShape)DIAGRAM_GUI.Items[nIndex];
                    string strItemTag = diagramItem.Tag.ToString();
                    if (strItemTag == nodeID)
                        return new PointF(diagramItem.Position.X, diagramItem.Position.Y);
                }
            }
            return pt;
        }

        private string FindNodeNameFromIndex(int idx)
        {
            return globalRouteInfo.nodes[idx];
        }

        public static PointFloat DBStringToPointFloat(int nX, int nY)
        {
            PointFloat ptPos = new PointFloat(new System.Drawing.Point(nX, nY));

            return ptPos;
        }

        /// <summary>
        /// 데이터 값에 따라 색 변경
        /// </summary>
        /// <param name="OnOff"> 데이터 값</param>
        /// <param name="ds"> 다이어그램 객체</param>
        void changeShapeColor(int OnOff, DiagramShape ds)
        {
            if (ds != null)
            {
                if (OnOff == 1) ds.Appearance.BackColor = Color.Lime;
                else ds.Appearance.BackColor = SystemColors.ControlLight;

                if (ds.Tag != null)
                {
                    if (ds.Tag.ToString().Contains("Emergency"))
                    {
                        if (OnOff == 1) ds.Appearance.BackColor = SystemColors.ControlLight;
                        else ds.Appearance.BackColor = Color.Red;
                    }

                    if (ds.Tag.ToString().Contains("Abnormal"))
                    {
                        if (OnOff == 0)
                        {
                            ds.Appearance.BackColor = Color.Red;
                            ds.Content = "이상";
                        }
                        else
                        {
                            ds.Appearance.BackColor = Color.Lime;
                            ds.Content = "정상";
                        }
                    }

                    if (ds.Tag.ToString().Contains("Manual"))
                    {
                        if (OnOff == 1) ds.Appearance.BackColor = Color.Red;
                        else ds.Appearance.BackColor = SystemColors.ControlLight;
                    }

                    if (ds.Tag.ToString().Contains("blocking"))
                    {
                        if (OnOff == 0) //자동
                        {
                            // 명령이 남아 있는지 확인
                            tb_mcs_commandTableAdapter cadtp = new tb_mcs_commandTableAdapter();
                            int CntRemainOrder = 0;
                            CntRemainOrder = (int)cadtp.GetRemainOrderCnt();

                            if (CntRemainOrder > 0 || isworking_AGV) ds.Appearance.BackColor = Color.Red;
                            else ds.Appearance.BackColor = SystemColors.ControlLight;
                        }
                        else if (OnOff == 1) ds.Appearance.BackColor = SystemColors.ControlLight; // 닫힘
                        else if (OnOff == 2) ds.Appearance.BackColor = Color.Red; //열림
                    }

                    if (ds.Tag.ToString().Contains("SitDown"))
                    {
                        if (OnOff == 0) ds.Appearance.BackColor = SystemColors.ControlLight;
                        else ds.Appearance.BackColor = Color.Red;
                    }
                }
            }
        }

        void changeShapeText(string Text, DiagramShape ds)
        {
            ds.Content = Text;
            ds.Appearance.BackColor = Color.LightGray;
        }

        #region button Event

        /// <summary>
        /// 전체화면 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullScreen_Button_Click(object sender, EventArgs e)
        {
            DIAGRAM_GUI.FitToDrawing();
        }

        /// <summary>
        /// AGV 1호 위치보기 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View1_Button_Click(object sender, EventArgs e)
        {
            DIAGRAM_GUI.FitToDrawing();
            foreach (agvInfo item in m_agv)
            {
                if (item.agv_id == "001")
                {
                    int fidx = DIAGRAM_GUI.Items.FindIndex(x => x.Tag.ToString() == item.agv_id);
                    if (fidx != -1)
                    {
                        for (int cnt = 0; cnt < 9; cnt++)
                        {
                            DIAGRAM_GUI.ZoomIn();
                        }
                        DIAGRAM_GUI.ScrollToPoint(new PointFloat(DIAGRAM_GUI.Items[fidx].Position.X, DIAGRAM_GUI.Items[fidx].Position.Y), HorzAlignment.Center, VertAlignment.Center);
                        return;
                    }
                    else
                    {
                        XtraMessageBox.Show("No exist AGV", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
        }
        #endregion

        #region make GUI in List - sublineComm
        void make_subLine_Frame()
        {
            MakeGUI subline_Frame = new MakeGUI
            {
                color = Color.White,
                postionX = 2725,
                postionY = -820,
                tagName = "subline_Frame",
                shapes = BasicShapes.Frame,
                bordersize = 1,
                width = 180,
                height = 40
            };
            subline_Frame.rendring();
            arrayGUI.Add(subline_Frame);
        }

        void make_subLine_Auto()
        {
            MakeGUI subLine_Auto = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2740,
                postionY = -815,
                tagName = "subLine_Auto",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            subLine_Auto.rendring();
            arrayGUI.Add(subLine_Auto);
        }

        void make_subLine_Manual()
        {
            MakeGUI subLine_Manual = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2780,
                postionY = -815,
                tagName = "subLine_Manual",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            subLine_Manual.rendring();
            arrayGUI.Add(subLine_Manual);
        }

        void make_subLine_Abnormal()
        {
            MakeGUI subLine_Abnormal = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2820,
                postionY = -815,
                tagName = "subLine_Abnormal",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            subLine_Abnormal.rendring();
            arrayGUI.Add(subLine_Abnormal);
        }

        void make_subLine_Emergency()
        {
            MakeGUI subLine_Emergency = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2860,
                postionY = -815,
                tagName = "subLine_Emergency",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            subLine_Emergency.rendring();
            arrayGUI.Add(subLine_Emergency);
        }
        #endregion

        #region make GUI in List - sublineA
        void make_subLineA_pltOn()
        {
            MakeGUI subLineA_pltOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2625,
                postionY = -810,
                tagName = "subLineA_pltOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineA_pltOn.rendring();
            arrayGUI.Add(subLineA_pltOn);
        }

        void make_subLineA_CallAGV()
        {
            MakeGUI subLineA_CallAGV = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2650,
                postionY = -810,
                tagName = "subLineA_CallAGV",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineA_CallAGV.rendring();
            arrayGUI.Add(subLineA_CallAGV);
        }

        void make_subLineA_CarInfo()
        {
            MakeGUI subLineA_CarInfo = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2600,
                postionY = -810,
                tagName = "subLineA_CarInfo",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineA_CarInfo.rendring();
            arrayGUI.Add(subLineA_CarInfo);
        }

        void make_subLineA_pltPartOn()
        {
            MakeGUI subLineA_pltPartOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2550,
                postionY = -810,
                tagName = "subLineA_pltPartOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineA_pltPartOn.rendring();
            arrayGUI.Add(subLineA_pltPartOn);
        }

        void make_subLineA_pltPartCnt()
        {
            MakeGUI subLineA_pltPartCnt = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2575,
                postionY = -810,
                tagName = "subLineA_pltPartCnt",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineA_pltPartCnt.rendring();
            arrayGUI.Add(subLineA_pltPartCnt);
        }
        #endregion

        #region make GUI in List - sublineB
        void make_subLineB_CallAGV()
        {
            MakeGUI subLineB_CallAGV = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 3060,
                postionY = -810,
                tagName = "subLineB_CallAGV",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineB_CallAGV.rendring();
            arrayGUI.Add(subLineB_CallAGV);
        }

        void make_subLineB_pltOn()
        {
            MakeGUI subLineB_pltOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 3035,
                postionY = -810,
                tagName = "subLineB_pltOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineB_pltOn.rendring();
            arrayGUI.Add(subLineB_pltOn);
        }

        void make_subLineB_CarInfo()
        {
            MakeGUI subLineB_CarInfo = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 3010,
                postionY = -810,
                tagName = "subLineB_CarInfo",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineB_CarInfo.rendring();
            arrayGUI.Add(subLineB_CarInfo);
        }

        void make_subLineB_pltPartOn()
        {
            MakeGUI subLineB_pltPartOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2960,
                postionY = -810,
                tagName = "subLineB_pltPartOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineB_pltPartOn.rendring();
            arrayGUI.Add(subLineB_pltPartOn);
        }

        void make_subLineB_pltPartCnt()
        {
            MakeGUI subLineB_pltPartCnt = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2985,
                postionY = -810,
                tagName = "subLineB_pltPartCnt",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            subLineB_pltPartCnt.rendring();
            arrayGUI.Add(subLineB_pltPartCnt);
        }
        #endregion

        #region make GUI in List - mainLineComm
        void make_mainLine_Frame()
        {
            MakeGUI mainLine_Frame = new MakeGUI
            {
                color = Color.White,
                postionX = 2635,
                postionY = -455,
                tagName = "mainLine_Frame",
                shapes = BasicShapes.Frame,
                bordersize = 1,
                width = 180,
                height = 40
            };
            mainLine_Frame.rendring();
            arrayGUI.Add(mainLine_Frame);
        }

        void make_mainLine_Auto()
        {
            MakeGUI mainLine_Auto = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2650,
                postionY = -450,
                tagName = "mainLine_Auto",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            mainLine_Auto.rendring();
            arrayGUI.Add(mainLine_Auto);
        }

        void make_mainLine_Manual()
        {
            MakeGUI mainLine_Manual = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2690,
                postionY = -450,
                tagName = "mainLine_Manual",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            mainLine_Manual.rendring();
            arrayGUI.Add(mainLine_Manual);
        }

        void make_mainLine_Abnormal()
        {
            MakeGUI mainLine_Abnormal = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2730,
                postionY = -450,
                tagName = "mainLine_Abnormal",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            mainLine_Abnormal.rendring();
            arrayGUI.Add(mainLine_Abnormal);
        }

        void make_mainLine_Emergency()
        {
            MakeGUI mainLine_Emergency = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2770,
                postionY = -450,
                tagName = "mainLine_Emergency",
                shapes = BasicShapes.RoundCornerRectangle,
                bordersize = 1,
                width = 30,
                height = 30
            };
            mainLine_Emergency.rendring();
            arrayGUI.Add(mainLine_Emergency);
        }
        #endregion

        #region make GUI in List - mainlineA
        void make_mainLineA_CallAGV()
        {
            MakeGUI mainLineA_CallAGV = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2565,
                postionY = -447,
                tagName = "mainLineA_CallAGV",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineA_CallAGV.rendring();
            arrayGUI.Add(mainLineA_CallAGV);
        }

        void make_mainLineA_pltOn()
        {
            MakeGUI mainLineA_pltOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2540,
                postionY = -447,
                tagName = "mainLineA_pltOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineA_pltOn.rendring();
            arrayGUI.Add(mainLineA_pltOn);
        }

        void make_mainLineA_CarInfo()
        {
            MakeGUI mainLineA_CarInfo = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2515,
                postionY = -447,
                tagName = "mainLineA_CarInfo",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineA_CarInfo.rendring();
            arrayGUI.Add(mainLineA_CarInfo);
        }

        void make_mainLineA_pltPartOn()
        {
            MakeGUI mainLineA_pltPartOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2465,
                postionY = -447,
                tagName = "mainLineA_pltPartOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineA_pltPartOn.rendring();
            arrayGUI.Add(mainLineA_pltPartOn);
        }

        void make_mainLineA_pltPartCnt()
        {
            MakeGUI mainLineA_pltPartCnt = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2490,
                postionY = -447,
                tagName = "mainLineA_pltPartCnt",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineA_pltPartCnt.rendring();
            arrayGUI.Add(mainLineA_pltPartCnt);
        }
        #endregion

        #region make GUI in List - mainlineB
        void make_mainLineB_CallAGV()
        {
            MakeGUI mainLineB_CallAGV = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2967,
                postionY = -447,
                tagName = "mainLineB_CallAGV",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineB_CallAGV.rendring();
            arrayGUI.Add(mainLineB_CallAGV);
        }

        void make_mainLineB_pltOn()
        {
            MakeGUI mainLineB_pltOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2942,
                postionY = -447,
                tagName = "mainLineB_pltOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineB_pltOn.rendring();
            arrayGUI.Add(mainLineB_pltOn);
        }

        void make_mainLineB_CarInfo()
        {
            MakeGUI mainLineB_CarInfo = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2917,
                postionY = -447,
                tagName = "mainLineB_CarInfo",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineB_CarInfo.rendring();
            arrayGUI.Add(mainLineB_CarInfo);
        }

        void make_mainLineB_pltPartOn()
        {
            MakeGUI mainLineB_pltPartOn = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2867,
                postionY = -447,
                tagName = "mainLineB_pltPartOn",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineB_pltPartOn.rendring();
            arrayGUI.Add(mainLineB_pltPartOn);
        }

        void make_mainLineB_pltPartCnt()
        {
            MakeGUI mainLineB_pltPartCnt = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2892,
                postionY = -447,
                tagName = "mainLineB_pltPartCnt",
                shapes = BasicShapes.SnipDiagonalCornerRectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            mainLineB_pltPartCnt.rendring();
            arrayGUI.Add(mainLineB_pltPartCnt);
        }
        #endregion

        #region make BlockingBar
        void make_blockingBar_R()
        {
            MakeGUI blockingBar_R = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 3100,
                postionY = -770,
                tagName = "blockingBar_R",
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 20,
                height = 300
            };
            blockingBar_R.rendring();
            arrayGUI.Add(blockingBar_R);
        }

        void make_blockingBar_L()
        {
            MakeGUI blockingBar_L = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2475,
                postionY = -770,
                tagName = "blockingBar_L",
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 20,
                height = 300
            };
            blockingBar_L.rendring();
            arrayGUI.Add(blockingBar_L);
        }
        #endregion

        #region make Aram PltSitDown
        void make_Aram_PltSitDown_SubA()
        {
            MakeGUI Aram_PltSitDown_SubA = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2650 + 31,
                postionY = -810 - 30,
                tagName = "Aram_PltSitDown_SubA",
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            Aram_PltSitDown_SubA.rendring();
            arrayGUI.Add(Aram_PltSitDown_SubA);
        }

        void make_Aram_PltSitDown_SubB()
        {
            MakeGUI Aram_PltSitDown_SubB = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2960 - 30,
                postionY = -810 - 30,
                tagName = "Aram_PltSitDown_SubB",
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            Aram_PltSitDown_SubB.rendring();
            arrayGUI.Add(Aram_PltSitDown_SubB);
        }

        void make_Aram_PltSitDown_MainA()
        {
            MakeGUI Aram_PltSitDown_MainA = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2565 + 30,
                postionY = -447 + 35,
                tagName = "Aram_PltSitDown_MainA",
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            Aram_PltSitDown_MainA.rendring();
            arrayGUI.Add(Aram_PltSitDown_MainA);
        }

        void make_Aram_PltSitDown_MainB()
        {
            MakeGUI Aram_PltSitDown_MainB = new MakeGUI
            {
                color = SystemColors.ControlLight,
                postionX = 2867 - 30,
                postionY = -447 + 35,
                tagName = "Aram_PltSitDown_MainB",
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 20,
                height = 20
            };
            Aram_PltSitDown_MainB.rendring();
            arrayGUI.Add(Aram_PltSitDown_MainB);
        }
        #endregion


    }

    #region button Size Control
    public class TabPane : DevExpress.XtraBars.Navigation.TabPane
    {
        protected override ButtonsPanel CreateButtonsPanel()
        {
            return new MyNavigationPaneButtonsPanel(this);
        }
    }
    public class MyNavigationPaneButtonsPanel : NavigationPaneButtonsPanel
    {
        public MyNavigationPaneButtonsPanel(IButtonsPanelOwner owner) : base(owner)
        {
        }
        protected override IButtonsPanelViewInfo CreateViewInfo()
        {
            return new MyNavigationPaneButtonsPanelViewInfo(this);
        }
    }
    public class MyNavigationPaneButtonsPanelViewInfo : NavigationPaneButtonsPanelViewInfo
    {
        public MyNavigationPaneButtonsPanelViewInfo(IButtonsPanel panel) : base(panel)
        {
        }
        protected override BaseButtonInfo CreateButtonInfo(IBaseButton button)
        {
            BaseButtonInfo info = new MyBaseButtonInfo(button, (this.Panel.Owner as TabPane).ItemOrientation == System.Windows.Forms.Orientation.Vertical);
            return info;
        }
    }
    public class MyBaseButtonInfo : BaseButtonInfo
    {
        public MyBaseButtonInfo(IBaseButton button, bool verticalRotated = false) : base(button, verticalRotated)
        {
        }

        protected override System.Drawing.Size GetContentSize(System.Drawing.Size textSize, System.Drawing.Size imageSize, int interval, System.Drawing.Size minSize)
        {
            System.Drawing.Size size = base.GetContentSize(textSize, imageSize, interval, minSize);
            return new System.Drawing.Size(size.Width + 666, size.Height + 20); // HERE
        }
    }
    #endregion


}
