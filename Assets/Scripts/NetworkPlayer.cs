using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

/// <summary>
/// The class that controls the movement and animation of the networked player
/// </summary>
public class NetworkPlayer : MonoBehaviour
{
    /// <summary>
    /// Variable that holds the refernce to the user's left hand prefab
    /// </summary>
    public Transform leftHand;

    private Animator leftHandAnimator;

    /// <summary>
    /// Variable that holds the reference to the user's right hand prefab
    /// </summary>
    public Transform rightHand;

    private Animator rightHandAnimator;
    private PhotonView photonView;
    private Transform leftHandOrigin;
    private Animator leftHandOriginAnimator;
    private Transform rightHandOrigin;
    private Animator rightHandOriginAnimator;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        XROrigin origin = FindObjectOfType<XROrigin>();
        leftHandOrigin = origin.transform.Find("Camera Offset/LeftHand Controller");
        leftHandOriginAnimator = leftHandOrigin.GetComponentInChildren<Animator>();
        leftHandAnimator = leftHand.GetComponentInChildren<Animator>();
        rightHandOrigin = origin.transform.Find("Camera Offset/RightHand Controller");
        rightHandOriginAnimator = rightHandOrigin.GetComponentInChildren<Animator>();
        rightHandAnimator = rightHand.GetComponentInChildren<Animator>();

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(leftHand, leftHandOrigin);
            MapPosition(rightHand, rightHandOrigin);
            leftHandAnimator.SetFloat("Grip", leftHandOriginAnimator.GetFloat("Grip"));
            rightHandAnimator.SetFloat("Grip", rightHandOriginAnimator.GetFloat("Grip"));
            leftHandAnimator.SetFloat("Trigger", leftHandOriginAnimator.GetFloat("Trigger"));
            rightHandAnimator.SetFloat("Trigger", rightHandOriginAnimator.GetFloat("Trigger"));
        }
    }

    void MapPosition(Transform target, Transform originTransform)
    {
        target.position = originTransform.position;
        target.rotation = originTransform.rotation;
    }
}
