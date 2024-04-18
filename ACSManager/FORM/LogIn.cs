using DevExpress.XtraEditors;
using System;
using System.Linq;
using System.Windows.Forms;
using ACSManager.data.MSSQL.MapDesignTableAdapters;
using ACSManager.data.MSSQL;

namespace ACSManager.FORM
{
    public partial class LogIn : DevExpress.XtraEditors.XtraForm
    {

        
        /// <summary>
        /// 생성자1
        /// </summary>
        public LogIn()
        {       
            InitializeComponent();
            DialogResult = DialogResult.Cancel;            
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            radioGroup1.SelectedIndex = 0;
            
            checkEdit1.Checked = Setting.SAVED_ID;

            if (Setting.SAVED_ID)
                txtID.Text = Setting.LOGIN_ID;
            else
                txtID.Text = "";

            this.ActiveControl = txtID;
        }


        #region Event Function
        /// <summary>
        /// 확인 버튼 이벤트 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            Check_ID_Password();

            if (this.DialogResult == DialogResult.OK)
                ChangeSetting();
        }

        /// <summary>
        /// 취소 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 키 눌렀을때 반응 (엔터키)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                Check_ID_Password();

                if (this.DialogResult == DialogResult.OK)
                    ChangeSetting();
            }
        }
        #endregion


        #region OtherFunction
        private void Check_ID_Password()
        {
            if (txtID.Text.Length < 3)
            {
                XtraMessageBox.Show("Please ID Check", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPass.Text.Length < 4)
            {
                XtraMessageBox.Show("Please PassWord Check", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataSet1MTableAdapters.tb_userTableAdapter adtUsr = new DataSet1MTableAdapters.tb_userTableAdapter();
                //long findCon = (long)adtUsr.GetLoginAction(txtID.Text.Trim(), txtPass.Text.Trim());
                DataSet1M.tb_userDataTable tbl = adtUsr.GetDataByLogin(txtID.Text.Trim(), txtPass.Text.Trim());
                foreach (DataSet1M.tb_userRow ritem in tbl)
                {
                    Program.USER_DATA.UserID = ritem.user_id;
                    Program.USER_DATA.UserGroup = ritem.user_type;

                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }
            }
            catch (Exception cc)
            {
                
            }
            XtraMessageBox.Show("Please Check your information", "AGV Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ChangeSetting()
        {
            Setting.SAVED_ID = checkEdit1.Checked;

            if (checkEdit1.Checked)
                Setting.LOGIN_ID = txtID.Text;

            var table = new mapTableAdapter().GetCurrentMap(); //DataSet1MTableAdapters.m_map_masterTableAdapter().GetActiveFloorID();

            if (table != null && table.Rows.Count > 0)
            {
                Setting.MAP_ID = (long)table.Rows[0]["id"];
            }

            // 노드정보 setting
            nodeTableAdapter nodAdt = new nodeTableAdapter();
            Setting.nodTbl = nodAdt.GetNodeByMapID(Setting.MAP_ID);

            // 그룹 노드정보 setting
            Group_Node_JoinTableAdapter gnjAdt = new Group_Node_JoinTableAdapter();
            Setting.groupNodeJoinTbl = gnjAdt.GetGroupNode(Setting.MAP_ID, "", "");


        }
        #endregion


    }
}