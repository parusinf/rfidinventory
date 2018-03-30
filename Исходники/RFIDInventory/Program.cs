// File: Program.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace ru.nikitin.RFIDInventory
{
    static class Program
    {
        // The main entry point for the application.
        [MTAThread]
        static void Main()
        {
            SingleInstanceApplication.Run(new MainForm());
        }
    }

    // Run a single instance of the application. Setup the focus to the running instance.
    static class SingleInstanceApplication
    {
        [DllImport("coredll.dll", SetLastError = true)]
        public static extern IntPtr CreateMutex(IntPtr Attr, bool Own, string Name);

        [DllImport("coredll.dll", SetLastError = true)]
        public static extern bool ReleaseMutex(IntPtr hMutex);

        const long ALREADY_EXISTS = 183;

        public static void Run(MainForm form)
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name;
            IntPtr mutexHandle = CreateMutex(IntPtr.Zero, true, name);
            long error = Marshal.GetLastWin32Error();

            if (error != ALREADY_EXISTS && form != null)
                Application.Run(form);
            else
                form.TopMost = true;

            ReleaseMutex(mutexHandle);
        }
    }
}