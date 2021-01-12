using AltV.Net;
using System;

namespace Minerva.Server.EntitySync.WritableData
{
    public class WritableRgba
        : WritableRgb, IEquatable<WritableRgb>
    {
        public int Alpha { get; set; }

        public WritableRgba(int red, int green, int blue, int alpha)
            : base(red, green, blue)
        {
            Alpha = alpha;
        }

        public override void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("red");
            writer.Value(Red);
            writer.Name("green");
            writer.Value(Green);
            writer.Name("blue");
            writer.Value(Blue);
            writer.Name("alpha");
            writer.Value(Alpha);
            writer.EndObject();
        }

        public bool Equals(WritableRgba other) => this == other && Alpha == other.Alpha;
    }
}
