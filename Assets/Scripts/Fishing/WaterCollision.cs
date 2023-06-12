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
            if (fishingControlls.fishingStatus == FishingControlls.FishingStatus.Casting)
            {
                audioSource.Play();
            }
            if (fishingControlls.fishingStatus != FishingControlls.FishingStatus.Reeling)
            {
                fishingControlls.fishingStatus = FishingControlls.FishingStatus.Fishing;

            }
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
