using System;
using System.Collections.Generic;
using System.IO;

namespace MatrixMultiplier
{
    public static class MatrixReader
    {
        public static Matrix ReadMatrix(String fileName) {
            List<int[]> lines = new List<int[]>();
            using (StreamReader reader = File.OpenText(fileName)) {
                String str = reader.ReadLine();
                int i;
                while (str != null) {
                    String[] numbers = str.Split(" ");
                    int[] line = new int[numbers.Length];
                    for (i = 0; i < numbers.Length; ++i)
                        line[i] = int.Parse(numbers[i]);
                    lines.Add(line);
                    str = reader.ReadLine();
                }
            }
            return new Matrix(lines.ToArray());
        }
    }
}