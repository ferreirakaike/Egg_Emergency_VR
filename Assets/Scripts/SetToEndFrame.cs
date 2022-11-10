using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that sets an animator to the very last frame of the animation
/// </summary>
public class SetToEndFrame : MonoBehaviour
{
    /// <summary>
    /// Animator to be set to the last frame
    /// </summary>
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (animator)
        {
            animator.SetFloat("Grip", 1);
        }
    }

}
