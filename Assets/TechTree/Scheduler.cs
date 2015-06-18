using UnityEngine;
using System.Collections;
using Assets.Util;
using System;
using System.Collections.Generic;

public class Scheduler : MonoBehaviour {

    private static Heap<float, Action> tasks = new Heap<float, Action>();

    void FixedUpdate() {
        while (tasks.Count > 0 && tasks.PeekKey() <= Time.time) {
            var a = tasks.PopValue();
            a();
        }
    }

    public static void Add(float when, Action a) {
        tasks.Push(Time.time + when, a);
    }

    public static void Loop(float delay,int count,Action a){
        Add(getLooper(delay,count,a));
    }

    public static void Loop(Func<float> a) {
        Add(getLooper(a));
    }

    private static IEnumerator<float> getLooper(Func<float> a) {
        while(true){
            var delay = a();
            if (delay > 0) yield return delay;
            else break;
        }
    }

    private static IEnumerator<float> getLooper(float delay,int count,Action a){
        for(int i=0;i<count;++i){
            yield return delay;
            a();
        }
    }

    private class Enumerate {
        public IEnumerator<float> iter;
        public void Exec() {
            if (iter.MoveNext()) {
                var i = iter.Current;
                if (i>0) Scheduler.Add(i, Exec);
            }
        }
    }
    public static void Add(IEnumerator<float> iter) {
        var e = new Enumerate();
        e.iter = iter;
        e.Exec();
    }

    public static void Add(Func<IEnumerator<float>> iter) {
        var e = new Enumerate();
        e.iter = iter();
        e.Exec();
    }
}
