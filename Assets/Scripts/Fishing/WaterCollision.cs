using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] GameObject sea;
    [SerializeField] Bait bait;
    [SerializeField] FishingControlls fishingControlls;
    [SerializeField] AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sea)
        {
            if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.Casting)
            {
                audioSource.Play();
            }
            fishingControlls.fishingStatus = FishingControlls.GetFishingStatus.Fishing;
            bait.inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sea)
        {
            bait.inWater = false;
        }
    }


}
