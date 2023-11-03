using UnityEngine;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private BaitLogic baitLogic;

    private void OnEnable()
    {
        CatchArea.OnBaitFish += TryBaitingFish;
    }

    private void OnDestroy()
    {
        CatchArea.OnBaitFish -= TryBaitingFish;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (CanFishBeBaited(collider))
        {
            TryBaitingFish(collider, baitLogic.gameObject);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (baitLogic.IsPulling && CanFishBeBaited(collider))
        {
            print("In trigger");
            TryBaitingFish(collider, baitLogic.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsFishLeavingBaitArea(other))
        {
            ReleaseBaitedFish(other);
        }
    }

    public void TryBaitingFish(Collider collider, GameObject target)
    {
        FishController fishController = collider.GetComponent<FishController>();
        FishDisplay fish = collider.GetComponent<FishDisplay>();
        float probability = GetProbability(fish.fish.level, MainManager.Instance.Inventory.EquippedBait.level);
        if (WillGetBaited(fishController, probability))
        {
            BaitFish(fishController, target);
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }

    private bool CanFishBeBaited(Collider collider)
    {
        return collider.CompareTag("Fish") && PlayerController.Instance.BaitedFish == null;
    }

    private bool IsFishLeavingBaitArea(Collider other)
    {
        return other.CompareTag("Fish") && PlayerController.Instance.BaitedFish != null;
    }

    private void ReleaseBaitedFish(Collider other)
    {
        FishController fishController = other.GetComponent<FishController>();
        if (fishController != null && fishController.GetCurrentState() is Baited)
        {
            PlayerController.Instance.BaitedFish = null;
            fishController.SetState(new Swimming(fishController));
        }
    }

    private bool WillGetBaited(FishController fishController, float probability)
    {
        return Random.Range(0f, 1f) < probability && fishController.GetCurrentState() is Swimming;
    }

    private void BaitFish(FishController fishController, GameObject target)
    {
        fishController.fishBehaviour.GetBaited(target);
        PlayerController.Instance.BaitedFish = fishController.GetComponent<FishDisplay>();

    }
}
