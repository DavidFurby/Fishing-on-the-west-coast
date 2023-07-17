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
            if (fishingControlls.stateMachine.GetCurrentState() is not Reeling && fishingControlls.stateMachine.GetCurrentState() is not ReelingFish)
            {
                bait.UpdateDistanceRecord();
                fishingControlls.stateMachine.SetState(new Fishing());
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
