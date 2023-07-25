using UnityEngine;
using System.Collections;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private FishingSystem fishingSystem;
    private const float baitShakeDelay = 2f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Fish") && !fishingSystem.IsInBaitArea)
        {
            TryBaitingFish(collider);
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
            if (collider.CompareTag("Fish") && !fishingSystem.IsInBaitArea)
            {
                TryBaitingFish(collider);
            }
            yield return new WaitForSeconds(baitShakeDelay);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fishingSystem.IsInBaitArea)
        {
            FishMovement fishMovement = other.GetComponent<FishMovement>();
            if (fishMovement.GetCurrentState() is Baited)
            {
                fishingSystem.IsInBaitArea = false;
                fishMovement.SetState(new Swimming(fishMovement));
            }
        }
    }

    private void TryBaitingFish(Collider collider)
    {
        FishMovement fishMovement = collider.GetComponent<FishMovement>();
        FishDisplay fish = collider.GetComponent<FishDisplay>();
        float probability = GetProbability(fish.fish.level, fishingSystem.bait.level);
        if (UnityEngine.Random.Range(0f, 1f) < probability)
        {
            fishMovement.GetBaited(fishingSystem.baitLogic.gameObject);
            fishingSystem.IsInBaitArea = true;
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
