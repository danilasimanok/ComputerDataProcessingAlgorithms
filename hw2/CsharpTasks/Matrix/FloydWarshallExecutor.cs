using System;

namespace Matrix
{
    public static class FloydWarshallExecutor<T> {
        public static Matrix<T> Execute(Matrix<T> matrix, ISemigroupWithPartialOrder<T> sg) {
            if (matrix.Width != matrix.Height)
                throw new ArgumentException("Matrix should be square.");
            T[][] result = new T[matrix.Height][];
            int i, j, k;
            for (i = 0; i < matrix.Height; ++i) {
                result[i] = new T[matrix.Height];
                for (j = 0; j < matrix.Height; ++j)
                {
                    result[i][j] = matrix[i, j];
                    for (k = 0; k < matrix.Height; ++k)
                    {
                        T alternative = sg.Multiply(matrix[i, k], matrix[k, j]);
                        result[i][j] = sg.LessOrEqual(alternative, result[i][j]) ? alternative : result[i][j];
                    }
                }
            }
            return new Matrix<T>(result);
        }
    }
}
