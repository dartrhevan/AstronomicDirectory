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
        //private readonly Planet planet;
        private readonly PlanetForm planetForm;
        readonly PictureBox moonPicture = new PictureBox();
        public MoonControl(Moon moon, PlanetForm planetForm)
        {
            this.planetForm = planetForm;
            this.moonPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                             | System.Windows.Forms.AnchorStyles.Left)
                                                                            | System.Windows.Forms.AnchorStyles.Right)));
            this.moonPicture.Location = new System.Drawing.Point(5, 308);
            this.moonPicture.Size = new System.Drawing.Size(497, 263);
            this.moonPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.moonPicture.TabIndex = 1;
            this.moonPicture.TabStop = false;

            this.moon = moon;
            //PlanetForm planetForm = Parent as PlanetForm;
            planetForm.Controls.Remove(planetForm.pictureBox1);
            planetForm.Controls.Add(moonPicture);

            //planet = pl;
            InitializeComponent();
            LoadMoon();
            Invalidate();
        }

        private void photoButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            var dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
                moonPicture.Image = Image.FromFile(ofd.FileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //InitializeMoon();

            int index = planetForm.Moons.SelectedIndex;
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                planetForm.Moons.Items.Remove(moon);
                InitializeMoon();
                planetForm.Moons.Items.Insert(index, moon);
            }
            else
            {
                InitializeMoon();
                planetForm.Moons.Items.Add(moon);
            }

            planetForm.Controls.Remove(moonPicture);
            planetForm.Controls.Add(planetForm.pictureBox1);

            Parent.Controls.Remove(this);

        }

        void LoadMoon()
        {
            nameTextBox.Text = moon.Name;
            dateTimePicker1.Value = moon.InventingDate;
            moonPicture.Image = moon.Photo;
            radiusTextBox.Text = moon.Radius.ToString();
            //distanceTextBox.Text = moon.MiddleDistance.Value.ToString();
            moon.HasAtmosphere = checkBox1.Checked;
            //switch (moon.MiddleDistance.Unit)
            //{
            //    case UnitType.Kilometers:
            //        comboBox1.Text = "км";
            //        break;
            //    case UnitType.LightYears:
            //        comboBox1.Text = "св. г.";
            //        break;
            //    case UnitType.AstronomicUnits:
            //        comboBox1.Text = "а.е.";
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}

            checkBox1.Checked = moon.HasAtmosphere;
        }

        private void InitializeMoon()
        {
            moon.Name = nameTextBox.Text;
            moon.InventingDate = dateTimePicker1.Value;
            moon.Photo = moonPicture.Image;
            uint temp;
            if (uint.TryParse(radiusTextBox.Text, out temp))
                moon.Radius = temp;
            moon.HasAtmosphere = checkBox1.Checked;
        }
    }
}
