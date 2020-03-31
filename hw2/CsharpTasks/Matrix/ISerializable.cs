using System;

namespace Matrix
{
    public interface ISerializable
    {
        public void FromWord(String word);

        public String ToWord();
    }
}
