using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpUtils
{
    interface ISerializable
    {
        public void FromWord(String word);

        public String ToWord();
    }
}
