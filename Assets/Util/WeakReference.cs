using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public class WeakReference<T> : WeakReference {

        public WeakReference(T target) :base(target){
            
        }

        public WeakReference(T target,bool trackResurrection)
            : base(target, trackResurrection) {

        }

        public new T Target {
            get {
                return (T)base.Target;
            }
            set {
                base.Target = value;
            }
        }

        public static implicit operator T(WeakReference<T> reference) {
            return reference.Target;
        }

        public static implicit operator WeakReference<T>(T val) {
            return new WeakReference<T>(val);
        }

        public override bool Equals(object obj) {
            if (obj == null) return !IsAlive;
            if (obj is T&&IsAlive) return Object.Equals(Target,obj);
            if (obj is WeakReference<T>) return Object.Equals(Target,((WeakReference)obj).Target);
            return false;
        }

        public override int GetHashCode() {
            return IsAlive ? Target.GetHashCode() : 0;
        }
    }
}
