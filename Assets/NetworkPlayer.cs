using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class NetworkPlayer : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    private PhotonView photonView;
    private Transform leftHandOrigin;
    private Transform rightHandOrigin;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        XROrigin origin = FindObjectOfType<XROrigin>();
        leftHandOrigin = origin.transform.Find("Camera Offset/LeftHand Controller");
        rightHandOrigin = origin.transform.Find("Camera Offset/RightHand Controller");

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
        }
    }

    void MapPosition(Transform target, Transform originTransform)
    {
        target.position = originTransform.position;
        target.rotation = originTransform.rotation;
    }
}
