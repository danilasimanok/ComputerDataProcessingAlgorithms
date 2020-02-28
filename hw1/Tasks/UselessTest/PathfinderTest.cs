using MatrixMultiplier;
using NUnit.Framework;
using Pathfinder_joke_;
using QuickGraph;
using QuickGraph.Algorithms;
using System;
using System.Collections.Generic;

namespace TasksTests
{
    [TestFixture]
    class PathfinderTest
    {
        IVertexAndEdgeListGraph<int, Edge<int>> graph;
        Func<Edge<int>, double> edgeCost;

        public PathfinderTest() {
            Matrix matrix = new Matrix(
                new int[][] {
                    new int[] {-1, 10, 30, 50, 10},
                    new int[] {-1, -1, -1, -1, -1},
                    new int[] {-1, -1, -1, -1, 10},
                    new int[] {-1, 40, 20, -1, -1},
                    new int[] {10, -1, 10, 30, -1}
                });
            (this.graph, this.edgeCost) = new GraphCreator(matrix).GetResult();
        }

        [Test]
        public void GraphCreatorTest() {
            
            int[][] invalid = new int[][] {
                new int[] {1, 2, 6},
                new int[] {3, 4, 5}
            };
            Assert.Throws<ArgumentException>(
                delegate {
                    new GraphCreator(new Matrix(invalid));
                });
            Edge<int> edge0 = new Edge<int>(0, 3), 
                edge1 = new Edge<int>(2, 4),
                edge2 = new Edge<int>(4, 1);
            Assert.IsTrue(this.graphContainsEdge(this.graph, edge0) && this.edgeCost(edge0) == 50);
            Assert.IsTrue(this.graphContainsEdge(this.graph, edge1) && this.edgeCost(edge1) == 10);
            Assert.IsFalse(this.graphContainsEdge(this.graph, edge2));
            for (int i = 0; i < 5; ++i)
                Assert.IsTrue(this.graph.ContainsVertex(i));
        }

        private bool graphContainsEdge(IVertexAndEdgeListGraph<int, Edge<int>> graph, Edge<int> edge) {
            foreach (Edge<int> e in graph.Edges)
                if (e.Source == edge.Source && e.Target == edge.Target)
                    return true;
            return false;
        }

        [Test]
        public void TestShortestPaths() {
            TryFunc<int, IEnumerable<Edge<int>>> tryGetPath = this.graph.ShortestPathsDijkstra(this.edgeCost, 0);
            IEnumerable<Edge<int>> path;
            Assert.IsFalse(tryGetPath(0, out path));
            tryGetPath(2, out path);
            int[] pattern = new int[] {4, 2};
            Assert.IsTrue(this.checkPath(path, pattern));
        }

        private bool checkPath(IEnumerable<Edge<int>> path, IEnumerable<int> pattern) {
            IEnumerator<Edge<int>> edgesEnumerator = path.GetEnumerator();
            IEnumerator<int> patternEnumerator = pattern.GetEnumerator();
            bool edgesHasNext = edgesEnumerator.MoveNext(),
                patternHasNext = patternEnumerator.MoveNext();
            while (edgesHasNext && patternHasNext) {
                if (edgesEnumerator.Current.Target != patternEnumerator.Current)
                    return false;
                edgesHasNext = edgesEnumerator.MoveNext();
                patternHasNext = patternEnumerator.MoveNext();
            }
            return !(edgesHasNext || patternHasNext);
        }
    }
}