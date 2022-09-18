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

// Class for handling key combinations.
[System.Serializable]
public class KeyCombination
{
    // The key combination's modifiers (e.g., Ctrl).
    [SerializeField]
    [Tooltip("The key combination's modifiers (e.g., Ctrl).")]
    EventModifiers m_Modifiers = EventModifiers.None;
    public EventModifiers Modifiers { get { return m_Modifiers; } set { m_Modifiers = value; } }

    // The key combination's key (e.g., K).
    [SerializeField]
    [Tooltip("The key combination's key (e.g., K).")]
    KeyCode m_Key = KeyCode.None;
    public KeyCode Key { get { return m_Key; } set { m_Key = value; } }

    // Whether the key combination is down.
    bool m_KeyDown = false;

    // Default constructor with the None key and None modifiers.
    public KeyCombination()
    {
        // Set the modifiers.
        m_Modifiers = EventModifiers.None;
        // Set the key.
        m_Key = KeyCode.None;
    }

    // Parameterized constructor with at least a key.
    public KeyCombination(KeyCode key, EventModifiers modifiers = EventModifiers.None)
    {
        // Set the modifiers.
        m_Modifiers = modifiers;
        // Set the key.
        m_Key = key;
    }

    // Returns whether the key combination is pressed down.
    public bool KeyCombinationDown()
    {
        // Get the current event.
        Event current = Event.current;

        // If there is an event.
        if (current != null)
        {
            // Check if the event is a key down.
            if (current.type == EventType.KeyDown)
            {
                // Set key down if the modifiers and key are pressed.
                if (current.modifiers == m_Modifiers && current.keyCode == m_Key)
                {
                    m_KeyDown = true;
                }
            }
            // Check if the event is a key up.
            else if (current.type == EventType.KeyUp)
            {
                // Reset key down if the key is released.
                if (current.keyCode == m_Key)
                {
                    m_KeyDown = false;
                }
            }
        }

        // Return whether the key is down or not.
        return m_KeyDown;
    }

    // Returns whether the key combination has changed.
    public bool KeyCombinationClick()
    {
        // Get the current event.
        Event current = Event.current;

        // Get whether the key was clicked.
        bool click = false;

        // If there is an event.
        if (current != null)
        {
            // Check if the event is a key up.
            if (current.type == EventType.KeyUp)
            {
                // Click and reset key down if the modifiers and key are released.
                if (current.modifiers == m_Modifiers && current.keyCode == m_Key)
                {
                    click = true;
                    m_KeyDown = false;
                }
            }
        }

        // Return whether the key was clicked.
        return click;
    }
}

// Class for setting up shortcuts for XRST interactions.
[AddComponentMenu("XRST/XRSTShortcuts")]
[DisallowMultipleComponent]
public class XRSTShortcuts : MonoBehaviour
{
    // Header to seperate components.
    [Header("XRST Components")]

    // The XRST Rig.
    [SerializeField]
    [Tooltip("The XRST Rig.")]
    XRSTRig m_Rig = null;
    public XRSTRig Rig { get { return m_Rig; } set { m_Rig = value; } }

    // The left hand XRST Controller.
    [SerializeField]
    [Tooltip("The left hand XRST Controller.")]
    XRSTController m_LeftHandController = null;
    public XRSTController LeftHandController { get { return m_LeftHandController; } set { m_LeftHandController = value; } }

    // The right hand XRST Controller.
    [SerializeField]
    [Tooltip("The right hand XRST Controller.")]
    XRSTController m_RightHandController = null;
    public XRSTController RightHandController { get { return m_RightHandController; } set { m_RightHandController = value; } }

    // Header to seperate user shortcuts.
    [Header("User Shortcuts")]

