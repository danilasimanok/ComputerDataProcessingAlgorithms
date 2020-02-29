using System;

namespace MatrixMultiplier
{
    class Program
    {
        static void Main(String[] args)
        {
            String path1 = args[0],
                path2 = args[1];
            Matrix matrix1 = MatrixReader.ReadMatrix(path1),
                matrix2 = MatrixReader.ReadMatrix(path2);
            Console.WriteLine(matrix1 * matrix2);
        }
    }
}