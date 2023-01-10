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

    [Test]
    public void TestReservedKeywords()
    {
        var expect = new List<Token>()
        {
            new(TokenType.Break, "break"),
            new(TokenType.Do, "do"),
            new(TokenType.Else, "else"),
            new(TokenType.ElseIf, "elseif"),
            new(TokenType.End, "end"),
            new(TokenType.Function, "function"),
            new(TokenType.Goto, "goto"),
            new(TokenType.If, "if"),
            new(TokenType.Name, "custom"),
            new(TokenType.Name, "name"),
        };
        const string source = "break do else elseif end function goto if custom name";
        TestLexer(source, expect);
    }

    private static void TestLexer(string source, IList<Token> expect)
    {
        var actual = Lexer.Lex(source);
        Assert.AreEqual(expect, actual);
    }
}