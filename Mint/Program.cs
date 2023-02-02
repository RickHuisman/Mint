using Mint.Compiler;
using Mint.VM;

var compiler = new Compiler();
var functionProto = compiler.Compile(@"
foo = 5
bar = 5
foo = 7
hello = foo + bar
");

Console.WriteLine(functionProto);
var vm = new VM();
vm.Run(functionProto);
Console.WriteLine(vm.Peek());