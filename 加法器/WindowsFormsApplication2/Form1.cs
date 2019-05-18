using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random rnd = new Random();
        int number1, number2;
        int range=10, n=10, count=0, num=0;
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            num += 1;
            toolStripStatusLabel1.Text = "Range: " + range + " Question: " + num;
            number1 = rnd.Next(range);
            number2 = rnd.Next(range - number1);
            textBox1.Text = number1.ToString();
            textBox2.Text = number2.ToString();
            textBox3.Text = "";
            textBox3.Focus();
            if (num == n+1)
            {
                DialogResult dr;
                dr = MessageBox.Show("Accuracy: " + count*100 / n  + "%" + Environment.NewLine+"New Questions or Quit? ", "Accuracy", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK){ 
                    num = 0; button1_Click(sender, e); 
                }
                else{
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int answer;
            if (int.TryParse(textBox3.Text, out answer))
            {
                if (answer == number1 + number2)
                {
                    MessageBox.Show("Right!");
                    count += 1;
                }
                else
                    MessageBox.Show("Wrong!");
            }
            else
            {
                MessageBox.Show("Please input a positive integer");
                textBox3.Focus();
                textBox3.SelectAll();
            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button2_Click(sender, e);
                button1_Click(sender, e);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Are you sure you want to QUIT?", "Comfirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            { 
                e.Cancel = false;
                num = 0;
            }
            else
            { e.Cancel = true; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private int totalSecond = 0;
        private int tenthSecond = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int minute = totalSecond / 60;
            int second = totalSecond % 60;
            String str = "Time: "+minute.ToString() + ":" + second.ToString() + ":" + tenthSecond.ToString();
            label3.Text = str;
            tenthSecond++;
            if (tenthSecond == 10)
            {
                tenthSecond = 1;
                totalSecond++;
            }
        }

        private void label3_Click(object sender, EventArgs e){}

        private void noviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            range = 10;
            totalSecond = 0;
            num = 0;
            button1_Click(sender, e);
            toolStripStatusLabel1.Text = "Range: "+range+" Question: "+num;
        }

        private void intermediateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            range = 30;
            totalSecond = 0;
            num = 0;
            button1_Click(sender,e);
            toolStripStatusLabel1.Text = "Range: " + range + " Question: " + num;
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            range = 50;
            totalSecond = 0;
            num = 0;
            button1_Click(sender, e);
            toolStripStatusLabel1.Text = "Range: " + range + " Question: " + num;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e){}

        private void numbersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            n = 10;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            n = 20;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            n = 50;
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            string str = Interaction.InputBox("Please input the number of questions.(Default:10)", "Numbers", "", -1, -1);
            n = Convert.ToInt32(str);
        }

        private void customToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string str = Interaction.InputBox("Please input the range of numbers.(Default:10)", "Numbers", "", -1, -1);
            range = Convert.ToInt32(str);
        }
       
    }
}
