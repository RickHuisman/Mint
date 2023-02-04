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

var functionProto = compiler.Compile(@"
20 == 20
");


Console.WriteLine(functionProto);
var vm = new VM();
vm.Run(functionProto);
Console.WriteLine(vm.Peek()!);