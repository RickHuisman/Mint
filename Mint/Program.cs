using Mint.VM;

const string source = @"
print(not not nil)
";
var value = VM.Interpret(source);
Console.WriteLine(value);