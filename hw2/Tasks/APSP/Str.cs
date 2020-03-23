using System;
using Matrix;

namespace APSP
{
    public class Str : ISerializable {

        public String value { get; private set; }
        public void FromWord(String word) {
            this.value = word;
        }

        public String ToWord() => this.value;

        public Str() {
            this.value = "";
        }
        
        public Str(String str) {
            this.value = str;
        }
    }
}
