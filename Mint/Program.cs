using Mint.VM;

const string source = @" -- foobar
-- This is a test
local a = not true -- false
-- Hello
return a == false";
var value = VM.Interpret(source, true);
Console.WriteLine(value);