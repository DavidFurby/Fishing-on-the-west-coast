using Unity.VisualScripting;
using UnityEngine;

public class FishDisplay : MonoBehaviour
{
    public Fish fish;

    private void Start()
    {
        SetFishSize();
    }

    private void SetFishSize()
    {

        fish.size = Random.Range(fish.averageSize / 2, fish.averageSize * 2);

    }

    public void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(this.gameObject);
    }
}
