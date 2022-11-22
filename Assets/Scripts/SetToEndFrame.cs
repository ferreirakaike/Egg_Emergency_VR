using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Public animation parameter to see if the end frame should be set to "Trigger" or "Grip"
/// </summary>
public enum AnimationState {None, Grip, Trigger};

/// <summary>
/// Class that sets an animator to the very last frame of the animation
/// </summary>
public class SetToEndFrame : MonoBehaviour
{
    /// <summary>
    /// Animator to be set to the last frame
    /// </summary>
    public Animator animator;

    public AnimationState state = AnimationState.None;
    // Start is called before the first frame update
    void Start()
    {
        if (animator)
        {
            if (state == AnimationState.Grip)
            {
                animator.SetFloat("Grip", 1);
            }
            else if (state == AnimationState.Trigger)
            {
                animator.SetFloat("Trigger", 1);
            }
        }
    }

}
