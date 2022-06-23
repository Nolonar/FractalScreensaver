namespace FractalScreenSaver
{
	partial class SettingsForm
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
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.groupGeneral = new System.Windows.Forms.GroupBox();
            this.cbRandomCount = new System.Windows.Forms.CheckBox();
            this.lblIterations = new System.Windows.Forms.Label();
            this.numIterations = new System.Windows.Forms.NumericUpDown();
            this.lblCount = new System.Windows.Forms.Label();
            this.numEdgeCount = new System.Windows.Forms.NumericUpDown();
            this.lblType = new System.Windows.Forms.Label();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.groupAppearance = new System.Windows.Forms.GroupBox();
            this.numMaxBump = new System.Windows.Forms.NumericUpDown();
            this.lblMaxBump = new System.Windows.Forms.Label();
            this.numMinBump = new System.Windows.Forms.NumericUpDown();
            this.lblMinBump = new System.Windows.Forms.Label();
            this.cbKeepInViewport = new System.Windows.Forms.CheckBox();
            this.cbRainbow = new System.Windows.Forms.CheckBox();
            this.groupTiming = new System.Windows.Forms.GroupBox();
            this.numFractalDelay = new System.Windows.Forms.NumericUpDown();
            this.lblFractalDelay = new System.Windows.Forms.Label();
            this.numIterationDelay = new System.Windows.Forms.NumericUpDown();
            this.lblIterationDelay = new System.Windows.Forms.Label();
            this.groupOthers = new System.Windows.Forms.GroupBox();
            this.btnBrowseSaveDir = new System.Windows.Forms.Button();
            this.tbSaveDir = new System.Windows.Forms.TextBox();
            this.cbSave = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel.SuspendLayout();
            this.groupGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeCount)).BeginInit();
            this.groupAppearance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBump)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinBump)).BeginInit();
            this.groupTiming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFractalDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIterationDelay)).BeginInit();
            this.groupOthers.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Controls.Add(this.groupGeneral);
            this.flowLayoutPanel.Controls.Add(this.groupAppearance);
            this.flowLayoutPanel.Controls.Add(this.groupTiming);
            this.flowLayoutPanel.Controls.Add(this.groupOthers);
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(382, 598);
            this.flowLayoutPanel.TabIndex = 0;
            this.flowLayoutPanel.WrapContents = false;
            // 
            // groupGeneral
            // 
            this.groupGeneral.Controls.Add(this.cbRandomCount);
            this.groupGeneral.Controls.Add(this.lblIterations);
            this.groupGeneral.Controls.Add(this.numIterations);
            this.groupGeneral.Controls.Add(this.lblCount);
            this.groupGeneral.Controls.Add(this.numEdgeCount);
            this.groupGeneral.Controls.Add(this.lblType);
            this.groupGeneral.Controls.Add(this.comboType);
            this.groupGeneral.Location = new System.Drawing.Point(2, 2);
            this.groupGeneral.Margin = new System.Windows.Forms.Padding(2);
            this.groupGeneral.Name = "groupGeneral";
            this.groupGeneral.Padding = new System.Windows.Forms.Padding(2);
            this.groupGeneral.Size = new System.Drawing.Size(378, 152);
            this.groupGeneral.TabIndex = 2;
            this.groupGeneral.TabStop = false;
            this.groupGeneral.Text = "General";
            // 
            // cbRandomCount
            // 
            this.cbRandomCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRandomCount.AutoSize = true;
            this.cbRandomCount.Location = new System.Drawing.Point(263, 71);
            this.cbRandomCount.Margin = new System.Windows.Forms.Padding(2);
            this.cbRandomCount.Name = "cbRandomCount";
            this.cbRandomCount.Size = new System.Drawing.Size(106, 29);
            this.cbRandomCount.TabIndex = 4;
            this.cbRandomCount.Text = "Random";
            this.cbRandomCount.UseVisualStyleBackColor = true;
            this.cbRandomCount.CheckedChanged += new System.EventHandler(this.CbRandomCount_CheckedChanged);
            // 
            // lblIterations
            // 
            this.lblIterations.AutoSize = true;
            this.lblIterations.Location = new System.Drawing.Point(8, 109);
            this.lblIterations.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIterations.Name = "lblIterations";
            this.lblIterations.Size = new System.Drawing.Size(86, 25);
            this.lblIterations.TabIndex = 5;
            this.lblIterations.Text = "Iterations";
            // 
            // numIterations
            // 
            this.numIterations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numIterations.Location = new System.Drawing.Point(96, 108);
            this.numIterations.Margin = new System.Windows.Forms.Padding(2);
            this.numIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIterations.Name = "numIterations";
            this.numIterations.Size = new System.Drawing.Size(273, 31);
            this.numIterations.TabIndex = 6;
            this.numIterations.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(8, 72);
            this.lblCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(60, 25);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "Edges";
            // 
            // numEdgeCount
            // 
            this.numEdgeCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numEdgeCount.Location = new System.Drawing.Point(96, 70);
            this.numEdgeCount.Margin = new System.Windows.Forms.Padding(2);
            this.numEdgeCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEdgeCount.Name = "numEdgeCount";
            this.numEdgeCount.Size = new System.Drawing.Size(164, 31);
            this.numEdgeCount.TabIndex = 3;
            this.numEdgeCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(8, 32);
            this.lblType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(49, 25);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type";
            // 
            // comboType
            // 
            this.comboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.ItemHeight = 25;
            this.comboType.Location = new System.Drawing.Point(96, 30);
            this.comboType.Margin = new System.Windows.Forms.Padding(2);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(274, 33);
            this.comboType.TabIndex = 1;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.ComboType_SelectedIndexChanged);
            // 
            // groupAppearance
            // 
            this.groupAppearance.Controls.Add(this.numMaxBump);
            this.groupAppearance.Controls.Add(this.lblMaxBump);
            this.groupAppearance.Controls.Add(this.numMinBump);
            this.groupAppearance.Controls.Add(this.lblMinBump);
            this.groupAppearance.Controls.Add(this.cbKeepInViewport);
            this.groupAppearance.Controls.Add(this.cbRainbow);
            this.groupAppearance.Location = new System.Drawing.Point(2, 158);
            this.groupAppearance.Margin = new System.Windows.Forms.Padding(2);
            this.groupAppearance.Name = "groupAppearance";
            this.groupAppearance.Padding = new System.Windows.Forms.Padding(2);
            this.groupAppearance.Size = new System.Drawing.Size(378, 189);
            this.groupAppearance.TabIndex = 3;
            this.groupAppearance.TabStop = false;
            this.groupAppearance.Text = "Appearance";
            // 
            // numMaxBump
            // 
            this.numMaxBump.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numMaxBump.Location = new System.Drawing.Point(202, 142);
            this.numMaxBump.Margin = new System.Windows.Forms.Padding(2);
            this.numMaxBump.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numMaxBump.Name = "numMaxBump";
            this.numMaxBump.Size = new System.Drawing.Size(167, 31);
            this.numMaxBump.TabIndex = 6;
            this.numMaxBump.ValueChanged += new System.EventHandler(this.NumMaxBump_ValueChanged);
            // 
            // lblMaxBump
            // 
            this.lblMaxBump.AutoSize = true;
            this.lblMaxBump.Location = new System.Drawing.Point(8, 145);
            this.lblMaxBump.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMaxBump.Name = "lblMaxBump";
            this.lblMaxBump.Size = new System.Drawing.Size(199, 25);
            this.lblMaxBump.TabIndex = 5;
            this.lblMaxBump.Text = "Maximum bump length";
            // 
            // numMinBump
            // 
            this.numMinBump.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numMinBump.Location = new System.Drawing.Point(202, 106);
            this.numMinBump.Margin = new System.Windows.Forms.Padding(2);
            this.numMinBump.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numMinBump.Name = "numMinBump";
            this.numMinBump.Size = new System.Drawing.Size(167, 31);
            this.numMinBump.TabIndex = 4;
            this.numMinBump.ValueChanged += new System.EventHandler(this.NumMinBump_ValueChanged);
            // 
            // lblMinBump
            // 
            this.lblMinBump.AutoSize = true;
            this.lblMinBump.Location = new System.Drawing.Point(8, 108);
            this.lblMinBump.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMinBump.Name = "lblMinBump";
            this.lblMinBump.Size = new System.Drawing.Size(196, 25);
            this.lblMinBump.TabIndex = 3;
            this.lblMinBump.Text = "Minimum bump length";
            // 
            // cbKeepInViewport
            // 
            this.cbKeepInViewport.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbKeepInViewport.AutoSize = true;
            this.cbKeepInViewport.Location = new System.Drawing.Point(102, 30);
            this.cbKeepInViewport.Margin = new System.Windows.Forms.Padding(2);
            this.cbKeepInViewport.Name = "cbKeepInViewport";
            this.cbKeepInViewport.Size = new System.Drawing.Size(156, 35);
            this.cbKeepInViewport.TabIndex = 1;
            this.cbKeepInViewport.Text = "Keep in Viewport";
            this.cbKeepInViewport.UseVisualStyleBackColor = true;
            // 
            // cbRainbow
            // 
            this.cbRainbow.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbRainbow.AutoSize = true;
            this.cbRainbow.Location = new System.Drawing.Point(9, 30);
            this.cbRainbow.Margin = new System.Windows.Forms.Padding(2);
            this.cbRainbow.Name = "cbRainbow";
            this.cbRainbow.Size = new System.Drawing.Size(91, 35);
            this.cbRainbow.TabIndex = 0;
            this.cbRainbow.Text = "Rainbow";
            this.cbRainbow.UseVisualStyleBackColor = true;
            // 
            // groupTiming
            // 
            this.groupTiming.Controls.Add(this.numFractalDelay);
            this.groupTiming.Controls.Add(this.lblFractalDelay);
            this.groupTiming.Controls.Add(this.numIterationDelay);
            this.groupTiming.Controls.Add(this.lblIterationDelay);
            this.groupTiming.Location = new System.Drawing.Point(2, 351);
            this.groupTiming.Margin = new System.Windows.Forms.Padding(2);
            this.groupTiming.Name = "groupTiming";
            this.groupTiming.Padding = new System.Windows.Forms.Padding(2);
            this.groupTiming.Size = new System.Drawing.Size(378, 112);
            this.groupTiming.TabIndex = 4;
            this.groupTiming.TabStop = false;
            this.groupTiming.Text = "Timing";
            // 
            // numFractalDelay
            // 
            this.numFractalDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numFractalDelay.Location = new System.Drawing.Point(162, 68);
            this.numFractalDelay.Margin = new System.Windows.Forms.Padding(2);
            this.numFractalDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numFractalDelay.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numFractalDelay.Name = "numFractalDelay";
            this.numFractalDelay.Size = new System.Drawing.Size(208, 31);
            this.numFractalDelay.TabIndex = 3;
            this.numFractalDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lblFractalDelay
            // 
            this.lblFractalDelay.AutoSize = true;
            this.lblFractalDelay.Location = new System.Drawing.Point(8, 69);
            this.lblFractalDelay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFractalDelay.Name = "lblFractalDelay";
            this.lblFractalDelay.Size = new System.Drawing.Size(148, 25);
            this.lblFractalDelay.TabIndex = 2;
            this.lblFractalDelay.Text = "Next fractal delay";
            // 
            // numIterationDelay
            // 
            this.numIterationDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numIterationDelay.Location = new System.Drawing.Point(162, 30);
            this.numIterationDelay.Margin = new System.Windows.Forms.Padding(2);
            this.numIterationDelay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numIterationDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIterationDelay.Name = "numIterationDelay";
            this.numIterationDelay.Size = new System.Drawing.Size(208, 31);
            this.numIterationDelay.TabIndex = 1;
            this.numIterationDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblIterationDelay
            // 
            this.lblIterationDelay.AutoSize = true;
            this.lblIterationDelay.Location = new System.Drawing.Point(8, 32);
            this.lblIterationDelay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIterationDelay.Name = "lblIterationDelay";
            this.lblIterationDelay.Size = new System.Drawing.Size(125, 25);
            this.lblIterationDelay.TabIndex = 0;
            this.lblIterationDelay.Text = "Iteration delay";
            // 
            // groupOthers
            // 
            this.groupOthers.Controls.Add(this.btnBrowseSaveDir);
            this.groupOthers.Controls.Add(this.tbSaveDir);
            this.groupOthers.Controls.Add(this.cbSave);
            this.groupOthers.Location = new System.Drawing.Point(2, 467);
            this.groupOthers.Margin = new System.Windows.Forms.Padding(2);
            this.groupOthers.Name = "groupOthers";
            this.groupOthers.Padding = new System.Windows.Forms.Padding(2);
            this.groupOthers.Size = new System.Drawing.Size(378, 119);
            this.groupOthers.TabIndex = 5;
            this.groupOthers.TabStop = false;
            this.groupOthers.Text = "Others";
            // 
            // btnBrowseSaveDir
            // 
            this.btnBrowseSaveDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseSaveDir.Location = new System.Drawing.Point(286, 59);
            this.btnBrowseSaveDir.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowseSaveDir.Name = "btnBrowseSaveDir";
            this.btnBrowseSaveDir.Size = new System.Drawing.Size(83, 45);
            this.btnBrowseSaveDir.TabIndex = 2;
            this.btnBrowseSaveDir.Text = "Browse";
            this.btnBrowseSaveDir.UseVisualStyleBackColor = true;
            this.btnBrowseSaveDir.Click += new System.EventHandler(this.BtnBrowseSaveDir_Click);
            // 
            // tbSaveDir
            // 
            this.tbSaveDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSaveDir.Location = new System.Drawing.Point(9, 66);
            this.tbSaveDir.Margin = new System.Windows.Forms.Padding(2);
            this.tbSaveDir.Name = "tbSaveDir";
            this.tbSaveDir.ReadOnly = true;
            this.tbSaveDir.Size = new System.Drawing.Size(273, 31);
            this.tbSaveDir.TabIndex = 1;
            // 
            // cbSave
            // 
            this.cbSave.AutoSize = true;
            this.cbSave.Location = new System.Drawing.Point(9, 31);
            this.cbSave.Margin = new System.Windows.Forms.Padding(2);
            this.cbSave.Name = "cbSave";
            this.cbSave.Size = new System.Drawing.Size(136, 29);
            this.cbSave.TabIndex = 0;
            this.cbSave.Text = "Save fractals";
            this.cbSave.UseVisualStyleBackColor = true;
            this.cbSave.CheckedChanged += new System.EventHandler(this.CbSave_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(112, 604);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 45);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(289, 604);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(83, 45);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(201, 604);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 45);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(386, 676);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.flowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(408, 732);
            this.MinimumSize = new System.Drawing.Size(408, 732);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.flowLayoutPanel.ResumeLayout(false);
            this.groupGeneral.ResumeLayout(false);
            this.groupGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeCount)).EndInit();
            this.groupAppearance.ResumeLayout(false);
            this.groupAppearance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxBump)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinBump)).EndInit();
            this.groupTiming.ResumeLayout(false);
            this.groupTiming.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFractalDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIterationDelay)).EndInit();
            this.groupOthers.ResumeLayout(false);
            this.groupOthers.PerformLayout();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.GroupBox groupGeneral;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.NumericUpDown numEdgeCount;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblIterations;
        private System.Windows.Forms.NumericUpDown numIterations;
        private System.Windows.Forms.GroupBox groupAppearance;
        private System.Windows.Forms.CheckBox cbRainbow;
        private System.Windows.Forms.CheckBox cbKeepInViewport;
        private System.Windows.Forms.NumericUpDown numMaxBump;
        private System.Windows.Forms.Label lblMaxBump;
        private System.Windows.Forms.NumericUpDown numMinBump;
        private System.Windows.Forms.Label lblMinBump;
        private System.Windows.Forms.GroupBox groupTiming;
        private System.Windows.Forms.NumericUpDown numIterationDelay;
        private System.Windows.Forms.Label lblIterationDelay;
        private System.Windows.Forms.NumericUpDown numFractalDelay;
        private System.Windows.Forms.Label lblFractalDelay;
        private System.Windows.Forms.GroupBox groupOthers;
        private System.Windows.Forms.CheckBox cbSave;
        private System.Windows.Forms.Button btnBrowseSaveDir;
        private System.Windows.Forms.TextBox tbSaveDir;
        private System.Windows.Forms.CheckBox cbRandomCount;
    }
}