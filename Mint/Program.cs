using Mint.Compiler;
using Mint.VM;

var compiler = new Compiler();
var closure = compiler.Compile(@"
local y = 6

function foo()
    local x = 2
    local y = 3
    return x + y
end

local z = foo() + y

print(z)
");
closure.Function.FunctionProto.Name = "main";
Console.WriteLine(closure.Function.FunctionProto);

var vm = new VM();
vm.Run(closure);