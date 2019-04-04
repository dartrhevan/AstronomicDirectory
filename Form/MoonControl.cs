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
    
    public partial class MoonControl : System.Windows.Forms.UserControl
    {
        private Moon moon;
        private readonly Planet planet;
        public MoonControl(Planet pl, Moon moon = null)
        {
            if (moon != null) this.moon = moon;
            planet = pl;
            InitializeComponent();
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
                //planet.Name = nameTextBox.Text;
                //planet.InventingDate = dateTimePicker1.Value;
                //planet.Photo = pictureBox1.Image;
            uint radius;
            uint.TryParse(radiusTextBox.Text, out radius);
            moon= new Moon(dateTimePicker1.Value, pictureBox1.Image, nameTextBox.Text, radius, checkBox1.Checked, planet);
            (Parent.Parent as PlanetForm).Planets.Items.Add(moon);
            Parent.Controls.Remove(this);
        }

        //private void SaveAndCloseAndClosre()
        //{
        //    this.Hide();
        //}
    }
}
