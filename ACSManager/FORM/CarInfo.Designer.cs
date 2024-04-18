namespace ACSManager.FORM
{
    partial class CarInfo

    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarInfo));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.txtIP = new DevExpress.XtraEditors.TextEdit();
            this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.cmb_area = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtAGVID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbAGVType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbFloor = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmb_waiting = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_area.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAGVID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAGVType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFloor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_waiting.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txtPort);
            this.layoutControl1.Controls.Add(this.txtIP);
            this.layoutControl1.Controls.Add(this.btn_cancel);
            this.layoutControl1.Controls.Add(this.btn_save);
            this.layoutControl1.Controls.Add(this.cmb_area);
            this.layoutControl1.Controls.Add(this.txtAGVID);
            this.layoutControl1.Controls.Add(this.labelControl1);
            this.layoutControl1.Controls.Add(this.cmbAGVType);
            this.layoutControl1.Controls.Add(this.cmbFloor);
            this.layoutControl1.Controls.Add(this.cmb_waiting);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(463, 300);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtPort
            // 
            this.txtPort.EditValue = "0";
            this.txtPort.Location = new System.Drawing.Point(92, 217);
            this.txtPort.Name = "txtPort";
            this.txtPort.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtPort.Properties.Appearance.Options.UseFont = true;
            this.txtPort.Properties.Mask.EditMask = "d";
            this.txtPort.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPort.Size = new System.Drawing.Size(359, 26);
            this.txtPort.StyleController = this.layoutControl1;
            this.txtPort.TabIndex = 10;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(92, 187);
            this.txtIP.Name = "txtIP";
            this.txtIP.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtIP.Properties.Appearance.Options.UseFont = true;
            this.txtIP.Size = new System.Drawing.Size(359, 26);
            this.txtIP.StyleController = this.layoutControl1;
            this.txtIP.TabIndex = 9;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_cancel.Image")));
            this.btn_cancel.Location = new System.Drawing.Point(308, 247);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(143, 38);
            this.btn_cancel.StyleController = this.layoutControl1;
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_save
            // 
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.Location = new System.Drawing.Point(160, 247);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(144, 38);
            this.btn_save.StyleController = this.layoutControl1;
            this.btn_save.TabIndex = 7;
            this.btn_save.Text = "Save";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // cmb_area
            // 
            this.cmb_area.Location = new System.Drawing.Point(92, 127);
            this.cmb_area.Name = "cmb_area";
            this.cmb_area.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmb_area.Properties.Appearance.Options.UseFont = true;
            this.cmb_area.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmb_area.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmb_area.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_area.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_area.Size = new System.Drawing.Size(359, 26);
            this.cmb_area.StyleController = this.layoutControl1;
            this.cmb_area.TabIndex = 6;
            // 
            // txtAGVID
            // 
            this.txtAGVID.Location = new System.Drawing.Point(92, 37);
            this.txtAGVID.Name = "txtAGVID";
            this.txtAGVID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtAGVID.Properties.Appearance.Options.UseFont = true;
            this.txtAGVID.Size = new System.Drawing.Size(359, 26);
            this.txtAGVID.StyleController = this.layoutControl1;
            this.txtAGVID.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(145, 21);
            this.labelControl1.StyleController = this.layoutControl1;
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "AGV Information";
            // 
            // cmbAGVType
            // 
            this.cmbAGVType.Location = new System.Drawing.Point(92, 67);
            this.cmbAGVType.Name = "cmbAGVType";
            this.cmbAGVType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmbAGVType.Properties.Appearance.Options.UseFont = true;
            this.cmbAGVType.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmbAGVType.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbAGVType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAGVType.Properties.Items.AddRange(new object[] {
            "General"});
            this.cmbAGVType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbAGVType.Size = new System.Drawing.Size(359, 26);
            this.cmbAGVType.StyleController = this.layoutControl1;
            this.cmbAGVType.TabIndex = 11;
            // 
            // cmbFloor
            // 
            this.cmbFloor.Location = new System.Drawing.Point(92, 97);
            this.cmbFloor.Name = "cmbFloor";
            this.cmbFloor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmbFloor.Properties.Appearance.Options.UseFont = true;
            this.cmbFloor.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmbFloor.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmbFloor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFloor.Properties.DropDownRows = 4;
            this.cmbFloor.Properties.Items.AddRange(new object[] {
            "None",
            "1 Floor"});
            this.cmbFloor.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbFloor.Size = new System.Drawing.Size(359, 26);
            this.cmbFloor.StyleController = this.layoutControl1;
            this.cmbFloor.TabIndex = 13;
            this.cmbFloor.SelectedIndexChanged += new System.EventHandler(this.cmbFloor_SelectedIndexChanged);
            // 
            // cmb_waiting
            // 
            this.cmb_waiting.Location = new System.Drawing.Point(92, 157);
            this.cmb_waiting.Name = "cmb_waiting";
            this.cmb_waiting.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmb_waiting.Properties.Appearance.Options.UseFont = true;
            this.cmb_waiting.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cmb_waiting.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cmb_waiting.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_waiting.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb_waiting.Size = new System.Drawing.Size(359, 26);
            this.cmb_waiting.StyleController = this.layoutControl1;
            this.cmb_waiting.TabIndex = 6;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem4,
            this.layoutControlItem10,
            this.layoutControlItem9});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(463, 300);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.labelControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(443, 25);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 235);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(148, 45);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.txtAGVID;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 25);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(443, 30);
            this.layoutControlItem2.Text = "ID";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(77, 19);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.cmb_area;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 115);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(443, 30);
            this.layoutControlItem3.Text = "AGV 구분";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(77, 19);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn_cancel;
            this.layoutControlItem5.Location = new System.Drawing.Point(296, 235);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(147, 45);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem6.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem6.Control = this.txtIP;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 175);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(443, 30);
            this.layoutControlItem6.Text = "IP Address";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(77, 19);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem7.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem7.Control = this.txtPort;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 205);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(443, 30);
            this.layoutControlItem7.Text = "Port";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(77, 19);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem8.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem8.Control = this.cmbAGVType;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 55);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(443, 30);
            this.layoutControlItem8.Text = "Type";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(77, 19);
            this.layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btn_save;
            this.layoutControlItem4.Location = new System.Drawing.Point(148, 235);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(148, 45);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem10.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem10.Control = this.cmbFloor;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 85);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(443, 30);
            this.layoutControlItem10.Text = "Floor";
            this.layoutControlItem10.TextSize = new System.Drawing.Size(77, 19);
            this.layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlItem9.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem9.Control = this.cmb_waiting;
            this.layoutControlItem9.CustomizationFormText = "Move Area";
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 145);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(443, 30);
            this.layoutControlItem9.Text = "대기장소";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(77, 19);
            // 
            // CarInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(463, 300);
            this.ControlBox = false;
            this.Controls.Add(this.layoutControl1);
            this.Name = "CarInfo";
            this.Text = "AGV Information";
            this.Load += new System.EventHandler(this.CarInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_area.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAGVID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAGVType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFloor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_waiting.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_area;
        private DevExpress.XtraEditors.TextEdit txtAGVID;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit txtPort;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.ComboBoxEdit cmbAGVType;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.ComboBoxEdit cmbFloor;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_waiting;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
    }
}