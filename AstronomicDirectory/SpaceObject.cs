using System;
using System.Drawing;

namespace AstronomicDirectory
{
    public abstract class SpaceObject
    {
        /// <summary>
        /// Изображение
        /// </summary>
        public Bitmap Photo { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Среднее расстояние
        /// </summary>
        public readonly Distance MiddleDistance;
        /// <summary>
        /// Радиус в километрах
        /// </summary>
        public readonly uint Radius;
        /// <summary>
        /// Средняя температура на поверхности в Кельвинах, если не известна, то 0
        /// </summary>
        public readonly uint? Temperature;
        /// <summary>
        /// Название галактики, в которой расположен объект
        /// </summary>
        public readonly string Galaxy;
        /// <summary>
        /// Дата открытия
        /// </summary>
        public readonly DateTime InventingDate;

        protected SpaceObject(DateTime inventingDate, Bitmap photo, string name, Distance middleDistance, uint radius, string galaxy, uint temperature = 0)
        {
            Photo = photo;
            Name = name;
            MiddleDistance = middleDistance;
            Radius = radius;
            Temperature = temperature;
            Galaxy = galaxy;
            InventingDate = inventingDate;
        }
    }
}