using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Management.Automation;

namespace GraphNet
{
    public class DotGraph<TNode, TEdge> where TNode : IGraphNode where TEdge : IGraphEdge<TNode>
    {
        public ISet<TNode> Nodes { get; private set; }
        public ISet<TEdge> Edges { get; private set; }

        public DotGraph()
        {
            Nodes = new HashSet<TNode>();
            Edges = new HashSet<TEdge>();
        }

        public DotGraph(DotGraph<TNode, TEdge> dotGraph)
        {
            Nodes = new HashSet<TNode>(dotGraph.Nodes);
            Edges = new HashSet<TEdge>(dotGraph.Edges);
        }

        public bool AddNode(TNode node)
        {
            return Nodes.Add(node);
        }

        public bool AddEdge(TEdge edge)
        {
            var fromAdded = Nodes.Add(edge.From);
            var toAdded = Nodes.Add(edge.To);
            var edgeAdded = Edges.Add(edge);
            return fromAdded || toAdded || edgeAdded;
        }

        public DotGraph<TNode, TEdge> CombineWithGraph(DotGraph<TNode, TEdge> graph)
        {
            var combined = new DotGraph<TNode, TEdge>(this);
            foreach (var node in graph.Nodes)
            {
                combined.AddNode(node);
            }
            foreach (var edge in graph.Edges)
            {
                combined.AddEdge(edge);
            }
            return combined;
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            const string indent = "   ";

            output.AppendLine("digraph {\n");
            output.AppendLine();
            output.Append(indent);
            output.AppendLine("graph [fontname=\"helvetica\", fontsize=10];");
            output.Append(indent);
            output.AppendLine("node [fontname=\"helvetica\", fontsize=10];");
            output.Append(indent);
            output.AppendLine("edge [fontname=\"helvetica\", fontsize=8];");
            output.AppendLine();

            output.Append(indent);
            output.AppendLine("/* Nodes */");
            foreach (var node in Nodes)
            {
                output.Append(indent);
                output.AppendLine(node.GetDotMarkup());
            }
            output.AppendLine();

            output.Append(indent);
            output.AppendLine("/* Edges */");
            foreach (var edge in Edges)
            {
                output.Append(indent);
                output.AppendLine(edge.GetDotMarkup());
            }
            output.AppendLine();

            output.AppendLine("}");

            return output.ToString();
        }

        public string SaveDotGraph(string filename = "graph.gv")
        {
            var writer = new StreamWriter(new FileStream(filename, FileMode.Create));
            writer.Write(this);
            writer.Flush();
            writer.Close();
            return filename;
        }

        public string ToSvg(string dotfile = "graph.gv")
        {
            var ps = PowerShell.Create();
            ps.AddScript(String.Format("dot -Tsvg graph.gv"));
            var output = new StringBuilder();
            foreach (var result in ps.Invoke().Skip(6))
            {
                output.AppendLine(result.ToString());
            }
            ps.Commands.Clear();
            ps.Commands.AddCommand("rm").AddArgument(dotfile);
            ps.Invoke();
            ps.Dispose();
            return output.ToString();
        }

        public string SaveSvg(string filename = "graph.svg")
        {
            var writer = new StreamWriter(new FileStream(filename, FileMode.Create));
            writer.Write(ToSvg(SaveDotGraph()));
            writer.Flush();
            writer.Close();
            return filename;
        }
    }
}