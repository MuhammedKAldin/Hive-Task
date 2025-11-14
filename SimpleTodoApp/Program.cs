using System;
using System.Threading;
using System.Windows.Forms;

namespace SimpleTodoApp
{
    internal static class Program
    {
        private static Mutex? appMutex;
        private const string MutexName = "SimpleTodoApp_Mutex";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // Single instance check
            bool createdNew;
            appMutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                // Another instance is running, try to bring it to front
                IntPtr hWnd = NativeMethods.FindWindow(null, "Todo Application");
                if (hWnd != IntPtr.Zero)
                {
                    NativeMethods.ShowWindow(hWnd, NativeMethods.SW_RESTORE);
                    NativeMethods.SetForegroundWindow(hWnd);
                }
                return;
            }

            try
            {
                Application.Run(new MainForm());
            }
            finally
            {
                appMutex?.ReleaseMutex();
                appMutex?.Dispose();
            }
        }
    }

    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public const int SW_RESTORE = 9;
    }
}

