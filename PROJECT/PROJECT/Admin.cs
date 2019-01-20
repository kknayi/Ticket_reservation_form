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

namespace PROJECT
{
    public partial class Admin : Form
    {
        SqlDataAdapter adapter;
        DataTable table;
        string connectionString = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";

        public Admin()
        {
            InitializeComponent();
            timer1.Enabled = true;
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            adapter = new SqlDataAdapter("select * from Ticket_info", conn);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            head();
            conn.Close();
        }

        private void head()
        {
            dataGridView1.Columns["PlaneID"].HeaderText = "航班号";
            dataGridView1.Columns["Startplace"].HeaderText = "出发地";
            dataGridView1.Columns["Endplace"].HeaderText = "到达地";
            dataGridView1.Columns["Startdate"].HeaderText = "出发日期";
            dataGridView1.Columns["Enddate"].HeaderText = "到达日期";
            dataGridView1.Columns["weekday"].HeaderText = "星期";
            dataGridView1.Columns["contents"].HeaderText = "余票数量";
            dataGridView1.Columns["price"].HeaderText = "机票价格";
        }

        private void head_2()
        {
            dataGridView2.Columns["UName"].HeaderText = "用户名";
            dataGridView2.Columns["name"].HeaderText = "姓名";
            dataGridView2.Columns["IDcard"].HeaderText = "身份证";
            dataGridView2.Columns["phone"].HeaderText = "电话";
        }

        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            SqlConnection conn_ = new SqlConnection(connectionString);
            conn_.Open();
            if (tabControl1.SelectedTab.Name == "tabPage1")
            {
                adapter = new SqlDataAdapter("select * from Ticket_info", conn_);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head();
                conn_.Close();
            }
            else if (tabControl1.SelectedTab.Name == "tabPage2")
            {
                adapter = new SqlDataAdapter("select * from User_login", conn_);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView2.DataSource = table;
                dataGridView2.Columns[1].Visible = false;
                head_2();
                conn_.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要修改航班信息吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int index = dataGridView1.CurrentRow.Index; //获取选中行的行号
                    string planeid = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    string startplace = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    string endplace = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    string startdate = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    string enddate = dataGridView1.Rows[index].Cells[4].Value.ToString();
                    string weekday = dataGridView1.Rows[index].Cells[5].Value.ToString();
                    decimal price = decimal.Parse(dataGridView1.Rows[index].Cells[7].Value.ToString());
                    addchange_form addorchange = new addchange_form(planeid, startplace, endplace, startdate, enddate, weekday, price, true);
                    addorchange.ShowDialog();
                }
                catch (Exception err)
                {
                    MessageBox.Show("好像出错了……" + err.Message);
                }
                SqlConnection conn_ = new SqlConnection(connectionString);
                conn_.Open();
                adapter = new SqlDataAdapter("select * from Ticket_info", conn_);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head();
                conn_.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要增加航班信息吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    addchange_form addorchange = new addchange_form();
                    addorchange.ShowDialog();
                }
                catch (Exception err)
                {
                    MessageBox.Show("好像出错了……" + err.Message);
                }
                SqlConnection conn_ = new SqlConnection(connectionString);
                conn_.Open();
                adapter = new SqlDataAdapter("select * from Ticket_info", conn_);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head();
                conn_.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除该航班信息吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int index = dataGridView1.CurrentRow.Index; //获取选中行的行号
                    string planeid = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "delete Ticket_info where PlaneID= '" + planeid + "'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("已删除该航班！");
                    conn.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show("好像出错了……" + err.Message);
                }
            }
            SqlConnection conn_ = new SqlConnection(connectionString);
            conn_.Open();
            adapter = new SqlDataAdapter("select * from Ticket_info", conn_);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            head();
            conn_.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel2.Text = "当前系统时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = dataGridView2.CurrentRow.Index; //获取选中行的行号
            string uname = dataGridView2.Rows[index].Cells[0].Value.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string pass = "1234567";
            SqlCommand cmd = new SqlCommand("update User_login set UPassword= '" + pass + "' where UName= '" + uname + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("重置当前用户密码成功！密码：1234567");
        }
    }
}
