using UnityEngine;

public class BaitArea : MonoBehaviour
{
    [SerializeField] private FishingSystem system;
    [HideInInspector] public FishDisplay fish;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fish") && fish == null)
        {
            FishMovement fishMovement = collider.gameObject.GetComponent<FishMovement>();
            FishDisplay fish = collider.gameObject.GetComponent<FishDisplay>();
            float probability = GetProbability(fish.fish.level, system.bait.level);
            if (Random.Range(0f, 1f) < probability)
            {
                this.fish = fish;
                fishMovement.GetBaited(system.baitLogic.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fish == other.gameObject)
        {
            Debug.Log("exit");
            fish = null;
        }
    }

    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
