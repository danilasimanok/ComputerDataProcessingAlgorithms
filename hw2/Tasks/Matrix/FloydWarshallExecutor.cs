using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public static class FloydWarshallExecutor<T> {

        public static void checkMatrix(T[][] table) {
            Matrix<T>.CheckIsTableIsMatrix(table);
            if (table.Length != table[0].Length)
                throw new ArgumentException("Матрица должна быть квадратной.");
        }

        public static T[][] Execute(T[][] matrix, ISemigroupWithPartialOrder<T> sg) {
            T[][] result = new T[matrix.Length][];
            int i;
            for (i = 0; i < matrix.Length; ++i)
                result[i] = (T[])matrix[i].Clone();
            FloydWarshallExecutor<T>.checkMatrix(result);
            int j, k;
            for (i = 0; i < matrix.Length; ++i)
                for (j = 0; j < matrix.Length; ++j)
                    for (k = 0; k < matrix.Length; ++k) {
                        T alternative = sg.Multiply(result[i][k], result[k][j]);
                        result[i][j] = sg.LessOrEqual(alternative, result[i][j]) ? alternative : result[i][j];
                    }
            return result;
        }
    }
}
