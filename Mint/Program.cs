using Mint.Compiler;
using Mint.VM;

var compiler = new Compiler();
var closure = compiler.Compile(@"
function foo()
    local x = 2 + 3
    return x
end

print(foo())
");
closure.Function.FunctionProto.Name = "main";
Console.WriteLine(closure.Function.FunctionProto);

var vm = new VM();
vm.Run(closure);