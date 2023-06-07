using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] AudioSource theMusic;
    public void PlayMusicGameMusic()
    {
        theMusic.Play();

    }
    public void StopMusicGameMusic()
    {
        theMusic.Stop();
    }
}
