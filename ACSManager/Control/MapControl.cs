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
using DevExpress.Utils;
using DevExpress.XtraDiagram;
using DevExpress.Diagram.Core;
using static ACSManager.DataDefine;
using System.IO;
using DevExpress.Skins;
using DevExpress.XtraDiagram.Base;
using DevExpress.Utils.Serializing;
using DevExpress.XtraDiagram.ViewInfo;
using DevExpress.Utils.Drawing;
using ImVader;
using ImVader.Algorithms.ShortestPaths;
using ACSManager.data.MSSQL;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using System.Diagnostics;


namespace ACSManager.Control
{
    public partial class MapControl : DevExpress.XtraEditors.XtraUserControl
    {
        long map_id = 0;

        public List<NodeInfo> m_listNodeDate = new List<NodeInfo>(); //노드 데이터 정의
        public List<LinkInfo> m_listLinkDate = new List<LinkInfo>(); //링크 데이터 정의
   
        public LinkInfo [] m_OrglistLinkDate = null;

        public List<NodeInfo> m_listTempNodeDate = new List<NodeInfo>();
        public List<LinkInfo> m_listTempLinkDate = new List<LinkInfo>();
        public Image m_ImgSelectMap = null;

        //생성자
        public MapControl()
        {
            InitializeComponent(); 
        }

        //이미지 로드
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

        //안쓰는거
        private Image byteArrayToImage()
        {
              Image imgData = null;

            try
            {


                //imgData = Image.FromFile(@"sungwoo_background.png");


            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return imgData;
        }

        //온 로드
        void LoadMapInfo()
        {
            globalRouteInfo.nodes.Clear(); //클리어
            globalRouteInfo.m_linkes.Clear(); //클리어

            this.map_id = Setting.MAP_ID;
            
            if(map_id == 0)
            {
                Console.WriteLine("지정된 맵이 없습니다.");
                return;
            }


            Link_Node_JoinTableAdapter lnjAdt = new Link_Node_JoinTableAdapter();
            Setting.linkNodeJoinTbl = lnjAdt.GetLinkNode(map_id);

            foreach (MapDesign.Link_Node_JoinRow rw in Setting.linkNodeJoinTbl )
            {

                int f = globalRouteInfo.nodes.FindIndex(x => x == rw.FROM_NODE_NAME);
                int t = globalRouteInfo.nodes.FindIndex(x => x == rw.TO_NODE_NAME);
                if (f == -1)
                    globalRouteInfo.nodes.Add(rw.FROM_NODE_NAME);
                if (t == -1)
                    globalRouteInfo.nodes.Add(rw.TO_NODE_NAME);

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

            foreach (MapDesign.Link_Node_JoinRow rw in Setting.linkNodeJoinTbl )
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
//                    globalRouteInfo.m_path.AddEdge(new WeightedEdge(d, f, rw.length + fadd));
                }
            }
        }


        private string FindNodeNameFromIndex(int idx)
        {
            return globalRouteInfo.nodes[idx];
        }

        private void MapControl_Load(object sender, EventArgs e)
        {
            //return;
            
            LoadAgvList();

            LoadMapInfo();

            // Find Map ID
            //DataSet1MTableAdapters.m_map_masterTableAdapter madtp = new DataSet1MTableAdapters.m_map_masterTableAdapter();

            //map_id = Setting.MAP_ID;
            LoadMapLink(map_id);
            LoadMapNode(map_id);
            AddNodeFromList();
            AddLinkFromList();
            


            //DataSet1M.m_map_masterDataTable mTbl= madtp.GetDataByID(map_id);
            //foreach(DataSet1M.m_map_masterRow rw in mTbl)
            
            //this.m_ImgSelectMap = this.byteArrayToImage();
            

            //if (m_ImgSelectMap != null)
            //{
            //    SizeF szBKImg = new SizeF(m_ImgSelectMap.Width, m_ImgSelectMap.Height);
            //    diagramControl1.OptionsView.PageSize = szBKImg;
            //    SkinElement backgroundSkin = PrintingSkins.GetSkin(LookAndFeel)[PrintingSkins.SkinBorderPage];
            //    backgroundSkin.SetActualImage(m_ImgSelectMap, false);
            //    backgroundSkin.Image.ImageCount = 1;
            //    backgroundSkin.Image.Stretch = SkinImageStretch.Stretch;
            //    backgroundSkin.Image.SizingMargins = new SkinPaddingEdges(0);
            //}
            //diagramControl1.FitToDrawing();
            //diagramControl1.FitToWidth();
            diagramControl1.FitToPage();

            ShowItemTypeMode(ref diagramControl1, true);
            // add CAR

            reloadAgvInfo();
            //makecar("001", new PointF(1000, 1000), 0);
            
        }

        

