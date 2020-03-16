using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpUtils
{
    static class FsharpMatrixConvertor<T> {
        public static T[][] ToTable(IEnumerable<IEnumerable<T>> matrix) {
            List<T[]> preresult = new List<T[]>();
            List<T> rowL;
            foreach (IEnumerable<T> row in matrix) {
                rowL = new List<T>();
                rowL.AddRange(row);
                preresult.Add(rowL.ToArray());
            }
            return preresult.ToArray();
        }
    }
}
