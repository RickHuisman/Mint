using System.Collections.Generic;
using Mint.Syntax;
using NUnit.Framework;

namespace Mint.Tests;

public class Tests
{
    [Test]
    public void LexNumerals()
    {
        var expect = new List<Token>
        {
            new(TokenType.Number, "2"),
            new(TokenType.Number, "3"),
            new(TokenType.Number, "5"),
        };
        const string source = "2 3 5";
        RunLexerTest(source, expect);
    }
    
    [Test]
    public void LexOperators()
    {
        var expect = new List<Token>
        {
            new(TokenType.Plus, "+"),
            new(TokenType.Minus, "-"),
            new(TokenType.Star, "*"),
            new(TokenType.Slash, "/"),
            // new(TokenType.EqualEqual, "=="),
            // new(TokenType.BangEqual, "!="),
            // new(TokenType.GreaterThan, ">"),
            // new(TokenType.GreaterThanEqual, ">="),
            // new(TokenType.LessThan, "<="),
            // new(TokenType.LessThanEqual, "<="),
        };
        // TODO: Fix.
        // const string source = "+ - * / == != > >= < <=";
        // RunLexerTest(source, expect);
    }

    [Test]
    public void LexReservedKeywords()
    {
        var expect = new List<Token>
        {
            new(TokenType.True, "true"),
            new(TokenType.False, "false"),
            new(TokenType.And, "and"),
            new(TokenType.Or, "or"),
            new(TokenType.Not, "not"),
            new(TokenType.Break, "break"),
            new(TokenType.Do, "do"),
            new(TokenType.Else, "else"),
            new(TokenType.ElseIf, "elseif"),
            new(TokenType.End, "end"),
            new(TokenType.Function, "function"),
            new(TokenType.Goto, "goto"),
            new(TokenType.If, "if"),
            new(TokenType.Return, "return"),
            new(TokenType.Print, "print"),
            new(TokenType.Local, "local"),
            new(TokenType.Name, "custom"),
            new(TokenType.Name, "name"),
        };
        const string source = "true false and or not break do else elseif end function goto if return print local custom name";
        RunLexerTest(source, expect);
    }

    [Test]
    public void LexFunction()
    {
        var expect = new List<Token>
        {
            new(TokenType.Function, "function"),
            new(TokenType.Name, "foo"),
            new(TokenType.LeftParen, "("),
            new(TokenType.RightParen, ")"),
            new(TokenType.End, "end"),
        };
        const string source = @"
function foo()
end
";
        RunLexerTest(source, expect);
    }

    private static void RunLexerTest(string source, IList<Token> expect)
    {
        var actual = Lexer.Lex(source);
        Assert.AreEqual(expect, actual);
    }
}