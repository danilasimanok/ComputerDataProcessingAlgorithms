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
                throw new ArgumentException("Матрица должна содержать хотя бы один элемент.");
            for (int i = 1; i < table.Length; ++i)
                if (table[i].Length != table[0].Length)
                    throw new ArgumentException("Длины строк матрицы должны быть одинаковыми.");
            this.table = table;
        }

        public T this[int i, int j] {
            get => this.table[i][j];
        }

        public static Matrix<T> multiply(Matrix<T> m1, Matrix<T> m2, ISemiring<T> sr) {
            if (m1.Width != m2.Height)
                throw new ArgumentException("Длина строк первой матрицы должна быть равна высоте столбцов второй.");
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
