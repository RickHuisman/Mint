using Mint.Compiler;
using Mint.VM;

var compiler = new Compiler();
var functionProto = compiler.Compile("5 != 5");

var vm = new VM();
vm.Run(functionProto);
Console.WriteLine(vm.Peek());