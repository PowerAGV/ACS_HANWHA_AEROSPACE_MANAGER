using ImVader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
using ACSManager.Control;
using ACSManager.DataSetPLCTableAdapters;

namespace ACSManager
{
    class MakeGUI
    {
        public DiagramShape ds = new DiagramShape();
        public Color color;
        public ShapeDescription shapes;
        public string tagName = "";
        public string content = "";
        public int width = 0;
        public int height = 0;
        public int bordersize = 0;
        public int postionX = 0;
        public int postionY = 0;

        public void rendring()
        {
            setCommon_DiagramShape(ds);
            setColor_DiagramShape(ds, color);
            setSize_DiagramShape(ds, width, height);
            setShape_DiagramShape(ds, shapes, bordersize);
            setTag_DiagramShape(ds, tagName);
            setPosition_DiagramShape(ds, postionX, postionY);
        }

        public void setCommon_DiagramShape(DiagramShape ds)
        {
            ds.CanResize    = false;
            ds.CanMove      = false;
            ds.CanDelete    = false;
            ds.CanCopy      = false;
            ds.CanEdit      = false;
            ds.CanResize    = false;
            ds.CanSelect    = false;
        }

        public void setColor_DiagramShape(DiagramShape ds, Color color)
        {
            ds.Appearance.BackColor = color;
        }

        public void setSize_DiagramShape(DiagramShape ds, int width, int height)
        {
            ds.Width = width;
            ds.Height = height;
        }

        public void setShape_DiagramShape(DiagramShape ds, ShapeDescription shapes, int bordersize)
        {
            ds.Shape = shapes;
            ds.Appearance.BorderSize = bordersize;
        }

        public void setTag_DiagramShape(DiagramShape ds, string tagName)
        {
            ds.Tag = tagName;
        }

        public void setPosition_DiagramShape(DiagramShape ds, int x, int y)
        {
            ds.Position = new PointFloat(new Point(x, y));
        }

        
    }

    class MakeCustomShape
    {
        public List<MakeGUI> arrayGUI = new List<MakeGUI>();
        public void Rendering()
        {
            make_ChargeRoom_Box();
            make_AGVChargeRoom_Box();
            make_LeeHyunGongRoom_Box();

            make_HardeningRoom_Box1();
            make_HardeningRoom_Box2();
            make_HardeningRoom_Box3();
            make_HardeningRoom_Box4();
            make_HardeningRoom_Box5();
            make_HardeningRoom_Box6();
            make_HardeningRoom_Box7();
            make_HardeningRoom_Box8();
            make_HardeningRoom_Tag1();
        }

        void make_ChargeRoom_Box()
        {
            MakeGUI make_ChargeRoom_Box = new MakeGUI
            {
                tagName = "make_ChargeRoom_Box",
                color = Color.FromArgb(243, 221, 191),
                postionX = 2300,
                postionY = 3100,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 700,
                height = 540
            };
            make_ChargeRoom_Box.rendring();
            arrayGUI.Add(make_ChargeRoom_Box);

            PLCView.plcView.shape_AreaBox_parts[0] = make_ChargeRoom_Box.ds;
        }

        void make_AGVChargeRoom_Box()
        {
            MakeGUI make_AGVChargeRoom_Box = new MakeGUI
            {
                tagName = "make_AGVChargeRoom_Box",
                color = Color.FromArgb(243, 221, 191),
                postionX = 3425,
                postionY = 100,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 385,
                height = 530
            };
            make_AGVChargeRoom_Box.rendring();
            arrayGUI.Add(make_AGVChargeRoom_Box);

            PLCView.plcView.shape_AreaBox_parts[1] = make_AGVChargeRoom_Box.ds;
        }

        void make_LeeHyunGongRoom_Box()
        {
            MakeGUI make_LeeHyunGongRoom_Box = new MakeGUI
            {
                tagName = "make_LeeHyunGongRoom_Box",
                color = Color.FromArgb(243, 221, 191),
                postionX = 255,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 385,
                height = 330
            };
            make_LeeHyunGongRoom_Box.rendring();
            arrayGUI.Add(make_LeeHyunGongRoom_Box);

            PLCView.plcView.shape_AreaBox_parts[2] = make_LeeHyunGongRoom_Box.ds;
        }

