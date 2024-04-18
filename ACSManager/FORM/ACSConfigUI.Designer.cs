namespace ACSManager
{
    partial class ACSConfigUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ACSConfigUI));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txtTimeOut = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtAgvAliveCritical = new DevExpress.XtraEditors.TextEdit();
            this.txtNodeChangeInterval = new DevExpress.XtraEditors.TextEdit();
            this.txtAliveInterval = new DevExpress.XtraEditors.TextEdit();
            this.txtBatery = new DevExpress.XtraEditors.TextEdit();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAgvAliveCritical.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNodeChangeInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAliveInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatery.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.textEdit1);
            this.layoutControl1.Controls.Add(this.txtTimeOut);
            this.layoutControl1.Controls.Add(this.simpleButton1);
            this.layoutControl1.Controls.Add(this.txtAgvAliveCritical);
            this.layoutControl1.Controls.Add(this.txtNodeChangeInterval);
            this.layoutControl1.Controls.Add(this.txtAliveInterval);
            this.layoutControl1.Controls.Add(this.txtBatery);
            this.layoutControl1.Controls.Add(this.txtPort);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(466, 377);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.EditValue = "0";
            this.txtTimeOut.Location = new System.Drawing.Point(274, 50);
            this.txtTimeOut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtTimeOut.Properties.Appearance.Options.UseFont = true;
            this.txtTimeOut.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTimeOut.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtTimeOut.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTimeOut.Size = new System.Drawing.Size(176, 28);
            this.txtTimeOut.StyleController = this.layoutControl1;
            this.txtTimeOut.TabIndex = 10;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(16, 220);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(434, 40);
            this.simpleButton1.StyleController = this.layoutControl1;
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "ACS Engine Apply";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtAgvAliveCritical
            // 
            this.txtAgvAliveCritical.EditValue = "0";
            this.txtAgvAliveCritical.Location = new System.Drawing.Point(274, 186);
            this.txtAgvAliveCritical.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAgvAliveCritical.Name = "txtAgvAliveCritical";
            this.txtAgvAliveCritical.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtAgvAliveCritical.Properties.Appearance.Options.UseFont = true;
            this.txtAgvAliveCritical.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAgvAliveCritical.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtAgvAliveCritical.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAgvAliveCritical.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAgvAliveCritical.Size = new System.Drawing.Size(176, 28);
            this.txtAgvAliveCritical.StyleController = this.layoutControl1;
            this.txtAgvAliveCritical.TabIndex = 8;
            // 
            // txtNodeChangeInterval
            // 
            this.txtNodeChangeInterval.EditValue = "0";
            this.txtNodeChangeInterval.Location = new System.Drawing.Point(274, 152);
            this.txtNodeChangeInterval.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNodeChangeInterval.Name = "txtNodeChangeInterval";
            this.txtNodeChangeInterval.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtNodeChangeInterval.Properties.Appearance.Options.UseFont = true;
            this.txtNodeChangeInterval.Properties.Appearance.Options.UseTextOptions = true;
            this.txtNodeChangeInterval.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtNodeChangeInterval.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNodeChangeInterval.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtNodeChangeInterval.Size = new System.Drawing.Size(176, 28);
            this.txtNodeChangeInterval.StyleController = this.layoutControl1;
            this.txtNodeChangeInterval.TabIndex = 7;
            // 
            // txtAliveInterval
            // 
            this.txtAliveInterval.EditValue = "0";
            this.txtAliveInterval.Location = new System.Drawing.Point(274, 118);
            this.txtAliveInterval.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAliveInterval.Name = "txtAliveInterval";
            this.txtAliveInterval.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtAliveInterval.Properties.Appearance.Options.UseFont = true;
            this.txtAliveInterval.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAliveInterval.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtAliveInterval.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAliveInterval.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAliveInterval.Size = new System.Drawing.Size(176, 28);
            this.txtAliveInterval.StyleController = this.layoutControl1;
            this.txtAliveInterval.TabIndex = 6;
            // 
            // txtBatery
            // 
            this.txtBatery.EditValue = "0";
            this.txtBatery.Location = new System.Drawing.Point(274, 84);
            this.txtBatery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatery.Name = "txtBatery";
            this.txtBatery.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtBatery.Properties.Appearance.Options.UseFont = true;
            this.txtBatery.Properties.Appearance.Options.UseTextOptions = true;
            this.txtBatery.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtBatery.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtBatery.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtBatery.Size = new System.Drawing.Size(176, 28);
            this.txtBatery.StyleController = this.layoutControl1;
            this.txtBatery.TabIndex = 5;
            // 
            // txtPort
            // 
            this.txtPort.EditValue = "0";
            this.txtPort.Location = new System.Drawing.Point(274, 16);
            this.txtPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtPort.Properties.Appearance.Options.UseFont = true;
            this.txtPort.Properties.Appearance.Options.UseTextOptions = true;
            this.txtPort.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtPort.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtPort.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPort.Size = new System.Drawing.Size(176, 28);
            this.txtPort.StyleController = this.layoutControl1;
            this.txtPort.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(466, 377);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 11F);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.txtPort;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(440, 34);
            this.layoutControlItem1.Text = "Event Linten Port";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(255, 22);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 11F);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.txtBatery;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 68);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(440, 34);
            this.layoutControlItem2.Text = "Batery limite(%)";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(255, 22);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 11F);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.txtAliveInterval;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 102);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(440, 34);
            this.layoutControlItem3.Text = "Avilable AGV Count";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(255, 22);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 11F);
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.Control = this.txtNodeChangeInterval;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 136);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(440, 34);
            this.layoutControlItem4.Text = "Node change check interval(sec)";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(255, 22);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 11F);
            this.layoutControlItem5.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem5.Control = this.txtAgvAliveCritical;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 170);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(440, 34);
            this.layoutControlItem5.Text = "AGV Alive critical(sec) ";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(255, 22);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem6.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem6.Control = this.simpleButton1;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 204);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(440, 46);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 11F);
            this.layoutControlItem7.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem7.Control = this.txtTimeOut;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 34);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(440, 34);
            this.layoutControlItem7.Text = "Comm TimeOut(sec)";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(255, 22);
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "0";
            this.textEdit1.Location = new System.Drawing.Point(274, 266);
            this.textEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textEdit1.Properties.Appearance.Options.UseFont = true;
            this.textEdit1.Properties.Appearance.Options.UseTextOptions = true;
            this.textEdit1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.textEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.textEdit1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.textEdit1.Size = new System.Drawing.Size(176, 28);
            this.textEdit1.StyleController = this.layoutControl1;
            this.textEdit1.TabIndex = 11;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.textEdit1;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 250);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(440, 101);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(255, 18);
            // 
            // ACSConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ACSConfigUI";
            this.Size = new System.Drawing.Size(466, 377);
            this.Load += new System.EventHandler(this.ACSConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAgvAliveCritical.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNodeChangeInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAliveInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBatery.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit txtAgvAliveCritical;
        private DevExpress.XtraEditors.TextEdit txtNodeChangeInterval;
        private DevExpress.XtraEditors.TextEdit txtAliveInterval;
        private DevExpress.XtraEditors.TextEdit txtBatery;
        private DevExpress.XtraEditors.TextEdit txtPort;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.TextEdit txtTimeOut;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
    }
}
