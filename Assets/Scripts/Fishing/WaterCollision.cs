using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] GameObject sea;
    [SerializeField] BaitLogic bait;
    [SerializeField] FishingSystem fishingControlls;
    [SerializeField] AudioSource splashSound;
    [SerializeField] SeaSpawner seaSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sea)
        {
            if (fishingControlls.GetCurrentState() is not Reeling && fishingControlls.GetCurrentState() is not ReelingFish)
            {
                bait.UpdateDistanceRecord();
                fishingControlls.SetState(new Fishing(fishingControlls));
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
