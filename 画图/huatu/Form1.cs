using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
 

namespace huatu
{
    public enum ToolType
    {
        Pen,
        Eclipse,
        Line,
        SprayGun,
        Curve,
        Eraser,
        PaintBucket
    }
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        private CheckBox curCheckbox;
        private Point curP,startP;
        private ToolType curTool;
        private Stack<Bitmap> unDoStack, reDoStack;
        private bool isdrawing;
        Color color=Color.Black;
        Random rnd = new Random();
        int wid = 1, x1, y1,r1,r2;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            color = button1.BackColor;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.FromArgb(100,255,0),10);
            g.DrawLine(Pens.Beige, new Point(0, 0), new Point(this.ClientSize.Width));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            unDoStack = new Stack<Bitmap>();
            reDoStack = new Stack<Bitmap>();
            undoToolStripMenuItem.Enabled = false;
            redoToolStripMenuItem.Enabled = false;
             bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            toolboxToolStripMenuItem.Checked = true;
            colorboxToolStripMenuItem.Checked = true;
            checkBox1.Checked = true;
            curCheckbox = checkBox1;
            foreach (object a in panel1.Controls)
            {
                CheckBox tmp = (CheckBox)a;
                tmp.Click += new EventHandler(checkBox_Click);
            }
        }
        
        private void fileFToolStripMenuItem_Click(object sender, EventArgs e){}
        private void panel1_Paint(object sender, PaintEventArgs e){}

        private void toolboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolboxToolStripMenuItem.Checked = !toolboxToolStripMenuItem.Checked;
            if (toolboxToolStripMenuItem.Checked == false)
                panel1.Hide();
            if (toolboxToolStripMenuItem.Checked == true)
                panel1.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {}

        private void checkBox1_Click(object sender, EventArgs e){}
        private void checkBox_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("ooo");
            curCheckbox.Checked = false;
            curCheckbox = (CheckBox)sender;
            curCheckbox.Checked = true;
            switch (curCheckbox.Name)
            {
                case "checkBox1":
                    curTool = ToolType.Pen;
                    break;
                case "checkBox2":
                    curTool = ToolType.Eclipse;
                    break;
                case "checkBox3":
                    curTool = ToolType.Line;
                    break;
                case "checkBox4":
                    curTool = ToolType.SprayGun;
                    break;
                case "checkBox5":
                    curTool = ToolType.Curve;
                    break;
                case "checkBox6":
                    curTool = ToolType.Eraser;
                    break;
                case "checkBox7":
                    curTool = ToolType.PaintBucket;
                    break;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            r1 = rnd.Next(-150, 150);
            r2 = rnd.Next(-150, 150);
            if(e.Button==System.Windows.Forms.MouseButtons.Left)
            {
                curP = new Point(e.X, e.Y);
                x1 = e.X;
                y1 = e.Y;
                startP = new Point(e.X, e.Y);
                unDoStack.Push((Bitmap)bmp.Clone());
                undoToolStripMenuItem.Enabled = true;
            }
            isdrawing = true;
            switch (curTool)
            {
                case ToolType.SprayGun:
                    timer1.Enabled = true;
                    break;
                case ToolType.PaintBucket:
                    bmp = FloodFill((Bitmap)pictureBox1.Image, new Point(e.X, e.Y), color,10);
                    pictureBox1.Image = bmp;
                    break;
            }

        }
                
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
            Graphics g = Graphics.FromImage(bmp);
            if(isdrawing){
                switch (curTool)
                {
                    case ToolType.Pen:
                        Pen P = new Pen(color,wid);
                        g.DrawLine(P, curP, new Point(e.X, e.Y));
                        curP = new Point(e.X, e.Y);
                        pictureBox1.Image = bmp;
                        break;
                    case ToolType.Eclipse:
                        Pen myPen = new Pen(color);
                        Bitmap tmp1 = (Bitmap)bmp.Clone();
                        g = Graphics.FromImage(tmp1);
                        g.DrawEllipse(myPen, x1, y1, e.X, e.Y);
                        curP = new Point(e.X, e.Y);
                        pictureBox1.Image = tmp1;
                        break;
                    case ToolType.Line:
                        Pen Pe = new Pen(color);
                        Bitmap tmp = (Bitmap)bmp.Clone();
                        g = Graphics.FromImage(tmp);
                        g.DrawLine(Pe, startP, new Point(e.X, e.Y));
                        pictureBox1.Image = tmp;
                        break;
                    case ToolType.SprayGun:
                        int n=0, r=8;  
                        Brush b = new SolidBrush(color);
                        while (n < 50)
                        {
                            int x = rnd.Next(-r, r);
                            int y = rnd.Next(-r, r);
                            int px = e.X + x;
                            int py = e.Y + y;
                            if (x * x + y * y < r * r && px > 0 && py > 0 && px < bmp.Width && py < bmp.Height)
                            {
                                bmp.SetPixel(px, py, color);
                            }
                            n++;      
                        }
                        pictureBox1.Image = bmp;
                        break;
                    case ToolType.Curve:
                        Pen n3 = new Pen(color);
                        Bitmap tmp2 = (Bitmap)bmp.Clone();
                        g = Graphics.FromImage(tmp2);
                        g.DrawCurve(n3, new Point[] { new Point(x1, y1), new Point(x1 + r1, y1 + r2), new Point(e.X, e.Y) }, 0.8f);
                        curP = new Point(e.X, e.Y);
                        pictureBox1.Image = tmp2;
                        break;
                    case ToolType.Eraser:
                        Pen p1 = new Pen(pictureBox1.BackColor, 10);
                        g.DrawLine(p1, curP, new Point(e.X, e.Y));
                        curP = new Point(e.X, e.Y);
                        pictureBox1.Image = bmp;
                        break;
                 }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                switch (curTool)
                {
                    case ToolType.Line:
                        Pen n = new Pen(color);
                        Graphics g = Graphics.FromImage(bmp);
                        g.DrawLine(n, startP, new Point(e.X, e.Y));
                        pictureBox1.Image = bmp;
                        break;
                    case ToolType.Eclipse:
                        Pen n2 = new Pen(color);
                        Graphics g2 = Graphics.FromImage(bmp);
                        g2.DrawEllipse(n2, x1, y1, e.X, e.Y);
                        pictureBox1.Image = bmp;
                        break;
                    case ToolType.Curve:
                        Pen n3 = new Pen(color);
                        Graphics g3 = Graphics.FromImage(bmp);
                        g3.DrawCurve(n3, new Point[]{new Point(x1,y1), new Point(x1+r1,y1+r2), new Point(e.X,e.Y)}, 0.8f);
                        pictureBox1.Image = bmp;
                        break;
                }
            }
            isdrawing = false;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (unDoStack.Count == 0)
                return;
            reDoStack.Push((Bitmap)bmp.Clone());
            redoToolStripMenuItem.Enabled = true;
            bmp = unDoStack.Pop();
            pictureBox1.Image = bmp;
            if(unDoStack.Count==0)
            {
                undoToolStripMenuItem.Enabled = false;
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reDoStack.Count == 0)
                return;
            unDoStack.Push((Bitmap)bmp.Clone());
            bmp = reDoStack.Pop();
            pictureBox1.Image = bmp;
            if (reDoStack.Count == 0)
            {
                redoToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Enabled = true;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "bmp file (*.bmp) |*.bmp|JPEG File (*.jpg)|*.jpg";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                bmp.Save(dlg.FileName);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "bmp file (*.bmp) |*.bmp|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image = Bitmap.FromFile(dlg.FileName);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            g.FillRegion(Brushes.White, new Region(new Rectangle(0,0,bmp.Width,bmp.Height)));
            pictureBox1.Image = bmp;
        }

        private void colorboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorboxToolStripMenuItem.Checked = !colorboxToolStripMenuItem.Checked;
            if (colorboxToolStripMenuItem.Checked == false)
                panel2.Hide();
            if (colorboxToolStripMenuItem.Checked == true)
                panel2.Show();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e){}

        private void button2_Click(object sender, EventArgs e)
        {
            color = button2.BackColor;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            color = button3.BackColor;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            color = button7.BackColor;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            color = button6.BackColor;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            color = button4.BackColor;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            color = button5.BackColor;
        }

        private void button9_Click(object sender, EventArgs e)
        {}

        private void lineShape1_Click(object sender, EventArgs e)
        {
            wid = 1;
        }

        private void lineShape2_Click(object sender, EventArgs e)
        {
            wid = 3;
        }

        private void lineShape3_Click(object sender, EventArgs e)
        {
            wid = 5;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e){}

        private void checkBox2_CheckedChanged(object sender, EventArgs e){}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Do you want to save the picture?", "Comfirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                saveToolStripMenuItem_Click(sender, e);
                e.Cancel = false;
            }
            else
            { e.Cancel = false; }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            int n = 0, r = 8;
            Brush b = new SolidBrush(color);
            while (n < 50)
            {
                int x = rnd.Next(-r, r);
                int y = rnd.Next(-r, r);
                int px = x1 + x;
                int py = y1 + y;
                if (x * x + y * y < r * r)
                {
                    bmp.SetPixel(px, py, color);
                }
                n++;
            }
            pictureBox1.Image = bmp;
        }

        public Bitmap FloodFill(Bitmap src, Point location, Color fillColor, int threshould)
        {
            try
            {
                Bitmap a = new Bitmap(src);
                int w = a.Width;
                int h = a.Height;
                Stack<Point> fillPoints = new Stack<Point>(w * h);
                System.Drawing.Imaging.BitmapData bmpData = a.LockBits(new Rectangle(0, 0, a.Width, a.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                IntPtr ptr = bmpData.Scan0;
                int stride = bmpData.Stride;
                int bytes = bmpData.Stride * a.Height;
                byte[] grayValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, grayValues, 0, bytes);
                byte[] temp = (byte[])grayValues.Clone();
                Color backColor = Color.FromArgb(temp[location.X * 3 + 2 + location.Y * stride], temp[location.X * 3 + 1 + location.Y * stride], temp[location.X * 3 + location.Y * stride]);
                int gray = (int)((backColor.R + backColor.G + backColor.B) / 3);
                if (location.X < 0 || location.X >= w || location.Y < 0 || location.Y >= h) return null;
                fillPoints.Push(new Point(location.X, location.Y));
                int[,] mask = new int[w, h];
                while (fillPoints.Count > 0)
                {
                    Point p = fillPoints.Pop();
                    mask[p.X, p.Y] = 1;
                    temp[3 * p.X + p.Y * stride] = (byte)fillColor.B;
                    temp[3 * p.X + 1 + p.Y * stride] = (byte)fillColor.G;
                    temp[3 * p.X + 2 + p.Y * stride] = (byte)fillColor.R;
                    if (p.X > 0 && (Math.Abs(gray - (int)((temp[3 * (p.X - 1) + p.Y * stride] + temp[3 * (p.X - 1) + 1 + p.Y * stride] + temp[3 * (p.X - 1) + 2 + p.Y * stride]) / 3)) < threshould) && (mask[p.X - 1, p.Y] != 1))
                    {
                        temp[3 * (p.X - 1) + p.Y * stride] = (byte)fillColor.B;
                        temp[3 * (p.X - 1) + 1 + p.Y * stride] = (byte)fillColor.G;
                        temp[3 * (p.X - 1) + 2 + p.Y * stride] = (byte)fillColor.R;
                        fillPoints.Push(new Point(p.X - 1, p.Y));
                        mask[p.X - 1, p.Y] = 1;
                    }
                    if (p.X < w - 1 && (Math.Abs(gray - (int)((temp[3 * (p.X + 1) + p.Y * stride] + temp[3 * (p.X + 1) + 1 + p.Y * stride] + temp[3 * (p.X + 1) + 2 + p.Y * stride]) / 3)) < threshould) && (mask[p.X + 1, p.Y] != 1))
                    {
                        temp[3 * (p.X + 1) + p.Y * stride] = (byte)fillColor.B;
                        temp[3 * (p.X + 1) + 1 + p.Y * stride] = (byte)fillColor.G;
                        temp[3 * (p.X + 1) + 2 + p.Y * stride] = (byte)fillColor.R;
                        fillPoints.Push(new Point(p.X + 1, p.Y));
                        mask[p.X + 1, p.Y] = 1;
                    }
                    if (p.Y > 0 && (Math.Abs(gray - (int)((temp[3 * p.X + (p.Y - 1) * stride] + temp[3 * p.X + 1 + (p.Y - 1) * stride] + temp[3 * p.X + 2 + (p.Y - 1) * stride]) / 3)) < threshould) && (mask[p.X, p.Y - 1] != 1))
                    {
                        temp[3 * p.X + (p.Y - 1) * stride] = (byte)fillColor.B;
                        temp[3 * p.X + 1 + (p.Y - 1) * stride] = (byte)fillColor.G;
                        temp[3 * p.X + 2 + (p.Y - 1) * stride] = (byte)fillColor.R;
                        fillPoints.Push(new Point(p.X, p.Y - 1));
                        mask[p.X, p.Y - 1] = 1;
                    }
                    if (p.Y < h - 1 && (Math.Abs(gray - (int)((temp[3 * p.X + (p.Y + 1) * stride] + temp[3 * p.X + 1 + (p.Y + 1) * stride] + temp[3 * p.X + 2 + (p.Y + 1) * stride]) / 3)) < threshould) && (mask[p.X, p.Y + 1] != 1))
                    {
                        temp[3 * p.X + (p.Y + 1) * stride] = (byte)fillColor.B;
                        temp[3 * p.X + 1 + (p.Y + 1) * stride] = (byte)fillColor.G;
                        temp[3 * p.X + 2 + (p.Y + 1) * stride] = (byte)fillColor.R;
                        fillPoints.Push(new Point(p.X, p.Y + 1));
                        mask[p.X, p.Y + 1] = 1;
                    }
                }
                fillPoints.Clear();
                grayValues = (byte[])temp.Clone();
                System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr, bytes);
                a.UnlockBits(bmpData);
                return a;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                return null;
            }
        }

    }
}