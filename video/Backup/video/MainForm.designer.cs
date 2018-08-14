namespace video
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.toolBarBtnGrab = new System.Windows.Forms.ToolBarButton();
            this.toolBarBtnSep = new System.Windows.Forms.ToolBarButton();
            this.toolBarBtnSave = new System.Windows.Forms.ToolBarButton();
            this.imgListToolBar = new System.Windows.Forms.ImageList(this.components);
            this.videoPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.stillPanel = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btngrab = new System.Windows.Forms.Button();
            this.btncrop = new System.Windows.Forms.Button();
            this.stillPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolBar
            // 
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarBtnGrab,
            this.toolBarBtnSep,
            this.toolBarBtnSave});
            this.toolBar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.imgListToolBar;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(844, 42);
            this.toolBar.TabIndex = 0;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            // 
            // toolBarBtnGrab
            // 
            this.toolBarBtnGrab.ImageIndex = 1;
            this.toolBarBtnGrab.Name = "toolBarBtnGrab";
            this.toolBarBtnGrab.Text = "Grab";
            this.toolBarBtnGrab.ToolTipText = "Grab picture from stream";
            // 
            // toolBarBtnSep
            // 
            this.toolBarBtnSep.Enabled = false;
            this.toolBarBtnSep.Name = "toolBarBtnSep";
            this.toolBarBtnSep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarBtnSave
            // 
            this.toolBarBtnSave.Enabled = false;
            this.toolBarBtnSave.ImageIndex = 2;
            this.toolBarBtnSave.Name = "toolBarBtnSave";
            this.toolBarBtnSave.Text = "Crop";
            this.toolBarBtnSave.ToolTipText = "Crop image ";
            // 
            // imgListToolBar
            // 
            this.imgListToolBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListToolBar.ImageStream")));
            this.imgListToolBar.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListToolBar.Images.SetKeyName(0, "");
            this.imgListToolBar.Images.SetKeyName(1, "");
            this.imgListToolBar.Images.SetKeyName(2, "");
            // 
            // videoPanel
            // 
            this.videoPanel.BackColor = System.Drawing.Color.Black;
            this.videoPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.videoPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.videoPanel.Location = new System.Drawing.Point(0, 42);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new System.Drawing.Size(342, 525);
            this.videoPanel.TabIndex = 1;
            this.videoPanel.Resize += new System.EventHandler(this.videoPanel_Resize);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(342, 42);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 525);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // stillPanel
            // 
            this.stillPanel.AutoScroll = true;
            this.stillPanel.AutoScrollMargin = new System.Drawing.Size(8, 8);
            this.stillPanel.AutoScrollMinSize = new System.Drawing.Size(32, 32);
            this.stillPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.stillPanel.Controls.Add(this.pictureBox);
            this.stillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stillPanel.Location = new System.Drawing.Point(347, 42);
            this.stillPanel.Name = "stillPanel";
            this.stillPanel.Size = new System.Drawing.Size(497, 525);
            this.stillPanel.TabIndex = 3;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(493, 521);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btngrab
            // 
            this.btngrab.Location = new System.Drawing.Point(99, 0);
            this.btngrab.Name = "btngrab";
            this.btngrab.Size = new System.Drawing.Size(75, 42);
            this.btngrab.TabIndex = 4;
            this.btngrab.Text = "Grab";
            this.btngrab.UseVisualStyleBackColor = true;
            this.btngrab.Visible = false;
            // 
            // btncrop
            // 
            this.btncrop.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btncrop.Location = new System.Drawing.Point(180, 0);
            this.btncrop.Name = "btncrop";
            this.btncrop.Size = new System.Drawing.Size(75, 42);
            this.btncrop.TabIndex = 5;
            this.btncrop.Text = "Crop";
            this.btncrop.UseVisualStyleBackColor = true;
            this.btncrop.Visible = false;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btncrop;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(844, 567);
            this.Controls.Add(this.btncrop);
            this.Controls.Add(this.btngrab);
            this.Controls.Add(this.stillPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.videoPanel);
            this.Controls.Add(this.toolBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cam Capture";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.stillPanel.ResumeLayout(false);
            this.stillPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imgListToolBar;
        private System.Windows.Forms.ToolBar toolBar;
        private System.Windows.Forms.ToolBarButton toolBarBtnGrab;
        private System.Windows.Forms.ToolBarButton toolBarBtnSep;
        private System.Windows.Forms.ToolBarButton toolBarBtnSave;
        private System.Windows.Forms.Panel videoPanel;
        private System.Windows.Forms.Panel stillPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btngrab;
        private System.Windows.Forms.Button btncrop;
        //private System.ComponentModel.IContainer components;
    }
}