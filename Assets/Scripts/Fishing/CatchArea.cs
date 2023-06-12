using UnityEngine;

public class CatchArea : MonoBehaviour
{
    public bool isInCatchArea;
    public GameObject fish;
    private FishMovement fishMovement;
    [SerializeField] private RythmGameController rythmGame;
    [SerializeField] private AudioSource wowSound;

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
        if (fish != null && fishMovement != null && fishMovement.state != FishMovement.FishState.Hooked && rythmGame != null)
        {
            fishMovement.state = FishMovement.FishState.Hooked;
            rythmGame.StartMusicGame();
        }
    }

    public void CollectFish()
    {
        if (fish != null && rythmGame != null)
        {
            MainManager.Instance.game.Fishes++;
            rythmGame.EndMusicGame();
            PresentCatch();
        }
    }

    private void PresentCatch()
    {
        wowSound.PlayDelayed(2);
        fishMovement.PresentFish();
    }
}