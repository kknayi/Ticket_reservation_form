/*可进行的改进：
 * 1. 加入钱包功能，附加充值操作，如果钱够则购买机票，否则不可购买——充值？？？
 * 6. 窗体美化？？？
 * 8. 统计航班总营业额
 * 9. 推荐航班
 */

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
    public partial class begin_form : Form
    {
        //数据桥接器
        SqlDataAdapter adapter;
        //临时数据存储表
        DataTable table;
        //创建本地数据库连接字段
        string connectionString = @"Data Source = .; Initial Catalog = Csharp_data; User ID = sa; Pwd = 123456";

        public begin_form()
        {
            InitializeComponent();
        }

        //窗体加载事件，更新航班信息表
        private void begin_form_Load(object sender, EventArgs e)
        {
            //建立与本地数据库之间的连接
            SqlConnection conn = new SqlConnection(connectionString);
            //连接数据库
            conn.Open();
            //使用数据桥接器更新数据连接数据库
            adapter = new SqlDataAdapter("select * from Ticket_info", conn);
            //创建临时存储表对象
            table = new DataTable();
            //用adapter数据桥接器选中的数据库表中的信息更新临时数据库存储表
            adapter.Fill(table);
            //更新窗体中的数据信息
            dataGridView1.DataSource = table;
            //更新数据表中的表头文本
            head();
            //关闭连接
            conn.Close();
            //将time时间控件设置为可用
            timer1.Enabled = true;
        }

        //表头文本信息替换函数
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

        //用户注册功能
        private void 用户注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            register_form reg = new register_form();
            reg.ShowDialog();
        }

        //用户登录功能
        private void 用户登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login_form log = new login_form();
            log.ShowDialog();
        }

        //管理员登录功能
        private void 管理员登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alogin_form admin = new alogin_form();
            admin.ShowDialog();
        }

        //帮助功能
        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help_form help = new help_form();
            help.ShowDialog();
        }

        //退出功能
        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //timer控件显示系统当前时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "当前系统时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
