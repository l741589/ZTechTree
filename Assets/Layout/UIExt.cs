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
    }
}
