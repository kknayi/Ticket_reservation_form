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
using System.Text.RegularExpressions;

namespace PROJECT
{
    public partial class register_form : Form
    {
        //用户名接口
        public string UName { get; set; }
        //用户密码接口
        public string UPassword { get; set; }
        //用户重复密码接口
        public string UPassword_2 { get; set; }
        //用户邮箱地址
        public string Email { get; set; }
        //标记输入注册信息是否正确
        public int flag1, flag2, flag3, flag4;
        string connectionString = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";
        
        //窗体加载事件
        public register_form()
        {
            InitializeComponent();
            flag1 = 1; flag2 = 1; flag3 = 1; flag4 = 1;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 0;
            toolTip1.SetToolTip(textBox1, "用户名字符数3 - 20，且只能用字母，数字!");
            toolTip1.SetToolTip(textBox2, "密码不能少于6个字符！");
            toolTip1.SetToolTip(textBox3, "请再输入一次密码！");
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        //验证用户名是否大于2个字符，并且必须是数字和字母的组合
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int flag = 1;
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                if (char.IsLetterOrDigit(textBox1.Text[i]) == false)
                {
                    flag = 0;
                }
                else
                {
                    flag = 1;
                }
            }
            if (textBox1.Text.Length <= 2 || textBox1.Text.Length > 20 || flag == 0)
            {
                errorProvider1.SetError(textBox1, "用户名非法！用户名字符数3 - 20，且只能用字母，数字!");
                flag2 = 0;
            }
            else
            {
                errorProvider1.Clear();
                flag2 = 1;
            }
        }

        //验证密码是否大于6个字符
        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox2.Text.Length <= 6 || textBox2.Text.Length > 20)
            {
                errorProvider2.SetError(textBox2, "密码非法！密码不能少于6个字符，且不能大于20个字符！");
                flag1 = 0;
            }
            else
            {
                errorProvider2.Clear();
                flag1 = 1;
            }
        }

        //验证确认密码是否一致
        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox3.Text != textBox2.Text || textBox3.Text.Length <= 6)
            {
                errorProvider3.SetError(textBox3, "密码非法！密码不一致！");
                flag3 = 0;
            }
            else
            {
                errorProvider3.Clear();
                flag3 = 1;
            }
        }

        //验证邮箱是否符合规范
        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {
            if (!isValidEmail(textBox4.Text))
            {
                errorProvider4.SetError(textBox4, "邮箱非法！系统需使用QQ邮箱(@qq.com)");
                flag4 = 0;
            }
            else
            {
                errorProvider4.Clear();
                flag4 = 1;
            }
        }

        public bool isValidEmail(string strln)
        {
            return Regex.IsMatch(strln, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)" + @"|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        //取消注册按钮
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //将注册用户数据添加至用户表
        private void button1_Click_1(object sender, EventArgs e)
        {
            UName = textBox1.Text;
            UPassword = textBox2.Text;
            Email = textBox4.Text;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    if (flag1 == 1 && flag2 == 1 && flag3 == 1 && flag4 == 1 && textBox2.Text == textBox3.Text)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        SqlDataAdapter adapter = new SqlDataAdapter("select * from User_login where UName='" + textBox1.Text + "'", conn);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        if (table.Rows.Count == 0)
                        {
                            cmd.CommandText = "insert into User_login(UName,UPassword,Email) values(@UName,@UPassword,@Email)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("UName", UName));
                            cmd.Parameters.Add(new SqlParameter("UPassword", UPassword));
                            cmd.Parameters.Add(new SqlParameter("Email", Email));
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("恭喜您，注册成功");
                            this.Dispose();
                        }
                        else
                        {
                            MessageBox.Show("对不起，此用户名已被注册！");
                            textBox1.Text = textBox2.Text = textBox3.Text = "";
                            textBox1.Focus();
                            flag1 = 1;
                        }
                    }
                    else
                    {
                        MessageBox.Show("似乎出了点问题……请尝试重新注册！");
                        textBox1.Text = textBox2.Text = textBox3.Text = "";
                        textBox1.Focus();
                        flag1 = 1;
                    }

                }
                catch (Exception err)
                {
                    MessageBox.Show("似乎出错了……" + err.Message);
                }
            }
        }
    }
}