        public void ShowItemTypeMode(ref DevExpress.XtraDiagram.DiagramControl diagramControl, bool bShowItemMode)
        {
            for (int nIndex = 0; nIndex < diagramControl.Items.Count; nIndex++)
            {
                if ("DiagramShape" == diagramControl.Items[nIndex].GetType().Name)
                {
                    DiagramShape diagramItem = null;

                    diagramItem = new DiagramShape();
                    diagramItem = (DiagramShape)diagramControl.Items[nIndex];

                    string strItemTag = diagramItem.Tag.ToString().Substring(3, diagramItem.Tag.ToString().Length - 3);//4

                    if (true == bShowItemMode)
                    {
                        diagramItem.Content = strItemTag;
                        diagramItem.Appearance.Font = new Font("Tahoma", 7);
                        diagramItem.Appearance.ForeColor = Color.Black;
                    }
                    else
                    {
                        // Tag 값을 기준으로 리스트에서 타입을 얻어오자
                        for (int nNodeIndex = 0; nNodeIndex < m_listNodeDate.Count; nNodeIndex++)
                        {
                            if (strItemTag == m_listNodeDate[nNodeIndex].sNode_Name)
                            {
                                diagramItem.Content = GetNodeVisibleContent(m_listNodeDate[nNodeIndex].sType);
                                diagramItem.Appearance.Font = new Font("Tahoma", 9);
                                diagramItem.Appearance.ForeColor = Color.White;
                            }
                        }
                    }
                }
                else
                {
                    /*
                    DiagramConnector diagramItem = new DiagramConnector();
                    diagramItem = new DiagramConnector();
                    diagramItem = (DiagramConnector)diagramControl.Items[nIndex];

                    if (true == bShowItemMode)
                    {
                        string strItem = diagramItem.Tag.ToString().Substring(4, diagramItem.Tag.ToString().Length - 4);
                        diagramItem.Content = strItem;
                        diagramItem.Appearance.Font = new Font("Tahoma", 7);
                        diagramItem.Appearance.ForeColor = Color.Black;
                    }
                    else
                    {
                        diagramItem.Content = "";
                    }
                    */
                }
            }
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
                    dcon.BeginItem = GetNodeItemInDiagram(diagramControl1, m_listTempLinkDate[nIndex].strFrom_Node);
                    dcon.EndItem = GetNodeItemInDiagram(diagramControl1, m_listTempLinkDate[nIndex].strTo_Node);
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

                    diagramControl1.Items.Add(dcon);

                    LinkInfo linkInfoTemp = new LinkInfo();
                    linkInfoTemp.Clear();
                    linkInfoTemp.strFrom_Node = m_listTempLinkDate[nIndex].strFrom_Node; // dcon.BeginItem;
                    linkInfoTemp.strTo_Node = m_listTempLinkDate[nIndex].strTo_Node; // dcon.EndItem;

                    linkInfoTemp.strId = m_listTempLinkDate[nIndex].strId;
                    linkInfoTemp.Direction = m_listTempLinkDate[nIndex].Direction;
                    // strTag.Substring(4, strTag.Length - 4);

                    //linkInfoTemp.strBeginItemTag = m_listTempLinkDate[nIndex].strBeginItemTag.Substring(4, m_listTempLinkDate[nIndex].strBeginItemTag.Length - 4);
                    //linkInfoTemp.strEndItemTag = m_listTempLinkDate[nIndex].strEndItemTag.Substring(4, m_listTempLinkDate[nIndex].strEndItemTag.Length - 4);
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

                //m_OrglistLinkDate = new LinkInfo[m_listLinkDate.Count()]
                //m_listLinkDate.CopyTo()
                //m_OrglistLinkDate.CopyTo(m_listLinkDate.ToArray());

                bRet = true;
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return bRet;
        }

