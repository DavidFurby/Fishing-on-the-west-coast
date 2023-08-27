using UnityEngine;
using System.Collections;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private BaitLogic baitLogic;
    private const float baitShakeDelay = 2f;
    private bool isShaking = false;

    void OnEnable()
    {
        CatchArea.OnBaitFish += TryBaitingFish;
    }

    void OnDisable()
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

    private void OnTriggerStay(Collider other)
    {
        StartCoroutine(ShakeBaitOnKeyPress(other));
    }

    private IEnumerator ShakeBaitOnKeyPress(Collider collider)
    {
        if (!isShaking && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isShaking = true;
            baitLogic.PlaySplashSound();
            if (collider.CompareTag("Fish") && !FishingController.Instance.FishIsBaited)
            {
                TryBaitingFish(collider, baitLogic.gameObject);
            }

            yield return new WaitForSeconds(baitShakeDelay);
            isShaking = false;
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
