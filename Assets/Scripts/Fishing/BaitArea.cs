using UnityEngine;
using System.Collections;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private FishingController fishingController;
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
        if (fishingController != null && collider.CompareTag("Fish") && !fishingController.FishIsBaited)
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
            if (collider.CompareTag("Fish") && !fishingController.FishIsBaited)
            {
                TryBaitingFish(collider, baitLogic.gameObject);
            }

            yield return new WaitForSeconds(baitShakeDelay);
            isShaking = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fishingController.FishIsBaited)
        {
            FishMovement fishMovement = other.GetComponent<FishMovement>();
            if (fishMovement != null && fishMovement.GetCurrentState() is Baited)
            {
                fishingController.FishIsBaited = false;
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
            fishingController.FishIsBaited = true;
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
