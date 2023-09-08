using UnityEngine;

public class FishSpawnHandler : MonoBehaviour
{

    private float rightSideThreshold = 500f;
    private float leftSideThreshold = 1000f;

    private void OnBecameInvisible()
    {
        Vector3 fishScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (fishScreenPosition.x < 0 - leftSideThreshold || fishScreenPosition.x > Screen.width + rightSideThreshold)
        {
            // Use object pooling instead of destroying the game object
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
