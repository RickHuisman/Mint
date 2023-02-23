using System.Collections.Generic;
using Mint.Syntax;
using NUnit.Framework;

namespace Mint.Tests;

public class ParserTests
{
    [Test]
    public void ParseNumerals()
    {
        var expect = new Chunk(new Block(new()
        {
            new ExpressionStatement(new BinaryExpression(
                new NumberExpression(2),
                BinaryOperator.Add,
                new NumberExpression(3)
            ))
        }, new ReturnStatement(null)));
        const string source = "2 + 3";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseGlobal()
    {
        var expect = new Chunk(new Block(new()
        {
            new AssignmentStatement("x", new NumberExpression(10))
        }, new ReturnStatement(null)));
        const string source = @"x = 10";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseLocal()
    {
        var expect = new Chunk(new Block(new()
        {
            new LocalStatement("x", new NumberExpression(10))
        }, new ReturnStatement(null)));
        const string source = @"local x = 10";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseBlock()
    {
        var expect = new Chunk(new Block(new()
        {
            new Block(new()
            {
                new LocalStatement("x", new NumberExpression(2)),
                new LocalStatement("y", new NumberExpression(3))
            }, null)
        }, new ReturnStatement(null)));
        const string source = @"
do
    x = 2
    y = 3
end";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseFunction()
    {
        var expect = new Chunk(new Block(new()
        {
            new FunctionStatement(
                "foo",
                new List<string>(),
                new Block(new List<IStatement>()
                {
                    new LocalStatement("x", new NumberExpression(10))
                }, null)
            ),
        }, new ReturnStatement(null)));
        const string source = @"
function foo()
    local x = 10
end
";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseFunctionCall()
    {
        var expect = new Chunk(new Block(new()
        {
            new FunctionStatement(
                "foo",
                new List<string>(),
                new Block(new List<IStatement>(), null)
            ),
            new ExpressionStatement(new FunctionCallExpression(new NameExpression("foo"), new List<IExpression>()))
        }, new ReturnStatement(null)));
        const string source = @"
function foo()
end

foo()
";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseOperators()
    {
        var expect = new Chunk(new Block(new()
        {
            new ExpressionStatement(
                new BinaryExpression(new NumberExpression(1), BinaryOperator.Add,
                    new BinaryExpression(new NumberExpression(2), BinaryOperator.Multiply,
                        new BinaryExpression(new NumberExpression(3), BinaryOperator.Divide, new NumberExpression(4)))))
        }, new ReturnStatement(null)));
        const string source = @"1 + 2 * 3 / 4";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseIfInline()
    {
        var expect = new Chunk(new Block(new()
        {
            new IfElseStatement(
                new BinaryExpression(new NameExpression("x"), BinaryOperator.Less, new NumberExpression(0)),
                new Block(new List<IStatement>
                {
                    new AssignmentStatement("x", new NumberExpression(0))
                }, null), null)
        }, new ReturnStatement(null)));
        const string source = @"if x < 0 then x = 0 end";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseIfMultiLine()
    {
        var expect = new Chunk(new Block(new()
        {
            new IfElseStatement(
                new BinaryExpression(new NameExpression("x"), BinaryOperator.Greater, new NameExpression("y")),
                new Block(new List<IStatement>
                {
                    new AssignmentStatement("x", new NumberExpression(0))
                }, null), null)
        }, new ReturnStatement(null)));
        const string source = @"
if x > y then
    x = 0
end";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseIfElse()
    {
        const string source = @"if x < y then return x else return y end";
        var expect = new Chunk(new Block(new()
        {
            new IfElseStatement(
                new BinaryExpression(new NameExpression("x"), BinaryOperator.Less, new NameExpression("y")),
                new Block(new List<IStatement>(), new ReturnStatement(new NameExpression("x")))
                , new Block(new List<IStatement>(), new ReturnStatement(new NameExpression("y"))))
        }, new ReturnStatement(null)));
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseReturn()
    {
        const string source = @"return";
        var expect = new Chunk(new Block(new(), new ReturnStatement(null)));
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseReturnWithValue()
    {
        const string source = @"return 5";
        var expect = new Chunk(new Block(new(), new ReturnStatement(new NumberExpression(5))));
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseReturnInBlock()
    {
        const string source = @"do
    return 5
end";
        var expect = new Chunk(new Block(new()
        {
            new Block(new List<IStatement>(), new ReturnStatement(new NumberExpression(5)))
        }, new ReturnStatement(null)));
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseAnd()
    {
        var expect = new Chunk(new Block(new()
        {
            new ExpressionStatement(
                new AndExpression(new BoolExpression(true), new BoolExpression(false)))
        }, new ReturnStatement(null)));
        const string source = @"true and false";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseOr()
    {
        var expect = new Chunk(new Block(new()
        {
            new ExpressionStatement(
                new OrExpression(new BoolExpression(true), new BoolExpression(false)))
        }, new ReturnStatement(null)));
        const string source = @"true or false";
        RunParserTest(source, expect);
    }

    private static void RunParserTest(string source, object expect)
    {
        var tokens = Lexer.Lex(source);
        var parser = new Parser();
        var actual = parser.ParseChunk(tokens);
        TestHelper.AreEqual(expect, actual);
    }
}