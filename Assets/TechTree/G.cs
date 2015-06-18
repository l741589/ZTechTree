using Assets.TechTree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.TechTree {
    public class Prefabs : Dictionary<String,GameObject>{
        public static readonly String[] Names = { "NavButton", "ItemButton" };
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
        public ListBox ListBox { get; set; }
        public Prefabs Prefabs { get; private set; }
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
            DataLoader.LoadObject<GameData>("gamedata.json", d => {
                data = d;
                UpdateLoaded();
            });
            OnLoaded(e => {
                fdata = new FastData(data);
                udata = new UserData();
                udata.InitData();
            });
        }

        public void OnLoaded(LoadedHandler h) {
            if (IsLoaded) h(Instance);
            else Loaded += h;
        }
    }
}
