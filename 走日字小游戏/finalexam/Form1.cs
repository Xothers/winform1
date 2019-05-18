using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finalexam
{
    public partial class Form1 : Form
    {
        private Button[,] btn;
        private int level = 6;
        Random rnd = new Random();
        private const int gridSize = 60;
        int moves = 1;
        int[] row = new int[100];
        int[] col = new int[100];
        int[] mov = new int[100];
        int undocount = 0;
        public Form1()
        {
            InitializeComponent();
        }
         private void setGrid(int level){
             undocount = 0;
             redocount = 0;
            panel1.Controls.Clear();
            moves = 1;
            this.ClientSize = new Size(level * gridSize, level * gridSize + statusStrip1.Height + menuStrip1.Height);
            btn = new Button[level, level];
            int s = rnd.Next(level);
            for(int i = 0; i < level; i++)
                for(int j=0;j<level;j++)
                {
                    btn[i, j] = new Button();
                    btn[i, j].Location = new Point(j * gridSize, i * gridSize);
                    btn[i, j].Size = new Size(gridSize, gridSize);
                    btn[i, j].Font = new System.Drawing.Font("Calibri", 20);
                    btn[i, j].Tag = i * level + j;
                    btn[i,j].BackColor=Color.White;
                    btn[i, j].Click += new EventHandler(btn_Click);
                    btn[i, j].MouseDown += new MouseEventHandler(btn_MouseDown);
                    btn[i, j].MouseUp += new MouseEventHandler(btn_MouseUp);
                    panel1.Controls.Add(btn[i, j]);
                }
            btn[s, s].Text = "1";
            btn[s, s].BackColor = Color.Maroon;
        }

         private void Form1_Load(object sender, EventArgs e)
         {
             toolStripMenuItem1.Checked = true;
             level = 6;
             setGrid(level);
         }

         private void noviceNToolStripMenuItem_Click(object sender, EventArgs e)
         {
             totalSecond = 0;
             moves = 1;
             toolStripMenuItem1.Checked = true;
             toolStripMenuItem2.Checked = false;
             toolStripMenuItem3.Checked = false;
             level = 6;
             setGrid(level);
         }
        private int totalSecond = 0;
        private int tenthSecond = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int minute = totalSecond / 60;
            int second = totalSecond % 60;
            int move = moves - 1;
            String str = "Time: " + minute.ToString() + ":" + second.ToString() + ":" + tenthSecond.ToString();
            toolStripStatusLabel1.Text = str + " Moves: " + move;
            tenthSecond++;
            if (tenthSecond == 10)
            {
                tenthSecond = 1;
                totalSecond++;
            }
        }

        private void intermediateIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            totalSecond = 0;
            moves = 1;
            toolStripMenuItem1.Checked = false;
            toolStripMenuItem2.Checked = true;
            toolStripMenuItem3.Checked = false;
            level = 8;
            setGrid(level);
        }

        private void advancedAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            totalSecond = 0;
            moves = 1;
            toolStripMenuItem1.Checked = false;
            toolStripMenuItem2.Checked = false;
            toolStripMenuItem3.Checked = true;
            level = 10;
            setGrid(level);
        }

        private void btn_Click(object sender, EventArgs e)
        {
            int flag = 0 ;
            Button curBtn = (Button)sender;
            Button prev = btn[0, 0];
            for (int r = 0; r < level; r++)
                for (int c = 0; c < level; c++)
                    if (btn[r, c].Text == moves.ToString())
                        prev = btn[r, c];
            int r1 = (int)curBtn.Tag / level;
            int c1 = (int)curBtn.Tag % level;
            int r2 = (int)prev.Tag / level;
            int c2 = (int)prev.Tag % level;
            if ((Math.Abs(r1-r2)==2 && Math.Abs(c1-c2 )==1) ||(Math.Abs(r1-r2)==1 && Math.Abs(c1-c2 )==2))
            {
               flag=1;
            }
            if (curBtn.Text == "" && flag == 1)
            {
                moves += 1;
                curBtn.Text = moves.ToString();
                curBtn.BackColor = Color.Maroon;
                timer1.Enabled = true;
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
            else
            {
                if (curBtn.BackColor != Color.Maroon)
                    MessageBox.Show("This button is not available, choose another button");
            }
        }
        bool IsSucceed()
        {
            for (int r = 0; r < level; r++)
                for (int c = 0; c < level; c++)
                {
                    if (btn[r, c].BackColor != Color.Maroon)
                        return false;
                }
            return true;
        }
        private void startSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setGrid(level);
        }

        private void exitEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e){}

        private void visibleVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusStrip1.Visible == true)
            {
                //statusStrip1.Visible = false;
                statusStrip1.Hide();
               // this.ClientSize = new Size(level * gridSize, level * gridSize);
            }
            else
            {
               // statusStrip1.Visible = true;
                statusStrip1.Show();
                //this.ClientSize = new Size(level * gridSize, level * gridSize + statusStrip1.Height + menuStrip1.Height);
            }
        }

        private void undoUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (moves != 1)
            {
                Button prev = btn[0, 0];
                for (int r = 0; r < level; r++)
                    for (int c = 0; c < level; c++)
                        if (btn[r, c].Text == moves.ToString())
                        {
                            prev = btn[r, c];
                            row[undocount] = r;
                            col[undocount] = c;
                            mov[undocount] = moves;
                        }
                undocount += 1;
                prev.BackColor = Color.White;
                prev.Text = "";
                moves -= 1;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)  { }
        int redocount = 0;
        private void redoRToolStripMenuItem_Click(object sender, EventArgs e)
        {
                btn[row[undocount-redocount-1], col[undocount-redocount-1]].BackColor = Color.Maroon;
                btn[row[undocount - redocount-1], col[undocount - redocount-1]].Text = mov[undocount - redocount-1].ToString();
                moves += 1;
                redocount += 1;
        }

        private void aboutAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("游戏玩法：马按日字格走，不出棋盘边界，不走已走过的格子。走过的格子显示步数的序号。");
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button curBtn = (Button)sender;
            if (curBtn.BackColor == Color.Maroon)
            {
                for (int r = 0; r < level; r++)
                    for (int c = 0; c < level; c++)
                    {
                        int r1 = (int)curBtn.Tag / level;
                        int c1 = (int)curBtn.Tag % level;
                        int r2 = (int)btn[r, c].Tag / level;
                        int c2 = (int)btn[r, c].Tag % level;
                        if ((Math.Abs(r1 - r2) == 2 && Math.Abs(c1 - c2) == 1) || (Math.Abs(r1 - r2) == 1 && Math.Abs(c1 - c2) == 2))
                        {
                            if(btn[r,c].BackColor!=Color.Maroon)
                                 btn[r, c].BackColor = Color.Aqua;
                        }
                    }
            }
        }
        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            Button curBtn = (Button)sender;
            if (curBtn.BackColor == Color.Maroon)
            {
                for (int r = 0; r < level; r++)
                    for (int c = 0; c < level; c++)
                    {
                        int r1 = (int)curBtn.Tag / level;
                        int c1 = (int)curBtn.Tag % level;
                        int r2 = (int)btn[r, c].Tag / level;
                        int c2 = (int)btn[r, c].Tag % level;
                        if ((Math.Abs(r1 - r2) == 2 && Math.Abs(c1 - c2) == 1) || (Math.Abs(r1 - r2) == 1 && Math.Abs(c1 - c2) == 2))
                        {
                            if (btn[r, c].BackColor == Color.Aqua)
                                btn[r, c].BackColor = Color.White;
                        }
                    }
            }
        }
    }
}
