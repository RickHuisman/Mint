using System.Collections.Generic;
using NUnit.Framework;

namespace Mint.Parser.Tests;

public class Tests
{
    [Test]
    public void LexNumerals()
    {
        const string source = "1 2 3.45";
        var expect = new List<Token>
        {
            new(TokenType.Number, "1"),
            new(TokenType.Number, "2"),
            new(TokenType.Number, "3.45"),
        };
        RunLexerTest(source, expect);
    }

    [Test]
    public void LexOperators()
    {
        const string source = "+ - * / == ~= > >= < <=";
        var expect = new List<Token>
        {
            new(TokenType.Plus, "+"),
            new(TokenType.Minus, "-"),
            new(TokenType.Star, "*"),
            new(TokenType.Slash, "/"),
            new(TokenType.EqualEqual, "=="),
            new(TokenType.TildeEqual, "~="),
            new(TokenType.GreaterThan, ">"),
            new(TokenType.GreaterThanEqual, ">="),
            new(TokenType.LessThan, "<"),
            new(TokenType.LessThanEqual, "<="),
        };
        RunLexerTest(source, expect);
    }

    [Test]
    public void LexReservedKeywords()
    {
        const string source = @"true false and or not break do if then
                                else elseif end function goto return print
                                local custom name";
        var expect = new List<Token>
        {
            new(TokenType.True, "true"),
            new(TokenType.False, "false"),
            new(TokenType.And, "and"),
            new(TokenType.Or, "or"),
            new(TokenType.Not, "not"),
            new(TokenType.Break, "break"),
            new(TokenType.Do, "do"),
            new(TokenType.If, "if"),
            new(TokenType.Then, "then"),
            new(TokenType.Else, "else"),
            new(TokenType.ElseIf, "elseif"),
            new(TokenType.End, "end"),
            new(TokenType.Function, "function"),
            new(TokenType.Goto, "goto"),
            new(TokenType.Return, "return"),
            new(TokenType.Print, "print"),
            new(TokenType.Local, "local"),
            new(TokenType.Name, "custom"),
            new(TokenType.Name, "name")
        };
        RunLexerTest(source, expect);
    }

    [Test]
    public void LexFunction()
    {
        const string source = @"
function foo()
end
";
        var expect = new List<Token>
        {
            new(TokenType.Function, "function"),
            new(TokenType.Name, "foo"),
            new(TokenType.LeftParen, "("),
            new(TokenType.RightParen, ")"),
            new(TokenType.End, "end"),
        };
        RunLexerTest(source, expect);
    }

    [Test]
    public void LexComments()
    {
        const string source = @"
local x = 2
-- This is a comment
print(x)
-- This won't be lexed.
";
        var expect = new List<Token>
        {
            new(TokenType.Local, "local"),
            new(TokenType.Name, "x"),
            new(TokenType.Equal, "="),
            new(TokenType.Number, "2"),
            new(TokenType.Print, "print"),
            new(TokenType.LeftParen, "("),
            new(TokenType.Name, "x"),
            new(TokenType.RightParen, ")")
        };
        RunLexerTest(source, expect);
    }

    private static void RunLexerTest(string source, IList<Token> expect)
    {
        var actual = Lexer.Lex(source);
        Assert.AreEqual(expect, actual);
    }
}