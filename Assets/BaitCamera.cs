using UnityEngine;

public class BaitCamera : MonoBehaviour
{
    public GameObject bait;
    [SerializeField] FishingControlls fishingControlls;
    private void LateUpdate()
    {
        if (fishingControlls.isFishing)
        {
            transform.position = new Vector3(bait.transform.position.x, 6, 2);
        }
    }
}
