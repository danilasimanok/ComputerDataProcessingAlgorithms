using Matrix;

namespace TransitiveClosure
{
    public class BooleanSemigroup : ISemigroupWithPartialOrder<Boolean>
    {
        public Boolean GetInfty() => new Boolean(false);

        public bool LessOrEqual(Boolean t1, Boolean t2) => 
            t1.value || !t2.value;

        public Boolean Multiply(Boolean t1, Boolean t2) =>
            new Boolean(t1.value && t2.value);
    }
}
