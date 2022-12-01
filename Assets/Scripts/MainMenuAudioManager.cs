using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class specifically handles the sound for the main menu scene.
/// </summary>
public class MainMenuAudioManager : MonoBehaviour
{
    /// <summary>
    /// AudioSource that contains the sound when the button is clicked
    /// </summary>
    public AudioSource ButtonClickSound;

    /// <summary>
    /// AudioSource that contains the sound when that plays when the game is over
    /// </summary>
	public AudioSource GameOverSound;
	
    /// <summary>
    /// This method plays sound when the button is clicked.
    /// </summary>
    public void PlayButtonClickSound()
    {
        ButtonClickSound.Play();
    }
	
    /// <summary>
    /// This method plays sound when game is over.
    /// </summary>
    public void PlayGameOverSound()
    {
        GameOverSound.Play();
    }
}
