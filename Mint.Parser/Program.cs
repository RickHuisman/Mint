using Mint.Parser;
using Newtonsoft.Json;

const string source = @"
1 + 2 and true
";
var tokens = Lexer.Lex(source);
var chunk = Parser.ParseChunk(tokens);

var json = JsonConvert.SerializeObject(chunk, Formatting.Indented);
Console.WriteLine(json);