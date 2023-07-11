using UnityEngine;

public class BaitArea : MonoBehaviour
{
    [SerializeField] BaitLogic baitLogic;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fish"))
        {
            FishMovement fishMovement = collider.gameObject.GetComponent<FishMovement>();
            Catch fish = collider.gameObject.GetComponent<Catch>();
            float probabilty = GetProbability(fish.Level, baitLogic.currentBait.level);
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
