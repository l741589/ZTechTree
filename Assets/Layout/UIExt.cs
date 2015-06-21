using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Layout {
    public static class UIExt {
        public static Vector3 getLocalLeftBottom(this RectTransform t) {
            Vector3 v = new Vector3(t.sizeDelta.x, t.sizeDelta.y);
            v.Scale(t.pivot);
            return v;
        }
        public static Vector3 getLeftBottom(this RectTransform t) {
            
            return t.position + getLocalLeftBottom(t);
        }

        public static void SetText(this Text text, string val) {
            if (text == null) return;
            if (text.IsDestroyed()) return;
            if (String.IsNullOrEmpty(val)) {
                text.text = "";
                text.gameObject.SetActive(false);
            } else {
                text.text = val;
                text.gameObject.SetActive(true);
            }
        }

        public static void EnumerateChildren(this Transform o,Func<Transform,bool> callback) {
            foreach (Transform e in o) {
                var b=callback(e);
                if (b) {
                    e.EnumerateChildren(callback);
                }                
            }
        }

        public static T FindByName<T>(this Component o,string name) {
            Transform t = null;
            o.transform.EnumerateChildren(c => {
                if (c.name == name) t = c;
                return t == null;
            });
            return t.GetComponent<T>();
        }
    }
}
