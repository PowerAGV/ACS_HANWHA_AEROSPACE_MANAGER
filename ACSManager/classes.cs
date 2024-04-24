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
    class classes
    {
    }

    static public class ConfigInfo
    {
        public static string map_id = "";
    }

    static public class globalRouteInfo
    {

        public static List<string> Charge_Location = new List<string>();

        public static DirectedListGraph<int, ImVader.WeightedEdge> m_path = null;
        public static List<string> nodes = new List<string>(); 
        public static List<LinkInfoD> m_linkes = new List<LinkInfoD>();
    }

    public class Angle
    {
        #region constructors
        /// <summary>
        /// Blank constructor. Remember to manually set some data to the properties!
        /// </summary>
        public Angle()
        {

        }
        /// <summary>
        /// Construct an angle with either a degree or a radian value
        /// </summary>
        /// <param name="input">Angle value</param>
        /// <param name="angleType">Specify whether input is in radians or degrees</param>
        public Angle(double input, Type angleType = Angle.Type.Degrees)
        {
            if (angleType == Type.Degrees)
            {
                Degrees = input;
            }
            else if (angleType == Type.Radians)
            {
                Radians = input;
            }
        }
        /// <summary>
        /// Quickly construct a common angle
        /// </summary>
        /// <param name="preset">Choose angle</param>
        public Angle(Angle.Preset preset)
        {
            if (preset == Angle.Preset.Deg0) { Degrees = 0; }
            else if (preset == Preset.Deg180) { Degrees = 180; }
            else if (preset == Preset.Deg360) { Degrees = 360; }
            else if (preset == Preset.Rad2Pi) { Radians = twoPi; }
            else if (preset == Preset.RadPi) { Radians = Pi; }
            else Radians = 0;
        }
        #endregion constructors


        #region properties
        private double degrees;

        /// <summary>
        /// Angle in degrees
        /// </summary>
        public double Degrees
        {
            get { return degrees; }
            set
            {
                degrees = value;
                radians = ToRadians(value);
                updateFixedangles();
            }
        }

        private double radians;
        /// <summary>
        /// Angle in radians between -pi to pi
        /// </summary>
        public double Radians
        {
            get { return radians; }
            set
            {
                radians = value;
                degrees = ToDegrees(value);
                updateFixedangles();
            }
        }

        private double radians2pi;
        /// <summary>
        /// Angle in radians, modified to fall between 0 and 2pi. Read-only.
        /// </summary>
        public double Radians2pi
        {
            get { return radians2pi; }
        }

        private double degrees360;
        /// <summary>
        /// Value in degrees between 0 and 360. Read-only. 
        /// </summary>
        public double Degrees360
        {
            get { return degrees360; }
        }


        #endregion properties


        #region enums
        public enum Type { Radians, Degrees }
        /// <summary>
        /// Presets for quick setup of Angle constructor
        /// </summary>
        public enum Preset { Deg0, Deg180, Deg360, RadPi, Rad2Pi }
        #endregion enums


        #region constants
        private const double twoPi = 2 * Math.PI;
        private const double Pi = Math.PI;
        #endregion constants


        #region methods
        /// <summary>
        /// Convert an input angle of degrees to radians. Does not affect any properties in this class - set properties instead.
        /// </summary>
        /// <param name="val">Input in degrees</param>
        /// <returns>Value in radians</returns>
        public static double ToRadians(double val)
        {
            return val / (180 / Pi);
        }

        /// <summary>
        /// Convert an input angle of radians into degrees. Does not affect any properties in this class - set properties instead.
        /// </summary>
        /// <param name="val">Input in radians</param>
        /// <returns>Value in degrees</returns>
        public static double ToDegrees(double val)
        {
            return val * (180 / Pi);
        }

        /// <summary>
        /// Fixes an angle to between 0 and 360 or 2pi.
        /// </summary>
        /// <param name="val">Input angle</param>
        /// <param name="type">Specify whether the input angle is radians or degrees</param>
        /// <returns>The angle, fixed to between 0 and 360 or 0 and 2pi</returns>
        public static double FixAngle(double val, Type type)
        {
            if (type == Type.Radians)
            {
                //-2pi to 0 to between 0 and 2pi
                if (val < 0)
                {
                    return 2 * Math.PI - (Math.Abs(val) % (2 * Math.PI));
                }
                //over 2pi to between 0 and 2pi
                else if (val > 2 * Math.PI)
                {
                    return val % (2 * Math.PI);
                }
                //else it's fine, return it back
                else
                {
                    return val;
                }
            }
            else if (type == Type.Degrees)
            {
                //-360 to 0 to between 0 and 360
                if (val < 0)
                {
                    return 360 - (Math.Abs(val) % 360);
                }
                //over 360 to between 0 and 360
                else if (val > 360)
                {
                    return val % 360;
                }
                //else it's fine, return it back
                else
                {
                    return val;
                }
            }
            else return -1; //something's gone wrong
        }

        /// <summary>
        /// Looks at the radians and degrees properties, and updates their respective fixed angles
        /// </summary>
        private void updateFixedangles()
        {
            radians2pi = FixAngle(radians, Type.Radians);
            degrees360 = FixAngle(degrees, Type.Degrees);
        }

        /// <summary>
        /// Copies Radians2pi to Radians, and Degrees360 to Degrees. (I.e. fixes radians to 0<2pi and degrees to 0<360)
        /// </summary>
        public void FixAngles()
        {
            updateFixedangles();
            Radians = Radians2pi; //this also calls Degrees
        }

        #endregion methods


        #region operators

        /// <summary>
        /// Returns the sum of two angles
        /// </summary>
        /// <param name="a1">First angle</param>
        /// <param name="a2">Second angle</param>
        /// <returns>An angle constructed from the radian sum of the input angles</returns>
        public static Angle operator +(Angle a1, Angle a2)
        {
            return new Angle(a1.Radians + a2.Radians, Type.Radians);
        }

        /// <summary>
        /// Returns the difference between two angles
        /// </summary>
        /// <param name="a1">First angle</param>
        /// <param name="a2">Second angle</param>
        /// <returns>An angle constructed from the value that is the first angle minus the second angle</returns>
        public static Angle operator -(Angle a1, Angle a2)
        {
            return new Angle(a1.Radians - a2.Radians, Type.Radians);
        }

        /// <summary>
        /// Returns the exact division between two angles (i.e. how many times does the second angle fit into the first)
        /// </summary>
        /// <param name="a1">Numerator angle</param>
        /// <param name="a2">Dedominator angle</param>
        /// <returns>A new angle constructed from the value that is the first angle in radians divided by the second angle in radians</returns>
        public static Angle operator /(Angle a1, Angle a2)
        {
            return new Angle(a1.Radians / a2.Radians, Type.Radians);
        }

        public static Angle operator *(Angle a, double d)
        {
            return new Angle(a.Radians * d, Type.Radians);
        }

        public static Angle operator *(double d, Angle a)
        {
            return new Angle(a.Radians * d, Type.Radians);
        }

        public static Angle operator /(Angle a, double d)
        {
            return new Angle(a.Radians / d, Type.Radians);
        }

        public static bool operator <(Angle a1, Angle a2)
        {
            if (a1.Radians < a2.Radians) return true;
            else return false;
        }

        public static bool operator >(Angle a1, Angle a2)
        {
            if (a1.Radians > a2.Radians) return true;
            else return false;
        }

        public static bool operator <=(Angle a1, Angle a2)
        {
            if (a1.Radians <= a2.Radians) return true;
            else return false;
        }

        public static bool operator >=(Angle a1, Angle a2)
        {
            if (a1.Radians >= a2.Radians) return true;
            else return false;
        }

        public static implicit operator double(Angle angleobj)
        {
            return angleobj.Radians;
        }

        public static implicit operator string(Angle a)
        {
            return a.ToString();
        }

        #endregion operators


        #region overrides

        public override string ToString()
        {
            return Radians.ToString() + " radians";
        }

        #endregion overrides


    }

    public class LinkInfoD
    {
        public int m_from; // only index
        public int m_to;

        public string sFrom;
        public string sTo;

        public int m_angle = 0;
        public string m_speed = "0";

        public bool m_sensorOnOff = true;
        //public short sensor_r = 1;
        //public short sensor_l = 1;
        public string str_id = "";

        public LinkInfoD(int f, int t, int a, string spd, string sf, string st, bool sensorOnOff, string strID)
        {
            sFrom = sf;
            sTo = st;
            m_from = f;
            m_to = t;
            m_angle = a;
            m_speed = spd;
            m_sensorOnOff = sensorOnOff;
            str_id = strID;
            //sensor_r = s_r;
            //sensor_l = s_l;
        }
    }
}
