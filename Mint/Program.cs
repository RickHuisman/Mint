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

var function = compiler.Compile(@"
print(2)

function foo()
    print(3)
end

function bar()
    print(3)
end

print(4)
foo()

print(5)
bar()
");

var vm = new VM();
vm.Run(function);