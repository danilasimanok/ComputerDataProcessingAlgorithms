using System;
using MatrixMultiplier;
using QuickGraph;

namespace Pathfinder_joke_
{
    public class GraphCreator
    {
        private int[][] matrix;
        private ArrayAdjacencyGraph<int,Edge<int>> graph;
        private Func<Edge<int>, double> edgeCost;
        
        public GraphCreator(Matrix matrix) {
            if (matrix.m != matrix.n)
                throw new ArgumentException("Adjacency matrix should be square.");
            this.matrix = matrix.GetContent();
            this.graph = null;
            this.edgeCost = null;
        }

        public (IVertexAndEdgeListGraph<int, Edge<int>>, Func<Edge<int>, double>) GetResult() {
            if (this.graph != null)
                return (this.graph, this.edgeCost);
            AdjacencyGraph<int, Edge<int>> graph = new AdjacencyGraph<int, Edge<int>>();
            int i, j;
            for (i = 0; i < this.matrix.Length; ++i)
                for (j = 0; j < this.matrix[i].Length; ++j)
                    if (matrix[i][j] >= 0)
                        graph.AddVerticesAndEdge(new Edge<int>(i, j));
            this.edgeCost = edge => matrix[edge.Source][edge.Target];
            this.graph = new ArrayAdjacencyGraph<int, Edge<int>>(graph);
            return this.GetResult();
        }
    }
}