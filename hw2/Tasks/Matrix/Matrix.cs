using System;

namespace Matrix
{
    public static class Matrix<T>
    {
        public static void CheckIsTableIsMatrix(T[][] table) {
            if (table.Length == 0 || table[0].Length == 0)
                throw new ArgumentException("Матрица должна содержать хотя бы один элемент.");
            for (int i = 1; i < table.Length; ++i)
                if (table[i].Length != table[0].Length)
                    throw new ArgumentException("Длины строк матрицы должны быть одинаковыми.");
        }

        public static T[][] multiply(T[][] m1, T[][] m2, ISemiring<T> sr) {
            Matrix<T>.CheckIsTableIsMatrix(m1);
            Matrix<T>.CheckIsTableIsMatrix(m2);
            if (m1[0].Length != m2.Length)
                throw new ArgumentException("Длина строк первой матрицы должна быть равна высоте столбцов второй.");
            int h = m1.Length,
                w = m2[0].Length;
            T[][] result = new T[h][];
            int i, j, k;
            for (i = 0; i < h; ++i) {
                T[] row = new T[w];
                for (j = 0; j < w; ++j) {
                    row[j] = sr.GetIdentityElement();
                    for (k = 0; k < w; ++k)
                        row[j] = sr.Add(row[j], sr.Multiply(m1[i][k], m2[k][j]));
                }
                result[i] = row;
            }
            return result;
        }
    }
}
