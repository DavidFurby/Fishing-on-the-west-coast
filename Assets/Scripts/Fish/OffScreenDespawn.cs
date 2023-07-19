using UnityEngine;

public class OffScreenDespawn : MonoBehaviour
{
    [SerializeField]
    private float despawnThreshold = 500f;
    [SerializeField]
    private float leftSideThreshold = 1000f;

    private void OnBecameInvisible()
    {
        Vector3 fishScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if (fishScreenPosition.x < 0 - leftSideThreshold || fishScreenPosition.x > Screen.width + despawnThreshold)
        {
            // Use object pooling instead of destroying the game object
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
