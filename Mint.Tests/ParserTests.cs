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
        var tokens = Lexer.Lex(source);

        var parser = new Parser();
        var actual = parser.ParseChunk(tokens);
        
        TestHelper.AreEqual(expect, actual);
    }
}