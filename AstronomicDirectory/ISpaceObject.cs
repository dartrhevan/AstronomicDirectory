using System;

namespace AstronomicDirectory
{
    public interface ISpaceObject
    {
        /// <summary>
        /// Изображение
        /// </summary>
        byte[] Photo { get; set; } //= new byte[0];

        /// <summary>
        /// Имя
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Среднее расстояние
        /// </summary>
        Distance MiddleDistance { get; set; }
        /// <summary>
        /// Радиус в километрах
        /// </summary>
        uint Radius { get; set; }
        /// <summary>
        /// Средняя температура на поверхности в Кельвинах, если не известна, то 0
        /// </summary>
        uint Temperature { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        DateTime InventingDate { get; set; }// = new DateTime(1800, 1, 1);

        //protected SpaceObject(DateTime inventingDate, byte[] photo, string name, Distance middleDistance, uint radius, uint temperature = 0)
        //{
        //    //Photo = photo;
        //    Photo = photo;

        //    Name = name;
        //    MiddleDistance = middleDistance;
        //    Radius = radius;
        //    Temperature = temperature;
        //    //Galaxy = galaxy;
        //    InventingDate = inventingDate;
        //}

        //protected SpaceObject()
        //{
        //}
    }
}