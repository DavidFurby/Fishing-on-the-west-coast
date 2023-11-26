using UnityEngine;

public class MusicController : MonoBehaviour
{

    private AudioSource music;

    void OnEnable()
    {
        music = GetComponent<AudioSource>();
        WaterCollision.OnPlayerEnterSea += PlayMusic;
    }

    void OnDestroy()
    {
        WaterCollision.OnPlayerEnterSea -= PlayMusic;
    }
    public void PlayMusic(string newSong = null)
    {
        if (newSong != null)
        {
            ChangeMusic(newSong);
        }
        music.Play();

    }
    public void StopMusic()
    {
        music.Stop();
    }

    public void ChangeMusic(string clipName)
    {
        AudioClip newClip = Resources.Load("Sound/Music/" + clipName) as AudioClip;
        music.clip = newClip;
    }
}