    // Moves the user forward.
    [SerializeField]
    [Tooltip("Moves the user forward. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_MoveForward = new KeyCombination(KeyCode.W);
    public KeyCombination MoveForward { get { return m_MoveForward; } set { m_MoveForward = value; } }

    // Moves the user backward.
    [SerializeField]
    [Tooltip("Moves the user bacward. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_MoveBackward = new KeyCombination(KeyCode.S);
    public KeyCombination MoveBackward { get { return m_MoveBackward; } set { m_MoveBackward = value; } }

    // Moves the user left.
    [SerializeField]
    [Tooltip("Moves the user left. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_MoveLeft = new KeyCombination(KeyCode.A);
    public KeyCombination MoveLeft { get { return m_MoveLeft; } set { m_MoveLeft = value; } }

    // Moves the user right.
    [SerializeField]
    [Tooltip("Moves the user right. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_MoveRight = new KeyCombination(KeyCode.D);
    public KeyCombination MoveRight { get { return m_MoveRight; } set { m_MoveRight = value; } }

    // Rotates the user left.
    [SerializeField]
    [Tooltip("Rotates the user left. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_RotateLeft = new KeyCombination(KeyCode.Q);
    public KeyCombination RotateLeft { get { return m_RotateLeft; } set { m_RotateLeft = value; } }

    // Rotates the user right.
    [SerializeField]
    [Tooltip("Rotates the user right. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_RotateRight = new KeyCombination(KeyCode.E);
    public KeyCombination RotateRight { get { return m_RotateRight; } set { m_RotateRight = value; } }

    // Rotates the user's head up.
    [SerializeField]
    [Tooltip("Rotates the user's head up. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_LookUp = new KeyCombination(KeyCode.X);
    public KeyCombination LookUp { get { return m_LookUp; } set { m_LookUp = value; } }

    // Rotates the user's head down.
    [SerializeField]
    [Tooltip("Rotates the user's head down. (Not all modifier keys work with this shortcut.)")]
    KeyCombination m_LookDown = new KeyCombination(KeyCode.C);
    public KeyCombination LookDown { get { return m_LookDown; } set { m_LookDown = value; } }

    // Header to seperate lefthand controller shortcuts.
    [Header("LeftHand Controller Shortcuts")]

    // Presses up on the primary 2D axis for the lefthand controller.
    [SerializeField]
    [Tooltip("Presses up on the primary 2D axis for the lefthand controller.")]
    KeyCombination m_LeftAxisUp = new KeyCombination(KeyCode.I, EventModifiers.Control);
    public KeyCombination LeftAxisUp { get { return m_LeftAxisUp; } set { m_LeftAxisUp = value; } }

    // Presses down on the primary 2D axis for the lefthand controller.
    [SerializeField]
    [Tooltip("Presses down on the primary 2D axis for the lefthand controller.")]
    KeyCombination m_LeftAxisDown = new KeyCombination(KeyCode.K, EventModifiers.Control);
    public KeyCombination LeftAxisDown { get { return m_LeftAxisDown; } set { m_LeftAxisDown = value; } }

    // Presses left on the primary 2D axis for the lefthand controller.
    [SerializeField]
    [Tooltip("Presses left on the primary 2D axis for the lefthand controller.")]
    KeyCombination m_LeftAxisLeft = new KeyCombination(KeyCode.J, EventModifiers.Control);
    public KeyCombination LeftAxisLeft { get { return m_LeftAxisLeft; } set { m_LeftAxisLeft = value; } }

    // Presses right on the primary 2D axis for the lefthand controller.
    [SerializeField]
    [Tooltip("Presses right on the primary 2D axis for the lefthand controller.")]
    KeyCombination m_LeftAxisRight = new KeyCombination(KeyCode.L, EventModifiers.Control);
    public KeyCombination LeftAxisRight { get { return m_LeftAxisRight; } set { m_LeftAxisRight = value; } }

    // Touches or untouhces the primary 2D axis for the lefthand controller.
    [SerializeField]
    [Tooltip("Touches or untouhces the primary 2D axis for the lefthand controller.")]
    KeyCombination m_LeftAxisTouch = new KeyCombination(KeyCode.O, EventModifiers.Control);
    public KeyCombination LeftAxisTouch { get { return m_LeftAxisTouch; } set { m_LeftAxisTouch = value; } }

    // Clicks or unclicks the primary 2D axis for the lefthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the primary 2D axis for the lefthand controller.")]
    KeyCombination m_LeftAxisClick = new KeyCombination(KeyCode.U, EventModifiers.Control);
    public KeyCombination LeftAxisClick { get { return m_LeftAxisClick; } set { m_LeftAxisClick = value; } }

    // Clicks or unclicks the trigger button for the lefthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the trigger button for the lefthand controller.")]
    KeyCombination m_LeftTriggerButton = new KeyCombination(KeyCode.M, EventModifiers.Control);
    public KeyCombination LeftTriggerButton { get { return m_LeftTriggerButton; } set { m_LeftTriggerButton = value; } }

    // Clicks or unclicks the grip button for the lefthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the grip button for the lefthand controller.")]
    KeyCombination m_LeftGripButton = new KeyCombination(KeyCode.Comma, EventModifiers.Control);
    public KeyCombination LeftGripButton { get { return m_LeftGripButton; } set { m_LeftGripButton = value; } }

    // Clicks or unclicks the primary button for the lefthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the primary button for the lefthand controller.")]
    KeyCombination m_LeftPrimaryButton = new KeyCombination(KeyCode.Period, EventModifiers.Control);
    public KeyCombination LeftPrimaryButton { get { return m_LeftPrimaryButton; } set { m_LeftPrimaryButton = value; } }

    // Header to seperate righthand controller shortcuts.
    [Header("RightHand Controller Shortcuts")]

    // Presses up on the primary 2D axis for the righthand controller.
    [SerializeField]
    [Tooltip("Presses up on the primary 2D axis for the righthand controller.")]
    KeyCombination m_RightAxisUp = new KeyCombination(KeyCode.I, EventModifiers.None);
    public KeyCombination RightAxisUp { get { return m_RightAxisUp; } set { m_RightAxisUp = value; } }

    // Presses down on the primary 2D axis for the righthand controller.
    [SerializeField]
    [Tooltip("Presses down on the primary 2D axis for the righthand controller.")]
    KeyCombination m_RightAxisDown = new KeyCombination(KeyCode.K, EventModifiers.None);
    public KeyCombination RightAxisDown { get { return m_RightAxisDown; } set { m_RightAxisDown = value; } }

    // Presses left on the primary 2D axis for the righthand controller.
    [SerializeField]
    [Tooltip("Presses left on the primary 2D axis for the righthand controller.")]
    KeyCombination m_RightAxisLeft = new KeyCombination(KeyCode.J, EventModifiers.None);
    public KeyCombination RightAxisLeft { get { return m_RightAxisLeft; } set { m_RightAxisLeft = value; } }

    // Presses right on the primary 2D axis for the righthand controller.
    [SerializeField]
    [Tooltip("Presses right on the primary 2D axis for the righthand controller.")]
    KeyCombination m_RightAxisRight = new KeyCombination(KeyCode.L, EventModifiers.None);
    public KeyCombination RightAxisRight { get { return m_RightAxisRight; } set { m_RightAxisRight = value; } }

    // Touches or untouhces the primary 2D axis for the righthand controller.
    [SerializeField]
    [Tooltip("Touches or untouhces the primary 2D axis for the righthand controller.")]
    KeyCombination m_RightAxisTouch = new KeyCombination(KeyCode.O, EventModifiers.None);
    public KeyCombination RightAxisTouch { get { return m_RightAxisTouch; } set { m_RightAxisTouch = value; } }

    // Clicks or unclicks the primary 2D axis for the righthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the primary 2D axis for the righthand controller.")]
    KeyCombination m_RightAxisClick = new KeyCombination(KeyCode.U, EventModifiers.None);
    public KeyCombination RightAxisClick { get { return m_RightAxisClick; } set { m_RightAxisClick = value; } }

    // Clicks or unclicks the trigger button for the righthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the trigger button for the righthand controller.")]
    KeyCombination m_RightTriggerButton = new KeyCombination(KeyCode.M, EventModifiers.None);
    public KeyCombination RightTriggerButton { get { return m_RightTriggerButton; } set { m_RightTriggerButton = value; } }

    // Clicks or unclicks the grip button for the righthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the grip button for the righthand controller.")]
    KeyCombination m_RightGripButton = new KeyCombination(KeyCode.Comma, EventModifiers.None);
    public KeyCombination RightGripButton { get { return m_RightGripButton; } set { m_RightGripButton = value; } }

    // Clicks or unclicks the primary button for the righthand controller.
    [SerializeField]
    [Tooltip("Clicks or unclicks the primary button for the righthand controller.")]
    KeyCombination m_RightPrimaryButton = new KeyCombination(KeyCode.Period, EventModifiers.None);
    public KeyCombination RightPrimaryButton { get { return m_RightPrimaryButton; } set { m_RightPrimaryButton = value; } }

    // Reset function for initializing the rig and controllers.
    void Reset()
    {
        // Attempt to find the XR Rig GameObject.
        if(GameObject.Find("XR Rig") != null)
        {
            // Attempt to fetch the XRSTRig.
            m_Rig = GameObject.Find("XR Rig").GetComponent<XRSTRig>();
            // Did not find the XRSTRig component.
            if (m_Rig == null)
            {
                Debug.LogWarning("[" + gameObject.name + "][XRSTShortcuts]: Did not find an XRSTRig component attached to the 'XR Rig' GameObject.");
            }
        }
        // Did not find the XR Rig GameObject.
        else
        {
            Debug.LogWarning("[" + gameObject.name + "][XRSTShortcuts]: Did not find an 'XR Rig' GameObject in the scene.");
        }

        // Attempt to find the LeftHand Controller GameObject.
        if (GameObject.Find("LeftHand Controller") != null)
        {
            // Attempt to fetch the XRSTController.
            m_LeftHandController = GameObject.Find("LeftHand Controller").GetComponent<XRSTController>();
            // Did not find the XRSTController component.
            if (m_LeftHandController == null)
            {
                Debug.LogWarning("[" + gameObject.name + "][XRSTShortcuts]: Did not find an XRSTController component attached to the 'LeftHand Controller' GameObject.");
            }
        }
        // Did not find the LeftHand Controller GameObject.
        else
        {
            Debug.LogWarning("[" + gameObject.name + "][XRSTShortcuts]: Did not find an 'LeftHand Controller' GameObject in the scene.");
        }

        // Attempt to find the RightHand Controller GameObject.
        if (GameObject.Find("RightHand Controller") != null)
        {
            // Attempt to fetch the XRSTController.
            m_RightHandController = GameObject.Find("RightHand Controller").GetComponent<XRSTController>();
            // Did not find the XRSTController component.
            if (m_RightHandController == null)
            {
                Debug.LogWarning("[" + gameObject.name + "][XRSTShortcuts]: Did not find an XRSTController component attached to the 'RightHand Controller' GameObject.");
            }
        }
        // Did not find the RightHand Controller GameObject.
        else
        {
            Debug.LogWarning("[" + gameObject.name + "][XRSTShortcuts]: Did not find an 'RightHand Controller' GameObject in the scene.");
        }
    }
}
