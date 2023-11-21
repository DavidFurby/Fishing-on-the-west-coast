using UnityEngine;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private BaitLogic baitLogic;
        private Collider _collider;


    private void OnEnable()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        PlayerEventController.OnEnterFishing += SetTriggerActive;
        PlayerEventController.OnEnterIdle += SetTriggerInactive;
        CatchArea.OnBaitFish += TryBaitingFish;
    }

    private void OnDestroy()
    {
        PlayerEventController.OnEnterFishing -= SetTriggerActive;
        PlayerEventController.OnEnterIdle -= SetTriggerInactive;
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
    private void SetTriggerActive()
    {
        _collider.enabled = true;
    }
    private void SetTriggerInactive()
    {
        _collider.enabled = false;
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

    private bool IsFishLeavingBaitArea(Collider collider)
    {
        return collider.CompareTag("Fish") && PlayerController.Instance.BaitedFish != null;
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
        fishController.behaviour.GetBaited(target);
        PlayerController.Instance.BaitedFish = fishController.GetComponent<FishDisplay>();

    }
}
