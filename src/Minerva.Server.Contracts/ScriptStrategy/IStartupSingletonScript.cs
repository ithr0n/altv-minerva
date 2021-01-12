namespace Minerva.Server.Contracts.ScriptStrategy
{
    /// <summary>
    /// Same as <see cref="ISingletonScript"/>, but additionally the script gets constructed as soon as the server starts.
    /// </summary>
    public interface IStartupSingletonScript
        : ISingletonScript
    {
    }
}
