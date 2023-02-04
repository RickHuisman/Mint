using System.Collections.Generic;
using Mint.Syntax;
using Mint.Test;
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
        }, null));
        const string source = "2 + 3";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseGlobal()
    {
        var expect = new Chunk(new Block(new()
        {
            new GlobalStatement("x", new NumberExpression(10))
        }, null));
        const string source = @"x = 10";
        RunParserTest(source, expect);
    }

    [Test]
    public void ParseLocal()
    {
        var expect = new Chunk(new Block(new()
        {
            new LocalStatement("x", new NumberExpression(10))
        }, null));
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
        }, null));
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
        }, null));
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
                new Block(new List<IStatement>()
                {
                }, null)
            ),
            new ExpressionStatement(new FunctionCallExpression(new NameExpression("foo"), new List<IExpression>()))
        }, null));
        const string source = @"
function foo()
end

foo()
";
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