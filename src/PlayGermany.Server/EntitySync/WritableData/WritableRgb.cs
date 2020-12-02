using AltV.Net;
using System;

namespace PlayGermany.Server.EntitySync.WritableData
{
    public class WritableRgb
        : IWritable, IEquatable<WritableRgb>
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public WritableRgb(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public virtual void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("red");
            writer.Value(Red);
            writer.Name("green");
            writer.Value(Green);
            writer.Name("blue");
            writer.Value(Blue);
            writer.EndObject();
        }

        public bool Equals(WritableRgb other) => Red == other.Red && Green == other.Green && Blue == other.Blue;
    }
}
