using UnityEngine;

public class OffScreenDespawn : MonoBehaviour
{
    private Camera _mainCamera;
    private float despawnThreshold = 500f;
    //threshold 0 == touch the screen edge and die, 500 == get a bit outside of the screeb edge and then die

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 fishScreenPosition = _mainCamera.WorldToScreenPoint(transform.position);

        //Yes it looks weird but it works
        if (fishScreenPosition.x < 0 - despawnThreshold || fishScreenPosition.x > Screen.width + despawnThreshold)
        {
            Destroy(gameObject);
        }
    }
}
