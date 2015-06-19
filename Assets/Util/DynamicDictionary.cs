using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public delegate void KeyChangedHandler();
    public interface IMapItem { event KeyChangedHandler KeyChange;}
    public interface IMap : IDictionary,IEnumerable{
        
    }
    public class Map<K,V> : Dictionary<K,V>,IMap {

        public class DefaultValueType {
            private V val;
            private Func<V> funcV;
            public DefaultValueType() :this(default(V)){

            }
            public DefaultValueType(V val){
                this.val = val;
                funcV = null;
            }

            public DefaultValueType(Func<V> val) {
                this.val = default(V);
                funcV = val;
            }

            public static implicit operator V(DefaultValueType val) {
                if (val.funcV == null) return val.val;
                else return val.funcV();
            }

            public static implicit operator DefaultValueType(V val) {
                return new DefaultValueType(val);
            }

            public static implicit operator DefaultValueType(Func<V> val) {
                return new DefaultValueType(val);
            }

            public void Set(Func<V> val) {
                funcV = val;
            }
            
        }

        public Map() {
            DefaultValue = new DefaultValueType();
        }

        public DefaultValueType DefaultValue { get; set; }
        
        public new V this[K key]{
            get{
                if (!base.ContainsKey(key)) {
                    return DefaultValue;
                }
                return base[key];
            }
            set{
                base[key] = value;
            }
        }

    }
}
