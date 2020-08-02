using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractalScreenSaver
{
    public partial class FractalForm : Form
    {
        public enum Option
        {
            None,
            Debug
        }

        private enum TimerToggleOption
        {
            Iteration,
            NextFractal
        }

        #region Preview API
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);
        #endregion

        private bool isProcessing;
        private bool isDebug;
        private bool isPreview;
        private bool isApplicationClosed;
        private int stepsRemaining;
        private Fractal fractal;
        private Bitmap image;
        private Stopwatch watch = new Stopwatch();
        private long computeTime, drawTime, displayTime, saveTime;
        private Point? mousePos;
        private List<Form> mirrorForms = new List<Form>();

        private bool doSave
        {
            get
            {
                return stepsRemaining == 0
                    && Screensaver.Settings.DoSaveFractal
                    && isPreview == false
                    && isDebug == false;
            }
        }

        private string SaveDirectory { get { return Screensaver.Settings.SaveDestination; } }

        public FractalForm(Option option)
        {
            InitializeComponent();

            isDebug = option == Option.Debug;

            if (isDebug == false)
                Cursor.Hide();

            foreach (Screen screen in Screen.AllScreens.Where(s => s.Primary == false))
                CreateMirrorScreen(screen);
        }

        public FractalForm(IntPtr previewHandle)
        {
            InitializeComponent();
            InitializeForPreview(previewHandle);
        }

        private void InitializeForPreview(IntPtr previewHandle)
        {
            // https://www.codeproject.com/articles/31376/making-a-c-screensaver
            SetParent(Handle, previewHandle);
            SetWindowLong(Handle, -16, new IntPtr(GetWindowLong(Handle, -16) | 0x40000000));

            isPreview = true;
        }

        private void CreateMirrorScreen(Screen screen)
        {
            var mirrorForm = new MirrorForm(screen.Bounds);
            mirrorForm.KeyDown += new KeyEventHandler(Fractal_KeyDown);
            mirrorForm.MouseClick += new MouseEventHandler(Fractal_MouseActivity);
            mirrorForm.MouseMove += new MouseEventHandler(Fractal_MouseActivity);

            mirrorForms.Add(mirrorForm);
            mirrorForm.Show(this);
        }

        private void Fractal_Load(object sender, EventArgs e)
        {
            iterationTimer.Interval = Screensaver.Settings.IterationDelay;
            nextFractalTimer.Interval = Screensaver.Settings.FractalDelay;

            nextFractalTimer_Tick(this, new EventArgs());
        }

        private void FractalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isApplicationClosed = true;
            foreach (Form form in mirrorForms)
                form.Dispose();

            foreach (Pen p in Fractal.Pens.Values)
                p.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (image == null || isApplicationClosed)
                return;

            if (doSave)
                saveTime = GetDurationInMilliseconds(() => SaveFractal());

            displayTime = GetDurationInMilliseconds(() =>
            {
                e.Graphics.DrawImage(image, new Point(0, 0));
                foreach (MirrorForm mf in mirrorForms)
                    mf.Draw(image);
            });

            if (isDebug)
                DrawDiagnostics(e.Graphics);
        }

        private void DrawDiagnostics(Graphics g)
        {
            DrawString(g, $"Res: {ClientRectangle.Width} x {ClientRectangle.Height}", 0, 0);
            DrawString(g, $"CPU: {computeTime} milliseconds", 0, 40);
            DrawString(g, $"Draw: {drawTime} milliseconds", 0, 60);
            DrawString(g, $"Draw/CPU: {drawTime / (float)computeTime}", 200, 60);
            DrawString(g, $"Display: {displayTime} milliseconds", 0, 80);
            DrawString(g, $"Save: {saveTime} milliseconds", 0, 100);
            DrawString(g, $"Remaining: {stepsRemaining}", 0, 120);
            DrawString(g, $"Vertices: {fractal.Vertices.Length}", 0, 160);
        }

        private void DrawString(Graphics g, string text, int x, int y)
        {
            using (var font = new Font("Arial", 12))
            using (var brush = new SolidBrush(Color.White))
                g.DrawString(text, font, brush, LogicalToDeviceUnits(x), LogicalToDeviceUnits(y));
        }

        private long GetDurationInMilliseconds(Action action)
        {
            watch.Restart();
            action.Invoke();
            return watch.ElapsedMilliseconds;
        }

        private void SaveFractal()
        {
            string fileName = $"Fractal{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}.png";
            string savePath = Path.Combine(SaveDirectory, fileName);

            try
            {
                Directory.CreateDirectory(SaveDirectory); // Ensure that the directory exists.
                image.Save(savePath, ImageFormat.Png);
            }
            catch { } // Best effort.
        }

        private void DrawFractal()
        {
            try
            {
                image?.Dispose();
                image = fractal.GetBitmap();
            }
            catch
            {
                if (isApplicationClosed == false) // Disposed pens can throw exceptions when form is closed -> ignore
                    throw;
            }
            Invalidate();
        }

        private async void iterationTimer_Tick(object sender, EventArgs e)
        {
            // Ensure that the screensaver can exit at any time, even if the next iteration may take a while to complete.
            await ProcessSingleInstance(HandleIteration);
        }

        private void HandleIteration()
        {
            if (stepsRemaining-- == 0)
            {
                ToggleActiveTimer(TimerToggleOption.NextFractal);
                return;
            }

            computeTime = GetDurationInMilliseconds(() => fractal.IncreaseFractalDepth());
            drawTime = GetDurationInMilliseconds(() => DrawFractal());
        }

        private async void nextFractalTimer_Tick(object sender, EventArgs e)
        {
            await ProcessSingleInstance(HandleNextFractal);
        }

        private void HandleNextFractal()
        {
            StartNewFractal();
            ToggleActiveTimer(TimerToggleOption.Iteration);
        }

        private void ToggleActiveTimer(TimerToggleOption option)
        {
            RunOnGuiThread(() => // Timers need to be handled from within the GUI thread, otherwise they won't work.
            {
                nextFractalTimer.Enabled = option == TimerToggleOption.NextFractal;
                iterationTimer.Enabled = option == TimerToggleOption.Iteration;
            });
        }

        private void RunOnGuiThread(Action action)
        {
            if (InvokeRequired)
                BeginInvoke(action);
        }

        private async Task ProcessSingleInstance(Action action)
        {
            if (isProcessing)
                return;

            isProcessing = true;
            await Task.Run(action);
            isProcessing = false;
        }

        private void StartNewFractal()
        {
            fractal = new Fractal(ClientRectangle);
            stepsRemaining = Screensaver.Settings.FractalIterations;

            DrawFractal();
        }

        private void Fractal_KeyDown(object sender, KeyEventArgs e)
        {
            if (isDebug)
            {
                if (e.KeyCode == Keys.Escape)
                    Application.Exit();

                return;
            }

            if (isPreview == false)
                Application.Exit();
        }

        private void Fractal_MouseActivity(object sender, MouseEventArgs e)
        {
            if (isPreview || isDebug)
                return;

            if (mousePos == null)
                mousePos = e.Location;

            if (mousePos != e.Location || e.Clicks > 0)
                Application.Exit();
        }
    }

    internal class MirrorForm : Form
    {
        private Bitmap bmp;

        public MirrorForm(Rectangle bounds)
        {
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            DoubleBuffered = true;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Fractal";

            SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        public void Draw(Bitmap bmp)
        {
            this.bmp = bmp;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (bmp != null)
                e.Graphics.DrawImage(bmp, new Point(0, 0));
        }
    }
}