using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.data.MSSQL;

namespace ACSManager
{
    public static class Setting
    {

        private static readonly string FILE_NAME = "Setting";

        // 선언은 여기서, 초기화는 Login ChangeSetting
        public static MapDesign.nodeDataTable nodTbl = null;

        public static MapDesign.Link_Node_JoinDataTable linkNodeJoinTbl = null;



        public static MapDesign.Group_Node_JoinDataTable groupNodeJoinTbl = null;

        #region Property
        private static string db_server_ip = "127.0.0.1";
        public static string DB_SERVER_IP
        {
            get
            {
                db_server_ip = IniControllor.GetValue(FILE_NAME, "DATABASE", "DB_SERVER_IP");
                return db_server_ip;
            }
            set
            {
                db_server_ip = value;
                IniControllor.SetValue(FILE_NAME, "DATABASE", "DB_SERVER_IP", db_server_ip);
            }
        }

        private static int floor = 1;
        public static int FLOOR
        {
            get
            {
                floor = Properties.Settings.Default.Floor;
                return floor;
            }
            set
            {
                floor = value;
                IniControllor.SetValue(FILE_NAME, "ETC", "FLOOR", floor.ToString());
                Properties.Settings.Default.Floor = floor;
                Properties.Settings.Default.Save();
            }
        }

        private static bool use_tub_process = false;
        public static bool USE_TUB_PROCESS
        {
            get
            {
                use_tub_process = bool.Parse(IniControllor.GetValue(FILE_NAME, "ETC", "TUB_PROCESS"));
                return use_tub_process;
            }
            set
            {
                use_tub_process = value;
                IniControllor.SetValue(FILE_NAME, "ETC", "TUB_PROCESS", use_tub_process.ToString());
            }
        }

        private static bool saved_id = false;
        public static bool SAVED_ID
        {
            get
            {
                saved_id = Properties.Settings.Default.SavedID;
                return saved_id;
            }
            set
            {
                saved_id = value;
                Properties.Settings.Default.SavedID = saved_id;
                Properties.Settings.Default.Save();
            }
        }

        private static string login_id = "admin";
        public static string LOGIN_ID
        {
            get
            {
                login_id = Properties.Settings.Default.LoginID;
                return login_id;
            }
            set
            {
                login_id = value;
                Properties.Settings.Default.LoginID = login_id;
                Properties.Settings.Default.Save();
            }
        }


        // Station 개수
        private static int station_count = 0;
        public static int Station_Count
        {
            get
            {
                station_count = Properties.Settings.Default.Station_Count;
                return station_count;
            }
            set
            {
                station_count = value;
                Properties.Settings.Default.Station_Count = station_count;
                Properties.Settings.Default.Save();
            }
        }


        // Port 개수
        private static int port_count = 0;
        public static int Port_Count
        {
            get
            {
                port_count = Properties.Settings.Default.Port_Count;
                return port_count;
            }
            set
            {
                port_count = value;
                Properties.Settings.Default.Port_Count = port_count;
                Properties.Settings.Default.Save();
            }
        }


        private static string station_name = "";
        public static string Station_Name
        {
            get
            {
                station_name = Properties.Settings.Default.Station_Name;
                return station_name;
            }
            set
            {
                station_name = value;
                Properties.Settings.Default.Station_Name = station_name;
                Properties.Settings.Default.Save();
            }
        }

        private static long map_id = 0;
        public static long MAP_ID
        {
            get { return map_id;}
            set { map_id = value; }
        }


        private static string logSaveDay = "";
        public static string LogSaveDay
        {
            get
            {
                try
                {
                    logSaveDay = Properties.Settings.Default.LogSaveDay;
                }
                catch
                {
                    logSaveDay = "365";
                }
                return logSaveDay;
            }
        }



        #endregion

        public static void CheckAndMakeFile()
        {
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, FILE_NAME+".ini")))
            {
                IniControllor.SetValue(FILE_NAME, "DATABASE", "DB_SERVER_IP", db_server_ip);
                IniControllor.SetValue(FILE_NAME, "ETC", "FLOOR", floor.ToString());
                IniControllor.SetValue(FILE_NAME, "ETC", "TUB_PROCESS", use_tub_process.ToString());
                IniControllor.SetValue(FILE_NAME, "ETC", "SAVED_ID", saved_id.ToString());
                IniControllor.SetValue(FILE_NAME, "ETC", "LOGIN_ID", login_id.ToString());

                FLOOR = Properties.Settings.Default.Floor;
            }
        }
    }
}
