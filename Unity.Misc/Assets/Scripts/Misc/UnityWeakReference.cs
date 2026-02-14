using UnityEngine;

namespace xpTURN.Coore
{
    public class UnityWeakReference<T> : System.WeakReference where T : UnityEngine.Object
    {
        public UnityWeakReference(T target) : base(target) { }

        public override bool IsAlive
        {
            get
            {
                // UnityEngine.Object has special null-check semantics, so we cast and check explicitly.
                T obj = Target as T;
                return obj != null;
            }
        }

        public new T Target
        {
            get { return (base.Target as T)!; }
            set { base.Target = value; }
        }

        public void SetTarget(T target)
        {
            base.Target = target;
        }

        public bool TryGetTarget(out T target)
        {
            target = (base.Target as T)!;
            return target != null;
        }
    }
}
