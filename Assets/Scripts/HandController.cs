using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
/// <summary>
/// This class is used to send grip and trigger value to the Hand class to trigger the hand animation.
/// </summary>
public class HandController : MonoBehaviour
{
    ActionBasedController controller;

    /// <summary>
    /// This variable holds the reference to an XRController with a Hand class attached.
    /// </summary>
    public Hand hand;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}
