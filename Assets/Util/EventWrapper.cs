using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public class EventWrapper<T>{
        public event Action<T> Event;
        public static Action<T> operator +(EventWrapper<T> l, Action<T> r) {
            return l.Event + r;
        }

        public static Action<T> operator -(EventWrapper<T> l, Action<T> r) {
            return l.Event - r;
        }

        public static implicit operator EventWrapper<T>(Action<T> t) {
            var e = new EventWrapper<T>();
            e.Event += t;
            return e;
        }

        public void Invoke(T args) {
            if (Event != null) Event(args);
        }
    }

}
