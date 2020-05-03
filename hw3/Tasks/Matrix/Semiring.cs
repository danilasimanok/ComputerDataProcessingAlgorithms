using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix
{
    public struct Semiring<T> {
        public Func<T, T, T> Add;
        public Func<T, T, T> Multiply;
        public T IdentityElement;
    }
}
