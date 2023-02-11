using Mint.VM;

const string source = @"
local x = 0
if true ~= false then
    x = 2
end

print(x)
";
var value = VM.Interpret(source, true);
Console.WriteLine(value);