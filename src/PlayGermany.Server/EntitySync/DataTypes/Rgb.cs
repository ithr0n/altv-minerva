using AltV.Net;
using System;

namespace PlayGermany.Server.EntitySync.DataTypes
{
    public class Rgb : IWritable, IEquatable<Rgb>
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public Rgb(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("Red");
            writer.Value(Red);
            writer.Name("Green");
            writer.Value(Green);
            writer.Name("Blue");
            writer.Value(Blue);
            writer.EndObject();
        }

        public bool Equals(Rgb other) => Red == other.Red && Green == other.Green && Blue == other.Blue;
    }
}
