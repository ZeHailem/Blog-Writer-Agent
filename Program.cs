// See https://aka.ms/new-console-template for more information
using Video_Blog_Writer;

var workflow = new BlogWriterWorkFlow();
workflow.ExcuteWorkFlow("https://www.youtube.com/watch?v=JT6YT93gRo8").GetAwaiter().GetResult(); ;