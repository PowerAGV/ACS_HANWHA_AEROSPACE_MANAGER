
using ACSManager;
using ACSManager.DataSet1MTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ACSManager
{
    class MCSDB
    {
        //public static string connStr = "server=150.150.247.174;user=icss_app;database=icssp;port=4000;password=test1234!;Pooling=false; convert zero datetime=True";

        public enum JosStatus
        {
            wait = 0,
            reserve,
            defature,
            arrival,
            server_cancel,
            req_start,
            cancel
        }

        /*
        public static List<vw_agvc_get_order_search> GetMCSInfo()
        {
            try
            {
               
                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                conn.Open();
                string query = "SELECT * FROM   vw_agvc_get_order_search limit 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                DataTable t1 = new DataTable();
                using (MySqlDataAdapter a = new MySqlDataAdapter(cmd))
                {
                    a.Fill(t1);
                }
                conn.Close();
                return Helper.DataTableToList<vw_agvc_get_order_search>(t1);
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
            }
            return null;
        }

        public static MCSReturnValue ReserveAckMCS(string ID,string agvID)
        {
            MCSReturnValue rval = new MCSReturnValue();
            try
            {
                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                conn.Open();
                string query = "CALL SP_AGVc_GET_ORDER_RESERVE('"+ agvID + "','"+ ID + "')";// limit 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                DataTable t1 = new DataTable();
                using (MySqlDataAdapter a = new MySqlDataAdapter(cmd))
                {
                    a.Fill(t1);
                    foreach(DataRow dr in t1.Select())
                    {
                        rval.RESULT = dr["RESULT"].ToString();
                        rval.ERRMSG = dr["ERRMSG"].ToString();
                        break;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
            }
            return rval;
        }

        public static MCSReturnValue AGVDepart(string ID)
        {
            MCSReturnValue rval = new MCSReturnValue();
            try
            {
                rval.RESULT = "";
                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                conn.Open();
                string query = "CALL SP_AGVc_GET_ORDER_DEPART('" + ID + "')";// limit 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                DataTable t1 = new DataTable();
                using (MySqlDataAdapter a = new MySqlDataAdapter(cmd))
                {
                    a.Fill(t1);
                    foreach (DataRow dr in t1.Select())
                    {
                        rval.RESULT = dr["RESULT"].ToString();
                        rval.ERRMSG = dr["ERRMSG"].ToString();
                        break;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
            }
            return rval;
        }
        */
        public static MCSReturnValue AGVArrived(string ID)
        {
            MCSReturnValue rval = new MCSReturnValue();
            try
            {
                InsertMessage(ID, "ORDER_COMPLETE");

                var MCSADT = new DataSet1MTableAdapters.tb_mcs_commandTableAdapter();
                MCSADT.UpdateJobArrive((int)JosStatus.arrival, DateTime.Now, ID);
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
            }
            return rval;
        }

        public static string AGVEmergency(string ID)
        {
            try
            {
                InsertMessage(ID, "ORDER_CANCEL");

                tb_mcs_commandTableAdapter MCSADT = new tb_mcs_commandTableAdapter();
                MCSADT.UpdateJobCancel((int)JosStatus.cancel, DateTime.Now, ID);

                //rval = GetResult(ID, "ORDER_CANCEL"); //확인 후 수정
            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
            }
            return "Send Cancel Messsage";
        }

        public static string AGVComplete(string ID)
        {
            try
            {
                InsertMessage(ID, "ORDER_COMPLETE");

                tb_mcs_commandTableAdapter MCSADT = new DataSet1MTableAdapters.tb_mcs_commandTableAdapter();
                MCSADT.UpdateJobArrive((int)JosStatus.arrival, DateTime.Now, ID);

            }
            catch (Exception ex)
            {
                TraceManager.AddLog(string.Format("{0}\r\n{1}", ex.StackTrace, ex.Message));
            }
            return "Send Cancel Messsage";
        }

        static void InsertMessage(string order_id, string message, string scanvalue = "")
        {
            try
            {
                string cnt = "0";
                tb_mcs_messageTableAdapter adtp = new tb_mcs_messageTableAdapter();
                cnt = adtp.IsExistMessage(order_id, message).ToString();

                if (cnt == "0")
                {
                    if (scanvalue.Length > 1)
                    {
                        adtp.InsertMessageWithScanValue(order_id, message, scanvalue);
                    }
                    else
                    {
                        adtp.InsertMessage(order_id, message);
                    }
                }
                else
                {
                    // 다시 보내기 위해 wait 으로 변경
                    if (message == "REQUEST_START" || message == "REQUEST_EMPTY") // 반복 전송을 위함.
                    {
                        adtp.UpdateMessageStatus("wait", order_id, message);
                    }
                }
            }
            catch (Exception ee)
            {
                TraceManager.AddLog(string.Format("{0}r\n{1}", ee.StackTrace, ee.Message));
            }

        }

    }

}

public class MCSReturnValue
{
    public string RESULT { get; set; }
    public string ERRMSG { get; set; }
}

public class vw_agvc_get_order_search
    {
        public string SUPPLYID { get; set; }
        public string CARRIERID { get; set; }
        public string FROM_LOCATION { get; set; }
        public string TO_LOCATION { get; set; }
        public string KANBANID { get; set; }
        public string PARTNO { get; set; }
        public DateTime SUPPLYDTTM { get; set; }
    }

    public static class Helper
    {
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }


        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return date == null ? string.Empty : Convert.ToDateTime(date).ConvertDate();
        }

        private static string ConvertToString(object value)
        {
            return Convert.ToString(ReturnEmptyIfNull(value));
        }

        private static int ConvertToInt(object value)
        {
            return Convert.ToInt32(ReturnZeroIfNull(value));
        }

        private static long ConvertToLong(object value)
        {
            return Convert.ToInt64(ReturnZeroIfNull(value));
        }

        private static decimal ConvertToDecimal(object value)
        {
            return Convert.ToDecimal(ReturnZeroIfNull(value));
        }

        private static DateTime convertToDateTime(object date)
        {
            return Convert.ToDateTime(ReturnDateTimeMinIfNull(date));
        }

        public static string ConvertDate(this DateTime datetTime, bool excludeHoursAndMinutes = false)
        {
            if (datetTime != DateTime.MinValue)
            {
                if (excludeHoursAndMinutes)
                    return datetTime.ToString("yyyy-MM-dd");
                return datetTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            return null;
        }
        public static object ReturnEmptyIfNull(this object value)
        {
            if (value == DBNull.Value)
                return string.Empty;
            if (value == null)
                return string.Empty;
            return value;
        }
        public static object ReturnZeroIfNull(this object value)
        {
            if (value == DBNull.Value)
                return 0;
            if (value == null)
                return 0;
            return value;
        }
        public static object ReturnDateTimeMinIfNull(this object value)
        {
            if (value == DBNull.Value)
                return DateTime.MinValue;
            if (value == null)
                return DateTime.MinValue;

            return value;        
        }
}