        public DiagramShape GetNodeItemInDiagram(DevExpress.XtraDiagram.DiagramControl diagramControl, string strTag)
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

        public DiagramShape GetNodeItemInDiagram(DevExpress.XtraDiagram.DiagramControl diagramControl, int nItemIndex)
        {
            DiagramShape diagramItem = null;

            try
            {
                diagramItem = new DiagramShape();
                diagramItem = (DiagramShape)diagramControl.Items[nItemIndex];
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return diagramItem;
        }

        public bool AddNodeFromList()
        {
            bool bRet = false;

            try
            {
                for (int nIndex = 0; nIndex < m_listTempNodeDate.Count; nIndex++)
                {
                    NodeInfo NodeInfoTemp = new NodeInfo();
                    NodeInfoTemp.Clear();

                    // 반지름 수치 15 보정
                    PointFloat ptTemp = m_listTempNodeDate[nIndex].ptPos;
                    ptTemp.X = ptTemp.X + 15;
                    ptTemp.Y = ptTemp.Y + 15;
                    NodeInfoTemp.ptPos = ptTemp;
                    NodeInfoTemp.sNode_Name = m_listTempNodeDate[nIndex].sNode_Name;
                    NodeInfoTemp.sType = m_listTempNodeDate[nIndex].sType;
                    NodeInfoTemp.ColorNode = m_listTempNodeDate[nIndex].ColorNode;
                    NodeInfoTemp.NodeObj = AddNodeOnDisplay(ref diagramControl1, NodeInfoTemp);
                    NodeInfoTemp.MCS_nodeID = m_listTempNodeDate[nIndex].MCS_nodeID;
                    NodeInfoTemp.sensor = m_listTempNodeDate[nIndex].sensor;

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
                diagramItem.Appearance.BackColor = GetNodeVisibleColor(nodeData.sType); // 실제 화면상에 뿌려지는 색상
                diagramItem.Appearance.ForeColor = GetNodeVisibleColor(nodeData.sType); // DeadZone을 위한 백업 색상
                diagramItem.CanResize = false;
                diagramItem.Tag = nodeData.sNode_Name;
                diagramItem.CanMove = false;
                diagramItem.CanDelete = false;
                diagramItem.CanCopy = false;
                diagramItem.CanEdit = false;
                diagramItem.CanResize = false;
                diagramItem.CanSelect = true;
                diagramItem.Content = GetNodeVisibleContent(nodeData.sType);

                diagramControl.Items.Add(diagramItem);
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }

            return diagramItem;
        }


        

        public bool LoadMapLink(long lMapID)
        {
            bool bRet = false;

            try
            {
                //DataSet1MTableAdapters.m_map_linkTableAdapter taLink = new DataSet1MTableAdapters.m_map_linkTableAdapter();
                Link_Node_JoinTableAdapter lnjAdt = new Link_Node_JoinTableAdapter();

                MapDesign.Link_Node_JoinDataTable tb = lnjAdt.GetLinkNode(lMapID);
                int nRowCnt = tb.Rows.Count;

                for (int nIndex = 0; nIndex < nRowCnt; nIndex++)
                {
                    MapDesign.Link_Node_JoinRow row = (MapDesign.Link_Node_JoinRow)tb.Rows[nIndex];

                    LinkInfo LinkInfoTemp = new LinkInfo();
                    LinkInfoTemp.strId = row.LINK_ID.ToString();
                    LinkInfoTemp.Direction = row.DIRECTION;
                    //LinkInfoTemp.ColorLink = ColorTranslator.FromHtml(row.col);
                    LinkInfoTemp.nSpeed = int.Parse(row.SPEED.ToString());
                    LinkInfoTemp.strFrom_Node = row.FROM_NODE_NAME;
                    LinkInfoTemp.strTo_Node = row.TO_NODE_NAME;
                    //LinkInfoTemp.fr = null;
                    //LinkInfoTemp.EndItem = null;

                    //LinkInfoTemp.sensorL = row.sensor_l;
                    //LinkInfoTemp.sensorR = row.sensor_r;


                    if (row.LENGTH ==0)
                    {
                        LinkInfoTemp.nLength = 1000;
                    }
                    else
                    {
                        LinkInfoTemp.nLength = row.LENGTH ; // 사이간격 확대용
                    }
                    LinkInfoTemp.nAngle = row.ANGLE;

                    m_listTempLinkDate.Add(LinkInfoTemp);
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
                foreach(MapDesign.nodeRow row in Setting.nodTbl)
                {

                    NodeInfo NodeInfoTemp = new NodeInfo();
                    NodeInfoTemp.sNode_Name = row.name;
                    NodeInfoTemp.sType = row.node_type;
                    //NodeInfoTemp.MCS_nodeID = "";
                    //NodeInfoTemp.sensor = "";

                    NodeInfoTemp.ptPos = DBStringToPointFloat((int)row.x , (int)row.y );
                    //NodeInfoTemp.ColorNode = ColorTranslator.FromHtml(row.colornode);
                    //NodeInfoTemp.strNodeObjTag = row.strnodeobjtag;
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
                    colorNode = Color.PowderBlue;//Black
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


        public static PointFloat DBStringToPointFloat(int nX,int nY)
        {
            PointFloat ptPos = new PointFloat(new Point(nX, nY));

            return ptPos;
        }

        void makecar(string carname, PointF pos, float ang)
        {
            DiagramShapeEx nitm = null;

            if (carname == "001")
            {
                nitm = new DiagramShapeEx(BasicShapes.Rectangle, pos.X, pos.Y, 35, 35,carname) { ImagePath = "car1.png" };
            }
            else
            {
                nitm = new DiagramShapeEx(BasicShapes.Rectangle, pos.X, pos.Y, 35, 35,carname) { ImagePath = "car2.png" };
            }

            nitm.Tag = carname;
            //nitm.Content = carname;
            nitm.Angle = ang;
            nitm.CanResize = false;
            nitm.CanRotate = false;
            
            //nitm.CanMove = false;

            nitm.Appearance.Font = new Font("Tahoma", 25);
            //nitm.Appearance.BackColor = Color.Yellow;


            nitm.Appearance.ForeColor = Color.Black;
            diagramControl1.Items.Add(nitm);            
        }
        List<agvInfo> m_agv = new List<agvInfo>();

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
                            m_agv[fidx].angle = int.Parse(GetGQStatus(item.agv_desc, "ANGLE"));
                        }
                        else
                            m_agv.Add(new agvInfo(item.agv_id, item.current_node, item.current_route,0,item.current_status));
                    }
                }
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ex.StackTrace, ex.Message));
            }
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

