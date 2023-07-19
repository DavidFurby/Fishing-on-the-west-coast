using Unity.VisualScripting;
using UnityEngine;

public class FishDisplay : MonoBehaviour
{
    public Fish fish;

    public void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(this.gameObject);
    }
}
