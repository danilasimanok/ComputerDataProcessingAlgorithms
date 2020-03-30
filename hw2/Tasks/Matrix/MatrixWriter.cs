using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Matrix
{
    public static class MatrixWriter<T> where T : ISerializable
    {
        public static void WriteMatrix(T[][] matrix, String output) {
            List<String> lines = new List<String>();
            foreach (T[] row in matrix) {
                StringBuilder builder = new StringBuilder();
                List<String> line = new List<String>();
                foreach (T t in row)
                    line.Add(t.ToWord());
                builder.AppendJoin(' ', line);
                lines.Add(builder.ToString());
            }
            File.WriteAllLines(output, lines);
        }
    }
}
