using System;
using System.IO;
using System.Linq;
using Mint.Compiler;
using Mint.Test;
using NUnit.Framework;
using ValueType = Mint.Compiler.ValueType;

namespace Mint.Tests;

public class SuiteTests
{
    [Test]
    public void RunSuiteTests()
    {
        var testFiles = Directory
            .GetFiles("/Users/rickhuisman/dev/C#/Mint/Mint.Tests/tests")
            .Select(File.ReadAllText);

        foreach (var file in testFiles)
        {
            RunTest(file);
        }
    }

    private static void RunTest(string source)
    {
        var value = Mint.Interpret(source);
        TestHelper.AreEqual(value, new Value(true));
    }
}