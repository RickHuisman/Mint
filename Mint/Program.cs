using Mint.VM;

const string source = @"
local a = 2
local b = 3
print(a > b)
print(a < b)
";
var value = VM.Interpret(source);
Console.WriteLine(value);