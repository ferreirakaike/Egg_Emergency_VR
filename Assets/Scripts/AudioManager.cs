using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the audio of the scene
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource CollectSound;
    public AudioSource MissedSound;
    public AudioSource BombSound;
    public AudioSource BackGroundMusic;

    /// <summary>
    /// Method that is called to play the "THUD" sound when pumpkin is caught
    /// </summary>
    public void PlayCollectSound()
    {
        CollectSound.Play();
    }

    /// <summary>
    /// Method that is called to play the laughing sound when pumpkin is missed
    /// </summary>
    public void PlayMissedSound()
    {
        MissedSound.Play();
    }

    /// <summary>
    /// Method that is called to play the explosion sound when bomb is caught
    /// </summary>
    public void PlayBombSound()
    {
        BombSound.Play();
    }

    /// <summary>
    /// Method that is called to stop the background music
    /// </summary>
    public void StopBackgroundMusic()
    {
        BackGroundMusic.Stop();
    }
}
