using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public interface Initable {
        void Init();
    }
    public interface Initable<T> {
        void Init(T arg);
    }
    public interface Initable<T1,T2> {
        void Init(T1 arg1, T2 arg2);
    }
}
