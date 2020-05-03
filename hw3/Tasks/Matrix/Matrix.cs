using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Matrix
{
    public struct Matrix<T> {
        MatrixType type;
        IList<IList<T>> lists;
    }

    public enum MatrixType {
        ROWS, COLUMNS
    }
}
