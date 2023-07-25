using UnityEngine;
using System.Collections;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private FishingSystem fishingSystem;
    [HideInInspector] public bool fishInBaitArea = false;
    private const float baitShakeDelay = 2f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Fish") && !fishInBaitArea)
        {
            TryBaitingFish(collider);
            fishInBaitArea = true;
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
            if (collider.CompareTag("Fish") && !fishInBaitArea)
            {
                TryBaitingFish(collider);
                fishInBaitArea = true;
            }
            yield return new WaitForSeconds(baitShakeDelay);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fishInBaitArea)
        {
            FishMovement fishMovement = other.GetComponent<FishMovement>();
            if (fishMovement.GetCurrentState() is Baited)
            {
                fishInBaitArea = false;
                fishMovement.SetState(new Swimming(fishMovement));
                Debug.Log(fishMovement.GetCurrentState());
            }
        }
    }

    private void TryBaitingFish(Collider collider)
    {
        FishMovement fishMovement = collider.GetComponent<FishMovement>();
        Fish fish = collider.GetComponent<Fish>();
        float probability = GetProbability(fish.level, fishingSystem.bait.level);
        if (UnityEngine.Random.Range(0f, 1f) < probability)
        {
            fishMovement.GetBaited(fishingSystem.baitLogic.gameObject);
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
