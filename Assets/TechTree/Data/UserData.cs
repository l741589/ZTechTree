using Assets.TechTree.Actions;
using Assets.Util;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TechTree.Data {

    public class Rawable<T> : Initable<T> {
        private T raw;

        [JsonIgnore]
        public T Raw { get { return raw; } }

        public override void Init(T raw) {
            this.raw = raw;
        }
    }

    public class UBaseItem : Rawable<BaseItem> {
       

        public bool ForceShow { get; set; }
        public List<UBaseItem> ReverseDependences { get; private set; }
        public List<UBaseItem> Dependences { get; private set; }

        public UBaseItem() {
            CanShow = false;
            ReverseDependences = new List<UBaseItem>();
            Dependences = new List<UBaseItem>();
            ForceShow = false;
        }

        public override string ToString() {
            return Raw.realId;
        }

       
        [JsonIgnore]
        public WeakReference<ItemButton> Button { get; set; }
        private bool canShow = false;
        public bool CanShow {
            get { return canShow; }
            set {
                if (canShow == value) return;
                canShow = value;
                if (canShow) ForceShow = true;
                UpdateButtonVisibility();
            }
        }
        private bool visible = false;
        public bool Visible {
            get {
                return visible;
            }
            set {
                if (visible==value) return;
                visible = value;
                UpdateButtonVisibility();
            }
        }

        public void UpdateButtonVisibility(){
            if ((CanShow||ForceShow)&&Visible) {
                if (Button != null) return;
                var o = GameObject.Instantiate(G.Instance.Prefabs["ItemButton"]);
                Button = o.GetComponent<ItemButton>();
                Button.Target.Data = this;
            } else {
                if (Button == null) return;
                GameObject.Destroy(Button.Target.gameObject);
                Button = null;
            }
        }

        public void UpdateReverseDependences() {
            foreach (var e in ReverseDependences) {
                foreach (var a in e.Raw.action) { a.UserData.OnDataUpdate(); }
                e.CanShow = e.Raw.cond.Can();
            }
        }

        public void UpdateDependences() {
            foreach (var e in Dependences) {
                foreach (var a in e.Raw.action) {a.UserData.OnDataUpdate();}
                if (e.Button != null) e.Button.Target.DataUpdated();
            }
        }
    }


    public class UTech : UBaseItem {
        public UTech() : base() { Studied = false; }
        public bool Studied { get; set; }
    }
    public class UItem : UBaseItem {
        public UItem() : base() { Count = 0; }
        public int Count { get; set; }
        
    }
    public class UBuld : UBaseItem {
    }


    public class UserData {
        #region Funcs
        public UserData() {
            InitData();
        }


        public GameData gdata { get { return G.Instance.data; } }


        private void AnalyseDenpendency(IEnumerable baseitems){
            foreach(var _e in baseitems){
                var e=_e as BaseItem;
                var uthis = e.UserData;
                if (e.cond == null) continue;
                switch (e.cond.type) {
                case 1:
                    foreach (var c in e.cond.items) {
                        var k = FastData.Lookup<BaseItem>(c.id);
                        k.UserData.ReverseDependences.Add(uthis);
                        uthis.Dependences.Add(k.UserData);
                    }
                    break;
                default: goto case 1;
                }
            }
        }

        public void InitData() {
            if (G.Instance.udata == null) return;
            var allitems = FastData.Instance.AllItems;
            AnalyseDenpendency(allitems);
            foreach (var e in allitems) {
                if (e.UserData.Dependences.IsEmpty()) {
                    e.UserData.CanShow = true;
                }
            }
            foreach (var e in gdata.var) Var.SetVar(e.Key, e.Value);
        }

        

        #endregion Funcs
    }

}
