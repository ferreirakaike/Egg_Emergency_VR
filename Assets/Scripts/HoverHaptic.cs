using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
 
public class HoverHaptic : MonoBehaviour, IPointerEnterHandler
{
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        XRRayInteractor interactor = InputModule.GetInteractor(eventData.pointerId) as XRRayInteractor;
        interactor.xrController.SendHapticImpulse(0.25f, 0.25f);
    }
}