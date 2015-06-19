using Assets.TechTree.Data;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.TechTree {
    public class ResourceBinder : MonoBehaviour {
        public enum ResourceType {
            String,Variable
        }
        public string component;
        public string property;
        public ResourceType resourceType;
        public string resourceId;

        private Component com;
        private PropertyInfo prop;

        void Start() {
            G.Instance.OnLoaded(g => {
                try {
                    com = GetComponent(component);
                    prop = com.GetType().GetProperty(property);
                    object v = null;
                    switch (resourceType) {
                    case ResourceType.String:
                        v = G.Instance.data.strings[resourceId];
                        break;
                    case ResourceType.Variable:
                        v = Var.GetVar(resourceId);
                        Var.VarChanged[resourceId] += Var_VarChanged;
                        break;
                    }
                    prop.SetValue(com, Converter.Convert(v, prop.PropertyType), null);
                } catch (Exception e) {
                    Debug.LogWarning("Invalid Component/Property/ReourceId: " + e);
                }
            });            
        }

        void Var_VarChanged(Var.VarChangedEventArgs args) {
            prop.SetValue(com, Converter.Convert(args.Value, prop.PropertyType), null);
        }

    }
}
