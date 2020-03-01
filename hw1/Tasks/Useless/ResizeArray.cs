using System;

namespace Useless
{
    public class ResizeArray<T>
    {
        private T[] content;
        private int count;
        static int DEFAULT_LENGTH = 10;

        public ResizeArray()
        {
            this.count = 0;
            this.content = new T[ResizeArray<T>.DEFAULT_LENGTH];
        }

        public void AddLast(T element)
        {
            ++this.count;
            if (this.count > this.content.Length)
            {
                T[] newContent = new T[this.content.Length * 3];
                this.content.CopyTo(newContent, 0);
                this.content = newContent;
            }
            this.content[this.count - 1] = element;
        }

        private void checkIndex(int i)
        {
            if (i < 0 || i >= this.count)
                throw new IndexOutOfRangeException("Индекс должен лежать в границах [0;" + this.count + "]. Дано " + i);
        }

        public T this[int i]
        {

            get
            {
                this.checkIndex(i);
                return this.content[i];
            }

            set
            {
                this.checkIndex(i);
                this.content[i] = value;
            }
        }
    }
}