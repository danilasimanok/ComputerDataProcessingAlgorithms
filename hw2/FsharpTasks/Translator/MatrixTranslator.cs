using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator
{
    public class MatrixTranslator<T1, T2> {
        private Func<T1, T2> t1ToT2;

        public MatrixTranslator(Func<T1, T2> t1ToT2) {
            this.t1ToT2 = t1ToT2;
        }

        public T2[][] Translate(IEnumerable<IEnumerable<T1>> lists) {
            List<T2[]> result = new List<T2[]>();
            foreach (IEnumerable<T1> ts1 in lists)
                result.Add(ts1.Select(this.t1ToT2).ToArray());
            return result.ToArray();
        }
    }
}