            //string svalue = query.FirstOrDefault().ToString().Split(new char[] { '=' })[1];


            return svalue.Trim();
        }

        void displayAGV()
        {
           
            // 객체가 없다면 차를 만들어야 한다.
            foreach(agvInfo item in m_agv)
            {
                int agv_degree = 0;
                if (item.angle == 1)
                {
                    agv_degree = 90;
                }
                else if (item.angle == 2)
                {
                    agv_degree = 0;
                }
                else if (item.angle == 3)
                {
                    agv_degree = 270;
                }
                else if (item.angle == 4)
                {
                    agv_degree = 180;
                }

                //AGV에서 방향값 오지 않음.
                agv_degree = 0;

                if (isExist(item.agv_id))
                {// 해당 위치 변경
                    SetAgvPoint(item.agv_id, agv_degree);
                }
                else
                {// 생성 및 위치 지정
                    
                    makecar(item.agv_id, GetNodePosition(item.location), agv_degree);
                }
            }
        }

        PointF GetNodePosition(string nodeID)
        {
            PointF pt = new PointF(0, 0);
            for (int nIndex = 0; nIndex < diagramControl1.Items.Count; nIndex++)
            {
                if ("DiagramShape" == diagramControl1.Items[nIndex].GetType().Name)
                {
                    DiagramShape diagramItem = null;
                    diagramItem = new DiagramShape();
                    diagramItem = (DiagramShape)diagramControl1.Items[nIndex];
                    string strItemTag = diagramItem.Tag.ToString();
                    if (strItemTag == nodeID)
                        return new PointF(diagramItem.Position.X, diagramItem.Position.Y);
                    
                    

                }
            }
            return pt;
        }

