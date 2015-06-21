using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.TechTree {
    public class JsWrapper {
        public Component Component { get; private set; }
        private Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

        public JsWrapper(Component component) {
            Component = component;
        }

        public object Call(String name, params object[] args) {
            MethodInfo method;
            if (!methods.TryGetValue(name,out method)) {
                method=Component.GetType().GetMethod(name);
                methods[name] = method;
            }
            return method.Invoke(Component, args);
        }
    }

}
