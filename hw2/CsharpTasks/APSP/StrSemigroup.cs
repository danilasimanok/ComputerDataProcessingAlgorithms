using Matrix;

namespace APSP
{
    public class StrSemigroup : ISemigroupWithPartialOrder<Str> {
        public Str GetInfty() => new Str("infty");

        public bool LessOrEqual(Str t1, Str t2) => t1.value.Length <= t2.value.Length;

        public Str Multiply(Str t1, Str t2) => t1.value.Length + t2.value.Length >= 5 ? this.GetInfty() : new Str(t1.value + t2.value);
    }
}
