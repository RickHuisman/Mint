using Newtonsoft.Json;
using NUnit.Framework;

namespace Mint.Test;

public static class TestHelper
{
    public static void AreEqual(object expected, object actual)
    {
        var expectedJson = JsonConvert.SerializeObject(expected);
        var actualJson = JsonConvert.SerializeObject(actual);
        Assert.AreEqual(expectedJson, actualJson);
    }

    public static string AsJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}