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

using FractalScreenSaver.Fractals;

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

        [DllImport("shlwapi.dll")]
        private static extern int ColorHLSToRGB(int H, int L, int S);

        public const int HlsMaxValue = 240; // Determined through testing.

        private readonly bool isDebug;
        private readonly bool isPreview;
        private readonly Stopwatch watch = new();
        private readonly List<Form> mirrorForms = new();
        private readonly Dictionary<int, Pen> Pens;

        private bool isProcessing;
        private bool isApplicationClosed;
        private int stepsRemaining;
        private IFractal fractal;
        private Bitmap image;
        private long computeTime, drawTime, displayTime, saveTime;
        private Point? mousePos;

        private readonly Dictionary<IFractal.Type, Func<Rectangle, IFractal>> FractalFactoryMapper = new()
        {
            { IFractal.Type.Tree, rectangle => new Tree(rectangle) },
            { IFractal.Type.Snowflake, rectangle => new Snowflake(rectangle) }
        };

        private bool DoSave =>
            stepsRemaining == 0 &&
            Screensaver.Settings.DoSaveFractal &&
            isPreview == false &&
            isDebug == false;

        private static string SaveDirectory => Screensaver.Settings.SaveDestination;

        public FractalForm()
        {
            Pens = Enumerable.Range(0, HlsMaxValue + 1)
                .ToDictionary(i => i, i => new Pen(GetColorFromHue(i), LogicalToDeviceUnits(1)));
        }

        public FractalForm(Option option) : this()
        {
            InitializeComponent();

            isDebug = option == Option.Debug;

            if (isDebug == false)
                Cursor.Hide();

            foreach (Screen screen in Screen.AllScreens.Where(s => s.Primary == false))
                CreateMirrorScreen(screen);
        }

        public FractalForm(IntPtr previewHandle) : this()
        {
            InitializeComponent();

            // https://www.codeproject.com/articles/31376/making-a-c-screensaver
            _ = SetParent(Handle, previewHandle);
            _ = SetWindowLong(Handle, -16, new IntPtr(GetWindowLong(Handle, -16) | 0x40000000));

            isPreview = true;
        }

        public static Color GetColorFromHue(int hue)
        {
            const int luminance = HlsMaxValue / 2; // 0 is black, 240 is white.
            const int saturation = HlsMaxValue;

            return ColorTranslator.FromWin32(ColorHLSToRGB(hue, luminance, saturation));
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

            NextFractalTimer_Tick(this, new EventArgs());
        }

        private void FractalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isApplicationClosed = true;
            foreach (Form form in mirrorForms)
                form.Dispose();

            foreach (Pen p in Pens.Values)
                p.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (image == null || isApplicationClosed)
                return;

            if (DoSave)
                saveTime = GetDurationInMilliseconds(() => SaveFractal());

            displayTime = GetDurationInMilliseconds(() =>
            {
                e.Graphics.DrawImage(image, new Point(0));
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
            DrawString(g, $"Edges: {fractal.EdgeCount}", 0, 160);
        }

        private void DrawString(Graphics g, string text, int x, int y)
        {
            using var font = new Font("Arial", 12);
            using var brush = new SolidBrush(Color.White);
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
            string fileName = $"Fractal{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.png";
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
            if (isApplicationClosed)
                return;

            image?.Dispose();
            image = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            using var g = Graphics.FromImage(image);
            foreach (var polyline in fractal.GetColoredPolyline())
                g.DrawLines(Pens[polyline.Hue], polyline.Vertices);

            Invalidate();
        }

        private async void NextFractalTimer_Tick(object sender, EventArgs e) =>
            await ProcessSingleInstance(HandleNextFractal);

        private async void IterationTimer_Tick(object sender, EventArgs e) =>
            await ProcessSingleInstance(HandleIteration); // Ensure that the screensaver can exit at any time, even if the next iteration may take a while to complete.

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
                _ = BeginInvoke(action);
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
            fractal = FractalFactoryMapper[(IFractal.Type)Screensaver.Settings.FractalType](ClientRectangle);
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
                e.Graphics.DrawImage(bmp, new Point(0));
        }
    }
}