using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public AudioSource ButtonClickSound;
	public AudioSource GameOverSound;
	
    public void PlayButtonClickSound()
    {
        ButtonClickSound.Play();
    }
	
    public void PlayGameOverSound()
    {
        GameOverSound.Play();
    }
}
