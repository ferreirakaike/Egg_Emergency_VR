using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// reference https://www.youtube.com/watch?v=Ie0-oKN3Lq0
// need to modify current prefab to use

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;
    public enum TwoHandRotationType { None, First, Second};
    public TwoHandRotationType twoHandRotationType;
    public bool snapToSecondHand = true;

    public GameObject firstGrabObject;
    public GameObject secondGrabObject;
    [SerializeField]
    public int secondHandGrabLayer = -1;
    [SerializeField]
    private int defaultLayer = -1;
    private Quaternion initialRotationOffset;
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
        if (defaultLayer == -1)
        {
            defaultLayer = 9;
        }
        if (secondHandGrabLayer == -1)
        {
            // Second Hand Grab layer index
            secondHandGrabLayer = 11;
        }
        // Add listener all second hand grab points
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void updateTargetBoth()
    {
        Vector3 currentHandPosition1 = selectingInteractor.transform.position;
        Vector3 currentHandPosition2 = secondInteractor.transform.position;

        Vector3 handDir1 = (initialHandPosition1 - initialHandPosition2).normalized;
        Vector3 handDir2 = (currentHandPosition1 - currentHandPosition2).normalized;

        Quaternion handRotation = Quaternion.FromToRotation(handDir1, handDir2);

        float currentGrabDistance = Vector3.Distance(currentHandPosition1, currentHandPosition2);
        float initialGrabDistance = Vector3.Distance(initialHandPosition1, initialHandPosition2);
        float percentage = (currentGrabDistance / initialGrabDistance);

        Vector3 newScale = new Vector3(percentage * initialObjectScale.x, percentage * initialObjectScale.y, initialObjectScale.z);

        // this.transform.rotation = handRotation * initialObjectRotation;
        if (newScale.x >= minScale && newScale.x <= maxScale)
        {
            this.transform.localScale = newScale;
        }
        this.transform.position = (0.5f * (currentHandPosition1 + currentHandPosition2)) + (handRotation * (initialObjectDirection * percentage));
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {
            // Compute rotation
            if(snapToSecondHand)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            }
            else
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * initialRotationOffset;
            }
            if (scaleObject)
            {
                updateTargetBoth();
            }
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {  // interpret all axes rotations as X rotation. To get rid of this behavior, use .up instead of .right
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }
        return targetRotation;
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("Second hand grab");
        secondInteractor = interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
        secondGrabObject.GetComponent<XRTintInteractableVisual>().tintOnHover = false;
        initialHandPosition1 = selectingInteractor.transform.position;
        initialHandPosition2 = secondInteractor.transform.position;
        initialObjectRotation = this.transform.rotation;
        initialObjectScale = this.transform.localScale;
        initialObjectDirection = this.transform.position - (initialHandPosition1 + initialHandPosition2) * 0.5f;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("Second hand release");
        secondInteractor = null;
        if (firstGrabObject && secondGrabObject)
        {
            secondGrabObject.GetComponent<XRTintInteractableVisual>().tintOnHover = true;
        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("First hand grab");
        base.OnSelectEntered(interactor);
        attachInitialRotation = interactor.attachTransform.localRotation;
        if (firstGrabObject && secondGrabObject)
        {
            firstGrabObject.layer = secondHandGrabLayer;
            secondGrabObject.layer = defaultLayer;
            secondGrabObject.GetComponent<XRTintInteractableVisual>().tintOnHover = true;
        }
        GetComponent<XRTintInteractableVisual>().tintOnHover = false;
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("First hand release");
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
        if (firstGrabObject && secondGrabObject)
        {
            firstGrabObject.layer = defaultLayer;
            secondGrabObject.layer = secondHandGrabLayer;
            secondGrabObject.GetComponent<XRTintInteractableVisual>().tintOnHover = false;
        }
        GetComponent<XRTintInteractableVisual>().tintOnHover = true;
        this.transform.localScale = initialObjectScale;
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool is_already_grabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !is_already_grabbed;
    }
}
