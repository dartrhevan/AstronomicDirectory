namespace AstronomicDirectory
{
    public struct Distance
    {
        /// <summary>
        /// Значение
        /// </summary>
        public uint Value { get; set; }
        /// <summary>
        /// Единица Измерения
        /// </summary>
        public readonly UnitType Unit;

        public Distance(uint value, UnitType unit)
        {
            Value = value;
            Unit = unit;
        }

        public override bool Equals(object obj)
        {
            var distance = (Distance)obj;
            return 
                   Value == distance.Value &&
                   Unit == distance.Unit;
        }

        public override int GetHashCode()
        {
            var hashCode = -177567199;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Unit.GetHashCode();
            return hashCode;
        }
    }
}