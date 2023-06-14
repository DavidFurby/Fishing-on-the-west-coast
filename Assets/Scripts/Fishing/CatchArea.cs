using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool isInCatchArea;
    public GameObject fish;
    private FishMovement fishMovement;
    [SerializeField] private CatchSummary catchSummary;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && fish == null)
        {
            isInCatchArea = true;
            fish = other.gameObject;
            fishMovement = fish.GetComponent<FishMovement>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish") && fish == other.gameObject)
        {
            isInCatchArea = false;
            fish = null;
        }
    }

    public void CatchFish()
    {
        if (fish != null && fishMovement != null && fishMovement.state != FishMovement.FishState.Hooked)
        {
            fishMovement.state = FishMovement.FishState.Hooked;
        }
    }
    public void RemoveCatch()
    {
        Destroy(fish);
        fishMovement = null;
        isInCatchArea = false;
    }
}