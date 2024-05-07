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
        public int pontsize = 0;

        public void rendring()
        {
            setCommon_DiagramShape(ds);
            setColor_DiagramShape(ds, color);
            setSize_DiagramShape(ds, width, height);
            setShape_DiagramShape(ds, shapes, bordersize);
            setTag_DiagramShape(ds, tagName);
            setPosition_DiagramShape(ds, postionX, postionY);
            setContent_DiagramShape(content);
            setPontSize_DiagramShape(pontsize);
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

        public void setContent_DiagramShape(string content)
        {
            ds.Content = content;
        }

        public void setPontSize_DiagramShape(int pontsize)
        {
            ds.Appearance.FontSizeDelta = pontsize;
        }
    }

    class MakeCustomShape
    {
        //private PLCView plcView = null;
        public DiagramShape[] shape_AreaBox_parts = new DiagramShape[3];
        public DiagramShape[] shape_HardeningRoom_Door = new DiagramShape[8];
        public DiagramShape[,] shape_HardeningRoom_Plt = new DiagramShape[8, 3];
        public DiagramShape[] shape_ChargeRoom_Door = new DiagramShape[1];
        public DiagramShape[] shape_ChargeRoom_Plt = new DiagramShape[2];
        public DiagramShape[] shape_AGVChargeRoom_Doors = new DiagramShape[4];
        public DiagramShape[] shape_LeeHyungGongRoom_Door = new DiagramShape[1];
        public DiagramShape[] shape_LeeHyungGongRoom_Plt = new DiagramShape[2];

        public List<MakeGUI> arrayGUI = new List<MakeGUI>();
        public void Rendering(PLCView plcView)
        {
            //this.plcView = plcView;
            make_ChargeRoom_Box();
            make_ChargeRoom_Tag();
            make_ChargeRoom_Door();
            make_CharegRoom_Plt1();
            make_CharegRoom_Plt2();

            make_AGVChargeRoom_Box();
            make_AGVChargeRoom_Tag();
            make_AGVChargeRoom_Door1();
            make_AGVChargeRoom_Door2();
            make_AGVChargeRoom_Door1_Send();
            make_AGVChargeRoom_Door2_Send();

            make_LeeHyunGongRoom_Box();
            make_LeeHyunGongRoom_Tag();
            make_LeeHyunGongRoom_Door();
            make_LeeHyunGongRoom_Plt1();
            make_LeeHyunGongRoom_Plt2();

            make_HardeningRoom_Box1();
            make_HardeningRoom_Box2();
            make_HardeningRoom_Box3();
            make_HardeningRoom_Box4();
            make_HardeningRoom_Box5();
            make_HardeningRoom_Box6();
            make_HardeningRoom_Box7();
            make_HardeningRoom_Box8();

            make_HardeningRoom_Plt1_1();
            make_HardeningRoom_Plt1_2();
            make_HardeningRoom_Plt1_3();

            make_HardeningRoom_Plt2_1();
            make_HardeningRoom_Plt2_2();
            make_HardeningRoom_Plt2_3();

            make_HardeningRoom_Plt3_1();
            make_HardeningRoom_Plt3_2();
            make_HardeningRoom_Plt3_3();

            make_HardeningRoom_Plt4_1();
            make_HardeningRoom_Plt4_2();
            make_HardeningRoom_Plt4_3();

            make_HardeningRoom_Plt5_1();
            make_HardeningRoom_Plt5_2();
            make_HardeningRoom_Plt5_3();

            make_HardeningRoom_Plt6_1();
            make_HardeningRoom_Plt6_2();
            make_HardeningRoom_Plt6_3();

            make_HardeningRoom_Plt7_1();
            make_HardeningRoom_Plt7_2();
            make_HardeningRoom_Plt7_3();

            make_HardeningRoom_Plt8_1();
            make_HardeningRoom_Plt8_2();
            make_HardeningRoom_Plt8_3();

            make_HardeningRoom_Door1();
            make_HardeningRoom_Door2();
            make_HardeningRoom_Door3();
            make_HardeningRoom_Door4();
            make_HardeningRoom_Door5();
            make_HardeningRoom_Door6();
            make_HardeningRoom_Door7();
            make_HardeningRoom_Door8();

            make_HardeningRoom_Tag1();
            make_HardeningRoom_Tag2();
            make_HardeningRoom_Tag3();
            make_HardeningRoom_Tag4();
            make_HardeningRoom_Tag5();
            make_HardeningRoom_Tag6();
            make_HardeningRoom_Tag7();
            make_HardeningRoom_Tag8();

            make_HardeningRoom_TitleTag();
        }

        void make_ChargeRoom_Box()
        {
            MakeGUI make_ChargeRoom_Box = new MakeGUI
            {
                tagName = "make_ChargeRoom_Box",
                color = Color.FromArgb(243, 221, 191),
                postionX = 2300,
                postionY = 3050,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 700,
                height = 590
            };
            make_ChargeRoom_Box.rendring();
            arrayGUI.Add(make_ChargeRoom_Box);

            shape_AreaBox_parts[0] = make_ChargeRoom_Box.ds;
        }

        void make_ChargeRoom_Tag()
        {
            MakeGUI make_ChargeRoom_Tag = new MakeGUI
            {
                tagName = "make_ChargeRoom_Tag",
                color = Color.FromArgb(255, 255, 255),
                postionX = 2460,
                postionY = 2930,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 400,
                height = 100,
                content = "충전공실",
                pontsize = 25
            };
            make_ChargeRoom_Tag.rendring();
            arrayGUI.Add(make_ChargeRoom_Tag);
        }

        void make_ChargeRoom_Door()
        {
            MakeGUI make_ChargeRoom_Door = new MakeGUI
            {
                tagName = "make_ChargeRoom_Door",
                color = Color.CadetBlue,
                postionX = 2980,
                postionY = 3490,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 50,
                height = 150,
                content = "D\nO\nO\nR",
                pontsize = 7
            };
            make_ChargeRoom_Door.rendring();
            arrayGUI.Add(make_ChargeRoom_Door);

            shape_ChargeRoom_Door[0] = make_ChargeRoom_Door.ds;
        }

        void make_CharegRoom_Plt1()
        {
            MakeGUI make_CharegRoom_Plt1 = new MakeGUI
            {
                tagName = "make_CharegRoom_Plt1",
                color = Color.CadetBlue,
                postionX = 2760,
                postionY = 3100,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT",
                pontsize = 7
            };
            make_CharegRoom_Plt1.rendring();
            arrayGUI.Add(make_CharegRoom_Plt1);

            shape_ChargeRoom_Plt[0] = make_CharegRoom_Plt1.ds;
        }

        void make_CharegRoom_Plt2()
        {
            MakeGUI make_CharegRoom_Plt2 = new MakeGUI
            {
                tagName = "make_CharegRoom_Plt2",
                color = Color.CadetBlue,
                postionX = 2330,
                postionY = 3180,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT",
                pontsize = 7
            };
            make_CharegRoom_Plt2.rendring();
            arrayGUI.Add(make_CharegRoom_Plt2);

            shape_ChargeRoom_Plt[1] = make_CharegRoom_Plt2.ds;
        }



        void make_AGVChargeRoom_Box()
        {
            MakeGUI make_AGVChargeRoom_Box = new MakeGUI
            {
                tagName = "make_AGVChargeRoom_Box",
                color = Color.FromArgb(243, 221, 191),
                postionX = 3425,
                postionY = 180,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 385,
                height = 430
            };
            make_AGVChargeRoom_Box.rendring();
            arrayGUI.Add(make_AGVChargeRoom_Box);

            shape_AreaBox_parts[1] = make_AGVChargeRoom_Box.ds;
        }

        void make_AGVChargeRoom_Tag()
        {
            MakeGUI make_AGVChargeRoom_Tag = new MakeGUI
            {
                tagName = "make_AGVChargeRoom_Tag",
                color = Color.FromArgb(255, 255, 255),
                postionX = 3425,
                postionY = 80,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 450,
                height = 100,
                content = "AGV충전소",
                pontsize = 25
            };
            make_AGVChargeRoom_Tag.rendring();
            arrayGUI.Add(make_AGVChargeRoom_Tag);
        }

        void make_AGVChargeRoom_Door1()
        {
            MakeGUI make_AGVChargeRoom_Door1 = new MakeGUI
            {
                tagName = "make_AGVChargeRoom_Door1",
                color = Color.CadetBlue,
                postionX = 3400,
                postionY = 200,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 50,
                height = 150,
                content = "D\nO\nO\nR",
                pontsize = 7
            };
            make_AGVChargeRoom_Door1.rendring();
            arrayGUI.Add(make_AGVChargeRoom_Door1);

            shape_AGVChargeRoom_Doors[0] = make_AGVChargeRoom_Door1.ds;
        }

        void make_AGVChargeRoom_Door2()
        {
            MakeGUI make_AGVChargeRoom_Door2 = new MakeGUI
            {
                tagName = "make_AGVChargeRoom_Door2",
                color = Color.CadetBlue,
                postionX = 3555,
                postionY = 600,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_AGVChargeRoom_Door2.rendring();
            arrayGUI.Add(make_AGVChargeRoom_Door2);

            shape_AGVChargeRoom_Doors[1] = make_AGVChargeRoom_Door2.ds;
        }

        void make_AGVChargeRoom_Door1_Send()
        {
            MakeGUI make_AGVChargeRoom_Door1_Send1 = new MakeGUI
            {
                tagName = "make_AGVChargeRoom_Door1_Send1",
                color = Color.CadetBlue,
                postionX = 3850,
                postionY = 200,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 100,
                height = 70,
                content = "SEND\nDOOR1",
                pontsize = 5
            };
            make_AGVChargeRoom_Door1_Send1.rendring();
            arrayGUI.Add(make_AGVChargeRoom_Door1_Send1);

            shape_AGVChargeRoom_Doors[2] = make_AGVChargeRoom_Door1_Send1.ds;
        }

        void make_AGVChargeRoom_Door2_Send()
        {
            MakeGUI make_AGVChargeRoom_Door2_Send = new MakeGUI
            {
                tagName = "make_AGVChargeRoom_Door2_Send",
                color = Color.CadetBlue,
                postionX = 3850,
                postionY = 290,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 100,
                height = 70,
                content = "SEND\nDOOR2",
                pontsize = 5
            };
            make_AGVChargeRoom_Door2_Send.rendring();
            arrayGUI.Add(make_AGVChargeRoom_Door2_Send);

            shape_AGVChargeRoom_Doors[3] = make_AGVChargeRoom_Door2_Send.ds;
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
                height = 530
            };
            make_LeeHyunGongRoom_Box.rendring();
            arrayGUI.Add(make_LeeHyunGongRoom_Box);

            shape_AreaBox_parts[2] = make_LeeHyunGongRoom_Box.ds;
        }

        void make_LeeHyunGongRoom_Tag()
        {
            MakeGUI make_LeeHyunGongRoom_Tag = new MakeGUI
            {
                tagName = "make_LeeHyunGongRoom_Tag",
                color = Color.FromArgb(255, 255, 255),
                postionX = 255,
                postionY = 1080,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 400,
                height = 100,
                content = "이형공실",
                pontsize = 25
            };
            make_LeeHyunGongRoom_Tag.rendring();
            arrayGUI.Add(make_LeeHyunGongRoom_Tag);
        }

        void make_LeeHyunGongRoom_Door()
        {
            MakeGUI make_LeeHyunGongRoom_Door = new MakeGUI
            {
                tagName = "make_LeeHyunGongRoom_Door",
                color = Color.CadetBlue,
                postionX = 377,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_LeeHyunGongRoom_Door.rendring();
            arrayGUI.Add(make_LeeHyunGongRoom_Door);

            shape_LeeHyungGongRoom_Door[0] = make_LeeHyunGongRoom_Door.ds;
        }

        void make_LeeHyunGongRoom_Plt1()
        {
            MakeGUI make_LeeHyunGongRoom_Plt1 = new MakeGUI
            {
                tagName = "make_LeeHyunGongRoom_Plt1",
                color = Color.CadetBlue,
                postionX = 405,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_LeeHyunGongRoom_Plt1.rendring();
            arrayGUI.Add(make_LeeHyunGongRoom_Plt1);

            shape_LeeHyungGongRoom_Plt[0] = make_LeeHyunGongRoom_Plt1.ds;
        }

        void make_LeeHyunGongRoom_Plt2()
        {
            MakeGUI make_LeeHyunGongRoom_Plt2 = new MakeGUI
            {
                tagName = "make_LeeHyunGongRoom_Plt2",
                color = Color.CadetBlue,
                postionX = 405,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_LeeHyunGongRoom_Plt2.rendring();
            arrayGUI.Add(make_LeeHyunGongRoom_Plt2);

            shape_LeeHyungGongRoom_Plt[1] = make_LeeHyunGongRoom_Plt2.ds;
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
                height = 630
            };
            make_HardeningRoom_Box1.rendring();
            arrayGUI.Add(make_HardeningRoom_Box1);
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
                height = 630
            };
            make_HardeningRoom_Box2.rendring();
            arrayGUI.Add(make_HardeningRoom_Box2);
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
                height = 630
            };
            make_HardeningRoom_Box3.rendring();
            arrayGUI.Add(make_HardeningRoom_Box3);
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
                height = 630
            };
            make_HardeningRoom_Box4.rendring();
            arrayGUI.Add(make_HardeningRoom_Box4);
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
                height = 630
            };
            make_HardeningRoom_Box5.rendring();
            arrayGUI.Add(make_HardeningRoom_Box5);
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
                height = 630
            };
            make_HardeningRoom_Box6.rendring();
            arrayGUI.Add(make_HardeningRoom_Box6);
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
                height = 630
            };
            make_HardeningRoom_Box7.rendring();
            arrayGUI.Add(make_HardeningRoom_Box7);
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
                height = 630
            };
            make_HardeningRoom_Box8.rendring();
            arrayGUI.Add(make_HardeningRoom_Box8);
        }



        void make_HardeningRoom_Plt1_1()
        {
            MakeGUI make_HardeningRoom_Plt1_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt1_1",
                color = Color.CadetBlue,
                postionX = 2830,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt1_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt1_1);

            shape_HardeningRoom_Plt[0,0] = make_HardeningRoom_Plt1_1.ds;
        }

        void make_HardeningRoom_Plt1_2()
        {
            MakeGUI make_HardeningRoom_Plt1_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt1_2",
                color = Color.CadetBlue,
                postionX = 2830,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt1_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt1_2);

            shape_HardeningRoom_Plt[0, 1] = make_HardeningRoom_Plt1_2.ds;
        }

        void make_HardeningRoom_Plt1_3()
        {
            MakeGUI make_HardeningRoom_Plt1_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt1_3",
                color = Color.CadetBlue,
                postionX = 2830,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt1_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt1_3);

            shape_HardeningRoom_Plt[0, 2] = make_HardeningRoom_Plt1_3.ds;
        }


        void make_HardeningRoom_Plt2_1()
        {
            MakeGUI make_HardeningRoom_Plt2_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt2_1",
                color = Color.CadetBlue,
                postionX = 2595,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt2_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt2_1);

            shape_HardeningRoom_Plt[1, 0] = make_HardeningRoom_Plt2_1.ds;
        }

        void make_HardeningRoom_Plt2_2()
        {
            MakeGUI make_HardeningRoom_Plt2_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt2_2",
                color = Color.CadetBlue,
                postionX = 2595,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt2_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt2_2);

            shape_HardeningRoom_Plt[1, 1] = make_HardeningRoom_Plt2_2.ds;
        }

        void make_HardeningRoom_Plt2_3()
        {
            MakeGUI make_HardeningRoom_Plt2_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt1_3",
                color = Color.CadetBlue,
                postionX = 2595,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt2_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt2_3);

            shape_HardeningRoom_Plt[1, 2] = make_HardeningRoom_Plt2_3.ds;
        }


        void make_HardeningRoom_Plt3_1()
        {
            MakeGUI make_HardeningRoom_Plt3_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt3_1",
                color = Color.CadetBlue,
                postionX = 2360,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt3_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt3_1);

            shape_HardeningRoom_Plt[2, 0] = make_HardeningRoom_Plt3_1.ds;
        }

        void make_HardeningRoom_Plt3_2()
        {
            MakeGUI make_HardeningRoom_Plt3_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt3_2",
                color = Color.CadetBlue,
                postionX = 2360,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt3_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt3_2);

            shape_HardeningRoom_Plt[2, 1] = make_HardeningRoom_Plt3_2.ds;
        }

        void make_HardeningRoom_Plt3_3()
        {
            MakeGUI make_HardeningRoom_Plt3_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt3_3",
                color = Color.CadetBlue,
                postionX = 2360,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt3_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt3_3);

            shape_HardeningRoom_Plt[2, 2] = make_HardeningRoom_Plt3_3.ds;
        }


        void make_HardeningRoom_Plt4_1()
        {
            MakeGUI make_HardeningRoom_Plt4_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt4_1",
                color = Color.CadetBlue,
                postionX = 2130,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt4_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt4_1);

            shape_HardeningRoom_Plt[3, 0] = make_HardeningRoom_Plt4_1.ds;
        }

        void make_HardeningRoom_Plt4_2()
        {
            MakeGUI make_HardeningRoom_Plt4_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt4_2",
                color = Color.CadetBlue,
                postionX = 2130,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt4_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt4_2);

            shape_HardeningRoom_Plt[3, 1] = make_HardeningRoom_Plt4_2.ds;
        }

        void make_HardeningRoom_Plt4_3()
        {
            MakeGUI make_HardeningRoom_Plt4_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt4_3",
                color = Color.CadetBlue,
                postionX = 2130,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt4_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt4_3);

            shape_HardeningRoom_Plt[3, 2] = make_HardeningRoom_Plt4_3.ds;
        }


        void make_HardeningRoom_Plt5_1()
        {
            MakeGUI make_HardeningRoom_Plt5_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt5_1",
                color = Color.CadetBlue,
                postionX = 1905,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt5_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt5_1);

            shape_HardeningRoom_Plt[4, 0] = make_HardeningRoom_Plt5_1.ds;
        }

        void make_HardeningRoom_Plt5_2()
        {
            MakeGUI make_HardeningRoom_Plt5_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt5_2",
                color = Color.CadetBlue,
                postionX = 1905,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt5_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt5_2);

            shape_HardeningRoom_Plt[4, 1] = make_HardeningRoom_Plt5_2.ds;
        }

        void make_HardeningRoom_Plt5_3()
        {
            MakeGUI make_HardeningRoom_Plt5_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt5_3",
                color = Color.CadetBlue,
                postionX = 1905,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt5_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt5_3);

            shape_HardeningRoom_Plt[4, 2] = make_HardeningRoom_Plt5_3.ds;
        }


        void make_HardeningRoom_Plt6_1()
        {
            MakeGUI make_HardeningRoom_Plt6_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt6_1",
                color = Color.CadetBlue,
                postionX = 1675,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt6_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt6_1);

            shape_HardeningRoom_Plt[5, 0] = make_HardeningRoom_Plt6_1.ds;
        }

        void make_HardeningRoom_Plt6_2()
        {
            MakeGUI make_HardeningRoom_Plt6_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt6_2",
                color = Color.CadetBlue,
                postionX = 1675,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt6_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt6_2);

            shape_HardeningRoom_Plt[5, 1] = make_HardeningRoom_Plt6_2.ds;
        }

        void make_HardeningRoom_Plt6_3()
        {
            MakeGUI make_HardeningRoom_Plt6_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt6_3",
                color = Color.CadetBlue,
                postionX = 1675,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt6_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt6_3);

            shape_HardeningRoom_Plt[5, 2] = make_HardeningRoom_Plt6_3.ds;
        }


        void make_HardeningRoom_Plt7_1()
        {
            MakeGUI make_HardeningRoom_Plt7_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt7_1",
                color = Color.CadetBlue,
                postionX = 1445,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt7_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt7_1);

            shape_HardeningRoom_Plt[6, 0] = make_HardeningRoom_Plt7_1.ds;
        }

        void make_HardeningRoom_Plt7_2()
        {
            MakeGUI make_HardeningRoom_Plt7_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt7_2",
                color = Color.CadetBlue,
                postionX = 1445,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt7_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt7_2);

            shape_HardeningRoom_Plt[6, 1] = make_HardeningRoom_Plt7_2.ds;
        }

        void make_HardeningRoom_Plt7_3()
        {
            MakeGUI make_HardeningRoom_Plt7_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt7_3",
                color = Color.CadetBlue,
                postionX = 1445,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt7_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt7_3);

            shape_HardeningRoom_Plt[6, 2] = make_HardeningRoom_Plt7_3.ds;
        }


        void make_HardeningRoom_Plt8_1()
        {
            MakeGUI make_HardeningRoom_Plt8_1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt8_1",
                color = Color.CadetBlue,
                postionX = 1215,
                postionY = 1055,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#1",
                pontsize = 7
            };
            make_HardeningRoom_Plt8_1.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt8_1);

            shape_HardeningRoom_Plt[7, 0] = make_HardeningRoom_Plt8_1.ds;
        }

        void make_HardeningRoom_Plt8_2()
        {
            MakeGUI make_HardeningRoom_Plt8_2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt8_2",
                color = Color.CadetBlue,
                postionX = 1215,
                postionY = 955,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#2",
                pontsize = 7
            };
            make_HardeningRoom_Plt8_2.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt8_2);

            shape_HardeningRoom_Plt[7, 1] = make_HardeningRoom_Plt8_2.ds;
        }

        void make_HardeningRoom_Plt8_3()
        {
            MakeGUI make_HardeningRoom_Plt8_3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Plt8_3",
                color = Color.CadetBlue,
                postionX = 1215,
                postionY = 855,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 80,
                height = 80,
                content = "PLT#3",
                pontsize = 7
            };
            make_HardeningRoom_Plt8_3.rendring();
            arrayGUI.Add(make_HardeningRoom_Plt8_3);

            shape_HardeningRoom_Plt[7, 2] = make_HardeningRoom_Plt8_3.ds;
        }


        void make_HardeningRoom_Door1()
        {
            MakeGUI make_HardeningRoom_Door1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door1",
                color = Color.CadetBlue,
                postionX = 2790,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door1.rendring();
            arrayGUI.Add(make_HardeningRoom_Door1);

            shape_HardeningRoom_Door[0] = make_HardeningRoom_Door1.ds;
        }

        void make_HardeningRoom_Door2()
        {
            MakeGUI make_HardeningRoom_Door2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door2",
                color = Color.CadetBlue,
                postionX = 2555,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door2.rendring();
            arrayGUI.Add(make_HardeningRoom_Door2);

            shape_HardeningRoom_Door[1] = make_HardeningRoom_Door2.ds;
        }

        void make_HardeningRoom_Door3()
        {
            MakeGUI make_HardeningRoom_Door3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door3",
                color = Color.CadetBlue,
                postionX = 2320,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door3.rendring();
            arrayGUI.Add(make_HardeningRoom_Door3);

            shape_HardeningRoom_Door[2] = make_HardeningRoom_Door3.ds;
        }

        void make_HardeningRoom_Door4()
        {
            MakeGUI make_HardeningRoom_Door4 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door4",
                color = Color.CadetBlue,
                postionX = 2090,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door4.rendring();
            arrayGUI.Add(make_HardeningRoom_Door4);

            shape_HardeningRoom_Door[3] = make_HardeningRoom_Door4.ds;
        }

        void make_HardeningRoom_Door5()
        {
            MakeGUI make_HardeningRoom_Door5 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door5",
                color = Color.CadetBlue,
                postionX = 1865,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door5.rendring();
            arrayGUI.Add(make_HardeningRoom_Door5);

            shape_HardeningRoom_Door[4] = make_HardeningRoom_Door5.ds;
        }

        void make_HardeningRoom_Door6()
        {
            MakeGUI make_HardeningRoom_Door6 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door6",
                color = Color.CadetBlue,
                postionX = 1635,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door6.rendring();
            arrayGUI.Add(make_HardeningRoom_Door6);

            shape_HardeningRoom_Door[5] = make_HardeningRoom_Door6.ds;
        }

        void make_HardeningRoom_Door7()
        {
            MakeGUI make_HardeningRoom_Door7 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door7",
                color = Color.CadetBlue,
                postionX = 1405,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door7.rendring();
            arrayGUI.Add(make_HardeningRoom_Door7);

            shape_HardeningRoom_Door[6] = make_HardeningRoom_Door7.ds;
        }

        void make_HardeningRoom_Door8()
        {
            MakeGUI make_HardeningRoom_Door8 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Door8",
                color = Color.CadetBlue,
                postionX = 1175,
                postionY = 515,
                shapes = BasicShapes.Rectangle,
                bordersize = 1,
                width = 150,
                height = 50,
                content = "DOOR",
                pontsize = 7
            };
            make_HardeningRoom_Door8.rendring();
            arrayGUI.Add(make_HardeningRoom_Door8);

            shape_HardeningRoom_Door[7] = make_HardeningRoom_Door8.ds;
        }


        void make_HardeningRoom_Tag1()
        {
            MakeGUI make_HardeningRoom_Tag1 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag1",
                color = Color.FromArgb(255, 255, 255),
                postionX = 2760,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#1",
                pontsize = 25
            };
            make_HardeningRoom_Tag1.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag1);
        }

        void make_HardeningRoom_Tag2()
        {
            MakeGUI make_HardeningRoom_Tag2 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag2",
                color = Color.FromArgb(255, 255, 255),
                postionX = 2510,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#2",
                pontsize = 25
            };
            make_HardeningRoom_Tag2.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag2);
        }

        void make_HardeningRoom_Tag3()
        {
            MakeGUI make_HardeningRoom_Tag3 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag3",
                color = Color.FromArgb(255, 255, 255),
                postionX = 2280,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#3",
                pontsize = 25
            };
            make_HardeningRoom_Tag3.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag3);
        }

        void make_HardeningRoom_Tag4()
        {
            MakeGUI make_HardeningRoom_Tag4 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag4",
                color = Color.FromArgb(255, 255, 255),
                postionX = 2050,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#8",
                pontsize = 25
            };
            make_HardeningRoom_Tag4.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag4);
        }

        void make_HardeningRoom_Tag5()
        {
            MakeGUI make_HardeningRoom_Tag5 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag5",
                color = Color.FromArgb(255, 255, 255),
                postionX = 1825,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#4",
                pontsize = 25
            };
            make_HardeningRoom_Tag5.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag5);
        }

        void make_HardeningRoom_Tag6()
        {
            MakeGUI make_HardeningRoom_Tag6 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag6",
                color = Color.FromArgb(255, 255, 255),
                postionX = 1595,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#5",
                pontsize = 25
            };
            make_HardeningRoom_Tag6.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag6);
        }

        void make_HardeningRoom_Tag7()
        {
            MakeGUI make_HardeningRoom_Tag7 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag7",
                color = Color.FromArgb(255, 255, 255),
                postionX = 1365,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#6",
                pontsize = 25
            };
            make_HardeningRoom_Tag7.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag7);
        }

        void make_HardeningRoom_Tag8()
        {
            MakeGUI make_HardeningRoom_Tag8 = new MakeGUI
            {
                tagName = "make_HardeningRoom_Tag8",
                color = Color.FromArgb(255, 255, 255),
                postionX = 1135,
                postionY = 1180,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 250,
                height = 100,
                content = "#7",
                pontsize = 25
            };
            make_HardeningRoom_Tag8.rendring();
            arrayGUI.Add(make_HardeningRoom_Tag8);
        }

        void make_HardeningRoom_TitleTag()
        {
            MakeGUI make_HardeningRoom_TitleTag = new MakeGUI
            {
                tagName = "make_HardeningRoom_TitleTag",
                color = Color.FromArgb(255, 255, 255),
                postionX = 2050,
                postionY = 1280,
                shapes = BasicShapes.Rectangle,
                bordersize = 0,
                width = 300,
                height = 100,
                content = "경화실",
                pontsize = 25
            };
            make_HardeningRoom_TitleTag.rendring();
            arrayGUI.Add(make_HardeningRoom_TitleTag);
        }

        
    }
}
