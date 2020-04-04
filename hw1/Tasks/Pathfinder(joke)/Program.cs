using QuickGraph;
using QuickGraph.Algorithms;
using System;
using MatrixMultiplier;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace Pathfinder_joke_
{
    class Program
    {
        static void Main(String[] args)
        {
            try {
                String path = args[0];
                (IVertexAndEdgeListGraph<int, Edge<int>> graph,
                    Func<Edge<int>, double> edgeCost) = new GraphCreator(MatrixReader.ReadMatrix(path)).GetResult();
                TryFunc<int, IEnumerable<Edge<int>>> tryGetPath = graph.ShortestPathsDijkstra(edgeCost, 0);
                DotGenerator<int, Edge<int>> generator = new DotGenerator<int, Edge<int>>(graph, tryGetPath);
                (new PDFGenerator(generator.GetDotCode(), args[1])).GeneratePDF();
            } catch (IndexOutOfRangeException exn) {
                Console.Error.WriteLine("Path to adjacency matrix should be given.");
            } catch (Exception exn) {
                Console.Error.WriteLine(exn.Message);
            }
        }
    }
}