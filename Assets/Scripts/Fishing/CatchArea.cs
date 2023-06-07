using TMPro;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool isInCatchArea;
    private GameObject fish;
    [SerializeField] private FishingControls fishingControls;
    private FishMovement fishMovement;
    [SerializeField] private RythmGameController rythmGame;

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
        if (fish != null && fishMovement != null && rythmGame != null)
        {
            fishMovement.state = FishMovement.FishState.Hooked;
            rythmGame.StartMusicGame();
        }
    }

    public void CollectFish()
    {
        if (fish != null && rythmGame != null)
        {
            Destroy(fish);
            MainManager.Instance.game.Fishes++;
            rythmGame.EndMusicGame();
        }
    }
}