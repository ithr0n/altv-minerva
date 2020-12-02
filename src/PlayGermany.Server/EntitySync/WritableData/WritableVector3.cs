using AltV.Net;
using System.Numerics;

namespace PlayGermany.Server.EntitySync.WritableData
{
    public class WritableVector3
        : IWritable
    {
        public Vector3 Data { get; set; }

        public WritableVector3()
        {
            Data = Vector3.Zero;
        }

        public WritableVector3(Vector3 data)
        {
            Data = data;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("x");
            writer.Value(Data.X);
            writer.Name("y");
            writer.Value(Data.Y);
            writer.Name("z");
            writer.Value(Data.Z);
            writer.EndObject();
        }
    }
}
