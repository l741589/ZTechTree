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
        public Dictionary<String, object> strLookupTable = new Dictionary<String, object>();
        private MultiEnumerable<BaseItem> allitems;
        public MultiEnumerable<BaseItem> AllItems { get { if (allitems == null) allitems = new MultiEnumerable<BaseItem>(gdata.tech, gdata.item, gdata.buld); return allitems; } }
        public static GameData gdata { get { return G.Instance.data; } }
        
        public GameData raw{get;private set;}


        public FastData(GameData data) {
            raw = data;
            Visit(raw, "");
            foreach (Tech t in data.tech) {
                t.userData = new UTech().Init(t);
                strLookupTable[t.realId] = t;
            }
            foreach (Item t in data.item) {
                t.userData = new UItem().Init(t);
                strLookupTable[t.realId] = t;
            }
            foreach (Buld t in data.buld) {
                t.userData = new UBuld().Init(t);
                strLookupTable[t.realId] = t;
            }
        }

        private void Visit(object o, String prefix) {
            if (o == null) return;
            if (o is IEnumerable) {
                var i = o as IEnumerable;
                foreach (var e in i) Visit(e,prefix);
            } else {
                if (o is IIdentifiable) {
                    var i = o as IIdentifiable;
                    i.realId = prefix+i.id;
                    i.cateId = prefix.TrimEnd('.');
                }
                var ps = o.GetType().GetProperties();
                foreach (var p in ps) {
                    Visit(p.GetValue(o, null), prefix + p.Name+".");
                }
            }
        }

        public static T Lookup<T>(String key) {
            return (T)Lookup(key);
        }

        public static object Lookup(String key) {
            object r;
            if (Instance.strLookupTable.TryGetValue(key, out r)) return r;
            Debug.LogWarning("Key:'" + key + "' not found when Lookup " + typeof(FastData).Name);
            return null;
        }
    }


    public static class ___FastData {
        public static ItemAction GetAction(string name) {
            return null;
        }
    }
}

