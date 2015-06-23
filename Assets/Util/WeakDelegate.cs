using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Assets.Util {
    public class WeakDelegate{
        public WeakReference dele;
        public WeakDelegate Next;
        public WeakDelegate() {
            dele = null;
            Next = null;
        }

        public void Init(Delegate method) {
            dele = new WeakReference(method);
            Next = null;
        }
        public WeakDelegate(Delegate method) {
            Init(method);
        }

        public void Add(Delegate method) {
            RecycleCurrent();
            if (dele == null||!dele.IsAlive) dele = new WeakReference(method);
            else if (Next == null) {
                Next = (WeakDelegate)GetType().GetConstructor(new Type[0]).Invoke(null);
                Next.Init(method);
            } else Next.Add(method);
        }

        protected bool RecycleCurrent() {
            if (dele == null) {
                if (Next == null) return false;
                dele = Next.dele;
                Next = Next.Next;
                RecycleCurrent();
                return true;
            }
            if (dele.IsAlive) return false;
            if (Next == null) {
                dele = null;
            } else {
                dele = Next.dele;
                Next = Next.Next;
                RecycleCurrent();
            }
            return true;
        }

        public void Remove(Delegate method) {
            if (!dele.IsAlive) {
                if (Next == null) dele = null;
                else {
                    dele = Next.dele;
                    Next = Next.Next;
                }
                Remove(method);
            }else if (Object.Equals(dele.Target, method)) {
                if (Next == null) dele = null;
                else {
                    dele = Next.dele;
                    Next = Next.Next;
                }
            } else {
                if (Next == null) return;
                Next.Remove(method);
            }
        }

        public virtual object Invoke(params object[] args) {
            object ret = null;
            RecycleCurrent();
            if (dele!=null) ret=((Delegate)dele.Target).DynamicInvoke(args);
            if (Next!=null) ret=Next.Invoke(args);
            return ret;   
        }

        public bool Empty() {
            while(RecycleCurrent());
            return dele == null;
        }
    }

    public class WeakDelegate<T> : WeakDelegate {
        public void Add(Action<T> method) {
            base.Add(method);
        }

        public void Remove(Action<T> method) {
            base.Remove(method);
        }

        public void Invoke(T args) {
            RecycleCurrent();
            if (dele!=null) ((Action<T>)dele.Target)(args);
            if (Next!=null) ((WeakDelegate<T>)Next).Invoke(args);
        }
    }

    public class WeakDelegate<T1,T2> : WeakDelegate {
        public void Add(Action<T1,T2> method) {
            base.Add(method);
        }

        public void Remove(Action<T1, T2> method) {
            base.Remove(method);
        }

        //public void Invoke(T1 arg1,T2 arg2) {
        //    RecycleCurrent();
        //    if (dele != null) ((Action<T1, T2>)dele.Target)(arg1, arg2);
        //    if (Next != null) ((WeakDelegate<T1, T2>)Next).Invoke(arg1, arg2);
        //}
    }
}
