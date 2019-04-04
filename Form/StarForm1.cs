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
        private PlanetForm planetForm;
        private Star star = new Star();
        public StarForm1()
        {
            InitializeComponent();
            Planets.MouseDoubleClick += Planets_MouseDoubleClick;
            comboBox1.Text = comboBox1.Items[0] as string;
            //planetForm = new PlanetForm(star) {Owner = this};
            Planets.SelectedIndexChanged += (sender, args) => { button2.Enabled = Planets.SelectedItem is Planet; };
            addPlanetButton.Click += (sender, args) =>
            {
                var planet = new Planet(star);
                planetForm = new PlanetForm(planet) {Owner = this};
                InitializeStar();
                //star = new Star(dateTimePicker1.Value, pictureBox1.Image, nameTextBox, new Distance(uint.Parse()), );
                planetForm.ShowDialog();
                Planets.Items.Add(planet);
                Planets.Refresh();
                Invalidate();
            };
            button2.Enabled = false;
        }

        private void Planets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(sender.ToString());
            //throw new NotImplementedException();
            int index = this.Planets.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var item = (Planet)Planets.Items[index];
                var ff = new PlanetForm(item);//();// { Flight = item };
                //ff.ShowDialog(this);
                if (ff.ShowDialog(this) == DialogResult.OK)
                {
                    Planets.Items.Remove(item);
                    Planets.Items.Insert(index, item);
                }
            }
            //Planets.Refresh();
            //Invalidate();

        }

        //private void Planets_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    //MessageBox.Show(sender.ToString());
        //    //throw new System.NotImplementedException();
        //}

        private void InitializeStar()
        {
            //if (star == null)
            //    star = new Star();
            star.Name = nameTextBox.Text;
            star.Galaxy = galacticTextBox.Text;
            star.InventingDate = dateTimePicker1.Value;
            star.Photo = pictureBox1.Image;
            uint temp;
            if (uint.TryParse(radiusTextBox.Text, out temp))
                star.Radius = temp;
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
                star.MiddleDistance = new Distance(temp, t);
            }
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
            InitializeStar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Planets.SelectedItem is Planet)
            {
                Planets.Items.Remove(Planets.SelectedItem);
            }
        }
    }
}
