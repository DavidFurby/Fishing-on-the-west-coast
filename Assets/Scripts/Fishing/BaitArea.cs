using UnityEngine;

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
        if (PlayerController.Instance != null && collider.CompareTag("Fish") && !PlayerController.Instance.BaitedFish)
        {
            TryBaitingFish(collider, baitLogic.gameObject);
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (baitLogic.IsPulling && PlayerController.Instance != null && collider.CompareTag("Fish") && !PlayerController.Instance.BaitedFish)
        {
            TryBaitingFish(collider, baitLogic.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && PlayerController.Instance.BaitedFish)
        {
            FishMovement fishMovement = other.GetComponent<FishMovement>();
            if (fishMovement != null && fishMovement.GetCurrentState() is Baited)
            {
                PlayerController.Instance.BaitedFish = null;
                fishMovement.SetState(new Swimming(fishMovement));
            }
        }
    }

    public void TryBaitingFish(Collider collider, GameObject target)
    {
        FishMovement fishMovement = collider.GetComponent<FishMovement>();
        FishDisplay fish = collider.GetComponent<FishDisplay>();
        float probability = GetProbability(fish.fish.level, MainManager.Instance.Inventory.EquippedBait.level);
        if (
            Random.Range(0f, 1f) < probability)
        {
            fishMovement.GetBaited(target);
            PlayerController.Instance.BaitedFish = target.GetComponent<FishDisplay>();
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
