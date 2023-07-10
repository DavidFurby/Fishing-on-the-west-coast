using UnityEngine;

public class BaitArea : MonoBehaviour
{
    [SerializeField] BaitLogic baitLogic;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fish"))
        {
            FishMovement fishMovement = collider.gameObject.GetComponent<FishMovement>();
            FishDisplay fish = collider.gameObject.GetComponent<FishDisplay>();
            float probabilty = GetProbability(fish.fish.level, baitLogic.currentBait.level);
            if (Random.Range(0f, 1f) < probabilty)
            {
                fishMovement.GetBaited(baitLogic.gameObject);
            }

        }
    }
    private float GetProbability(int fishLevel, int baitLevel)
    {
        int difference = Mathf.Abs(fishLevel - baitLevel);
        return 1f / (1f + difference);
    }
}
