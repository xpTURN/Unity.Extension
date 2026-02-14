using NUnit.Framework;
using UnityEngine;
using xpTURN.Coore;

namespace xpTURN.Misc.Tests
{
    public class UnityWeakReferenceTests
    {
        [Test]
        public void Constructor_WithNull_IsNotAlive()
        {
            var wr = new UnityWeakReference<ScriptableObject>(null);
            Assert.That(wr.IsAlive, Is.False);
            Assert.That(wr.TryGetTarget(out var _), Is.False);
        }

        [Test]
        public void Target_SetAndGet_ReturnsSameObject()
        {
            var obj = ScriptableObject.CreateInstance<ScriptableObject>();
            var wr = new UnityWeakReference<ScriptableObject>(obj);

            Assert.That(wr.IsAlive, Is.True);
            Assert.That(wr.Target, Is.SameAs(obj));
            Assert.That(wr.TryGetTarget(out var target), Is.True);
            Assert.That(target, Is.SameAs(obj));

            Object.DestroyImmediate(obj);
        }

        [Test]
        public void SetTarget_UpdatesTarget()
        {
            var a = ScriptableObject.CreateInstance<ScriptableObject>();
            var b = ScriptableObject.CreateInstance<ScriptableObject>();
            var wr = new UnityWeakReference<ScriptableObject>(a);

            wr.SetTarget(b);
            Assert.That(wr.Target, Is.SameAs(b));
            Assert.That(wr.TryGetTarget(out var target), Is.True);
            Assert.That(target, Is.SameAs(b));

            Object.DestroyImmediate(a);
            Object.DestroyImmediate(b);
        }

        [Test]
        public void Target_Setter_UpdatesTarget()
        {
            var a = ScriptableObject.CreateInstance<ScriptableObject>();
            var b = ScriptableObject.CreateInstance<ScriptableObject>();
            var wr = new UnityWeakReference<ScriptableObject>(a);

            wr.Target = b;
            Assert.That(wr.Target, Is.SameAs(b));

            Object.DestroyImmediate(a);
            Object.DestroyImmediate(b);
        }

        [Test]
        public void After_DestroyImmediate_IsNotAlive()
        {
            var wr = new UnityWeakReference<ScriptableObject>(null);

            {
                var obj = ScriptableObject.CreateInstance<ScriptableObject>();
                wr.Target = obj;

                Object.DestroyImmediate(obj);
                obj = null;    
            }

            Assert.That(wr.IsAlive, Is.False);
            Assert.That(wr.TryGetTarget(out var _), Is.False);
        }
    }
}
