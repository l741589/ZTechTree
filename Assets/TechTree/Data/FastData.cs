using Assets.Util;
using Newtonsoft.Json;
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
        public Dictionary<object, object> parentLookupTable = new Dictionary<object, object>();

        private MultiEnumerable<BaseItem> allitems;
        public MultiEnumerable<BaseItem> AllItems { get { if (allitems == null) allitems = new MultiEnumerable<BaseItem>(gdata.tech, gdata.item, gdata.buld); return allitems; } }
        public static GameData gdata { get { return G.Instance.data; } }
        
        public GameData raw{get;private set;}


        public FastData(GameData data) {
            raw = data;
            Visit(raw, "");
        }

        private void Visit(object o, String prefix) {
            if (o == null) return;
            if (o is IEnumerable) {
                var i = o as IEnumerable;
                foreach (var e in i) {
                    if (e != null) parentLookupTable[e] = o;
                    Visit(e, prefix);
                }
            } else {
                if (o is IIdentifiable) {
                    var i = o as IIdentifiable;
                    i.realId = prefix+i.id;
                    i.cateId = prefix.TrimEnd('.');
                    strLookupTable[i.realId] = i;
                }
                var ps = o.GetType().GetProperties();
                foreach (var p in ps) {
                    if (!p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).IsEmpty()) continue;
                    var child=p.GetValue(o, null);
                    if (child != null) parentLookupTable[child] = o;
                    Visit(child, prefix + p.Name+".");
                }
            }
        }

        public static T Lookup<T>(String key) {
            return (T)Lookup(key);
        }

        public static T LookupParent<T>(object obj) {
            return (T)Instance.parentLookupTable.GetOrDefault(obj);
        }

        public static T LookupAcestor<T>(object obj) {
            while (obj != null) {
                obj = Instance.parentLookupTable.GetOrDefault(obj);
                if (obj is T) return (T)obj;
            }
            return default(T);
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

        public static ItemActionInfo GetInfo(this ItemAction action){
            return G.Instance.data.action[action.id];
        }
    }
}

