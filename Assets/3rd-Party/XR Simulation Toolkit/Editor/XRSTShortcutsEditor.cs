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
using UnityEditor;

// Static class for handling shortcuts in the Unity Editor for the XRST package.
[InitializeOnLoad]
public static class HandleXRSTShortcuts
{
    // Attempt to find XRSTShortcuts.
    static XRSTShortcuts Shortcuts { get { return Object.FindObjectOfType<XRSTShortcuts>(); } }

    // Attempt to find the XRSTRig set by XRSTShortcuts.
    static XRSTRig Rig
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.Rig;
            return null;
        }
    }

    // Attempt to find the left hand XRSTController set by XRSTShortcuts.
    static XRSTController LeftHandController
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftHandController;
            return null;
        }
    }

    // Attempt to find the right hand XRSTController set by XRSTShortcuts.
    static XRSTController RightHandController
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightHandController;
            return null;
        }
    }

    // Attempt to find the MoveForward shortcut.
    static KeyCombination MoveForward
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.MoveForward;
            return new KeyCombination();
        }
    }

    // Attempt to find the MoveBackward shortcut.
    static KeyCombination MoveBackward
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.MoveBackward;
            return new KeyCombination();
        }
    }

    // Attempt to find the MoveLeft shortcut.
    static KeyCombination MoveLeft
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.MoveLeft;
            return new KeyCombination();
        }
    }

    // Attempt to find the MoveRight shortcut.
    static KeyCombination MoveRight
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.MoveRight;
            return new KeyCombination();
        }
    }

    // Attempt to find the RotateLeft shortcut.
    static KeyCombination RotateLeft
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RotateLeft;
            return new KeyCombination();
        }
    }

    // Attempt to find the RotateRight shortcut.
    static KeyCombination RotateRight
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RotateRight;
            return new KeyCombination();
        }
    }

    // Attempt to find the LookUp shortcut.
    static KeyCombination LookUp
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LookUp;
            return new KeyCombination();
        }
    }

    // Attempt to find the LookDown shortcut.
    static KeyCombination LookDown
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LookDown;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftAxisUp shortcut.
    static KeyCombination LeftAxisUp
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftAxisUp;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftAxisDown shortcut.
    static KeyCombination LeftAxisDown
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftAxisDown;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftAxisLeft shortcut.
    static KeyCombination LeftAxisLeft
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftAxisLeft;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftAxisRight shortcut.
    static KeyCombination LeftAxisRight
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftAxisRight;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftAxisTouch shortcut.
    static KeyCombination LeftAxisTouch
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftAxisTouch;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftAxisClick shortcut.
    static KeyCombination LeftAxisClick
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftAxisClick;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftTriggerButton shortcut.
    static KeyCombination LeftTriggerButton
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftTriggerButton;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftGripButton shortcut.
    static KeyCombination LeftGripButton
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftGripButton;
            return new KeyCombination();
        }
    }

    // Attempt to find the LeftPrimaryButton shortcut.
    static KeyCombination LeftPrimaryButton
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.LeftPrimaryButton;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightAxisUp shortcut.
    static KeyCombination RightAxisUp
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightAxisUp;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightAxisDown shortcut.
    static KeyCombination RightAxisDown
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightAxisDown;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightAxisLeft shortcut.
    static KeyCombination RightAxisLeft
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightAxisLeft;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightAxisRight shortcut.
    static KeyCombination RightAxisRight
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightAxisRight;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightAxisTouch shortcut.
    static KeyCombination RightAxisTouch
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightAxisTouch;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightAxisClick shortcut.
    static KeyCombination RightAxisClick
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightAxisClick;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightTriggerButton shortcut.
    static KeyCombination RightTriggerButton
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightTriggerButton;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightGripButton shortcut.
    static KeyCombination RightGripButton
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightGripButton;
            return new KeyCombination();
        }
    }

    // Attempt to find the RightPrimaryButton shortcut.
    static KeyCombination RightPrimaryButton
    {
        get
        {
            if (Shortcuts != null) return Shortcuts.RightPrimaryButton;
            return new KeyCombination();
        }
    }

    // Method that uses reflection to add a listener to Unity Editor's globalEventHandler.
    [InitializeOnLoadMethod]
    static void EditorInit()
    {
        // Get reflection field info about the globalEventHandler.
        System.Reflection.FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        // Get the actual value of the globalEventHandler.
        EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction)info.GetValue(null);
        // Add our custom global key press listener to the globalEventHandler.
        value += GlobalEventHandler;
        // Set the actual value of the globalEventHandler back.
        info.SetValue(null, value);
    }

    // Method for handling global events in the Unity Editor.
    static void GlobalEventHandler()
    {
        // Reduce computational load by only considering key events.
        if (Event.current.type != EventType.KeyUp && Event.current.type != EventType.KeyDown)
            return;

        // If a reference to the rig exists.
        if (Rig != null)
        {
            // Set Rig movements based on key combinations.
            Rig.MovingForward = MoveForward.KeyCombinationDown();
            Rig.MovingBackward = MoveBackward.KeyCombinationDown();
            Rig.MovingLeft = MoveLeft.KeyCombinationDown();
            Rig.MovingRight = MoveRight.KeyCombinationDown();
            Rig.RotatingLeft = RotateLeft.KeyCombinationDown();
            Rig.RotatingRight = RotateRight.KeyCombinationDown();
            Rig.LookingUp = LookUp.KeyCombinationDown();
            Rig.LookingDown = LookDown.KeyCombinationDown();
        }

        // If a reference to the lefthand controller exists.
        if (LeftHandController != null)
        {
            // Presses up on the primary 2D axis for the lefthand controller.
            if (LeftAxisUp.KeyCombinationClick())
            {
                LeftHandController.Primary2DAxis.TouchAxisUp();
            }

            // Presses down on the primary 2D axis for the lefthand controller.
            if (LeftAxisDown.KeyCombinationClick())
            {
                LeftHandController.Primary2DAxis.TouchAxisDown();
            }

            // Presses left on the primary 2D axis for the lefthand controller.
            if (LeftAxisLeft.KeyCombinationClick())
            {
                LeftHandController.Primary2DAxis.TouchAxisLeft();
            }

            // Presses right on the primary 2D axis for the lefthand controller.
            if (LeftAxisRight.KeyCombinationClick())
            {
                LeftHandController.Primary2DAxis.TouchAxisRight();
            }

            // Touches or untouhces the primary 2D axis for the lefthand controller.
            if (LeftAxisTouch.KeyCombinationClick())
            {
                LeftHandController.Primary2DAxis.Touch = !LeftHandController.Primary2DAxis.Touch;
            }

            // Clicks or unclicks the primary 2D axis for the lefthand controller.
            if (LeftAxisClick.KeyCombinationClick())
            {
                LeftHandController.Primary2DAxis.Click = !LeftHandController.Primary2DAxis.Click;
            }

            // Clicks or unclicks the trigger button for the lefthand controller.
            if (LeftTriggerButton.KeyCombinationClick())
            {
                LeftHandController.Trigger.Button = !LeftHandController.Trigger.Button;
            }

            // Clicks or unclicks the grip button for the lefthand controller.
            if (LeftGripButton.KeyCombinationClick())
            {
                LeftHandController.Grip.Button = !LeftHandController.Grip.Button;
            }

            // Clicks or unclicks the primary button for the lefthand controller.
            if (LeftPrimaryButton.KeyCombinationClick())
            {
                LeftHandController.PrimaryButton.Button = !LeftHandController.PrimaryButton.Button;
            }
        }

        // If a reference to the righthand controller exists.
        if (RightHandController != null)
        {
            // Presses up on the primary 2D axis for the righthand controller.
            if (RightAxisUp.KeyCombinationClick())
            {
                RightHandController.Primary2DAxis.TouchAxisUp();
            }

            // Presses down on the primary 2D axis for the righthand controller.
            if (RightAxisDown.KeyCombinationClick())
            {
                RightHandController.Primary2DAxis.TouchAxisDown();
            }

            // Presses left on the primary 2D axis for the righthand controller.
            if (RightAxisLeft.KeyCombinationClick())
            {
                RightHandController.Primary2DAxis.TouchAxisLeft();
            }

            // Presses right on the primary 2D axis for the righthand controller.
            if (RightAxisRight.KeyCombinationClick())
            {
                RightHandController.Primary2DAxis.TouchAxisRight();
            }

            // Touches or untouhces the primary 2D axis for the righthand controller.
            if (RightAxisTouch.KeyCombinationClick())
            {
                RightHandController.Primary2DAxis.Touch = !RightHandController.Primary2DAxis.Touch;
            }

            // Clicks or unclicks the primary 2D axis for the righthand controller.
            if (RightAxisClick.KeyCombinationClick())
            {
                RightHandController.Primary2DAxis.Click = !RightHandController.Primary2DAxis.Click;
            }

            // Clicks or unclicks the trigger button for the righthand controller.
            if (RightTriggerButton.KeyCombinationClick())
            {
                RightHandController.Trigger.Button = !RightHandController.Trigger.Button;
            }

            // Clicks or unclicks the grip button for the righthand controller.
            if (RightGripButton.KeyCombinationClick())
            {
                RightHandController.Grip.Button = !RightHandController.Grip.Button;
            }

            // Clicks or unclicks the primary button for the righthand controller.
            if (RightPrimaryButton.KeyCombinationClick())
            {
                RightHandController.PrimaryButton.Button = !RightHandController.PrimaryButton.Button;
            }
        }
    }
}

