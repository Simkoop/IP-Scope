
namespace IPScanner
{
    partial class UIDevice
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
            this.lbl_IP = new System.Windows.Forms.Label();
            this.pnl_Body = new System.Windows.Forms.Panel();
            this.lbl_Ping = new System.Windows.Forms.Label();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.lbl_MAC = new System.Windows.Forms.Label();
            this.tmr_Uninvert = new System.Windows.Forms.Timer(this.components);
            this.pnl_Body.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_IP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(118)))), ((int)(((byte)(215)))));
            this.lbl_IP.Location = new System.Drawing.Point(8, 7);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(66, 16);
            this.lbl_IP.TabIndex = 1;
            this.lbl_IP.Text = "IP Adress";
            this.lbl_IP.Click += new System.EventHandler(this.Clicked);
            this.lbl_IP.MouseEnter += new System.EventHandler(this.HoverEnter);
            this.lbl_IP.MouseLeave += new System.EventHandler(this.HoverLeave);
            // 
            // pnl_Body
            // 
            this.pnl_Body.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnl_Body.Controls.Add(this.lbl_Ping);
            this.pnl_Body.Controls.Add(this.lbl_Name);
            this.pnl_Body.Controls.Add(this.lbl_MAC);
            this.pnl_Body.Controls.Add(this.lbl_IP);
            this.pnl_Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Body.Location = new System.Drawing.Point(0, 0);
            this.pnl_Body.Name = "pnl_Body";
            this.pnl_Body.Padding = new System.Windows.Forms.Padding(5);
            this.pnl_Body.Size = new System.Drawing.Size(259, 58);
            this.pnl_Body.TabIndex = 2;
            this.pnl_Body.Click += new System.EventHandler(this.Clicked);
            this.pnl_Body.MouseEnter += new System.EventHandler(this.HoverEnter);
            this.pnl_Body.MouseLeave += new System.EventHandler(this.HoverLeave);
            // 
            // lbl_Ping
            // 
            this.lbl_Ping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Ping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.lbl_Ping.Location = new System.Drawing.Point(176, 37);
            this.lbl_Ping.Name = "lbl_Ping";
            this.lbl_Ping.Size = new System.Drawing.Size(75, 13);
            this.lbl_Ping.TabIndex = 4;
            this.lbl_Ping.Text = "ms";
            this.lbl_Ping.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_Ping.Click += new System.EventHandler(this.Clicked);
            this.lbl_Ping.MouseEnter += new System.EventHandler(this.HoverEnter);
            this.lbl_Ping.MouseLeave += new System.EventHandler(this.HoverLeave);
            // 
            // lbl_Name
            // 
            this.lbl_Name.AutoSize = true;
            this.lbl_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.lbl_Name.Location = new System.Drawing.Point(8, 37);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(35, 13);
            this.lbl_Name.TabIndex = 3;
            this.lbl_Name.Text = "Name";
            this.lbl_Name.Click += new System.EventHandler(this.Clicked);
            this.lbl_Name.MouseEnter += new System.EventHandler(this.HoverEnter);
            this.lbl_Name.MouseLeave += new System.EventHandler(this.HoverLeave);
            // 
            // lbl_MAC
            // 
            this.lbl_MAC.AutoSize = true;
            this.lbl_MAC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.lbl_MAC.Location = new System.Drawing.Point(8, 24);
            this.lbl_MAC.Name = "lbl_MAC";
            this.lbl_MAC.Size = new System.Drawing.Size(28, 13);
            this.lbl_MAC.TabIndex = 2;
            this.lbl_MAC.Text = "Mac";
            this.lbl_MAC.Click += new System.EventHandler(this.Clicked);
            this.lbl_MAC.MouseEnter += new System.EventHandler(this.HoverEnter);
            this.lbl_MAC.MouseLeave += new System.EventHandler(this.HoverLeave);
            // 
            // tmr_Uninvert
            // 
            this.tmr_Uninvert.Tick += new System.EventHandler(this.tmr_Uninvert_Tick);
            // 
            // UIDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(118)))), ((int)(((byte)(215)))));
            this.Controls.Add(this.pnl_Body);
            this.Name = "UIDevice";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.Size = new System.Drawing.Size(259, 60);
            this.MouseEnter += new System.EventHandler(this.HoverEnter);
            this.MouseLeave += new System.EventHandler(this.HoverLeave);
            this.pnl_Body.ResumeLayout(false);
            this.pnl_Body.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_IP;
        private System.Windows.Forms.Panel pnl_Body;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.Label lbl_MAC;
        private System.Windows.Forms.Label lbl_Ping;
        private System.Windows.Forms.Timer tmr_Uninvert;
    }
}
