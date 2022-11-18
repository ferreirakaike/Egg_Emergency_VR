using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
 
 /// <summary>
 /// This class enables haptics feedback for when a raycast is hovered over the UI canvas.
 /// </summary>
public class HoverHaptic : MonoBehaviour, IPointerEnterHandler
{
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;
 
    /// <summary>
    /// This method sets the haptics to the controller.
    /// </summary>
    /// <param name="eventData">This variable holds the reference to the ray interactor</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        XRRayInteractor interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;
        interactor.xrController.SendHapticImpulse(0.25f, 0.25f);
    }
}