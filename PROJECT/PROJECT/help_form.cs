using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECT
{
    public partial class help_form : Form
    {
        public help_form()
        {
            InitializeComponent();
            textBox1.Select(textBox1.TextLength, 0);
        }
    }
}
