using Mint.VM;

const string source = @"
print(not true)
print(-5)
";
var value = VM.Interpret(source);
Console.WriteLine(value);