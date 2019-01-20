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
    public partial class complate_form : Form
    {
        SqlDataAdapter adapter;
        DataTable table;
        string connectionString = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";

        public string UName { get; set; }
        public complate_form()
        {
            InitializeComponent();
        }
        public complate_form(string uname)
        {
            InitializeComponent();
            UName = uname;
            textBox1.Text = UName;

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            adapter = new SqlDataAdapter("select * from User_login where UName= '" + UName + "'", conn);
            table = new DataTable();
            adapter.Fill(table);
            textBox1.Text = table.Rows[0]["UName"].ToString();
            textBox1.Enabled = false;
            textBox2.Text = table.Rows[0]["name"].ToString();
            textBox3.Text = table.Rows[0]["IDcard"].ToString();
            textBox4.Text = table.Rows[0]["phone"].ToString();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update User_login set name= '" + textBox2.Text + "', IDcard= '" + textBox3.Text + "', phone= '" + textBox4.Text + "' where UName='" + textBox1.Text + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("个人信息更新成功！");
            }
            catch (Exception err)
            {
                MessageBox.Show("好像出错了……" + err.Message);
            }
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox6.Text.Length <= 6 || textBox6.Text.Length > 20)
            {
                errorProvider1.SetError(textBox6, "密码非法！密码不能少于6个字符，且不能大于20个字符！");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select count(*) from User_login where UName='" + UName + "' and UPassword='" + textBox5.Text + "'";
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    if (textBox6.Text != "")
                    {
                        if (textBox6.Text == textBox7.Text)
                        {
                            SqlConnection conn_ = new SqlConnection(connectionString);
                            conn_.Open();
                            SqlCommand cmd_ = new SqlCommand("update User_login set UPassword= '" + textBox6.Text + "' where UName= '" + UName + "'", conn_);
                            cmd_.ExecuteNonQuery();
                            conn_.Close();

                            MessageBox.Show("设置新密码成功！");
                            textBox5.Text = "";
                            textBox6.Text = "";
                            textBox7.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("密码设置不一致，请重新设置！");
                            textBox6.Text = "";
                            textBox7.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("未设置新密码，请输入新的密码……");
                    }
                }
                conn.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show("好像出错了……" + err.Message);
            }
        }

    }
}
