using MeDsCore.Base;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class OptionAttribute : Attribute
{
    public OptionAttribute(string name, string description) : 
        this(name, description, null, null)
    {
        
    }

    public OptionAttribute(string name, string description, ChannelType[] channelsTypes) : this(name, description,
        null, channelsTypes)
    {
    }

    public OptionAttribute(string name, string description,
        object? minValue = null, object? maxValue = null) : this(name, description,
        null, minValue, maxValue)
    {
        
    }

    private OptionAttribute(string name, string description, ChannelType[]? channelsTypes = null, object? minValue = null, object? maxValue = null)
    {
        Name = name;
        Description = description;
        ChannelsTypes = channelsTypes;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public string Name { get; }
    public string Description { get; }
    public ChannelType[]? ChannelsTypes { get; }
    public object? MinValue { get; }
    public object? MaxValue { get; }
}