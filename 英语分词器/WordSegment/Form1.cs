using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WordSegment
{
    public partial class Form1 : Form
    {
        private delegate void AddListView(ListViewItem Itmes);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            saveSToolStripMenuItem.Enabled = false;
            executeEToolStripMenuItem.Enabled = false;
            listView1.Columns.Add("Word", 120);
            listView1.Columns.Add("Count", 100);
            listView1.View = View.Details;
        }

        private void newNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            saveSToolStripMenuItem.Enabled = false;
            executeEToolStripMenuItem.Enabled = false;
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "txt file (*.txt) |*.txt|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file=dlg.FileName;
                textBox1.Text = System.IO.File.ReadAllText(file,Encoding.Default);
            }
            executeEToolStripMenuItem.Enabled = true;
            toolStripStatusLabel1.Text = dlg.FileName;
        }

        private void executeEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSToolStripMenuItem.Enabled = true;
            byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(textBox1.Text);
            int[] num = new int[100];
            string[] str = new string[10000];
            int count = 1;
            int c = 0;
            //ListViewItem item=new ListViewItem();
            int[] strc=new int[10000];
            for (int i = 0; i < 10000; i++)
                strc[i] = 1;
            num[0] = -1;
            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i] == ' ' | buf[i] == ',' | buf[i] == '.')
                {
                    int flag = 1;
                    num[count] = i;
                    count = count + 1;
                    string s="";
                    for (int j = num[count - 2] + 1; j < num[count - 1]; j++)
                    {
                        s += ((char)buf[j]).ToString();
                    }
                    for (int k = 0; k < c; k++)
                        if (s == str[k])
                        {
                            strc[k] = strc[k] + 1;
                            flag = 0;
                        }
                    if(flag==1)
                    {
                        str[c] = s;
                        //item = listView1.Items.Add(str[c]);
                        ListViewItem item = listView1.Items.Add(str[c]);
                        item.SubItems.Add(strc[c].ToString());
                        c = c + 1;
                    }
                   
                }
                
            }
            listView1.BeginUpdate();
            for (int i = 0; i < c; i++)
            {
                listView1.Items[i].SubItems[1].Text = strc[i].ToString();
            }
            listView1.EndUpdate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Are you sure you want to exit this system?", "Comfirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            { e.Cancel = true; }
        }

        private void saveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "txt file (*.txt) |*.txt|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream fs = new FileStream(@"C:\Users\Fisica\Desktop\b.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                //sw.WriteLine(listView1.Text);
                string s = "";
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    s += listView1.Columns[i].Text + "  ";
                }
                s += "\n";
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    for (int j = 0; j < listView1.Items[i].SubItems.Count; j++)
                    {
                        s += listView1.Items[i].SubItems[j].Text + " ";
                    }
                    s += "\n";
                }
                sw.WriteLine(s);
                sw.Close();
            }
        }

        private void newNToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void saveSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void openOToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }
         private void AddListViews(ListViewItem listView)
        {
            if (this.listView1.InvokeRequired)
            {
                var _addListView = new AddListView(this.AddListViews);
                this.listView1.Invoke(_addListView, listView);
            }
            else
            {
                
                this.listView1.Items.Add(listView);
            }
        }
       

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            newNToolStripMenuItem_Click(sender, e);
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            openOToolStripMenuItem_Click(sender, e);
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            executeEToolStripMenuItem_Click(sender, e);
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            saveSToolStripMenuItem_Click(sender, e);
        }
    }
}
