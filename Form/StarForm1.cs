using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using AstronomicDirectory;
using System.Net;

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
            Planets.SelectedIndexChanged += (sender, args) => { button2.Enabled = Planets.SelectedItem is Planet; };
            addPlanetButton.Click += (sender, args) =>
            {
                var planet = new Planet(star);
                planetForm = new PlanetForm(planet) {Owner = this};
                InitializeStar();
                planetForm.ShowDialog();
                Planets.Items.Add(planet);
                Planets.Refresh();
                Invalidate();
            };
            button2.Enabled = false;
        }

        private void Planets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.Planets.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                var item = (Planet)Planets.Items[index];
                var ff = new PlanetForm(item);
                if (ff.ShowDialog(this) == DialogResult.OK)
                {
                    Planets.Items.Remove(item);
                    Planets.Items.Insert(index, item);
                }
            }
        }

        private void InitializeStar()
        {
            star.Name = nameTextBox.Text;
            star.Galaxy = galacticTextBox.Text;
            star.InventingDate = dateTimePicker1.Value;
            star.Photo = ConvertImage(pictureBox1.Image);
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
            foreach (Planet pl in Planets.Items)
                if (!star.Planets.Contains(pl))
                    star.Planets.Add(pl);
            //star.Planets.AddRange(Planets.Items.Cast<Planet>());//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public static byte[] ConvertImage(Image photo)
        {
            if (photo == null) return null;
            var stream = new MemoryStream();
            
            photo.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public static Image ConvertToImage(byte[] arr) => Image.FromStream(new MemoryStream(arr));


        void LoadStarFromFile(string path)
        {
            var xml = new XmlSerializer(typeof(Star));
            var fs = File.OpenRead(path);
            star = (Star)xml.Deserialize(fs);
            fs.Close();
        }

        void LoadStar()
        {
            nameTextBox.Text = star.Name;
            galacticTextBox.Text = star.Galaxy;
            dateTimePicker1.Value = star.InventingDate;
            pictureBox1.Image = star.Photo != null ? ConvertToImage(star.Photo) : null;
            radiusTextBox.Text = star.Radius.ToString();
            distanceTextBox.Text = star.MiddleDistance.Value.ToString();
            switch (star.MiddleDistance.Unit)
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
                default: throw new ArgumentException();
            }

            //foreach (var planet in star.Planets)
            Planets.Items.AddRange(star.Planets.ToArray());
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
            var sfd = new SaveFileDialog() { Filter = "Звезда|*.star" };
            if (sfd.ShowDialog(this) != DialogResult.OK)
                return;

            var xs = new XmlSerializer(typeof(Star));

            var file = File.Create(sfd.FileName);

            xs.Serialize(file, star);
            file.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Planets.SelectedItem is Planet)
            {
                Planets.Items.Remove(Planets.SelectedItem);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var opf = new OpenFileDialog() {Filter = "Звезда|*.star" };
            if(opf.ShowDialog() != DialogResult.OK) return;
            Planets.Items.Clear();
            LoadStarFromFile(opf.FileName);
            //InitializeStar();
            LoadStar();
        }
    }
}
