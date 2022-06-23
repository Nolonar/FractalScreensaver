using System;
using System.Windows.Forms;

using FractalScreenSaver.Fractals;

namespace FractalScreenSaver
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            comboType.DataSource = Enum.GetValues(typeof(IFractal.Type));
        }

        private void ComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCount.Enabled = numEdgeCount.Enabled = cbRandomCount.Enabled = (IFractal.Type)comboType.SelectedItem == IFractal.Type.Snowflake;
            CbRandomCount_CheckedChanged(sender, e);
        }

        private void CbRandomCount_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRandomCount.Enabled)
                numEdgeCount.Enabled = cbRandomCount.Checked == false;
        }

        private void NumMinBump_ValueChanged(object sender, EventArgs e) =>
            numMaxBump.Value = Math.Max(numMinBump.Value, numMaxBump.Value);

        private void NumMaxBump_ValueChanged(object sender, EventArgs e) =>
            numMinBump.Value = Math.Min(numMinBump.Value, numMaxBump.Value);

        private void CbSave_CheckedChanged(object sender, EventArgs e) =>
            tbSaveDir.Enabled = btnBrowseSaveDir.Enabled = cbSave.Checked;

        private void BtnBrowseSaveDir_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbSaveDir.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
                tbSaveDir.Text = fbd.SelectedPath;
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            Screensaver.Settings.FractalType = (int)comboType.SelectedItem;
            Screensaver.Settings.EdgeCount = (int)numEdgeCount.Value;
            Screensaver.Settings.IsRandomCount = cbRandomCount.Checked;
            Screensaver.Settings.FractalIterations = (int)numIterations.Value;
            Screensaver.Settings.IsRainbow = cbRainbow.Checked;
            Screensaver.Settings.KeepInViewport = cbKeepInViewport.Checked;
            Screensaver.Settings.MinBumpLength = numMinBump.Value;
            Screensaver.Settings.MaxBumpLength = numMaxBump.Value;
            Screensaver.Settings.IterationDelay = (int)numIterationDelay.Value;
            Screensaver.Settings.FractalDelay = (int)numFractalDelay.Value;
            Screensaver.Settings.DoSaveFractal = cbSave.Checked;
            Screensaver.Settings.SaveDestination = tbSaveDir.Text;

            Screensaver.Settings.Save();
        }

        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            comboType.SelectedItem = (IFractal.Type)Screensaver.Settings.FractalType;
            numEdgeCount.Value = GetSanitizedEdgeCountValue(Screensaver.Settings.EdgeCount);
            cbRandomCount.Checked = Screensaver.Settings.IsRandomCount;
            numIterations.Value = Screensaver.Settings.FractalIterations;
            cbRainbow.Checked = Screensaver.Settings.IsRainbow;
            cbKeepInViewport.Checked = Screensaver.Settings.KeepInViewport;
            numMinBump.Value = Screensaver.Settings.MinBumpLength;
            numMaxBump.Value = Screensaver.Settings.MaxBumpLength;
            numIterationDelay.Value = Screensaver.Settings.IterationDelay;
            numFractalDelay.Value = Screensaver.Settings.FractalDelay;
            cbSave.Checked = Screensaver.Settings.DoSaveFractal;
            tbSaveDir.Text = Screensaver.Settings.SaveDestination;

            ComboType_SelectedIndexChanged(this, new EventArgs());
            CbSave_CheckedChanged(this, new EventArgs());
        }

        private int GetSanitizedEdgeCountValue(int edgeCount) =>
            (int)Math.Min(Math.Max(edgeCount, numEdgeCount.Minimum), numEdgeCount.Maximum);
    }
}
