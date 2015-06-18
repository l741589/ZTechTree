using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public class DictionaryList<K,V> : Dictionary<K,List<V>> {

        public void Add(K key, V value) {
            List<V> list;
            if (!TryGetValue(key, out list)) {
                list = new List<V>();
                this.Add(key, list);
            }
            list.Add(value);
        }

        public V RemoveOne(K key) {
            List<V> list;
            if (!TryGetValue(key, out list)) return default(V);
            if (list.IsEmpty()) return default(V);
            var r = list.Last();
            list.RemoveAt(list.Count - 1);
            return r;
        }

        public new List<V> this[K key] {
            get {
                if (!base.ContainsKey(key)) base[key] = new List<V>();
                return base[key];
            }
            set { base[key] = value; }
        }
    }
}
