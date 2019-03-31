using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AstronomicDirectory;

namespace Form
{
    
    public partial class StarForm1 : System.Windows.Forms.Form
    {
        //private Star star;
        //private readonly Panel planetsBox = new Panel();
        //private readonly Label planetsLabel = new Label{Text = "Планета"};
        //private readonly ListBox planetsList = new ListBox();
        //private readonly Button addPlanetButton = new Button{Text = "Добавить планету"};
        public StarForm1()
        {
            InitializeComponent();
            //planetsLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            //addPlanetButton.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            //planetsList.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            ////planetsList.Dock = DockStyle.Fill;
            //planetsLabel.Location = new Point(0, 0);
            //planetsLabel.Height = 25;
            //addPlanetButton.Height = 25;

            //addPlanetButton.Location = new Point(0, planetsLabel.Bottom);
            //planetsList.Location = new Point(0, addPlanetButton.Bottom);


            //planetsBox.Controls.Add(planetsLabel);
            //planetsBox.Controls.Add(addPlanetButton);
            //planetsBox.Controls.Add(planetsList);
            //mainTableLayoutPanel.Controls.Add(planetsBox, 1, 0);

            //planetLabel.Location
        }

        private void planetButton_Click(object sender, EventArgs e)
        {
            mainTableLayoutPanel.ColumnStyles[1].Width = mainTableLayoutPanel.ColumnStyles[1].Width == 0 ? 75 : 0;
        }

        private void photoButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
                pictureBox1.Image = Image.FromFile(ofd.FileName);
        }
    }
}
