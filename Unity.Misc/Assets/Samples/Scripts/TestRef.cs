using Cysharp.Threading.Tasks;
using UnityEngine;

using xpTURN.Coore;

public class TestRef : MonoBehaviour
{
    UnityWeakReference<Test> refOne = new(null);

    void Start()
    {
        refOne.Target = FindAnyObjectByType<Test>();

        DoCheck().Forget();
    }

    async UniTask DoCheck()
    {
        bool alive = true;
        while (alive)
        {
            await UniTask.WaitForEndOfFrame();

            Debug.Log($"RefOne : IsAlive : {refOne.IsAlive}");
            Debug.Log($"RefOne : TryGetTarget : {refOne.TryGetTarget(out var _)}");

            alive = refOne.IsAlive;
        }

        return;
    }
}
