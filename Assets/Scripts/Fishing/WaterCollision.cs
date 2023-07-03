using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] GameObject sea;
    [SerializeField] BaitLogic bait;
    [SerializeField] FishingController fishingControlls;
    [SerializeField] AudioSource splashSound;
    [SerializeField] SeaSpawner seaSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sea)
        {

            if (fishingControlls.fishingStatus != FishingController.FishingStatus.Reeling && fishingControlls.fishingStatus != FishingController.FishingStatus.ReelingFish)
            {
                bait.UpdateDistanceRecord();
                fishingControlls.SetFishingStatus(FishingController.FishingStatus.Fishing);
            }
            splashSound.Play();
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
