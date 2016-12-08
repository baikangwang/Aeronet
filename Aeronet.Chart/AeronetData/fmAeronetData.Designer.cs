﻿namespace Aeronet.Chart
{
    partial class fmAeronetData
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmAeronetData));
            this.btAction = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvDirs = new System.Windows.Forms.TreeView();
            this.fileBrowser1 = new Aeronet.Chart.AeronetData.FileViewer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lblDescription = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btAction
            // 
            this.btAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAction.Location = new System.Drawing.Point(472, 560);
            this.btAction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAction.Name = "btAction";
            this.btAction.Size = new System.Drawing.Size(87, 30);
            this.btAction.TabIndex = 2;
            this.btAction.Text = global::Aeronet.Chart.Properties.Settings.Default.BTN_PROCESS_TEXT;
            this.btAction.UseVisualStyleBackColor = true;
            this.btAction.Click += new System.EventHandler(this.btAction_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvDirs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fileBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(640, 452);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // tvDirs
            // 
            this.tvDirs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDirs.ImageIndex = 0;
            this.tvDirs.ImageList = this.imageList1;
            this.tvDirs.Location = new System.Drawing.Point(0, 0);
            this.tvDirs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tvDirs.Name = "tvDirs";
            this.tvDirs.SelectedImageIndex = 0;
            this.tvDirs.Size = new System.Drawing.Size(257, 452);
            this.tvDirs.TabIndex = 0;
            // 
            // fileBrowser1
            // 
            this.fileBrowser1.CurrentDirectory = "";
            this.fileBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileBrowser1.Location = new System.Drawing.Point(0, 0);
            this.fileBrowser1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fileBrowser1.Name = "fileBrowser1";
            this.fileBrowser1.Size = new System.Drawing.Size(378, 452);
            this.fileBrowser1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(14, 37);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblDescription);
            this.splitContainer2.Size = new System.Drawing.Size(640, 509);
            this.splitContainer2.SplitterDistance = 452;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescription.Location = new System.Drawing.Point(0, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(640, 52);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "lblDescription";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImport,
            this.btnRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(668, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnImport
            // 
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(57, 22);
            this.btnImport.Text = "导入(&I)...";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(52, 22);
            this.btnRefresh.Text = global::Aeronet.Chart.Properties.Settings.Default.BTN_REFRESH_TEXT;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(567, 560);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = global::Aeronet.Chart.Properties.Settings.Default.BTN_CLOSE_TEXT;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // fmAeronetData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 605);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btAction);
            this.Font = global::Aeronet.Chart.Properties.Settings.Default.DEFAULT_FONT;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fmAeronetData";
            this.Text = global::Aeronet.Chart.Properties.Settings.Default.FM_AERONET_DATA_TEXT;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btAction;
        private System.Windows.Forms.TreeView tvDirs;
        private AeronetData.FileViewer fileBrowser1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnImport;
        private System.Windows.Forms.ToolStripButton btnRefresh;
    }
}