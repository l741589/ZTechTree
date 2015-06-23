using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public class BindableProperty<T> {

        public delegate T Getter();
        public delegate void Setter(T value);
        public delegate void ValueChangedHandler(T oldVal, T newVal);
        private WeakDelegate<T, T> _ValueChanged = new WeakDelegate<T, T>();
        public event ValueChangedHandler ValueChanged {
            add { _ValueChanged.Add(value); }
            remove { _ValueChanged.Remove(value); }
        }


        public Getter Get { get; private set; }
        public Setter Set { get; private set; }

        private T value;
        public T Value { get { return Get(); } }

        private Setter Wrap(Setter set) {
            return v => {
                if (this._ValueChanged==null) set(v);
                else {
                    var old = Get();
                    set(v);
                    _ValueChanged.Invoke(old, Get());
                }
            };
        }

        public BindableProperty(T defaultValue) {
            Get = () => value;
            Set = Wrap(v => value = v);
            value = defaultValue;
        }

        public BindableProperty() :this(default(T)){ }

        public BindableProperty(Getter get,Setter set) {
            Get = get;
            Set = Wrap(set);
        }

        public BindableProperty(Getter get) :this(get,null){}

        public static explicit operator BindableProperty<T>(T val){
            return new BindableProperty<T>(val);
        }

        public static implicit operator T(BindableProperty<T> val) {
            if (val == null) return default(T);
            return val.Get();
        }

        public static bool operator==(BindableProperty<T> l,T r){
            return Object.Equals(l.Value,r);
        }

        public static bool operator !=(BindableProperty<T> l, T r) {
            return !!(l == r);
        }

        public static bool operator ==(T l,BindableProperty<T> r) {
            return Object.Equals(l, r.Value);
        }

        public static bool operator !=(T l, BindableProperty<T> r) {
            return !(l == r);
        }

        public static bool operator ==(BindableProperty<T> l, BindableProperty<T> r) {
            if (Object.ReferenceEquals(l, null) && Object.ReferenceEquals(r, null)) return true;
            if (Object.ReferenceEquals(l, null)) return ((T)l) == r;
            if (Object.ReferenceEquals(r, null)) return l == ((T)r);
            return Object.Equals(l.Value, r.Value);
        }

        public static bool operator !=(BindableProperty<T> l, BindableProperty<T> r) {
            return !(l == r);
        }

        public override bool Equals(object obj) {
            if (obj == null) return Value==null;
            if (obj is BindableProperty<T>) return Object.Equals(Value, (obj as BindableProperty<T>).Value);
            if (obj is T) return Object.Equals(Value, (T)obj);
            return false;
        }
      
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    }
}
