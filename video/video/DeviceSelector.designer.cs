namespace video
{
    partial class DeviceSelector
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
            this.deviceListVw = new System.Windows.Forms.ListView();
            this.nameColHd = new System.Windows.Forms.ColumnHeader();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deviceListVw
            // 
            this.deviceListVw.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColHd});
            this.deviceListVw.FullRowSelect = true;
            this.deviceListVw.GridLines = true;
            this.deviceListVw.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.deviceListVw.HideSelection = false;
            this.deviceListVw.Location = new System.Drawing.Point(5, 12);
            this.deviceListVw.MultiSelect = false;
            this.deviceListVw.Name = "deviceListVw";
            this.deviceListVw.Size = new System.Drawing.Size(351, 112);
            this.deviceListVw.TabIndex = 1;
            this.deviceListVw.UseCompatibleStateImageBehavior = false;
            this.deviceListVw.View = System.Windows.Forms.View.Details;
            this.deviceListVw.DoubleClick += new System.EventHandler(this.deviceListVw_DoubleClick);
            // 
            // nameColHd
            // 
            this.nameColHd.Text = "Name";
            this.nameColHd.Width = 400;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(71, 130);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(71, 24);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(177, 130);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(71, 24);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // DeviceSelector
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(365, 157);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.deviceListVw);
            this.Name = "DeviceSelector";
            this.Text = "Select video capture device";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView deviceListVw;
        private System.Windows.Forms.ColumnHeader nameColHd;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}