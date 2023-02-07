using Mint.Compiler;
using Mint.VM;

var compiler = new Compiler();
// var functionProto = compiler.Compile(@"
// local foo = 2
//
// do
//     print(foo)
//     local bar = 3
//     print(bar)
// end
//
// print(bar)
// ");

var closure = compiler.Compile(@"
function foo()
    return 5
end

print(foo())
");
closure.Function.FunctionProto.Name = "main";
Console.WriteLine(closure.Function.FunctionProto);

var vm = new VM();
vm.Run(closure);