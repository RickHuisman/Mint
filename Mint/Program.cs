using Mint.VM;

const string source = @"
local a = 2
a = a + 2
print(a)
";
var value = VM.Interpret(source);
Console.WriteLine(value);