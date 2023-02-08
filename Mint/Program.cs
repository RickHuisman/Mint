using Mint.Compiler;
using Mint.VM;

var compiler = new Compiler();
var closure = compiler.Compile(@"
x = 1 + 2 - 3 * 8 / 4
print(x)
");
closure.Function.FunctionProto.Name = "main";
Console.WriteLine(closure.Function.FunctionProto);

var vm = new VM();
vm.Run(closure);