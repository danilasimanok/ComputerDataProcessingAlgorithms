using System;
using Matrix;

namespace APSP
{
    class Program
    {
        static void Main(string[] args) {
            String input = Console.ReadLine(),
                output = Console.ReadLine();
            Str[][] origin = new MatrixReader<Str>(input).GetResult(),
                result = FloydWarshallExecutor<Str>.Execute(origin, new StrSemigroup());
            MatrixWriter<Str>.WriteMatrix(result, output);
        }
    }
}
