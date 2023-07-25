using UnityEngine;
using System;
using System.Collections;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private FishingSystem system;
    [HideInInspector] public bool fishInBaitArea = false;

    private void OnTriggerEnter(Collider collider)
    {
        TryBaitingFish(collider);
    }

    private void OnTriggerStay(Collider other)
    {
        StartCoroutine(BaitManually(other));
    }

    private IEnumerator BaitManually(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            system.baitLogic.Shake();
            TryBaitingFish(other);
            yield return new WaitForSeconds(2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fishInBaitArea)
        {

            FishMovement fishMovement = other.gameObject.GetComponent<FishMovement>();
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
        if (collider.gameObject.CompareTag("Fish") && !fishInBaitArea)
        {
            FishMovement fishMovement = collider.gameObject.GetComponent<FishMovement>();
            FishDisplay fish = collider.gameObject.GetComponent<FishDisplay>();
            float probability = GetProbability(fish.fish.level, system.bait.level);
            if (UnityEngine.Random.Range(0f, 1f) < probability)
            {
                fishInBaitArea = true;
                fishMovement.GetBaited(system.baitLogic.gameObject);
            }
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
