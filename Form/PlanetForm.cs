﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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
            InitializeComponent();
            //dateTimePicker1.MinDate = new DateTime(0, 0, 0);
            LoadPlanet();
            Moons.MouseDoubleClick += Planets_MouseDoubleClick;
            Moons.SelectedIndexChanged += (sender, args) => { button2.Enabled = Moons.SelectedItem is Planet; };
            button2.Click += button2_Click;
            comboBox1.Text = comboBox1.Items[0] as string;
            comboBox2.Text = comboBox2.Items[0] as string;
            button2.Click += (sender, args) => { };
            addMoonButton.Click += (sender, args) =>
            {
                InitializePlanet(); 
                moon = new MoonControl(new Moon(planet), this);
                mainTableLayoutPanel.Controls.Add(moon, 1, 0);
            };
            Closing += PlanetForm_Closing;
            button2.Enabled = false;
        }

        private void Planets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.Moons.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var item = (Moon)Moons.Items[index];
                moon = new MoonControl(item, this);
                mainTableLayoutPanel.Controls.Add(moon, 1, 0);
            }

        }

        private void PlanetForm_Closing(object sender, CancelEventArgs e)
        {
            InitializePlanet();
            DialogResult = DialogResult.OK;
        }

        void LoadPlanet()
        {
            nameTextBox.Text = planet.Name;
            dateTimePicker1.Value = planet.InventingDate;
            if (planet.Photo != null)
                pictureBox1.Image = StarForm1.ConvertToImage(planet.Photo);
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
            foreach (var moon in planet.Moons)
                if (!Moons.Items.Contains(moon))
                    Moons.Items.Add(moon);
            Moons.Refresh();
        }

        private void InitializePlanet()
        {
            planet.Name = nameTextBox.Text;
            planet.InventingDate = dateTimePicker1.Value;
            planet.Photo = StarForm1.ConvertImage(pictureBox1.Image);
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
            //planet.Moons.AddRange(Moons.Items.Cast<Moon>());
            foreach (Moon pl in Moons.Items)
                if (!planet.Moons.Contains(pl))
                    planet.Moons.Add(pl);

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
    }
}
