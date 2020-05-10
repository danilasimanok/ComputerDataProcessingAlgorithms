namespace Matrix
{
    public interface ISemigroupWithPartialOrder<T> {
        public T Multiply(T t1, T t2);

        public T GetInfty();

        public bool LessOrEqual(T t1, T t2);
    }
}
