namespace ImageProcessing.UI
{
    partial class MainWindow
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btn_RChannel = new System.Windows.Forms.Button();
            this.btn_BChannel = new System.Windows.Forms.Button();
            this.btn_GChannel = new System.Windows.Forms.Button();
            this.btn_RGBChannels = new System.Windows.Forms.Button();
            this.btn_Solarization = new System.Windows.Forms.Button();
            this.btn_Stamping = new System.Windows.Forms.Button();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorHistogram)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(719, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // colorHistogram
            // 
            chartArea1.Name = "ChartArea1";
            this.colorHistogram.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.colorHistogram.Legends.Add(legend1);
            this.colorHistogram.Location = new System.Drawing.Point(13, 28);
            this.colorHistogram.Name = "colorHistogram";
            this.colorHistogram.Size = new System.Drawing.Size(615, 392);
            this.colorHistogram.TabIndex = 1;
            this.colorHistogram.Text = "chart1";
            // 
            // btn_RChannel
            // 
            this.btn_RChannel.Location = new System.Drawing.Point(635, 28);
            this.btn_RChannel.Name = "btn_RChannel";
            this.btn_RChannel.Size = new System.Drawing.Size(75, 23);
            this.btn_RChannel.TabIndex = 2;
            this.btn_RChannel.Text = "R";
            this.btn_RChannel.UseVisualStyleBackColor = true;
            this.btn_RChannel.Click += new System.EventHandler(this.btn_RChannel_Click);
            // 
            // btn_BChannel
            // 
            this.btn_BChannel.Location = new System.Drawing.Point(635, 58);
            this.btn_BChannel.Name = "btn_BChannel";
            this.btn_BChannel.Size = new System.Drawing.Size(75, 23);
            this.btn_BChannel.TabIndex = 3;
            this.btn_BChannel.Text = "B";
            this.btn_BChannel.UseVisualStyleBackColor = true;
            this.btn_BChannel.Click += new System.EventHandler(this.btn_BChannel_Click);
            // 
            // btn_GChannel
            // 
            this.btn_GChannel.Location = new System.Drawing.Point(635, 88);
            this.btn_GChannel.Name = "btn_GChannel";
            this.btn_GChannel.Size = new System.Drawing.Size(75, 23);
            this.btn_GChannel.TabIndex = 4;
            this.btn_GChannel.Text = "G";
            this.btn_GChannel.UseVisualStyleBackColor = true;
            this.btn_GChannel.Click += new System.EventHandler(this.btn_GChannel_Click);
            // 
            // btn_RGBChannels
            // 
            this.btn_RGBChannels.Location = new System.Drawing.Point(635, 118);
            this.btn_RGBChannels.Name = "btn_RGBChannels";
            this.btn_RGBChannels.Size = new System.Drawing.Size(75, 23);
            this.btn_RGBChannels.TabIndex = 5;
            this.btn_RGBChannels.Text = "RGB";
            this.btn_RGBChannels.UseVisualStyleBackColor = true;
            this.btn_RGBChannels.Click += new System.EventHandler(this.btn_RGBChannels_Click);
            // 
            // btn_Solarization
            // 
            this.btn_Solarization.Location = new System.Drawing.Point(635, 148);
            this.btn_Solarization.Name = "btn_Solarization";
            this.btn_Solarization.Size = new System.Drawing.Size(75, 23);
            this.btn_Solarization.TabIndex = 6;
            this.btn_Solarization.Text = "Solarization";
            this.btn_Solarization.UseVisualStyleBackColor = true;
            this.btn_Solarization.Click += new System.EventHandler(this.btn_Solarization_Click);
            // 
            // btn_Stamping
            // 
            this.btn_Stamping.Location = new System.Drawing.Point(635, 178);
            this.btn_Stamping.Name = "btn_Stamping";
            this.btn_Stamping.Size = new System.Drawing.Size(75, 23);
            this.btn_Stamping.TabIndex = 7;
            this.btn_Stamping.Text = "Stamping";
            this.btn_Stamping.UseVisualStyleBackColor = true;
            this.btn_Stamping.Click += new System.EventHandler(this.btn_Stamping_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 435);
            this.Controls.Add(this.btn_Stamping);
            this.Controls.Add(this.btn_Solarization);
            this.Controls.Add(this.btn_RGBChannels);
            this.Controls.Add(this.btn_GChannel);
            this.Controls.Add(this.btn_BChannel);
            this.Controls.Add(this.btn_RChannel);
            this.Controls.Add(this.colorHistogram);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "MainWindow";
            this.Text = "MiSOI Lab01";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorHistogram)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart colorHistogram;
        private System.Windows.Forms.Button btn_RChannel;
        private System.Windows.Forms.Button btn_BChannel;
        private System.Windows.Forms.Button btn_GChannel;
        private System.Windows.Forms.Button btn_RGBChannels;
        private System.Windows.Forms.Button btn_Solarization;
        private System.Windows.Forms.Button btn_Stamping;
    }
}

