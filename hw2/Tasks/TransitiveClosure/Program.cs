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
            try {
                Boolean[][] origin = new MatrixReader<Boolean>(input).GetResult(),
                    result = FloydWarshallExecutor<Boolean>.Execute(new Matrix<Boolean>(origin), new BooleanSemigroup()).GetTable();
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
            } catch (Exception exception) {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