        void make_HardeningRoom_Box1()
        {
            MakeGUI make_HardeningRoom_Box1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box1",
                color = Color.FromArgb(206, 223, 246),
                postionX = 2790,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box1.rendring();
            arrayGUI.Add(make_HardeningRoom_Box1);

            PLCView.plcView.shape_HardeningRoom_parts[0] = make_HardeningRoom_Box1.ds;
        }

        void make_HardeningRoom_Box2()
        {
            MakeGUI make_HardeningRoom_Box2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box2",
                color = Color.FromArgb(206, 223, 246),
                postionX = 2555,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box2.rendring();
            arrayGUI.Add(make_HardeningRoom_Box2);

            PLCView.plcView.shape_HardeningRoom_parts[1] = make_HardeningRoom_Box2.ds;
        }

        void make_HardeningRoom_Box3()
        {
            MakeGUI make_HardeningRoom_Box3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box3",
                color = Color.FromArgb(206, 223, 246),
                postionX = 2320,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box3.rendring();
            arrayGUI.Add(make_HardeningRoom_Box3);

            PLCView.plcView.shape_HardeningRoom_parts[2] = make_HardeningRoom_Box3.ds;
        }

        void make_HardeningRoom_Box4()
        {
            MakeGUI make_HardeningRoom_Box4 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box4",
                color = Color.FromArgb(206, 223, 246),
                postionX = 2090,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box4.rendring();
            arrayGUI.Add(make_HardeningRoom_Box4);

            PLCView.plcView.shape_HardeningRoom_parts[3] = make_HardeningRoom_Box4.ds;
        }

        void make_HardeningRoom_Box5()
        {
            MakeGUI make_HardeningRoom_Box5 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box5",
                color = Color.FromArgb(206, 223, 246),
                postionX = 1865,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box5.rendring();
            arrayGUI.Add(make_HardeningRoom_Box5);

            PLCView.plcView.shape_HardeningRoom_parts[4] = make_HardeningRoom_Box5.ds;
        }

        void make_HardeningRoom_Box6()
        {
            MakeGUI make_HardeningRoom_Box6 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box6",
                color = Color.FromArgb(206, 223, 246),
                postionX = 1635,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box6.rendring();
            arrayGUI.Add(make_HardeningRoom_Box6);

            PLCView.plcView.shape_HardeningRoom_parts[5] = make_HardeningRoom_Box6.ds;
        }

        void make_HardeningRoom_Box7()
        {
            MakeGUI make_HardeningRoom_Box7 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box7",
                color = Color.FromArgb(206, 223, 246),
                postionX = 1405,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box7.rendring();
            arrayGUI.Add(make_HardeningRoom_Box7);

            PLCView.plcView.shape_HardeningRoom_parts[6] = make_HardeningRoom_Box7.ds;
        }

        void make_HardeningRoom_Box8()
        {
            MakeGUI make_HardeningRoom_Box8 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Box8",
                color = Color.FromArgb(206, 223, 246),
                postionX = 1175,
                postionY = 540,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 330
            };
            make_HardeningRoom_Box8.rendring();
            arrayGUI.Add(make_HardeningRoom_Box8);

            PLCView.plcView.shape_HardeningRoom_parts[7] = make_HardeningRoom_Box8.ds;
        }

        void make_HardeningRoom_Tag1()
        {
            MakeGUI make_HardeningRoom_Tag1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag1",
                color = Color.FromArgb(255, 255, 255),
                postionX = 975,
                postionY = 300,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 300,
                height = 300
            };
            make_HardeningRoom_Tag1.rendring();
            make_HardeningRoom_Tag1.ds.Content = "경화#1";
            
            arrayGUI.Add(make_HardeningRoom_Tag1);

            //PLCView.plcView.shape_HardeningRoom_parts[7] = make_HardeningRoom_Box8.ds;
        }
    }
}
