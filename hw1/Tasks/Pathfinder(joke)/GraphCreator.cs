using System;
using MatrixMultiplier;
using QuickGraph;

namespace Pathfinder_joke_
{
    public class GraphCreator
    {
        private int[][] matrix;
        private AdjacencyGraph<int,Edge<int>> graph;
        private Func<Edge<int>, double> edgeCost;
        
        public GraphCreator(Matrix matrix) {
            if (matrix.m != matrix.n)
                throw new ArgumentException("Матрица смежности должна быть квадратной.");
            this.matrix = matrix.GetContent();
            this.graph = null;
            this.edgeCost = null;
        }

        public (IVertexAndEdgeListGraph<int, Edge<int>>, Func<Edge<int>, double>) GetResult() {
            if (this.graph != null)
                return (this.graph.Clone(), this.edgeCost);
            this.graph = new AdjacencyGraph<int, Edge<int>>();
            int i, j;
            for (i = 0; i < this.matrix.Length; ++i)
                this.graph.AddVertex(i);
            for (i = 0; i < this.matrix.Length; ++i)
                for (j = 0; j < this.matrix[i].Length; ++j)
                    if (matrix[i][j] >= 0)
                        this.graph.AddEdge(new Edge<int>(i, j));
            this.edgeCost = edge => matrix[edge.Source][edge.Target];
            return this.GetResult();
        }
    }
}