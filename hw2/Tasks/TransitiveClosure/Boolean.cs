﻿using Matrix;
using System;

namespace TransitiveClosure
{
    public class Boolean : ISerializable {
        public bool value { get; private set; }

        public Boolean() {
            this.value = false;
        }

        public Boolean(bool f) {
            this.value = f;
        }

        public void FromWord(String word) {
            if (word.Equals("t"))
                this.value = true;
            else if (word.Equals("f"))
                this.value = false;
            else
                throw new ArgumentException("Boolean м. б. восстановлено только из слов 't' или 'f'.");
        }

        public String ToWord() => this.value ? "t" : "f";
    }
}
