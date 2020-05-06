using QuickGraph;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinder_joke_
{
    public class DotGenerator<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        private IVertexAndEdgeListGraph<TVertex, TEdge> graph;
        private TryFunc<TVertex, IEnumerable<TEdge>> tryGetPath;
        private String dotCode;

        public DotGenerator(IVertexAndEdgeListGraph<TVertex, TEdge> graph, TryFunc<TVertex, IEnumerable<TEdge>> tryGetPath) {
            this.graph = graph;
            this.tryGetPath = tryGetPath;
            this.dotCode = null;
        }

        public String GetDotCode() {
            if (this.dotCode != null)
                return this.dotCode;
            HashSet<TEdge> shortestTree = new HashSet<TEdge>();
            IEnumerable<TEdge> edges;
            foreach (TVertex vertex in this.graph.Vertices)
                if (this.tryGetPath(vertex, out edges))
                    shortestTree.UnionWith(edges);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("digraph G {");
            foreach (TVertex vertex in this.graph.Vertices) {
                builder.Append(vertex.ToString());
                builder.AppendLine(";");
            }
            foreach (TEdge edge in this.graph.Edges) {
                builder.Append(edge.ToString());
                if (shortestTree.Contains(edge))
                    builder.AppendLine(" [color = red];");
                else
                    builder.AppendLine(" [];");
            }
            builder.AppendLine("}");
            this.dotCode = builder.ToString();
            return this.GetDotCode();
        }
    }
}