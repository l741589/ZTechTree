using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TechTree.Data {
    public static class DataUtil {
        private static UserData udata { get { return G.Instance.udata; } }
        public static bool Can(this Conditions cond) {
            if (cond == null) return true;
            var b = true;
            switch (cond.type) {
            case 1:
                foreach (var e in cond.items) if (!e.Can()) return false;
                break;
            default: goto case 1;
            }
            return b;
        }

        public static bool Can(this Conditions.ConditionItem cond) {
            var t = FastData.Lookup(cond.id).UserObj as UBaseItem;
            if (t == null) return false;
            if (t is UTech) {
                var x = t as UTech;
                if (!x.Studied) return false;
            } else if (t is UItem) {
                var x = t as UItem;
                if (x.Count < cond.count) return false;
            } else {
                throw new NotImplementedException();
            }
            return true;
        }

        public static bool Pay(this BaseItem item) {
            if (item.paid == null) return Pay(item.cond);
            else return Pay(item.paid);
        }

        public static bool Pay(this Conditions cond) {
            if (cond == null) return false;
            if (!cond.Can()) return false;
            switch (cond.type) {
            case 1:
                foreach (var e in cond.items) {
                    var ext=FastData.Lookup(e.id);
                    if (ext.UserObj is UTech) {
                    } else if (ext.UserObj is UItem) {
                        ((UItem)ext.UserObj).Count -= e.count;
                    } else {
                        throw new NotImplementedException();
                    }
                }
                break;
            default: goto case 1;
            }
            return true;
        }

        public static V GetOrDefault<K, V>(this IDictionary<K, V> dic, K key) {
            V v;
            if (dic.TryGetValue(key, out v)) return v;
            return default(V);
        }

        public static V AddNotConfict<K, V>(this IDictionary<K, V> dic, K key, V value) {
            V v = default(V);
            dic.TryGetValue(key, out v);
            dic[key] = value;
            return v;
        }
    }
}
