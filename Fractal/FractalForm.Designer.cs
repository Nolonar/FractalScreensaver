namespace FractalScreenSaver
{
	partial class FractalForm
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.iterationTimer = new System.Windows.Forms.Timer(this.components);
            this.nextFractalTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // iterationTimer
            // 
            this.iterationTimer.Tick += new System.EventHandler(this.iterationTimer_Tick);
            // 
            // nextFractalTimer
            // 
            this.nextFractalTimer.Tick += new System.EventHandler(this.nextFractalTimer_Tick);
            // 
            // FractalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(2560, 1385);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimizeBox = false;
            this.Name = "FractalForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fractal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FractalForm_FormClosed);
            this.Load += new System.EventHandler(this.Fractal_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Fractal_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Fractal_MouseActivity);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Fractal_MouseActivity);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer iterationTimer;
        private System.Windows.Forms.Timer nextFractalTimer;
    }
}

