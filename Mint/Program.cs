using Mint.VM;

const string source = @"
print(10)
";
// const string source = @"
// return 1 + 2 and false
// ";
var value = VM.Interpret(source, true);
Console.WriteLine(value);