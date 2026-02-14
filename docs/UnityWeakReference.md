# UnityWeakReference\<T\>

A weak-reference wrapper for Unity’s `UnityEngine.Object`.  
It extends `System.WeakReference` but respects Unity’s special null semantics so that `IsAlive` and the target are evaluated correctly.

## Why it exists

- **Plain `WeakReference`**: Once the target is collected by the GC, `Target` becomes null and `IsAlive` is false.
- **Unity objects**: `UnityEngine.Object` can be in a “destroyed” state (fake null) even when the C# reference exists, so null checks must use Unity’s overloaded `== null` behavior.
- **UnityWeakReference**: It casts `Target` to `T` (a Unity object) and checks that result with Unity’s null semantics, then exposes `IsAlive` and `TryGetTarget` accordingly.

So when holding a weak reference to a Unity object, you can safely tell whether it’s still valid from Unity’s point of view.

## Namespace / location

- **Namespace**: `xpTURN.Coore`
- **Script path**: [Unity.Misc/Assets/Scripts/Misc/UnityWeakReference.cs](../Unity.Misc/Assets/Scripts/Misc/UnityWeakReference.cs)

## Usage example

```csharp
var obj = GetComponent<SomeBehaviour>();
var weak = new UnityWeakReference<SomeBehaviour>(obj);

// Later...
if (weak.TryGetTarget(out var target))
{
    target.DoSomething(); // Use only while still valid
}
```

## Notes

- `T` must be a type derived from `UnityEngine.Object`.
- The `Target` getter uses `as T` followed by a non-null assertion (`!`) internally, so reading `Target` when the object is already destroyed can still yield null at runtime. Prefer checking with `IsAlive` or `TryGetTarget` before use.
