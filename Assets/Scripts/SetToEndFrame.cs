using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToEndFrame : MonoBehaviour
{
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
