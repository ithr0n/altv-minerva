namespace PlayGermany.Server.Enums
{
    public enum EntitySyncType
        : ulong
    {
        // based on https://github.com/lameule123/altv-csharp-streamer
        Marker = 0, // implementation missing
        TextLabel = 1, // implementation missing
        Prop = 2,
        HelpText = 3, // implementation missing
        StaticBlip = 4, // implementation missing
        DynamicBlip = 5 // implementation missing
    }
}
