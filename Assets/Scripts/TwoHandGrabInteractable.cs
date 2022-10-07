using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// reference https://www.youtube.com/watch?v=Ie0-oKN3Lq0
// need to modify current prefab to use

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public bool scaleObject = true;
    public float minScale = 15f;
    public float maxScale = 50f;

    private Vector3 initialHandPosition1;
    private Vector3 initialHandPosition2;
    private Quaternion initialObjectRotation;
    private Vector3 initialObjectScale;
    private Vector3 initialObjectDirection;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void updateTargetBoth()
    {
        Vector3 currentHandPosition1 = interactorsSelecting[0].transform.position;
        Vector3 currentHandPosition2 = interactorsSelecting[1].transform.position;

        Vector3 handDir1 = (initialHandPosition1 - initialHandPosition2).normalized;
        Vector3 handDir2 = (currentHandPosition1 - currentHandPosition2).normalized;

        Quaternion handRotation = Quaternion.FromToRotation(handDir1, handDir2);

        float currentGrabDistance = Vector3.Distance(currentHandPosition1, currentHandPosition2);
        float initialGrabDistance = Vector3.Distance(initialHandPosition1, initialHandPosition2);
        float percentage = (currentGrabDistance / initialGrabDistance);

        if (scaleObject)
        {
            Vector3 newScale = new Vector3(percentage * initialObjectScale.x, percentage * initialObjectScale.y, initialObjectScale.z);
            if (newScale.x >= minScale && newScale.x <= maxScale)
            {
                this.transform.localScale = newScale;
            }
        }
        // this.transform.rotation = handRotation * initialObjectRotation;
        this.transform.position = (0.5f * (currentHandPosition1 + currentHandPosition2)) + (handRotation * (initialObjectDirection * percentage));
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable (updatePhase);
        if (interactorsSelecting.Count == 2)
        {
            updateTargetBoth();
        }
    }

    protected override void Awake() 
    {
        base.Awake();
        selectMode = InteractableSelectMode.Multiple;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        args.interactorObject.transform.GetChild(0).gameObject.SetActive(false);
        base.OnSelectEntered(args);
        if (interactorsSelecting.Count == 1)
        {
            Debug.Log("First hand grab");
            // GetComponent<XRTintInteractableVisual>().enabled = false;
            // GetComponent<XRTintInteractableVisual>().enabled = true;
            // GetComponent<XRTintInteractableVisual>().tintColor = Color.green;
        }
        if (interactorsSelecting.Count == 2)
        {
            Debug.Log("Second hand grab");
            // GetComponent<XRTintInteractableVisual>().enabled = false;
            initialHandPosition1 = interactorsSelecting[0].transform.position;
            initialHandPosition2 = interactorsSelecting[1].transform.position;
            // set x axis to point toward first hand
            this.transform.right = interactorsSelecting[0].transform.right;
            initialObjectRotation = this.transform.rotation;
            initialObjectScale = this.transform.localScale;
            initialObjectDirection = this.transform.position - (initialHandPosition1 + initialHandPosition2) * 0.5f;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        args.interactorObject.transform.GetChild(0).gameObject.SetActive(true);
        base.OnSelectExited(args);
        if (interactorsSelecting.Count == 1)
        {
            Debug.Log("One hand exited");
            // GetComponent<XRTintInteractableVisual>().enabled = true;
        }
        if (interactorsSelecting.Count == 0)
        {
            Debug.Log("Both hands exited");
            // GetComponent<XRTintInteractableVisual>().enabled = true;
            // GetComponent<XRTintInteractableVisual>().tintColor = Color.yellow;
            this.transform.localScale = new Vector3(25,25,25);
        }
    }

}
