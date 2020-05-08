using System;
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
                DotMediator.CreateDot(origin, result, dotfile);
                DotMediator.ProcessDot(dotfile, output);
                File.Delete(dotfile);
            } catch (Exception exception) {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
