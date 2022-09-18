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
using UnityEngine.XR.Interaction.Toolkit;
using XRST;

/// A component for simulating an XR controller based on simulated input values.
/// Must be attached to the same GameObject that represents the XR controller (e.g., LeftHand Controller, RightHand Controller).
[DefaultExecutionOrder(XRInteractionUpdateOrder.k_Controllers)]
[AddComponentMenu("XRST/XRSTController")]
[RequireComponent(typeof(XRController))]
[DisallowMultipleComponent]
public class XRSTController : MonoBehaviour
{
    // The 2D axis used to simulate the XR controller's Primary 2D Axis.
    [SerializeField]
    public Input2DAxis m_Primary2DAxis;
    public Input2DAxis Primary2DAxis { get { return m_Primary2DAxis; } set { m_Primary2DAxis = value; } }

    // The 1D axis used to simulate the XR controller's Trigger.
    [SerializeField]
    public Input1DAxis m_Trigger;
    public Input1DAxis Trigger { get { return m_Trigger; } set { m_Trigger = value; } }

    // The 1D axis used to simulate the XR controller's Grip.
    [SerializeField]
    public Input1DAxis m_Grip;
    public Input1DAxis Grip { get { return m_Grip; } set { m_Grip = value; } }

    // The button used to simulate the XR controller's Primary Button.
    [SerializeField]
    public InputButton m_PrimaryButton;
    public InputButton PrimaryButton { get { return m_PrimaryButton; } set { m_PrimaryButton = value; } }

    // The local controller.
    public XRController Controller { get { return GetComponent<XRController>(); } }

    // The local input device.
    public UnityEngine.XR.Interaction.Toolkit.InputDevice ControllerDevice
    {
        get
        {
            // Return the local input device if the controller exists.
            if (Controller != null)
            {
                return Controller.inputDevice;
            }
            // Otherwise, return a default input device.
            return new InputDevice();
        }
        set
        {
            // Set the local input device if the controller exists.
            if (Controller != null)
            {
                Controller.inputDevice = value;
            }
        }
    }

    // Reset function for initializing the controller.
    void Reset()
    {
        // Reset the primary 2D axis.
        m_Primary2DAxis = new Input2DAxis();
        m_Primary2DAxis.Name = "Primary 2D Axis";
        m_Primary2DAxis.TouchIncluded = true;
        m_Primary2DAxis.ClickIncluded = true;

        // Reset the trigger.
        m_Trigger = new Input1DAxis();
        m_Trigger.Name = "Trigger";
        m_Trigger.ButtonIncluded = true;

        // Reset the grip.
        m_Grip = new Input1DAxis();
        m_Grip.Name = "Grip";
        m_Grip.ButtonIncluded = true;

        // Reset the primary button (i.e., sandwich button on the Vive).
        m_PrimaryButton = new InputButton();
        m_PrimaryButton.Name = "Primary";
        m_PrimaryButton.Touch = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Reset the controller values.
        Reset();

        // Validate the input device.
        InputDevice inputDevice = ControllerDevice;
        inputDevice.isValid = true;
        ControllerDevice = inputDevice;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the input device.
        InputDevice inputDevice = ControllerDevice;
        inputDevice.isValid = true;
        inputDevice.primary2DAxis = Primary2DAxis.Axis;
        inputDevice.primary2DAxisTouch = Primary2DAxis.Touch;
        inputDevice.primary2DAxisClick = Primary2DAxis.Click;
        inputDevice.trigger = Trigger.Axis;
        inputDevice.triggerButton = Trigger.Button;
        inputDevice.grip = Grip.Axis;
        inputDevice.gripButton = Grip.Button;
        inputDevice.primaryButton = PrimaryButton.Button;
        ControllerDevice = inputDevice;
    }
}
