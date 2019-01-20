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
using System.Collections;

namespace PROJECT
{
    public partial class User : Form
    {
        SqlDataAdapter adapter;
        DataTable table;
        public string UName { get; set; }
        int CNT;
        string connectionString = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";

        #region //tab切换改变数据库表

        //切换tab按钮，改变相应的数据库表
        private void tabControl1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tabPage2")
            {
                label13.Text = "票务信息";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                adapter = new SqlDataAdapter("select * from Order_info", conn);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head_2();
                conn.Close();

                //拒绝选中textbox
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
            }
            else if (tabControl1.SelectedTab.Name == "tabPage1")
            {
                label13.Text = "航班信息";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                adapter = new SqlDataAdapter("select * from Ticket_info", conn);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head();
                conn.Close();
            }
        }

        #endregion

        #region //订票功能

        public User()
        {
            InitializeComponent();
            label13.Text = "航班信息";
            //timer1.Enabled = true;
            //this.toolStripStatusLabel1.Text = ("当前状态：" + UName).ToString();
        }
        public User(string uname, int cnt)
        {
            InitializeComponent();
            label13.Text = "航班信息";
            timer1.Enabled = true;
            this.toolStripStatusLabel1.Text = ("当前状态：用户 [" + uname + "]").ToString();
            UName = uname;
            CNT = cnt;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            adapter = new SqlDataAdapter("select * from Ticket_info", conn);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            head();
            conn.Close();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
        }

        //订票开始
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要订票吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (CNT == 1) {
                    try
                    {
                        int index = dataGridView1.CurrentRow.Index; //获取选中行的行号
                        SqlConnection conn = new SqlConnection(connectionString);
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("update Ticket_info set Contents=Contents-1 where PlaneID='" + dataGridView1.Rows[index].Cells[0].Value.ToString() + "'", conn);
                        cmd.ExecuteNonQuery();
                        int content = int.Parse(dataGridView1.Rows[index].Cells[6].Value.ToString());//机票余量
                        string planeid = dataGridView1.Rows[index].Cells[0].Value.ToString();
                        decimal Price = decimal.Parse(dataGridView1.Rows[index].Cells[7].Value.ToString());
                        if (content <= 0)
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "update Ticket_info set Contents=Contents+1 where PlaneID='" + dataGridView1.Rows[index].Cells[0].Value.ToString() + "'";
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("对不起，没有票啦！请选乘其他航班！");
                        }
                        else
                        {
                            /*
                             * 加功能（选座位、给出相关信息）
                             * 当前是自动生成座位号
                             */
                            int seat = 100 - content + 1;
                            char[] arrChar = new char[] { 'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v', 'w', 'z', 'y', 'x', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U', 'W', 'X', 'Y', 'Z' };
                            StringBuilder num = new StringBuilder();
                            Random rnd = new Random(DateTime.Now.Millisecond);
                            for (int i = 0; i < 5; i++)
                            {
                                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
                            }
                            string ordernum = num.ToString();
                            MessageBox.Show("购买成功！您的座位号是：" + seat.ToString() + "\n您的订单号是：" + ordernum);
                            Addinfo(ordernum, UName, planeid, Price, seat);
                            SqlConnection conn_ = new SqlConnection(connectionString);
                            conn_.Open();
                            adapter = new SqlDataAdapter("select * from Ticket_info", conn_);
                            table = new DataTable();
                            adapter.Fill(table);
                            dataGridView1.DataSource = table;
                            head();
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

        //添加订票信息
        private void Addinfo(string ordernum, string uname, string planeid, decimal price, int seat)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into Order_info(Ordernum,UName,PlaneID,Price,Seat) values('" + ordernum + "','" + uname + "','" + planeid + "','" + price + "','" + seat + "')", conn);
            cmd.ExecuteNonQuery();
        }

        //修改表头标题
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

        //按照地点筛选
        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        //按照航班号筛选
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = true;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        //按照价格筛选
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                adapter = new SqlDataAdapter("select * from Ticket_info where Startplace='" + textBox1.Text + "' and Endplace='" + textBox2.Text + "'", conn);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head();
                conn.Close();
            }
            else if (textBox3.Text != "")
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                adapter = new SqlDataAdapter("select * from Ticket_info where PlaneID= '" + textBox3.Text + "'", conn);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head();
                conn.Close();
            }
            else if (textBox4.Text != "" && textBox5.Text != "")
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                adapter = new SqlDataAdapter("select * from Ticket_info where Price<= '" + textBox4.Text + "' and Price>='" + textBox5.Text + "'", conn);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
                head();
                conn.Close();
            }
            else
            {
                MessageBox.Show("筛选有误！");
            }
        }

        #endregion

        #region //退票功能

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退票吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int index = dataGridView1.CurrentRow.Index; //获取选中行的行号
                    string ordernum = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    string planeid = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    decimal price = decimal.Parse(dataGridView1.Rows[index].Cells[3].Value.ToString());
                    decimal seat = decimal.Parse(dataGridView1.Rows[index].Cells[4].Value.ToString());
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "delete Order_info where Ordernum='" + ordernum + "' and UName='" + UName + "' and PlaneID='" + planeid + "' and Price='" + price + "' and Seat='" + seat + "'";
                    int i = cmd.ExecuteNonQuery();
                    cmd.CommandText = "update Ticket_info set contents=contents+1 where PlaneID='" + planeid + "'";
                    int n = cmd.ExecuteNonQuery();
                    if (i == 1 && n == 1)
                    {
                        MessageBox.Show("退票成功！");
                        SqlConnection conn_ = new SqlConnection(connectionString);
                        conn_.Open();
                        adapter = new SqlDataAdapter("select * from Order_info", conn_);
                        table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                        head_2();
                        conn_.Close();
                    }
                    else
                        MessageBox.Show("对不起，你未预定此航班，无需退票");
                    conn.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show("好像出错了。。。" + err.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
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

        #endregion

        private void 注销EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要注销当前账户吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Dispose();
        }

        private void 信息修改ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            complate_form com = new complate_form(UName);
            com.ShowDialog();
        }

        private void 票务查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkticket_form tick = new checkticket_form(UName);
            tick.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel2.Text = "当前系统时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
