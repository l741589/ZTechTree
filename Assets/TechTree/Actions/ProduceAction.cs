using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Layout;
using Assets.TechTree.Data;

namespace Assets.TechTree.Actions {
    public class ProduceAction : DelayedAction{

        public override void OnDataUpdate() {
            if (Item is UItem) {
                Time = Raw.time + "秒";
                Num = "*" + ((UItem)Item).Count;
            }
            base.OnDataUpdate();
        }

        protected override bool OnPreTime() {
            if (!Raw.paid.Pay()) return false;
            return true;
        }

        protected override void OnTimeDone() {
            if (Item is UItem) {
                ++((UItem)Item).Count;
            }
        }

        public override bool Enabled {
            get {
                return Raw.paid.Can();
            }
        }
    }
}
