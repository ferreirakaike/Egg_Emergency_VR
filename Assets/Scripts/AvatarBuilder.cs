using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AvatarBuilder : MonoBehaviour
{
    private Animator animator;
    public void Awake() 
    {
        animator = GetComponent<Animator>();
        // Avatar avatar = AvatarBuilder.BuildGenericAvatar(gameObject, "");
        // avatar.name = "Shadow Basket";
        // animator.avatar = avatar;
    }
}
