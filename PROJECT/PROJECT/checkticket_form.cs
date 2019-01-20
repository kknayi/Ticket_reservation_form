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
    public partial class checkticket_form : Form
    {
        SqlDataAdapter adapter;
        DataTable table;
        string connectionString = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";
        public checkticket_form()
        {
            InitializeComponent();
        }
        public checkticket_form(string uname)
        {
            InitializeComponent();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            adapter = new SqlDataAdapter("select * from Order_info where UName= '" + uname + "'", conn);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            head_2();
            conn.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index; //获取选中行的行号
            string planeid = dataGridView1.Rows[index].Cells[2].Value.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            adapter = new SqlDataAdapter("select * from Ticket_info where PlaneID= '" + planeid + "'", conn);
            table = new DataTable();
            adapter.Fill(table);
            textBox6.Text = table.Rows[0]["PlaneID"].ToString();
            textBox7.Text = table.Rows[0]["Startplace"].ToString();
            textBox8.Text = table.Rows[0]["Endplace"].ToString();
            textBox9.Text = table.Rows[0]["Startdate"].ToString();
            textBox10.Text = table.Rows[0]["Enddate"].ToString();
            textBox11.Text = table.Rows[0]["Weekday"].ToString();
            textBox12.Text = table.Rows[0]["Price"].ToString();
            conn.Close();
        }

        private void head_2()
        {
            dataGridView1.Columns["Ordernum"].HeaderText = "订单号";
            dataGridView1.Columns["UName"].HeaderText = "用户名";
            dataGridView1.Columns["PlaneID"].HeaderText = "航班号";
            dataGridView1.Columns["Price"].HeaderText = "机票价格";
            dataGridView1.Columns["Seat"].HeaderText = "座位号";
        }
    }
}
