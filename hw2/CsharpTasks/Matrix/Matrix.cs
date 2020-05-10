using System;

namespace Matrix
{
    public class Matrix<T>
    {
        private T[][] table;
        public int Width => this.table[0].Length;
        public int Height => this.table.Length;

        public Matrix(T[][] table) {
            if (table.Length == 0 || table[0].Length == 0)
                throw new ArgumentException("Matrix should contain at least one element.");
            for (int i = 1; i < table.Length; ++i)
                if (table[i].Length != table[0].Length)
                    throw new ArgumentException("Lengths of each row should be equal.");
            this.table = table;
        }

        public T this[int i, int j] {
            get => this.table[i][j];
        }

        public static Matrix<T> Multiply(Matrix<T> m1, Matrix<T> m2, ISemiring<T> sr) {
            if (m1.Width != m2.Height)
                throw new ArgumentException("Width of first matrix should equal to height of second one.");
            int height = m1.Height,
                width = m2.Width;
            T[][] result = new T[height][];
            int i, j, k;
            for (i = 0; i < height; ++i) {
                T[] row = new T[width];
                for (j = 0; j < width; ++j) {
                    row[j] = sr.GetIdentityElement();
                    for (k = 0; k < width; ++k)
                        row[j] = sr.Add(row[j], sr.Multiply(m1[i, k], m2[k, j]));
                }
                result[i] = row;
            }
            return new Matrix<T>(result);
        }

        public T[][] GetTable() => TableCopier<T>.Copy(this.table);
    }
}
