using UnityEngine;

public class RythmGameController : MonoBehaviour
{
    [SerializeField] MusicController musicController;
    [SerializeField] GameObject rythmGame;

    private void Start()
    {
        rythmGame.SetActive(false);
    }
    public void StartMusicGame()
    {
        musicController.PlayMusicGameMusic();
        rythmGame.SetActive(true);
    }
    public void EndMusicGame()
    {
        musicController.StopMusicGameMusic();
        rythmGame.SetActive(false);
    }
}
