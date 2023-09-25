using UnityEngine;

public class MusicController : MonoBehaviour
{

    private AudioSource theMusic;

    void OnEnable()
    {
        theMusic = GetComponent<AudioSource>();
    }
    public void PlayMiniGameMusic()
    {
        theMusic.Play();

    }
    public void StopFishingMiniGameMusic()
    {
        theMusic.Stop();
    }
}
