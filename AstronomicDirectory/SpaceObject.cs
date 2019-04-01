using System;
using System.Drawing;

namespace AstronomicDirectory
{
    public abstract class SpaceObject
    {
        /// <summary>
        /// Изображение
        /// </summary>
        public Image Photo { get; set; }
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
        /// Название галактики, в которой расположен объект
        /// </summary>
        public string Galaxy { get; set; }
        /// <summary>
        /// Дата открытия
        /// </summary>
        public DateTime InventingDate { get; set; }

        protected SpaceObject(DateTime inventingDate, Image photo, string name, Distance middleDistance, uint radius, string galaxy, uint temperature = 0)
        {
            Photo = photo;
            Name = name;
            MiddleDistance = middleDistance;
            Radius = radius;
            Temperature = temperature;
            Galaxy = galaxy;
            InventingDate = inventingDate;
        }

        protected SpaceObject()
        {
        }
    }
}