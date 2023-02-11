using Mint.VM;

const string source = @"
print(not true)
";
var value = VM.Interpret(source);
Console.WriteLine(value);