using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRST;

[RequireComponent(typeof(XRController))]
public class HandController : MonoBehaviour
{
    // The 1D axis used to simulate the XR controller's Trigger.
    [SerializeField]
    public Input1DAxis m_Trigger;
    public Input1DAxis Trigger { get { return m_Trigger; } set { m_Trigger = value; } }

    // The 1D axis used to simulate the XR controller's Grip.
    [SerializeField]
    public Input1DAxis m_Grip;
    public Input1DAxis Grip { get { return m_Grip; } set { m_Grip = value; } }
   // The local controller.
    public XRController Controller { get { return GetComponent<XRController>(); } }
    public Hand hand;

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

    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice inputDevice = ControllerDevice;
        hand.SetGrip(inputDevice.grip);
        hand.SetTrigger(inputDevice.trigger);
    }
}
