using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public class MultiEnumerable<T> : MultiEnumerable,IEnumerable<T> {
        public MultiEnumerable(params IEnumerable[] enums) :base(enums){}

        public new IEnumerator<T> GetEnumerator() {
            foreach (var e in enums) {
                foreach (T f in e) {
                    yield return f;
                }
            }            
        }
    }
    public class MultiEnumerable : IEnumerable {
        /*
        private class Iter : IEnumerator<T> {

            private MultiEnumerable<T> owner;

            private int index=0;
            private IEnumerator iter;
            public T Current {
                get { return (T)iter.Current; }
            }

            object System.Collections.IEnumerator.Current {
                get { return Current; }
            }

            public bool MoveNext() {
                if (iter.MoveNext()) return true;
                do { ++index; } while (index < owner.enums.Length && owner.enums[index].IsEmpty());
                if (index >= owner.enums.Length) return false;
                iter = owner.enums[index].GetEnumerator();
                return true;
            }

            public void Reset() {
                index = 0;
                iter = owner.enums[index].GetEnumerator();
            }

            public void Dispose() {
                owner = null;
                iter = null;
            }

            public Iter(MultiEnumerable<T> owner) {
                this.owner = owner;
                Reset();
            }
        }
        */
        protected IEnumerable[] enums;

        public IEnumerator GetEnumerator() {
            foreach (var e in enums) {
                foreach (var f in e) {
                    yield return f;
                }
            }
        }

        public MultiEnumerable(params IEnumerable[] enums) {
            this.enums = enums;
        }


    }
}
