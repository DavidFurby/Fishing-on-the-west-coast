using UnityEngine;
using System;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private FishingSystem system;
    [HideInInspector] public bool fishInBaitArea = false;

    private void OnTriggerEnter(Collider collider)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fishInBaitArea)
        {
            fishInBaitArea = false;
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
