using UnityEngine;
using System.Collections;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private FishingController fishingSystem;
    private const float baitShakeDelay = 2f;


    void Start()
    {
        CatchArea.BaitFish += TryBaitingFish;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Fish") && !fishingSystem.FishIsBaited)
        {
            TryBaitingFish(collider, fishingSystem.baitLogic.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        StartCoroutine(ShakeBaitOnKeyPress(other));
    }

    private IEnumerator ShakeBaitOnKeyPress(Collider collider)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            fishingSystem.baitLogic.Shake();
            if (collider.CompareTag("Fish") && !fishingSystem.FishIsBaited)
            {
                TryBaitingFish(collider, fishingSystem.baitLogic.gameObject);
            }

            yield return new WaitForSeconds(baitShakeDelay);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fishingSystem.FishIsBaited)
        {
            FishMovement fishMovement = other.GetComponent<FishMovement>();
            if (fishMovement.GetCurrentState() is Baited)
            {
                fishingSystem.FishIsBaited = false;
                fishMovement.SetState(new Swimming(fishMovement));
            }
        }
    }

    public void TryBaitingFish(Collider collider, GameObject target)
    {
        FishMovement fishMovement = collider.GetComponent<FishMovement>();
        FishDisplay fish = collider.GetComponent<FishDisplay>();
        float probability = GetProbability(fish.fish.level, MainManager.Instance.game.Inventory.EquippedBait.level);
        if (UnityEngine.Random.Range(0f, 1f) < probability)
        {
            fishMovement.GetBaited(target);
            fishingSystem.FishIsBaited = true;
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
