using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    static class TableCopier<T> {
        public static T[][] Copy(T[][] origin) {
            T[][] copy = new T[origin.Length][];
            for (int i = 0; i < copy.Length; ++i)
                copy[i] = (T[])origin[i].Clone();
            return copy;
        }
    }
}
