using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Layout {
    [AddComponentMenu("Layout/ListView", 153)]
    public class ListView : LayoutGroup{

        [Serializable]
        public class CreateObjectEvent : UnityEvent<GameObject> { };
        public class ExtraInfo : Component {
            public int index { get; internal set; }
            public Dictionary<String, System.Object> extras = new Dictionary<string, object>();
        }
       

        [SerializeField]
        private float rowHeight = 40;
        [SerializeField]
        private float spacing = 0;
        [SerializeField]
        private int colCount = 1;
        [SerializeField]
        private CreateObjectEvent onCreateObject;
        public ScrollRect scrollRect;

        public CreateObjectEvent OnCreateObject { get { return onCreateObject; } set { onCreateObject = value; } }
        public float RowHeight { get { return rowHeight; } set { rowHeight = value; } }
        public int ColCount { get { return colCount; } set { colCount = value; } }
        public float Spacing { get { return spacing; } set { spacing = value; } }

        private Queue<GameObject> pool = new Queue<GameObject>();
        private Dictionary<int, GameObject> showing = new Dictionary<int, GameObject>();
        private List<object> Data { get; set; }
       

        
        public int RowCount {
            get {
                return (Data.Count() - 1) / colCount + 1;
            }
        }

        public RectTransform Content {
            get {
                return scrollRect.content;
            }
        }

        public float ColWidth {
            get {
                return (Content.rect.width - padding.horizontal - (Spacing * (ColCount - 1))) / ColCount;
            }
        }

        public int FirstRow {
            get {
                int firstRow = (int)(-Content.position.y / RowHeight);
                if (firstRow < 0) firstRow = 0;
                return firstRow;
            }
        }

        public RectTransform Trans {
            get {
                return transform as RectTransform;
            }
        }

        public void CalculateLayout(){
            if (Data == null) return;
            Content.sizeDelta.Set(Trans.rect.width, padding.vertical + RowCount * (RowHeight + Spacing) - Spacing);
            for (int i = FirstRow * ColCount; i < Data.Count; ++i) {
                GameObject item;

                if (!showing.TryGetValue(i, out item)) {
                    if (pool.Count > 0) item = pool.Dequeue();
                }
                OnCreateObject.Invoke(item);
                item.transform.position.Set(i % ColCount * (ColWidth + Spacing) + padding.left, i / ColCount * RowHeight, item.transform.position.z);
            }
        }

        public override void CalculateLayoutInputHorizontal() {
            CalculateLayout();                
        }

        public override void CalculateLayoutInputVertical() {
            CalculateLayout();
        }

        public override void SetLayoutHorizontal() {
            CalculateLayoutInputHorizontal();
        }

        public override void SetLayoutVertical() {
            CalculateLayoutInputVertical();
        }

        public void Add(object data) {
            if (Data == null) Data = new List<object>();
            Data.Add(data);
        }
        

       // public abstract void CreateView(GameObject obj);
    }
}
