using Mint.VM;

const string source = @"
local a = not true
return a == false";
var value = VM.Interpret(source);
Console.WriteLine(value);