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

public record GlobalStatement(string Name, IExpression Value) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        Value.Compile(compiler);

        compiler.Emit(Opcode.SetGlobal);
        var constantId = compiler.AddConstant(new Value(Name));
        compiler.Emit(constantId);
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
        fun.Function.FunctionProto.Name = Name;
        // TODO: Set arity.
        Console.WriteLine(fun.Function.FunctionProto);

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
            case BinaryOperator.Equal:
                compiler.Emit(Opcode.Equal);
                break;
            case BinaryOperator.NotEqual:
                compiler.Emit(Opcode.NotEqual);
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
        throw new NotImplementedException();
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

public record NameExpression(string Name) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        // Get local.
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
            compiler.Emit(Opcode.GetLocal);
            compiler.Emit((byte) slot);
        }
    }
}

public enum BinaryOperator
{
    Add,
    Equal,
    NotEqual
}

public enum UnaryOperator
{
}