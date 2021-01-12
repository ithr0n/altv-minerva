﻿namespace Minerva.Server.Contracts.ScriptStrategy
{
    /// <summary>
    /// Dependency Injection will configure every class which implements this interface with a transient lifetime.
    /// This means every time the class is requested one new instance gets created.
    /// </summary>
    public interface ITranscientScript
    {
    }
}
