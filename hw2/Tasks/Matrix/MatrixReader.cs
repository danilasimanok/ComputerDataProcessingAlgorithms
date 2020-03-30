using System;
using System.Collections.Generic;
using System.IO;

namespace Matrix
{
    public class MatrixReader<T> where T : ISerializable, new()
    {
        private String input;
        private T[][] result;

        public MatrixReader(String input){
            this.input = input;
            this.result = null;
        }

        public T[][] GetResult() {
            if (this.result != null)
                return TableCopier<T>.Copy(this.result);
            List<T[]> lines = new List<T[]>();
            foreach (String str in File.ReadLines(this.input)) {
                if (str.Length == 0)
                    break;
                List<T> line = new List<T>();
                foreach (String word in str.Split(" ")) {
                    T t = new T();
                    t.FromWord(word);
                    line.Add(t);
                }
                lines.Add(line.ToArray());
            }
            this.result = lines.ToArray();
            return this.GetResult();
        }
    }
}
