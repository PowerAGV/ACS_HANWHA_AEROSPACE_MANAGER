using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraDiagram;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace ACSManager
{

    public class ListItem : Object
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public ListItem(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Text;
        }
    }

    public class RouteInfo
    {
        public int path_index = 0;
        public string job_id;
        public string node_id;
        public string move_kind; // 움직임 종류
        public string speed;
        public int angle = 0;
        public bool sensor = true;
        public string link_id = "";
        public RouteInfo(int idx, string jid, string n_id, string mk, string spd, int ang, bool sen, string linkID)
        {
            path_index = idx;
            job_id = jid;
            node_id = n_id;
            move_kind = mk;
            speed = spd;
            angle = ang;
            sensor = sen;
            link_id = linkID;
        }
    }

    class DataDefine
    {
        public enum SelItemType
        {
            NONE = 0,
            NODE = 1,
            LINK = 2
        }

        public enum NodeType
        {
            VOID = 0,
            //NOMAL,
            //STAND_BY,
            //EMPTY,
            //DEPOT,
            //CHARGE
            //INDEX,
            //STATION,
            //HOME_POSITION,
            //CHARGING_POSITION
            INDEX,
            BACK_STATION,
            FORWARD_STATION
        }


        public class EnumStringAttribute : Attribute
        {
            public EnumStringAttribute(string stringValue)
            {
                this.stringValue = stringValue;
            }
            private string stringValue;
            public string StringValue
            {
                get { return stringValue; }
                set { stringValue = value; }
            }
        }
        public enum LinkDirection
        {
            [EnumStringAttribute("TwoWay")]
            TwoWay,
            [EnumStringAttribute("OneWay")]
            OneWay
        }


        public enum LinkSpeed
        {
            SLOW = 0,
            NORMAL,
            FAST,
            MAX
        }
    }

    public class EnumStringAttribute : Attribute
    {
        public EnumStringAttribute(string stringValue)
        {
            this.stringValue = stringValue;
        }
        private string stringValue;
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    public static class ExtenstionClass
    {
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            // Get the stringvalue attributes  
            EnumStringAttribute[] attribs = fieldInfo.GetCustomAttributes(
                 typeof(EnumStringAttribute), false) as EnumStringAttribute[];
            // Return the first if there was a match.  
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }


    public class NodeInfo
    {
        public string sNode_Name { get; set; }
        public string sType { get; set; }
        public PointFloat ptPos { get; set; }
        public Color ColorNode { get; set; }
        public DiagramShape NodeObj { get; set; }

        public string MCS_nodeID { get; set; }

        public int sensor { get; set; }

        public void Clear()
        {
            sNode_Name = "";
            sType = "";
            ptPos = new PointFloat(0, 0);
            ColorNode = Color.Silver;
            NodeObj = null;
            MCS_nodeID = "";
            sensor = 1;
        }
    }


    public class MapMasterInfo
    {
        public long lMapID { get; set; }
        public string strMapName { get; set; }
        public string strMapCreatorName { get; set; }
        public DateTime dtCreateMap { get; set; }
        public DateTime dtEditMap { get; set; }
        public bool bMapApply { get; set; }


        public void Clear()
        {
            lMapID = 0;
            strMapName = "";
            strMapCreatorName = "";
            dtCreateMap = DateTime.Now;
            dtEditMap = DateTime.Now;
            bMapApply = false;
        }
    }


    public class MapNodeInfo
    {
        public string strId { get; set; }
        public string sType { get; set; }
        public PointFloat ptPos { get; set; }
        //public Color ColorNode { get; set; }
        public string strColorNode { get; set; }

        public string strNodeObjTag { get; set; }


        public void Clear()
        {
            strId = "";
            sType = "";
            ptPos = new PointFloat(0, 0);
            //ColorNode = Color.Silver;
            strColorNode = "";
            strNodeObjTag = "";
        }
    }


    public class LinkInfo
    {
        public string strId { get; set; }
        //public IDiagramItem BeginItem { get; set; }
        //public IDiagramItem EndItem { get; set; }

        public string Direction { get; set; }
        public Color ColorLink { get; set; }
        public int nSpeed { get; set; }

        public string strFrom_Node { get; set; }
        public string strTo_Node { get; set; }
        public double nLength { get; set; }
        public PointFloat ptPos1 { get; set; }
        public PointFloat ptPos2 { get; set; }
        public double nAngle { get; set; }

        public short sensorR { get; set; }
        public short sensorL { get; set; }

        public void Clear()
        {
            strId = "";
            
            Direction = "";
            ColorLink = Color.Black;
            nSpeed = (int)DataDefine.LinkSpeed.NORMAL;
            this.strFrom_Node = "";
            this.strTo_Node = "";
            //strBeginItemTag = "";
            //strEndItemTag = "";
            nLength = 0;
            ptPos1 = new PointFloat(0, 0);
            ptPos2 = new PointFloat(0, 0);
            nAngle = 0;
            sensorR = 1;
            sensorL = 1;
        }
    }


    public class MapLinkInfo
    {
        public string strId { get; set; }
        //public IDiagramItem BeginItem { get; set; }
        //public IDiagramItem EndItem { get; set; }

        public string sDirection { get; set; }
        //public Color ColorLink { get; set; }
        public string strColorLink { get; set; }
        public int nSpeed { get; set; }

        public string strFrom_Node { get; set; }
        public string strTo_Node { get; set; }
        public int nLength { get; set; }
        public PointFloat ptPos1 { get; set; }
        public PointFloat ptPos2 { get; set; }

        public void Clear()
        {
            strId = "";
            //BeginItem = null;
            //EndItem = null;

            sDirection = "";
            //ColorLink = Color.Black;
            strColorLink = "";
            nSpeed = (int)DataDefine.LinkSpeed.NORMAL;
            strFrom_Node = "";
            strTo_Node = "";
            nLength = 0;
            ptPos1 = new PointFloat(0, 0);
            ptPos2 = new PointFloat(0, 0);
        }
    }


    public class MapData
    {
        public List<MapNodeInfo> listNodeDate = new List<MapNodeInfo>();
        public List<MapLinkInfo> listLinkDate = new List<MapLinkInfo>();
    }


    public class NodeGroup
    {
        public List<string> node = new List<string>();
        public string strGroupName = "";

        public NodeGroup(string strName)
        {
            strGroupName = strName;
        }

        public void AddNode(string strNodeId)
        {
            node.Add(strNodeId);
        }
    }


    public class DeadZoneGroup
    {
        public List<string> node = new List<string>();
        public string strGroupName = "";

        public DeadZoneGroup(string strName)
        {
            strGroupName = strName;
        }

        public void AddNode(string strNodeId)
        {
            node.Add(strNodeId);
        }
    }

    public class DeadLineGroup
    {
        public List<string> node = new List<string>();
        public string strGroupName = "";

        public DeadLineGroup(string strName)
        {
            strGroupName = strName;
        }

        public void AddNode(string strNodeId)
        {
            node.Add(strNodeId);
        }
    }

    public class DeadPararellGroup
    {
        public List<string> node = new List<string>();
        public string strGroupName = "";

        public DeadPararellGroup(string strName)
        {
            strGroupName = strName;
        }

        public void AddNode(string strNodeId)
        {
            node.Add(strNodeId);
        }
    }





}


