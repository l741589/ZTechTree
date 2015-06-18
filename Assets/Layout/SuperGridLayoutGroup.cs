using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Layout {
    [AddComponentMenu("Layout/SuperGrid Layout Group", 154)]
    public class SuperGridLayoutGroup : LayoutGroup {
        [Serializable]
        public enum RowColType { Weight, Fix }
        [Serializable]
        public class RowColDefinition {
            public float num;
            public RowColType type;
            public RowColDefinition() {
                num = 1;
                type = RowColType.Weight;
            }
        }
        private int ColCount { get { return colDefinitions.Count; } }
        private int RowCount { get { return rowDefinitions.Count; } }

        [SerializeField]
        private float spacing = 0;
        [SerializeField]
        private Vector2 defaultCellSize = new Vector2(100, 40);
        [SerializeField]
        private bool visualize;
        [SerializeField]
        private List<RowColDefinition> rowDefinitions = new List<RowColDefinition>();
        [SerializeField]
        private List<RowColDefinition> colDefinitions = new List<RowColDefinition>();
        
        public int LineCount {
            get {
                return (transform.childCount - 1) / ColCount + 1;
            }
        }

        private float[] rowdefs;
        private float[] coldefs;

        public Rect cellRect(int x, int y, int spanX, int spanY) {
            if (spanX <= 0) spanX = 1;
            if (spanY <= 0) spanY = 1;
            if (coldefs!=null&& rowdefs!=null) {
                return new Rect(
                    coldefs[1 + x * 2],
                    rowdefs[1 + y * 2],
                    coldefs[2 + (x + spanX - 1) * 2] - coldefs[1 + x * 2],
                    rowdefs[2 + (y + spanY - 1) * 2] - rowdefs[1 + y * 2]
                   );
            } else if (coldefs!=null) {
                return new Rect(
                    coldefs[1 + x * 2],
                    padding.bottom+(defaultCellSize.y+spacing)*y-spacing,
                    coldefs[2 + (x + spanX - 1) * 2] - coldefs[1 + x * 2],
                    defaultCellSize.y + (spanY-1) * (defaultCellSize.y+spacing)
                   );
            } else if (rowdefs != null) {
                return new Rect(
                    padding.left + (defaultCellSize.x + spacing) * x - spacing,
                    rowdefs[1 + y * 2],
                    defaultCellSize.x + (spanX-1) * (defaultCellSize.x + spacing),
                    rowdefs[2 + (y + spanY - 1) * 2] - rowdefs[1 + y * 2]
                   );
            } else {
                //Assert.Error("Both RowCount and ColCount are zero");
                return default(Rect);
            }
        }

        private SuperGridElement lastElement = null;
        public void CalculateLayout(){
            lastElement = null;
            
            RectTransform t=transform as RectTransform;
            if (ColCount>0){
                //t.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
                t.sizeDelta = new Vector2(t.sizeDelta.x, padding.vertical-spacing+(defaultCellSize.y+spacing)*LineCount);
            }
            var r=t.rect;
            if (r.width == 0) return;
            if (ColCount > 0) defaultCellSize.x = (r.width - padding.horizontal - spacing * (ColCount-1)) / ColCount;
            if (RowCount > 0) defaultCellSize.y = (r.height - padding.vertical - spacing * (RowCount - 1)) / RowCount;
            if (ColCount>0) {
                foreach (Transform e in t) {
                    var p = e.gameObject.GetComponent<SuperGridElement>();
                    if (p == null) {
                        p = e.gameObject.AddComponent<SuperGridElement>();
                    }
                    if (p.autoIndex) {
                        if (lastElement != null) {
                            p.x = lastElement.x + 1;
                            p.y = lastElement.y;
                            if (p.x >= ColCount) {
                                p.x = 0;
                                ++p.y;
                                if (RowCount > 0 && p.y >= RowCount) p.y = 0;
                            }
                        }
                        
                    }
                    lastElement = p;
                    var et=e.transform as RectTransform;
                    var x=padding.left + (spacing + defaultCellSize.x) * p.x;
                    var y=r.height - (padding.top + (spacing + defaultCellSize.y) * p.y);
                    var z = e.transform.localPosition.z;
                    et.sizeDelta = new Vector3(defaultCellSize.x, defaultCellSize.y);
                    et.localPosition = new Vector3(x+et.pivot.x*et.sizeDelta.x, y-et.pivot.y*et.sizeDelta.y, z);                    
                }
            } else if (RowCount>0){
                throw new NotImplementedException();
            }
        }


        private bool dirty = true;
        public void CalculateLayout2() {
            RectTransform t=transform as RectTransform;
            if (dirty) {
                rowdefs = GenRowColDefs(rowDefinitions, t.sizeDelta.y, padding.bottom, padding.top, defaultCellSize.y);
                coldefs = GenRowColDefs(colDefinitions, t.sizeDelta.x, padding.left, padding.right, defaultCellSize.x);
#if !UNITY_EDITOR
                dirty = false;
#endif
            }
            DebugDraw();
            lastElement = null;
            foreach (Transform e in t) {
                var p = e.GetComponent<SuperGridElement>();
                if (p == null) {
                    p = e.gameObject.AddComponent<SuperGridElement>();
                }
                if (p.autoIndex) {
                    if (lastElement != null) {
                        p.x = lastElement.x + 1;
                        p.y = lastElement.y;
                        if (p.x >= ColCount) {
                            p.x = 0;
                            ++p.y;
                            if (RowCount > 0 && p.y >= RowCount) p.y = 0;
                        }
                    }

                }
                lastElement = p;
                var r = cellRect(p.x, p.y, p.spanX, p.spanY);
                var et = e.transform as RectTransform;
                
                et.offsetMin = r.min - Vector2.Scale(et.anchorMin, t.sizeDelta);
                et.offsetMax = r.max - Vector2.Scale(et.anchorMax, t.sizeDelta);
            }

            if (rowdefs == null && ColCount > 0) {
                var c = ((t.childCount - 1) / ColCount) + 1;
                t.sizeDelta = new Vector2(t.sizeDelta.x, padding.vertical + c * defaultCellSize.y + (c - 1) * spacing);
            }

            if (coldefs == null&&RowCount>0) {
                var c = ((t.childCount - 1) / RowCount) + 1;
                t.sizeDelta = new Vector2(padding.horizontal + c * defaultCellSize.x + (c - 1) * spacing, t.sizeDelta.y);
            }
        }

        private Vector3 localPos(float x, float y, float z = 0) {
            return localPos(new Vector3(x, y, z));
        }

        private Vector3 localPos(Vector3 v) {
            RectTransform t = transform as RectTransform;
            var leftbottom = t.position - Vector3.Scale(t.pivot, t.sizeDelta);
            return leftbottom + v;
        }

        private void DebugDraw() {
#if UNITY_EDITOR
            if (visualize) {
                RectTransform t = transform as RectTransform;
                float z = -100;
                if (rowdefs != null) {
                    foreach (var e in rowdefs) {
                        Debug.DrawLine(localPos(0, e, z), localPos(t.sizeDelta.x, e, z));
                    }
                }
                if (coldefs != null) {
                    foreach (var e in coldefs) {
                        Debug.DrawLine(localPos(e, 0, z), localPos(e, t.sizeDelta.y, z));
                    }
                }
            }
#endif
        }

        private float[] GenRowColDefs(List<RowColDefinition> indefs, float total, int paddingStart, int paddingEnd, float defaultValue) {
            var count = indefs.Count;
            if (count > 0&&total>0) {
                float[] defs = new float[2 + count * 2];
                float h = total;
                defs[0] = 0;
                defs[1] = paddingStart;
                defs[defs.Length - 1] = paddingEnd;
                for (int i = 1; i < count; ++i) defs[i * 2 + 1] = spacing;
                h -= paddingEnd + paddingStart + (count - 1) * spacing;
                float fcount = 0;
                for (int i = 0; i < count; ++i) {
                    var e = indefs[i];
                    Assert.True(e.num >= 0,"num in row or col definitions mustn't be nagetive");
                    if (e.num == 0) {
                        defs[2 + i * 2] = defaultValue;
                        h -= defaultValue;
                    } else if (e.type == RowColType.Fix) {
                        defs[2 + i * 2] = e.num;
                        h -= e.num;
                    } else if (e.type == RowColType.Weight) {
                        fcount += e.num;
                    }
                }
                //Assert.True(h >= 0, "total size over contianer size");
                if (fcount > 0 && h > 0) {
                    var slice = h / fcount;
                    for (int i = 0; i < count; ++i) {
                        var e = indefs[i];
                        if (e.type == RowColType.Weight&&e.num!=0) {
                            defs[2 * i + 2] = e.num * slice;
                        }
                    }
                }
                for (int i = 1; i < defs.Length; ++i) defs[i] = defs[i - 1] + defs[i];
                return defs;
            }
            return null;
        }

        public override void CalculateLayoutInputHorizontal() {
            CalculateLayout2();
        }

        public override void CalculateLayoutInputVertical() {
            CalculateLayout2();
        }

        public override void SetLayoutHorizontal() {
            CalculateLayout2();
        }

        public override void SetLayoutVertical() {
            CalculateLayout2();
        }
    }
}
