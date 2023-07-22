using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] GameObject sea;
    [SerializeField] FishingSystem system;
    [SerializeField] AudioSource splashSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sea)
        {
            if (system.GetCurrentState() is Casting)
            {
                system.baitLogic.UpdateDistanceRecord();
                system.SetState(new Fishing(system));
            }
            splashSound.Play();
            system.baitLogic.inWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sea)
        {
            system.baitLogic.inWater = false;
        }
    }
}
