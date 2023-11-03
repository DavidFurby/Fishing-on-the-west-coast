using UnityEngine;

public class FishSpawnHandler : MonoBehaviour
{

    private readonly float rightSideThreshold = 50f;
    Vector3 cameraPosition;

    void Start()
    {
        cameraPosition = Camera.main.WorldToScreenPoint(transform.position);
    }
    void Update()
    {
        OnOutOfBounds();
    }
    private void OnOutOfBounds()
    {

        if (cameraPosition.x > Screen.width + rightSideThreshold)
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
