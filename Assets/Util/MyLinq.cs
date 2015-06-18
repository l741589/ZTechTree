using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public static class MyLinq {
        public static bool IsEmpty(this IEnumerable en) {
            if (en == null) return true;
            var iter = en.GetEnumerator();
            return !iter.MoveNext();
        }
    }
}
