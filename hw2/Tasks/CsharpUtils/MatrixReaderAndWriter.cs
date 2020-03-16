using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsharpUtils
{
    static class MatrixReaderAndWriter<T> where T: ISerializable, new()
    {
        public static T[][] ReadMatrix(String input) {
            IEnumerable<String> lines = File.ReadLines(input);
            List<T[]> rows = new List<T[]>();
            List<T> elements;
            T element;
            foreach (String line in lines) {
                elements = new List<T>();
                foreach (String word in line.Split(" ")) {
                    element = new T();
                    element.FromWord(word);
                    elements.Add(element);
                }
                rows.Add(elements.ToArray());
            }
            return rows.ToArray();
        }

        public static void WriteMatrix(T[][] matrix, String output) {
            StringBuilder builder = new StringBuilder();
            List<String> words;
            foreach (T[] row in matrix) {
                words = new List<String>();
                foreach (T element in row)
                    words.Add(element.ToWord());
                builder.AppendJoin<String>(' ', words);
                builder.AppendLine();
            }
            File.WriteAllText(output, builder.ToString());
        }
    }
}
