using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace DLLInjectorCS
{
    public partial class DLLInjector : Form
    {
        private ProcsSnap procsSnap;
        public DLLInjector()
        {
            InitializeComponent();
            procsSnap = new ProcsSnap();
            listView1.Items.AddRange(procsSnap.LVItems);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            procsSnap.Refresh();
            listView1.Items.Clear();
            listView1.Items.AddRange(procsSnap.LVItems);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Injector i = new Injector(textBox1.Text, ((Process)listView1.SelectedItems[0].Tag).Id);
                i.injectDll();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Injector i = new Injector(textBox1.Text);
                i.suspendOnInject = checkBox1.Checked;
                i.startInjectingThread(textBox2.Text, new InjectedCallback(injectSuccess));
                button5.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            textBox2.Text = Program.stripExeName(openFileDialog2.FileName);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread t;
            if ((t = Injector.getThread()) != null)
            {
                t.Abort();
                button5.Enabled = false;
            }
        }

        private void injectSuccess()
        {
            if (this.button5.InvokeRequired)
            {
                this.Invoke(new InjectedCallback(injectSuccess));
            }
            else
            {
                button5.Enabled = false;
            }
        }
    }
}
