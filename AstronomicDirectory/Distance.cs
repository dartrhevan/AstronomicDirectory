using System.Runtime.Serialization;

namespace AstronomicDirectory
{
    public struct Distance : ISerializable
    {
        /// <summary>
        /// Значение
        /// </summary>
        public readonly uint Value;
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", Value, typeof(uint));
            info.AddValue("Unit", Unit, typeof(UnitType));
        }

        public Distance(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            Value = (uint)info.GetValue("Value", typeof(string));
            Unit = (UnitType)info.GetValue("Unit", typeof(string));
        }

        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}