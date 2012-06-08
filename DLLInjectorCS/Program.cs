using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace DLLInjectorCS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DLLInjector());
        }

        public static Process[] curProcSnapshot { get; set; }

        public static string[] getProcSnapStrings()
        {
            string[] result = new string[curProcSnapshot.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = curProcSnapshot[i].ProcessName;
            }

            return result;
        }

        public static void takeProcSnapshot()
        {
            curProcSnapshot = Process.GetProcesses();
        }

        public static string stripExeName(string s)
        {
            string result = string.Copy(s);
            int lslash = result.LastIndexOf('\\');
            result = result.Remove(0, lslash+1);
            result = result.Remove(result.Length - 4);

            return result;
        }
    }
}
