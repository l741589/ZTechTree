using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.TechTree.Data {
    public static class GameDataPrepare {

        public static string[] actionsNeedCondtion ={
                                                        "study","produce",
                                                   };

        public static JToken HandleBaseItem(JToken inobj) {
            foreach (var obj in (JArray)inobj) {
                JArray action = obj["action"] as JArray;
                if (action == null) {
                    obj["action"] = new JArray();
                    continue;
                }
                for (var i = 0; i < action.Count; ++i) {
                    if (action[i].Type == JTokenType.String) {
                        var a = JObject.FromObject(new {
                            id = (string)action[i]
                        });
                        if (a["cond"] == null && actionsNeedCondtion.Contains((string)a["name"])) {
                            a["cond"] = obj["cond"];
                        }
                        action[i] = a;
                    }
                }
            }
            return inobj;
        }

        public static void HandleField(JToken obj, string key, Func<JToken, JToken> func) {
            if (obj is JArray) {
                var arr = obj as JArray;
                foreach(var e in arr){
                    HandleField(e, key, func);
                }
            } else {
                obj[key] = func(obj[key]);
            }
        }

        public static JObject Prepare(JObject obj) {
            HandleField(obj, "tech", HandleBaseItem);
            HandleField(obj, "item", HandleBaseItem);
            HandleField(obj, "buld", HandleBaseItem);
#if UNITY_STANDALONE_WIN
            File.WriteAllText("e:/test/zttgamedata.json",obj.ToString());
#endif
            return obj;
        }
    }
}
