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
        
        public new V this[K key]{
            get{
                if (!base.ContainsKey(key)) return default(V);
                return base[key];
            }
            set{
                Add(key,value);
            }
        }

    }
}
