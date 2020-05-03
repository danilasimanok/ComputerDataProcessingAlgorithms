using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public static class Functions<T> {
        private static Func<IList<IList<T>>, IList<IList<T>>> transposeLists = lists => {
            Func<IList<IList<T>>, IList<T>, IList<IList<T>>, IList<T>, IList<IList<T>>> inner =
                (lists, acc, resAcc, newLists) => {
                    switch (lists.Count) {
                        case 0:
                            resAcc.Add(acc);
                            return inner(newLists, new List<T>(), resAcc, new List<T>());
                    }
                };
        };
    }
}
