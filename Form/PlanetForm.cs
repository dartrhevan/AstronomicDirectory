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
    
    public partial class PlanetForm : System.Windows.Forms.Form
    {
        private Planet planet;
        public PlanetForm(Star star)
        {
            this.planet = new Planet(star);
            InitializeComponent();
            comboBox1.Text = comboBox1.Items[0] as string;
            comboBox2.Text = comboBox2.Items[0] as string;
            addMoonButton.Click += (sender, args) => { InitializePlanet(); };
            Closing += PlanetForm_Closing;
        }

        private void PlanetForm_Closing(object sender, CancelEventArgs e)
        {
            SaveAndClosre();
            e.Cancel = true;
            //button1_Click();
        }

        private void InitializePlanet()
        {
            planet.Name = nameTextBox.Text;
            planet.InventingDate = dateTimePicker1.Value;
            planet.Photo = pictureBox1.Image;
            uint temp;
            if (uint.TryParse(radiusTextBox.Text, out temp))
                planet.Radius = temp;
            if (uint.TryParse(distanceTextBox.Text, out temp))
            {
                UnitType t;
                switch (comboBox1.Text)
                {
                    case "км":
                        t = UnitType.Kilometers;
                        break;
                    case "св. г.":
                        t = UnitType.LightYears;
                        break;
                    case "а.е.":
                        t = UnitType.AstronomicUnits;
                        break;
                    default: throw new ArgumentException();
                }
                planet.MiddleDistance = new Distance(temp, t);
            }
            planet.HasAtmosphere = checkBox1.Checked;
            PlanetType type;
            switch (comboBox2.Text)
            {
                case "Газовый гигант":
                    type = PlanetType.Gas;
                    break;
                case "Каменистая планета":
                    type = PlanetType.Tought;
                    break;
                default: throw new ArgumentException();
            }
            planet.Type = type;
        }

        private void planetsButton_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            SaveAndClosre();
        }

        private void SaveAndClosre()
        {
            InitializePlanet();
            (Owner as StarForm1).Planets.Items.Add(planet);
            this.Hide();
        }
    }
}
