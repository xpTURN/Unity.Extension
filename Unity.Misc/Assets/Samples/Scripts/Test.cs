using UnityEngine;

using Cysharp.Threading.Tasks;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await UniTask.DelayFrame(10);
        Destroy(gameObject);
    }
}
