namespace ACSManager.Control
{
    partial class PLCView
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PLCView));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.vGridControl1 = new DevExpress.XtraVerticalGrid.VGridControl();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.TAB_PANEL = new ACSManager.Control.TabPane();
            this.PAGE_GUI = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.DIAGRAM_GUI = new DevExpress.XtraDiagram.DiagramControl();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TAB_PANEL)).BeginInit();
            this.TAB_PANEL.SuspendLayout();
            this.PAGE_GUI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DIAGRAM_GUI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // vGridControl1
            // 
            resources.ApplyResources(this.vGridControl1, "vGridControl1");
            this.vGridControl1.Name = "vGridControl1";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.vGridControl1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(782, 638);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(50, 20);
            // 
            // TAB_PANEL
            // 
            this.TAB_PANEL.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("TAB_PANEL.Appearance.BackColor")));
            this.TAB_PANEL.Appearance.Options.UseBackColor = true;
            this.TAB_PANEL.Appearance.Options.UseTextOptions = true;
            this.TAB_PANEL.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TAB_PANEL.AppearanceButton.Hovered.Font = ((System.Drawing.Font)(resources.GetObject("TAB_PANEL.AppearanceButton.Hovered.Font")));
            this.TAB_PANEL.AppearanceButton.Hovered.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("TAB_PANEL.AppearanceButton.Hovered.FontStyleDelta")));
            this.TAB_PANEL.AppearanceButton.Hovered.Options.UseFont = true;
            this.TAB_PANEL.AppearanceButton.Hovered.Options.UseTextOptions = true;
            this.TAB_PANEL.AppearanceButton.Hovered.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TAB_PANEL.AppearanceButton.Hovered.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.TAB_PANEL.AppearanceButton.Normal.Font = ((System.Drawing.Font)(resources.GetObject("TAB_PANEL.AppearanceButton.Normal.Font")));
            this.TAB_PANEL.AppearanceButton.Normal.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("TAB_PANEL.AppearanceButton.Normal.FontStyleDelta")));
            this.TAB_PANEL.AppearanceButton.Normal.Options.UseFont = true;
            this.TAB_PANEL.AppearanceButton.Normal.Options.UseTextOptions = true;
            this.TAB_PANEL.AppearanceButton.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TAB_PANEL.AppearanceButton.Pressed.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("TAB_PANEL.AppearanceButton.Pressed.FontStyleDelta")));
            this.TAB_PANEL.AppearanceButton.Pressed.Options.UseFont = true;
            this.TAB_PANEL.AppearanceButton.Pressed.Options.UseTextOptions = true;
            this.TAB_PANEL.AppearanceButton.Pressed.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.TAB_PANEL, "TAB_PANEL");
            this.TAB_PANEL.Controls.Add(this.PAGE_GUI);
            this.TAB_PANEL.Controls.Add(this.tabNavigationPage2);
            this.TAB_PANEL.LookAndFeel.SkinName = "London Liquid Sky";
            this.TAB_PANEL.Name = "TAB_PANEL";
            this.TAB_PANEL.PageProperties.AppearanceCaption.Font = ((System.Drawing.Font)(resources.GetObject("TAB_PANEL.PageProperties.AppearanceCaption.Font")));
            this.TAB_PANEL.PageProperties.AppearanceCaption.Options.UseFont = true;
            this.TAB_PANEL.PageProperties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.Text;
            this.TAB_PANEL.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.PAGE_GUI});
            this.TAB_PANEL.RegularSize = new System.Drawing.Size(1603, 847);
            this.TAB_PANEL.SelectedPage = this.PAGE_GUI;
            // 
            // PAGE_GUI
            // 
            this.PAGE_GUI.AllowTouchScroll = true;
            this.PAGE_GUI.Appearance.BackColor = ((System.Drawing.Color)(resources.GetObject("PAGE_GUI.Appearance.BackColor")));
            this.PAGE_GUI.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("PAGE_GUI.Appearance.Font")));
            this.PAGE_GUI.Appearance.Options.UseBackColor = true;
            this.PAGE_GUI.Appearance.Options.UseFont = true;
            this.PAGE_GUI.Caption = "AGV 화면";
            this.PAGE_GUI.Controls.Add(this.layoutControl2);
            resources.ApplyResources(this.PAGE_GUI, "PAGE_GUI");
            this.PAGE_GUI.Name = "PAGE_GUI";
            this.PAGE_GUI.Properties.AppearanceCaption.Font = ((System.Drawing.Font)(resources.GetObject("PAGE_GUI.Properties.AppearanceCaption.Font")));
            this.PAGE_GUI.Properties.AppearanceCaption.FontStyleDelta = ((System.Drawing.FontStyle)(resources.GetObject("PAGE_GUI.Properties.AppearanceCaption.FontStyleDelta")));
            this.PAGE_GUI.Properties.AppearanceCaption.Options.UseFont = true;
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.DIAGRAM_GUI);
            resources.ApplyResources(this.layoutControl2, "layoutControl2");
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(-1262, 309, 450, 400);
            this.layoutControl2.Root = this.layoutControlGroup2;
            // 
            // DIAGRAM_GUI
            // 
            this.DIAGRAM_GUI.AllowDrop = true;
            resources.ApplyResources(this.DIAGRAM_GUI, "DIAGRAM_GUI");
            this.DIAGRAM_GUI.Name = "DIAGRAM_GUI";
            this.DIAGRAM_GUI.OptionsBehavior.SelectedStencils = new DevExpress.Diagram.Core.StencilCollection(new string[] {
            "BasicShapes",
            "BasicFlowchartShapes",
            "ArrowShapes"});
            this.DIAGRAM_GUI.OptionsView.CanvasSizeMode = DevExpress.Diagram.Core.CanvasSizeMode.Fill;
            this.DIAGRAM_GUI.OptionsView.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.DIAGRAM_GUI.OptionsView.ShowGrid = false;
            this.DIAGRAM_GUI.OptionsView.ShowPageBreaks = false;
            this.DIAGRAM_GUI.OptionsView.ShowRulers = false;
            this.DIAGRAM_GUI.OptionsView.Theme = DevExpress.Diagram.Core.DiagramThemes.NoTheme;
            this.DIAGRAM_GUI.StyleController = this.layoutControl2;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.BackColor = ((System.Drawing.Color)(resources.GetObject("layoutControlGroup2.AppearanceGroup.BackColor")));
            this.layoutControlGroup2.AppearanceGroup.Options.UseBackColor = true;
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "Root";
            this.layoutControlGroup2.Size = new System.Drawing.Size(1585, 781);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.DIAGRAM_GUI;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(54, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(1565, 761);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // tabNavigationPage2
            // 
            this.tabNavigationPage2.Caption = "tabNavigationPage2";
            this.tabNavigationPage2.Name = "tabNavigationPage2";
            resources.ApplyResources(this.tabNavigationPage2, "tabNavigationPage2");
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            resources.ApplyResources(this.ribbonPage2, "ribbonPage2");
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            resources.ApplyResources(this.ribbonPageGroup2, "ribbonPageGroup2");
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1627, 871);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.TAB_PANEL;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(212, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1607, 851);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.TAB_PANEL);
            resources.ApplyResources(this.layoutControl1, "layoutControl1");
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            // 
            // PLCView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "PLCView";
            this.Load += new System.EventHandler(this.PLCView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TAB_PANEL)).EndInit();
            this.TAB_PANEL.ResumeLayout(false);
            this.PAGE_GUI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DIAGRAM_GUI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraVerticalGrid.VGridControl vGridControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private TabPane TAB_PANEL;
        private DevExpress.XtraBars.Navigation.TabNavigationPage PAGE_GUI;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        public DevExpress.XtraDiagram.DiagramControl DIAGRAM_GUI;
    }
}
