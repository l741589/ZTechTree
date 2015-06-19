using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public class Converter {
        private static Dictionary<Type, Dictionary<Type, Func<object,object>>> converters=new Dictionary<Type,Dictionary<Type,Func<object,object>>>();

        public static void Register<S,T>(Func<S,T> converter){
            Dictionary<Type, Func<object,object>> dic;
            if (!converters.TryGetValue(typeof(S), out dic)) {
                dic = new Dictionary<Type, Func<object, object>>();
                converters[typeof(S)] = dic;
            }
            dic[typeof(T)] = o => converter((S)o);
        }

        public static T Convert<S,T>(S src) {
            return (T)Convert(src, typeof(T));
        }


        public static Func<object, object> Get(Type src, Type target) {
            Dictionary<Type, Func<object, object>> dic;
            if (!converters.TryGetValue(src, out dic)) return null;
            Func<object, object> val;
            if (!dic.TryGetValue(target, out val)) return null;
            return val;
        }

        public static object Convert(object src, Type target) {
            if (target.IsInstanceOfType(src)) return src;
            var f = Get(src.GetType(), target);
            if (f==null) return System.Convert.ChangeType(src, target);
            return f(src);
        }

        public static T Convert<T>(object src) {
            return (T)Convert(src, typeof(T));
        }
    }
}
