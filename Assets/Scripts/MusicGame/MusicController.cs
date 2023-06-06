using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] AudioSource theMusic;
    public bool startPlaying = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startPlaying)
        {
            theMusic.Play();
            startPlaying = false;
        }
    }
}
