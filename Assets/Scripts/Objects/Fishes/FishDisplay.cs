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
        if (gameObject != null)
        {
            if (gameObject.TryGetComponent(out ConfigurableJoint joint))
            {
                Destroy(joint);
            }
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
