using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Microsoft.VisualBasic;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private Button[,] btn;
        private int level = 3;
        Random rnd = new Random();
        private const int gridSize = 100;
        int moves = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void noviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            totalSecond = 0;
            moves = 0;
            noviceToolStripMenuItem.Checked = true;
            intermediateToolStripMenuItem.Checked = false;
            advancedToolStripMenuItem.Checked = false;
            level = 3;
            setGrid(level);
        }
        private void setGrid(int level){
            panel1.Controls.Clear();
            this.ClientSize = new Size(level * gridSize, level * gridSize + menuStrip1.Height + toolStripStatusLabel1.Height);
            btn = new Button[level, level];
            int[] s=new int[level*level];
            for (int i = 0; i < level*level; i++)
                s[i] = i;
            s = (from c in s
                 orderby Guid.NewGuid()
                 select c).ToArray<int>();
            int[] array = s;
            for(int i = 0; i < level; i++)
                for(int j=0;j<level;j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Location = new Point(j * gridSize, i * gridSize);
                    btn[i, j].Size = new Size(gridSize, gridSize);
                    btn[i, j].Font = new System.Drawing.Font("Calibri", 20);
                    btn[i, j].Text = array[i * level + j].ToString();
                    btn[i, j].Tag = i * level + j;
                    if (btn[i, j].Text == "0")
                        btn[i, j].Text = "";
                    btn[i, j].Click += new EventHandler(btn_Click);
                    panel1.Controls.Add(btn[i, j]);
                }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            noviceToolStripMenuItem.Checked = true;
            level = 3;
            setGrid(level);
        }
        
        Button FindHiddenButton()
        {
            for (int r = 0; r < level; r++)
                for (int c = 0; c < level; c++)
                {
                    if (btn[r,c].Text=="")
                    {
                        return btn[r, c];
                    }
                }
            return null;
        }
        bool IsNoSolution()
        {
            int num1=0,num2=0;
            for (int r = 0; r < level; r++)
                for (int c = 0; c < level; c++)
                    if ((btn[r, c].Text == (r * level + c + 1).ToString()))
                        num1 += 1;
            for (int r = 0; r < level; r++)
                for (int c = 0; c < level - 1; c++)
                    if((btn[r, c].Text == (r * level + c + 2).ToString()) & (btn[r, c + 1].Text == (r * level + c + 1).ToString()))
                        num2 += 1;
            if ((num1+num2*2==level*level-1) & (num2 % 2 == 1))
                return true;
            else
                return false;
        }

        bool IsSucceed()
        {
            for (int r = 0; r < level; r++)
                for (int c = 0; c < level; c++)
                {
                    if(!(r==(level-1)&&c==(level-1)))
                        if (btn[r, c].Text != (r * level + c + 1).ToString())
                            return false;
                }
            return true;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button curBtn = (Button)sender;
            Button blank = btn[0,0];
            for (int r = 0; r < level; r++)
                for (int c = 0; c < level; c++)
                    if (btn[r, c].Text == "")
                        blank = btn[r, c];
            moves += 1;
            int r1 = (int)curBtn.Tag / level;
            int c1 = (int)curBtn.Tag % level;
            int r2 = (int)blank.Tag / level;
            int c2 = (int)blank.Tag % level;
            if (r1 == r2 && (c1 == c2 - 1 || c1 == c2 + 1)
               || c1 == c2 && (r1 == r2 - 1 || r1 == r2 + 1))
            {
                string t = curBtn.Text;
                curBtn.Text = blank.Text;
                blank.Text = t;
                blank.Focus();
            }
            timer1.Enabled = true;
            if(IsNoSolution()){
                DialogResult dr;
                dr = MessageBox.Show("No solution" + Environment.NewLine + "Start a new game?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    setGrid(level);
                    totalSecond = 0;
                }
                else
                    this.Close();
            }
            if (IsSucceed())
            {
                timer1.Enabled = false;
                DialogResult dr2;
                dr2 = MessageBox.Show("Congratulations!" + Environment.NewLine + toolStripStatusLabel1.Text + Environment.NewLine + "Start a new game?", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr2 == DialogResult.OK)
                {
                    totalSecond = 0;
                    setGrid(level);
                }
                else
                    this.Close();
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void intermediateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            totalSecond = 0;
            moves = 0;
            noviceToolStripMenuItem.Checked = false;
            intermediateToolStripMenuItem.Checked = true;
            advancedToolStripMenuItem.Checked = false;
            level = 4;
            setGrid(level);
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            totalSecond = 0;
            moves = 0;
            noviceToolStripMenuItem.Checked = false;
            intermediateToolStripMenuItem.Checked = false;
            advancedToolStripMenuItem.Checked = true;
            level = 5;
            setGrid(level);
        }
        private int totalSecond = 0;
        private int tenthSecond = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int minute = totalSecond / 60;
            int second = totalSecond % 60;
            String str = "Time: " + minute.ToString() + ":" + second.ToString() + ":" + tenthSecond.ToString();
            toolStripStatusLabel1.Text = str+" Moves: "+moves;
            tenthSecond++;
            if (tenthSecond == 10)
            {
                tenthSecond = 1;
                totalSecond++;
            }
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("Please input the size(3~8)", "Size", "", -1, -1);
            level = Convert.ToInt32(str);
            setGrid(level);
        }

        private void levelToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
