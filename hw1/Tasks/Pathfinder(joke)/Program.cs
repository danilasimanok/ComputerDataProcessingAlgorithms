using QuickGraph;
using QuickGraph.Algorithms;
using System;
using MatrixMultiplier;
using System.Collections.Generic;
using System.IO;

namespace Pathfinder_joke_
{
    class Program
    {
        static void Main(string[] args)
        {
            String path = Console.ReadLine();
            (IVertexAndEdgeListGraph<int, Edge<int>> graph,
                Func<Edge<int>, double> edgeCost) = new GraphCreator(MatrixReader.ReadMatrix(path)).GetResult();
            TryFunc<int, IEnumerable<Edge<int>>> tryGetPath = graph.ShortestPathsDijkstra(edgeCost, 0);
            DotGenerator<int, Edge<int>> generator = new DotGenerator<int, Edge<int>>(graph, tryGetPath);
            String dot = generator.GetDotCode(),
                output = Console.ReadLine();
            using (FileStream fs = new FileStream(output, FileMode.OpenOrCreate))
                fs.Write(System.Text.Encoding.Default.GetBytes(dot));
        }
    }
}