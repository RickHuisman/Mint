using Mint.VM;

const string source = @" -- foobar
-- This is a test
local a = not true
-- Hello
return a == false";
var value = VM.Interpret(source);
Console.WriteLine(value);