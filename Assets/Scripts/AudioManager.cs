using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that manages the audio of the scene
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// AudioSource that contains the "THUD" sound when pumpkin is caught
    /// </summary>
    public AudioSource CollectSound;

    /// <summary>
    /// AudioSource that contains the laughing sound when pumpkin is missed
    /// </summary>
    public AudioSource MissedSound;

    /// <summary>
    /// AudioSource that contains the explosion sound when bomb is caught
    /// </summary>
    public AudioSource BombSound;

    /// <summary>
    /// AudioSource that contains the background music
    /// </summary>
    public AudioSource BackGroundMusic;

    /// <summary>
    /// AudioSource that contains the heart collected sound when caught
    /// </summary>
    public AudioSource HeartSound;

    /// <summary>
    /// AudioSource that contains the countdown sound when the game is about to start
    /// </summary>
    public AudioSource CountdownSound;

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

    /// <summary>
    /// Method that is called to play the heart collected sound when caught
    /// </summary>
    public void PlayHeartSound()
    {
        HeartSound.Play();
    }

    /// <summary>
    /// Method that is called to play the countdown sound when the game is about to start
    /// </summary>
    public void PlayCountdownSound()
    {
        CountdownSound.Play();
    }
}
