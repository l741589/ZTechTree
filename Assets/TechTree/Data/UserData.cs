using Assets.Util;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TechTree.Data {

    public class UBaseItem {
        private BaseItem raw;

        [JsonIgnore]
        public BaseItem Raw {get {return raw;}}

        public String Id { get { return raw.id; } }
        public String RealId { get { return raw.realId; } }
        public DateTime? EndTime { get; set; }
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
            return RealId;
        }

        public UBaseItem Init(BaseItem raw) {
            this.raw = raw;
            return this;
        }
        [JsonIgnore]
        public ItemButton Button { get; set; }
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
                Button.Data = this;
            } else {
                if (Button == null) return;
                GameObject.Destroy(Button.gameObject);
                Button = null;
            }
        }
    }

    

    public static class __UBaseItemExt {
        public static UBaseItem getUserItem(this BaseItem item) {
            return FastData.Lookup(item.realId).UserObj as UBaseItem;
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

        public void AddItem<T>(Dictionary<String,T> dic,BaseItem item) where T:UBaseItem{
            if (dic.ContainsKey(item.realId)) {
                if (object.ReferenceEquals(dic[item.realId], item.getUserItem())) return;
                throw new ApplicationException("don't create new useritem");
            }
            dic[item.realId] = item.getUserItem() as T;
        }

        private void AnalyseDenpendency(IEnumerable baseitems){
            foreach(var _e in baseitems){
                var e=_e as BaseItem;
                var uthis=e.getUserItem();
                if (e.cond == null) continue;
                switch (e.cond.type) {
                case 1:
                    foreach (var c in e.cond.items) {
                        var k = FastData.Lookup(c.id);
                        (k.UserObj as UBaseItem).ReverseDependences.Add(uthis);
                        uthis.Dependences.Add(k.UserObj as UBaseItem);
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
                if (e.getUserItem().Dependences.IsEmpty()) {
                    e.getUserItem().CanShow = true;
                }
            }
            foreach (var e in gdata.var) Var.SetVar(e.Key, e.Value);
        }

        

        #endregion Funcs
    }

}
