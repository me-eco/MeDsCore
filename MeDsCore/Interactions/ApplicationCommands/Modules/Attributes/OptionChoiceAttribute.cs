namespace MeDsCore.Interactions.ApplicationCommands.Modules.Attributes;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
public class OptionChoiceAttribute : Attribute
{
    public OptionChoiceAttribute(string name, string value) : 
        this(name, (object)value)
    {
    }

    public OptionChoiceAttribute(string name, int value) : 
        this(name, (object)value)
    {
        
    }

    public OptionChoiceAttribute(string name, double value) : 
        this(name, (object)value)
    {
        
    }
    
    private OptionChoiceAttribute(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public object Value { get; }
}