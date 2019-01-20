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
    public partial class login_form : Form
    {
        //用户名接口
        public string UName { get; set; }
        public string YANZHENG { get; set; }
        SqlDataAdapter adapter;
        DataTable table;
        string connection = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";
        public login_form()
        {
            InitializeComponent();
        }

        //登录按钮
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select count(*) from User_login where UName='" + textBox1.Text + "' and UPassword='" + textBox2.Text + "'";
                if ((int)cmd.ExecuteScalar() == 1 && textBox3.Text == YANZHENG)
                {
                    UName = textBox1.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                    User user = new User(UName, 1);
                    user.ShowDialog();
                }
                else if (textBox3.Text != YANZHENG)
                {
                    MessageBox.Show("验证码有误，退出……");
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("登录名或密码错误！请重新输入……");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                }
                conn.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show("似乎出了点问题……" + err.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() == "")
            {
                MessageBox.Show("未输入用户名，请输入后再进行相关操作……");
                return;
            }
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select count(*) from User_login where UName='" + textBox1.Text + "' and UPassword='" + textBox2.Text + "'";
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    UName = textBox1.ToString();
                    SqlConnection conn_ = new SqlConnection(connection);
                    conn_.Open();
                    adapter = new SqlDataAdapter("select * from User_login where UName= '" + textBox1.Text + "' and UPassword='" + textBox2.Text + "'", conn_);
                    table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                    string Email = dataGridView1.Rows[0].Cells[2].Value.ToString();
                    if (Email == "")
                    {
                        MessageBox.Show("邮箱未添加，请添加后使用……");
                    }
                    string yanzheng = Email_send.createrandom(6);
                    YANZHENG = yanzheng;
                    ////////////////////////////
                    textBox4.Text = yanzheng;
                    ///////////////////////////
                    Email_send.Creat(Email, yanzheng);
                }
                else
                {
                    MessageBox.Show("登录名或密码错误！请重新输入……");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("似乎出了点问题……" + err.Message);
            }
        }
    }
}
