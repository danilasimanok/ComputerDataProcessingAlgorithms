using System;

namespace MatrixMultiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            String path1 = Console.ReadLine(),
                path2 = Console.ReadLine();
            Matrix matrix1 = MatrixReader.ReadMatrix(path1),
                matrix2 = MatrixReader.ReadMatrix(path2);
            Console.WriteLine(matrix1 * matrix2);
        }
    }
}