namespace Mint.Compiler;

public enum ValueType
{
    Boolean,
    Number
}

public class Value
{
    public ValueType ValueType { get; }
    public double Number { get; set; }
    public bool Boolean { get; set; }

    public Value(ValueType valueType, double number)
    {
        ValueType = valueType;
        Number = number;
    }
    
    public Value(ValueType valueType, bool boolean)
    {
        ValueType = valueType;
        Boolean = boolean;
    }

    public static Value operator +(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(ValueType.Number, a.Number + b.Number);
    }
    
    public static Value operator ==(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(ValueType.Boolean, a.Number == b.Number);
    }
    
    public static Value operator !=(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(ValueType.Boolean, a.Number != b.Number);
    }

    public override string ToString()
    {
        if (ValueType == ValueType.Number) return Number.ToString();
        if (ValueType == ValueType.Boolean) return Boolean.ToString();
        throw new NotImplementedException();
    }
}