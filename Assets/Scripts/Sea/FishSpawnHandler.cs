using UnityEngine;

public class FishSpawnHandler : MonoBehaviour
{

    private readonly float rightSideThreshold = 500f;
    Vector3 fishScreenPosition;

    void Start()
    {
        fishScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
    }
    void Update()
    {
        OnOutOfBounds();
    }
    private void OnOutOfBounds()
    {

        if (fishScreenPosition.x > Screen.width + rightSideThreshold)
        {
            // Use object pooling instead of destroying the game object
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
