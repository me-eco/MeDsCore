namespace MeDsCore.Interactions.ApplicationCommands.Modules;

/// <summary>
/// Представляет шаблонный выбор значения параметраы
/// </summary>
public class OptionChoice
{
    public OptionChoice(string name, double value) : this(name, (object) value)
    {
    }

    public OptionChoice(string name, int value) : this(name, (object)value)
    {
    }
    
    public OptionChoice(string name, string value) : this(name, (object)value)
    {
    }
    
    internal OptionChoice(string name, object value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Value. Can be <see cref="string"/>, <see cref="int"/>, <see cref="double"/>
    /// </summary>
    public object Value { get; }
}