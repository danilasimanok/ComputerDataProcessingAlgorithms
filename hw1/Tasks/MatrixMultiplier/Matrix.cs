using System;
using System.Text;

namespace MatrixMultiplier
{
    public class Matrix
    {
        private int[][] content;
        public int n { get; private set; }
        public int m { get; private set; }

        public int[][] GetContent() => (int[][])this.content.Clone();

        private void notZero(int rowsOrColumns) {
            if (rowsOrColumns == 0)
                throw new ArgumentException("В матрице должна содержаться хотя бы одна клетка.");
        }

        public Matrix(int[][] content) {
            int n = content.Length;
            this.notZero(n);
            int m = content[0].Length;
            this.notZero(m);
            foreach (int[] line in content)
                if (line.Length != m)
                    throw new ArgumentException("Размеры строк должны быть одинаковыми.");
            this.n = n;
            this.m = m;
            this.content = content;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            int n = a.n,
                m = b.m;
            if (a.m != b.n)
                throw new ArgumentException("Количество столбцов первой и строк второй матрицы должно совпадать.");
            int[][] content = new int[n][];
            int i, j, k;
            for (i = 0; i < n; ++i)
            {
                content[i] = new int[m];
                for (j = 0; j < m; ++j)
                {
                    content[i][j] = 0;
                    for (k = 0; k < a.m; ++k)
                        content[i][j] += a.content[i][k] * b.content[k][j];
                }
            }
            return new Matrix(content);
        }

        public override bool Equals(Object obj) {
            if (obj == null)
                return false;
            int[][] content = (obj as Matrix).content;
            if (content.Length != this.n || content[0].Length != this.m)
                return false;
            int i, j;
            for (i = 0; i < this.n; ++i)
                for (j = 0; j < this.m; ++j)
                    if (this.content[i][j] != content[i][j])
                        return false;
            return true;
        }

        public override int GetHashCode() => this.content.GetHashCode();

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            foreach (int[] line in this.content)
            {
                foreach (int i in line) {
                    builder.Append(i.ToString());
                    builder.Append(' ');
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}