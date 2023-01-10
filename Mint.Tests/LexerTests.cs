using System.Collections.Generic;
using Mint.Syntax;
using NUnit.Framework;

namespace Mint.Tests;

public class Tests
{
    [Test]
    public void Numerals()
    {
        var expect = new List<Token>()
        {
            new(TokenType.Number, "2"),
            new(TokenType.Number, "3"),
            new(TokenType.Number, "5"),
        };
        
        const string source = "2 3 5";
        TestLexer(source, expect);
    }

    private static void TestLexer(string source, IList<Token> expect)
    {
        var actual = Lexer.Lex(source);
        Assert.AreEqual(expect, actual);
    }
}