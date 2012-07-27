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
