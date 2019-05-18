using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Contact
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)  {  }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("Name", 60);
            listView1.Columns.Add("Phone Number", 90);
            listView1.Columns.Add("Sex", 50);
            listView1.Columns.Add("E-mail", 100);
            listView1.Columns.Add("QQ", 80);
            listView1.View = View.Details;
            toolStripStatusLabel1.Text = "Function: Select";
            dataGridView1.Visible = true;
        }
        SqlConnection conn = new SqlConnection();
        string str_con= "server=.;database=Contact;integrated security=SSPI";
        private void button1_Click(object sender, EventArgs e)
        {
            //conn.ConnectionString = "Data Source = .\\SQL2012 ; Integrated Security = true ; Initial Catalog = Contact";
            conn.ConnectionString =str_con;
            conn.Open();
            if (conn.State == ConnectionState.Open)
                MessageBox.Show("Connect Successfull");
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel1.Text == "Function: Select")
            {
                SqlDataAdapter da = new SqlDataAdapter(textBox1.Text, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            if (toolStripStatusLabel1.Text == "Function: Add")
            {
                string sql = textBox1.Text;
                int i = changeSqlData(sql);
                if (i == 0) MessageBox.Show("添加失败", "提示：");
                else
                {
                    MessageBox.Show("添加成功", "提示：");
                    button3_Click(sender, e);
                }
            }
            if (toolStripStatusLabel1.Text == "Function: Delete")
            {
                string sql = textBox1.Text;
                int i=changeSqlData(sql);
                if (i == 0)
                {
                    MessageBox.Show("删除失败", "提示：");
                }
                else
                {
                    MessageBox.Show("删除成功", "提示：");
                    button3_Click(sender, e);
                }
            }
            if (toolStripStatusLabel1.Text == "Function: Update")
            {
                string sql = textBox1.Text;
                int i = changeSqlData(sql);
                if (i == 0) MessageBox.Show("修改失败", "提示：");
                else
                {
                    MessageBox.Show("修改成功", "提示：");
                    button3_Click(sender, e);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText ="select * from Contact";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem lt = new ListViewItem();
                lt.Text = dr["name"].ToString();
                lt.SubItems.Add(dr["phonenumber"].ToString());
                lt.SubItems.Add(dr["sex"].ToString());
                lt.SubItems.Add(dr["email"].ToString());
                lt.SubItems.Add(dr["qq"].ToString());
                listView1.Items.Add(lt);
            }
            dr.Close();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void deleteDToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void searchSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Function: Select";
        }
        public int changeSqlData(String sql)
        {
            using(SqlConnection con=new SqlConnection(str_con))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                int i=cmd.ExecuteNonQuery();//执行操作返回影响行数（）
                con.Close();
                return i;
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Function: Add";
            dataGridView1.Visible = false;
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Function: Delete";
            dataGridView1.Visible = false;
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Function: Select";
            dataGridView1.Visible = true;
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
              toolStripStatusLabel1.Text = "Function: Update";
              dataGridView1.Visible = false;
        }
    }
}
