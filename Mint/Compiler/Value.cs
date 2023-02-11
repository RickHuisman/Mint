namespace Mint.Compiler;

/// <summary>
/// There are eight basic types in Lua: nil, boolean, number, string, function, userdata, thread, and table.
/// The type nil has one single value, nil, whose main property is to be different from any other value;
/// it often represents the absence of a useful value. The type boolean has two values, false and true.
/// Both nil and false make a condition false; they are collectively called false values.
/// Any other value makes a condition true.Despite its name, false is frequently used as an alternative to nil,
/// with the key difference that false behaves like a regular value in a table,
/// while a nil in a table represents an absent key.
/// </summary>
public enum ValueType
{
    Nil,
    Boolean,
    Number,
    String,
    Function // TODO: Change to Function.
    // TODO: Implement: table, userdata, and thread value types.
}

public record Closure(Function Function);

public record Function(FunctionProto FunctionProto);

public class Value
{
    public ValueType ValueType { get; private set; }
    public double Number { get; set; }
    public bool Boolean { get; set; }
    public string String { get; set; }
    public Function Function { get; set; }

    public static Value NilValue()
    {
        return new Value
        {
            ValueType = ValueType.Nil
        };
    }

    public Value()
    {
    }

    public Value(double number)
    {
        ValueType = ValueType.Number;
        Number = number;
    }

    public Value(bool boolean)
    {
        ValueType = ValueType.Boolean;
        Boolean = boolean;
    }

    public Value(string @string)
    {
        ValueType = ValueType.String;
        String = @string;
    }

    public Value(Function function)
    {
        ValueType = ValueType.Function;
        Function = function;
    }

    public static Value operator +(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number + b.Number);
    }
    
    public static Value operator -(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number - b.Number);
    }
    public static Value operator *(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number * b.Number);
    }
    public static Value operator /(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number / b.Number);
    }

    public static Value operator >(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number > b.Number);
    }
    
    public static Value operator <(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number < b.Number);
    }
    
    public static Value operator !(Value a)
    {
        if (a.ValueType == ValueType.Boolean) return new Value(!a.Boolean);
        if (a.ValueType == ValueType.Nil) return new Value(true);
        throw new Exception();
    }

    public static Value operator ==(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number == b.Number);
    }

    public static Value operator !=(Value a, Value b)
    {
        if (a.ValueType != ValueType.Number) throw new Exception();
        if (b.ValueType != ValueType.Number) throw new Exception();
        return new(a.Number != b.Number);
    }

    public override string ToString()
    {
        if (ValueType == ValueType.Nil) return "nil";
        if (ValueType == ValueType.Number) return Number.ToString();
        if (ValueType == ValueType.Boolean) return Boolean.ToString();
        if (ValueType == ValueType.String) return String;
        if (ValueType == ValueType.Function) return $"Function({Function.FunctionProto.Name})";
        throw new NotImplementedException();
    }
}