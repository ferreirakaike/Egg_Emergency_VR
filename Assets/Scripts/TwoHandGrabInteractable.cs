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
        {
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
        GetComponent<ScaleBasket>().grabbedWithBothHands = true;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("Second hand release");
        secondInteractor = null;
        if (firstGrabObject && secondGrabObject)
        {
            secondGrabObject.GetComponent<XRTintInteractableVisual>().tintOnHover = false;
        }
        GetComponent<XRTintInteractableVisual>().tintOnHover = true;
        GetComponent<ScaleBasket>().grabbedWithBothHands = false;
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
        GetComponent<ScaleBasket>().grabbedWithBothHands = false;
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool is_already_grabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !is_already_grabbed;
    }
}
