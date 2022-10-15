using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public AudioSource ButtonClickSound;
	
    public void PlayButtonClickSound()
    {
        ButtonClickSound.Play();
    }
}
