using UnityEngine;
using System.Collections;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private BaitLogic baitLogic;

    void OnEnable()
    {
        CatchArea.OnBaitFish += TryBaitingFish;
    }

    void OnDestroy()
    {
        CatchArea.OnBaitFish -= TryBaitingFish;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (FishingController.Instance != null && collider.CompareTag("Fish") && !FishingController.Instance.FishIsBaited)
        {
            TryBaitingFish(collider, baitLogic.gameObject);
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        // Check if the bait is being pulled and if the collider is a fish
        if (baitLogic.IsPulling && FishingController.Instance != null && collider.CompareTag("Fish") && !FishingController.Instance.FishIsBaited)
        {
            // Call the TryBaitingFish method
            TryBaitingFish(collider, baitLogic.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && FishingController.Instance.FishIsBaited)
        {
            FishMovement fishMovement = other.GetComponent<FishMovement>();
            if (fishMovement != null && fishMovement.GetCurrentState() is Baited)
            {
                FishingController.Instance.FishIsBaited = false;
                fishMovement.SetState(new Swimming(fishMovement));
            }
        }
    }

    public void TryBaitingFish(Collider collider, GameObject target)
    {
        FishMovement fishMovement = collider.GetComponent<FishMovement>();
        FishDisplay fish = collider.GetComponent<FishDisplay>();
        float probability = GetProbability(fish.fish.level, MainManager.Instance.Inventory.EquippedBait.level);
        if (UnityEngine.Random.Range(0f, 1f) < probability)
        {
            fishMovement.GetBaited(target);
            FishingController.Instance.FishIsBaited = true;
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
