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
    public partial class addchange_form : Form
    {
        //SqlDataAdapter adapter;
        //DataTable table;
        string connectionString = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";

        public addchange_form()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        public addchange_form(string planeid, string startplace, string endplace, string startdate, string enddate, string weekday, decimal price, bool flag)
        {
            InitializeComponent();
            textBox6.Text = planeid;
            textBox7.Text = startplace;
            textBox8.Text = endplace;
            textBox9.Text = startdate;
            textBox10.Text = enddate;
            textBox11.Text = weekday;
            textBox12.Text = price.ToString();
            if (flag)
            {
                textBox6.Enabled = false;
                button2.Enabled = false;
            }
        }

        //修改按钮
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("update Ticket_info set Startplace= '" + textBox7.Text + "', Endplace= '" + textBox8.Text + "', Startdate= '" + textBox9.Text + "', Enddate= '" + textBox10.Text + "', Weekday= '" + textBox11.Text + "', Price= '" + textBox12.Text + "' where PlaneID='" + textBox6.Text + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("航班信息更新成功！");
            }
            catch (Exception err)
            {
                MessageBox.Show("好像出错了……" + err.Message);
            }
        }

        //添加按钮
        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into Ticket_info(PlaneID, Startplace, Endplace, Startdate, Enddate, Weekday, Contents, Price) values('" + textBox6.Text + "','" + textBox7.Text + "','" + textBox8.Text + "','" + textBox9.Text + "','" + textBox10.Text + "','" + textBox11.Text + "','" +  100 + "','" + textBox12.Text + "')", conn);
            cmd.ExecuteNonQuery();
            this.DialogResult = DialogResult.OK;
            MessageBox.Show("已添加航班信息！");
        }
    }
}
