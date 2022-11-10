using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using TMPro;

/// <summary>
/// This class works together with the colliders to enable collision for UI canvas.
/// </summary>
public class ColliderUIButton : XRSimpleInteractable
{
    private bool rightHandClicked = false;
    private bool leftHandClicked = false;

    /// <summary>
    /// This variable holds the material of the button for when it is clicked.
    /// </summary>
    public Material clickedButtonMaterial;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable") || other.gameObject.CompareTag("Deterrent") 
        || other.gameObject.CompareTag("Basket") || other.gameObject.CompareTag("InnerBasket"))
        {
            return;
        }
        // currently on one of the hand controller
        Debug.Log("TriggerEntered");
        if (clickedButtonMaterial)
        {
            GetComponent<Image>().material = clickedButtonMaterial;
        }
        else
        {
             GetComponent<Image>().material = Resources.Load("Pressed Button Material.mat", typeof(Material)) as Material;
        }
        
        if (other.gameObject.CompareTag("LeftHand") && !leftHandClicked)
        {
            leftHandClicked = true;

            // get xr controller
            GameObject controller = other.gameObject;
            while(!controller.GetComponentInParent<XROrigin>())
            {
                controller = controller.transform.parent.gameObject;
            }
            controller.GetComponent<XRDirectInteractor>().xrController.SendHapticImpulse(0.25f, 0.25f);
        }
        if (other.gameObject.CompareTag("RightHand") && !rightHandClicked)
        {
            rightHandClicked = true;

            // get xr controller
            GameObject controller = other.gameObject;
            while(!controller.GetComponentInParent<XROrigin>())
            {
                controller = controller.transform.parent.gameObject;
            }
            controller.GetComponent<XRDirectInteractor>().xrController.SendHapticImpulse(0.25f, 0.25f);
        }
        StartCoroutine(ClickAfterASecond());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LeftHand"))
        {
            leftHandClicked = false;
        }
        if (other.gameObject.CompareTag("RightHand"))
        {
            rightHandClicked = false;
        }
    }

    IEnumerator ClickAfterASecond()
    {
        yield return new WaitForSeconds(0.01f);
        base.OnActivated(new ActivateEventArgs());
    }
}
