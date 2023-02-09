using Mint.VM;

const string source = @"
x = 3
return x == 2
";
var value = VM.Interpret(source);
Console.WriteLine(value);