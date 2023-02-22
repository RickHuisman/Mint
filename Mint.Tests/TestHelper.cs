using Newtonsoft.Json;
using NUnit.Framework;

namespace Mint.Tests;

public static class TestHelper
{
    public static void AreEqual(object expected, object actual)
    {
        var expectedJson = AsJson(expected);
        var actualJson = AsJson(actual);
        Assert.AreEqual(expectedJson, actualJson);
    }

    private static string AsJson(object obj) => JsonConvert.SerializeObject(obj);
}