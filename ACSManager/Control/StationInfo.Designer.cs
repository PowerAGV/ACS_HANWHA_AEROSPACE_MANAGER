namespace ACSManager.Control
{
    partial class StationInfo
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
            this.components = new System.ComponentModel.Container();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grpExample = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCreateStation = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlStation = new System.Windows.Forms.Panel();
            this.grpExample.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(828, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(56, 761);
            this.panel3.TabIndex = 2;
            // 
            // grpExample
            // 
            this.grpExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpExample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(219)))), ((int)(((byte)(233)))));
            this.grpExample.Controls.Add(this.label29);
            this.grpExample.Controls.Add(this.label28);
            this.grpExample.Controls.Add(this.panel2);
            this.grpExample.Controls.Add(this.panel1);
            this.grpExample.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpExample.Location = new System.Drawing.Point(1219, 796);
            this.grpExample.Name = "grpExample";
            this.grpExample.Size = new System.Drawing.Size(200, 100);
            this.grpExample.TabIndex = 30;
            this.grpExample.TabStop = false;
            this.grpExample.Text = "범례";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(123, 65);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(61, 23);
            this.label29.TabIndex = 3;
            this.label29.Text = "실대차";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(123, 33);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(61, 23);
            this.label28.TabIndex = 2;
            this.label28.Text = "공대차";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Orange;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(24, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(96, 26);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(24, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 26);
            this.panel1.TabIndex = 0;
            // 
            // btnCreateStation
            // 
            this.btnCreateStation.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateStation.Location = new System.Drawing.Point(36, 19);
            this.btnCreateStation.Name = "btnCreateStation";
            this.btnCreateStation.Size = new System.Drawing.Size(152, 32);
            this.btnCreateStation.TabIndex = 32;
            this.btnCreateStation.Text = "Station 추가";
            this.btnCreateStation.UseVisualStyleBackColor = true;
            this.btnCreateStation.Click += new System.EventHandler(this.btnCreateStation_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlStation
            // 
            this.pnlStation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlStation.Location = new System.Drawing.Point(36, 68);
            this.pnlStation.Name = "pnlStation";
            this.pnlStation.Size = new System.Drawing.Size(1383, 713);
            this.pnlStation.TabIndex = 33;
            // 
            // StationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlStation);
            this.Controls.Add(this.btnCreateStation);
            this.Controls.Add(this.grpExample);
            this.Name = "StationInfo";
            this.Size = new System.Drawing.Size(1428, 908);
            this.Load += new System.EventHandler(this.StationInfo_Load);
            this.grpExample.ResumeLayout(false);
            this.grpExample.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox grpExample;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCreateStation;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel pnlStation;
    }
}
