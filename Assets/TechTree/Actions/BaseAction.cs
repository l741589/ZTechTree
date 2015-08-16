using Assets.TechTree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Layout;
using Assets.Util;
using UnityEngine;

namespace Assets.TechTree.Actions {
    public class BaseAction : Rawable<ItemAction>{

        public BaseAction() { }

        private string time;
        private string num;
        private string tag;

        public String Time { get { return time; } set { time = value; DataUpdate(); } }
        public String Num { get { return num; } set { num = value; DataUpdate(); } }
        public String Tag { get { return tag; } set { tag = value; DataUpdate(); } }

        public virtual bool Enabled{get{return true;}}

        public UBaseItem Item { get; private set; }
        public WeakReference<ActionButton> Button { get; set; }
        public virtual void OnLoaded() { OnDataUpdate(); }
        public virtual void OnAction(int action) { }
        public override void Init(ItemAction raw) {
            base.Init(raw);
            this.Item = FastData.LookupAcestor<BaseItem>(raw).UserData;
            OnLoaded();
        }

        public void DataUpdate() {
            if (Item.Button != null) {
                Item.Button.Target.DataUpdated();
            }
            if (Button != null&&Button.Target!=null) {
                Button.Target.DataUpdated();
            }
        }

        public virtual void OnDataUpdate() {
            DataUpdate();
        }

        public static BaseAction Create(ItemAction action) {
            BaseAction ba=null;
            switch (action.id) {
            case "study": ba = new StudyAction(); break;
            case "produce": ba = new ProduceAction(); break;
            }
            return ba;
        }
    }

    


    public abstract class DelayedAction : BaseAction {
        protected DateTime? EndTime { get; set; }
        public DelayedAction() {

        }

        public override void OnAction(int action) {
            if (EndTime != null) return;            
            CountDown();
            Item.UpdateDependences();            
        }

        public void CountDown() {
            if (OnPreTime()) {
                EndTime = DateTime.UtcNow.AddSeconds(Raw.time);
                Scheduler.Loop(OnCountDown);
            }
        }

        public void TimeDone() {
            OnTimeDone();
            Time=null;
            EndTime = null;
            OnDataUpdate();
            Item.UpdateReverseDependences();
        }

        protected virtual float OnCountDown() {
            if (EndTime != null) {
                var span = EndTime.Value.Subtract(DateTime.UtcNow);
                if (span.Ticks > 0) {
                    Time=span.Hours + ":" + span.Minutes + ":" + span.Seconds;
                } else {
                    TimeDone();
                    OnDataUpdate();
                    return -1;
                }
                return 0.5f;
            } else {
                Time=null;
            }
            return -1;
        }
        protected virtual void OnTimeDone() {  }
        protected virtual bool OnPreTime() { return true; }
    }

}