using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Util {
    public class Heap<T> {
        private T[] data;
        private Comparison<T> comp;
        protected const int defaultConpacity = 16;
        public int Count{get;private set;}
        public Heap(int conpacity, Comparison<T> comp) {
            data = new T[conpacity];
            this.comp = comp;
            Count = 0;
        }
        public Heap(int conpacity, Comparer<T> comp) : this(conpacity, comp.Compare) { }
        public Heap(Comparison<T> comp) : this(defaultConpacity, comp) { }
        public Heap(Comparer<T> comp) : this(defaultConpacity, comp) { }
        public Heap(int conpacity) : this(conpacity, Comparer<T>.Default) { }
        public Heap() : this(defaultConpacity) { }

        public void Push(T value) {
            lock (data) {
                if (Count == data.Length) Array.Resize(ref data, data.Length * 2);
                var i = Count++;
                data[i] = value;
                while (i > 0 && comp(data[i], data[i >> 1]) < 0) {
                    swap(i, i >> 1);
                    i >>= 1;
                }
            }
        }
        public T Peek() {
            lock (data) {
                if (Count == 0) return default(T);
                return data[0];
            }
        }
        public T Pop() {
            lock (data) {
                if (Count == 0) return default(T);
                T ret = Peek();
                --Count;
                data[0] = data[Count];
                data[Count] = default(T);
                var i = 0;
                while ((i << 1) < Count) {
                    var t = min(i << 1, i << 1 + 1);
                    if (comp(data[t], data[i]) < 0) swap(t, i);
                    else break;
                }
                return ret;
            }
        }

        private int min(int l, int r) {
            if (r >= Count) return l;
            if (comp(data[l], data[r]) < 0) return l;
            return r;
        }

        private void swap(int i,int j){
            T t = data[i];
            data[i] = data[j];
            data[j] = t;
        }       
    }

    public class Heap<K, V> : Heap<KeyValuePair<K, V>> {
        public Heap(int conpacity, Comparison<K> comp)
            : base(conpacity, (l, r) => { return comp(l.Key, r.Key); }) {
        }
        public Heap(int conpacity, Comparer<K> comp) : this(conpacity, comp.Compare) { }
        public Heap(Comparison<K> comp) : this(defaultConpacity, comp) { }
        public Heap(Comparer<K> comp) : this(defaultConpacity, comp) { }
        public Heap(int conpacity) : this(conpacity, Comparer<K>.Default) { }
        public Heap() : this(defaultConpacity) { }

        public void Push(K key, V val) {
            Push(new KeyValuePair<K, V>(key, val));
        }

        public K PeekKey() {
            return Peek().Key;
        }

        public V PeekValue() {
            return Peek().Value;
        }

        public K PopKey() {
            return Pop().Key;
        }

        public V PopValue() {
            return Pop().Value;
        }
    }
}
