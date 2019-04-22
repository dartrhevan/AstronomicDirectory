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
        private readonly PlanetForm planetForm;
        readonly PictureBox moonPicture = new PictureBox();
        public MoonControl(Moon moon, PlanetForm planetForm)
        {
            this.planetForm = planetForm;
            moonPicture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            moonPicture.Location = new Point(5, 308);
            moonPicture.Size = new Size(497, 263);
            moonPicture.SizeMode = PictureBoxSizeMode.Zoom;
            moonPicture.TabIndex = 1;
            moonPicture.TabStop = false;

            this.moon = moon;
            planetForm.Controls.Remove(planetForm.pictureBox1);
            planetForm.Controls.Add(moonPicture);
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
            int index = planetForm.Moons.SelectedIndex;
            if (index != ListBox.NoMatches)
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
            if(moon.Photo != null)
                moonPicture.Image = StarForm1.ConvertToImage(moon.Photo);
            radiusTextBox.Text = moon.Radius.ToString();
            moon.HasAtmosphere = checkBox1.Checked;
            checkBox1.Checked = moon.HasAtmosphere;
        }

        private void InitializeMoon()
        {
            moon.Name = nameTextBox.Text;
            moon.InventingDate = dateTimePicker1.Value;
            moon.Photo = StarForm1.ConvertImage(moonPicture.Image);
            uint temp;
            if (uint.TryParse(radiusTextBox.Text, out temp))
                moon.Radius = temp;
            moon.HasAtmosphere = checkBox1.Checked;
        }
    }
}
