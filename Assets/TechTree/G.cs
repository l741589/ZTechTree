using Assets.TechTree.Data;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Macros;
using UnityEngine;

namespace Assets.TechTree {
    public class Prefabs : Dictionary<String,GameObject>{
        public static readonly String[] Names = { "NavButton", "ItemButton","ActionButton" };
        public bool IsLoaded {
            get {
                foreach (var e in Names) if (!ContainsKey(e)) return false;
                return true;
            }
        }

        public void Init() {
            foreach (var e in Names) {
                DataLoader.LoadPrefab(e, x => {
                    G.Instance.UpdateLoaded();
                });
            }
        }
    }
    
    public class G : MonoBehaviour {
        public delegate void LoadedHandler(G g);

        public static G Instance { get; private set; }
        public GameData data { get;private set; }
        public FastData fdata { get; private set; }
        public UserData udata { get; private set; }

        public event LoadedHandler Loaded;
        private Boolean isLoaded = false;
        public Boolean IsLoaded {get { return UpdateLoaded(); } }
        public Prefabs Prefabs { get; private set; }

        public WeakReference<ListBox> ListBox { get; set; }
        public WeakReference<ActionPanel> ActionPanel { get; set; }

        public bool UpdateLoaded(){
            if (isLoaded) return true;
            if (data == null) return false;
            if (Prefabs == null) return false;
            if (!Prefabs.IsLoaded) return false;
            isLoaded = true;
            if (Loaded != null) Loaded(Instance);
            return true;
        }

        public G() {
            Instance = this;
            Prefabs = new Prefabs();
           
        }


        void Start() {
            Prefabs.Init();
            DataLoader.LoadObject<GameData>("gamedata.json", GameDataPrepare.Prepare, d => {
                data = d;
                fdata = new FastData(data);
                udata = new UserData();
                udata.InitData();
                UpdateLoaded();
            });            
            
            
            OnLoaded(g => {
                Scheduler.Loop(() => {
                    Var.SetVar<float>(Var.food, f => f - Var.GetVar<int>(Var.people) * 0.1f);
                    return 1;
                });
            });
        }

        public void OnLoaded(LoadedHandler h) {
            if (IsLoaded) h(Instance);
            else Loaded += h;
        }
    }
}
