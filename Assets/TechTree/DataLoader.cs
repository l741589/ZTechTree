using Assets.TechTree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

namespace Assets.TechTree {
    class DataLoader {
        public static int PLATFORM_UNKNOWN = 0;
        public static int PLATFORM_WINDOWS = 1;
        public static int PLATFORM_ANDROID = 2;
        public static int PLATFORM_IOS = 2;
        public static int Platform =
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
 PLATFORM_WINDOWS
#elif UNITY_ANDROID
 PLATFORM_ANDROID
#elif UNITY_IPHONE
 PLATFORM_IOS
#else
 PLATFORM_UNKNOWN
#endif
;
        public static readonly String[] BaseUrls = {
            "",
            "file://"+Application.dataPath+"/",
            "jar:file://" + Application.dataPath + "!/assets/",
            Application.dataPath + "/Raw/"
        };

        public static String BaseUrl { get { return BaseUrls[Platform]; } }
        public static String ResUrl { get{return BaseUrl+"Resources/";} }

        private static String ModPath(String baseStr, String path) {
            if (path.StartsWith("/") || path.Contains(':')) return path;
            return baseStr + path;
        }

        private static void CheckError(WWW w){
            if (w.error != null) {
                if (Debug.isDebugBuild) {
                    Debug.LogError(w.error);
                    throw new ApplicationException(w.error);
                }
            }
        }

        private static IEnumerator getObj<T>(String path,Action<T> cb) {
            return getString(path, s => {
                if (cb != null) {
                    T obj = JsonConvert.DeserializeObject<T>(s);
                    if (cb != null) cb(obj);
                }
            });
        }

        private static IEnumerator getString(String path, Action<String> cb) {
            WWW w = new WWW(ModPath(ResUrl,path));
            yield return w;
            CheckError(w);
            if (cb != null) cb(w.text);
        }

        private static IEnumerator getPrefab(String path, Action<UnityEngine.Object> cb) {
            var o=Resources.Load(path);
            if (cb!=null) cb(o);
            yield break;
        }


        public static void LoadObject<T>(String path, Action<T> cb) {
            G.Instance.StartCoroutine(getObj(path, cb));
        }

        public static void LoadString(String path, Action<String> cb) {
            G.Instance.StartCoroutine(getString(path, cb));
        }

        public static void LoadPrefab(String name,Action<UnityEngine.Object> cb) {
            G.Instance.StartCoroutine(getPrefab(name, e => {
                G.Instance.Prefabs[name] = e as GameObject;
                if (cb!=null) cb(e);
            }));
            
        }
    }
}
