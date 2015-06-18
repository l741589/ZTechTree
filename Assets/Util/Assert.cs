using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Util {
    public class AssertException : ApplicationException{
        public AssertException(String s) : base("Assertion Failed: "+s) { }
        public AssertException(String fmt,params object[] args) : this(String.Format(fmt,args)) { }
    }
    public static class Assert {
        private static void T(String fmt,params object[] args){
            throw new AssertException(fmt,args);
        }
        public static void AreEqual(object a,object b){
            if (!Object.Equals(a,b)) T("{0}={!}",a,b);
        }

        public static void IsNull(object a) {
            if (!Object.Equals(a, null)) T("{0} is null", a);
        }

        public static void NotNull(object a) {
            if (Object.Equals(a, null)) T("{0} not null", a);
        }

        public static void IsType<T>(object a) {
            if (!(a is T)) Assert.T("{0} is {1}", a, typeof(T).FullName);
        }
        public static void True(bool b) {
            True(b, "not true");
        }
        public static void True(bool b,string msg) {
            if (!b) Assert.T(msg);
        }
        public static void False(bool b) {
            True(b, "not false");
        }
        public static void False(bool b,string msg) {
            if (b) Assert.T(msg);
        }
        public static void Error(string msg){
            T(msg);
        }
    }
}
