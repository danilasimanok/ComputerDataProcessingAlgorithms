using Matrix;

namespace MatrixMultiplier
{
    public class NaturalSemiring : ISemiring<Natural> {
        public Natural Add(Natural t1, Natural t2) => new Natural(t1.value + t2.value);

        public Natural GetIdentityElement() => new Natural(0);

        public Natural Multiply(Natural t1, Natural t2) => new Natural(t1.value * t2.value);
    }
}