// Class for drawing KeyCombination properties.
[CustomPropertyDrawer(typeof(KeyCombination))]
public class KeyCombinationDrawer : PropertyDrawer
{
    // Draw the KeyCombination inside the given rect.
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Begin drawing the KeyCombination.
        EditorGUI.BeginProperty(position, label, property);

        // Draw the label.
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Calculate the positions of the modifier and key.
        var modifierPosition = new Rect(position.x, position.y, (position.width / 2.0f) - 9.0f, position.height);
        var plusPosition = new Rect(position.x + (position.width / 2.0f) - 9.0f, position.y, 18.0f, position.height);
        var keyPosition = new Rect(position.x + (position.width / 2.0f) + 9.0f, position.y, (position.width / 2.0f) - 9.0f, position.height);

        // Draw the fields with labels (i.e., GUIContent.none).
        EditorGUI.PropertyField(modifierPosition, property.FindPropertyRelative("m_Modifiers"), GUIContent.none);
        GUIContent separator = new GUIContent(" + ");
        EditorGUI.PrefixLabel(plusPosition, GUIUtility.GetControlID(FocusType.Passive), separator);
        EditorGUI.PropertyField(keyPosition, property.FindPropertyRelative("m_Key"), GUIContent.none);

        // Finish drawing the KeyCombination.
        EditorGUI.EndProperty();
    }
}