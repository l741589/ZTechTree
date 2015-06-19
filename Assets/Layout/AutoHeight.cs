using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Macros;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Layout {
    public class AutoHeight : MonoBehaviour {
        private Text Text;
        private RectTransform Transform { get { return transform as RectTransform; } }
        public void Start(){
            Text = GetComponent<Text>();
        }
    }
}
