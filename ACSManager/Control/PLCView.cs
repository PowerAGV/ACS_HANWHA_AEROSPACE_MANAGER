/*
* © SYSCON 
* © TEAM :        SoftWare3
* @ Start Date :  2024.04.23
* @ Project :     HANWHA_AREOSPACE
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
        public static PLCView plcView;
        long map_id = 0;
        bool isworking_AGV = false;

        private string select_agv = "";
        
        List<agvInfo> m_agv = new List<agvInfo>();

        public DiagramShape[] shape_AreaBox_parts = new DiagramShape[3];

        public DiagramShape[] shape_HardeningRoom_Door = new DiagramShape[8];
        public DiagramShape[,] shape_HardeningRoom_Plt = new DiagramShape[8,3];

        public DiagramShape[] shape_ChargeRoom_Door = new DiagramShape[1];
        public DiagramShape[] shape_ChargeRoom_Plt = new DiagramShape[2];

        public DiagramShape[] shape_AGVChargeRoom_Doors = new DiagramShape[4];

        public DiagramShape[] shape_LeeHyungGongRoom_Door = new DiagramShape[1];
        public DiagramShape[] shape_LeeHyungGongRoom_Plt = new DiagramShape[2];

        public List<NodeInfo> m_listTempNodeDate = new List<NodeInfo>();
        public List<LinkInfo> m_listTempLinkDate = new List<LinkInfo>();
        

        /// <summary>
        /// 생성자
        /// </summary>
        public PLCView()
        {
            InitializeComponent();
            plcView = this;
        }

        /// <summary>
        /// 온로딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PLCView_Load(object sender, EventArgs e)
        {
            map_id = Setting.MAP_ID;
            bool mapLoaded = map_id != 0;

            if (mapLoaded)
            {
                Load_MAP_Info(map_id);
                Load_Map_Link(map_id);
                Load_Map_Node(map_id);

                Paint_Map_Shape();
                Paint_Map_Node();
                Paint_Map_Link();

                reloadAgvInfo();
                Vision_FullScreen();
                
            }
            else
            {
                XtraMessageBox.Show("Check Map", "ACSManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        /// <summary>
        /// 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;

                Change_Color_Shapes();
                reloadAgvInfo();
                displayAGV();
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
            finally
            {
                timer1.Enabled = true;
            }
        }

        

        /// <summary>
        /// 맵 정보 로딩
        /// </summary>
        void Load_MAP_Info(long map_id)
        {
            globalRouteInfo.nodes.Clear();
            globalRouteInfo.m_linkes.Clear();
            
            Link_Node_JoinTableAdapter lnjAdt = new Link_Node_JoinTableAdapter();
            Setting.linkNodeJoinTbl = lnjAdt.GetLinkNode(map_id);

            foreach (MapDesign.Link_Node_JoinRow rw in Setting.linkNodeJoinTbl)
            {
                AddNodeIfNotExist(rw.FROM_NODE_NAME);
                AddNodeIfNotExist(rw.TO_NODE_NAME);

                int f = globalRouteInfo.nodes.FindIndex(x => x == rw.FROM_NODE_NAME);
                int t = globalRouteInfo.nodes.FindIndex(x => x == rw.TO_NODE_NAME);

                if (f != -1 && t != -1)
                {
                    int reversedAngle = CalculateReversedAngle(rw.ANGLE);
                    if (rw.DIRECTION == $"{LinkDirection.TwoWay}")
                    {
                        globalRouteInfo.m_linkes.Add(new LinkInfoD(f, t, (int)rw.ANGLE, rw.SPEED.ToString(), FindNodeNameFromIndex(f), FindNodeNameFromIndex(t), false, rw.LINK_ID.ToString()));
                        globalRouteInfo.m_linkes.Add(new LinkInfoD(t, f, reversedAngle, rw.SPEED.ToString(), FindNodeNameFromIndex(t), FindNodeNameFromIndex(f), false, rw.LINK_ID.ToString()));

                    }
                    else if (rw.DIRECTION == $"{LinkDirection.OneWay}")
                    {
                        globalRouteInfo.m_linkes.Add(new LinkInfoD(f, t, (int)rw.ANGLE, rw.SPEED.ToString(), FindNodeNameFromIndex(f), FindNodeNameFromIndex(t), false, rw.LINK_ID.ToString()));
                    }
                }
            }

            globalRouteInfo.m_path = new DirectedListGraph<int, WeightedEdge>(globalRouteInfo.nodes.Count + 2); // 안전을 위해 2 추가

            foreach (MapDesign.Link_Node_JoinRow rw in Setting.linkNodeJoinTbl)
            {
                int f = globalRouteInfo.nodes.FindIndex(x => x == rw.FROM_NODE_NAME);
                int d = globalRouteInfo.nodes.FindIndex(x => x == rw.TO_NODE_NAME);

                int fCount = CountNodesWithName(rw.FROM_NODE_NAME);
                int dCount = CountNodesWithName(rw.TO_NODE_NAME);

                int fAdd = fCount > 2 ? 100 : 0;
                int dAdd = dCount > 2 ? 100 : 0;

                if (rw.DIRECTION == $"{LinkDirection.TwoWay}")
                {
                    globalRouteInfo.m_path.AddEdge(new WeightedEdge(f, d, rw.LENGTH + fAdd));
                    globalRouteInfo.m_path.AddEdge(new WeightedEdge(d, f, rw.LENGTH + dAdd));
                }
                else if (rw.DIRECTION == $"{LinkDirection.OneWay}")
                {
                    globalRouteInfo.m_path.AddEdge(new WeightedEdge(f, d, rw.LENGTH + dAdd));
                }
            }
        }

        /// <summary>
        /// 맵 링크 로딩
        /// </summary>
        /// <param name="MapID"></param>
        void Load_Map_Link(long map_id)
        {
            try
            {
                Link_Node_JoinTableAdapter lnjAdt = new Link_Node_JoinTableAdapter();
                MapDesign.Link_Node_JoinDataTable tb = lnjAdt.GetLinkNode(map_id);

                foreach (MapDesign.Link_Node_JoinRow row in tb.Rows)
                {
                    LinkInfo LinkInfoTemp       = new LinkInfo();
                    LinkInfoTemp.strId          = row.LINK_ID.ToString();
                    LinkInfoTemp.Direction      = row.DIRECTION;
                    LinkInfoTemp.nSpeed         = Convert.ToInt32(row.SPEED);
                    LinkInfoTemp.strFrom_Node   = row.FROM_NODE_NAME;
                    LinkInfoTemp.strTo_Node     = row.TO_NODE_NAME;
                    LinkInfoTemp.nAngle         = row.ANGLE;

                    if (row.LENGTH == 0) LinkInfoTemp.nLength = 1000;
                    else LinkInfoTemp.nLength = row.LENGTH; // 사이간격 확대용

                    m_listTempLinkDate.Add(LinkInfoTemp);

                }
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }   
        }

        /// <summary>
        /// 맵 노드 로딩
        /// </summary>
        /// <param name="map_id"></param>
        void Load_Map_Node(long map_id)
        {
            try
            {
                nodeTableAdapter nodAdt = new nodeTableAdapter();
                MapDesign.nodeDataTable nodTbl = nodAdt.GetNodeByMapID(map_id);

                foreach (MapDesign.nodeRow row in Setting.nodTbl)
                {
                    NodeInfo NodeInfoTemp       = new NodeInfo();
                    NodeInfoTemp.sNode_Name     = row.name ?? string.Empty;
                    NodeInfoTemp.sType          = row.node_type ?? string.Empty;
                    NodeInfoTemp.ptPos          = DBStringToPointFloat((int)row.x, (int)row.y);
                    NodeInfoTemp.NodeObj        = null;

                    m_listTempNodeDate.Add(NodeInfoTemp);
                }
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }
        
        /// <summary>
        /// 노드 그리기
        /// </summary>
        void Paint_Map_Node()
        {
            try
            {
                foreach (NodeInfo tempNodeInfo in m_listTempNodeDate)
                {
                    if (tempNodeInfo == null)
                    {
                        // null인 경우에 대한 처리
                        continue;
                    }

                    DiagramShape diagramItem = new DiagramShape();
                    diagramItem.Position     = new PointFloat(tempNodeInfo.ptPos.X + 5 , tempNodeInfo.ptPos.Y + 5 );

                    diagramItem.CanResize            = false;
                    diagramItem.CanMove              = false;
                    diagramItem.CanDelete            = false;
                    diagramItem.CanCopy              = false;
                    diagramItem.CanEdit              = false;
                    diagramItem.CanResize            = false;
                    diagramItem.CanSelect            = false;

                    diagramItem.Shape                = GetDiagramShape(tempNodeInfo.sType);
                    diagramItem.Height               = GetDiagramHeight(tempNodeInfo.sType);
                    diagramItem.Width                = GetDiagramWidth(tempNodeInfo.sType);
                    diagramItem.Appearance.BackColor = GetNodeColor(tempNodeInfo.sType);
                    diagramItem.Appearance.ForeColor = GetNodeColor(tempNodeInfo.sType);
                    diagramItem.Tag                  = tempNodeInfo.sNode_Name ?? string.Empty;;

                    DIAGRAM_GUI.Items.Add(diagramItem);
                }
                for (int idx = 0; idx < DIAGRAM_GUI.Items.Count; idx++)
                {
                    if (DIAGRAM_GUI.Items[idx].Tag.ToString().Contains("00"))
                    {
                        DiagramShape ds = (DiagramShape)DIAGRAM_GUI.Items[idx];

                        ds.Appearance.Font = new Font("Tahoma", 5);
                        ds.Appearance.ForeColor = Color.Black;
                        ds.Content = ds.Tag.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }

        /// <summary>
        /// 링크 그리기
        /// </summary>
        void Paint_Map_Link()
        {
            try
            {
                foreach (var linkData in m_listTempLinkDate)
                {
                    DiagramConnector dcon = new DiagramConnector();

                    if (linkData.ptPos1 == new PointFloat() && linkData.ptPos2 == new PointFloat())
                    {
                        dcon.Type = ConnectorType.Straight;
                    }
                    else
                    {
                        dcon.Type = ConnectorType.Curved;
                        dcon.Points = new PointCollection(new List<PointFloat> { linkData.ptPos1, linkData.ptPos2 });
                    }

                    dcon.Appearance.BackColor       = Color.Red;
                    dcon.Appearance.BorderColor     = linkData.ColorLink;
                    dcon.Tag                        = linkData.strId;
                    dcon.BeginItem                  = GetNodeItemInDiagram(DIAGRAM_GUI, linkData.strFrom_Node);
                    dcon.EndItem                    = GetNodeItemInDiagram(DIAGRAM_GUI, linkData.strTo_Node);

                    dcon.BeginArrow                 = linkData.Direction == LinkDirection.TwoWay.ToString() ? ArrowDescriptions.Filled90 : ArrowDescriptions.ClosedDot;
                    dcon.EndArrow                   = linkData.Direction != LinkDirection.OneWay.ToString() ? ArrowDescriptions.Filled90 : ArrowDescriptions.ClosedDot;
                    dcon.Appearance.BorderSize      = 1;

                    dcon.CanResize                  = false;
                    dcon.CanMove                    = false;
                    dcon.CanDragBeginPoint          = false;
                    dcon.CanDragEndPoint            = false;
                    dcon.CanEdit                    = false;

                    DIAGRAM_GUI.Items.Add(dcon);
                }
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
        }
       
        /// <summary>
        /// 추가 도형 그리기
        /// </summary>
        void Paint_Map_Shape()
        {
            MakeCustomShape makeCustomShape = new MakeCustomShape();
            makeCustomShape.Rendering();

            foreach (MakeGUI GUI in makeCustomShape.arrayGUI)
            {
                DIAGRAM_GUI.Items.Add(GUI.ds);
            }
        }
        
        /// <summary>
        /// 도형 네이밍
        /// </summary>
        void Rename_Shapes()
        {
            for (int idx = 0; idx < DIAGRAM_GUI.Items.Count; idx++)
            {
                if (DIAGRAM_GUI.Items[idx].GetType().Name == "DiagramShape")
                {
                    DiagramShape ds = (DiagramShape)DIAGRAM_GUI.Items[idx];

                    ds.Appearance.Font = new Font("Tahoma", 4);
                    ds.Appearance.ForeColor = Color.Black;

                    if (ds.Tag.ToString().Contains("Auto")) ds.Content = "자동";
                }
            }
        }

        /// <summary>
        /// DB데이터 매칭 도형 색변경
        /// </summary>
        void Change_Color_Shapes()
        {
            try
            {
                bool AGV_Door1_Open = false;
                bool AGV_Door1_Close = false; 
                bool AGV_Door2_Open = false;
                bool AGV_Door2_Close = false;
                bool AGV_Door1_Send = false;
                bool AGV_Door2_Send = false;

                bool Charge_Door = false;
                bool Charge_PLT1 = false;
                bool Charge_PLT2 = false;

                bool LeeHynungGong_Door = false;
                bool LeeHynungGong_PLT1 = false;
                bool LeeHynungGong_PLT2 = false;

                bool[] Hardening_Door = new bool[8];
                bool[,] Hardening_Plt = new bool[8,3];

                TB_PLCTableAdapter PLCadpt = new TB_PLCTableAdapter();
                foreach (DataSetPLC.TB_PLCRow Row in PLCadpt.GetData())
                {
                    if (Row.PLC_GROUP == 1)
                    {
                        if      (Row.PLC_ADDRESS.Contains("D104")) Charge_Door = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.0")) Charge_PLT1 = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.1")) Charge_PLT2 = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 11)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.2")) Hardening_Door[0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.4")) Hardening_Plt[0, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.5")) Hardening_Plt[0, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.6")) Hardening_Plt[0, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 12)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.3")) Hardening_Door[1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.8")) Hardening_Plt[1, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.9")) Hardening_Plt[1, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.A")) Hardening_Plt[1, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 13)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.4")) Hardening_Door[2] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.C")) Hardening_Plt[2, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.D")) Hardening_Plt[2, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D101.E")) Hardening_Plt[2, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 14)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.5")) Hardening_Door[3] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.0")) Hardening_Plt[3, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.1")) Hardening_Plt[3, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.2")) Hardening_Plt[3, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 15)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.6")) Hardening_Door[4] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.4")) Hardening_Plt[4, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.5")) Hardening_Plt[4, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.6")) Hardening_Plt[4, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 16)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.7")) Hardening_Door[5] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.8")) Hardening_Plt[5, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.9")) Hardening_Plt[5, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.A")) Hardening_Plt[5, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 17)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.8")) Hardening_Door[6] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.C")) Hardening_Plt[6, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.D")) Hardening_Plt[6, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D102.E")) Hardening_Plt[6, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 18)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.9")) Hardening_Door[7] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D103.0")) Hardening_Plt[7, 0] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D103.1")) Hardening_Plt[7, 1] = Row.PLC_VALUE.Contains("true") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D103.2")) Hardening_Plt[7, 2] = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 20)
                    {
                        if (Row.PLC_ADDRESS.Contains("D104.A")) LeeHynungGong_Door = Row.PLC_VALUE.Contains("true") ? true : false;
                        if (Row.PLC_ADDRESS.Contains("D103.4")) LeeHynungGong_PLT1 = Row.PLC_VALUE.Contains("true") ? true : false;
                        if (Row.PLC_ADDRESS.Contains("D103.5")) LeeHynungGong_PLT2 = Row.PLC_VALUE.Contains("true") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 30)
                    {
                        if      (Row.PLC_ADDRESS.Contains("D700")) AGV_Door1_Open = Row.PLC_VALUE.Contains("1") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D702")) AGV_Door1_Close = Row.PLC_VALUE.Contains("1") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D701")) AGV_Door2_Open = Row.PLC_VALUE.Contains("1") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D703")) AGV_Door2_Close = Row.PLC_VALUE.Contains("1") ? true : false;
                    }
                    else if (Row.PLC_GROUP == 100)
                    {
                        if (Row.PLC_ADDRESS.Contains("D710")) AGV_Door1_Send = Row.PLC_VALUE.Contains("1") ? true : false;
                        else if (Row.PLC_ADDRESS.Contains("D711")) AGV_Door2_Send = Row.PLC_VALUE.Contains("1") ? true : false;
                    }
                }
                shape_ChargeRoom_Door[0].Appearance.BackColor = Charge_Door ? Color.Lime : Color.Red;
                shape_ChargeRoom_Plt[0].Appearance.BackColor = Charge_PLT1 ? Color.Lime : Color.LightSlateGray;
                shape_ChargeRoom_Plt[1].Appearance.BackColor = Charge_PLT2 ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Door[0].Appearance.BackColor = Hardening_Door[0] ? Color.Lime : Color.Red;
                shape_HardeningRoom_Door[1].Appearance.BackColor = Hardening_Door[1] ? Color.Lime : Color.Red;
                shape_HardeningRoom_Door[2].Appearance.BackColor = Hardening_Door[2] ? Color.Lime : Color.Red;
                shape_HardeningRoom_Door[3].Appearance.BackColor = Hardening_Door[3] ? Color.Lime : Color.Red;
                shape_HardeningRoom_Door[4].Appearance.BackColor = Hardening_Door[4] ? Color.Lime : Color.Red;
                shape_HardeningRoom_Door[5].Appearance.BackColor = Hardening_Door[5] ? Color.Lime : Color.Red;
                shape_HardeningRoom_Door[6].Appearance.BackColor = Hardening_Door[6] ? Color.Lime : Color.Red;
                shape_HardeningRoom_Door[7].Appearance.BackColor = Hardening_Door[7] ? Color.Lime : Color.Red;

                shape_HardeningRoom_Plt[0, 0].Appearance.BackColor = Hardening_Plt[0, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[0, 1].Appearance.BackColor = Hardening_Plt[0, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[0, 2].Appearance.BackColor = Hardening_Plt[0, 2] ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Plt[1, 0].Appearance.BackColor = Hardening_Plt[1, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[1, 1].Appearance.BackColor = Hardening_Plt[1, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[1, 2].Appearance.BackColor = Hardening_Plt[1, 2] ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Plt[2, 0].Appearance.BackColor = Hardening_Plt[2, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[2, 1].Appearance.BackColor = Hardening_Plt[2, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[2, 2].Appearance.BackColor = Hardening_Plt[2, 2] ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Plt[3, 0].Appearance.BackColor = Hardening_Plt[3, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[3, 1].Appearance.BackColor = Hardening_Plt[3, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[3, 2].Appearance.BackColor = Hardening_Plt[3, 2] ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Plt[4, 0].Appearance.BackColor = Hardening_Plt[4, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[4, 1].Appearance.BackColor = Hardening_Plt[4, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[4, 2].Appearance.BackColor = Hardening_Plt[4, 2] ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Plt[5, 0].Appearance.BackColor = Hardening_Plt[5, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[5, 1].Appearance.BackColor = Hardening_Plt[5, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[5, 2].Appearance.BackColor = Hardening_Plt[5, 2] ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Plt[6, 0].Appearance.BackColor = Hardening_Plt[6, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[6, 1].Appearance.BackColor = Hardening_Plt[6, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[6, 2].Appearance.BackColor = Hardening_Plt[6, 2] ? Color.Lime : Color.LightSlateGray;

                shape_HardeningRoom_Plt[7, 0].Appearance.BackColor = Hardening_Plt[7, 0] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[7, 1].Appearance.BackColor = Hardening_Plt[7, 1] ? Color.Lime : Color.LightSlateGray;
                shape_HardeningRoom_Plt[7, 2].Appearance.BackColor = Hardening_Plt[7, 2] ? Color.Lime : Color.LightSlateGray;

                shape_LeeHyungGongRoom_Door[0].Appearance.BackColor = LeeHynungGong_Door ? Color.Lime : Color.Red;
                shape_LeeHyungGongRoom_Plt[0].Appearance.BackColor = LeeHynungGong_PLT1 ? Color.Lime : Color.LightSlateGray;
                shape_LeeHyungGongRoom_Plt[1].Appearance.BackColor = LeeHynungGong_PLT2 ? Color.Lime : Color.LightSlateGray;

                shape_AGVChargeRoom_Doors[0].Appearance.BackColor = AGV_Door1_Open && !AGV_Door1_Close ? Color.Lime : Color.Red;
                shape_AGVChargeRoom_Doors[1].Appearance.BackColor = AGV_Door2_Open && !AGV_Door2_Close ? Color.Lime : Color.Red;
                shape_AGVChargeRoom_Doors[2].Appearance.BackColor = AGV_Door1_Send ? Color.Lime : Color.Red;
                shape_AGVChargeRoom_Doors[3].Appearance.BackColor = AGV_Door2_Send ? Color.Lime : Color.Red;
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


        //-------------------------------------------Method-----------------------------------------//

        void AddNodeIfNotExist(string nodeName)
        {
            if (!globalRouteInfo.nodes.Contains(nodeName))
                globalRouteInfo.nodes.Add(nodeName);
        }

        int CalculateReversedAngle(double angle)
        {
            Angle ca = new Angle(180);
            Angle la = new Angle(angle);
            Angle a3 = ca + la;
            int reversedAngle = (int)a3.Degrees360;
            return reversedAngle == 360 ? 0 : reversedAngle;
        }

        int CountNodesWithName(string name)
        {
            return globalRouteInfo.nodes.Count(x => x == name);
        }

        Color GetNodeColor(string NodeType)
        {
            Color color = Color.PowderBlue;

            switch (NodeType)
            {
                case "Void":
                    color = Color.Silver;
                    break;
                case "ForwardStation":
                    color = Color.DarkOrange;
                    break;
                case "BackwardStation":
                    color = Color.PowderBlue;
                    break;
                default:
                    color = Color.PowderBlue;
                    break;
            }
            return color;
        }

        ShapeDescription GetDiagramShape(string Type)
        {
            if (Type == "BackwardStation" || Type == "ForwardStation")
            {
                return BasicShapes.Rectangle;
            }
            else
            {
                return BasicShapes.Ellipse;
            }
        }

        float GetDiagramHeight(string Type)
        {
            if (Type == "BackwardStation" || Type == "ForwardStation")
            {
                return 35;
            }
            else
            {
                return 30;
            }
        }

        float GetDiagramWidth(string Type)
        {
            if (Type == "BackwardStation" || Type == "ForwardStation")
            {
                return 35;
            }
            else
            {
                return 30;
            }
        }

        PointFloat DBStringToPointFloat(int nX, int nY)
        {
            PointFloat ptPos = new PointFloat(new System.Drawing.Point(nX, nY));

            return ptPos;
        }

        void Vision_FullScreen()
        {
            DIAGRAM_GUI.FitToDrawing();
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

        /// <summary>
        /// 전체화면 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullScreen_Button_Click(object sender, EventArgs e)
        {
            Vision_FullScreen();
        }

        /// <summary>
        /// 충전실 뷰 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeScreen_Button_Click(object sender, EventArgs e)
        {
            Vision_FullScreen();
            for (int cnt = 0; cnt < 4; cnt++)
            {
                DIAGRAM_GUI.ZoomIn();
            }

            DIAGRAM_GUI.ScrollToPoint(new PointFloat(2500, 3300), HorzAlignment.Center, VertAlignment.Center);
        }

        /// <summary>
        /// 경화실 뷰 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardeningScreen_Button_Click(object sender, EventArgs e)
        {
            Vision_FullScreen();
            for (int cnt = 0; cnt < 2; cnt++)
            {
                DIAGRAM_GUI.ZoomIn();
            }

            DIAGRAM_GUI.ScrollToPoint(new PointFloat(1800, 700), HorzAlignment.Center, VertAlignment.Center);
        }

        /// <summary>
        /// 이형공실 뷰 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeeHyungGongScreen_Button_Click(object sender, EventArgs e)
        {
            Vision_FullScreen();
            for (int cnt = 0; cnt < 5; cnt++)
            {
                DIAGRAM_GUI.ZoomIn();
            }

            DIAGRAM_GUI.ScrollToPoint(new PointFloat(500, 700), HorzAlignment.Center, VertAlignment.Center);
        }

        /// <summary>
        /// AGV충전소 뷰 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AGVChargeScreen_Button_Click(object sender, EventArgs e)
        {
            Vision_FullScreen();
            for (int cnt = 0; cnt < 5; cnt++)
            {
                DIAGRAM_GUI.ZoomIn();
            }

            DIAGRAM_GUI.ScrollToPoint(new PointFloat(3500, 400), HorzAlignment.Center, VertAlignment.Center);
        }

        /// <summary>
        /// AGV 1호 위치보기 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AGV_View1_Button_Click(object sender, EventArgs e)
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

        /// <summary>
        /// AGV 2호 위치보기 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AGV_View2_Button_Click(object sender, EventArgs e)
        {
            DIAGRAM_GUI.FitToDrawing();
            foreach (agvInfo item in m_agv)
            {
                if (item.agv_id == "002")
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






        //-------------------------------------------ETC-----------------------------------------//

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


        public void SetAgvInformation(DataSet1M.tb_agvRow agv, bool nodeChange)
        {
            try
            {
                
                //EventHandler eh5 = delegate
                //{
                    gridControl1.DataSource = null;

                    //DataTable tbRoute = new DataSet1MTableAdapters.tb_final_routeTableAdapter().GetDataByID(agv.agv_id);
                    var tbRoute = new tb_final_routeTableAdapter().GetDataByID(agv.agv_id);

                    if (tbRoute != null && tbRoute.Rows.Count > 0)
                    {
                        gridControl1.DataSource = tbRoute.OrderBy(p => p.sequance);
                        gridView1.BestFitColumns();
                    }

                //};


            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        
    }


}
