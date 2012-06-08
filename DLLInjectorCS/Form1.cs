using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DLLInjectorCS
{
    public partial class DLLInjector : Form
    {
        public DLLInjector()
        {
            InitializeComponent();
            Program.takeProcSnapshot();
            listBox1.Items.AddRange(Program.getProcSnapStrings());
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
            Program.takeProcSnapshot();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(Program.getProcSnapStrings());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Injector i = new Injector(textBox1.Text, Program.curProcSnapshot[listBox1.SelectedIndex].Id);
                i.injectDll();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Injector i = new Injector(textBox1.Text);
                i.startInjectingThread(textBox2.Text);
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
    }
}
