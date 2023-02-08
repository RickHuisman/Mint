namespace Mint.Compiler;

public enum ValueType
{
    Nil,
    Boolean,
    Number,
    String,
    Closure
    // nil, boolean, number, table, userdata, and thread TODO
}

public record Closure(Function Function);

public record Function(FunctionProto FunctionProto);

public class Value
{
    public ValueType ValueType { get; private set; }
    public double Number { get; set; }
    public bool Boolean { get; set; }
    public string String { get; set; }
    public Closure Closure { get; set; }

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

    public Value(Closure closure)
    {
        ValueType = ValueType.Closure;
        Closure = closure;
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
        if (ValueType == ValueType.Closure) return $"Closure({Closure.Function.FunctionProto.Name})";
        throw new NotImplementedException();
    }
}