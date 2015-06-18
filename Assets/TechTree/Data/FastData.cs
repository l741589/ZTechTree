using Assets.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TechTree.Data {
    public class FastData {
        public static FastData Instance { get { return G.Instance.fdata; } }
        public Dictionary<object, ItemExtInfo> extMember = new Dictionary<object, ItemExtInfo>();
        public Dictionary<String, ItemExtInfo> strLookupTable = new Dictionary<String, ItemExtInfo>();
        private MultiEnumerable<BaseItem> allitems;
        public MultiEnumerable<BaseItem> AllItems { get { if (allitems == null) allitems = new MultiEnumerable<BaseItem>(gdata.tech, gdata.item, gdata.buld); return allitems; } }
        public static GameData gdata { get { return G.Instance.data; } }
        public class ItemExtInfo : Dictionary<String,object>{
            public ItemExtInfo(object raw) {
                this["Raw"] = raw;
            }
            public object Raw { get { return this["Raw"]; } }
            public T raw<T>() { return (T)Raw; }
            public object UserObj { get { return this["User"]; } set { this["User"] = value; } }
            public override string ToString() {
                return UserObj.ToString();
            }
        }

        public GameData raw{get;private set;}

        private ItemExtInfo AddItem(object t,Action<ItemExtInfo> prepare) {
            var r=new ItemExtInfo(t);
            extMember[t] = r;
            prepare(r);
            strLookupTable[r.ToString()] = r;
            return r;
        }

        public FastData(GameData data) {
            raw = data;
            Visit(raw, "");
            foreach (Tech t in data.tech) {
                AddItem(t, r => {
                    r.UserObj = new UTech().Init(t);
                });
            }
            foreach (Item t in data.item) {
                AddItem(t, r => {
                    r.UserObj = new UItem().Init(t);
                });
            }
            foreach (Buld t in data.buld) {
                AddItem(t, r => {
                    r.UserObj = new UBuld().Init(t);
                });
            }
        }

        private void Visit(object o, String prefix) {
            if (o == null) return;
            if (o is IEnumerable) {
                var i = o as IEnumerable;
                foreach (var e in i) Visit(e,prefix);
            } else {
                if (o is Identifiable) {
                    var i = o as Identifiable;
                    i.realId = prefix+i.id;
                    i.cateId = prefix.TrimEnd('.');
                }
                var ps = o.GetType().GetProperties();
                foreach (var p in ps) {
                    Visit(p.GetValue(o, null), prefix + p.Name+".");
                }
            }
        }

        public static ItemExtInfo Lookup(String key) {
            ItemExtInfo r;
            if (Instance.strLookupTable.TryGetValue(key, out r)) return r;
            Debug.LogWarning("Key:'" + key + "' not found when Lookup " + typeof(FastData).Name);
            return null;
        }
    }
}

