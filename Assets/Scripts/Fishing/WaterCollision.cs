using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] GameObject sea;
    [SerializeField] BaitLogic bait;
    [SerializeField] FishingControlls fishingControlls;
    [SerializeField] AudioSource splashSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sea)
        {

            if (fishingControlls.fishingStatus != FishingControlls.FishingStatus.Reeling && fishingControlls.fishingStatus != FishingControlls.FishingStatus.ReelingFish)
            {
                fishingControlls.SetFishingStatus(FishingControlls.FishingStatus.Fishing);
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
