using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Layout {
    [AddComponentMenu("Layout/SuperGrid Element", 155)]
    public class SuperGridElement : UIBehaviour {
        public bool autoIndex = true;
        public int x = 0;
        public int y = 0;
        public int spanX = 1;
        public int spanY = 1;
    }
}
