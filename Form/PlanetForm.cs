using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using AstronomicDirectory;

namespace Form
{
    
    public partial class PlanetForm : System.Windows.Forms.Form
    {
        private readonly Planet planet;
        private MoonControl moon;
        public PlanetForm(Planet planet)
        {

            this.planet = planet;// ?? new Planet(star);
            //moon = new MoonControl(this.planet);
            InitializeComponent();
            LoadPlanet();
            Moons.MouseDoubleClick += Planets_MouseDoubleClick;
            //planetForm = new PlanetForm(star) {Owner = this};
            Moons.SelectedIndexChanged += (sender, args) => { button2.Enabled = Moons.SelectedItem is Planet; };
            button2.Click += button2_Click;
            comboBox1.Text = comboBox1.Items[0] as string;
            comboBox2.Text = comboBox2.Items[0] as string;
            button2.Click += (sender, args) => { };
            addMoonButton.Click += (sender, args) =>
            {
                InitializePlanet(); 
                //if (mainTableLayoutPanel.Controls.Contains(moon))
                //    mainTableLayoutPanel.Controls.Remove(moon);
                //else
                moon = new MoonControl(new Moon(planet), this);
                mainTableLayoutPanel.Controls.Add(moon, 1, 0);
            };
            Closing += PlanetForm_Closing;
        }

        private void Planets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.Moons.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var item = (Moon)Moons.Items[index];
                //var ff = new PlanetForm(item);//();// { Flight = item };
                moon = new MoonControl(item, this);
                mainTableLayoutPanel.Controls.Add(moon, 1, 0);

                //if (ff.ShowDialog(this) == DialogResult.OK)
                //{
                //}
            }

        }

        private void PlanetForm_Closing(object sender, CancelEventArgs e)
        {
            InitializePlanet();

            DialogResult = DialogResult.OK;
            //SaveAndClose();
            //e.Cancel = true;
            //button1_Click();
            //(Owner as StarForm1).Planets.Refresh();
            //Invalidate();
        }

        void LoadPlanet()
        {
            nameTextBox.Text = planet.Name;
            dateTimePicker1.Value = planet.InventingDate;
            pictureBox1.Image = planet.Photo;
            radiusTextBox.Text = planet.Radius.ToString();
            distanceTextBox.Text = planet.MiddleDistance.Value.ToString();
            planet.HasAtmosphere = checkBox1.Checked;
            switch (planet.MiddleDistance.Unit)
            {
                case UnitType.Kilometers:
                    comboBox1.Text = "км";
                    break;
                case UnitType.LightYears:
                    comboBox1.Text = "св. г.";
                    break;
                case UnitType.AstronomicUnits:
                    comboBox1.Text = "а.е.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (planet.Type)
            {
                case PlanetType.Tought:
                    this.comboBox2.Text = "Каменистая планета";
                    break;
                case PlanetType.Gas:
                    this.comboBox2.Text = "Газовый гигант";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            checkBox1.Checked = planet.HasAtmosphere;
            Moons.Items.AddRange(planet.Moons.ToArray());
            Moons.Refresh();
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
            planet.Moons.AddRange(Moons.Items.Cast<Moon>());
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
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Moons.SelectedItem is Planet)
            {
                Moons.Items.Remove(Moons.SelectedItem);
            }
        }

        //private void SaveAndClose()
        //{
        //    InitializePlanet();
        //    //(Owner as StarForm1).Planets.Items.Add(planet);
        //    //(Owner as StarForm1).Planets.Refresh();

        //    //Owner.Invalidate();

        //}
    }
}
