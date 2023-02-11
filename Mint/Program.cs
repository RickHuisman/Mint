using Mint.VM;

const string source = @"
local y = 0
if true and true then
    x = 2
end

return x == 2 and true
";
var value = VM.Interpret(source, true);
Console.WriteLine(value);