using Mint.VM;

const string source = @"
local a = true
local b = false
print(a == b)
";
var value = VM.Interpret(source);
Console.WriteLine(value);