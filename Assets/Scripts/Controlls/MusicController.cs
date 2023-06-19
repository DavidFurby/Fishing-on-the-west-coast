using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] AudioSource theMusic;
    public void PlayMiniGameMusic()
    {
        theMusic.Play();

    }
    public void StopFishingMiniGameMusic()
    {
        theMusic.Stop();
    }
}