        void SetAgvPoint(string agvID,int angle=0)
        {

            int fidx = diagramControl1.Items.FindIndex(x => x.Tag.ToString() == agvID);
            if(fidx != -1)
            {
                agvInfo av = m_agv.Find(x => x.agv_id == agvID);
                diagramControl1.Items[fidx].Position = new PointFloat(GetNodePosition(av.location));
                //if (diagramControl1.Items[fidx].Position.X  < 0)
                //    diagramControl1.Items[fidx].Position = new PointFloat(0, diagramControl1.Items[fidx].Position.Y);
                //else
                    diagramControl1.Items[fidx].Position = new PointFloat(diagramControl1.Items[fidx].Position.X , diagramControl1.Items[fidx].Position.Y);

                diagramControl1.Items[fidx].Angle = angle;

                //diagramControl1.Click += Diagram_click(null,null);
            }
            


        }

        private void Diagram_click(object sender, EventArgs eags)
        {

        }
        

        PointF GetFromDiagramAgvPoint(string agvID)
        {
            PointF pt = new PointF(0, 0);
            int fidx = diagramControl1.Items.FindIndex(x => x.Tag.ToString() == agvID);
            if (fidx != -1)
            {
                return new PointF(diagramControl1.Items[fidx].Position.X, diagramControl1.Items[fidx].Position.Y);
                
            }

            
            
            return pt;
        }
        bool isExist(string agvID)
        {
            bool res = false;
            int idx = diagramControl1.Items.FindIndex(x => x.Tag.ToString() == agvID);
            if (idx != -1)
                return true;
            else
                return false;
            //for (int nIndex = 0; nIndex < diagramControl1.Items.Count; nIndex++)
            //{
            //    if ("DiagramShape" == diagramControl1.Items[nIndex].GetType().Name)
            //    {
            //        DiagramShape diagramItem = null;
            //        diagramItem = new DiagramShape();
            //        diagramItem = (DiagramShape)diagramControl1.Items[nIndex];
            //        string strItemTag = diagramItem.Tag.ToString();
            //        if (strItemTag == agvID)
            //            return true;
            //    }
            //}
            return res;
        }

