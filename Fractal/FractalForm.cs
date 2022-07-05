using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
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

        private const int HlsMaxValue = 240; // Determined through testing.

        private bool IsDebug { get; init; }
        private bool IsPreview { get; init; }

        private readonly Stopwatch watch = new();
        private readonly Dictionary<int, Pen> Pens;

        private IEnumerable<Form> mirrorForms;
        private bool isProcessing;
        private bool isApplicationClosed;
        private int stepsRemaining;
        private IFractal fractal;
        private Bitmap image;
        private long computeTime, drawTime, displayTime, saveTime;
        private Point? mousePos;

        private readonly Dictionary<IFractal.Type, Func<(int width, int height), IFractal>> FractalFactoryMapper = new()
        {
            { IFractal.Type.Tree, dimensions => new Tree(dimensions) },
            { IFractal.Type.Snowflake, dimensions => new Snowflake(dimensions) }
        };

        private bool DoSave =>
            stepsRemaining == 0 &&
            Screensaver.Settings.DoSaveFractal &&
            IsPreview == false &&
            IsDebug == false;

        private static string SaveDirectory => Screensaver.Settings.SaveDestination;

        private static int GetHueFromFactor(double factor) => (int)(HlsMaxValue * factor);

        public FractalForm()
        {
            InitializeComponent();

            Pens = Enumerable.Range(0, HlsMaxValue + 1).ToDictionary(i => i, GetPenFromHue);
        }

        public static FractalForm FromOption(Option option)
        {
            FractalForm form = new() { IsDebug = option == Option.Debug };

            form.mirrorForms = Screen.AllScreens.Where(s => s.Primary == false).Select(form.CreateMirrorScreen).ToList();
            if (form.IsDebug == false)
                Cursor.Hide();

            return form;
        }

        public static FractalForm FromIntPtr(IntPtr previewHandle)
        {
            FractalForm form = new() { IsPreview = true };

            // https://www.codeproject.com/articles/31376/making-a-c-screensaver
            _ = SetParent(form.Handle, previewHandle);
            _ = SetWindowLong(form.Handle, -16, new IntPtr(GetWindowLong(form.Handle, -16) | 0x40000000));

            return form;
        }

        private Pen GetPenFromHue(int hue) => new(GetColorFromHue(hue), LogicalToDeviceUnits(1));

        private static Color GetColorFromHue(int hue)
        {
            const int luminance = HlsMaxValue / 2; // 0 is black, 240 is white.
            const int saturation = HlsMaxValue;

            return ColorTranslator.FromWin32(ColorHLSToRGB(hue, luminance, saturation));
        }

        private MirrorForm CreateMirrorScreen(Screen screen)
        {
            MirrorForm mirrorForm = new(screen.Bounds);
            mirrorForm.KeyDown += new KeyEventHandler(Fractal_KeyDown);
            mirrorForm.MouseClick += new MouseEventHandler(Fractal_MouseActivity);
            mirrorForm.MouseMove += new MouseEventHandler(Fractal_MouseActivity);
            mirrorForm.Show(this);

            return mirrorForm;
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

            if (IsDebug)
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

        private void ResetDiagnosticsStats()
        {
            computeTime = drawTime = displayTime = 0;
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
            foreach ((int hue, Vector2[] vertices) in GetPolylines(fractal.Vertices))
            {
                if (isApplicationClosed)
                    return;

                g.DrawLines(Pens[hue], vertices.Select(v => new PointF(v.X, v.Y)).ToArray());
            }

            Invalidate();
        }

        public static IEnumerable<(int hue, Vector2[] vertices)> GetPolylines(Vector2[] vertices)
        {
            if (Screensaver.Settings.IsRainbow == false)
            {
                yield return (GetHueFromFactor(Screensaver.Random.NextDouble()), vertices);
                yield break;
            }

            int previousHue = 0, groupStart = 0;
            for (int i = 1; i < vertices.Length; i++)
            {
                int hue = GetHueFromFactor(i / (double)vertices.Length);
                if (hue == previousHue)
                    continue;

                yield return (previousHue, vertices[groupStart..(i + 1)]);
                previousHue = hue;
                groupStart = i;
            }

            if (groupStart != vertices.Length - 1) // In case there's a group at the end which we haven't yielded yet.
                yield return (previousHue, vertices[groupStart..vertices.Length]);
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
            fractal = FractalFactoryMapper[(IFractal.Type)Screensaver.Settings.FractalType]((ClientRectangle.Width, ClientRectangle.Height));
            stepsRemaining = Screensaver.Settings.FractalIterations;

            ResetDiagnosticsStats();
            DrawFractal();
        }

        private void Fractal_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsDebug)
            {
                if (e.KeyCode == Keys.Escape)
                    Application.Exit();

                return;
            }

            if (IsPreview == false)
                Application.Exit();
        }

        private void Fractal_MouseActivity(object sender, MouseEventArgs e)
        {
            if (IsPreview || IsDebug)
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