using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public static class FloydWarshallExecutor<T> {

        public static void checkMatrix(T[][] table) {
            if (table.Length != table[0].Length)
                throw new ArgumentException("Матрица должна быть квадратной.");
        }

        public static Matrix<T> Execute(Matrix<T> matrix, ISemigroupWithPartialOrder<T> sg) {
            T[][] result = matrix.GetTable();
            FloydWarshallExecutor<T>.checkMatrix(result);
            int i, j, k;
            for (i = 0; i < matrix.Height; ++i)
                for (j = 0; j < matrix.Height; ++j)
                    for (k = 0; k < matrix.Height; ++k) {
                        T alternative = sg.Multiply(result[i][k], result[k][j]);
                        result[i][j] = sg.LessOrEqual(alternative, result[i][j]) ? alternative : result[i][j];
                    }
            return new Matrix<T>(result);
        }
    }
}
