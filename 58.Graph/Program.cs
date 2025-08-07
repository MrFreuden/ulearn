using System.Diagnostics;

namespace FluentApi.Graph;

class Program
{
	private const string PathToGraphviz = @"D:\Program Files\Graphviz\bin\dot.exe";
    static void Main(string[] args)
	{
		var dot =
			DotGraphBuilder.DirectedGraph("CommentParser")
				.AddNode("START").With(a => a.Shape(NodeShape.Ellipse).Color("green"))
				.AddNode("comment").With(a => a.Shape(NodeShape.Box))
				.AddEdge("START", "slash").With(a => a.Label("'/'"))
				.AddEdge("slash", "comment").With(a => a.Label("'/'"))
				.AddEdge("comment", "comment").With(a => a.Label("other chars"))
				.AddEdge("comment", "START").With(a => a.Label("'\\\\n'"))
				.Build();
		Console.WriteLine(dot);
		ShowRenderedGraph(dot);
	}

	private static void ShowRenderedGraph(string dot)
	{
		File.WriteAllText("comment.dot", dot);
		var processStartInfo = new ProcessStartInfo(PathToGraphviz)
		{
			UseShellExecute = false,
			Arguments = "comment.dot -Tpng -o comment.png",
			RedirectStandardError = true,
			RedirectStandardOutput = true,
		};
		var p = Process.Start(processStartInfo);
		p.WaitForExit();
		Console.WriteLine(p.StandardError.ReadToEnd());
		Console.WriteLine("Result is saved to comment.png");
		//Process.Start("comment.png");
	}
}