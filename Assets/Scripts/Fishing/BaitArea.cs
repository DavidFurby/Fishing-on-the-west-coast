using UnityEngine;

public class BaitArea : MonoBehaviour
{
    [SerializeField] FishingSystem system;
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("test");
        if (collider.gameObject.CompareTag("Fish"))
        {
            FishMovement fishMovement = collider.gameObject.GetComponent<FishMovement>();
            FishDisplay fish = collider.gameObject.GetComponent<FishDisplay>();
            float probabilty = GetProbability(fish.fish.level, system.bait.level);
            Debug.Log(Random.Range(0f, 1f) < probabilty);
            if (Random.Range(0f, 1f) < probabilty)
            {
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
