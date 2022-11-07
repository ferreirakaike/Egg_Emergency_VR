using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource CollectSound;
    public AudioSource MissedSound;
    public AudioSource BombSound;
    public AudioSource BackGroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCollectSound()
    {
        CollectSound.Play();
    }

    public void PlayMissedSound()
    {
        MissedSound.Play();
    }

    public void PlayBombSound()
    {
        BombSound.Play();
    }

    public void StopBackgroundMusic()
    {
        BackGroundMusic.Stop();
    }
}
