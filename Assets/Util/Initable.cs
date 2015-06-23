using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public interface Initable {
        void Init();
    }
    public interface Initable1 { void Init(object arg);}
    public abstract class Initable<T> : Initable1 {
        public abstract void Init(T arg);
        public void Init(object arg) {Init((T)arg);}
    }
    public interface Initable2 { void Init(object arg1,object arg2);}
    public abstract class Initable<T1, T2> : Initable<object> {
        public abstract void Init(T1 arg1, T2 arg2);
        public void Init(object arg1, object arg2) { Init((T1)arg1, (T2)arg2); }
    }
}
