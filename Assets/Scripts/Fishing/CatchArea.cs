using TMPro;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool isInCatchArea;
    private GameObject fish;
    [SerializeField] FishingControlls fishingControlls;
    private FishMovement fishMovement;
    [SerializeField] RythmGameController rythmGame;
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
            rythmGame.StartMusicGame();

        }
    }
    public void CollectFish()
    {
        if (fish != null)
        {
            Debug.Log(fish);
            Destroy(fish);
            MainManager.Instance.game.Fishes++;
            rythmGame.EndMusicGame();
        }
    }
}
