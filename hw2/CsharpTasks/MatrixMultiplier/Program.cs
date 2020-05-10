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
            Natural[][] t1 = new MatrixReader<Natural>(m1_input).GetResult(),
                t2 = new MatrixReader<Natural>(m2_input).GetResult();
            try {
                Matrix<Natural> m1 = new Matrix<Natural>(t1),
                    m2 = new Matrix<Natural>(t2);
                Matrix<Natural> m = Matrix<Natural>.Multiply(m1, m2, new NaturalSemiring());
                MatrixWriter<Natural>.WriteMatrix(m.GetTable(), output);
            } catch (ArgumentException exception) {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
