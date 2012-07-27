using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace DLLInjectorCS
{
    class ProcsSnap
    {
        private Process[] Processes;

        public ProcsSnap()
        {
            Refresh();
        }

        public void Refresh()
        {
            Processes = Process.GetProcesses();
        }

        public ListViewItem[] LVItems
        {
            get
            {
                ListViewItem[] result = new ListViewItem[Processes.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = Processes[i];
                    lvi.Text = Processes[i].ProcessName;
                    lvi.SubItems.Add(Processes[i].Id.ToString());
                    result[i] = lvi;
                }
                return result;
            }
        }
    }
}
