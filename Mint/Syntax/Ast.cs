using Mint.Compiler;
using Mint.VM;

namespace Mint.Syntax;

public record Chunk(Block Block);

public interface IStatement
{
    public void Compile(Compiler.Compiler compiler);
}

public record Block(List<IStatement> Statements, ReturnStatement ReturnStatement) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        compiler.BeginScope();
        foreach (var statement in Statements)
        {
            statement.Compile(compiler);
        }
        compiler.EndScope();
    }
}

public record PrintStatement(IExpression Value) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        Value.Compile(compiler);
        compiler.Emit(Opcode.Print);
    }
}

public record AssignmentStatement(string Name, IExpression Value) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        Value.Compile(compiler);
        
        var slot = compiler.ResolveLocal(Name);
        if (slot == -1)
        {
            // Set global.
            compiler.Emit(Opcode.SetGlobal);
            var constantId = compiler.AddConstant(new Value(Name));
            compiler.Emit(constantId);
        }
        else
        {
            // Set local.
            compiler.Emit(Opcode.SetLocal);
            compiler.Emit((byte) slot);
        }
    }
}

public record LocalStatement(string Name, IExpression Value) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        compiler.DeclareVariable(Name);

        // Compile initializer.
        Value.Compile(compiler);

        compiler.Emit(Opcode.SetLocal);

        var slot = compiler.ResolveLocal(Name);
        if (slot == -1) throw new Exception();

        compiler.Emit((byte) slot);
    }
}

public record FunctionStatement(string Name, List<string> Parameters, Block Body) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        compiler.SetInstance(new CompilerInstance());
        CompileClosure(compiler);

        // Define function name.
        compiler.Emit(Opcode.SetGlobal);
        var constantId = compiler.AddConstant(new Value(Name));
        compiler.Emit(constantId);
    }

    private void CompileClosure(Compiler.Compiler compiler)
    {
        compiler.BeginScope();

        var arity = Parameters.Count;

        // Compile arguments.
        // TODO:

        // Compile body.
        Body.Compile(compiler);

        // Create the function body.
        var fun = compiler.EndCompiler();
        fun.FunctionProto.Name = Name;
        // TODO: Set arity.
        Console.WriteLine(fun.FunctionProto);

        compiler.Emit(Opcode.Closure);

        var constantId = compiler.AddConstant(new Value(fun));
        compiler.Emit(constantId);
    }
}

public record FunctionCallExpression(IExpression Callee, List<IExpression> Arguments) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        var arity = Arguments.Count;

        // Compile callee.
        Callee.Compile(compiler);

        // Compile arguments.
        foreach (var arg in Arguments)
        {
            arg.Compile(compiler);
        }

        compiler.Emit(Opcode.Call);
        compiler.Emit((byte) arity);
    }
}

public record ReturnStatement(IExpression? Expression) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        if (Expression != null)
        {
            Expression.Compile(compiler);
            compiler.Emit(Opcode.Return);
        }
        else
        {
            compiler.EmitReturn();
        }
    }
}

public record ExpressionStatement(IExpression Expression) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        Expression.Compile(compiler);
    }
}

public interface IExpression
{
    public void Compile(Compiler.Compiler compiler);
}

public record BinaryExpression(IExpression Left, BinaryOperator Operator, IExpression Right) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        Left.Compile(compiler);
        Right.Compile(compiler);
        switch (Operator)
        {
            case BinaryOperator.Add:
                compiler.Emit(Opcode.Add);
                break;
            case BinaryOperator.Subtract:
                compiler.Emit(Opcode.Subtract);
                break;
            case BinaryOperator.Multiply:
                compiler.Emit(Opcode.Multiply);
                break;
            case BinaryOperator.Divide:
                compiler.Emit(Opcode.Divide);
                break;
            case BinaryOperator.Equal:
                compiler.Emit(Opcode.Equal);
                break;
            case BinaryOperator.NotEqual:
                compiler.Emit(Opcode.Equal);
                compiler.Emit(Opcode.Not);
                break;
            case BinaryOperator.Greater:
                compiler.Emit(Opcode.Greater);
                break;
            case BinaryOperator.GreaterThanEqual:
                compiler.Emit(Opcode.Less);
                compiler.Emit(Opcode.Not);
                break;
            case BinaryOperator.Less:
                compiler.Emit(Opcode.Less);
                break;
            case BinaryOperator.LessThanEqual:
                compiler.Emit(Opcode.Greater);
                compiler.Emit(Opcode.Not);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public record UnaryExpression(UnaryOperator Operator, IExpression Left) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        Left.Compile(compiler);
        var opcode = Operator switch
        {
            UnaryOperator.Not => Opcode.Not,
            UnaryOperator.Negate => Opcode.Negate,
            _ => throw new ArgumentOutOfRangeException()
        };
        compiler.Emit(opcode);
    }
}

public record NumberExpression(double Number) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        compiler.Emit(Opcode.LoadConstant);
        var constantId = compiler.AddConstant(new Value(Number));
        compiler.Emit(constantId);
    }
}

public record BoolExpression(bool Bool) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        compiler.Emit(Opcode.LoadConstant);
        var constantId = compiler.AddConstant(new Value(Bool));
        compiler.Emit(constantId);
    }
}

public record NilExpression : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        compiler.Emit(Opcode.LoadNil);
    }
}

public record NameExpression(string Name) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        var slot = compiler.ResolveLocal(Name);
        if (slot == -1)
        {
            // Get global.
            compiler.Emit(Opcode.GetGlobal);
            var constantId = compiler.AddConstant(new Value(Name));
            compiler.Emit(constantId);
        }
        else
        {
            // Get local.
            compiler.Emit(Opcode.GetLocal);
            compiler.Emit((byte) slot);
        }
    }
}

public record IfElseStatement(IExpression Condition, Block Then, Block? Else) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
}

public enum BinaryOperator
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Equal,
    NotEqual,
    Greater,
    GreaterThanEqual,
    Less,
    LessThanEqual
}

public enum UnaryOperator
{
    Not,
    Negate
}