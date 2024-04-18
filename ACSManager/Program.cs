using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using ACSManager.FORM;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.IO;

namespace ACSManager
{
    static class Program
    {
        public static MainForm main_process;
        public static UserData USER_DATA = new UserData();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();


            //INI 파일이 있는지 없는지 확인하고 없을 시 초기화 파일 생성
            //Setting.CheckAndMakeFile();
            
            LogIn lg = new LogIn();
            if (lg.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show("T");
                //splash msp = new splash();
                //msp.ShowDialog();
                main_process = new MainForm();
                Application.Run(main_process);
            }
        }
    }
}
