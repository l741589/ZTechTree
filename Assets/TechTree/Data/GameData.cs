using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.TechTree.Data {

    public class HasUserData<T> {
        [JsonIgnore]
        public T userData;
    }
    public interface IIdentifiable {
        string cateId { get; set; }
        string id { get; set; }
        string realId { get; set; }
    }
    public class Identifiable<T> : HasUserData<T>,IIdentifiable {
        public String id { get; set; }
        [JsonIgnore]
        public String realId { get; set; }
        [JsonIgnore]
        public String cateId { get; set; }
        public override string ToString() {
            return realId ?? id ?? base.ToString();
        }
    }

    public class ItemAction {
        public string id { get; set; }
        public Conditions cond { get; set; }
        public string macro { get; set; }
    }

    public class BaseItem : Identifiable<UBaseItem> {
        public String name { get; set; }
        public Conditions cond { get; set; }
        public int time { get; set; }
        public List<ItemAction> action { get; set; }
    }

    public class Tech : BaseItem {
    }

    public class Item : BaseItem {
    }

    public class Need : Dictionary<string,int>{
        
    }

    public class Buld : BaseItem {
        public class Product {
            public string id { get; set; }
            public string need { get; set; }
        }       
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


        
    }
}
