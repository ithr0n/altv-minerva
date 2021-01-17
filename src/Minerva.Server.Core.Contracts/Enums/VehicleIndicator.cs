using System;

namespace Minerva.Server.Core.Contracts.Enums
{
    [Flags]
    public enum VehicleIndicator
    {
        None = 0x00,
        Right = 0x01,
        Left = 0x10,
        Hazard = Left | Right
    }
}
