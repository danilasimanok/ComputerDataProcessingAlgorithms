using System;
using Matrix;

namespace MatrixMultiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            String m1_input = Console.ReadLine(),
                m2_input = Console.ReadLine(),
                output = Console.ReadLine();
            Natural[][] m1 = new MatrixReader<Natural>(m1_input).GetResult(),
                m2 = new MatrixReader<Natural>(m2_input).GetResult();
            try {
                Natural[][] m = Matrix<Natural>.multiply(m1, m2, new NaturalSemiring());
                MatrixWriter<Natural>.WriteMatrix(m, output);
            } catch (ArgumentException exception) {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
