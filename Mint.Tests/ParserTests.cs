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

    private static void RunParserTest(string source, object expect)
    {
        var tokens = Lexer.Lex(source);

        var parser = new Parser();
        var actual = parser.ParseChunk(tokens);

        TestHelper.AreEqual(expect, actual);
    }
}