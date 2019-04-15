using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AstronomicDirectory
{
    public abstract class SpaceObject
    {
        /// <summary>
        /// Изображение
        /// </summary>
        public byte[] Photo { get; set; } //= new byte[0];

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Среднее расстояние
        /// </summary>
        public Distance MiddleDistance { get; set; }
        /// <summary>
        /// Радиус в километрах
        /// </summary>
        public uint Radius { get; set; }
        /// <summary>
        /// Средняя температура на поверхности в Кельвинах, если не известна, то 0
        /// </summary>
        public uint? Temperature { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public DateTime InventingDate { get; set; } = DateTimePicker.MinDateTime;

        protected SpaceObject(DateTime inventingDate, Image photo, string name, Distance middleDistance, uint radius, uint temperature = 0)
        {
            //Photo = photo;
            Photo = ConvertImage(photo);

            Name = name;
            MiddleDistance = middleDistance;
            Radius = radius;
            Temperature = temperature;
            //Galaxy = galaxy;
            InventingDate = inventingDate;
        }

        public static byte[] ConvertImage(Image photo)
        {
            if (photo == null) return null;
            var stream = new MemoryStream();
            photo.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        public static Image ConvertToImage(byte[] arr) => Image.FromStream(new MemoryStream(arr));

        protected SpaceObject()
        {
        }
    }
}