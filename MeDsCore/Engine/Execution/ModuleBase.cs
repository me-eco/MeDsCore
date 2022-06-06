namespace MeDsCore.Engine.Execution;

/// <summary>
/// Base of MVC-like modules
/// </summary>
public class ModuleBase
{
    public ModuleBase()
    {
        Socket = new ModuleSocket(this);
    }

    /// <summary>
    /// Socket allows you to create manipulations with mdule
    /// </summary>
    internal ModuleSocket Socket { get; }
}