using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form
{
    public partial class StarForm1 : System.Windows.Forms.Form
    {
        public StarForm1()
        {
            InitializeComponent();
        }

        private void planetButton_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnStyles[2].Width = tableLayoutPanel1.ColumnStyles[2].Width == 0 ? 50 : 0;
        }
    }
}