        void LoadAgvList()
        {
            try
            {
                DataSet1MTableAdapters.tb_agvTableAdapter tadp = new DataSet1MTableAdapters.tb_agvTableAdapter();
                DataSet1M.tb_agvDataTable tavtbl = tadp.GetData();
                gridControl1.DataSource = tavtbl;

            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadAgvList();
            reloadAgvInfo();
            displayAGV();
        }

        private void MapControl_Enter(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void MapControl_Leave(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Task mytask = Task.Run(() =>
            {
                //threadRefrashAgvList();
            });
        }
        private int FindIndexToStringNode(string node)
        {
            return globalRouteInfo.nodes.FindIndex(x => x == node);
        }
        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            // 선택된 AGV 의 경로를 표시 한다.
            object selectObj = gridView1.GetRow(gridView1.FocusedRowHandle);// (gridView1.FocusedRowHandle);
            string select_agv = (((DataRowView)selectObj)[0].ToString());

            string fromto = (((DataRowView)selectObj)[5].ToString());

            string[] ftarr = fromto.Split("-".ToCharArray());

            if(ftarr.Count() > 1)
            {
                // init default
                for (int nIndex = 0; nIndex < diagramControl1.Items.Count; nIndex++)
                {                    
                    if (diagramControl1.Items[nIndex].GetType().Name == "DiagramConnector")
                    {
                        DiagramConnector dcon = (DiagramConnector)diagramControl1.Items[nIndex];
                        dcon.Appearance.BorderSize = 1;
                    }
                }

                int f = FindIndexToStringNode(ftarr[0]);
                int t = FindIndexToStringNode(ftarr[1]);
                if(f != -1 && t != -1)
                {
                    List<LinkInfoD> retAttay = new List<LinkInfoD>();
                    var fpath = new Dijkstra<int, ImVader.WeightedEdge>(globalRouteInfo.m_path, f);
                    IEnumerable<WeightedEdge> path = fpath.PathTo(t);
                    foreach (WeightedEdge we in path)
                    {
                        LinkInfoD LinkItem = globalRouteInfo.m_linkes.Find(x => ((x.m_to == we.To) && (x.m_from == we.From)));
                        if(LinkItem != null)
                        {
                            // LinkItem.str_id
                            int fidx = diagramControl1.Items.FindIndex(x=>x.Tag.ToString() == LinkItem.str_id);
                            if (fidx != -1)
                            {
                                DiagramConnector dcon = (DiagramConnector)diagramControl1.Items[fidx];
                                //dcon.Appearance.BackColor = Color.White;
                                //dcon.Appearance.BackColor2 = Color.White;
                                //dcon.Width = 100;
                                dcon.BeginArrow = ArrowDescriptions.Open90;
                                dcon.EndArrow = ArrowDescriptions.FilledDot;

                                dcon.Appearance.BorderSize = 4;
                                dcon.Type = ConnectorType.Straight;
                            }
                        }                            
                    }                    
                }
            }
        }
    }

    public class DiagramShapeEx : DiagramShape
    {
        public DiagramShapeEx() { }
        public DiagramShapeEx(ShapeDescription shape, float x, float y, float width, float height, string name) : base(shape, x, y, width, height, name) { }
      
        public Rectangle RoundToRectangle(RectangleF rect)
        {
            var x = (int)Math.Round(rect.X);
            var y = (int)Math.Round(rect.Y);
            return new Rectangle(x, y, (int)Math.Round(rect.Right) - x, (int)Math.Round(rect.Bottom) - y);
        }

        string imagePath;
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                this.OnPropertiesChanged();
            }
        }
    }

    public class agvInfo
    {
        public string agv_id = "";
        public string location = "";
        public string fromto = "";
        public int angle = 0;
        public string status = "";
        public string lift = "";

        public agvInfo(string id,string loc,string ft,int angle,string status)
        {
            this.agv_id = id;
            this.location = loc;
            this.fromto = ft;
            this.angle = angle;
            this.status = status;
        }

        public agvInfo(string id, string loc, string ft, int angle, string status, string lift)
        {
            this.agv_id = id;
            this.location = loc;
            this.fromto = ft;
            this.angle = angle;
            this.status = status;
            this.lift = lift;
        }
    } 
}
