using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.UI;
using Unity.XR.CoreUtils;
using TMPro;

public class ColliderUIButton : XRSimpleInteractable
{
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
        
       
        // args.interactorObject.transform.GetComponent<XRDirectInteractor>().xrController.SendHapticImpulse(0.25f, 0.25f);
        // base.OnActivated(new ActivateEventArgs());
        StartCoroutine(ClickAfterASecond());
    }

    IEnumerator ClickAfterASecond()
    {
        yield return new WaitForSeconds(0.2f);
        base.OnActivated(new ActivateEventArgs());
    }
}
