using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TechTree.Data {
    public static class Var {

        static Var() {
            Converter.Register<string, int>(s => int.Parse(s));
            Converter.Register<float, string>(f => f.ToString("#0.00"));
            VarChanged = new Map<string, EventWrapper<VarChangedEventArgs>>();
            VarChanged.DefaultValue.Set(()=>new EventWrapper<VarChangedEventArgs>());

            VarChanged[food] += a => {
                if (Converter.Convert<float>(a.Value) < 0) {
                    a.Value = 0;
                    SetVar<int>(people, x => x - 1);
                }
            };

            VarChanged[people] += a => {
                if (Converter.Convert<int>(a.Value) <= 0) {
                    a.Value = 1;
                }
            };
        }

        public const string people = "$people";
        public const string food = "$food";


        public class VarChangedEventArgs : EventArgs {
            public VarChangedEventArgs(object oldVal, object newVal) { OldValue = oldVal; Value = newVal; }
            public object OldValue { get; private set;}
            public object Value { get; set; }
        }
        public delegate void VarChangedHandler(VarChangedEventArgs args);
        public static Map<string, EventWrapper<VarChangedEventArgs>> VarChanged { get; private set; }
        private static Dictionary<string, object> var = new Dictionary<string, object>();

        public static object GetVar(string key) {
            return var[key];
        }

        public static T GetVar<T>(string key) {
            return (T)Converter.Convert(var[key],typeof(T));
        }

        public static void SetVar(string key, object value) {
            if (VarChanged != null) value = Invoke(key, var.GetOrDefault(key), value);
            var[key] = value;
        }

        public static void SetVar(string key, Func<object, object> handle) {
            SetVar<object>(key, handle);
        }

        public static void SetVar<T>(string key, Func<T,T> handle) {
            object value = var[key];
            value = Converter.Convert(value, typeof(T));
            value = handle((T)value);
            SetVar(key, value);
        }
        
        private static object Invoke(string key,object old,object val){
            var f=VarChanged.GetOrDefault(key);
            var args=new VarChangedEventArgs(old, val);
            if (f != null) f.Invoke(args);
            return args.Value;
        }
    }
}
