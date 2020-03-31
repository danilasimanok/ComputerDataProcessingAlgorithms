using System;
using Matrix;

namespace MatrixMultiplier
{
    public class Natural : ISerializable {
        
        public uint value { get; private set; }

        public void FromWord(String word) {
            this.value = UInt32.Parse(word);
        }

        public String ToWord() => this.value.ToString();

        public Natural(uint i) {
            this.value = i;
        }

        public Natural() {
            this.value = 0;
        }
    }
}
