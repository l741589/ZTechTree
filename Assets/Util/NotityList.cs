using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TechTree.Data {
    public class NotityList<T> : ICollection<T>, IEnumerable<T>, IList<T> {
        public delegate void DataAddHandler(T item);
        public event DataAddHandler DataAdd;
        public delegate void DataRemoveHandler(T item);
        public event DataRemoveHandler DataRemove;
        private List<T> data = new List<T>();
        public void Add(T item) {
            data.Add(item);
            if (DataAdd!=null) DataAdd(item);
        }

        public void Clear() {
            if (DataRemove != null) foreach (var e in data) DataRemove(e);
            data.Clear();
        }

        public bool Contains(T item) {
            return data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            data.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return data.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(T item) {
            if (DataRemove != null) DataRemove(item);
            return data.Remove(item);
        }

        public IEnumerator<T> GetEnumerator() {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return data.GetEnumerator();
        }

        public int IndexOf(T item) {
            return data.IndexOf(item);
        }

        public void Insert(int index, T item) {
            data.Insert(index, item);
            if (DataAdd != null) DataAdd(item);
        }

        public void RemoveAt(int index) {
            if (DataRemove != null) DataRemove(data[index]);
            data.RemoveAt(index);
        }

        public T this[int index] {
            get {
                return data[index];
            }
            set {
                data[index] = value;
            }
        }

        public T Find(Predicate<T> predicate) {
            return data.Find(predicate);
        }

        public List<T> Raw{ get{ return data; }}

    }
}
