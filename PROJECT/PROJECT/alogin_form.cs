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
    public partial class alogin_form : Form
    {
        public string UName { get; set; }
        string connection = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";

        public alogin_form()
        {
            InitializeComponent();
        }

        //登录
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connection);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select count(*) from Admin_login where AName='" + textBox1.Text + "' and APassword='" + textBox2.Text + "'";
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    UName = textBox1.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                    Admin user = new Admin();
                    user.ShowDialog();
                }
                else
                {
                    MessageBox.Show("登录名或密码错误！请重新输入");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox1.Focus();
                }
                conn.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
