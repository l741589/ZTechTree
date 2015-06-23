using Assets.TechTree.Actions;
using Assets.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TechTree.Data {

    public class HasUserData<T> where T:new(){
        private T userData;
        private bool initialized = false;
        [JsonIgnore]
        public T UserData {
            get {
                if (initialized) return userData;
                userData = CreateNew();
                if (userData is Initable1) {
                    ((Initable1)userData).Init(this);
                }
                initialized = true;
                return userData;
            }
        }
        protected virtual T CreateNew(){
            return new T();
        }
    }
    public interface IIdentifiable {
        string cateId { get; set; }
        string id { get; set; }
        string realId { get; set; }
    }
    public class Identifiable<T> : HasUserData<T>,IIdentifiable where T:new(){
        public String id { get; set; }
        [JsonIgnore]
        public String realId { get; set; }
        [JsonIgnore]
        public String cateId { get; set; }
        public override string ToString() {
            return realId ?? id ?? base.ToString();
        }
    }

    public class ItemAction : Identifiable<BaseAction>{
        public Conditions paid { get; set; }
        public float time { get; set; }
        public string macro { get; set; }
        protected override BaseAction CreateNew() {
            return BaseAction.Create(this);
        }
    }

    public class ItemActionInfo {
        public string name { get; set; }
    }

    public class BaseItem : Identifiable<UBaseItem> {
        public String name { get; set; }
        public String desc { get; set; }
        public Conditions cond { get; set; }
        public List<ItemAction> action { get; set; }
    }

    public class Tech : BaseItem {
        protected override UBaseItem CreateNew() {return new UTech();}
    }

    public class Item : BaseItem {
        protected override UBaseItem CreateNew() { return new UItem(); }
    }

    public class Buld : BaseItem {
        protected override UBaseItem CreateNew() { return new UBuld(); }      
    }


    public class Need : Dictionary<string, int> {

    }


    public class Conditions {

        public class ConditionItem {
            public String id { get; set; }
            public int count { get; set; }
        }

        //0:default(1),1:preitem
        public int type { get; set; }
        public List<ConditionItem> items;
    }

    public class GameData {
        public List<Tech> tech { get; set; }
        public List<Item> item { get; set; }
        public List<Buld> buld { get; set; }
        public Dictionary<string, string> var { get; set; }
        public Dictionary<string, string> strings { get; set; }
        public Dictionary<string, ItemActionInfo> action { get; set; }
    }
}
