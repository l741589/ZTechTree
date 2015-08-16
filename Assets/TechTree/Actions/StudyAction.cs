using Assets.TechTree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Layout;
using UnityEngine;

namespace Assets.TechTree.Actions {

    public class StudyAction : DelayedAction {

        protected override bool OnPreTime() {
            if (!(Item is UTech)|| (Item as UTech).Studied) return false;
            return true;
        }

        public override void OnDataUpdate() {
            if (Item is UTech) {
                if (((UTech)Item).Studied) {
                    Time = null;
                    Tag = null;
                } else {
                    Time = Raw.time + "秒";
                    Tag = "New!!";
                }
            }
        }

        protected override void OnTimeDone() {
            if (Item is UTech) {
                var t = Item as UTech;
                t.Studied = true;
                Tag=null;
            }
        }

        public override bool Enabled {
            get {
                return (Item is UTech && !(Item as UTech).Studied);
            }
        }
    }

        
}
