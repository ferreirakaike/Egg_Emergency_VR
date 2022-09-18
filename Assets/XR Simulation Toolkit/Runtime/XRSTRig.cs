/*
*   Copyright (C) 2020 University of Central Florida, created by Dr. Ryan P. McMahan.
*
*   This program is free software: you can redistribute it and/or modify
*   it under the terms of the GNU General Public License as published by
*   the Free Software Foundation, either version 3 of the License, or
*   (at your option) any later version.
*
*   This program is distributed in the hope that it will be useful,
*   but WITHOUT ANY WARRANTY; without even the implied warranty of
*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*   GNU General Public License for more details.
*
*   You should have received a copy of the GNU General Public License
*   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*   Primary Author Contact:  Dr. Ryan P. McMahan <rpm@ucf.edu>
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit;

// A component for simulating an XR rig based on simulated input values.
// Must be attached to the same GameObject that represents the XR rig (e.g., XR Rig).
[DefaultExecutionOrder(XRInteractionUpdateOrder.k_Controllers)]
[AddComponentMenu("XRST/XRSTRig")]
[RequireComponent(typeof(XRRig))]
[DisallowMultipleComponent]
public class XRSTRig : MonoBehaviour
{
    // The GameObject that represents the Camera Offset or user's torso.
    [SerializeField]
    [Tooltip("The GameObject that represents the Camera Offset or user's torso.")]
    GameObject m_CameraOffset;
    public GameObject CameraOffset { get { return m_CameraOffset;  } set { m_CameraOffset = value; } }

    // The TrackedPoseDriver that represents the Main Camera or user's head.
    [SerializeField]
    [Tooltip("The TrackedPoseDriver that represents the Main Camera or user's head.")]
    TrackedPoseDriver m_MainCamera;
    public TrackedPoseDriver MainCamera { get { return m_MainCamera; } set { m_MainCamera = value; } }

    // The XRController that represents the LeftHand Controller or user's left hand.
    [SerializeField]
    [Tooltip("The XRController that represents the LeftHand Controller or user's left hand.")]
    XRController m_LeftHandController;
    public XRController LeftHandController { get { return m_LeftHandController; } set { m_LeftHandController = value; } }

    // The XRController that represents the RightHand Controller or user's right hand.
    [SerializeField]
    [Tooltip("The XRController that represents the RightHand Controller or user's right hand.")]
    XRController m_RightHandController;
    public XRController RightHandController { get { return m_RightHandController; } set { m_RightHandController = value; } }

    // The simulated height of the user.
    [SerializeField]
    [Tooltip("The simulated height of the user.")]
    float m_UserHeight = 1.7f;
    public float UserHeight { get { return m_UserHeight; } set { m_UserHeight = value; } }

    // The simulated walking speed of the user in meters per second.
    [SerializeField]
    [Tooltip("The simulated walking speed of the user in meters per second.")]
    float m_MoveSpeed = 1.45f;
    public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

    // The simulated rotation speed of the user in degrees per second.
    [SerializeField]
    [Tooltip("The simulated rotation speed of the user in degrees per second.")]
    float m_RotateSpeed = 30.0f;
    public float RotateSpeed { get { return m_RotateSpeed; } set { m_RotateSpeed = value; } }

    // The simulated tracking space radius in meters.
    [SerializeField]
    [Tooltip("The simulated tracking space radius in meters.")]
    float m_TrackingRadius = 2.0f;
    public float TrackingRadius { get { return m_TrackingRadius; } set { m_TrackingRadius = value; } }

    // Whether moving forward.
    [SerializeField]
    [HideInInspector]
    bool m_MovingForward;
    public bool MovingForward { get { return m_MovingForward; } set { m_MovingForward = value; } }

    // Whether moving backward.
    [SerializeField]
    [HideInInspector]
    bool m_MovingBackward;
    public bool MovingBackward { get { return m_MovingBackward; } set { m_MovingBackward = value; } }

    // Whether moving left.
    [SerializeField]
    [HideInInspector]
    bool m_MovingLeft;
    public bool MovingLeft { get { return m_MovingLeft; } set { m_MovingLeft = value; } }

    // Whether moving right.
    [SerializeField]
    [HideInInspector]
    bool m_MovingRight;
    public bool MovingRight { get { return m_MovingRight; } set { m_MovingRight = value; } }

    // Whether rotating left.
    [SerializeField]
    [HideInInspector]
    bool m_RotatingLeft;
    public bool RotatingLeft { get { return m_RotatingLeft; } set { m_RotatingLeft = value; } }

    // Whether rotating right.
    [SerializeField]
    [HideInInspector]
    bool m_RotatingRight;
    public bool RotatingRight { get { return m_RotatingRight; } set { m_RotatingRight = value; } }

    // Whether looking up.
    [SerializeField]
    [HideInInspector]
    bool m_LookingUp;
    public bool LookingUp { get { return m_LookingUp; } set { m_LookingUp = value; } }

    // Whether looking down.
    [SerializeField]
    [HideInInspector]
    bool m_LookingDown;
    public bool LookingDown { get { return m_LookingDown; } set { m_LookingDown = value; } }

    // Property to quickly determine if the rig is validly configured.
    bool IsValid
    {
        get
        {
            return (CameraOffset != null && MainCamera != null && LeftHandController != null && RightHandController != null);
        }
    }

    // Reset function for initializing the rig.
    void Reset()
    {
        // Attempt to find the Camera Offset GameObject.
        if (GameObject.Find("Camera Offset") != null)
        {
            // Attempt to fetch the XRSTRig.
            CameraOffset = GameObject.Find("Camera Offset");
        }
        // Did not find the Camera Offset GameObject.
        else
        {
            Debug.LogWarning("[" + gameObject.name + "][XRSTRig]: Did not find a 'Camera Offset' GameObject in the scene.");
        }

        // Attempt to find the Main Camera GameObject.
        if (Camera.main != null)
        {
            // Attempt to fetch the TrackedPoseDriver.
            MainCamera = Camera.main.gameObject.GetComponent<TrackedPoseDriver>();
            // Did not find the TrackedPoseDriver component.
            if (MainCamera == null)
            {
                Debug.LogWarning("[" + gameObject.name + "][XRSTRig]: Did not find a TrackedPoseDriver component attached to the main Camera GameObject.");
            }
        }
        // Did not find the main Camera GameObject.
        else
        {
            Debug.LogWarning("[" + gameObject.name + "][XRSTRig]: Did not find a main Camera in the scene.");
        }

        // Attempt to find the LeftHand Controller GameObject.
        if (GameObject.Find("LeftHand Controller") != null)
        {
            // Attempt to fetch the XRController.
            LeftHandController = GameObject.Find("LeftHand Controller").GetComponent<XRController>();
            // Did not find the TrackedPoseDriver component.
            if (LeftHandController == null)
            {
                Debug.LogWarning("[" + gameObject.name + "][XRSTRig]: Did not find an XRController component attached to the 'LeftHand Controller' GameObject.");
            }
        }
        // Did not find the Main Camera GameObject.
        else
        {
            Debug.LogWarning("[" + gameObject.name + "][XRSTRig]: Did not find a 'LeftHand Controller' GameObject in the scene.");
        }

        // Attempt to find the RightHand Controller GameObject.
        if (GameObject.Find("RightHand Controller") != null)
        {
            // Attempt to fetch the XRController.
            RightHandController = GameObject.Find("RightHand Controller").GetComponent<XRController>();
            // Did not find the TrackedPoseDriver component.
            if (RightHandController == null)
            {
                Debug.LogWarning("[" + gameObject.name + "][XRSTRig]: Did not find an XRController component attached to the 'RightHand Controller' GameObject.");
            }
        }
        // Did not find the Main Camera GameObject.
        else
        {
            Debug.LogWarning("[" + gameObject.name + "][XRSTRig]: Did not find a 'RightHand Controller' GameObject in the scene.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsValid)
        {
            // Reset the torso's local rotation.
            CameraOffset.transform.localRotation = Quaternion.identity;
            // Set the torso's local position.
            CameraOffset.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            // Disable the tracked pose.
            MainCamera.enabled = false;
            // Reset the head's local rotation.
            MainCamera.transform.localRotation = Quaternion.identity;
            // Set the head's local position and height.
            MainCamera.transform.localPosition = new Vector3(0.0f, UserHeight, 0.0f);

            // Reset the left hand's local rotation.
            LeftHandController.transform.localRotation = Quaternion.identity;
            // Set the left hand's local position and height.
            LeftHandController.transform.localPosition = new Vector3(-UserHeight / 3.75f, UserHeight / 2.0f, UserHeight / 7.5f);

            // Reset the right hand's local rotation.
            RightHandController.transform.localRotation = Quaternion.identity;
            // Set the right hand's local position and height.
            RightHandController.transform.localPosition = new Vector3(UserHeight / 3.75f, UserHeight / 2.0f, UserHeight / 7.5f);
        }

        // Cancel any preexisting movements.
        MovingForward = false;
        MovingBackward = false;
        MovingLeft = false;
        MovingRight = false;
        RotatingLeft = false;
        RotatingRight = false;
        LookingUp = false;
        LookingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsValid)
        {
            // Process moving forward.
            if (MovingForward)
            {
                // Move the camera offset forward.
                CameraOffset.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
            }

            // Process moving backward.
            if (MovingBackward)
            {
                // Move the camera offset backward.
                CameraOffset.transform.Translate(-Vector3.forward * MoveSpeed * Time.deltaTime);
            }

            // Process moving left.
            if (MovingLeft)
            {
                // Move the camera offset left.
                CameraOffset.transform.Translate(-Vector3.right * MoveSpeed * Time.deltaTime);
            }

            // Process moving right.
            if (MovingRight)
            {
                // Move the camera offset right.
                CameraOffset.transform.Translate(Vector3.right * MoveSpeed * Time.deltaTime);
            }

            // Process rotating left.
            if (RotatingLeft)
            {
                // Rotate the camera offset left.
                CameraOffset.transform.Rotate(Vector3.up, -RotateSpeed * Time.deltaTime);
            }

            // Process rotating right.
            if (RotatingRight)
            {
                // Rotate the camera offset right.
                CameraOffset.transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
            }

            // Process looking up.
            if (LookingUp)
            {
                // Fetch the current euler angles.
                Vector3 eulerAngles = MainCamera.transform.localRotation.eulerAngles;
                // Remap the angles from [0.0f, 360.0f] to [-180.0f, 180.0f].
                eulerAngles.x = (eulerAngles.x + 180.0f) % 360.0f - 180.0f;
                // Rotate the camera up.
                eulerAngles.x -= RotateSpeed * Time.deltaTime;
                // Clamp the rotation.
                eulerAngles.x = Mathf.Clamp(eulerAngles.x, -89.0f, 89.0f);
                // Apply the rotation back.
                MainCamera.transform.localRotation = Quaternion.Euler(eulerAngles);
            }

            // Process looking down.
            if (LookingDown)
            {
                // Fetch the current euler angles.
                Vector3 eulerAngles = MainCamera.transform.localRotation.eulerAngles;
                // Remap the angles from [0.0f, 360.0f] to [-180.0f, 180.0f].
                eulerAngles.x = (eulerAngles.x + 180.0f) % 360.0f - 180.0f;
                // Rotate the camera up.
                eulerAngles.x += RotateSpeed * Time.deltaTime;
                // Clamp the rotation.
                eulerAngles.x = Mathf.Clamp(eulerAngles.x, -89.0f, 89.0f);
                // Apply the rotation back.
                MainCamera.transform.localRotation = Quaternion.Euler(eulerAngles);
            }

            // Disable/Enable the HMD based on the tracking space.
            MainCamera.gameObject.SetActive(Vector3.Distance(transform.position + new Vector3(0.0f, UserHeight, 0.0f), MainCamera.transform.position) < TrackingRadius);

            // Disable/Enable the left controller based on the tracking space.
            LeftHandController.gameObject.SetActive(Vector3.Distance(transform.position + new Vector3(0.0f, UserHeight, 0.0f), LeftHandController.transform.position) < TrackingRadius);

            // Disable/Enable the right controller based on the tracking space.
            RightHandController.gameObject.SetActive(Vector3.Distance(transform.position + new Vector3(0.0f, UserHeight, 0.0f), RightHandController.transform.position) < TrackingRadius);
        }
    }
}
