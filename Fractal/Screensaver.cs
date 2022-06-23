using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FractalScreenSaver.Properties;

namespace FractalScreenSaver
{
    class Screensaver
    {
        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        // According to https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512(v=vs.85).aspx
        private enum DpiAwareness
        {
            None = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }

        internal static Settings Settings { get { return Settings.Default; } }

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _ = SetProcessDpiAwareness((int)DpiAwareness.PerMonitorAware); // Required because DPI Awareness from app.manifest doesn't work when run as screensaver

            EnsureSaveDestinationIsSet();
            switch (args.FirstOrDefault()?.Substring(1, 1).ToUpper())
            {
                case null:
                    Run(FractalForm.Option.Debug);
                    break;

                case "S":
                    Run(FractalForm.Option.None);
                    break;

                case "C":
                    using (var configForm = new SettingsForm()) configForm.ShowDialog();
                    break;

                case "P":
                    Application.Run(new FractalForm(new IntPtr(long.Parse(args[1]))));
                    break;
            }
        }

        private static void Run(FractalForm.Option option)
        {
            Application.Run(new FractalForm(option));
        }

        private static void EnsureSaveDestinationIsSet()
        {
            if (string.IsNullOrWhiteSpace(Settings.SaveDestination)) // Default save location.
            {
                Settings.SaveDestination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Fractal");
                Settings.Save();
            }
        }
    }
}
