using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ACSManager
{
    public class IniControllor
    {
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        /// ini파일에서 값을 읽어온다
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <param name="group">그룹(섹션)</param>
        /// <param name="key">key</param>
        /// <param name="isIncludedExcuteProject">프로젝트에 표함된 ini파일여부(프로젝트에 포함된경우 bin/debug 경로에 존재하는 fileName에 해당하는 ini파일을 의미)</param>
        /// <returns></returns>
        public static string GetValue(string fileName,string group, string key, bool isIncludedExcuteProject = true)
        {
            var strReturn = new StringBuilder(255);

            GetPrivateProfileString(group, key, "", strReturn, 255, GetInitPath(fileName, isIncludedExcuteProject));

            return strReturn.ToString().Trim();
        }

        /// <summary>
        /// ini파일에 값을 Setting 한다
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isIncludedExcuteProject">프로젝트에 표함된 ini파일여부(프로젝트에 포함된경우 bin/debug 경로에 존재하는 fileName에 해당하는 ini파일을 의미)</param>
        public static void SetValue(string fileName, string group, string key, string value, bool isIncludedExcuteProject = true)
        {
            WritePrivateProfileString(group, key, value, GetInitPath(fileName, isIncludedExcuteProject));
        }

        private static string GetInitPath(string fileName, bool isIncludedExcuteProject = true)
        {
            if(isIncludedExcuteProject)
                return Path.Combine(Environment.CurrentDirectory, $"{fileName}.ini");

            return fileName;
            
            //return Path.Combine(Environment.CurrentDirectory, "Config", "config.ini");
        }
    }
}