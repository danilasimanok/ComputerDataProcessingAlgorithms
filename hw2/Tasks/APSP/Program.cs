using System;
using Matrix;

namespace APSP
{
    class Program
    {
        static void Main(string[] args) {
            String input = Console.ReadLine(),
                output = Console.ReadLine();
            try {
                Str[][] origin = new MatrixReader<Str>(input).GetResult(),
                result = FloydWarshallExecutor<Str>.Execute(new Matrix<Str>(origin), new StrSemigroup()).GetTable();
                MatrixWriter<Str>.WriteMatrix(result, output);
            } catch (ArgumentException exception) {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
