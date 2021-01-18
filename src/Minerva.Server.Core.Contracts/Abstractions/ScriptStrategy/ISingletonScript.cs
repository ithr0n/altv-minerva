namespace Minerva.Server.Core.Contracts.Abstractions.ScriptStrategy
{
    /// <summary>
    /// Dependency Injection will configure every class implementing this interface with a singleton lifetime.
    /// This means the class gets only created once and on every request the same instance is returned.
    /// </summary>
    public interface ISingletonScript
    {
    }
}
