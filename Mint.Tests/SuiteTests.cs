using System;
using System.IO;
using System.Linq;
using Mint.Compiler;
using NUnit.Framework;

namespace Mint.Tests;

public class SuiteTests
{
    [Test]
    public void RunSuiteTests()
    {
        var testFiles = Directory
            .GetFiles("/Users/rickhuisman/dev/C#/Mint/Mint.Tests/tests")
            .Select(f => (FileName: f, Content: File.ReadAllText(f)));

        foreach (var (name, content) in testFiles)
        {
            Console.WriteLine($"Running file: {name}");
            RunTest(content);
        }
    }

    private static void RunTest(string source)
    {
        var value = Mint.Interpret(source);
        TestHelper.AreEqual(value, new Value(true));
    }
}