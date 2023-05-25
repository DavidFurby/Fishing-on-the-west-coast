using TMPro;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool isInCatchArea;
    private GameObject fish;
    [SerializeField] FishingControlls fishingControlls;
    [SerializeField] FishingCanvas fishingCanvas;
    private FishMovement fishMovement
        ;
    private void Update()
    {
        CollectFish();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            isInCatchArea = true;
            fish = other.gameObject;
            fishMovement = fish.GetComponent<FishMovement>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            isInCatchArea = false;
            fish = null;
        }
    }
    public void CatchFish()
    {
        if (fish != null)
        {
            fishMovement.hooked = true;
        }
    }
    public void CollectFish()
    {
        if (fishingControlls.fishingStatus == FishingControlls.GetFishingStatus.StandBy && fish != null)
        {
            Destroy(fish);
            fishingCanvas.fishCount++;
        }
    }
}
