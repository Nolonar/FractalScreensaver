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

        internal static Settings Settings => Settings.Default;

        internal static Random Random = new();

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
#if DEBUG
                    Run(FractalForm.Option.Debug);
                    break;
#else
                    goto case "C";
#endif

                case "S":
                    Run(FractalForm.Option.None);
                    break;

                case "C":
                    using (SettingsForm configForm = new())
                        configForm.ShowDialog();

                    break;

                case "P":
                    Run(args[1]);
                    break;
            }
        }

        private static void Run(FractalForm fractalForm) =>
            Application.Run(fractalForm);

        private static void Run(FractalForm.Option option) =>
            Run(FractalForm.FromOption(option));

        private static void Run(string address) =>
            Run(FractalForm.FromIntPtr(new IntPtr(long.Parse(address))));

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
