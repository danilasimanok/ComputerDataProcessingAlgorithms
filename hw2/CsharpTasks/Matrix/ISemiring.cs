namespace Matrix
{
    public interface ISemiring<T>
    {
        public T GetIdentityElement();

        public T Add(T t1, T t2);

        public T Multiply(T t1, T t2);
    }
}
