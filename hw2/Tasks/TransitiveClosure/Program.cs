using System;
using System.Diagnostics;
using System.IO;
using Matrix;

namespace TransitiveClosure
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = Console.ReadLine(),
                output = Console.ReadLine(),
                dotfile = output + ".tmp";
            Boolean[][] origin = new MatrixReader<Boolean>(input).GetResult(),
                result = FloydWarshallExecutor<Boolean>.Execute(origin, new BooleanSemigroup());
            DotGenerator.createDot(origin, result, dotfile);
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dot";
                process.StartInfo.Arguments = "-Tpdf -o" + output + " " + dotfile;
                process.Start();
                while (!process.HasExited)
                    process.Refresh();
            }
            File.Delete(dotfile);
        }
    }
}
