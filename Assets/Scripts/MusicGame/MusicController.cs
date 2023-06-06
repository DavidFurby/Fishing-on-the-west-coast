using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] AudioSource theMusic;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayMusicGameMusic()
    {
        theMusic.Play();

    }
    public void StopMusicGameMusic()
    {
        theMusic.Play();
    }
}
